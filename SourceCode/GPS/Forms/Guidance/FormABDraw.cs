using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABDraw : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private int start = -1, end = -1;

        private bool isDrawSections = false;
        
        public FormABDraw(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            lblCmInch.Text = mf.unitsInCm;

            nudDistance.Controls[0].Enabled = false;

            if (!mf.isMetric)
            {
                nudDistance.Maximum = (int)(nudDistance.Maximum / 2.54M);
                nudDistance.Minimum = (int)(nudDistance.Minimum / 2.54M);
            }
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            nudDistance.Value = (decimal)Math.Round(mf.tool.toolWidth * mf.m2InchOrCm * 0.5, 0);
            label6.Text = Math.Round(mf.tool.toolWidth * mf.m2InchOrCm, 0).ToString();
            FixLabelsABLine();
            FixLabelsCurve();

            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mf.gyd.numABLineSelected > 0)
            {
                mf.gyd.refPoint1 = mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin;
                mf.gyd.abHeading = mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading;
                mf.gyd.SetABLineByHeading();

                if (mf.gyd.isBtnABLineOn)
                {
                    mf.gyd.isABLineSet = true;
                    mf.gyd.isABLineLoaded = true;
                }
                else
                {
                    mf.gyd.isABLineSet = false;
                }
            }
            else
            {
                mf.gyd.DeleteAB();
                mf.gyd.isABLineSet = false;
                mf.gyd.isABLineLoaded = false;
            }

            mf.FileSaveABLines();


            //curve
            if (mf.gyd.numCurveLineSelected > 0)
            {
                int idx = mf.gyd.numCurveLineSelected - 1;
                mf.gyd.aveLineHeading = mf.gyd.curveArr[idx].aveHeading;
                mf.gyd.refList?.Clear();
                foreach (vec3 v in mf.gyd.curveArr[idx].curvePts) mf.gyd.refList.Add(v);
                mf.gyd.isCurveSet = true;
            }
            else
            {
                mf.gyd.refList?.Clear();
                mf.gyd.isCurveSet = false;
            }

            mf.FileSaveCurveLines();

            if (mf.gyd.isBtnABLineOn)
            {
                if (mf.gyd.numABLineSelected == 0)
                {
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    mf.gyd.isABLineSet = false;
                    mf.gyd.isABLineLoaded = false;
                    mf.btnABLine.Image = Properties.Resources.ABLineOff;
                    mf.gyd.isBtnABLineOn = false;
                }
            }

            if (mf.gyd.isBtnCurveOn)
            {
                if (mf.gyd.numCurveLineSelected == 0)
                {
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    mf.gyd.isCurveSet = false;
                    mf.gyd.refList?.Clear();
                    mf.gyd.isBtnCurveOn = false;
                    mf.btnCurve.Image = Properties.Resources.CurveOff;
                }
            }
        }

        private void FixLabelsCurve()
        {
            lblNumCu.Text = mf.gyd.numCurveLines.ToString();
            lblCurveSelected.Text = mf.gyd.numCurveLineSelected.ToString();

            if (mf.gyd.numCurveLineSelected > 0)
            {
                tboxNameCurve.Text = mf.gyd.curveArr[mf.gyd.numCurveLineSelected - 1].Name;
                tboxNameCurve.Enabled = true;
            }
            else
            {
                tboxNameCurve.Text = "***";
                tboxNameCurve.Enabled = false;
            }
        }

        private void FixLabelsABLine()
        {
            lblNumAB.Text = mf.gyd.numABLines.ToString();
            lblABSelected.Text = mf.gyd.numABLineSelected.ToString();

            if (mf.gyd.numABLineSelected > 0)
            {
                tboxNameLine.Text = mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].Name;
                tboxNameLine.Enabled = true;
            }
            else
            {
                tboxNameLine.Text = "***";
                tboxNameLine.Enabled = false;
            }
        }

        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            if (mf.gyd.numCurveLines > 0)
            {
                mf.gyd.numCurveLineSelected++;
                if (mf.gyd.numCurveLineSelected > mf.gyd.numCurveLines) mf.gyd.numCurveLineSelected = 1;
            }
            else
            {
                mf.gyd.numCurveLineSelected = 0;
            }

            FixLabelsCurve();
        }

        private void btnSelectABLine_Click(object sender, EventArgs e)
        {
            if (mf.gyd.numABLines > 0)
            {
                mf.gyd.numABLineSelected++;
                if (mf.gyd.numABLineSelected > mf.gyd.numABLines) mf.gyd.numABLineSelected = 1;
            }
            else
            {
                mf.gyd.numABLineSelected = 0;
            }

            FixLabelsABLine();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = end = -1;

            btnCancelTouch.Enabled = false;
            btnExit.Focus();
        }

        private void nudDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSelectABLine.Focus();

        }

        private void btnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (mf.gyd.curveArr.Count > 0 && mf.gyd.numCurveLineSelected > 0)
            {
                mf.gyd.curveArr.RemoveAt(mf.gyd.numCurveLineSelected - 1);
                mf.gyd.numCurveLines--;

            }

            if (mf.gyd.numCurveLines > 0) mf.gyd.numCurveLineSelected = 1;
            else mf.gyd.numCurveLineSelected = 0;

            FixLabelsCurve();
        }

        private void btnDeleteABLine_Click(object sender, EventArgs e)
        {
            if (mf.gyd.lineArr.Count > 0 && mf.gyd.numABLineSelected > 0)
            {
                mf.gyd.lineArr.RemoveAt(mf.gyd.numABLineSelected - 1);
                mf.gyd.numABLines--;
                mf.gyd.numABLineSelected--;
            }

            if (mf.gyd.numABLines > 0) mf.gyd.numABLineSelected = 1;
            else mf.gyd.numABLineSelected = 0;

            FixLabelsABLine();
        }

        private void btnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        private void tboxNameCurve_Leave(object sender, EventArgs e)
        {
            if (mf.gyd.numCurveLineSelected > 0)
                mf.gyd.curveArr[mf.gyd.numCurveLineSelected - 1].Name = tboxNameCurve.Text.Trim();
        }

        private void tboxNameLine_Leave(object sender, EventArgs e)
        {
            if (mf.gyd.numABLineSelected > 0)
                mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].Name = tboxNameLine.Text.Trim();
        }

        private void btnFlipOffset_Click(object sender, EventArgs e)
        {
            nudDistance.Value *= -1;
        }

        private void tboxNameCurve_Enter(object sender, EventArgs e)
        {
            if (mf.gyd.curveArr[mf.gyd.numCurveLineSelected - 1].Name == "Boundary Curve")
            {
                btnExit.Focus();
                return;
            }

            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                if (mf.gyd.numCurveLineSelected > 0)
                    mf.gyd.curveArr[mf.gyd.numCurveLineSelected - 1].Name = tboxNameCurve.Text.Trim();
                btnExit.Focus();
            }
        }

        private void tboxNameLine_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                if (mf.gyd.numABLineSelected > 0)
                    mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].Name = tboxNameLine.Text.Trim();
                btnExit.Focus();
            }
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {

            btnCancelTouch.Enabled = true;
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;


            Point pt = oglSelf.PointToClient(Cursor.Position);

            //Convert to Origin in the center of window, 800 pixels
            vec3 plotPt = new vec3
            {
                //convert screen coordinates to field coordinates
                easting = (pt.X - 350) * mf.maxFieldDistance / 632.0,
                northing = (700 - pt.Y - 350) * mf.maxFieldDistance / 632.0,
                heading = 0
            };

            plotPt.easting += mf.fieldCenterX;
            plotPt.northing += mf.fieldCenterY;



            double minDistA = double.MaxValue;
            int A = -1;
            //find the closest 2 points to current fix
            for (int t = 0; t < mf.bnd.bndList[0].fenceLine.points.Count; t++)
            {
                double dist = ((plotPt.easting - mf.bnd.bndList[0].fenceLine.points[t].easting) * (plotPt.easting - mf.bnd.bndList[0].fenceLine.points[t].easting))
                                + ((plotPt.northing - mf.bnd.bndList[0].fenceLine.points[t].northing) * (plotPt.northing - mf.bnd.bndList[0].fenceLine.points[t].northing));

                if (dist < minDistA)
                {
                    minDistA = dist;
                    A = t;
                }
            }

            if (start == -1)
            {
                start = A;
                end = -1;
            }
            else
            {
                end = A;

                if (((mf.bnd.bndList[0].fenceLine.points.Count - end + start) % mf.bnd.bndList[0].fenceLine.points.Count) < ((mf.bnd.bndList[0].fenceLine.points.Count - start + end) % mf.bnd.bndList[0].fenceLine.points.Count)) { int index = start; start = end; end = index; }

                btnMakeABLine.Enabled = true;
                btnMakeCurve.Enabled = true;
            }
        }

        private void btnMakeBoundaryCurve_Click(object sender, EventArgs e)
        {
            //count the points from the boundary
            mf.gyd.refList?.Clear();

            //outside point
            double moveDist = (double)nudDistance.Value * mf.inchOrCm2m;

            Polyline New = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(moveDist, true, -1, -1, true);

            //make the boundary tram outer array
            for (int i = 0; i < New.points.Count; i++)
            {
                mf.gyd.refList.Add(new vec3(New.points[i].easting, New.points[i].northing, 0));
            }

            btnCancelTouch.Enabled = false;

            int cnt = mf.gyd.refList.Count;
            if (cnt > 3)
            {
                //make sure distance isn't too big between points on Turn
                for (int i = 0; i < cnt - 1; i++)
                {
                    int j = i + 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(mf.gyd.refList[i], mf.gyd.refList[j]);
                    if (distance > 1.2)
                    {
                        vec3 pointB = new vec3((mf.gyd.refList[i].easting + mf.gyd.refList[j].easting) / 2.0,
                            (mf.gyd.refList[i].northing + mf.gyd.refList[j].northing) / 2.0,
                            mf.gyd.refList[i].heading);

                        mf.gyd.refList.Insert(j, pointB);
                        cnt = mf.gyd.refList.Count;
                        i = -1;
                    }
                }
                //who knows which way it actually goes
                mf.gyd.CalculateTurnHeadings();

                mf.gyd.isCurveSet = true;

                mf.gyd.aveLineHeading = 0;

                //mf.curve.SmoothAB(4);
                //mf.curve.CalculateTurnHeadings();

                mf.gyd.isCurveSet = true;

                //double offset = ((double)nudDistance.Value) / 200.0;

                mf.gyd.curveArr.Add(new CCurveLines());
                mf.gyd.numCurveLines = mf.gyd.curveArr.Count;
                mf.gyd.numCurveLineSelected = mf.gyd.numCurveLines;

                //array number is 1 less since it starts at zero
                int idx = mf.gyd.curveArr.Count - 1;

                //create a name
                mf.gyd.curveArr[idx].Name = "Boundary Curve";

                mf.gyd.curveArr[idx].aveHeading = mf.gyd.aveLineHeading;

                //write out the Curve Points
                foreach (vec3 item in mf.gyd.refList)
                {
                    mf.gyd.curveArr[idx].curvePts.Add(item);
                }

                mf.FileSaveCurveLines();

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = end = -1;

                FixLabelsCurve();
            }
            else
            {
                mf.gyd.isCurveSet = false;
                mf.gyd.refList?.Clear();
            }
            btnExit.Focus();
        }

        private void BtnMakeCurve_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            double moveDist = (double)nudDistance.Value * mf.inchOrCm2m;

            mf.gyd.refList?.Clear();

            Polyline New = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(moveDist, false, start, end, false);

            for (int i = 0; i < New.points.Count; i++)
            {
                mf.gyd.refList.Add(new vec3(New.points[i].easting, New.points[i].northing, 0));
            }

            int cnt = mf.gyd.refList.Count;
            if (cnt > 3)
            {
                //make sure distance isn't too big between points on Turn
                for (int i = 0; i < cnt - 1; i++)
                {
                    int j = i + 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(mf.gyd.refList[i], mf.gyd.refList[j]);
                    if (distance > 1.6)
                    {
                        vec3 pointB = new vec3((mf.gyd.refList[i].easting + mf.gyd.refList[j].easting) / 2.0,
                            (mf.gyd.refList[i].northing + mf.gyd.refList[j].northing) / 2.0,
                            mf.gyd.refList[i].heading);

                        mf.gyd.refList.Insert(j, pointB);
                        cnt = mf.gyd.refList.Count;
                        i--;
                    }
                }

                //who knows which way it actually goes
                mf.gyd.CalculateTurnHeadings();

                //calculate average heading of line
                double x = 0, y = 0;
                mf.gyd.isCurveSet = true;

                foreach (vec3 pt in mf.gyd.refList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.gyd.refList.Count;
                y /= mf.gyd.refList.Count;
                mf.gyd.aveLineHeading = Math.Atan2(y, x);
                if (mf.gyd.aveLineHeading < 0) mf.gyd.aveLineHeading += glm.twoPI;

                //build the tail extensions
                mf.gyd.AddFirstLastPoints();
                mf.gyd.SmoothAB(4);
                mf.gyd.CalculateTurnHeadings();

                mf.gyd.isCurveSet = true;

                mf.gyd.curveArr.Add(new CCurveLines());
                mf.gyd.numCurveLines = mf.gyd.curveArr.Count;
                mf.gyd.numCurveLineSelected = mf.gyd.numCurveLines;

                //array number is 1 less since it starts at zero
                int idx = mf.gyd.curveArr.Count - 1;

                //create a name
                mf.gyd.curveArr[idx].Name = (Math.Round(glm.toDegrees(mf.gyd.aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture)
                     + "\u00B0" + mf.FindDirection(mf.gyd.aveLineHeading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                mf.gyd.curveArr[idx].aveHeading = mf.gyd.aveLineHeading;

                //write out the Curve Points
                foreach (vec3 item in mf.gyd.refList)
                {
                    mf.gyd.curveArr[idx].curvePts.Add(item);
                }

                mf.FileSaveCurveLines();

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = end = -1;

                FixLabelsCurve();
            }
            else
            {
                mf.gyd.isCurveSet = false;
                mf.gyd.refList?.Clear();
            }
            btnExit.Focus();
        }


        private void BtnMakeABLine_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            //calculate the AB Heading

            double abHead = Math.Atan2(mf.bnd.bndList[0].fenceLine.points[end].easting - mf.bnd.bndList[0].fenceLine.points[start].easting,
                mf.bnd.bndList[0].fenceLine.points[end].northing - mf.bnd.bndList[0].fenceLine.points[start].northing);
            if (abHead < 0) abHead += glm.twoPI;

            double offset = ((double)nudDistance.Value * mf.inchOrCm2m);

            double headingCalc = abHead + glm.PIBy2;

            mf.gyd.lineArr.Add(new CABLines());
            mf.gyd.numABLines = mf.gyd.lineArr.Count;
            mf.gyd.numABLineSelected = mf.gyd.numABLines;

            int idx = mf.gyd.numABLines - 1;

            mf.gyd.lineArr[idx].heading = abHead;
            //calculate the new points for the reference line and points
            mf.gyd.lineArr[idx].origin.easting = (Math.Sin(headingCalc) * (offset)) + mf.bnd.bndList[0].fenceLine.points[start].easting;
            mf.gyd.lineArr[idx].origin.northing = (Math.Cos(headingCalc) * (offset)) + mf.bnd.bndList[0].fenceLine.points[start].northing;

            //create a name
            mf.gyd.lineArr[idx].Name = (Math.Round(glm.toDegrees(mf.gyd.lineArr[idx].heading), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(mf.gyd.lineArr[idx].heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = end = -1;

            FixLabelsABLine();
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -mf.maxFieldDistance);

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX, -mf.fieldCenterY, 0);

            GL.Color3(1, 1, 1);

            //draw all the boundaries
            mf.bnd.DrawFenceLines();

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.90f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            if (isDrawSections) DrawSections();

            //draw the line building graphics
            if (start != -1)
            {
                GL.Color3(0.65, 0.650, 0.0);
                GL.PointSize(8);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(0.95, 0.950, 0.0);
                if (start != -1) GL.Vertex3(mf.bnd.bndList[0].fenceLine.points[start].easting, mf.bnd.bndList[0].fenceLine.points[start].northing, 0);

                GL.Color3(0.950, 096.0, 0.0);
                if (end != -1) GL.Vertex3(mf.bnd.bndList[0].fenceLine.points[end].easting, mf.bnd.bndList[0].fenceLine.points[end].northing, 0);
                GL.End();
            }
            else //draw the actual built lines
                DrawBuiltLines();

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawBuiltLines()
        {
            int numLines = mf.gyd.lineArr.Count;

            if (numLines > 0)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0707);
                GL.Color3(1.0f, 0.0f, 0.0f);

                for (int i = 0; i < numLines; i++)
                {
                    GL.LineWidth(2);
                    GL.Begin(PrimitiveType.Lines);

                    foreach (CABLines item in mf.gyd.lineArr)
                    {
                        GL.Vertex3(item.origin.easting - (Math.Sin(item.heading) * mf.gyd.abLength), item.origin.northing - (Math.Cos(item.heading) * mf.gyd.abLength), 0);
                        GL.Vertex3(item.origin.easting + (Math.Sin(item.heading) * mf.gyd.abLength), item.origin.northing + (Math.Cos(item.heading) * mf.gyd.abLength), 0);
                    }

                    GL.End();
                }

                GL.Disable(EnableCap.LineStipple);

                if (mf.gyd.numABLineSelected > 0)
                {
                    GL.Color3(1.0f, 0.0f, 0.0f);

                    GL.LineWidth(4);
                    GL.Begin(PrimitiveType.Lines);

                    GL.Vertex3(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin.easting - (Math.Sin(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading) * mf.gyd.abLength),
                        mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin.northing - (Math.Cos(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading) * mf.gyd.abLength), 0);
                    GL.Vertex3(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin.easting + (Math.Sin(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading) * mf.gyd.abLength),
                        mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].origin.northing + (Math.Cos(mf.gyd.lineArr[mf.gyd.numABLineSelected - 1].heading) * mf.gyd.abLength), 0);

                    GL.End();
                }
            }

            int numCurv = mf.gyd.curveArr.Count;

            if (numCurv > 0)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x7070);

                for (int i = 0; i < numCurv; i++)
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (vec3 item in mf.gyd.curveArr[i].curvePts)
                    {
                        GL.Vertex3(item.easting, item.northing, 0);
                    }
                    GL.End();
                }

                GL.Disable(EnableCap.LineStipple);

                if (mf.gyd.numCurveLineSelected > 0)
                {
                    GL.LineWidth(4);
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (vec3 item in mf.gyd.curveArr[mf.gyd.numCurveLineSelected - 1].curvePts)
                    {
                        GL.Vertex3(item.easting, item.northing, 0);
                    }
                    GL.End();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();

            bool isBounCurve = false;
            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].Name == "Boundary Curve") isBounCurve = true;
            }

            if (isBounCurve) btnMakeBoundaryCurve.Enabled = false;
            else btnMakeBoundaryCurve.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void oglSelf_Resize(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.23122f, 0.2318f, 0.2315f, 1.0f);
        }

        private void DrawSections()
        {
            int cnt, step, patchCount;
            int mipmap = 8;

            GL.Color3(0.0, 0.0, 0.352);

            //draw patches j= # of sections
            for (int j = 0; j < mf.tool.numSuperSection; j++)
            {
                //every time the section turns off and on is a new patch
                patchCount = mf.section[j].patchList.Count;

                if (patchCount > 0)
                {
                    //for every new chunk of patch
                    foreach (System.Collections.Generic.List<vec3> triList in mf.section[j].patchList)
                    {
                        //draw the triangle in each triangle strip
                        GL.Begin(PrimitiveType.TriangleStrip);
                        cnt = triList.Count;

                        //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                        if (cnt >= (mipmap))
                        {
                            step = mipmap;
                            for (int i = 1; i < cnt; i += step)
                            {
                                GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;
                                GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;

                                //too small to mipmap it
                                if (cnt - i <= (mipmap + 2))
                                    step = 0;
                            }
                        }

                        else { for (int i = 1; i < cnt; i++) GL.Vertex3(triList[i].easting, triList[i].northing, 0); }
                        GL.End();

                    }
                }
            } //end of section patches
        }

        #region Help
        private void btnCancelTouch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnCancelTouch, gStr.gsHelp);
        }

        private void nudDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_nudDistance, gStr.gsHelp);
        }

        private void btnFlipOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnFlipOffset, gStr.gsHelp);
        }

        private void btnMakeBoundaryCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnMakeBoundaryCurve, gStr.gsHelp);
        }

        private void btnMakeCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnMakeCurve, gStr.gsHelp);
        }

        private void btnSelectCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnSelectCurve, gStr.gsHelp);
        }

        private void btnDeleteCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnDeleteCurve, gStr.gsHelp);
        }

        private void btnMakeABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnMakeABLine, gStr.gsHelp);
        }

        private void btnSelectABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnSelectABLine, gStr.gsHelp);
        }

        private void btnDeleteABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnDeleteABLine, gStr.gsHelp);
        }

        private void btnDrawSections_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_btnDrawSections, gStr.gsHelp);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnExit, gStr.gsHelp);
        }

        private void oglSelf_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_oglSelf, gStr.gsHelp);
        }

        private void tboxNameCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_tboxNameLine, gStr.gsHelp);
        }

        private void tboxNameLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_tboxNameLine, gStr.gsHelp);
        }

        #endregion
    }
}