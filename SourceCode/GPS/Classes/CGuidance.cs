using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        private readonly FormGPS mf;

        public double distanceFromRefLine;
        public double howManyPathsAway;

        //steer, pivot, and ref indexes
        private int sA, sB, pA, pB;
        private int rA, rB;

        public bool isHeadingSameWay = true;

        public double pivotDistanceErrorLast, pivotDerivative, pivotDerivativeSmoothed;

        public double pivotErrorTotal;

        public double distanceFromCurrentLineSteer, distanceFromCurrentLinePivot;
        public double steerAngle, rEast, rNorth;

        public vec2 goalPoint = new vec2(0, 0);
        public vec2 radiusPoint = new vec2(0, 0);

        public double inty, xTrackSteerCorrection = 0;
        public double steerHeadingError, steerHeadingErrorDegrees;

        public double distSteerError, lastDistSteerError, derivativeDistError;

        public double pivotDistanceError, ppRadius;

        //for adding steering angle based on side slope hill
        public double sideHillCompFactor;

        //derivative counter
        private int counter2;

        public CGuidance(FormGPS _f)
        {
            //constructor
            mf = _f;
            sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;

            refList.Capacity = 1024;
            curList.Capacity = 1024;

            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            abLength = Properties.Settings.Default.setAB_lineLength;

            ctList.Capacity = 128;
            ptList.Capacity = 128;

            uturnDistanceFromBoundary = Properties.Vehicle.Default.set_youTurnDistanceFromBoundary;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Properties.Vehicle.Default.set_youTurnExtensionLength;

            rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            Set_Alternate_skips();

            ytList.Capacity = 128;
        }

        #region Stanley
        public void StanleyGuidance(vec3 pivot, vec3 steer, List<vec3> curList)
        {
            bool completeYouTurn = !isYouTurnTriggered;

            if (curList.Count > 1)
            {
                int cc = 0, dd;
                double minDistA = double.MaxValue;
                double minDistB = double.MaxValue;

                for (int j = 0; j < curList.Count; j += 10)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = double.MaxValue;

                dd = cc + 7; if (dd > curList.Count) dd = curList.Count;
                cc -= 7; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
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

                minDistA = minDistB = double.MaxValue;

                if (isHeadingSameWay)
                {
                    dd = sB + 1;
                    cc = dd - 12;
                    if (cc < 0)
                        cc = 0;
                }
                else
                {
                    cc = sA;
                    dd = cc + 12;
                    if (dd > curList.Count)
                        dd = curList.Count;
                }

                //find the closest 2 points of pivot back from steer
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((pivot.easting - curList[j].easting) * (pivot.easting - curList[j].easting))
                                    + ((pivot.northing - curList[j].northing) * (pivot.northing - curList[j].northing));
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

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx2 = curList[pB].easting - curList[pA].easting;
                double dy2 = curList[pB].northing - curList[pA].northing;

                if (Math.Abs(dx2) < double.Epsilon && Math.Abs(dy2) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dy2 * pivot.easting) - (dx2 * pivot.northing) + (curList[pB].easting
                            * curList[pA].northing) - (curList[pB].northing * curList[pA].easting))
                                / Math.Sqrt((dy2 * dy2) + (dx2 * dx2));

                if (!isHeadingSameWay)
                    distanceFromCurrentLinePivot *= -1.0;

                double U = (((pivot.easting - curList[pA].easting) * dx2)
                                + ((pivot.northing - curList[pA].northing) * dy2))
                                / ((dx2 * dx2) + (dy2 * dy2));

                rEast = curList[pA].easting + (U * dx2);
                rNorth = curList[pA].northing + (U * dy2);
                currentLocationIndex = pA;
                CurrentHeading = curList[pA].heading;

                //get the distance from currently active AB line
                double dx = curList[sB].easting - curList[sA].easting;
                double dy = curList[sB].northing - curList[sA].northing;

                if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (curList[sB].easting
                            * curList[sA].northing) - (curList[sB].northing * curList[sA].easting))
                                / Math.Sqrt((dy * dy) + (dx * dx));

                double steerHeading;
                if (isYouTurnTriggered)
                    steerHeading = curList[sA].heading;
                else if (isBtnCurveOn || isBtnABLineOn)
                    steerHeading = curList[sB].heading;
                else//contour
                {
                    steerHeading = Math.Atan2(dx, dy);
                    if (steerHeading < 0) steerHeading += glm.twoPI;

                    isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(steer.heading - steerHeading) - Math.PI) < glm.PIBy2;
                }

                if (isYouTurnTriggered || isHeadingSameWay)
                {
                    steerHeadingError = steer.heading - steerHeading;
                }
                else
                {
                    distanceFromCurrentLineSteer *= -1.0;
                    steerHeadingError = steer.heading - steerHeading + Math.PI;
                }

                if (!isYouTurnTriggered && (isBtnCurveOn || isBtnABLineOn))
                    distanceFromCurrentLineSteer -= inty;

                //Fix the circular error
                if (steerHeadingError > Math.PI) steerHeadingError -= Math.PI;
                else if (steerHeadingError < Math.PI) steerHeadingError += Math.PI;

                if (steerHeadingError > glm.PIBy2) steerHeadingError -= Math.PI;
                else if (steerHeadingError < -glm.PIBy2) steerHeadingError += Math.PI;

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

                steerAngle = glm.toDegrees((((!isYouTurnTriggered && (isBtnCurveOn || isBtnABLineOn)) ? xTrackSteerCorrection : XTEc) + steerHeadingError) * -1.0);

                if (!isYouTurnTriggered && (isBtnCurveOn || isBtnABLineOn))
                {
                    if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngle *= 0.5;
                    else steerAngle *= (1 - Math.Abs(distanceFromCurrentLineSteer));
                }

                //derivative of steer distance error
                distSteerError = (distSteerError * 0.95) + (xTrackSteerCorrection * 3.0);
                if (counter2++ > 5)
                {
                    derivativeDistError = distSteerError - lastDistSteerError;
                    lastDistSteerError = distSteerError;
                    counter2 = 0;
                }

                //pivot PID
                pivotDistanceError = (pivotDistanceError * 0.6) + (distanceFromCurrentLinePivot * 0.4);

                if (mf.vehicle.stanleyIntegralGainAB != 0 && !mf.isReverse)
                {
                    if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && mf.pn.speed > mf.startSpeed && Math.Abs(derivativeDistError) < 1)
                    {
                        //if over the line heading wrong way, rapidly decrease integral
                        if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                        {
                            inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.1;
                        }
                        else
                        {
                            inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.01;
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
        #endregion

        #region Pure Pursuit
        public void PurePursuit(vec3 pivot, vec3 steer, List<vec3> curList)
        {
            bool completeYouTurn = !isYouTurnTriggered;
            if (curList.Count > 1)
            {
                double minDistA = double.MaxValue, minDistB = double.MaxValue;

                //find the closest 2 points to current fix
                for (int t = 0; t < curList.Count; t++)
                {
                    double dist = glm.DistanceSquared(pivot, curList[t]);

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

                if (isYouTurnTriggered)
                {
                    onA = curList.Count / 2;
                    if (pA < onA)
                    {
                        onA = -pA;
                    }
                    else
                    {
                        onA = curList.Count - pA;
                    }

                    //return and reset if too far away or end of the line
                    if (pB >= curList.Count - 2)
                        completeYouTurn = true;
                }

                //just need to make sure the points continue ascending in list order or heading switches all over the place
                if (pA > pB) { int C = pA; pA = pB; pB = C; }

                currentLocationIndex = pA;

                if (isContourBtnOn)
                {
                    if (isLocked && (pA < 2 || pB > curList.Count - 3))
                    {
                        //ctList.Clear();
                        isLocked = false;
                        lastLockPt = int.MaxValue;
                        return;
                    }
                }

                //get the distance from currently active AB line
                //x2-x1
                double dx = curList[pB].easting - curList[pA].easting;
                //z2-z1
                double dy = curList[pB].northing - curList[pA].northing;

                if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dy * pivot.easting) - (dx * pivot.northing) + (curList[pB].easting
                            * curList[pA].northing) - (curList[pB].northing * curList[pA].easting))
                                / Math.Sqrt((dy * dy) + (dx * dx));

                //integral slider is set to 0
                if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
                {
                    pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                    if (counter2++ > 4)
                    {
                        pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                        pivotDistanceErrorLast = pivotDistanceError;
                        counter2 = 0;
                        pivotDerivative *= 2;
                    }

                    if (!isYouTurnTriggered && mf.isAutoSteerBtnOn && Math.Abs(pivotDerivative) < 0.1 && mf.avgSpeed > 2.5)
                    {
                        //if over the line heading wrong way, rapidly decrease integral
                        if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                        {
                            inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * (isContourBtnOn ? -0.06 : -0.04);
                        }
                        else
                        {
                            if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                            {
                                inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.02;
                                if (inty > 0.2) inty = 0.2;
                                else if (inty < -0.2) inty = -0.2;
                            }
                        }
                    }
                    else inty *= 0.95;
                }
                else inty = 0;

                if (isContourBtnOn)
                    isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - curList[pA].heading) - Math.PI) < glm.PIBy2;

                if (!isYouTurnTriggered && !isHeadingSameWay)
                    distanceFromCurrentLinePivot *= -1.0;

                // ** Pure pursuit ** - calc point on ABLine closest to current position
                double U = (((pivot.easting - curList[pA].easting) * dx) + ((pivot.northing - curList[pA].northing) * dy))
                        / ((dx * dx) + (dy * dy));

                rEast = curList[pA].easting + (U * dx);
                rNorth = curList[pA].northing + (U * dy);
                CurrentHeading = curList[pA].heading;

                //update base on autosteer settings and distance from line
                double goalPointDistance = mf.vehicle.UpdateGoalPointDistance() * (isYouTurnTriggered ? 0.8 : 1.0);

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
                            double dist = goalPointDistance - distSoFar;
                            goalPoint.northing = start.northing + (Math.Cos(start.heading) * (CountUp ? dist : -dist));
                            goalPoint.easting = start.easting + (Math.Sin(start.heading) * (CountUp ? dist : -dist));
                            break;
                        }

                        if (i == curList.Count - 1)//goalPointDistance is longer than remaining u-turn
                            completeYouTurn = true;
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

                //fill in the autosteer variables
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
            int ptCount = recList.Count;

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

                //save current position
                currentPositonIndex = pA;
            }
            else
            {
                //find the closest 2 points to current fix
                for (int t = 0; t < ptCount; t++)
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
                pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                if (counter2++ > 4)
                {
                    pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                    pivotDistanceErrorLast = pivotDistanceError;
                    counter2 = 0;
                    pivotDerivative *= 2;

                }

                if (mf.isAutoSteerBtnOn && Math.Abs(pivotDerivative) < 0.1 && mf.avgSpeed > 2.5)
                {
                    //if over the line heading wrong way, rapidly decrease integral
                    if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                    {
                        inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.04;
                    }
                    else
                    {
                        if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                        {
                            inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.02;
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

            for (int i = ReverseHeading ? pB : pA; i < ptCount && i >= 0; i += count)
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

            //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * Math.Tan(glm.toRadians(steerAngle)) / mf.vehicle.wheelbase;

            //clamp the steering angle to not exceed safe angular velocity
            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
            {
                steerAngle = glm.toDegrees(Math.Atan(mf.vehicle.wheelbase * (steerAngle > 0 ? mf.vehicle.maxAngularVelocity : -mf.vehicle.maxAngularVelocity)
                    / (glm.twoPI * mf.avgSpeed * 0.277777)));
            }

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }
        #endregion Pure Pursuit
    }
}
