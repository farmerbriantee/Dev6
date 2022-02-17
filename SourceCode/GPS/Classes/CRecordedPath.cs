using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CGuidance
    {
        //the recorded path from driving around
        public List<CRecPathPt> recList = new List<CRecPathPt>();

        public int currentPositonIndex;

        public bool isEndOfTheRecLine, isRecordOn;
        public bool isDrivingRecordedPath, isFollowingDubinsToPath, isFollowingRecPath, isFollowingDubinsHome;

        int starPathIndx = 0;

        public bool StartDrivingRecordedPath()
        {
            //create the dubins path based on start and goal to start of recorded path
            currentPositonIndex = 0;
            if (recList.Count < 5) return false;

            //save a copy of where we started.
            vec3 homePos = mf.pivotAxlePos;

            // Try to find the nearest point of the recordet path in relation to the current position:
            double distance = double.MaxValue;
            int idx = 0;

            for (int i = 0; i < recList.Count; i++)
            {
                double dist = ((recList[i].easting - homePos.easting) * (recList[i].easting - homePos.easting)) + ((recList[i].northing - homePos.northing) * (recList[i].northing - homePos.northing));
                if (dist < distance)
                {
                    distance = dist;
                    idx = i;
                }
            }
            if (idx + 8 < recList.Count)
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

            //has a valid dubins path been created?
            if (curList.Count == 0) return false;

            starPathIndx = idx;
            
            //technically all good if we get here so set all the flags
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = true;
            isEndOfTheRecLine = false;
            isDrivingRecordedPath = true;
            return true;
        }

        public void UpdatePosition()
        {
            if (isFollowingDubinsToPath)
            {
                //set a speed of 10 kmh
                mf.sim.stepDistance = 9.0 / 34.86;

                CalculateSteerAngle(mf.pivotAxlePos, mf.steerAxlePos, curList);

                //check if close to recorded path
                if (curList.Count - pB < 8)
                {
                    vec3 pivotAxlePosRP = mf.pivotAxlePos;

                    double distSqr = glm.DistanceSquared(pivotAxlePosRP.northing, pivotAxlePosRP.easting, recList[starPathIndx].northing, recList[starPathIndx].easting);
                    if (distSqr < 2)
                    {
                        isFollowingRecPath = true;
                        isFollowingDubinsToPath = false;
                        curList.Clear();
                        currentPositonIndex = starPathIndx;
                    }
                }
            }

            if (isFollowingRecPath)
            {
                PurePursuitRecPath(mf.pivotAxlePos, recList);

                //if end of the line then stop
                if (!isEndOfTheRecLine)
                {
                    mf.sim.stepDistance = recList[currentPositonIndex].speed / 34.86;

                    //section control - only if different click the button
                    bool autoBtn = (mf.autoBtnState == FormGPS.btnStates.Auto);
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
                if (curList.Count - pB < 3)
                {
                    StopDrivingRecordedPath();
                    return;
                }

                mf.sim.stepDistance = 9.0 / 35;

                //StanleyDubinsPath(shuttleListCount);
                CalculateSteerAngle(mf.pivotAxlePos, mf.steerAxlePos, curList);
            }
        }

        public void StopDrivingRecordedPath()
        {
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = false;
            curList.Clear();
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
            vec3 pivotAxlePosRP = mf.pivotAxlePos;

            //bump it forward
            vec3 pt2 = new vec3
            {
                easting = pivotAxlePosRP.easting + (Math.Sin(pivotAxlePosRP.heading) * 3),
                northing = pivotAxlePosRP.northing + (Math.Cos(pivotAxlePosRP.heading) * 3),
                heading = pivotAxlePosRP.heading
            };

            //get the dubins path vec3 point coordinates of turn
            curList = dubPath.GenerateDubins(pt2, goal);
            curList.Insert(0, mf.pivotAxlePos);
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
    }
}