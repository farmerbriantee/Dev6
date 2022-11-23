using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTram
    {
        private readonly FormGPS mf;

        //tram settings
        //public double wheelTrack;
        public double tramWidth, halfWheelTrack;
        public int firstPass, passes;
        public bool isOuter, isTramOnBackBuffer;

        //tramlines
        public List<Polyline2> tramList = new List<Polyline2>();
        public List<List<Polyline2>> tramBoundary = new List<List<Polyline2>>();


        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;
        internal int controlByte;
        private bool ResettramBoundary = false;
        public int outerTramPasses = 1;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.Vehicle.Default.setVehicle_trackWidth * 0.5;
            isTramOnBackBuffer = Properties.Settings.Default.setTram_isTramOnBackBuffer;

            firstPass = Properties.Settings.Default.setTram_Firstpass;
            passes = Properties.Settings.Default.setTram_passes;
            outerTramPasses = Properties.Settings.Default.setTram_OuterTramPasses;

            displayMode = 0;

            isOuter = ((int)(tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0;
        }

        public void DrawTram()
        {
            if (displayMode == 1 || displayMode == 2)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    tramList[i].DrawPolyLine(DrawType.Tram);
                }
            }

            if (displayMode == 1 || displayMode == 3)
            {
                for (int i = 0; i < tramBoundary.Count; i++)
                {
                    for (int j = 0; j < tramBoundary[i].Count; j++)
                    {
                        tramBoundary[i][j].DrawPolyLine(DrawType.Tram);
                    }
                }
            }
        }

        public void BuildTram()
        {
            ResettramBoundary = true;
            for (int j = 0; j < tramList.Count; j++)
                tramList[j].RemoveHandle();
            tramList.Clear();

            if (ResettramBoundary)
            {
                ResettramBoundary = false;
                for (int i = 0; i < tramBoundary.Count; i++)
                {
                    for (int j = 0; j < tramBoundary[i].Count; j++)
                        tramBoundary[i][j].RemoveHandle();
                }
                tramBoundary.Clear();

                if (mf.bnd.bndList.Count > 0)
                {
                    for (int i = 0; i < mf.bnd.bndList.Count; i++)
                    {
                        for (int j = 0; j < outerTramPasses; j++)
                        {
                            double offsetDist = tramWidth * (j + 0.5) * (i == 0 ? 1 : -1);

                            List<Polyline2> Build = mf.bnd.bndList[i].fenceLine.OffsetAndDissolvePolyline<Polyline2>(offsetDist);
                            if (Build.Count == 0) break;

                            for (int k = Build.Count - 1; k >= 0; k--)
                            {
                                if (Build[k].points.Count > 2)
                                    Build[k].points.CalculateRoundedCorner(mf.vehicle.minTurningRadius, Math.Max(mf.vehicle.minTurningRadius, mf.gyd.uturnDistanceFromBoundary), true, 0.0436332);

                                if (Build[k].points.Count > 1)
                                {
                                    BuildTramLeftRightOffset(Build[k], i == 0, j == outerTramPasses - 1);
                                }
                                else Build.RemoveAt(k);
                            }
                            tramBoundary.Add(Build);
                        }
                    }
                }
            }

            if (mf.gyd.currentGuidanceLine != null)
            {
                for (double i = 0; i < passes; i++)
                {
                    List<Polyline2> OffsetList = mf.gyd.currentGuidanceLine.OffsetAndDissolvePolyline<Polyline2>(tramWidth * (firstPass + i), mf.gyd.abLength);

                    for (int s = 0; s < OffsetList.Count; s++)
                    {
                        if (OffsetList[s].points.Count > 2)
                            OffsetList[s].points.CalculateRoundedCorner(mf.vehicle.minTurningRadius, mf.vehicle.minTurningRadius, OffsetList[s].loop, 0.0436332);
                    }

                    if (OffsetList.Count == 0)
                    {
                        continue;
                    }

                    if (tramBoundary.Count > 0)
                    {
                        for (int l = 0; l < tramBoundary.Count; l++)
                        {
                            if (tramBoundary[l].Count > 0 && tramBoundary[l][0].ResetIndexer)
                            {
                                List<Polyline2> Points2 = new List<Polyline2>();
                                for (int n = 0; n < OffsetList.Count; n++)
                                {
                                    if (OffsetList[n].points.Count > 0)
                                        Points2.AddRange(OffsetList[n].ClipPolyLine(tramBoundary[l], tramBoundary[l][0].ResetPoints));
                                }
                                OffsetList.Clear();
                                OffsetList = Points2;
                            }
                        }
                    }

                    for (int s = 0; s < OffsetList.Count; s++)
                    {
                        if (OffsetList[s].points.Count > 1)
                        {
                            BuildTramLeftRightOffset(OffsetList[s]);

                            tramList.Add(OffsetList[s]);
                        }
                    }
                }
            }
        }

        public void BuildTramLeftRightOffset(Polyline2 Build, bool outerBound = false, bool lastPass = false)
        {
            List<vec2> Left = Build.OffsetPolyline(halfWheelTrack, true, out int dddd);
            List<vec2> Right = Build.OffsetPolyline(-halfWheelTrack, true, out int eeee);

            if (dddd > 0)
            {
                Left.RemoveRange(0, dddd);
            }
            else if (dddd < 0)
            {
                Left.RemoveRange(Left.Count + dddd, -dddd);
            }
            else if (Left.Count > 2)
                Left.Add(Left[0]);

            if (eeee > 0)
            {
                Right.RemoveRange(0, eeee);
            }
            else if (eeee < 0)
            {
                Right.RemoveRange(Right.Count + eeee, -eeee);
            }
            else if (Right.Count > 2)
                Right.Add(Right[0]);

            if (Left.Count > 1)
            {
                if (Build.VBO == 0) Build.VBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, Build.VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Left.Count * 16), Left.ToArray(), BufferUsageHint.StaticDraw);

                Build.VBOCount = Left.Count;
            }
            if (Right.Count > 1)
            {
                if (Build.EBO == 0) Build.EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, Build.EBO);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Right.Count * 16), Right.ToArray(), BufferUsageHint.StaticDraw);

                Build.EBOCount = Right.Count;
            }

            Build.ResetPoints = outerBound;
            Build.ResetIndexer = lastPass;
        }
    }
}