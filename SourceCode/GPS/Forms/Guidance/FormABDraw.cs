﻿using OpenTK;
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

        private double currentFieldCenterX, currentFieldCenterY, currentDist;

        private CGuidanceLine selectedCurveLine, selectedABLine;

        public FormABDraw(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            lblCmInch.Text = glm.unitsInCm;

            nudDistance.Controls[0].Enabled = false;

            if (!glm.isMetric)
            {
                nudDistance.Maximum = (int)(nudDistance.Maximum / 2.54M);
                nudDistance.Minimum = (int)(nudDistance.Minimum / 2.54M);
            }

            currentFieldCenterX = mf.fieldCenterX;
            currentFieldCenterY = mf.fieldCenterY;
            currentDist = mf.maxFieldDistance;

            MouseWheel += ZoomByMouseWheel;
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            nudDistance.Value = (decimal)Math.Round(mf.tool.toolWidth * glm.mToUser * 0.5, 0);
            label6.Text = Math.Round(mf.tool.toolWidth * glm.mToUser, 0).ToString();

            FixLabelsCurve();

            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;

            bool isBounCurve = false;
            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].Name == "Boundary Curve") isBounCurve = true;
            }

            if (isBounCurve) btnMakeBoundaryCurve.Enabled = false;
            else btnMakeBoundaryCurve.Enabled = true;
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.moveDistance = 0;

            if (selectedABLine != null)
                mf.gyd.currentABLine = new CGuidanceLine(selectedABLine);
            else
            {
                mf.gyd.currentABLine = null;

                if (mf.gyd.CurrentGMode == Mode.AB)
                    mf.SetGuidanceMode(Mode.None);
            }
            mf.FileSaveABLines();

            //curve
            if (selectedCurveLine != null)
                mf.gyd.currentCurveLine = new CGuidanceLine(selectedCurveLine);
            else
            {
                mf.gyd.currentCurveLine = null;

                if (mf.gyd.CurrentGMode == Mode.Curve)
                    mf.SetGuidanceMode(Mode.None);
            }
            mf.FileSaveCurveLines();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                currentFieldCenterY += 10;
                if (currentFieldCenterY > mf.maxFieldY)
                    currentFieldCenterY = mf.maxFieldY;

                oglSelf.Refresh();
                return true;
            }
            else if (keyData == Keys.Down)
            {
                currentFieldCenterY -= 10;
                if (currentFieldCenterY < mf.minFieldY)
                    currentFieldCenterY = mf.minFieldY;

                oglSelf.Refresh();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                currentFieldCenterX -= 10;
                if (currentFieldCenterX < mf.minFieldX)
                    currentFieldCenterX = mf.minFieldX;

                oglSelf.Refresh();
                return true;
            }
            else if (keyData == Keys.Right)
            {
                currentFieldCenterX += 10;
                if (currentFieldCenterX > mf.maxFieldX)
                    currentFieldCenterX = mf.maxFieldX;

                oglSelf.Refresh();
                return true;
            }

            //handle your keys here
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ZoomByMouseWheel(object sender, MouseEventArgs e)
        {
            if (currentDist == mf.maxFieldDistance && e.Delta < 0)
            {
                //currentFieldCenterX = mf.fieldCenterX;
                //currentFieldCenterY = mf.fieldCenterY;

                return;
            }

            vec2 plotPt = new vec2(
                currentFieldCenterY + (e.X - 350) / 700.0 * currentDist,
                currentFieldCenterX + (350 - e.Y) / 700.0 * currentDist
            );

            currentDist -= e.Delta;

            if (currentDist < 50) currentDist = 50;
            if (currentDist > mf.maxFieldDistance)
                currentDist = mf.maxFieldDistance;

            vec2 plotPt2 = new vec2(
                currentFieldCenterY + (e.X - 350) / 700.0 * currentDist,
                currentFieldCenterX + (350 - e.Y) / 700.0 * currentDist
            );

            double easting = plotPt2.easting - plotPt.easting;
            double northing = plotPt2.northing - plotPt.northing;

            currentFieldCenterX -= easting;
            currentFieldCenterY -= northing;

            if (currentFieldCenterY > mf.maxFieldY)
                currentFieldCenterY = mf.maxFieldY;
            else if (currentFieldCenterY < mf.minFieldY)
                currentFieldCenterY = mf.minFieldY;

            if (currentFieldCenterX > mf.maxFieldX)
                currentFieldCenterX = mf.maxFieldX;
            else if (currentFieldCenterX < mf.minFieldX)
                currentFieldCenterX = mf.minFieldX;

            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 mat = Matrix4.CreateOrthographic((float)currentDist, (float)currentDist, -1.0f, 1.0f);
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
            oglSelf.Refresh();
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
                tboxNameCurve.Text = selectedCurveLine?.Name.Trim();
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
                tboxNameLine.Text = selectedABLine?.Name.Trim();
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

            oglSelf.Refresh();
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
                    found = false;
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
                    found = false;
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
            nudDistance.KeypadToNUD();
            btnSelectABLine.Focus();
        }

        private void btnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (selectedCurveLine != null)
            {
                if (selectedCurveLine.Name == "Boundary Curve") btnMakeBoundaryCurve.Enabled = true;

                if (mf.gyd.currentCurveLine?.Name == selectedCurveLine.Name)
                    mf.gyd.currentCurveLine = null;
                if (mf.gyd.currentGuidanceLine?.Name == selectedCurveLine.Name)
                    mf.SetGuidanceMode(Mode.None);

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
                    mf.SetGuidanceMode(Mode.None);

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

                if (tboxNameCurve.KeyboardToText())
                {
                    selectedCurveLine.Name = tboxNameCurve.Text.Trim();
                    btnExit.Focus();
                }
            }
        }

        private void tboxNameLine_Click(object sender, EventArgs e)
        {
            if (tboxNameLine.KeyboardToText())
            {
                if (selectedABLine != null)
                    selectedABLine.Name = tboxNameLine.Text.Trim();
                btnExit.Focus();
            }
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            if (end > -1)
            {
                btnCancelTouch.Enabled = false;
                btnMakeABLine.Enabled = false;
                btnMakeCurve.Enabled = false;
                start = end = -1;
                oglSelf.Refresh();
                return;
            }
            btnCancelTouch.Enabled = true;

            //convert screen coordinates to field coordinates
            vec2 plotPt = new vec2(
                currentFieldCenterX + (e.Location.X - 350) / 700.0 * currentDist,
                currentFieldCenterY + (350 - e.Location.Y) / 700.0 * currentDist
            );

            double minDistA = double.MaxValue;
            int A = -1;
            //find the closest 2 points to current fix
            for (int t = 0; t < mf.bnd.bndList[0].fenceLine.points.Count; t++)
            {
                double dist = glm.Distance(plotPt, mf.bnd.bndList[0].fenceLine.points[t]);
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
            oglSelf.Refresh();
        }

        private void btnMakeBoundaryCurve_Click(object sender, EventArgs e)
        {
            double offsetDist = (double)nudDistance.Value * glm.userToM;

            btnCancelTouch.Enabled = false;
            btnMakeBoundaryCurve.Enabled = false;

            Polyline poly = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline<Polyline>(offsetDist)[0];

            if (poly.points.Count > 3)
            {
                CGuidanceLine New = new CGuidanceLine(Mode.Curve, poly);
                selectedCurveLine = New;
                New.loop = true;
                //create a name
                New.Name = "Boundary Curve";

                mf.gyd.curveArr.Add(New);

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

            double offsetDist = (double)nudDistance.Value * glm.userToM;

            Polyline poly = mf.bnd.bndList[0].fenceLine.OffsetAndDissolvePolyline<Polyline>(offsetDist, 0, start, end, false)[0];

            if (poly.points.Count > 1)
            {
                CGuidanceLine New = new CGuidanceLine(Mode.Curve, poly);

                double aveLineHeading = New.points.CalculateAverageHeadings();
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                //create a name
                string text = (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture)
                     + "\u00B0" + mf.FindDirection(aveLineHeading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                    text += " ";
                New.Name = text;
                
                if (!New.loop) //build the tail extensions
                    New.points.AddFirstLastPoints(200, mf.tool.toolWidth, false);

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

            double offset = (double)nudDistance.Value * glm.userToM;

            double headingCalc = abHead + glm.PIBy2;

            //calculate the new points for the reference line and points
            New.points.Add(new vec2((Math.Sin(headingCalc) * offset) + mf.bnd.bndList[0].fenceLine.points[start].easting, (Math.Cos(headingCalc) * offset) + mf.bnd.bndList[0].fenceLine.points[start].northing));
            New.points.Add(new vec2(New.points[0].easting + Math.Sin(abHead), New.points[0].northing + Math.Cos(abHead)));

            //create a name
            string text = (Math.Round(glm.toDegrees(abHead), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(abHead) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
            while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
            text += " ";
            New.Name = text;

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

            //translate to that spot in the world
            GL.Translate(-currentFieldCenterX, -currentFieldCenterY, 0);

            GL.Color3(1, 1, 1);

            //draw all the boundaries
            mf.bnd.DrawFenceLines();

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.90f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            if (isDrawSections)
            {
                GL.Color3(0.0, 0.0, 0.352);

                foreach (Polyline2 triList in mf.patchList)
                {
                    triList.DrawPolyLine(DrawType.TriangleStrip);
                }
            }

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
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(1, 0x7070);
                }

                GL.LineWidth(2);
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.AB))
                    GL.Color3(1.0f, 0.0f, 0.0f);
                else if (mf.gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                    GL.Color3(0.0f, 1.0f, 0.0f);
                else
                    continue;

                if (mf.gyd.curveArr[i].loop)
                    GL.Begin(PrimitiveType.LineLoop);
                else
                    GL.Begin(PrimitiveType.LineStrip);
                if (mf.gyd.curveArr[i].points.Count > 1)
                {
                    for (int j = 0; j < mf.gyd.curveArr[i].points.Count; j++)
                    {
                        vec2 item = mf.gyd.curveArr[i].points[j];
                        if (j == 0 && !mf.gyd.curveArr[i].loop)
                        {
                            vec2 item2 = mf.gyd.curveArr[i].points[j + 1];
                            double lineHeading = Math.Atan2(item2.easting - item.easting, item2.northing - item.northing);

                            GL.Vertex3(item.easting - (Math.Sin(lineHeading) * mf.gyd.abLength), item.northing - (Math.Cos(lineHeading) * mf.gyd.abLength), 0);
                        }

                        GL.Vertex3(item.easting, item.northing, 0);

                        if (j == mf.gyd.curveArr[i].points.Count - 1 && !mf.gyd.curveArr[i].loop)
                        {
                            vec2 item2 = mf.gyd.curveArr[i].points[j - 1];
                            double lineHeading = Math.Atan2(item.easting - item2.easting, item.northing - item2.northing);
                            GL.Vertex3(item.easting + (Math.Sin(lineHeading) * mf.gyd.abLength), item.northing + (Math.Cos(lineHeading) * mf.gyd.abLength), 0);
                        }
                    }
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

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.23122f, 0.2318f, 0.2315f, 1.0f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 mat = Matrix4.CreateOrthographic((float)currentDist, (float)currentDist, -1.0f, 1.0f);
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.EnableClientState(ArrayCap.VertexArray);
        }

        #region Help
        private void btnCancelTouch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnCancelTouch, gStr.gsHelp).ShowDialog(this);
        }

        private void nudDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_nudDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void btnFlipOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnFlipOffset, gStr.gsHelp).ShowDialog(this);
        }

        private void btnMakeBoundaryCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnMakeBoundaryCurve, gStr.gsHelp).ShowDialog(this);
        }

        private void btnMakeCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnMakeCurve, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSelectCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnSelectCurve, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDeleteCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnDeleteCurve, gStr.gsHelp).ShowDialog(this);
        }

        private void btnMakeABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnMakeABLine, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSelectABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnSelectABLine, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDeleteABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnDeleteABLine, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDrawSections_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_btnDrawSections, gStr.gsHelp).ShowDialog(this);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnExit, gStr.gsHelp).ShowDialog(this);
        }

        private void oglSelf_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_oglSelf, gStr.gsHelp).ShowDialog(this);
        }

        private void tboxNameCurve_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_tboxNameLine, gStr.gsHelp).ShowDialog(this);
        }

        private void tboxNameLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hd_tboxNameLine, gStr.gsHelp).ShowDialog(this);
        }

        #endregion
    }
}