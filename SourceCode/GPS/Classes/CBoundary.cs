using OpenTK.Graphics.OpenGL;
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


        public CBoundary(FormGPS _f)
        {
            mf = _f;
            isHeadlandOn = false;
            isSectionControlledByHeadland = Properties.Settings.Default.setHeadland_isSectionControlled;
        }

        public void RemoveHandles(int idx)
        {
            bndList[idx].hdLine.RemoveHandle();
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
                bndList[j].turnLine = bndList[j].fenceLine.OffsetAndDissolvePolyline(j == 0 ? mf.gyd.uturnDistanceFromBoundary : -mf.gyd.uturnDistanceFromBoundary, true, -1, -1, true);
            }
        }
    }
}