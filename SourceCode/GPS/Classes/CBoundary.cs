using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundary
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public List<CBoundaryList> bndList = new List<CBoundaryList>();

        public bool isHeadlandOn, isSectionControlledByHeadland;

        public List<vec2> bndBeingMadePts = new List<vec2>(128);

        public double createBndOffset;
        public bool isBndBeingMade;

        public bool isDrawRightSide = true, isOkToAddPoints = false;

        //all the section area added up;
        public double workedAreaTotal;

        //just a cumulative tally based on distance and eq width.
        public double workedAreaTotalUser;

        //accumulated user distance
        public double distanceUser;

        public double overlapPercent = 0;

        //Outside area minus inner boundaries areas (m)
        public double areaBoundaryOuterLessInner;

        //used for overlap calcs - total done minus overlap
        public double actualAreaCovered;

        //Inner area of outer boundary(m)
        public double areaOuterBoundary;

        public CBoundary(FormGPS _f)
        {
            mf = _f;
            isHeadlandOn = false;
            isSectionControlledByHeadland = Properties.Settings.Default.setHeadland_isSectionControlled;

            workedAreaTotal = 0;
            workedAreaTotalUser = 0;
        }

        public void RemoveHandles(int idx)
        {
            for (int i = 0; i < bndList[idx].hdLine.Count; i++)
            {
                bndList[idx].hdLine[i].RemoveHandle();
            }

            bndList[idx].fenceLine.RemoveHandle();
            bndList[idx].turnLine.RemoveHandle();
            mf.bnd.bndList.RemoveAt(idx);
        }

        public void DrawFenceLines()
        {
            if (!mf.mc.isOutOfBounds)
            {
                GL.Color3(0.95f, 0.75f, 0.50f);
                GL.LineWidth(mf.gyd.lineWidth);
            }
            else
            {
                GL.LineWidth(mf.gyd.lineWidth * 3);
                GL.Color3(0.95f, 0.25f, 0.250f);
            }

            for (int i = 0; i < bndList.Count; i++)
            {
                bndList[i].fenceLine.DrawPolyLine(DrawType.LineLoop);
            }
        }

        public bool IsPointInsideFenceArea(vec2 testPoint)
        {
            //first where are we, must be inside outer and outside of inner geofence non drive thru turn borders
            if (bndList.Count > 0 && bndList[0].fenceLine.points.IsPointInPolygon(testPoint))
            {
                for (int i = 1; i < bndList.Count; i++)
                {
                    //make sure not inside a non drivethru boundary
                    if (bndList[i].isDriveThru) continue;
                    if (bndList[i].fenceLine.points.IsPointInPolygon(testPoint))
                    {
                        return false;
                    }
                }
                //we are safely inside outer, outside inner boundaries
                return true;
            }
            return false;
        }

        public int IsPointInsideTurnArea(vec2 pt)
        {
            if (bndList.Count > 0 && bndList[0].turnLine.points.IsPointInPolygon(pt))
            {
                for (int i = 1; i < bndList.Count; i++)
                {
                    if (bndList[i].isDriveThru) continue;
                    if (bndList[i].turnLine.points.IsPointInPolygon(pt))
                    {
                        return i;
                    }
                }
                return 0;
            }
            return -1;
        }

        public void BuildTurnLines()
        {
            for (int j = 0; j < bndList.Count; j++)
            {
                bndList[j].turnLine.RemoveHandle();
                bndList[j].turnLine = bndList[j].fenceLine.OffsetAndDissolvePolyline(j == 0 ? mf.gyd.uturnDistanceFromBoundary : -mf.gyd.uturnDistanceFromBoundary, 0, -1, -1, true);
            }
        }

        public string WorkedAreaRemainPercentage
        {
            get
            {
                if (areaBoundaryOuterLessInner > 10)
                {
                    return ((areaBoundaryOuterLessInner - workedAreaTotal) * 100 / areaBoundaryOuterLessInner).ToString("0.0") + "%";
                }
                else
                {
                    return "0.00%";
                }
            }
        }

        public string BoundaryArea
        {
            get
            {
                double userBoundaryArea = areaBoundaryOuterLessInner * glm.m2ToUser;
                return userBoundaryArea.ToString(userBoundaryArea < 9.995 ? "0.00" : "0.0") + glm.unitsHaAc;
            }
        }

        public string WorkedAreaRemain
        {
            get
            {
                return ((areaBoundaryOuterLessInner - workedAreaTotal) * glm.m2ToUser).ToString("0.00") + glm.unitsHaAc;
            }
        }

        public string WorkedArea
        {
            get
            {
                double userWorkedArea = workedAreaTotal * glm.m2ToUser;
                return userWorkedArea.ToString(userWorkedArea < 9.995 ? "0.00" : "0.0") + glm.unitsHaAc;
            }
        }


        public string TimeTillFinished
        {
            get
            {
                if (mf.mc.avgSpeed > 2 && mf.tool.toolWidth > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromHours(((areaBoundaryOuterLessInner - workedAreaTotal) * 0.0001
                        / (mf.tool.toolWidth * mf.mc.avgSpeed * 0.1)));
                    return timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00");
                }
                else return "\u221E Hrs";
            }
        }

        public void UpdateFieldBoundaryGUIAreas()
        {
            if (mf.bnd.bndList.Count > 0)
            {
                areaOuterBoundary = mf.bnd.bndList[0].area;
                areaBoundaryOuterLessInner = areaOuterBoundary;

                for (int i = 1; i < mf.bnd.bndList.Count; i++)
                {
                    areaBoundaryOuterLessInner -= mf.bnd.bndList[i].area;
                }
            }
            else
            {
                areaOuterBoundary = 0;
                areaBoundaryOuterLessInner = 0;
            }
        }
    }
}