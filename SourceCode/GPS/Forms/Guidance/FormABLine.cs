//Please, if you use this, share the improvements

using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormABLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isSaving;

        public FormABLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            this.Text = gStr.gsABline;
        }

        private void FormABLine_Load(object sender, EventArgs e)
        {
            panelPick.Top = 3;
            panelPick.Left = 3;
            panelAPlus.Top = 3;
            panelAPlus.Left = 3;
            panelName.Top = 3;
            panelName.Left = 3;

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
        }

        private void UpdateLineList()
        {
            lvLines.Clear();

            int idx = -1;

            for (int i = 0; i < mf.gyd.curveArr.Count; i++)
            {
                if (mf.gyd.curveArr[i].mode.HasFlag(Mode.AB) && mf.gyd.curveArr[i].curvePts.Count > 1)
                {
                    lvLines.Items.Add(new ListViewItem(mf.gyd.curveArr[i].Name.Trim(), i));
                    if (mf.gyd.curveArr[i].Name == mf.gyd.currentABLine?.Name) idx = lvLines.Items.Count - 1;
                }
            }

            if (idx > -1)
            {
                lvLines.Items[idx].EnsureVisible();
                lvLines.Items[idx].Selected = true;
            }
            // go to bottom of list - if there is a bottom
            else if (lvLines.Items.Count > 0)
            {
                lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();
                lvLines.Items[lvLines.Items.Count - 1].Selected = true;
            }
            lvLines.Select();
        }

        private void btnCancel_APlus_Click(object sender, EventArgs e)
        {
            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            btnBPoint.BackColor = System.Drawing.Color.Transparent;

            mf.gyd.EditGuidanceLine = null;
        }

        private void btnAPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset, fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset, fix.heading));

            nudHeading.Enabled = true;
            nudHeading.Value = (decimal)glm.toDegrees(fix.heading);

            btnBPoint.Enabled = true;
            btnAPoint.Enabled = false;

            btnEnter_APlus.Enabled = true;
        }

        private void btnBPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            btnBPoint.BackColor = System.Drawing.Color.Teal;

            vec3 tt = new vec3(fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset, fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset, fix.heading);

            // heading based on AB points
            double heading = Math.Atan2(tt.easting - mf.gyd.EditGuidanceLine.curvePts[0].easting,
                tt.northing - mf.gyd.EditGuidanceLine.curvePts[0].northing);
            if (heading < 0) heading += glm.twoPI;
            tt.heading = heading;

            if (mf.gyd.EditGuidanceLine.curvePts.Count > 1)
                mf.gyd.EditGuidanceLine.curvePts[1] = tt;
            else
                mf.gyd.EditGuidanceLine.curvePts.Add(tt);

            nudHeading.Value = (decimal)glm.toDegrees(heading);

            textBox1.Text = "AB " +
                (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                "\u00B0 " + mf.FindDirection(heading);
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                double heading = glm.toRadians((double)nudHeading.Value);

                if (mf.gyd.EditGuidanceLine.curvePts.Count > 1)
                {
                    mf.gyd.EditGuidanceLine.curvePts[0] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading);
                    mf.gyd.EditGuidanceLine.curvePts[1] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading);
                }
                else if (mf.gyd.EditGuidanceLine.curvePts.Count > 0)
                {
                    mf.gyd.EditGuidanceLine.curvePts[0] = new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading);
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading));
                }
                else
                {
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting, mf.gyd.EditGuidanceLine.curvePts[0].northing, heading));
                    mf.gyd.EditGuidanceLine.curvePts.Add(new vec3(mf.gyd.EditGuidanceLine.curvePts[0].easting + Math.Sin(heading), mf.gyd.EditGuidanceLine.curvePts[0].northing + Math.Cos(heading), heading));
                }

                textBox1.Text = "AB " +
                    (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(heading);
            }
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            panelAPlus.Visible = false;
            panelName.Visible = true;
        }

        private void BtnNewABLine_Click(object sender, EventArgs e)
        {
            lvLines.SelectedItems.Clear();
            panelPick.Visible = false;
            panelAPlus.Visible = true;
            panelName.Visible = false;

            btnAPoint.Enabled = true;
            btnBPoint.Enabled = false;
            nudHeading.Enabled = false;

            btnEnter_APlus.Enabled = false;

            mf.gyd.EditGuidanceLine = new CGuidanceLine(Mode.AB);

            this.Size = new System.Drawing.Size(270, 360);
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    textBox1.Text = mf.gyd.curveArr[idx].Name.Trim();

                    panelPick.Visible = false;
                    panelName.Visible = true;
                    this.Size = new System.Drawing.Size(270, 360);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "") textBox1.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            if (mf.gyd.EditGuidanceLine != null)
            {
                string text = textBox1.Text.Trim();
                while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                    text += " ";

                mf.gyd.EditGuidanceLine.Name = text;
                mf.gyd.curveArr.Add(mf.gyd.EditGuidanceLine);
                mf.gyd.EditGuidanceLine = null;
            }
            else if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    string text = textBox1.Text.Trim();
                    while (mf.gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                        text += " ";

                    mf.gyd.curveArr[idx].Name = text;
                }
            }

            mf.FileSaveABLines();
            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    panelAPlus.Visible = false;
                    panelPick.Visible = false;
                    panelName.Visible = true;

                    this.Size = new System.Drawing.Size(270, 360);

                    CGuidanceLine New = new CGuidanceLine(Mode.AB);
                    New.curvePts.AddRange(mf.gyd.curveArr[idx].curvePts.ToArray());
                    mf.gyd.EditGuidanceLine = New;
                    textBox1.Text = mf.gyd.curveArr[idx].Name.Trim() + " Copy";
                }
            }
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                mf.gyd.isValid = false;

                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                    mf.gyd.ReverseGuidanceLine(mf.gyd.curveArr[idx]);

                mf.FileSaveABLines();

                UpdateLineList();
                lvLines.Focus();
            }
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    if (mf.gyd.currentABLine.Name == mf.gyd.curveArr[idx].Name)
                        mf.gyd.currentABLine = null;

                    if (mf.gyd.currentGuidanceLine.Name == mf.gyd.curveArr[idx].Name)
                    {
                        mf.gyd.isValid = false;
                        mf.gyd.moveDistance = 0;
                        mf.gyd.currentGuidanceLine = null;
                        if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                        if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                    }

                    mf.gyd.curveArr.RemoveAt(idx);
                    lvLines.SelectedItems[0].Remove();
                }

                mf.FileSaveABLines();
            }

            UpdateLineList();
            lvLines.Focus();
        }

        private void FormABLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.gyd.isValid = false;
            mf.gyd.moveDistance = 0;

            if (isSaving && lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.Items[lvLines.SelectedIndices[0]].ImageIndex;
                if (idx > -1)
                {
                    mf.gyd.currentGuidanceLine = mf.gyd.currentABLine = new CGuidanceLine(mf.gyd.curveArr[idx]);

                    mf.EnableYouTurnButtons();
                }
            }
            else
            {
                mf.btnABLine.Image = Properties.Resources.ABLineOff;
                mf.gyd.isBtnABLineOn = false;

                mf.gyd.currentABLine = null;

                mf.DisableYouTurnButtons();
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
                mf.KeyboardToText((TextBox)sender, this);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnterAB(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK) 
                {
                    panelAPlus.Visible = false;
                    panelName.Visible = true;
                    textBox1.Text = form.headingText;
                }
                else
                {
                    btnCancel_APlus.PerformClick();
                }
            }
        }

        #region Help
        private void btnListDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnListDelete, gStr.gsHelp);
        }

        private void btnCancel_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancel, gStr.gsHelp);
        }

        private void btnNewABLine_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnNewABLine, gStr.gsHelp);
        }

        private void btnListUse_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnListUse, gStr.gsHelp);
        }

        private void btnSwapAB_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ht_btnSwapAB, gStr.gsHelp);
        }

        private void btnEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hd_tboxNameLine, gStr.gsHelp);
        }

        private void btnDuplicate_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnDuplicate, gStr.gsHelp);
        }

        private void btnAPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAPoint, gStr.gsHelp);
        }

        private void btnBPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnBPoint, gStr.gsHelp);
        }

        private void nudHeading_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_nudHeading, gStr.gsHelp);
        }

        private void btnManual_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnManual, gStr.gsHelp);
        }

        private void textBox1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_textBox1, gStr.gsHelp);
        }

        private void btnAddTime_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnAddTime, gStr.gsHelp);
        }

        private void btnCancel_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnCancel_Name_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnCancelEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnCancelCreate, gStr.gsHelp);
        }

        private void btnEnter_APlus_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        private void btnAdd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        private void btnSaveEditName_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_btnEnterContinue, gStr.gsHelp);
        }

        #endregion
    }
}
