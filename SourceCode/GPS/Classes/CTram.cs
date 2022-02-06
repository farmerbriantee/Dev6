using OpenTK.Graphics.OpenGL;
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
            if (mf.camera.camSetDistance > -250) GL.LineWidth(4);
            else GL.LineWidth(2);

            GL.Color4(0.30f, 0.93692f, 0.7520f, 0.3);

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2)
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

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3)
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