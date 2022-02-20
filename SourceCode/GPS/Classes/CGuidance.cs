using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        private readonly FormGPS mf;

        public List<CGuidanceLine> curveArr = new List<CGuidanceLine>();

        public CGuidanceLine currentGuidanceLine, currentABLine, currentCurveLine, EditGuidanceLine, creatingContour;

        //the list of points to drive on
        public List<vec3> curList = new List<vec3>();

        public bool isBtnABLineOn, isBtnCurveOn, isContourBtnOn, isLocked = false;

        private int currentLocationIndexA, currentLocationIndexB, backSpacing = 30;

        public bool isValid, isOkToAddDesPoints;
        public double lastSecond = 0, lastSecondSearch = 0, moveDistance, CurrentHeading;

        public double abLength;
        public double snapDistance;
        public int lineWidth;
        public int tramPassEvery;

        public double distanceFromRefLine;

        public int howManyPathsAway, oldHowManyPathsAway = int.MaxValue;

        //steer, pivot, and ref indexes
        private int sA, sB, pA, pB, rA, rB;

        public bool isSmoothWindowOpen, isHeadingSameWay = true, oldIsHeadingSameWay = true;

        public double distanceFromCurrentLinePivot;
        public double steerAngle, rEast, rNorth;

        public vec2 goalPoint = new vec2(0, 0), radiusPoint = new vec2(0, 0);

        public double inty, steerHeadingError, xTrackSteerCorrection = 0;

        private double pivotDistError, lastPivotDistError, pivotDerivativeDistError;
        public double steerDistError, lastSteerDistError, steerDerivativeDistError;

        //for adding steering angle based on side slope hill
        public double sideHillCompFactor, ppRadius;

        //derivative counter
        private int counter2;

        public CGuidance(FormGPS _f)
        {
            //constructor
            mf = _f;
            sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;

            curList.Capacity = 1024;

            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            abLength = Properties.Settings.Default.setAB_lineLength;

            uturnDistanceFromBoundary = Properties.Vehicle.Default.set_youTurnDistanceFromBoundary;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Properties.Vehicle.Default.set_youTurnExtensionLength;

            rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            Set_Alternate_skips();

            ytList.Capacity = 128;
        }

        public void CalculateSteerAngle(vec3 pivot, vec3 steer, List<vec3> curList)
        {
            bool completeYouTurn = !isYouTurnTriggered;

            if (curList.Count > 1)
            {
                int cc = 0, dd;
                double minDistA = double.MaxValue;
                double minDistB = double.MaxValue;

                for (int j = 0; j < curList.Count; j += 10)
                {
                    double dist = glm.DistanceSquared(pivot, curList[j]);

                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = double.MaxValue;

                dd = cc + 7; if (dd > curList.Count) dd = curList.Count;
                cc -= 7; if (cc < 0) cc = 0;

                for (int j = cc; j < dd; j++)
                {
                    double dist = glm.DistanceSquared(pivot, curList[j]);

                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        pB = pA;
                        minDistA = dist;
                        pA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        pB = j;
                    }
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (pA > pB) { int C = pA; pA = pB; pB = C; }

                //return and reset if too far away or end of the line
                if (pB > curList.Count - 2)
                    completeYouTurn = true;

                if (isContourBtnOn)
                {
                    if (isLocked && (pA < 1 || pB > curList.Count - 2))
                    {
                        isLocked = false;
                        return;
                    }
                }

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx2 = curList[pB].easting - curList[pA].easting;
                double dy2 = curList[pB].northing - curList[pA].northing;

                if (Math.Abs(dx2) < double.Epsilon && Math.Abs(dy2) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dy2 * pivot.easting) - (dx2 * pivot.northing) + (curList[pB].easting
                            * curList[pA].northing) - (curList[pB].northing * curList[pA].easting))
                                / Math.Sqrt((dy2 * dy2) + (dx2 * dx2));

                double U = (((pivot.easting - curList[pA].easting) * dx2)
                                + ((pivot.northing - curList[pA].northing) * dy2))
                                / ((dx2 * dx2) + (dy2 * dy2));

                rEast = curList[pA].easting + (U * dx2);
                rNorth = curList[pA].northing + (U * dy2);

                if (isYouTurnTriggered)
                {
                    onA = 0;
                    for (int k = 0; k < pA; k++)
                    {
                        onA += glm.Distance(ytList[k], ytList[k + 1]);
                    }

                    onA += glm.Distance(ytList[pA], rEast, rNorth);
                }

                currentLocationIndexA = pA;
                currentLocationIndexB = pB;
                CurrentHeading = curList[pA].heading;

                if (mf.isStanleyUsed)
                {
                    #region Stanley
                    minDistA = minDistB = double.MaxValue;

                    if (isYouTurnTriggered || isHeadingSameWay)
                    {
                        cc = pA;
                        dd = cc + 12;
                        if (dd > curList.Count)
                            dd = curList.Count;
                    }
                    else
                    {
                        dd = pB + 1;
                        cc = dd - 12;
                        if (cc < 0)
                            cc = 0;
                    }

                    //find the closest 2 points to current close call
                    for (int j = cc; j < dd; j++)
                    {
                        double dist = glm.DistanceSquared(steer, curList[j]);
                        if (dist < minDistA)
                        {
                            minDistB = minDistA;
                            sB = sA;
                            minDistA = dist;
                            sA = j;
                        }
                        else if (dist < minDistB)
                        {
                            minDistB = dist;
                            sB = j;
                        }
                    }

                    //just need to make sure the points continue ascending or heading switches all over the place
                    if (sA > sB) { int C = sA; sA = sB; sB = C; }

                    if (isYouTurnTriggered)
                    {
                        //feed backward to turn slower to keep pivot on
                        sA -= 7;
                        if (sA < 0)
                        {
                            sA = 0;
                        }
                        sB = sA + 1;

                        //return and reset if too far away or end of the line
                        if (sB >= curList.Count - 8 || minDistA > 16)
                        {
                            completeYouTurn = true;
                        }
                    }

                    //get the distance from currently active AB line
                    double dx = curList[sB].easting - curList[sA].easting;
                    double dy = curList[sB].northing - curList[sA].northing;

                    if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                    //how far from current AB Line is fix
                    double distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (curList[sB].easting
                                * curList[sA].northing) - (curList[sB].northing * curList[sA].easting))
                                    / Math.Sqrt((dy * dy) + (dx * dx));

                    double lineHeading;
                    if (isYouTurnTriggered)
                        lineHeading = curList[sA].heading;
                    else if (isBtnCurveOn || isBtnABLineOn)
                        lineHeading = isHeadingSameWay ? curList[sB].heading : curList[sA].heading;
                    else//contour
                    {
                        lineHeading = Math.Atan2(dx, dy);
                        if (lineHeading < 0) lineHeading += glm.twoPI;
                    }

                    if (!isYouTurnTriggered && (isBtnCurveOn || isBtnABLineOn))
                        distanceFromCurrentLineSteer -= inty;

                    if (isYouTurnTriggered || isHeadingSameWay)
                    {
                        steerHeadingError = steer.heading - lineHeading;
                    }
                    else
                    {
                        distanceFromCurrentLineSteer *= -1.0;
                        steerHeadingError = steer.heading - lineHeading + Math.PI;
                    }


                    while (steerHeadingError < -Math.PI)
                        steerHeadingError += glm.twoPI;
                    while (steerHeadingError > Math.PI)
                        steerHeadingError -= glm.twoPI;

                    if (mf.isReverse) steerHeadingError *= -1;
                    //Overshoot setting on Stanley tab
                    steerHeadingError *= mf.vehicle.stanleyHeadingErrorGain;

                    if (steerHeadingError > 0.74) steerHeadingError = 0.74;
                    if (steerHeadingError < -0.74) steerHeadingError = -0.74;

                    //the non linear distance error part of stanley
                    double XTEc = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                        / ((Math.Abs(mf.pn.speed) * 0.277777) + 1));

                    //clamp it to max 42 degrees
                    if (XTEc > 0.74) XTEc = 0.74;
                    if (XTEc < -0.74) XTEc = -0.74;

                    xTrackSteerCorrection = (xTrackSteerCorrection * 0.5) + XTEc * 0.5;

                    steerAngle = glm.toDegrees((((!isYouTurnTriggered || isContourBtnOn) ? XTEc : xTrackSteerCorrection) + steerHeadingError) * -1.0);

                    if (!isYouTurnTriggered && (isBtnCurveOn || isBtnABLineOn))
                    {
                        if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngle *= 0.5;
                        else steerAngle *= (1 - Math.Abs(distanceFromCurrentLineSteer));
                    }

                    //derivative of steer distance error
                    steerDistError = (steerDistError * 0.95) + (xTrackSteerCorrection * 3.0);
                    pivotDistError = (pivotDistError * 0.6) + distanceFromCurrentLinePivot * 0.4;

                    if (counter2++ > 5)
                    {
                        steerDerivativeDistError = steerDistError - lastSteerDistError;
                        lastSteerDistError = steerDistError;
                        pivotDerivativeDistError = pivotDistError - lastPivotDistError;
                        lastPivotDistError = pivotDistError;
                        counter2 = 0;
                    }


                    if (mf.vehicle.stanleyIntegralGainAB != 0 && !mf.isReverse)
                    {
                        if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && mf.pn.speed > mf.startSpeed && Math.Abs(steerDerivativeDistError) < 1 && Math.Abs(pivotDerivativeDistError) < 0.15)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistError * mf.vehicle.stanleyIntegralGainAB * -0.1;
                            }
                            else
                            {
                                inty += pivotDistError * mf.vehicle.stanleyIntegralGainAB * -0.01;
                            }
                        }
                        else inty *= 0.7;
                    }
                    else inty = 0;

                    if (!isYouTurnTriggered && mf.ahrs.imuRoll != 88888)
                        steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

                    //add them up and clamp to max in vehicle settings
                    if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
                    if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;
                    #endregion
                }
                else
                {
                    #region Pure Pursuit

                    if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
                    {
                        //integral slider is set to 0
                        pivotDistError = pivotDistError * 0.8 + (distanceFromCurrentLinePivot * 0.2);
                        if (counter2++ > 4)
                        {
                            pivotDerivativeDistError = pivotDistError - lastPivotDistError;
                            lastPivotDistError = pivotDistError;
                            counter2 = 0;
                            pivotDerivativeDistError *= 2;
                        }

                        if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && Math.Abs(pivotDerivativeDistError) < 0.1 && mf.avgSpeed > 2.5)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * (isContourBtnOn ? -0.06 : -0.04);
                            }
                            else
                            {
                                if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                                {
                                    inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * -0.02;
                                    if (inty > 0.2) inty = 0.2;
                                    else if (inty < -0.2) inty = -0.2;
                                }
                            }
                        }
                        else inty *= 0.95;
                    }
                    else inty = 0;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = (isYouTurnTriggered ? 0.8 : 1.0) * mf.vehicle.UpdateGoalPointDistance();

                    bool CountUp = mf.isReverse ^ (isYouTurnTriggered || isHeadingSameWay);

                    int count = CountUp ? 1 : -1;
                    vec3 start = new vec3(rEast, rNorth, 0);
                    double distSoFar = 0;

                    if (pA == 0 || pB == curList.Count - 1)//extend end of line
                    {
                        goalPoint.northing = rNorth + (Math.Cos(curList[pA].heading) * (CountUp ? goalPointDistance : -goalPointDistance));
                        goalPoint.easting = rEast + (Math.Sin(curList[pA].heading) * (CountUp ? goalPointDistance : -goalPointDistance));
                    }
                    else
                    {
                        for (int i = CountUp ? pB : pA; i < curList.Count && i >= 0; i += count)
                        {
                            // used for calculating the length squared of next segment.
                            double tempDist = glm.Distance(start, curList[i]);

                            //will we go too far?
                            if ((tempDist + distSoFar) > goalPointDistance)
                            {
                                double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                                goalPoint.easting = (((1 - j) * start.easting) + (j * curList[i].easting));
                                goalPoint.northing = (((1 - j) * start.northing) + (j * curList[i].northing));
                                break;
                            }
                            else distSoFar += tempDist;

                            start = curList[i];

                            if (i == 0 || i == curList.Count - 1)//extend end of line
                            {
                                //goalPointDistance is longer than remaining u-turn
                                completeYouTurn = true;

                                double dist = goalPointDistance - distSoFar;
                                goalPoint.northing = start.northing + (Math.Cos(start.heading) * (CountUp ? dist : -dist));
                                goalPoint.easting = start.easting + (Math.Sin(start.heading) * (CountUp ? dist : -dist));
                                break;
                            }
                        }
                    }
                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = glm.twoPI - mf.fixHeading + ((isYouTurnTriggered || isHeadingSameWay) ? inty : -inty);

                    ppRadius = goalPointDistanceSquared / (2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))));

                    steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (!isYouTurnTriggered && mf.ahrs.imuRoll != 88888)
                        steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

                    if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
                    if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

                    if (ppRadius < -500) ppRadius = -500;
                    if (ppRadius > 500) ppRadius = 500;

                    radiusPoint.easting = pivot.easting + (ppRadius * Math.Cos(localHeading));
                    radiusPoint.northing = pivot.northing + (ppRadius * Math.Sin(localHeading));
                    if (!isYouTurnTriggered)
                    {
                        if (isBtnABLineOn)
                        {
                            if (mf.isAngVelGuidance)
                            {
                                //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                                mf.setAngVel = 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;
                                mf.setAngVel = glm.toDegrees(mf.setAngVel) * 100;

                                //clamp the steering angle to not exceed safe angular velocity
                                if (Math.Abs(mf.setAngVel) > 1000)
                                {
                                    //mf.setAngVel = mf.setAngVel < 0 ? -mf.vehicle.maxAngularVelocity : mf.vehicle.maxAngularVelocity;
                                    mf.setAngVel = mf.setAngVel < 0 ? -1000 : 1000;
                                }
                            }
                        }
                        else
                        {
                            //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;

                            //clamp the steering angle to not exceed safe angular velocity
                            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                            {
                                steerAngle = glm.toDegrees(Math.Atan(mf.vehicle.wheelbase * (steerAngle > 0 ? mf.vehicle.maxAngularVelocity : -mf.vehicle.maxAngularVelocity)
                                    / (glm.twoPI * mf.pn.speed * 0.277777)));
                            }
                        }
                    }
                    #endregion Pure Pursuit
                }

                if (!isYouTurnTriggered && !isHeadingSameWay)
                    distanceFromCurrentLinePivot *= -1.0;

                //Convert to millimeters from meters
                mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
                mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
            }
            else
            {
                completeYouTurn = true;
                //invalid distance so tell AS module
                mf.guidanceLineDistanceOff = 32000;
            }
            if (completeYouTurn && isYouTurnTriggered)
                CompleteYouTurn();
        }

        private void PurePursuitRecPath(vec3 pivot, List<CRecPathPt> recList)
        {
            double dist, dx, dz;
            double minDistA = double.MaxValue, minDistB = double.MaxValue;

            if (isFollowingRecPath)
            {
                //set the search range close to current position
                int top = currentPositonIndex + 5;
                if (top > recList.Count) top = recList.Count;

                for (int t = currentPositonIndex; t < top; t++)
                {
                    dist = ((pivot.easting - recList[t].easting) * (pivot.easting - recList[t].easting))
                                    + ((pivot.northing - recList[t].northing) * (pivot.northing - recList[t].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        pA = t;
                    }
                }

                //next point is the next in list
                pB = pA + 1;
                if (pB == recList.Count)
                {
                    //don't go past the end of the list - "end of the line" trigger
                    pA--;
                    pB--;
                    isEndOfTheRecLine = true;
                }
            }
            else
            {
                //find the closest 2 points to current fix
                for (int t = 0; t < recList.Count; t++)
                {
                    dist = ((pivot.easting - recList[t].easting) * (pivot.easting - recList[t].easting))
                                    + ((pivot.northing - recList[t].northing) * (pivot.northing - recList[t].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        pB = pA;
                        minDistA = dist;
                        pA = t;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        pB = t;
                    }
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (pA > pB) { int C = pA; pA = pB; pB = C; }
            }

            //save current position
            currentPositonIndex = pA;

            //get the distance from currently active AB line
            dx = recList[pB].easting - recList[pA].easting;
            dz = recList[pB].northing - recList[pA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (recList[pB].easting
                        * recList[pA].northing) - (recList[pB].northing * recList[pA].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //integral slider is set to 0
            if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
            {
                pivotDistError = distanceFromCurrentLinePivot * 0.2 + pivotDistError * 0.8;

                if (counter2++ > 4)
                {
                    pivotDerivativeDistError = pivotDistError - lastPivotDistError;
                    lastPivotDistError = pivotDistError;
                    counter2 = 0;
                    pivotDerivativeDistError *= 2;

                }

                if (isFollowingRecPath && Math.Abs(pivotDerivativeDistError) < 0.1 && mf.avgSpeed > 2.5)
                {
                    //if over the line heading wrong way, rapidly decrease integral
                    if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                    {
                        inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * -0.04;
                    }
                    else
                    {
                        if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                        {
                            inty += pivotDistError * mf.vehicle.purePursuitIntegralGain * -0.02;
                            if (inty > 0.2) inty = 0.2;
                            else if (inty < -0.2) inty = -0.2;
                        }
                    }
                }
                else inty *= 0.95;
            }
            else inty = 0;

            // ** Pure pursuit ** - calc point on ABLine closest to current position
            double U = (((pivot.easting - recList[pA].easting) * dx)
                        + ((pivot.northing - recList[pA].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEast = recList[pA].easting + (U * dx);
            rNorth = recList[pA].northing + (U * dz);

            //update base on autosteer settings and distance from line
            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

            bool ReverseHeading = !mf.isReverse;

            int count = ReverseHeading ? 1 : -1;
            CRecPathPt start = new CRecPathPt(rEast, rNorth, 0, 0, false);
            double distSoFar = 0;

            for (int i = ReverseHeading ? pB : pA; i < recList.Count && i >= 0; i += count)
            {
                // used for calculating the length squared of next segment.
                double tempDist = Math.Sqrt((start.easting - recList[i].easting) * (start.easting - recList[i].easting)
                    + (start.northing - recList[i].northing) * (start.northing - recList[i].northing));

                //will we go too far?
                if ((tempDist + distSoFar) > goalPointDistance)
                {
                    double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                    goalPoint.easting = (((1 - j) * start.easting) + (j * recList[i].easting));
                    goalPoint.northing = (((1 - j) * start.northing) + (j * recList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = recList[i];
            }

            //calc "D" the distance from pivotAxlePosRP axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            double localHeading = glm.twoPI - mf.fixHeading + inty;

            ppRadius = goalPointDistanceSquared / (2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivot.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivot.northing + (ppRadius * Math.Sin(localHeading));

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }
    }

    public enum Mode { AB = 2, Curve = 4, Contour = 8, Boundary = 16 };//, Heading, Circle, Spiral

    public class CGuidanceLine
    {
        public Mode mode;
        public List<vec3> curvePts = new List<vec3>();
        public string Name = "aa";

        public CGuidanceLine(Mode _mode)
        {
            mode = _mode;
        }

        public CGuidanceLine(CGuidanceLine old)
        {
            mode = old.mode;
            Name = old.Name;
            curvePts.AddRange(old.curvePts.ToArray());
        }
    }
}
