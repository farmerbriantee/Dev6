﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CRecPathPt
    {
        public double easting { get; set; }
        public double northing { get; set; }
        public double heading { get; set; }
        public double speed { get; set; }
        public bool autoBtnState { get; set; }

        //constructor
        public CRecPathPt(double _easting, double _northing, double _heading, double _speed,
                            bool _autoBtnState)
        {
            easting = _easting;
            northing = _northing;
            heading = _heading;
            speed = _speed;
            autoBtnState = _autoBtnState;
        }
    }

    public partial class CGuidance
    {
        //the recorded path from driving around
        public List<CRecPathPt> recList = new List<CRecPathPt>();

        public int recListCount;

        //the dubins path to get there
        public List<CRecPathPt> shuttleDubinsList = new List<CRecPathPt>();

        public int shuttleListCount;

        //list of vec3 points of Dubins shortest path between 2 points - To be converted to RecPt
        public List<vec3> shortestDubinsList = new List<vec3>();

        public int currentPositonIndex;

        //pure pursuit values
        public vec3 pivotAxlePosRP = new vec3(0, 0, 0);

        public vec3 homePos = new vec3();
        public vec2 goalPointRP = new vec2(0, 0);

        public bool isBtnFollowOn, isEndOfTheRecLine, isRecordOn;
        public bool isDrivingRecordedPath, isFollowingDubinsToPath, isFollowingRecPath, isFollowingDubinsHome;

        int starPathIndx = 0;

        public bool StartDrivingRecordedPath()
        {
            //create the dubins path based on start and goal to start of recorded path
            pA = pB = currentPositonIndex = 0;
            recListCount = recList.Count;
            if (recListCount < 5) return false;

            //save a copy of where we started.
            homePos = mf.pivotAxlePos;

            // Try to find the nearest point of the recordet path in relation to the current position:
            double distance = double.MaxValue;
            int idx = 0;
            int i = 0;
            foreach(CRecPathPt pt in recList)
            {
                if((Math.Sqrt((((pt.easting- homePos.easting)* (pt.easting - homePos.easting)) + ((pt.northing- homePos.northing)*(pt.northing- homePos.northing))))) < distance)
                {
                    distance = Math.Sqrt((((pt.easting - homePos.easting) * (pt.easting - homePos.easting)) + ((pt.northing - homePos.northing) * (pt.northing - homePos.northing))));
                    idx = i;
                }
                i++;
            }
            if (idx + 8 < recListCount)
            {
                // Set the point where to lock on the recorded path four point forward
                idx += 4;
            }
            else
            {
                // if the nearest position is to near by the end of the recorded path,
                // create DubinPath to the starting point!
                idx = 0;
            }
            //the goal is the first point of path, the start is the current position
            vec3 goal = new vec3(recList[idx].easting, recList[idx].northing, recList[idx].heading);

            
             
            //get the dubins for approach to recorded path
            GetDubinsPath(goal);
            shuttleListCount = shuttleDubinsList.Count;

            //has a valid dubins path been created?
            if (shuttleListCount == 0) return false;

            starPathIndx = idx;
            
            //technically all good if we get here so set all the flags
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = true;
            isEndOfTheRecLine = false;
            currentPositonIndex = 0;
            isDrivingRecordedPath = true;
            return true;
        }

        public bool trig;
        public double north;
        public int pathCount = 0;

        public void UpdatePosition()
        {
            if (isFollowingDubinsToPath)
            {
                //set a speed of 10 kmh
                mf.sim.stepDistance = shuttleDubinsList[currentPositonIndex].speed / 34.86;

                pivotAxlePosRP = mf.pivotAxlePos;

                //StanleyDubinsPath(shuttleListCount);
                PurePursuitDubins(shuttleListCount);

                //check if close to recorded path
                int cnt = shuttleDubinsList.Count;
                pathCount = cnt - pB;
                if (pathCount < 8)
                {
                    double distSqr = glm.DistanceSquared(pivotAxlePosRP.northing, pivotAxlePosRP.easting, recList[starPathIndx].northing, recList[starPathIndx].easting);
                    if (distSqr < 2)
                    {
                        isFollowingRecPath = true;
                        isFollowingDubinsToPath = false;
                        shuttleDubinsList.Clear();
                        shortestDubinsList.Clear();
                        currentPositonIndex = starPathIndx;
                        pA = currentPositonIndex + 3;
                        pB = pA + 1;
                    }
                }
            }

            if (isFollowingRecPath)
            {
                pivotAxlePosRP = mf.pivotAxlePos;

                //StanleyRecPath(recListCount);
                PurePursuitRecPath(recListCount);

                //if end of the line then stop
                if (!isEndOfTheRecLine)
                {
                    mf.sim.stepDistance = recList[currentPositonIndex].speed / 34.86;
                    north = recList[currentPositonIndex].northing;

                    pathCount = recList.Count - currentPositonIndex;

                    //section control - only if different click the button
                    bool autoBtn = (mf.autoBtnState == FormGPS.btnStates.Auto);
                    trig = autoBtn;
                    if (autoBtn != recList[currentPositonIndex].autoBtnState) mf.btnSectionOffAutoOn.PerformClick();
                }
                else
                {
                    StopDrivingRecordedPath();
                    return;

                    //create the dubins path based on start and goal to start trip home
                    //GetDubinsPath(homePos);
                    //shuttleListCount = shuttleDubinsList.Count;

                    ////its too small
                    //if (shuttleListCount < 3)
                    //{
                    //    StopDrivingRecordedPath();
                    //    return;
                    //}

                    ////set all the flags
                    //isFollowingDubinsHome = true;
                    //A = B = C = 0;
                    //isFollowingRecPath = false;
                    //isFollowingDubinsToPath = false;
                    //isEndOfTheRecLine = false;
                }
            }

            if (isFollowingDubinsHome)
            {
                int cnt = shuttleDubinsList.Count;
                pathCount = cnt - pB;
                if (pathCount < 3)
                {
                    StopDrivingRecordedPath();
                    return;
                }

                mf.sim.stepDistance = shuttleDubinsList[currentPositonIndex].speed / 35;
                pivotAxlePosRP = mf.pivotAxlePos;

                //StanleyDubinsPath(shuttleListCount);
                PurePursuitDubins(shuttleListCount);
            }
        }

        public void StopDrivingRecordedPath()
        {
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = false;
            shuttleDubinsList.Clear();
            shortestDubinsList.Clear();
            mf.sim.stepDistance = 0;
            isDrivingRecordedPath = false;
            mf.btnPathGoStop.Image = Properties.Resources.boundaryPlay;
            mf.btnPathRecordStop.Enabled = true;
            mf.btnPathDelete.Enabled = true;

        }

        private void GetDubinsPath(vec3 goal)
        {
            CDubins.turningRadius = mf.vehicle.minTurningRadius * 2.0;
            CDubins dubPath = new CDubins();

            // current psition
            pivotAxlePosRP = mf.pivotAxlePos;

            //bump it forward
            vec3 pt2 = new vec3
            {
                easting = pivotAxlePosRP.easting + (Math.Sin(pivotAxlePosRP.heading) * 3),
                northing = pivotAxlePosRP.northing + (Math.Cos(pivotAxlePosRP.heading) * 3),
                heading = pivotAxlePosRP.heading
            };

            //get the dubins path vec3 point coordinates of turn
            shortestDubinsList.Clear();
            shuttleDubinsList.Clear();

            shortestDubinsList = dubPath.GenerateDubins(pt2, goal);

            //if Dubins returns 0 elements, there is an unavoidable blockage in the way.
            if (shortestDubinsList.Count > 0)
            {
                shortestDubinsList.Insert(0, mf.pivotAxlePos);

                //transfer point list to recPath class point style
                for (int i = 0; i < shortestDubinsList.Count; i++)
                {
                    CRecPathPt pt = new CRecPathPt(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading, 9.0, false);
                    shuttleDubinsList.Add(pt);
                }
                return;
            }
        }

        private void PurePursuitRecPath(int ptCount)
        {
            //find the closest 2 points to current fix
            double minDistA = 9999999999;
            double dist, dx, dz;


            //set the search range close to current position
            int top = currentPositonIndex + 5;
            if (top > ptCount) top = ptCount;

            for (int t = currentPositonIndex; t < top; t++)
            {
                dist = ((pivotAxlePosRP.easting - recList[t].easting) * (pivotAxlePosRP.easting - recList[t].easting))
                                + ((pivotAxlePosRP.northing - recList[t].northing) * (pivotAxlePosRP.northing - recList[t].northing));
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

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivotAxlePosRP.easting) - (dx * pivotAxlePosRP.northing) + (recList[pB].easting
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
            double U = (((pivotAxlePosRP.easting - recList[pA].easting) * dx)
                        + ((pivotAxlePosRP.northing - recList[pA].northing) * dz))
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

                    goalPointRP.easting = (((1 - j) * start.easting) + (j * recList[i].easting));
                    goalPointRP.northing = (((1 - j) * start.northing) + (j * recList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = recList[i];
            }

            //calc "D" the distance from pivotAxlePosRP axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPointRP.northing, goalPointRP.easting, pivotAxlePosRP.northing, pivotAxlePosRP.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

            double localHeading = glm.twoPI - mf.fixHeading + inty;

            ppRadius = goalPointDistanceSquared / (2 * (((goalPointRP.easting - pivotAxlePosRP.easting) * Math.Cos(localHeading)) + ((goalPointRP.northing - pivotAxlePosRP.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPointRP.easting - pivotAxlePosRP.easting) * Math.Cos(localHeading))
                + ((goalPointRP.northing - pivotAxlePosRP.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivotAxlePosRP.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivotAxlePosRP.northing + (ppRadius * Math.Sin(localHeading));

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

        private void PurePursuitDubins(int ptCount)
        {
            double dist, dx, dz;
            double minDistA = double.MaxValue, minDistB = double.MaxValue;


            //find the closest 2 points to current fix
            for (int t = 0; t < ptCount; t++)
            {
                dist = ((pivotAxlePosRP.easting - shuttleDubinsList[t].easting) * (pivotAxlePosRP.easting - shuttleDubinsList[t].easting))
                                + ((pivotAxlePosRP.northing - shuttleDubinsList[t].northing) * (pivotAxlePosRP.northing - shuttleDubinsList[t].northing));
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

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dz * pivotAxlePosRP.easting) - (dx * pivotAxlePosRP.northing) + (shuttleDubinsList[pB].easting
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
            double U = (((pivotAxlePosRP.easting - shuttleDubinsList[pA].easting) * dx)
                        + ((pivotAxlePosRP.northing - shuttleDubinsList[pA].northing) * dz))
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

                    goalPointRP.easting = (((1 - j) * start.easting) + (j * shuttleDubinsList[i].easting));
                    goalPointRP.northing = (((1 - j) * start.northing) + (j * shuttleDubinsList[i].northing));
                    break;
                }
                else distSoFar += tempDist;
                start = shuttleDubinsList[i];
            }

            //calc "D" the distance from pivotAxlePosRP axle to lookahead point
            double goalPointDistanceSquared = glm.DistanceSquared(goalPointRP.northing, goalPointRP.easting, pivotAxlePosRP.northing, pivotAxlePosRP.easting);

            //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
            //double localHeading = glm.twoPI - mf.fixHeading;

            double localHeading = glm.twoPI - mf.fixHeading + inty;

            ppRadius = goalPointDistanceSquared / (2 * (((goalPointRP.easting - pivotAxlePosRP.easting) * Math.Cos(localHeading)) + ((goalPointRP.northing - pivotAxlePosRP.northing) * Math.Sin(localHeading))));

            steerAngle = glm.toDegrees(Math.Atan(2 * (((goalPointRP.easting - pivotAxlePosRP.easting) * Math.Cos(localHeading))
                + ((goalPointRP.northing - pivotAxlePosRP.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

            if (steerAngle < -mf.vehicle.maxSteerAngle) steerAngle = -mf.vehicle.maxSteerAngle;
            if (steerAngle > mf.vehicle.maxSteerAngle) steerAngle = mf.vehicle.maxSteerAngle;

            if (ppRadius < -500) ppRadius = -500;
            if (ppRadius > 500) ppRadius = 500;

            radiusPoint.easting = pivotAxlePosRP.easting + (ppRadius * Math.Cos(localHeading));
            radiusPoint.northing = pivotAxlePosRP.northing + (ppRadius * Math.Sin(localHeading));

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


        public void DrawRecordedLine()
        {
            int ptCount = recList.Count;
            if (ptCount < 1) return;
            GL.LineWidth(1);
            GL.Color3(0.98f, 0.92f, 0.460f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(recList[h].easting, recList[h].northing, 0);
            GL.End();

            if (mf.isPureDisplayOn)
            {
                //Draw lookahead Point
                GL.PointSize(8.0f);
                GL.Begin(PrimitiveType.Points);

                //GL.Color(1.0f, 1.0f, 0.25f);
                //GL.Vertex(rEast, rNorth, 0.0);

                GL.Color3(1.0f, 0.5f, 0.95f);
                GL.Vertex3(rEast, rNorth, 0.0);
                GL.End();
                GL.PointSize(1.0f);
            }
        }

        public void DrawDubins()
        {
            if (shuttleDubinsList.Count > 1)
            {
                //GL.LineWidth(2);
                GL.PointSize(2);
                GL.Color3(0.298f, 0.96f, 0.2960f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < shuttleDubinsList.Count; h++)
                    GL.Vertex3(shuttleDubinsList[h].easting, shuttleDubinsList[h].northing, 0);
                GL.End();
            }
        }
    }
}
//private void StanleyDubinsPath(int ptCount)
//{
//    //distanceFromCurrentLine = 9999;
//    //find the closest 2 points to current fix
//    double minDistA = 9999999999;
//    for (int t = 0; t < ptCount; t++)
//    {
//        double dist = ((pivotAxlePosRP.easting - shuttleDubinsList[t].easting) * (pivotAxlePosRP.easting - shuttleDubinsList[t].easting))
//                        + ((pivotAxlePosRP.northing - shuttleDubinsList[t].northing) * (pivotAxlePosRP.northing - shuttleDubinsList[t].northing));
//        if (dist < minDistA)
//        {
//            minDistA = dist;
//            A = t;
//        }
//    }

//    //save the closest point
//    C = A;
//    //next point is the next in list
//    B = A + 1;
//    if (B == ptCount) { A--; B--; }                //don't go past the end of the list - "end of the line" trigger

//    //get the distance from currently active AB line
//    //x2-x1
//    double dx = shuttleDubinsList[B].easting - shuttleDubinsList[A].easting;
//    //z2-z1
//    double dz = shuttleDubinsList[B].northing - shuttleDubinsList[A].northing;

//    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

//    //abHeading = Math.Atan2(dz, dx);
//    abHeading = shuttleDubinsList[A].heading;

//    //how far from current AB Line is fix
//    distanceFromCurrentLinePivot = ((dz * pivotAxlePosRP.easting) - (dx * pivotAxlePosRP
//        .northing) + (shuttleDubinsList[B].easting
//                * shuttleDubinsList[A].northing) - (shuttleDubinsList[B].northing * shuttleDubinsList[A].easting))
//                    / Math.Sqrt((dz * dz) + (dx * dx));

//    //are we on the right side or not
//    isOnRightSideCurrentLine = distanceFromCurrentLinePivot > 0;

//    // calc point on ABLine closest to current position
//    double U = (((pivotAxlePosRP.easting - shuttleDubinsList[A].easting) * dx)
//                + ((pivotAxlePosRP.northing - shuttleDubinsList[A].northing) * dz))
//                / ((dx * dx) + (dz * dz));

//    rEastRP = shuttleDubinsList[A].easting + (U * dx);
//    rNorthRP = shuttleDubinsList[A].northing + (U * dz);

//    //the first part of stanley is to extract heading error
//    double abFixHeadingDelta = (pivotAxlePosRP.heading - abHeading);

//    //Fix the circular error - get it from -Pi/2 to Pi/2
//    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
//    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;
//    if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
//    else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

//    //normally set to 1, less then unity gives less heading error.
//    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
//    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
//    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

//    //the non linear distance error part of stanley
//    steerAngleRP = Math.Atan((distanceFromCurrentLinePivot * mf.vehicle.stanleyDistanceErrorGain) / ((mf.pn.speed * 0.277777) + 1));

//    //clamp it to max 42 degrees
//    if (steerAngleRP > 0.74) steerAngleRP = 0.74;
//    if (steerAngleRP < -0.74) steerAngleRP = -0.74;

//    //add them up and clamp to max in vehicle settings
//    steerAngleRP = glm.toDegrees((steerAngleRP + abFixHeadingDelta) * -1.0);
//    if (steerAngleRP < -mf.vehicle.maxSteerAngle) steerAngleRP = -mf.vehicle.maxSteerAngle;
//    if (steerAngleRP > mf.vehicle.maxSteerAngle) steerAngleRP = mf.vehicle.maxSteerAngle;

//    //Convert to millimeters and round properly to above/below .5
//    distanceFromCurrentLinePivot = Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);

//    //every guidance method dumps into these that are used and sent everywhere, last one wins
//    mf.guidanceLineDistanceOff = mf.distanceDisplaySteer = (Int16)distanceFromCurrentLinePivot;
//    mf.guidanceLineSteerAngle = (Int16)(steerAngleRP * 100);
//}
//private void StanleyRecPath(int ptCount)
//{
//    //find the closest 2 points to current fix
//    double minDistA = 9999999999;

//    //set the search range close to current position
//    int top = currentPositonIndex + 5;
//    if (top > ptCount) top = ptCount;

//    double dist;
//    for (int t = currentPositonIndex; t < top; t++)
//    {
//        dist = ((pivotAxlePosRP.easting - recList[t].easting) * (pivotAxlePosRP.easting - recList[t].easting))
//                        + ((pivotAxlePosRP.northing - recList[t].northing) * (pivotAxlePosRP.northing - recList[t].northing));
//        if (dist < minDistA)
//        {
//            minDistA = dist;
//            A = t;
//        }
//    }

//    //Save the closest point
//    C = A;

//    //next point is the next in list
//    B = A + 1;
//    if (B == ptCount)
//    {
//        //don't go past the end of the list - "end of the line" trigger
//        A--;
//        B--;
//        isEndOfTheRecLine = true;
//    }

//    //save current position
//    currentPositonIndex = A;

//    //get the distance from currently active AB line
//    double dx = recList[B].easting - recList[A].easting;
//    double dz = recList[B].northing - recList[A].northing;

//    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

//    abHeading = Math.Atan2(dx, dz);
//    //abHeading = recList[A].heading;

//    //how far from current AB Line is fix
//    distanceFromCurrentLinePivot =
//        ((dz * pivotAxlePosRP.easting) - (dx * pivotAxlePosRP.northing) + (recList[B].easting
//                * recList[A].northing) - (recList[B].northing * recList[A].easting))
//                    / Math.Sqrt((dz * dz) + (dx * dx));

//    //are we on the right side or not
//    isOnRightSideCurrentLine = distanceFromCurrentLinePivot > 0;

//    // calc point on ABLine closest to current position
//    double U = (((pivotAxlePosRP.easting - recList[A].easting) * dx)
//                + ((pivotAxlePosRP.northing - recList[A].northing) * dz))
//                / ((dx * dx) + (dz * dz));

//    rEastRP = recList[A].easting + (U * dx);
//    rNorthRP = recList[A].northing + (U * dz);

//    //the first part of stanley is to extract heading error
//    double abFixHeadingDelta = (pivotAxlePosRP.heading - abHeading);

//    //Fix the circular error - get it from -Pi/2 to Pi/2
//    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
//    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;
//    if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
//    else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

//    //normally set to 1, less then unity gives less heading error.
//    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
//    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
//    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

//    //the non linear distance error part of stanley
//    steerAngleRP = Math.Atan((distanceFromCurrentLinePivot * mf.vehicle.stanleyDistanceErrorGain) / ((mf.pn.speed * 0.277777) + 1));

//    //clamp it to max 42 degrees
//    if (steerAngleRP > 0.74) steerAngleRP = 0.74;
//    if (steerAngleRP < -0.74) steerAngleRP = -0.74;

//    //add them up and clamp to max in vehicle settings
//    steerAngleRP = glm.toDegrees((steerAngleRP + abFixHeadingDelta) * -1.0);
//    if (steerAngleRP < -mf.vehicle.maxSteerAngle) steerAngleRP = -mf.vehicle.maxSteerAngle;
//    if (steerAngleRP > mf.vehicle.maxSteerAngle) steerAngleRP = mf.vehicle.maxSteerAngle;

//    //Convert to millimeters and round properly to above/below .5
//    distanceFromCurrentLinePivot = Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);

//    //every guidance method dumps into these that are used and sent everywhere, last one wins
//    mf.guidanceLineDistanceOff = mf.distanceDisplaySteer = (Int16)distanceFromCurrentLinePivot;
//    mf.guidanceLineSteerAngle = (Int16)(steerAngleRP * 100);
//}