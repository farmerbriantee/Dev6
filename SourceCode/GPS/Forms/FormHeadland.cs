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

        private bool isA, isSet, isClosing;
        private int start = -1, end = -1;
        private double totalHeadlandWidth = 0;

        //list of coordinates of boundary line
        public List<vec3> headLineTemplate = new List<vec3>();

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
            isA = true;
            isSet = false;

            cboxIsSectionControlled.Checked = mf.bnd.isSectionControlledByHeadland;

            lblHeadlandWidth.Text = "0";
            lblWidthUnits.Text = mf.unitsFtM;

            //Builds line
            nudDistance.Value = 0;
            nudSetDistance.Value = 0;

            BuildHeadLineTemplateFromBoundary(mf.bnd.bndList[0].hdLine.Count > 0 ? mf.bnd.bndList[0].hdLine : mf.bnd.bndList[0].fenceLine);

            mf.CloseTopMosts();
        }

        private void FormHeadland_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        public void BuildHeadLineTemplateFromBoundary(List<vec3> list)
        {
            //to fill the list of line points
            vec3 point = new vec3();

            totalHeadlandWidth = 0;
            lblHeadlandWidth.Text = "0";
            nudDistance.Value = 0;
            //nudSetDistance.Value = 0;

            //outside boundary - count the points from the boundary
            headLineTemplate.Clear();

            int ptCount = mf.bnd.bndList[0].fenceLine.Count;
            for (int i = ptCount - 1; i >= 0; i--)
            {
                //calculate the point inside the boundary
                point.easting = mf.bnd.bndList[0].fenceLine[i].easting;
                point.northing = mf.bnd.bndList[0].fenceLine[i].northing;
                point.heading = mf.bnd.bndList[0].fenceLine[i].heading;
                headLineTemplate.Add(point);
            }

            start = end = -1;
            isSet = false;
        }

        private void FixTurnLine(double totalHeadWidth, List<vec3> curBnd, double spacing)
        {
            //count the points from the boundary

            int lineCount = headLineTemplate.Count;
            double distance;

            //int headCount = mf.bndArr[inTurnNum].bndLine.Count;
            int bndCount = curBnd.Count;
            //remove the points too close to boundary
            for (int i = 0; i < bndCount; i++)
            {
                for (int j = 0; j < lineCount; j++)
                {
                    //make sure distance between headland and boundary is not less then width
                    distance = glm.Distance(curBnd[i], headLineTemplate[j]);
                    if (distance < (totalHeadWidth * 0.96))
                    {
                        if (j > -1 && j < headLineTemplate.Count)
                        {
                            headLineTemplate.RemoveAt(j);
                            lineCount = headLineTemplate.Count;
                        }
                        j = -1;
                    }
                }
            }

            //make sure distance isn't too small between points on turnLine
            bndCount = headLineTemplate.Count;

            //double spacing = mf.tool.toolWidth * 0.25;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = glm.Distance(headLineTemplate[i], headLineTemplate[i + 1]);
                if (distance < spacing)
                {
                    if (i > -1 && (i + 1) < headLineTemplate.Count)
                    {
                        headLineTemplate.RemoveAt(i + 1);
                        bndCount = headLineTemplate.Count;
                    }
                    i--;
                }
            }
            if (headLineTemplate.Count > 1)
            {
                double headinga = Math.Atan2(headLineTemplate[headLineTemplate.Count - 1].easting - headLineTemplate[0].easting, headLineTemplate[headLineTemplate.Count - 1].northing - headLineTemplate[0].northing);
                if (headinga < 0) headinga += glm.twoPI;
                headLineTemplate[0] = new vec3(headLineTemplate[0].easting, headLineTemplate[0].northing, headinga);

                //middle points
                for (int i = 1; i < headLineTemplate.Count; i++)
                {
                    double heading = Math.Atan2(headLineTemplate[i - 1].easting - headLineTemplate[i].easting, headLineTemplate[i - 1].northing - headLineTemplate[i].northing);
                    if (heading < 0) heading += glm.twoPI;
                    headLineTemplate[i] = new vec3(headLineTemplate[i].easting, headLineTemplate[i].northing, heading);
                }
            }
        }

        private void btnSetDistance_Click(object sender, EventArgs e)
        {
            double width = (double)nudSetDistance.Value * mf.ftOrMtoM;

            if (start > end)
            {
                for (int i = start; i < headLineTemplate.Count; i++)
                {
                    double easting = headLineTemplate[i].easting + (-Math.Sin(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double northing = headLineTemplate[i].northing + (-Math.Cos(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double heading = headLineTemplate[i].heading;

                    headLineTemplate[i] = new vec3(easting, northing, heading);
                }

                for (int i = 0; i < end; i++)
                {
                    double easting = headLineTemplate[i].easting + (-Math.Sin(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double northing = headLineTemplate[i].northing + (-Math.Cos(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double heading = headLineTemplate[i].heading;

                    headLineTemplate[i] = new vec3(easting, northing, heading);
                }
            }
            else
            {
                for (int i = start; i < end; i++)
                {
                    double easting = headLineTemplate[i].easting + (-Math.Sin(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double northing = headLineTemplate[i].northing + (-Math.Cos(glm.PIBy2 + headLineTemplate[i].heading) * width);
                    double heading = headLineTemplate[i].heading;

                    headLineTemplate[i] = new vec3(easting, northing, heading);
                }
            }

            isSet = false;
            start = end = -1;
        }

        private void btnMakeFixedHeadland_Click(object sender, EventArgs e)
        {
            double width = (double)nudDistance.Value * mf.ftOrMtoM;

            for (int i = 0; i < headLineTemplate.Count; i++)
            {
                //calculate the point inside the boundary
                double easting = headLineTemplate[i].easting + (-Math.Sin(glm.PIBy2 + headLineTemplate[i].heading) * width);
                double northing = headLineTemplate[i].northing + (-Math.Cos(glm.PIBy2 + headLineTemplate[i].heading) * width);
                double heading = headLineTemplate[i].heading;
                headLineTemplate[i] = new vec3(easting, northing, heading);
            }

            totalHeadlandWidth += width;
            lblHeadlandWidth.Text = (totalHeadlandWidth * mf.m2FtOrM).ToString("N2");

            FixTurnLine(totalHeadlandWidth, mf.bnd.bndList[0].fenceLine, 2);

            isSet = false;
            start = end = -1;
        }

        private void cboxToolWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildHeadLineTemplateFromBoundary(mf.bnd.bndList[0].fenceLine);

            double width = (Math.Round(mf.tool.toolWidth * cboxToolWidths.SelectedIndex, 1));

            for (int i = 0; i < headLineTemplate.Count; i++)
            {
                //calculate the point inside the boundary
                double easting = headLineTemplate[i].easting + (-Math.Sin(glm.PIBy2 + headLineTemplate[i].heading) * width);
                double northing = headLineTemplate[i].northing + (-Math.Cos(glm.PIBy2 + headLineTemplate[i].heading) * width);
                double heading = headLineTemplate[i].heading;
                headLineTemplate[i] = new vec3(easting, northing, heading);
            }

            lblHeadlandWidth.Text = (width * mf.m2FtOrM).ToString("N2");
            totalHeadlandWidth = width;

            FixTurnLine(width, mf.bnd.bndList[0].fenceLine, 2);

            isSet = false;
            start = end = -1;
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

            if (headLineTemplate.Count > 1)
            {
                GL.LineWidth(1);
                GL.Color3(0.20f, 0.96232f, 0.30f);
                GL.PointSize(2);

                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < headLineTemplate.Count; h++) GL.Vertex3(headLineTemplate[h].easting, headLineTemplate[h].northing, 0);
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
                return;
            }

            Point pt = oglSelf.PointToClient(Cursor.Position);

            vec3 plotPt = new vec3
            {
                //convert screen coordinates to field coordinates
                easting = (pt.X - 350) * mf.maxFieldDistance / 632.0,
                northing = (700 - pt.Y - 350) * mf.maxFieldDistance / 632.0,
                heading = 0
            };

            plotPt.easting += mf.fieldCenterX;
            plotPt.northing += mf.fieldCenterY;

            double minDist = double.MaxValue;
            int A = -1;

            //find the closest 2 points to current fix
            for (int t = 0; t < headLineTemplate.Count; t++)
            {
                double dist = ((plotPt.easting - headLineTemplate[t].easting) * (plotPt.easting - headLineTemplate[t].easting))
                                + ((plotPt.northing - headLineTemplate[t].northing) * (plotPt.northing - headLineTemplate[t].northing));
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
                if (((headLineTemplate.Count - end + start) % headLineTemplate.Count) < ((headLineTemplate.Count - start + end) % headLineTemplate.Count)) { int index = start; start = end; end = index; }
            }
        }

        private void DrawABTouchLine()
        {
            GL.PointSize(6);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.990, 0.00, 0.250);
            if (start != -1) GL.Vertex3(headLineTemplate[start].easting, headLineTemplate[start].northing, 0);

            GL.Color3(0.990, 0.960, 0.250);
            if (end != -1) GL.Vertex3(headLineTemplate[end].easting, headLineTemplate[end].northing, 0);
            GL.End();

            if (start != -1 && end != -1)
            {
                GL.Color3(0.965, 0.250, 0.950);
                //draw the turn line oject
                GL.LineWidth(2.0f);
                GL.Begin(PrimitiveType.LineStrip);
                if (headLineTemplate.Count < 1) return;

                if (start > end)
                {
                    for (int c = start; c < headLineTemplate.Count; c++)
                        GL.Vertex3(headLineTemplate[c].easting, headLineTemplate[c].northing, 0);
                    for (int c = 0; c < end; c++)
                        GL.Vertex3(headLineTemplate[c].easting, headLineTemplate[c].northing, 0);
                }
                else
                {
                    for (int c = start; c < end; c++)
                        GL.Vertex3(headLineTemplate[c].easting, headLineTemplate[c].northing, 0);
                }
                GL.End();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BuildHeadLineTemplateFromBoundary(mf.bnd.bndList[0].fenceLine);
        }

        private void nudDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnExit.Focus();
        }

        private void nudSetDistance_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnExit.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
            if (isSet)
            {
                btnExit.Enabled = false;
                btnMakeFixedHeadland.Enabled = false;
                nudDistance.Enabled = false;

                nudSetDistance.Enabled = true;
                btnSetDistance.Enabled = true;
                //btnMoveLeft.Enabled = true;
                //btnMoveRight.Enabled = true;
                //btnMoveUp.Enabled = true;
                //btnMoveDown.Enabled = true;
                //btnDoneManualMove.Enabled = true;
                btnDeletePoints.Enabled = true;
                //btnStartUp.Enabled = true;
                //btnStartDown.Enabled = true;
                //btnEndDown.Enabled = true;
                //btnEndUp.Enabled = true;
            }
            else
            {
                nudSetDistance.Enabled = false;
                btnSetDistance.Enabled = false;
                //btnMoveLeft.Enabled = false;
                //btnMoveRight.Enabled = false;
                //btnMoveUp.Enabled = false;
                //btnMoveDown.Enabled = false;
                //btnDoneManualMove.Enabled = false;
                btnDeletePoints.Enabled = false;
                //btnStartUp.Enabled = false;
                //btnStartDown.Enabled = false;
                //btnEndDown.Enabled = false;
                //btnEndUp.Enabled = false;

                btnExit.Enabled = true;
                btnMakeFixedHeadland.Enabled = true;
                nudDistance.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();

            //does headland control sections
            mf.bnd.isSectionControlledByHeadland = cboxIsSectionControlled.Checked;
            Properties.Settings.Default.setHeadland_isSectionControlled = cboxIsSectionControlled.Checked;
            Properties.Settings.Default.Save();

            double delta = 0;
            for (int i = 0; i < headLineTemplate.Count; i++)
            {
                if (i == 0)
                {
                    mf.bnd.bndList[0].hdLine.Add(new vec3(headLineTemplate[i].easting, headLineTemplate[i].northing, headLineTemplate[i].heading));
                    continue;
                }
                delta += (headLineTemplate[i - 1].heading - headLineTemplate[i].heading);

                if (Math.Abs(delta) > 0.01)
                {
                    vec3 pt = new vec3(headLineTemplate[i].easting, headLineTemplate[i].northing, headLineTemplate[i].heading);

                    mf.bnd.bndList[0].hdLine.Add(pt);
                    delta = 0;
                }
            }
            mf.FileSaveHeadland();
            isClosing = true;
            Close();
        }

        private void btnTurnOffHeadland_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();

            mf.FileSaveHeadland();

            isClosing = true;
            Close();
        }

        private void btnDeletePoints_Click(object sender, EventArgs e)
        {
            if (start > end)
            {
                headLineTemplate.RemoveRange(start, headLineTemplate.Count - start);
                headLineTemplate.RemoveRange(0, end);
            }
            else
                headLineTemplate.RemoveRange(start, end - start);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.23122f, 0.2318f, 0.2315f, 1.0f);
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
