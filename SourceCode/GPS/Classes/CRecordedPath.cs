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

        public int resumeState;

        public bool StartDrivingRecordedPath()
        {
            //create the dubins path based on start and goal to start of recorded path
            if (recList.Count < 5) return false;

            if (resumeState == 0) //start at the start
                currentPositonIndex = 0;
            else if (resumeState == 1) //resume from where stopped mid path
            {
                if (currentPositonIndex + 5 > recList.Count)
                    currentPositonIndex = 0;
            }
            else //find closest point
            {
                // Try to find the nearest point of the recordet path in relation to the current position:
                double distance = double.MaxValue;
                int idx = 0;

                for (int i = 0; i < recList.Count; i++)
                {
                    double temp = glm.Distance(mf.pivotAxlePos, recList[i]);

                    if (temp < distance)
                    {
                        distance = temp;
                        idx = i;
                    }
                    i++;
                }

                //scootch down the line a bit
                if (idx + 5 < recList.Count) idx += 5;
                else idx = recList.Count - 1;

                currentPositonIndex = idx;
            }

            if (currentPositonIndex < recList.Count)
            {
                //the goal is the first point of path, the start is the current position
                //get the dubins for approach to recorded path
                GetDubinsPath(recList[currentPositonIndex]);

                //has a valid dubins path been created?
                if (curList.points.Count < 2) return false;

                //technically all good if we get here so set all the flags
                isFollowingDubinsHome = false;
                isFollowingRecPath = false;
                isFollowingDubinsToPath = true;
                isEndOfTheRecLine = false;
                isDrivingRecordedPath = true;
                return true;
            }
            else
            {
                isDrivingRecordedPath = false;
                return false;
            }
        }

        public void UpdatePosition(vec2 pivot, vec2 steer, double heading)
        {
            if (isFollowingDubinsToPath)
            {
                //set a speed of 10 kmh
                mf.sim.stepDistance = 2.77778;

                CalculateSteerAngle(pivot, steer, heading, curList);

                //check if close to recorded path
                if (curList.points.Count - pB < 8)
                {
                    double distSqr = glm.Distance(pivot, recList[currentPositonIndex]);
                    if (distSqr < 4)
                    {
                        isFollowingRecPath = true;
                        isFollowingDubinsToPath = false;
                        curList = new Polyline();
                    }
                }
            }

            if (isFollowingRecPath)
            {
                PurePursuitRecPath(pivot, heading, recList);

                //if end of the line then stop
                if (!isEndOfTheRecLine)
                {
                    mf.sim.stepDistance = recList[currentPositonIndex].speed / 3.6;

                    mf.setSectionBtnState(recList[currentPositonIndex].autoBtnState ? btnStates.Auto : btnStates.Off);
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
                if (curList.points.Count - pB < 3)
                {
                    StopDrivingRecordedPath();
                    return;
                }

                mf.sim.stepDistance = 2.77778;

                //StanleyDubinsPath(shuttleListCount);
                CalculateSteerAngle(pivot, steer, heading, curList);
            }
        }

        public void StopDrivingRecordedPath()
        {
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = false;
            curList = new Polyline();
            mf.sim.stepDistance = 0;
            isDrivingRecordedPath = false;
            mf.btnPathGoStop.Image = Properties.Resources.boundaryPlay;
            mf.btnPathRecordStop.Enabled = true;
            mf.btnPickPath.Enabled = true;
            mf.btnResumePath.Enabled = true;
        }

        private void GetDubinsPath(CRecPathPt goal)
        {
            CDubins dubPath = new CDubins(mf.vehicle.minTurningRadius * 1.2);

            // current psition
            vec2 pivotAxlePosRP = mf.pivotAxlePos;

            //bump it forward
            vec2 pt2 = new vec2(pivotAxlePosRP.easting + (Math.Sin(mf.fixHeading) * 3),
                pivotAxlePosRP.northing + (Math.Cos(mf.fixHeading) * 3));

            //get the dubins path vec2 point coordinates of turn
            curList.points = dubPath.GenerateDubins(pt2, mf.fixHeading, new vec2(goal.easting, goal.northing), goal.heading);
            curList.loop = false;
            curList.points.Insert(0, new vec2(pivotAxlePosRP.easting, pivotAxlePosRP.northing));
        }
    }
}