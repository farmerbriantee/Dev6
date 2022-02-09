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

        private int originalLine = 0;
        private bool isClosing;

        public FormABLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            this.Text = gStr.gsABline;
        }

        private void FormABLine_Load(object sender, EventArgs e)
        {
            //tboxABLineName.Enabled = false;
            //btnAddToFile.Enabled = false;
            //btnAddAndGo.Enabled = false;
            //btnAPoint.Enabled = false;
            //btnBPoint.Enabled = false;
            //cboxHeading.Enabled = false;
            //tboxHeading.Enabled = false;
            //tboxABLineName.Text = "";
            //tboxABLineName.Enabled = false;

            //small window
            //ShowFullPanel(true);

            panelPick.Top = 3;
            panelPick.Left = 3;
            panelAPlus.Top = 3;
            panelAPlus.Left = 3;
            panelName.Top = 3;
            panelName.Left = 3;

            panelEditName.Top = 3;
            panelEditName.Left = 3;

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;
            panelEditName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            originalLine = mf.gyd.numABLineSelected;

            mf.gyd.isABLineBeingSet = false;
            UpdateLineList();
            if (lvLines.Items.Count > 0 && originalLine > 0)
            {
                lvLines.Items[originalLine - 1].EnsureVisible();
                lvLines.Items[originalLine - 1].Selected = true;
                lvLines.Select();
            }
        }

        private void UpdateLineList()
        {
            lvLines.Clear();
            ListViewItem itm;

            foreach (CABLines item in mf.gyd.lineArr)
            {
                itm = new ListViewItem(item.Name);
                lvLines.Items.Add(itm);
            }

            // go to bottom of list - if there is a bottom
            if (lvLines.Items.Count > 0)
            {
                lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();
                lvLines.Items[lvLines.Items.Count - 1].Selected = true;
                lvLines.Select();
            }
        }

        private void btnCancel_APlus_Click(object sender, EventArgs e)
        {
            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelEditName.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            mf.gyd.isABLineBeingSet = false;
            btnBPoint.BackColor = System.Drawing.Color.Transparent;
        }

        private void btnAPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            mf.gyd.desPoint1.easting = fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset;
            mf.gyd.desPoint1.northing = fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset;
            mf.gyd.desHeading = fix.heading;

            mf.gyd.desPoint2.easting = 99999;
            mf.gyd.desPoint2.northing = 99999;

            nudHeading.Enabled = true;
            nudHeading.Value = (decimal)(glm.toDegrees(mf.gyd.desHeading));

            BuildDesLine();

            btnBPoint.Enabled = true;
            btnAPoint.Enabled = false;

            btnEnter_APlus.Enabled = true;
            mf.gyd.isABLineBeingSet = true;
        }

        private void btnBPoint_Click(object sender, EventArgs e)
        {
            vec3 fix = new vec3(mf.pivotAxlePos);

            btnBPoint.BackColor = System.Drawing.Color.Teal;

            mf.gyd.desPoint2.easting = fix.easting + Math.Cos(fix.heading) * mf.tool.toolOffset;
            mf.gyd.desPoint2.northing = fix.northing - Math.Sin(fix.heading) * mf.tool.toolOffset;

            // heading based on AB points
            mf.gyd.desHeading = Math.Atan2(mf.gyd.desPoint2.easting - mf.gyd.desPoint1.easting,
                mf.gyd.desPoint2.northing - mf.gyd.desPoint1.northing);
            if (mf.gyd.desHeading < 0) mf.gyd.desHeading += glm.twoPI;

            nudHeading.Value = (decimal)(glm.toDegrees(mf.gyd.desHeading));

            BuildDesLine();
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                BuildDesLine();
            }
        }

        private void BuildDesLine()
        {
            mf.gyd.desHeading = glm.toRadians((double)nudHeading.Value);

            //sin x cos z for endpoints, opposite for additional lines
            mf.gyd.desP1.easting = mf.gyd.desPoint1.easting - (Math.Sin(mf.gyd.desHeading) * mf.gyd.abLength);
            mf.gyd.desP1.northing = mf.gyd.desPoint1.northing - (Math.Cos(mf.gyd.desHeading) * mf.gyd.abLength);
            mf.gyd.desP2.easting = mf.gyd.desPoint1.easting + (Math.Sin(mf.gyd.desHeading) * mf.gyd.abLength);
            mf.gyd.desP2.northing = mf.gyd.desPoint1.northing + (Math.Cos(mf.gyd.desHeading) * mf.gyd.abLength);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnAdd.Focus();
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

            textBox1.Text = "AB " +
                (Math.Round(glm.toDegrees(mf.gyd.desHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                "\u00B0 " + mf.FindDirection(mf.gyd.desHeading);
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

            this.Size = new System.Drawing.Size(270, 360);

        }
        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];
                textBox2.Text = mf.gyd.lineArr[idx].Name;

                panelPick.Visible = false;
                panelEditName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);
            }
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = lvLines.SelectedIndices[0];

            panelEditName.Visible = false;
            panelPick.Visible = true;

            mf.gyd.lineArr[idx].Name = textBox2.Text.Trim();
            mf.FileSaveABLines();

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
            mf.gyd.isABLineBeingSet = false;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            mf.gyd.lineArr.Add(new CABLines());
            mf.gyd.numABLines = mf.gyd.lineArr.Count;
            mf.gyd.numABLineSelected = mf.gyd.numABLines;

            //index to last one. 
            int idx = mf.gyd.lineArr.Count - 1;

            mf.gyd.lineArr[idx].heading = mf.gyd.desHeading;
            //calculate the new points for the reference line and points
            mf.gyd.lineArr[idx].origin.easting = mf.gyd.desPoint1.easting;
            mf.gyd.lineArr[idx].origin.northing = mf.gyd.desPoint1.northing;

            //name
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            mf.gyd.lineArr[idx].Name = textBox1.Text.Trim();

            mf.FileSaveABLines();

            panelPick.Visible = true;
            panelAPlus.Visible = false;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(470, 360);

            UpdateLineList();
            lvLines.Focus();
            mf.gyd.isABLineBeingSet = false;
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];


                panelPick.Visible = false;
                panelName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);

                panelAPlus.Visible = false;
                panelName.Visible = true;

                mf.gyd.desHeading = mf.gyd.lineArr[idx].heading;

                //calculate the new points for the reference line and points                
                mf.gyd.desPoint1.easting = mf.gyd.lineArr[idx].origin.easting;
                mf.gyd.desPoint1.northing = mf.gyd.lineArr[idx].origin.northing;

                textBox1.Text = mf.gyd.lineArr[idx].Name + " Copy";
            }
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isClosing = true;
            mf.gyd.moveDistance = 0;
            //reset to generate new reference
            mf.gyd.isABValid = false;

            if (lvLines.SelectedItems.Count > 0)
            {
                int idx = lvLines.SelectedIndices[0];
                mf.gyd.numABLineSelected = idx + 1;

                mf.gyd.abHeading = mf.gyd.lineArr[idx].heading;
                mf.gyd.refPoint1 = mf.gyd.lineArr[idx].origin;

                mf.gyd.SetABLineByHeading();

                mf.EnableYouTurnButtons();

                //Go back with Line enabled
                Close();
            }

            //no item selected
            else
            {
                mf.btnABLine.Image = Properties.Resources.ABLineOff;
                mf.gyd.isBtnABLineOn = false;
                mf.gyd.isABLineSet = false;
                mf.gyd.isABLineLoaded = false;
                mf.gyd.numABLineSelected = 0;
                mf.DisableYouTurnButtons();
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                Close();
            }
        }
        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                mf.gyd.isABValid = false;
                int idx = lvLines.SelectedIndices[0];


                mf.gyd.lineArr[idx].heading += Math.PI;
                if (mf.gyd.lineArr[idx].heading > glm.twoPI) mf.gyd.lineArr[idx].heading -= glm.twoPI;


                mf.FileSaveABLines();

                UpdateLineList();
                lvLines.Focus();
            }
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int num = lvLines.SelectedIndices[0];
                mf.gyd.lineArr.RemoveAt(num);
                lvLines.SelectedItems[0].Remove();

                mf.gyd.numABLines = mf.gyd.lineArr.Count;
                if (mf.gyd.numABLineSelected > mf.gyd.numABLines) mf.gyd.numABLineSelected = mf.gyd.numABLines;

                if (mf.gyd.numABLines == 0)
                {
                    mf.gyd.DeleteAB();
                    if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                    if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
                }
                mf.FileSaveABLines();
            }
            else
            {
                if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
                if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            }
            UpdateLineList();
            lvLines.Focus();

        }

        private void FormABLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isClosing = true;

            mf.btnABLine.Image = Properties.Resources.ABLineOff;
            mf.gyd.isBtnABLineOn = false;
            mf.gyd.isABLineSet = false;
            mf.gyd.isABLineLoaded = false;
            mf.gyd.numABLineSelected = 0;
            mf.DisableYouTurnButtons();
            if (mf.isAutoSteerBtnOn) mf.btnAutoSteer.PerformClick();
            if (mf.gyd.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            Close();
            mf.gyd.isABValid = false;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSaveEditName.Focus();
            }
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnterAB(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK) 
                {
                    panelAPlus.Visible = false;
                    panelName.Visible = true;

                    textBox1.Text = "AB m " +
                        (Math.Round(glm.toDegrees(mf.gyd.desHeading), 1)).ToString(CultureInfo.InvariantCulture) +
                        "\u00B0 " + mf.FindDirection(mf.gyd.desHeading);

                    //sin x cos z for endpoints, opposite for additional lines
                    mf.gyd.desP1.easting = mf.gyd.desPoint1.easting - (Math.Sin(mf.gyd.desHeading) * mf.gyd.abLength);
                    mf.gyd.desP1.northing = mf.gyd.desPoint1.northing - (Math.Cos(mf.gyd.desHeading) * mf.gyd.abLength);
                    mf.gyd.desP2.easting = mf.gyd.desPoint1.easting + (Math.Sin(mf.gyd.desHeading) * mf.gyd.abLength);
                    mf.gyd.desP2.northing = mf.gyd.desPoint1.northing + (Math.Cos(mf.gyd.desHeading) * mf.gyd.abLength);
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

        private void textBox2_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.ha_textBox1, gStr.gsHelp);
        }

        #endregion
    }
}
