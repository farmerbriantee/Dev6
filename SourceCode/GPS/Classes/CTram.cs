using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTram
    {
        private readonly FormGPS mf;

        //the list of constants and multiples of the boundary
        public List<vec2> calcList = new List<vec2>();

        //the triangle strip of the outer tram highlight
        public List<vec2> tramBndOuterArr = new List<vec2>();
        public List<vec2> tramBndInnerArr = new List<vec2>();

        //tram settings
        //public double wheelTrack;
        public double tramWidth;
        public double halfWheelTrack;
        public int passes;
        public bool isOuter;

        //tramlines
        public List<vec2> tramArr = new List<vec2>();
        public List<List<vec2>> tramList = new List<List<vec2>>();


        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;
        internal int controlByte;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.Vehicle.Default.setVehicle_trackWidth * 0.5;

            passes = Properties.Settings.Default.setTram_passes;
            displayMode = 0;
            isOuter = Properties.Settings.Default.setTool_isTramOuter;
        }

        public void DrawTram()
        {
            if (displayMode == 1 || displayMode == 2)
            {
                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < tramList[i].Count; h++)
                            GL.Vertex3(tramList[i][h].easting, tramList[i][h].northing, 0);
                        GL.End();
                    }
                }
            }

            if (displayMode == 1 || displayMode == 3)
            {
                if (tramBndOuterArr.Count > 0)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    for (int h = 0; h < tramBndOuterArr.Count; h++) GL.Vertex3(tramBndOuterArr[h].easting, tramBndOuterArr[h].northing, 0);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    for (int h = 0; h < tramBndInnerArr.Count; h++) GL.Vertex3(tramBndInnerArr[h].easting, tramBndInnerArr[h].northing, 0);
                    GL.End();
                }
            }
        }

        public void BuildTram(CGuidanceLine currentGuidanceLine, CGuidanceLine currentGuidanceLine2)
        {
            BuildTramBnd();

            tramList?.Clear();
            tramArr?.Clear();
            List<vec2> tramRef = new List<vec2>();

            bool isBndExist = mf.bnd.bndList.Count != 0;

            double pass = 0.5;
            double hsin = Math.Sin(currentGuidanceLine.curvePts[0].heading);
            double hcos = Math.Cos(currentGuidanceLine.curvePts[0].heading);

            //divide up the AB line into segments
            vec2 P1 = new vec2();
            for (int i = 0; i < 3200; i += 4)
            {
                P1.easting = (hsin * i) + currentGuidanceLine.curvePts[0].easting;
                P1.northing = (hcos * i) + currentGuidanceLine.curvePts[0].northing;
                tramRef.Add(P1);
            }

            //create list of list of points of triangle strip of AB Highlight
            double headingCalc = currentGuidanceLine.curvePts[0].heading + glm.PIBy2;
            hsin = Math.Sin(headingCalc);
            hcos = Math.Cos(headingCalc);

            tramList?.Clear();
            tramArr?.Clear();

            //no boundary starts on first pass
            int cntr = 0;
            if (isBndExist) cntr = 1;

            for (int i = cntr; i < passes; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = (hsin * ((tramWidth * (pass + i)) - halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].easting;
                    P1.northing = (hcos * ((tramWidth * (pass + i)) - halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }

            for (int i = cntr; i < passes; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = (hsin * ((tramWidth * (pass + i)) + halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].easting;
                    P1.northing = (hcos * ((tramWidth * (pass + i)) + halfWheelTrack + mf.tool.halfToolWidth)) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }
        }

        public void BuildTram(CGuidanceLine currentGuidanceLine)
        {
            BuildTramBnd();

            tramList.Clear();
            tramArr.Clear();

            bool isBndExist = mf.bnd.bndList.Count != 0;

            int refCount = currentGuidanceLine.curvePts.Count;

            int cntr = 0;
            if (isBndExist) cntr = 1;

            for (int i = cntr; i <= passes; i++)
            {
                double offset = tramWidth * (i + 0.5) - halfWheelTrack + mf.tool.halfToolWidth;
                double distSqAway = offset * offset - 0.001;

                tramList.Add(tramArr);
                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(currentGuidanceLine.curvePts[j].easting + (Math.Sin(glm.PIBy2 + currentGuidanceLine.curvePts[j].heading) * offset),
                                         currentGuidanceLine.curvePts[j].northing + (Math.Cos(glm.PIBy2 + currentGuidanceLine.curvePts[j].heading) * offset));

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = glm.Distance(point, currentGuidanceLine.curvePts[t]);
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = tramArr.Count > 0 ? glm.Distance(point, tramArr[tramArr.Count - 1]) : 3.0;
                        if (dist > 2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(point))
                            {
                                tramArr.Add(point);
                            }
                        }
                    }
                }
            }

            for (int i = cntr; i <= passes; i++)
            {
                double offset = tramWidth * (i + 0.5) + halfWheelTrack + mf.tool.halfToolWidth;
                double distSqAway = offset * offset - 0.001;

                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);
                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(currentGuidanceLine.curvePts[j].easting + (Math.Sin(glm.PIBy2 + currentGuidanceLine.curvePts[j].heading) * offset),
                                         currentGuidanceLine.curvePts[j].northing + (Math.Cos(glm.PIBy2 + currentGuidanceLine.curvePts[j].heading) * offset));

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = glm.Distance(point, currentGuidanceLine.curvePts[t]);
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = tramArr.Count > 0 ? glm.Distance(point, tramArr[tramArr.Count - 1]) : 3.0;
                        if (dist > 2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLine.points.IsPointInPolygon(point))
                            {
                                tramArr.Add(point);
                            }
                        }
                    }
                }
            }
        }

        public void BuildTramBnd()
        {
            tramBndInnerArr?.Clear();
            tramBndOuterArr?.Clear();

            if (mf.bnd.bndList.Count > 0)
            {
                double distOut = (tramWidth * 0.5) - halfWheelTrack;

                Polyline NewOut = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(distOut, true, -1, -1, true);
                tramBndOuterArr.AddRange(NewOut.points);

                double distIn = (tramWidth * 0.5) + halfWheelTrack;

                Polyline NewIn = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(distIn, true, -1, -1, true);
                tramBndInnerArr.AddRange(NewIn.points);
            }
        }
    }
}