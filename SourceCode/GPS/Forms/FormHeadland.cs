using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormHeadland : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isA = true, isSet, isSaving;
        private int start = -1, end = -1;
        private double totalHeadlandWidth = 0;

        public Polyline headLineTemplate = new Polyline();

        public FormHeadland(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            //lblPick.Text = gStr.gsSelectALine;
            this.Text = gStr.gsHeadlandForm;
            btnReset.Text = gStr.gsResetAll;

            nudDistance.Controls[0].Enabled = false;
        }

        private void FormHeadland_Load(object sender, EventArgs e)
        {
            cboxIsSectionControlled.Checked = mf.bnd.isSectionControlledByHeadland;

            lblHeadlandWidth.Text = "0";
            lblWidthUnits.Text = mf.unitsFtM;

            //Builds line
            nudDistance.Value = 0;
            nudSetDistance.Value = 0;

            BuildHeadLineTemplateFromBoundary(mf.bnd.bndList[0].hdLine.points.Count > 0);

            oglSelf.Refresh();

            mf.CloseTopMosts();
        }

        private void FormHeadland_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.bnd.bndList[0].hdLine.points?.Clear();

            if (isSaving)
            {
                //does headland control sections
                mf.bnd.isSectionControlledByHeadland = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.setHeadland_isSectionControlled = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.Save();

                for (int i = 0; i < headLineTemplate.points.Count; i++)
                {
                    mf.bnd.bndList[0].hdLine.points.Add(new vec2(headLineTemplate.points[i].easting, headLineTemplate.points[i].northing));
                }
            }

            mf.FileSaveHeadland();
        }

        private void BuildHeadLineTemplateFromBoundary(bool fromHd = false)
        {
            headLineTemplate.points.Clear();
            if (fromHd)
                for (int i = 0; i < mf.bnd.bndList[0].hdLine.points.Count; i++)
                    headLineTemplate.points.Add(new vec2(mf.bnd.bndList[0].hdLine.points[i].easting, mf.bnd.bndList[0].hdLine.points[i].northing));
            else
                for (int i = 0; i < mf.bnd.bndList[0].fenceLine.points.Count; i++)
                    headLineTemplate.points.Add(new vec2(mf.bnd.bndList[0].fenceLine.points[i].easting, mf.bnd.bndList[0].fenceLine.points[i].northing));

            totalHeadlandWidth = 0;
            lblHeadlandWidth.Text = "0";
            nudDistance.Value = 0;
            start = end = -1;
            isSet = false;
        }

        private void btnSetDistance_Click(object sender, EventArgs e)
        {
            double width = (double)nudSetDistance.Value * mf.ftOrMtoM;

            headLineTemplate = headLineTemplate.OffsetAndDissolvePolyline(width, true, start, end, true);

            isSet = false;
            start = end = -1;

            oglSelf.Refresh();
        }

        private void btnMakeFixedHeadland_Click(object sender, EventArgs e)
        {
            double width = (double)nudDistance.Value * mf.ftOrMtoM;

            headLineTemplate = headLineTemplate.OffsetAndDissolvePolyline(width, true, -1, -1, true);

            totalHeadlandWidth += width;
            lblHeadlandWidth.Text = (totalHeadlandWidth * mf.m2FtOrM).ToString("N2");

            isSet = false;
            start = end = -1;

            oglSelf.Refresh();
        }

        private void cboxToolWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildHeadLineTemplateFromBoundary();

            double width = mf.tool.toolWidth * cboxToolWidths.SelectedIndex;

            headLineTemplate = headLineTemplate.OffsetAndDissolvePolyline(width, true, -1, -1, true);

            lblHeadlandWidth.Text = (width * mf.m2FtOrM).ToString("N2");
            totalHeadlandWidth = width;

            oglSelf.Refresh();
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX, -mf.fieldCenterY, 0);

            GL.Color3(1, 1, 1);

            //draw all the boundaries
            mf.bnd.DrawFenceLines();

            if (headLineTemplate.points.Count > 1)
            {
                GL.LineWidth(1);
                GL.Color3(0.20f, 0.96232f, 0.30f);
                GL.PointSize(2);

                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < headLineTemplate.points.Count; h++) GL.Vertex3(headLineTemplate.points[h].easting, headLineTemplate.points[h].northing, 0);
                GL.End();
            }

            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.90f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            DrawABTouchLine();

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            if (isSet)
            {
                isSet = false;
                start = end = -1;
            }
            else
            {
                Point pt = oglSelf.PointToClient(Cursor.Position);

                //convert screen coordinates to field coordinates
                vec2 plotPt = new vec2(
                    mf.fieldCenterX + (pt.X - 350)/700.0 * mf.maxFieldDistance,
                    mf.fieldCenterY + (350 - pt.Y)/700.0 * mf.maxFieldDistance
                );

                double minDist = double.MaxValue;
                int A = -1;

                //find the closest 2 points to current fix
                for (int t = 0; t < headLineTemplate.points.Count; t++)
                {
                    double dist = glm.Distance(plotPt, headLineTemplate.points[t]);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        A = t;
                    }
                }

                if (isA)
                {
                    start = A;
                    end = -1;
                    isA = false;
                }
                else
                {
                    end = A;
                    isA = true;
                    isSet = true;
                    if (((headLineTemplate.points.Count - end + start) % headLineTemplate.points.Count) < ((headLineTemplate.points.Count - start + end) % headLineTemplate.points.Count)) { int index = start; start = end; end = index; }
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
            if (start != -1) GL.Vertex3(headLineTemplate.points[start].easting, headLineTemplate.points[start].northing, 0);

            GL.Color3(0.990, 0.960, 0.250);
            if (end != -1) GL.Vertex3(headLineTemplate.points[end].easting, headLineTemplate.points[end].northing, 0);
            GL.End();

            if (start != -1 && end != -1)
            {
                GL.Color3(0.965, 0.250, 0.950);
                //draw the turn line oject
                GL.LineWidth(2.0f);
                GL.Begin(PrimitiveType.LineStrip);
                if (headLineTemplate.points.Count < 1) return;

                if (start > end)
                {
                    for (int c = start; c < headLineTemplate.points.Count; c++)
                        GL.Vertex3(headLineTemplate.points[c].easting, headLineTemplate.points[c].northing, 0);
                    for (int c = 0; c < end; c++)
                        GL.Vertex3(headLineTemplate.points[c].easting, headLineTemplate.points[c].northing, 0);
                }
                else
                {
                    for (int c = start; c < end; c++)
                        GL.Vertex3(headLineTemplate.points[c].easting, headLineTemplate.points[c].northing, 0);
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
                headLineTemplate.points.RemoveRange(start, headLineTemplate.points.Count - start);
                headLineTemplate.points.RemoveRange(0, end);
            }
            else
                headLineTemplate.points.RemoveRange(start, end - start);

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
            Matrix4 mat = Matrix4.CreateOrthographic((float)mf.maxFieldDistance, (float)mf.maxFieldDistance, -1.0f, 1.0f);
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
            oglSelf.Refresh();
        }

        #region Help
        private void cboxToolWidths_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_cboxToolWidths, gStr.gsHelp);
        }

        private void nudDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_nudDistance, gStr.gsHelp);
        }

        private void btnMakeFixedHeadland_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnMakeFixedHeadland, gStr.gsHelp);
        }

        private void nudSetDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_nudSetDistance, gStr.gsHelp);
        }

        private void btnSetDistance_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnSetDistance, gStr.gsHelp);
        }

        private void btnDeletePoints_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnDeletePoints, gStr.gsHelp);
        }

        private void cboxIsSectionControlled_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_cboxIsSectionControlled, gStr.gsHelp);
        }

        private void btnReset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnReset, gStr.gsHelp);
        }

        private void btnTurnOffHeadland_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnTurnOffHeadland, gStr.gsHelp);
        }

        private void btnExit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hh_btnExit, gStr.gsHelp);
        }

        #endregion

    }
}

/*
            
            MessageBox.Show(gStr, gStr.gsHelp);

            DialogResult result2 = MessageBox.Show(gStr, gStr.gsHelp,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
