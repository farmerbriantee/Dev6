using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigSections : UserControl2
    {

        private readonly FormGPS mf;

        private int numberOfSections, maxOverlap, xpos = 0;

        private List<double> sectionWidths = new List<double>();
        private List<Button> sectionButtons = new List<Button>();
        private List<Label> sectionLabels = new List<Label>();

        private double defaultSectionWidth, cutoffSpeed, bndOverlap, totalWidth = 0;
        private bool somethingChanged = false;

        public ConfigSections(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            Scroll += (s, e) => { HandleScroll(Math.Sign(e.NewValue - e.OldValue)); };
            MouseWheel += (s, e) => { HandleScroll(Math.Sign(e.Delta)); };
        }

        private void ConfigSections_Load(object sender, EventArgs e)
        {
            if (glm.isMetric)
            {
                lblSecTotalWidthFeet.Text = (mf.tool.toolWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM;;
                lblTurnOffBelowUnits.Text = gStr.gsKMH;
            }
            else
            {
                lblSecTotalWidthInches.Visible = true;
                lblTurnOffBelowUnits.Text = gStr.gsMPH;
                double toFeet = mf.tool.toolWidth * glm.mToUserBig;
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }

            cboxSectionResponse.Checked = mf.tool.isFastSections;

            cutoffSpeed = mf.tool.slowSpeedCutoff;
            nudCutoffSpeed.Text = (cutoffSpeed * glm.KMHToUser).ToString("0.0");

            numberOfSections = mf.tool.numOfSections;
            btnNumSections.Text = numberOfSections.ToString();

            maxOverlap = mf.tool.maxOverlap;
            nudMinCoverage.Text = maxOverlap.ToString();

            bndOverlap = mf.tool.boundOverlap;
            btnMaxBoundOverlap.Text = bndOverlap.ToString();

            defaultSectionWidth = Properties.Vehicle.Default.Tool_defaultSectionWidth;
            nudDefaultSectionWidth.Text = (defaultSectionWidth * glm.mToUser).ToString("0");

            //based on number of sections and values update the page before displaying
            UpdateSpinners();
        }

        public override void Close()
        {
            if (somethingChanged)
            {
                //turn section buttons all OFF
                for (int j = 0; j < mf.tool.sections.Count; j++)
                {
                    mf.tool.sections[j].sectionOnRequest = 0;
                    mf.tool.sections[j].isSectionOn = false;
                    if (mf.tool.sections[j].isMappingOn)
                        mf.tool.sections[j].TurnMappingOff();
                }

                string widthString = "";
                for (int j = 0; j < sectionWidths.Count; j++)
                {
                    widthString += sectionWidths[j].ToString("0.00", CultureInfo.InvariantCulture);
                    if (j < sectionWidths.Count - 1) widthString += ",";
                }

                Properties.Vehicle.Default.Tool_Width = mf.tool.toolWidth = totalWidth;
                Properties.Vehicle.Default.Tool_SectionWidths = widthString;

                bool update = false;


                if (mf.tool.numOfSections != numberOfSections)
                {
                    mf.tool.numOfSections = numberOfSections;

                    Properties.Vehicle.Default.Tool_numSections = mf.tool.numOfSections;
                    update = true;
                }

                //update the sections to newly configured widths and positions in main
                mf.tool.SectionSetPosition();


                //line up manual buttons based on # of sections
                if (update)
                    mf.LineUpManualBtns();

                mf.tram.isOuter = ((int)(mf.tram.tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0;
            }

            Properties.Vehicle.Default.Tool_slowSpeedCutoff = mf.tool.slowSpeedCutoff = cutoffSpeed;
            Properties.Vehicle.Default.Tool_minCoverage = mf.tool.maxOverlap = maxOverlap;
            Properties.Vehicle.Default.setVehicle_bndOverlap = mf.tool.boundOverlap = bndOverlap;

            Properties.Vehicle.Default.Tool_defaultSectionWidth = defaultSectionWidth;
            Properties.Vehicle.Default.Tool_Section_isFast = mf.tool.isFastSections = cboxSectionResponse.Checked;

            Properties.Vehicle.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HandleScroll(-6);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HandleScroll(6);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            double procent = (1.0 / button1.Width) * me.Location.X;
            xpos = (int)(procent * (panel1.Width - panelSections.Width));

            HandleScroll(0);
        }

        private void HandleScroll(int delta)
        {
            xpos += delta * 25;
            xpos = Math.Max(xpos, panel1.Width - panelSections.Width);
            xpos = Math.Min(xpos, panel1.Width - panelSections.Width > 0 ? (panel1.Width - panelSections.Width) / 2 : 0);

            panelSections.Location = new Point(xpos, 0);
        }

        private void btnMaxBoundOverlap_Click(object sender, EventArgs e)
        {
            btnMaxBoundOverlap.KeypadToButton(ref bndOverlap, 0, 100, 0);
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                int idx = Convert.ToInt32(button.Name);
                if (idx < sectionWidths.Count)
                {
                    double val = sectionWidths[idx];

                    if (button.KeypadToButton(ref val, 0.01, 50, 0, glm.mToUser, glm.userToM))
                    {
                        sectionWidths[idx] = val;
                        SectionFeetInchesTotalWidthLabelUpdate(idx);
                    }
                }
            }
        }

        private void nudCutoffSpeed_Click(object sender, EventArgs e)
        {
            nudCutoffSpeed.KeypadToButton(ref cutoffSpeed, 0, 30, 1, glm.KMHToUser, glm.userToKMH);
        }

        private void nudMinCoverage_Click(object sender, EventArgs e)
        {
            nudMinCoverage.KeypadToButton(ref maxOverlap, 0, 100);
        }

        private void btnNumSections_Click(object sender, EventArgs e)
        {
            if (btnNumSections.KeypadToButton(ref numberOfSections, 0, CTool.MAXSECTIONS))
            {
                double wide = defaultSectionWidth;

                if (numberOfSections * wide > 50)
                {
                    wide = 50.0 / numberOfSections;
                }

                UpdateSpinners();

                string tt = (wide * glm.mToUser).ToString("0");
                for (int j = 0; j < sectionWidths.Count; j++)
                {
                    sectionWidths[j] = wide;
                    sectionButtons[j].Text = tt;
                }

                //update in settings dialog ONLY total tool width
                SectionFeetInchesTotalWidthLabelUpdate(0);
            }
        }

        private void nudDefaultSectionWidth_Click(object sender, EventArgs e)
        {
            nudDefaultSectionWidth.KeypadToButton(ref defaultSectionWidth, 0.01, 50, 0, glm.mToUser, glm.userToM);
        }

        private void UpdateSpinners()
        {
            for (int j = sectionWidths.Count - 1; j >= numberOfSections; j--)
            {
                panelSections.Controls.Remove(sectionButtons[j]);
                panelSections.Controls.Remove(sectionLabels[j]);
                sectionWidths.RemoveAt(j);
                sectionButtons.RemoveAt(j);
                sectionLabels.RemoveAt(j);
            }

            string stringsetting = Properties.Vehicle.Default.Tool_SectionWidths;
            string[] words = stringsetting.Split(',');

            for (int j = sectionWidths.Count; j < numberOfSections; j++)
            {
                sectionWidths.Add(j < words.Length ? double.Parse(words[j], CultureInfo.InvariantCulture) : defaultSectionWidth);

                Button New = new Button
                {
                    Margin = new Padding(6),
                    Size = new Size(125, 50),
                    Name = j.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = (sectionWidths[j] * glm.mToUser).ToString("0"),
                    Font = new Font("Tahoma", 24F, FontStyle.Bold, GraphicsUnit.Point, 0),
                    UseVisualStyleBackColor = false
                };

                New.HelpRequested += new HelpEventHandler(nudSection_HelpRequested);
                New.Click += btnSection_Click;
                New.BackColor = SystemColors.ButtonFace;
                sectionButtons.Add(New);

                Label Newlbl = new Label
                {
                    Font = new Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point,0),
                    ForeColor = Color.Black,
                    ImeMode = ImeMode.NoControl,
                    Size = new Size(125, 25),
                    Text = sectionWidths.Count.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                sectionLabels.Add(Newlbl);

                //var newPoint = new Point(100 + 225 * j + panelSections.AutoScrollPosition.X, 50);
                var newPoint = new Point(25 + 150 * j + panelSections.AutoScrollPosition.X, 50);

                panelSections.Controls.Add(Newlbl);
                Newlbl.Location = new Point(newPoint.X, 50);
                panelSections.Controls.Add(New);
                New.Location = new Point(newPoint.X, 100);
            }

            panelSections.Width = 25 + 150 * numberOfSections;
            HandleScroll(0);
        }

        //update tool width label at bottom of window
        private void SectionFeetInchesTotalWidthLabelUpdate(int idx)
        {
            somethingChanged = true;
            totalWidth = 0;
            for (int j = 0; j < sectionWidths.Count; j++)
            {
                totalWidth += sectionWidths[j];
            }

            if (Math.Round(totalWidth, 3) > 50)
            {
                if (glm.isMetric)
                    mf.TimedMessageBox(3000, "Too Wide", "Max 50 Meters");
                else
                    mf.TimedMessageBox(3000, "Too Wide", "Max 164 Feet");

                totalWidth -= sectionWidths[idx];
                sectionWidths[idx] = 50 - (totalWidth - sectionWidths[idx]);
                sectionButtons[idx].Text = (sectionWidths[idx] * glm.mToUser).ToString("0");
                totalWidth += sectionWidths[idx];
            }

            if (glm.isMetric)
            {
                lblSecTotalWidthFeet.Text = (totalWidth * glm.mToUserBig).ToString("0.00") + glm.unitsFtM;;
            }
            else
            {
                double toFeet = (totalWidth * glm.mToUserBig);
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }
        }

        private void cboxSectionResponse_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxSectionResponse, gStr.gsHelp).ShowDialog(this);
        }

        private void cboxNumSections_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_cboxNumSections, gStr.gsHelp).ShowDialog(this);
        }

        private void nudMinCoverage_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudMinCoverage, gStr.gsHelp).ShowDialog(this);
        }

        private void nudSection_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudSectionWidth, gStr.gsHelp).ShowDialog(this);
        }

        private void nudCutoffSpeed_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudCutoffSpeed, gStr.gsHelp).ShowDialog(this);
        }

        private void nudDefaultSectionWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_nudDefaultSectionWidth, gStr.gsHelp).ShowDialog(this);
        }
    }
}
