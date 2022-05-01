using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormHeadland : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isA = true, isSet, isSaving;
        private int start = -1, end = -1, index = -1;
        private double totalHeadlandWidth = 0;

        private double currentFieldCenterX, currentFieldCenterY, currentDist;

        public List<Polyline2> headLineTemplate = new List<Polyline2>();

        public FormHeadland(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            //lblPick.Text = gStr.gsSelectALine;
            this.Text = gStr.gsHeadlandForm;
            btnReset.Text = gStr.gsResetAll;

            nudDistance.Controls[0].Enabled = false;


            currentFieldCenterX = mf.fieldCenterX;
            currentFieldCenterY = mf.fieldCenterY;
            currentDist = mf.maxFieldDistance;

            MouseWheel += ZoomByMouseWheel;
        }

        private void FormHeadland_Load(object sender, EventArgs e)
        {
            cboxIsSectionControlled.Checked = mf.bnd.isSectionControlledByHeadland;

            lblHeadlandWidth.Text = "0";
            lblWidthUnits.Text = mf.unitsFtM;

            //Builds line
            nudDistance.Value = 0;
            nudSetDistance.Value = 0;

            BuildHeadLineTemplateFromBoundary(mf.bnd.bndList[0].hdLine.Count > 0);

            oglSelf.Refresh();

            mf.CloseTopMosts();
        }

        private void FormHeadland_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaving)
            {
                for (int j = 0; j < mf.bnd.bndList[0].hdLine.Count; j++)
                {
                    mf.bnd.bndList[0].hdLine[j].RemoveHandle();
                }

                //does headland control sections
                mf.bnd.isSectionControlledByHeadland = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.setHeadland_isSectionControlled = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.Save();

                mf.bnd.bndList[0].hdLine = headLineTemplate;
                mf.FileSaveHeadland();
            }
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

            if (currentDist < 100) currentDist = 100;
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

        private void BuildHeadLineTemplateFromBoundary(bool fromHd = false)
        {
            headLineTemplate.Clear();
            if (fromHd)
            {
                for (int i = 0; i < mf.bnd.bndList[0].hdLine.Count; i++)
                {
                    Polyline2 New = new Polyline2();
                    for (int j = 0; j < mf.bnd.bndList[0].hdLine[i].points.Count; j++)
                    {
                        New.points.Add(new vec2(mf.bnd.bndList[0].hdLine[i].points[j].easting, mf.bnd.bndList[0].hdLine[i].points[j].northing));
                    }
                    headLineTemplate.Add(New);
                }
            }
            else
            {
                Polyline2 New = new Polyline2();
                for (int i = 0; i < mf.bnd.bndList[0].fenceLine.points.Count; i++)
                {
                    New.points.Add(new vec2(mf.bnd.bndList[0].fenceLine.points[i].easting, mf.bnd.bndList[0].fenceLine.points[i].northing));
                }
                headLineTemplate.Add(New);
            }

            totalHeadlandWidth = 0;
            lblHeadlandWidth.Text = "0";
            nudDistance.Value = 0;
            index = start = end = -1;
            isSet = false;
        }

        private void btnSetDistance_Click(object sender, EventArgs e)
        {
            double width = (double)nudSetDistance.Value * mf.userBigToM;

            headLineTemplate.AddRange(headLineTemplate[index].OffsetAndDissolvePolyline(true, width, true, start, end, true));
            headLineTemplate.Remove(headLineTemplate[index]);

            isSet = false;
            index = start = end = -1;

            oglSelf.Refresh();
        }

        private void btnMakeFixedHeadland_Click(object sender, EventArgs e)
        {
            double width = (double)nudDistance.Value * mf.userBigToM;
            if (index < 0)
            {
                List<Polyline2> New = new List<Polyline2>();
                for (int i = 0; i < headLineTemplate.Count; i++)
                {
                    New.AddRange(headLineTemplate[i].OffsetAndDissolvePolyline(true, width, true, -1, -1, true));
                }
                headLineTemplate = New;

                totalHeadlandWidth += width;
                lblHeadlandWidth.Text = (totalHeadlandWidth * mf.mToUserBig).ToString("0.00");
            }
            else
            {
                headLineTemplate.AddRange(headLineTemplate[index].OffsetAndDissolvePolyline(true, width, true, -1, -1, true));
                headLineTemplate.Remove(headLineTemplate[index]);
            }

            isSet = false;
            index = start = end = -1;

            oglSelf.Refresh();
        }

        private void cboxToolWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildHeadLineTemplateFromBoundary();

            double width = mf.tool.toolWidth * cboxToolWidths.SelectedIndex;

            List<Polyline2> New = new List<Polyline2>();
            for (int i = 0; i < headLineTemplate.Count; i++)
            {
                New.AddRange(headLineTemplate[i].OffsetAndDissolvePolyline(true, width, true, -1, -1, true));
            }
            headLineTemplate = New;

            lblHeadlandWidth.Text = (width * mf.mToUserBig).ToString("0.00");
            totalHeadlandWidth = width;

            oglSelf.Refresh();
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

            GL.LineWidth(1);
            GL.Color3(0.20f, 0.96232f, 0.30f);
            GL.PointSize(2);
            for (int i = 0; i < headLineTemplate.Count; i++)
            {
                if (headLineTemplate[i].points.Count > 1)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    for (int h = 0; h < headLineTemplate[i].points.Count; h++)
                    {
                        GL.Vertex3(headLineTemplate[i].points[h].easting, headLineTemplate[i].points[h].northing, 0);
                    }
                    GL.End();
                }
            }

            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.90f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            if (index != -1)
            {
                DrawABTouchLine();
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            if (isSet)
            {
                isSet = false;
                index = start = end = -1;
            }
            else
            {
                Point pt = oglSelf.PointToClient(Cursor.Position);

                //convert screen coordinates to field coordinates
                vec2 plotPt = new vec2(
                    currentFieldCenterX + (pt.X - 350)/700.0 * currentDist,
                    currentFieldCenterY + (350 - pt.Y)/700.0 * currentDist
                );

                double minDist = double.MaxValue;
                int A = -1;
                int indexB = -1;

                //find the closest 2 points to current fix
                for (int s = index < 0 ? 0 : index; s < headLineTemplate.Count; s++)
                {
                    for (int t = 0; t < headLineTemplate[s].points.Count; t++)
                    {
                        double dist = glm.Distance(plotPt, headLineTemplate[s].points[t]);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            A = t;
                            indexB = s;
                        }
                    }
                    if (index > -1) break;
                }

                if (isA || index < 0)
                {
                    start = A;
                    end = -1;
                    isA = false;
                    index = indexB;
                }
                else 
                {
                    end = A;
                    isA = true;
                    isSet = true;
                    if (((headLineTemplate[index].points.Count - end + start) % headLineTemplate[index].points.Count) < ((headLineTemplate[index].points.Count - start + end) % headLineTemplate[index].points.Count)) { int index = start; start = end; end = index; }
                }
            }

            oglSelf.Refresh();

            nudSetDistance.Enabled = btnSetDistance.Enabled = btnDeletePoints.Enabled = isSet;
            btnMakeFixedHeadland.Enabled = nudDistance.Enabled = !isSet;
        }

        private void DrawABTouchLine()
        {
            GL.PointSize(6);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.990, 0.00, 0.250);
            if (start != -1) GL.Vertex3(headLineTemplate[index].points[start].easting, headLineTemplate[index].points[start].northing, 0);

            GL.Color3(0.990, 0.960, 0.250);
            if (end != -1) GL.Vertex3(headLineTemplate[index].points[end].easting, headLineTemplate[index].points[end].northing, 0);
            GL.End();

            if (start != -1 && end != -1)
            {
                GL.Color3(0.965, 0.250, 0.950);
                //draw the turn line oject
                GL.LineWidth(2.0f);
                GL.Begin(PrimitiveType.LineStrip);
                if (headLineTemplate[index].points.Count < 1) return;

                if (start > end)
                {
                    for (int c = start; c < headLineTemplate[index].points.Count; c++)
                        GL.Vertex3(headLineTemplate[index].points[c].easting, headLineTemplate[index].points[c].northing, 0);
                    for (int c = 0; c <= end; c++)
                        GL.Vertex3(headLineTemplate[index].points[c].easting, headLineTemplate[index].points[c].northing, 0);
                }
                else
                {
                    for (int c = start; c <= end; c++)
                        GL.Vertex3(headLineTemplate[index].points[c].easting, headLineTemplate[index].points[c].northing, 0);
                }
                GL.End();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BuildHeadLineTemplateFromBoundary();

            oglSelf.Refresh();
        }

        private void nudDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnExit.Focus();

            oglSelf.Refresh();
        }

        private void nudSetDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnExit.Focus();

            oglSelf.Refresh();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void btnTurnOffHeadland_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDeletePoints_Click(object sender, EventArgs e)
        {
            if (start > end)
            {
                headLineTemplate[index].points.RemoveRange(start, headLineTemplate[index].points.Count - start);
                headLineTemplate[index].points.RemoveRange(0, end);
            }
            else
                headLineTemplate[index].points.RemoveRange(start, end - start);

            oglSelf.Refresh();
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
            oglSelf.Refresh();
        }

        #region Help
        private void cboxToolWidths_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_cboxToolWidths, gStr.gsHelp).ShowDialog(this);
        }

        private void nudDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_nudDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void btnMakeFixedHeadland_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnMakeFixedHeadland, gStr.gsHelp).ShowDialog(this);
        }

        private void nudSetDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_nudSetDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void btnSetDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnSetDistance, gStr.gsHelp).ShowDialog(this);
        }

        private void btnDeletePoints_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnDeletePoints, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxIsSectionControlled_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_cboxIsSectionControlled, gStr.gsHelp).ShowDialog(this);
        }

        private void btnReset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnReset, gStr.gsHelp).ShowDialog(this);
        }

        private void btnTurnOffHeadland_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnTurnOffHeadland, gStr.gsHelp).ShowDialog(this);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hh_btnExit, gStr.gsHelp).ShowDialog(this);
        }

        #endregion

    }
}

/*
            DialogResult result2 = new FormHelp(gStr, gStr.gsHelp).ShowDialog(this)

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
