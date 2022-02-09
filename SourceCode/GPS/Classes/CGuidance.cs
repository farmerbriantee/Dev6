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
        private void DoSteerAngleCalc()
        {
            if (mf.isReverse) steerHeadingError *= -1;
            //Overshoot setting on Stanley tab
            steerHeadingError *= mf.vehicle.stanleyHeadingErrorGain;

            double sped = Math.Abs(mf.avgSpeed);
            if (sped > 1) sped = 1 + 0.277 * (sped - 1);
            else sped = 1;
            double XTEc = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                / (sped));

            xTrackSteerCorrection = (xTrackSteerCorrection * 0.5) + XTEc * (0.5);

            //derivative of steer distance error
            distSteerError = (distSteerError * 0.95) + ((xTrackSteerCorrection * 60) * 0.05);
            if (counter2++ > 5)
            {
                derivativeDistError = distSteerError - lastDistSteerError;
                lastDistSteerError = distSteerError;
                counter2 = 0;
            }

            steerAngle = glm.toDegrees((xTrackSteerCorrection + steerHeadingError) * -1.0);

            if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngle *= 0.5;
            else steerAngle *= (1 - Math.Abs(distanceFromCurrentLineSteer));

            //pivot PID
            pivotDistanceError = (pivotDistanceError * 0.6) + (distanceFromCurrentLinePivot * 0.4);
            //pivotDistanceError = Math.Atan((distanceFromCurrentLinePivot) / (sped)) * 0.2;
            //pivotErrorTotal = pivotDistanceError + pivotDerivative;

            if (mf.pn.speed > mf.startSpeed
                && mf.isAutoSteerBtnOn
                && Math.Abs(derivativeDistError) < 1
                && Math.Abs(pivotDistanceError) < 0.25)
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

                //integral slider is set to 0
                if (mf.vehicle.stanleyIntegralGainAB == 0) inty = 0;
            }
            else inty *= 0.7;

            if (mf.isReverse) inty = 0;

            if (mf.ahrs.imuRoll != 88888)
                steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            else if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            //Convert to millimeters from meters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        public void StanleyGuidanceABLine(vec3 pivot, vec3 steer)
        {
            //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
            double dx = currentABLineP2.easting - currentABLineP1.easting;
            double dy = currentABLineP2.northing - currentABLineP1.northing;
            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dy * pivot.easting) - (dx * pivot.northing) + (currentABLineP2.easting
                        * currentABLineP1.northing) - (currentABLineP2.northing * currentABLineP1.easting))
                            / Math.Sqrt((dy * dy) + (dx * dx));

            if (!isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            double U = (((pivot.easting - currentABLineP1.easting) * dx)
                            + ((pivot.northing - currentABLineP1.northing) * dy))
                            / ((dx * dx) + (dy * dy));

            rEast = currentABLineP1.easting + (U * dx);
            rNorth = currentABLineP1.northing + (U * dy);
            CurrentHeading = curList[pA].heading;

            //get the distance from currently active AB segment of steer axle //////// steer /////////////
            vec3 steerA = new vec3(currentABLineP1);
            vec3 steerB = new vec3(currentABLineP2);


            //create the AB segment to offset
            steerA.easting += (Math.Sin(steerA.heading + glm.PIBy2) * (inty));
            steerA.northing += (Math.Cos(steerA.heading + glm.PIBy2) * (inty));

            steerB.easting += (Math.Sin(steerB.heading + glm.PIBy2) * (inty));
            steerB.northing += (Math.Cos(steerB.heading + glm.PIBy2) * (inty));

            dx = steerB.easting - steerA.easting;
            dy = steerB.northing - steerA.northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (steerB.easting
                        * steerA.northing) - (steerB.northing * steerA.easting))
                            / Math.Sqrt((dy * dy) + (dx * dx));

            if (!isHeadingSameWay)
                distanceFromCurrentLineSteer *= -1.0;

            // calc point on ABLine closest to current position - for display only
            U = (((steer.easting - steerA.easting) * dx)
                            + ((steer.northing - steerA.northing) * dy))
                            / ((dx * dx) + (dy * dy));

            rEast = steerA.easting + (U * dx);
            rNorth = steerA.northing + (U * dy);

            double steerErr = Math.Atan2(rEast - rEast, rNorth - rNorth);
            steerHeadingError = (steer.heading - steerErr);
            //Fix the circular error
            if (steerHeadingError > Math.PI)
                steerHeadingError -= Math.PI;
            else if (steerHeadingError < -Math.PI)
                steerHeadingError += Math.PI;

            if (steerHeadingError > glm.PIBy2)
                steerHeadingError -= Math.PI;
            else if (steerHeadingError < -glm.PIBy2)
                steerHeadingError += Math.PI;

            DoSteerAngleCalc();
        }

        public void StanleyGuidanceCurve(vec3 pivot, vec3 steer, List<vec3> curList)
        {            //calculate required steer angle

            //find the closest point roughly
            int cc = 0, dd;
            int ptCount = curList.Count;
            if (ptCount > 5)
            {
                double minDistA = double.MaxValue, minDistB;

                for (int j = 0; j < ptCount; j += 10)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = minDistB = double.MaxValue;
                dd = cc + 7; if (dd > ptCount - 1) dd = ptCount;
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

                ////too far from guidance line? Lost? Fresh delete of ref?
                //if (minDistA < (1.5 * (mf.tool.toolWidth * mf.tool.toolWidth)))
                //{
                //    if (minDistA == double.MaxValue)
                //        return;
                //}
                //else
                //{
                //    curList.Clear();
                //    return;
                //}

                //just need to make sure the points continue ascending or heading switches all over the place
                if (sA > sB) { int C = sA; sA = sB; sB = C; }

                //currentLocationIndex = sA;
                if (sA > ptCount - 1 || sB > ptCount - 1) return;

                minDistA = minDistB = double.MaxValue;

                if (isHeadingSameWay)
                {
                    dd = sB; cc = dd - 12; if (cc < 0) cc = 0;
                }
                else
                {
                    cc = sA; dd = sA + 12; if (dd >= ptCount) dd = ptCount - 1;
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

                if (pA > ptCount - 1 || pB > ptCount - 1)
                {
                    pA = ptCount - 2;
                    pB = ptCount - 1;
                }

                vec3 pivA = new vec3(curList[pA]);
                vec3 pivB = new vec3(curList[pB]);

                if (!isHeadingSameWay)
                {
                    pivA = curList[pB];
                    pivB = curList[pA];

                    pivA.heading += Math.PI;
                    if (pivA.heading > glm.twoPI) pivA.heading -= glm.twoPI;
                }

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx = pivB.easting - pivA.easting;
                double dz = pivB.northing - pivA.northing;

                if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (pivB.easting
                            * pivA.northing) - (pivB.northing * pivA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                double U = (((pivot.easting - pivA.easting) * dx)
                                + ((pivot.northing - pivA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEast = pivA.easting + (U * dx);
                rNorth = pivA.northing + (U * dz);
                CurrentHeading = pivA.heading;

                currentLocationIndex = pA;

                //get the distance from currently active AB segment of steer axle //////// steer /////////////
                vec3 steerA = new vec3(curList[sA]);
                vec3 steerB = new vec3(curList[sB]);

                if (!isHeadingSameWay)
                {
                    steerA = curList[sB];
                    steerA.heading += Math.PI;
                    if (steerA.heading > glm.twoPI) steerA.heading -= glm.twoPI;

                    steerB = curList[sA];
                    steerB.heading += Math.PI;
                    if (steerB.heading > glm.twoPI) steerB.heading -= glm.twoPI;
                }

                //double curvature = pivA.heading - steerA.heading;
                //if (curvature > Math.PI) curvature -= Math.PI; else if (curvature < Math.PI) curvature += Math.PI;
                //if (curvature > glm.PIBy2) curvature -= Math.PI; else if (curvature < -glm.PIBy2) curvature += Math.PI;

                ////because of draft 
                //curvature = Math.Sin(curvature) * mf.vehicle.wheelbase * 0.8;
                //pivotCurvatureOffset = (pivotCurvatureOffset * 0.7) + (curvature * 0.3);
                //pivotCurvatureOffset = 0;

                //create the AB segment to offset
                steerA.easting += (Math.Sin(steerA.heading + glm.PIBy2) * (inty));
                steerA.northing += (Math.Cos(steerA.heading + glm.PIBy2) * (inty));

                steerB.easting += (Math.Sin(steerB.heading + glm.PIBy2) * (inty));
                steerB.northing += (Math.Cos(steerB.heading + glm.PIBy2) * (inty));

                dx = steerB.easting - steerA.easting;
                dz = steerB.northing - steerA.northing;

                if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineSteer = ((dz * steer.easting) - (dx * steer.northing) + (steerB.easting
                            * steerA.northing) - (steerB.northing * steerA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                // calc point on ABLine closest to current position - for display only
                U = (((steer.easting - steerA.easting) * dx)
                                + ((steer.northing - steerA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEast = steerA.easting + (U * dx);
                rNorth = steerA.northing + (U * dz);

                //double segHeading = Math.Atan2(rEastSteer - rEastPivot, rNorthSteer - rNorthPivot);

                //steerHeadingError = Math.PI - Math.Abs(Math.Abs(pivot.heading - segHeading) - Math.PI);
                steerHeadingError = steer.heading - steerB.heading;


                //Fix the circular error
                if (steerHeadingError > Math.PI) steerHeadingError -= Math.PI;
                else if (steerHeadingError < Math.PI) steerHeadingError += Math.PI;

                if (steerHeadingError > glm.PIBy2) steerHeadingError -= Math.PI;
                else if (steerHeadingError < -glm.PIBy2) steerHeadingError += Math.PI;

                DoSteerAngleCalc();
            }
            else
            {
                //invalid distance so tell AS module
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        public void StanleyGuidanceContour(vec3 pivot, vec3 steer, List<vec3> ctList)
        {

            double minDistA = double.MaxValue, minDistB = double.MaxValue;

            //find the closest 2 points to current fix
            for (int t = 0; t < ctList.Count; t++)
            {
                double dist = ((steer.easting - ctList[t].easting) * (steer.easting - ctList[t].easting))
                                + ((steer.northing - ctList[t].northing) * (steer.northing - ctList[t].northing));
                if (dist < minDistA)
                {
                    minDistB = minDistA;
                    sB = sA;
                    minDistA = dist;
                    sA = t;
                }
                else if (dist < minDistB)
                {
                    minDistB = dist;
                    sB = t;
                }
            }

            //just need to make sure the points continue ascending in list order or heading switches all over the place
            if (sA > sB) { int C = sA; sA = sB; sB = C; }

            //get the distance from currently active AB line
            //x2-x1
            double dx = ctList[sB].easting - ctList[sA].easting;
            //z2-z1
            double dy = ctList[sB].northing - ctList[sA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (ctList[sB].easting
                        * ctList[sA].northing) - (ctList[sB].northing * ctList[sA].easting))
                            / Math.Sqrt((dy * dy) + (dx * dx));

            abHeading = Math.Atan2(dx, dy);
            if (abHeading < 0) abHeading += glm.twoPI;

            isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(steer.heading - abHeading) - Math.PI) < glm.PIBy2;

            // calc point on ABLine closest to current position
            double U = (((steer.easting - ctList[sA].easting) * dx) + ((steer.northing - ctList[sA].northing) * dy))
                        / ((dx * dx) + (dy * dy));

            rEast = ctList[sA].easting + (U * dx);
            rNorth = ctList[sA].northing + (U * dy);

            //distance is negative if on left, positive if on right
            if (isHeadingSameWay)
            {
                abFixHeadingDelta = (steer.heading - abHeading);
            }
            else
            {
                distanceFromCurrentLineSteer *= -1.0;
                abFixHeadingDelta = (steer.heading - abHeading + Math.PI);
            }

            //Fix the circular error
            if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
            else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;

            if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
            else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

            if (mf.isReverse) abFixHeadingDelta *= -1;

            abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
            if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
            if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

            steerAngle = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                / ((Math.Abs(mf.pn.speed) * 0.277777) + 1));

            if (steerAngle > 0.74) steerAngle = 0.74;
            if (steerAngle < -0.74) steerAngle = -0.74;

            steerAngle = glm.toDegrees((steerAngle + abFixHeadingDelta) * -1.0);

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;


            //fill in the autosteer variables
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        public void StanleyGuidanceYouTurn(vec3 pivot, vec3 steer, List<vec3> ytList)
        {
            double minDistA = double.MaxValue, minDistB = double.MaxValue;

            //find the closest 2 points to current fix
            for (int t = 0; t < ytList.Count; t++)
            {
                double dist = ((steer.easting - ytList[t].easting) * (steer.easting - ytList[t].easting))
                                + ((steer.northing - ytList[t].northing) * (steer.northing - ytList[t].northing));
                if (dist < minDistA)
                {
                    minDistB = minDistA;
                    B = A;
                    minDistA = dist;
                    A = t;
                }
                else if (dist < minDistB)
                {
                    minDistB = dist;
                    B = t;
                }
            }

            if (minDistA > 16)
            {
                CompleteYouTurn();
                return;
            }

            //just need to make sure the points continue ascending or heading switches all over the place
            if (A > B) { int C = A; A = B; B = C; }

            //minDistA = 100;
            //int closestPt = 0;
            //for (int i = 0; i < ptCount; i++)
            //{
            //    double distancePiv = glm.DistanceSquared(ytList[i], pivot);
            //    if (distancePiv < minDistA)
            //    {
            //        minDistA = distancePiv;
            //    }
            //}


            //feed backward to turn slower to keep pivot on
            A -= 7;
            if (A < 0)
            {
                A = 0;
            }
            B = A + 1;

            //return and reset if too far away or end of the line
            if (B >= ytList.Count - 8)
            {
                CompleteYouTurn();
                return;
            }

            //get the distance from currently active AB line, precalc the norm of line
            double dx = ytList[B].easting - ytList[A].easting;
            double dz = ytList[B].northing - ytList[A].northing;
            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            double abHeading = ytList[A].heading;

            //how far from current AB Line is steer point 90 degrees from steer position
            distanceFromCurrentLineSteer = ((dz * steer.easting) - (dx * steer.northing) + (ytList[B].easting
                        * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //Calc point on ABLine closest to current position and 90 degrees to segment heading
            double U = (((steer.easting - ytList[A].easting) * dx)
                        + ((steer.northing - ytList[A].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            //critical point used as start for the uturn path - critical
            rEast = ytList[A].easting + (U * dx);
            rNorth = ytList[A].northing + (U * dz);

            //the first part of stanley is to extract heading error
            double abFixHeadingDelta = (steer.heading - abHeading);

            //Fix the circular error - get it from -Pi/2 to Pi/2
            if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
            else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;
            if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
            else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

            if (mf.isReverse) abFixHeadingDelta *= -1;
            //normally set to 1, less then unity gives less heading error.
            abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
            if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
            if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

            //the non linear distance error part of stanley
            steerAngle = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain) / ((mf.pn.speed * 0.277777) + 1));

            //clamp it to max 42 degrees
            if (steerAngle > 0.74) steerAngle = 0.74;
            if (steerAngle < -0.74) steerAngle = -0.74;

            //add them up and clamp to max in vehicle settings
            steerAngle = glm.toDegrees((steerAngle + abFixHeadingDelta) * -1.0);
            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLineSteer * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }
        #endregion

        #region Pure Pursuit
        public void PurePursuitABLine(vec3 pivot, vec3 steer)
        {
            //get the distance from currently active AB line
            //x2-x1
            double dx = currentABLineP2.easting - currentABLineP1.easting;
            //z2-z1
            double dy = currentABLineP2.northing - currentABLineP1.northing;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dy * pivot.easting) - (dx * pivot.northing) + (currentABLineP2.easting
                        * currentABLineP1.northing) - (currentABLineP2.northing * currentABLineP1.easting))
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

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isAutoSteerBtnOn
                    && Math.Abs(pivotDerivative) < (0.1)
                    && mf.avgSpeed > 2.5
                    && !isYouTurnTriggered)
                //&& Math.Abs(pivotDistanceError) < 0.2)

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

            //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
            abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
            if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - glm.twoPI);

            // ** Pure pursuit ** - calc point on ABLine closest to current position
            double U = (((pivot.easting - currentABLineP1.easting) * dx)
                        + ((pivot.northing - currentABLineP1.northing) * dy))
                        / ((dx * dx) + (dy * dy));

            //point on AB line closest to pivot axle point
            rEast = currentABLineP1.easting + (U * dx);
            rNorth = currentABLineP1.northing + (U * dy);
            CurrentHeading = currentABLineP1.heading;

            //update base on autosteer settings and distance from line
            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

            if (mf.isReverse ? isHeadingSameWay : !isHeadingSameWay)
            {
                goalPoint.easting = rEast - (Math.Sin(abHeading) * goalPointDistance);
                goalPoint.northing = rNorth - (Math.Cos(abHeading) * goalPointDistance);
            }
            else
            {
                goalPoint.easting = rEast + (Math.Sin(abHeading) * goalPointDistance);
                goalPoint.northing = rNorth + (Math.Cos(abHeading) * goalPointDistance);
            }

            //calc "D" the distance from pivot axle to lookahead point
            double goalPointDistanceDSquared
                = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            //calculate the the new x in local coordinates and steering angle degrees based on wheelbase
            double localHeading;

            if (isHeadingSameWay) localHeading = glm.twoPI - mf.fixHeading + inty;
            else localHeading = glm.twoPI - mf.fixHeading - inty;

            ppRadius = goalPointDistanceDSquared / (2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase
                / goalPointDistanceDSquared));

            if (mf.ahrs.imuRoll != 88888)
                steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            //limit circle size for display purpose
            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivot.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivot.northing + (ppRadius * Math.Sin(localHeading));

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

            //distance is negative if on left, positive if on right
            if (!isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            //Convert to millimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        public void PurePursuitCurve(vec3 pivot, vec3 steer, List<vec3> curList)
        {
            double dist;
            double minDistA = double.MaxValue, minDistB = double.MaxValue;

            //find the closest 2 points to current fix
            for (int t = 0; t < curList.Count; t++)
            {
                dist = glm.DistanceSquared(pivot, curList[t]);

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

            currentLocationIndex = pA;

            //get the distance from currently active AB line
            double dx = curList[pB].easting - curList[pA].easting;
            double dz = curList[pB].northing - curList[pA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (curList[pB].easting
                        * curList[pA].northing) - (curList[pB].northing * curList[pA].easting))
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

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isAutoSteerBtnOn && mf.avgSpeed > 2.5 && Math.Abs(pivotDerivative) < 0.1)
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
            double U = (((pivot.easting - curList[pA].easting) * dx)
                        + ((pivot.northing - curList[pA].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEast = curList[pA].easting + (U * dx);
            rNorth = curList[pA].northing + (U * dz);
            CurrentHeading = curList[pA].heading;

            //update base on autosteer settings and distance from line
            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

            bool ReverseHeading = mf.isReverse ? !isHeadingSameWay : isHeadingSameWay;

            int count = ReverseHeading ? 1 : -1;
            vec3 start = new vec3(rEast, rNorth, 0);
            double distSoFar = 0;

            for (int i = ReverseHeading ? pB : pA; i < curList.Count && i >= 0; i += count)
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
            }

            //calc "D" the distance from pivot axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

            double localHeading;
            if (ReverseHeading) localHeading = glm.twoPI - mf.fixHeading + inty;
            else localHeading = glm.twoPI - mf.fixHeading - inty;

            ppRadius = goalPointDistanceSquared / (2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (mf.ahrs.imuRoll != 88888)
                steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivot.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivot.northing + (ppRadius * Math.Sin(localHeading));

            //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;

            //clamp the steering angle to not exceed safe angular velocity
            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
            {
                steerAngle = glm.toDegrees(steerAngle > 0 ?
                        (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777)))
                    : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777))));
            }

            if (!isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        public void PurePursuitContour(vec3 pivot, vec3 steer, List<vec3> ctList)
        {
            double minDistA = double.MaxValue, minDistB = double.MaxValue;
            //find the closest 2 points to current fix
            for (int t = 0; t < ctList.Count; t++)
            {
                double dist = ((pivot.easting - ctList[t].easting) * (pivot.easting - ctList[t].easting))
                                + ((pivot.northing - ctList[t].northing) * (pivot.northing - ctList[t].northing));
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


            //just need to make sure the points continue ascending in list order or heading switches all over the place
            if (pA > pB) { int C = pA; pA = pB; pB = C; }

            if (isLocked && (pA < 2 || pB > ctList.Count - 3))
            {
                //ctList.Clear();
                isLocked = false;
                lastLockPt = int.MaxValue;
                return;
            }

            //get the distance from currently active AB line
            //x2-x1
            double dx = ctList[pB].easting - ctList[pA].easting;
            //z2-z1
            double dy = ctList[pB].northing - ctList[pA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dy * mf.pn.fix.easting) - (dx * mf.pn.fix.northing) + (ctList[pB].easting
                        * ctList[pA].northing) - (ctList[pB].northing * ctList[pA].easting))
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

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isAutoSteerBtnOn
                    && Math.Abs(pivotDerivative) < (0.1)
                    && mf.avgSpeed > 2.5
                    && !isYouTurnTriggered)
                {
                    //if over the line heading wrong way, rapidly decrease integral
                    if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                    {
                        inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.06;
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

            isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - ctList[pA].heading) - Math.PI) < glm.PIBy2;

            if (!isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            // ** Pure pursuit ** - calc point on ABLine closest to current position
            double U = (((pivot.easting - ctList[pA].easting) * dx) + ((pivot.northing - ctList[pA].northing) * dy))
                    / ((dx * dx) + (dy * dy));

            rEast = ctList[pA].easting + (U * dx);
            rNorth = ctList[pA].northing + (U * dy);

            //update base on autosteer settings and distance from line
            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

            bool ReverseHeading = mf.isReverse ? !isHeadingSameWay : isHeadingSameWay;

            int count = ReverseHeading ? 1 : -1;
            vec3 start = new vec3(rEast, rNorth, 0);
            double distSoFar = 0;

            for (int i = ReverseHeading ? pB : pA; i < ctList.Count && i >= 0; i += count)
            {
                // used for calculating the length squared of next segment.
                double tempDist = glm.Distance(start, ctList[i]);

                //will we go too far?
                if ((tempDist + distSoFar) > goalPointDistance)
                {
                    double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                    goalPoint.easting = (((1 - j) * start.easting) + (j * ctList[i].easting));
                    goalPoint.northing = (((1 - j) * start.northing) + (j * ctList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = ctList[i];
            }

            //calc "D" the distance from pivot axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            double localHeading;// = glm.twoPI - mf.fixHeading;

            if (isHeadingSameWay) localHeading = glm.twoPI - mf.fixHeading + inty;
            else localHeading = glm.twoPI - mf.fixHeading - inty;

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (mf.ahrs.imuRoll != 88888)
                steerAngle += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;

            //clamp the steering angle to not exceed safe angular velocity
            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
            {
                steerAngle = glm.toDegrees(steerAngle > 0 ?
                        (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.pn.speed * 0.277777)))
                    : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.pn.speed * 0.277777))));
            }

            //fill in the autosteer variables
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        public void PurePursuitYouTurn(vec3 pivot, vec3 steer, List<vec3> ytList)
        {
            //grab a copy from main - the steer position
            double minDistA = double.MaxValue, minDistB = double.MaxValue;

            //find the closest 2 points to current fix
            for (int t = 0; t < ytList.Count; t++)
            {
                double dist = ((pivot.easting - ytList[t].easting) * (pivot.easting - ytList[t].easting))
                                + ((pivot.northing - ytList[t].northing) * (pivot.northing - ytList[t].northing));
                if (dist < minDistA)
                {
                    minDistB = minDistA;
                    B = A;
                    minDistA = dist;
                    A = t;
                }
                else if (dist < minDistB)
                {
                    minDistB = dist;
                    B = t;
                }
            }

            //just need to make sure the points continue ascending or heading switches all over the place
            if (A > B) { int C = A; A = B; B = C; }

            minDistA = 100;
            int closestPt = 0;
            for (int i = 0; i < ytList.Count; i++)
            {
                double distancePiv = glm.Distance(ytList[i], mf.pivotAxlePos);
                if (distancePiv < minDistA)
                {
                    minDistA = distancePiv;
                    closestPt = i;
                }
            }

            onA = ytList.Count / 2;
            if (closestPt < onA)
            {
                onA = -closestPt;
            }
            else
            {
                onA = ytList.Count - closestPt;
            }

            //return and reset if too far away or end of the line
            if (B >= ytList.Count - 1)
            {
                CompleteYouTurn();
                return;
            }

            //get the distance from currently active AB line
            double dx = ytList[B].easting - ytList[A].easting;
            double dz = ytList[B].northing - ytList[A].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (ytList[B].easting
                        * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            // ** Pure pursuit ** - calc point on ABLine closest to current position
            double U = (((pivot.easting - ytList[A].easting) * dx)
                        + ((pivot.northing - ytList[A].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEast = ytList[A].easting + (U * dx);
            rNorth = ytList[A].northing + (U * dz);

            //sharp turns on you turn.
            //update base on autosteer settings and distance from line
            double goalPointDistance = 0.8 * mf.vehicle.UpdateGoalPointDistance();

            isHeadingSameWay = true;
            bool ReverseHeading = !mf.isReverse;

            int count = ReverseHeading ? 1 : -1;
            vec3 start = new vec3(rEast, rNorth, 0);
            double distSoFar = 0;

            for (int i = ReverseHeading ? B : A; i < ytList.Count && i >= 0; i += count)
            {
                // used for calculating the length squared of next segment.
                double tempDist = glm.Distance(start, ytList[i]);

                //will we go too far?
                if ((tempDist + distSoFar) > goalPointDistance)
                {
                    double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                    goalPoint.easting = (((1 - j) * start.easting) + (j * ytList[i].easting));
                    goalPoint.northing = (((1 - j) * start.northing) + (j * ytList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = ytList[i];
                if (i == ytList.Count - 1)//goalPointDistance is longer than remaining u-turn
                {
                    CompleteYouTurn();
                    return;
                }
            }

            //calc "D" the distance from pivot axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            double localHeading = glm.twoPI - mf.fixHeading;
            ppRadius = goalPointDistanceSquared / (2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPoint.easting - pivot.easting) * Math.Cos(localHeading))
                + ((goalPoint.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivot.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivot.northing + (ppRadius * Math.Sin(localHeading));

            //distance is negative if on left, positive if on right
            if (!isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        private void PurePursuitRecPath(vec3 pivot, int ptCount)
        {
            //find the closest 2 points to current fix
            double minDistA = double.MaxValue;
            double dist, dx, dz;

            //set the search range close to current position
            int top = currentPositonIndex + 5;
            if (top > ptCount) top = ptCount;

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
            if (pB == ptCount)
            {
                //don't go past the end of the list - "end of the line" trigger
                pA--;
                pB--;
                isEndOfTheRecLine = true;
            }

            //save current position
            currentPositonIndex = pA;

            //get the distance from currently active AB line
            dx = recList[pB].easting - recList[pA].easting;
            dz = recList[pB].northing - recList[pA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (recList[pB].easting
                        * recList[pA].northing) - (recList[pB].northing * recList[pA].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //integral slider is set to 0
            if (mf.vehicle.purePursuitIntegralGain != 0)
            {
                pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                if (counter2++ > 4)
                {
                    pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                    pivotDistanceErrorLast = pivotDistanceError;
                    counter2 = 0;
                    pivotDerivative *= 2;

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isAutoSteerBtnOn
                    && Math.Abs(pivotDerivative) < (0.1)
                    && mf.avgSpeed > 2.5
                    && !isYouTurnTriggered)
                //&& Math.Abs(pivotDistanceError) < 0.2)

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

            if (mf.isReverse) inty = 0;

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

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

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
            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;

            //clamp the steering angle to not exceed safe angular velocity
            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
            {
                steerAngle = glm.toDegrees(steerAngle > 0 ?
                        (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777)))
                    : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777))));
            }

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }

        private void PurePursuitDubins(vec3 pivot, int ptCount)
        {
            double dist, dx, dz;
            double minDistA = double.MaxValue, minDistB = double.MaxValue;


            //find the closest 2 points to current fix
            for (int t = 0; t < ptCount; t++)
            {
                dist = ((pivot.easting - shuttleDubinsList[t].easting) * (pivot.easting - shuttleDubinsList[t].easting))
                                + ((pivot.northing - shuttleDubinsList[t].northing) * (pivot.northing - shuttleDubinsList[t].northing));
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

            //currentLocationIndex = A;

            //get the distance from currently active AB line
            dx = shuttleDubinsList[pB].easting - shuttleDubinsList[pA].easting;
            dz = shuttleDubinsList[pB].northing - shuttleDubinsList[pA].northing;

            if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (shuttleDubinsList[pB].easting
                        * shuttleDubinsList[pA].northing) - (shuttleDubinsList[pB].northing * shuttleDubinsList[pA].easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //integral slider is set to 0
            if (mf.vehicle.purePursuitIntegralGain != 0)
            {
                pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                if (counter2++ > 4)
                {
                    pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                    pivotDistanceErrorLast = pivotDistanceError;
                    counter2 = 0;
                    pivotDerivative *= 2;

                    //limit the derivative
                    //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                    //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                    //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                }

                //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                if (mf.isAutoSteerBtnOn
                    && Math.Abs(pivotDerivative) < (0.1)
                    && mf.avgSpeed > 2.5
                    && !isYouTurnTriggered)

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

            if (mf.isReverse) inty = 0;

            // ** Pure pursuit ** - calc point on ABLine closest to current position
            double U = (((pivot.easting - shuttleDubinsList[pA].easting) * dx)
                        + ((pivot.northing - shuttleDubinsList[pA].northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEast = shuttleDubinsList[pA].easting + (U * dx);
            rNorth = shuttleDubinsList[pA].northing + (U * dz);

            //update base on autosteer settings and distance from line
            double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

            bool ReverseHeading = !mf.isReverse;

            int count = ReverseHeading ? 1 : -1;
            CRecPathPt start = new CRecPathPt(rEast, rNorth, 0, 0, false);
            double distSoFar = 0;

            for (int i = ReverseHeading ? pB : pA; i < ptCount && i >= 0; i += count)
            {
                // used for calculating the length squared of next segment.
                double tempDist = Math.Sqrt((start.easting - shuttleDubinsList[i].easting) * (start.easting - shuttleDubinsList[i].easting)
                    + (start.northing - shuttleDubinsList[i].northing) * (start.northing - shuttleDubinsList[i].northing));

                //will we go too far?
                if ((tempDist + distSoFar) > goalPointDistance)
                {
                    double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                    goalPoint.easting = (((1 - j) * start.easting) + (j * shuttleDubinsList[i].easting));
                    goalPoint.northing = (((1 - j) * start.northing) + (j * shuttleDubinsList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = shuttleDubinsList[i];
            }

            //calc "D" the distance from pivotAxlePosRP axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPoint.northing, goalPoint.easting, pivot.northing, pivot.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

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
            double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngle))) / mf.vehicle.wheelbase;

            //clamp the steering angle to not exceed safe angular velocity
            if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
            {
                steerAngle = glm.toDegrees(steerAngle > 0 ?
                        (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777)))
                    : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777))));
            }

            //Convert to centimeters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngle * 100);
        }
        #endregion Pure Pursuit
    }
}
