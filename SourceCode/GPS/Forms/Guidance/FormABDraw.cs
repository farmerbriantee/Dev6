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

        private CGuidanceLine selectedCurveLine, selectedABLine;

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

            FixLabelsCurve();

            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.moveDistance = 0;

            if (selectedABLine != null)
                mf.gyd.currentABLine = new CGuidanceLine(selectedABLine);
            else
            {
                mf.gyd.currentABLine = null;

                if (mf.gyd.isBtnABLineOn)
                {
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

                    mf.gyd.isBtnABLineOn = false;
                    mf.btnABLine.Image = Properties.Resources.ABLineOff;
                }
            }
            mf.FileSaveABLines();

            //curve
            if (selectedCurveLine != null)
                mf.gyd.currentCurveLine = new CGuidanceLine(selectedCurveLine);
            else
            {
                mf.gyd.currentCurveLine = null;

                if (mf.gyd.isBtnCurveOn)
                {
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

                    mf.gyd.isBtnCurveOn = false;
                    mf.btnCurve.Image = Properties.Resources.CurveOff;
                }
            }
            mf.FileSaveCurveLines();
        }

        private void FixLabelsCurve()
        {
            int totalCurves = 0;
            int totalAB = 0;

            bool counting = selectedCurveLine?.mode.HasFlag(Mode.Curve) == true;
            bool counting2 = selectedABLine?.mode.HasFlag(Mode.AB) == true;
            int count = 0;
            int count2 = 0;
            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                {
                    if (counting)
                        count++;
                    totalCurves++;
                    if (mf.gyd.curveArr[i] == selectedCurveLine) counting = false;
                }

                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.AB))
                {
                    if (counting2)
                        count2++;
                    totalAB++;
                    if (mf.gyd.curveArr[i] == selectedABLine) counting2 = false;
                }
            }

            if (count > 0)
            {
                tboxNameCurve.Text = selectedCurveLine.Name.Trim();
                tboxNameCurve.Enabled = true;
                lblCurveSelected.Text = count.ToString();
            }
            else
            {
                tboxNameCurve.Text = "***";
                tboxNameCurve.Enabled = false;
                lblCurveSelected.Text = "0";
            }
            lblNumCu.Text = totalCurves.ToString();

            if (count2 > 0)
            {
                tboxNameLine.Text = selectedABLine.Name.Trim();
                tboxNameLine.Enabled = true;
                lblABSelected.Text = count2.ToString();
            }
            else
            {
                tboxNameLine.Text = "***";
                tboxNameLine.Enabled = false;
                lblABSelected.Text = "0";
            }

            lblNumAB.Text = totalAB.ToString();
        }

        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            bool found = !(selectedCurveLine?.mode.HasFlag(Mode.Curve) == true);
            bool loop = true;
            for (int i = 0; i < mf.gyd.curveArr.Count || loop; i++)
            {
                if (i >= mf.gyd.curveArr.Count)
                {
                    loop = false;
                    i = -1;
                    if (!found) break;
                    else continue;
                }
                if (mf.gyd.curveArr[i] == selectedCurveLine)
                    found = true;
                else if (found && mf.gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                {
                    selectedCurveLine = mf.gyd.curveArr[i];
                    break;
                }
            }
            if (!found)
                selectedCurveLine = null;

            FixLabelsCurve();
        }

        private void btnSelectABLine_Click(object sender, EventArgs e)
        {
            bool found = !(selectedABLine?.mode.HasFlag(Mode.AB) == true);
            bool loop = true;
            for (int i = 0; i < mf.gyd.curveArr.Count || loop; i++)
            {
                if (i >= mf.gyd.curveArr.Count)
                {
                    loop = false;
                    i = -1;
                    if (!found) break;
                    else continue;
                }
                if (mf.gyd.curveArr[i] == selectedABLine)
                    found = true;
                else if (found && mf.gyd.curveArr[i].mode.HasFlag(Mode.AB))
                {
                    selectedABLine = mf.gyd.curveArr[i];
                    break;
                }
            }
            if (!found)
                selectedABLine = null;

            FixLabelsCurve();
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
            if (selectedCurveLine != null)
            {
                if (mf.gyd.currentCurveLine?.Name == selectedCurveLine.Name)
                    mf.gyd.currentCurveLine = null;
                if (mf.gyd.currentGuidanceLine?.Name == selectedCurveLine.Name)
                {
                    mf.gyd.currentGuidanceLine = null;

                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    if (mf.gyd.isBtnCurveOn) mf.btnCurve.PerformClick();
                }
                mf.gyd.curveArr.Remove(selectedCurveLine);

                selectedCurveLine = null;
                mf.FileSaveCurveLines();
            }

            FixLabelsCurve();
        }

        private void btnDeleteABLine_Click(object sender, EventArgs e)
        {
            if (selectedABLine != null)
            {
                if (mf.gyd.currentABLine?.Name == selectedABLine.Name)
                    mf.gyd.currentABLine = null;
                if (mf.gyd.currentGuidanceLine?.Name == selectedABLine.Name)
                {
                    mf.gyd.currentGuidanceLine = mf.gyd.currentABLine = null;

                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    if (mf.gyd.isBtnABLineOn) mf.btnABLine.PerformClick();
                }
                mf.gyd.curveArr.Remove(selectedABLine);

                selectedABLine = null;
                mf.FileSaveABLines();
            }

            FixLabelsCurve();
        }

        private void btnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        private void tboxNameCurve_Leave(object sender, EventArgs e)
        {
            if (selectedCurveLine != null)
                selectedCurveLine.Name = tboxNameCurve.Text.Trim();
        }

        private void tboxNameLine_Leave(object sender, EventArgs e)
        {
            if (selectedABLine != null)
                selectedABLine.Name = tboxNameLine.Text.Trim();
        }

        private void btnFlipOffset_Click(object sender, EventArgs e)
        {
            nudDistance.Value *= -1;
        }

        private void tboxNameCurve_Click(object sender, EventArgs e)
        {
            if (selectedCurveLine != null)
            {
                if (selectedCurveLine.Name == "Boundary Curve")
                {
                    btnExit.Focus();
                    return;
                }

                if (mf.isKeyboardOn)
                {
                    mf.KeyboardToText((TextBox)sender, this);
                    selectedCurveLine.Name = tboxNameCurve.Text.Trim();
                    btnExit.Focus();
                }
            }
        }

        private void tboxNameLine_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                if (selectedABLine != null)
                    selectedABLine.Name = tboxNameLine.Text.Trim();
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
            //outside point
            double moveDist = (double)nudDistance.Value * mf.inchOrCm2m;

            Polyline poly = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(moveDist, true, -1, -1, true);

            btnCancelTouch.Enabled = false;

            if (poly.points.Count > 3)
            {
                CGuidanceLine New = new CGuidanceLine(Mode.Curve | Mode.Boundary);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    New.curvePts.Add(new vec3(poly.points[i].easting, poly.points[i].northing, 0));
                }
                //make sure distance isn't too big between points on Turn
                for (int i = 1; i < New.curvePts.Count; i++)
                {
                    int j = i - 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(New.curvePts[i], New.curvePts[j]);
                    if (distance > 1.6)
                    {
                        vec3 pointB = new vec3((New.curvePts[i].easting + New.curvePts[j].easting) / 2.0,
                            (New.curvePts[i].northing + New.curvePts[j].northing) / 2.0,
                            New.curvePts[i].heading);

                        New.curvePts.Insert(i, pointB);
                        i--;
                    }
                }

                //who knows which way it actually goes
                New.curvePts.CalculateHeadings(true);
                //create a name
                New.Name = "Boundary Curve";

                mf.gyd.curveArr.Add(New);
                selectedCurveLine = New;

                mf.FileSaveCurveLines();

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = end = -1;

                FixLabelsCurve();
            }
            btnExit.Focus();
        }

        private void BtnMakeCurve_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            double moveDist = (double)nudDistance.Value * mf.inchOrCm2m;

            Polyline poly = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline(moveDist, false, start, end, false);

            if (poly.points.Count > 3)
            {
                CGuidanceLine New = new CGuidanceLine(Mode.Curve);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    New.curvePts.Add(new vec3(poly.points[i].easting, poly.points[i].northing, 0));
                }

                //make sure distance isn't too big between points on Turn
                for (int i = 1; i < New.curvePts.Count; i++)
                {
                    int j = i - 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(New.curvePts[i], New.curvePts[j]);
                    if (distance > 1.6)
                    {
                        vec3 pointB = new vec3((New.curvePts[i].easting + New.curvePts[j].easting) / 2.0,
                            (New.curvePts[i].northing + New.curvePts[j].northing) / 2.0,
                            New.curvePts[i].heading);

                        New.curvePts.Insert(i, pointB);
                        i--;
                    }
                }

                //who knows which way it actually goes
                New.curvePts.CalculateHeadings(false);

                //calculate average heading of line
                double x = 0, y = 0;

                foreach (vec3 pt in New.curvePts)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= New.curvePts.Count;
                y /= New.curvePts.Count;
                double aveLineHeading = Math.Atan2(y, x);
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                //create a name
                New.Name = (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture)
                     + "\u00B0" + mf.FindDirection(aveLineHeading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                //build the tail extensions
                AddFirstLastPoints(New);
                New.curvePts.CalculateHeadings(false);

                mf.gyd.curveArr.Add(New);
                selectedCurveLine = New;

                mf.FileSaveCurveLines();

                //update the arrays
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = end = -1;

                FixLabelsCurve();
            }
            btnExit.Focus();
        }


        private void BtnMakeABLine_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            CGuidanceLine New = new CGuidanceLine(Mode.AB);

            double abHead = Math.Atan2(mf.bnd.bndList[0].fenceLine.points[end].easting - mf.bnd.bndList[0].fenceLine.points[start].easting,
                mf.bnd.bndList[0].fenceLine.points[end].northing - mf.bnd.bndList[0].fenceLine.points[start].northing);
            if (abHead < 0) abHead += glm.twoPI;

            double offset = (double)nudDistance.Value * mf.inchOrCm2m;

            double headingCalc = abHead + glm.PIBy2;

            //calculate the new points for the reference line and points
            New.curvePts.Add(new vec3((Math.Sin(headingCalc) * offset) + mf.bnd.bndList[0].fenceLine.points[start].easting, (Math.Cos(headingCalc) * offset) + mf.bnd.bndList[0].fenceLine.points[start].northing, abHead));
            New.curvePts.Add(new vec3(New.curvePts[0].easting + Math.Sin(abHead), New.curvePts[0].northing + Math.Cos(abHead), abHead));

            //create a name
            New.Name = (Math.Round(glm.toDegrees(abHead), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(abHead) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            mf.gyd.curveArr.Add(New);
            selectedABLine = New;

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            start = end = -1;

            FixLabelsCurve();
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
            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i] == selectedABLine || mf.gyd.curveArr[i] == selectedCurveLine)
                    GL.Disable(EnableCap.LineStipple);
                else
                {
                    GL.LineStipple(1, 0x7070);
                    GL.LineStipple(1, 0x7070);
                }

                GL.LineWidth(2);
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.AB))
                    GL.Color3(1.0f, 0.0f, 0.0f);
                else
                    GL.Color3(0.0f, 1.0f, 0.0f);

                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.Boundary))
                    GL.Begin(PrimitiveType.LineLoop);
                else
                    GL.Begin(PrimitiveType.LineStrip);

                for (int j = 0; j < mf.gyd.curveArr[i].curvePts.Count; j++)
                {
                    vec3 item = mf.gyd.curveArr[i].curvePts[j];
                    if (j == 0)
                        GL.Vertex3(item.easting - (Math.Sin(item.heading) * mf.gyd.abLength), item.northing - (Math.Cos(item.heading) * mf.gyd.abLength), 0);

                    GL.Vertex3(item.easting, item.northing, 0);

                    if (j == mf.gyd.curveArr[i].curvePts.Count - 1)
                        GL.Vertex3(item.easting + (Math.Sin(item.heading) * mf.gyd.abLength), item.northing + (Math.Cos(item.heading) * mf.gyd.abLength), 0);
                }

                GL.End();
            }

            GL.Disable(EnableCap.LineStipple);
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

        //add extensons
        public void AddFirstLastPoints(CGuidanceLine refList)
        {
            int ptCnt = refList.curvePts.Count - 1;
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(refList.curvePts[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                refList.curvePts.Add(pt);
            }

            //and the beginning
            vec3 start = new vec3(refList.curvePts[0]);
            for (int i = 1; i < 200; i++)
            {
                vec3 pt = new vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                refList.curvePts.Insert(0, pt);
            }
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