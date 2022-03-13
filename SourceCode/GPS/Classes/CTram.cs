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
        public double tramWidth;
        public double halfWheelTrack;
        public int passes;
        public bool isOuter;

        //tramlines
        public List<Polyline> tramList = new List<Polyline>();
        public List<List<Polyline>> tramBoundary = new List<List<Polyline>>();


        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;
        internal int controlByte;
        private bool ResettramBoundary = false;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.Vehicle.Default.setVehicle_trackWidth * 0.5;

            passes = Properties.Settings.Default.setTram_passes;
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
            int outerTramPasses = 5;
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
                            double Offset = tramWidth * (j + 0.5) * (i == 0 ? 1 : -1);

                            List<Polyline> Build = mf.bnd.bndList[i].fenceLine.OffsetAndDissolvePolyline(true, Offset, true);
                            if (Build.Count == 0) break;

                            for (int k = Build.Count - 1; k >= 0; k--)
                            {
                                if (Build[k].points.Count > 2)
                                    Build[k].points.CalculateRoundedCorner(mf.vehicle.minTurningRadius, Math.Max(mf.vehicle.minTurningRadius, mf.gyd.uturnDistanceFromBoundary), true, 0.0436332);

                                if (Build[k].points.Count > 1)
                                {
                                    List<vec2> Left = Build[k].points.OffsetPolyline(halfWheelTrack, true, 0);
                                    List<vec2> Right = Build[k].points.OffsetPolyline(-halfWheelTrack, true, 0);

                                    if (Left.Count > 1)
                                    {
                                        if (Build[k].BufferPoints == int.MinValue) GL.GenBuffers(1, out Build[k].BufferPoints);
                                        GL.BindBuffer(BufferTarget.ArrayBuffer, Build[k].BufferPoints);
                                        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Left.Count * 16), Left.ToArray(), BufferUsageHint.DynamicDraw);

                                        Build[k].BufferPointsCnt = Left.Count;
                                    }
                                    if (Right.Count > 1)
                                    {
                                        if (Build[k].BufferIndex == int.MinValue) GL.GenBuffers(1, out Build[k].BufferIndex);
                                        GL.BindBuffer(BufferTarget.ArrayBuffer, Build[k].BufferIndex);
                                        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Right.Count * 16), Right.ToArray(), BufferUsageHint.DynamicDraw);

                                        Build[k].BufferIndexCnt = Right.Count;
                                    }

                                    Build[k].ResetPoints = i == 0;
                                    Build[k].ResetIndexer = j == outerTramPasses - 1;
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
                for (double i = 0.0; i < passes; i++)
                {
                    List<vec2> OffsetList2 = new List<vec2>();
                    for (int s = 0; s < mf.gyd.currentGuidanceLine.curvePts.Count; s++)
                    {
                        OffsetList2.Add(new vec2(mf.gyd.currentGuidanceLine.curvePts[s].easting, mf.gyd.currentGuidanceLine.curvePts[s].northing));
                    }

                    List<vec2> OffsetPoints = OffsetList2.OffsetPolyline(tramWidth * i, mf.gyd.currentGuidanceLine.mode.HasFlag(Mode.Boundary), mf.gyd.abLength);
                    List<Polyline> OffsetList = OffsetPoints.DissolvePolyLine(mf.gyd.currentGuidanceLine.mode.HasFlag(Mode.Boundary));

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
                                List<Polyline> Points2 = new List<Polyline>();
                                for (int n = 0; n < OffsetList.Count; n++)
                                {
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
                            List<vec2> Left = OffsetList[s].points.OffsetPolyline(halfWheelTrack, OffsetList[s].loop);
                            List<vec2> Right = OffsetList[s].points.OffsetPolyline(-halfWheelTrack, OffsetList[s].loop);

                            if (OffsetList[s].loop && Left.Count > 2)
                                Left.Add(Left[0]);

                            if (OffsetList[s].loop && Right.Count > 2)
                                Right.Add(Right[0]);

                            if (Left.Count > 1)
                            {
                                if (OffsetList[s].BufferPoints == int.MinValue) GL.GenBuffers(1, out OffsetList[s].BufferPoints);
                                GL.BindBuffer(BufferTarget.ArrayBuffer, OffsetList[s].BufferPoints);
                                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Left.Count * 16), Left.ToArray(), BufferUsageHint.DynamicDraw);

                                OffsetList[s].BufferPointsCnt = Left.Count;
                            }
                            if (Right.Count > 1)
                            {
                                if (OffsetList[s].BufferIndex == int.MinValue) GL.GenBuffers(1, out OffsetList[s].BufferIndex);
                                GL.BindBuffer(BufferTarget.ArrayBuffer, OffsetList[s].BufferIndex);
                                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Right.Count * 16), Right.ToArray(), BufferUsageHint.DynamicDraw);

                                OffsetList[s].BufferIndexCnt = Right.Count;
                            }

                            tramList.Add(OffsetList[s]);
                        }
                    }
                }
            }
        }
    }
}