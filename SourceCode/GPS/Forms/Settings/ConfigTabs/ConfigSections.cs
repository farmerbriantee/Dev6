using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigSections : UserControl2
    {

        private readonly FormGPS mf;

        private int numberOfSections, minCoverage;

        private double sectionWidth1, sectionWidth2, sectionWidth3, sectionWidth4, sectionWidth5, sectionWidth6,
                sectionWidth7, sectionWidth8, sectionWidth9, sectionWidth10, sectionWidth11, sectionWidth12,
                sectionWidth13, sectionWidth14, sectionWidth15, sectionWidth16;

        private double sectionPosition1, sectionPosition2, sectionPosition3, sectionPosition4,
                        sectionPosition5, sectionPosition6, sectionPosition7, sectionPosition8, sectionPosition9,
                        sectionPosition10, sectionPosition11, sectionPosition12, sectionPosition13, sectionPosition14,
                        sectionPosition15, sectionPosition16, sectionPosition17;

        private double defaultSectionWidth, cutoffSpeed;
        private bool somethingChanged = false;

        public ConfigSections(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigSections_Load(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                lblSecTotalWidthFeet.Text = (mf.tool.toolWidth * mf.mToUser).ToString("0") + mf.unitsInCm;
                lblTurnOffBelowUnits.Text = gStr.gsKMH;
            }
            else
            {
                lblSecTotalWidthInches.Visible = true;
                lblTurnOffBelowUnits.Text = gStr.gsMPH;
                double toFeet = mf.tool.toolWidth * mf.mToUserBig;
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }

            cboxSectionResponse.Checked = Properties.Vehicle.Default.setSection_isFast;

            cutoffSpeed = Properties.Vehicle.Default.setVehicle_slowSpeedCutoff;
            nudCutoffSpeed.Text = (cutoffSpeed * mf.KMHToUser).ToString("0.0");

            numberOfSections = Properties.Vehicle.Default.setVehicle_numSections;
            btnNumSections.Text = numberOfSections.ToString();

            minCoverage = Properties.Vehicle.Default.setVehicle_minCoverage;
            nudMinCoverage.Text = minCoverage.ToString() + "%";

            defaultSectionWidth = Properties.Vehicle.Default.setTool_defaultSectionWidth;
            nudDefaultSectionWidth.Text = (defaultSectionWidth * mf.mToUser).ToString("0");

            sectionWidth1 = Math.Abs(Properties.Vehicle.Default.setSection_position2 - Properties.Vehicle.Default.setSection_position1);
            sectionWidth2 = Math.Abs(Properties.Vehicle.Default.setSection_position3 - Properties.Vehicle.Default.setSection_position2);
            sectionWidth3 = Math.Abs(Properties.Vehicle.Default.setSection_position4 - Properties.Vehicle.Default.setSection_position3);
            sectionWidth4 = Math.Abs(Properties.Vehicle.Default.setSection_position5 - Properties.Vehicle.Default.setSection_position4);
            sectionWidth5 = Math.Abs(Properties.Vehicle.Default.setSection_position6 - Properties.Vehicle.Default.setSection_position5);
            sectionWidth6 = Math.Abs(Properties.Vehicle.Default.setSection_position7 - Properties.Vehicle.Default.setSection_position6);
            sectionWidth7 = Math.Abs(Properties.Vehicle.Default.setSection_position8 - Properties.Vehicle.Default.setSection_position7);
            sectionWidth8 = Math.Abs(Properties.Vehicle.Default.setSection_position9 - Properties.Vehicle.Default.setSection_position8);
            sectionWidth9 = Math.Abs(Properties.Vehicle.Default.setSection_position10 - Properties.Vehicle.Default.setSection_position9);
            sectionWidth10 = Math.Abs(Properties.Vehicle.Default.setSection_position11 - Properties.Vehicle.Default.setSection_position10);
            sectionWidth11 = Math.Abs(Properties.Vehicle.Default.setSection_position12 - Properties.Vehicle.Default.setSection_position11);
            sectionWidth12 = Math.Abs(Properties.Vehicle.Default.setSection_position13 - Properties.Vehicle.Default.setSection_position12);
            sectionWidth13 = Math.Abs(Properties.Vehicle.Default.setSection_position14 - Properties.Vehicle.Default.setSection_position13);
            sectionWidth14 = Math.Abs(Properties.Vehicle.Default.setSection_position15 - Properties.Vehicle.Default.setSection_position14);
            sectionWidth15 = Math.Abs(Properties.Vehicle.Default.setSection_position16 - Properties.Vehicle.Default.setSection_position15);
            sectionWidth16 = Math.Abs(Properties.Vehicle.Default.setSection_position17 - Properties.Vehicle.Default.setSection_position16);

            nudSection1.Text = (sectionWidth1 * mf.mToUser).ToString("0");
            nudSection2.Text = (sectionWidth2 * mf.mToUser).ToString("0");
            nudSection3.Text = (sectionWidth3 * mf.mToUser).ToString("0");
            nudSection4.Text = (sectionWidth4 * mf.mToUser).ToString("0");
            nudSection5.Text = (sectionWidth5 * mf.mToUser).ToString("0");
            nudSection6.Text = (sectionWidth6 * mf.mToUser).ToString("0");
            nudSection7.Text = (sectionWidth7 * mf.mToUser).ToString("0");
            nudSection8.Text = (sectionWidth8 * mf.mToUser).ToString("0");
            nudSection9.Text = (sectionWidth9 * mf.mToUser).ToString("0");
            nudSection10.Text = (sectionWidth10 * mf.mToUser).ToString("0");
            nudSection11.Text = (sectionWidth11 * mf.mToUser).ToString("0");
            nudSection12.Text = (sectionWidth12 * mf.mToUser).ToString("0");
            nudSection13.Text = (sectionWidth13 * mf.mToUser).ToString("0");
            nudSection14.Text = (sectionWidth14 * mf.mToUser).ToString("0");
            nudSection15.Text = (sectionWidth15 * mf.mToUser).ToString("0");
            nudSection16.Text = (sectionWidth16 * mf.mToUser).ToString("0");

            //based on number of sections and values update the page before displaying
            UpdateSpinners();
        }

        public override void Close()
        {
            if (somethingChanged)
            {
                //take the section widths and convert to meters and positions along tool.
                CalculateSectionPositions();

                //save the values in each spinner for section position widths in settings
                Properties.Vehicle.Default.setSection_position1 = sectionPosition1;
                Properties.Vehicle.Default.setSection_position2 = sectionPosition2;
                Properties.Vehicle.Default.setSection_position3 = sectionPosition3;
                Properties.Vehicle.Default.setSection_position4 = sectionPosition4;
                Properties.Vehicle.Default.setSection_position5 = sectionPosition5;
                Properties.Vehicle.Default.setSection_position6 = sectionPosition6;
                Properties.Vehicle.Default.setSection_position7 = sectionPosition7;
                Properties.Vehicle.Default.setSection_position8 = sectionPosition8;
                Properties.Vehicle.Default.setSection_position9 = sectionPosition9;
                Properties.Vehicle.Default.setSection_position10 = sectionPosition10;
                Properties.Vehicle.Default.setSection_position11 = sectionPosition11;
                Properties.Vehicle.Default.setSection_position12 = sectionPosition12;
                Properties.Vehicle.Default.setSection_position13 = sectionPosition13;
                Properties.Vehicle.Default.setSection_position14 = sectionPosition14;
                Properties.Vehicle.Default.setSection_position15 = sectionPosition15;
                Properties.Vehicle.Default.setSection_position16 = sectionPosition16;
                Properties.Vehicle.Default.setSection_position17 = sectionPosition17;

                //turn section buttons all OFF
                for (int j = 0; j < FormGPS.MAXSECTIONS; j++)
                {
                    mf.section[j].sectionOnRequest = false;
                    mf.section[j].isSectionOn = false;
                    if (mf.section[j].isMappingOn)
                        mf.section[j].TurnMappingOff();
                    mf.section[j].UpdateButton(btnStates.Off);
                }

                if (mf.tool.numOfSections != numberOfSections)
                {
                    mf.tool.numOfSections = numberOfSections;
                    mf.tool.numSuperSection = numberOfSections + 1;

                    Properties.Vehicle.Default.setVehicle_numSections = mf.tool.numOfSections;

                    //line up manual buttons based on # of sections
                    mf.LineUpManualBtns();
                }

                mf.tram.isOuter = ((int)(mf.tram.tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0;

                //update the sections to newly configured widths and positions in main
                mf.SectionSetPosition();
            }


            Properties.Vehicle.Default.setVehicle_slowSpeedCutoff = mf.vehicle.slowSpeedCutoff = cutoffSpeed;
            Properties.Vehicle.Default.setVehicle_minCoverage = mf.tool.minOverlap = minCoverage;
            Properties.Vehicle.Default.setTool_defaultSectionWidth = defaultSectionWidth;
            Properties.Vehicle.Default.setSection_isFast = mf.isFastSections = cboxSectionResponse.Checked;

            Properties.Vehicle.Default.Save();
        }

        private void nudSection1_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection1, ref sectionWidth1, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection1, ref sectionWidth1);
        }

        private void nudSection2_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection2, ref sectionWidth2, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection2, ref sectionWidth2);
        }

        private void nudSection3_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection3, ref sectionWidth3, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection3, ref sectionWidth3);
        }

        private void nudSection4_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection4, ref sectionWidth4, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection4, ref sectionWidth4);
        }

        private void nudSection5_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection5, ref sectionWidth5, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection5, ref sectionWidth5);
        }
        private void nudSection6_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection6, ref sectionWidth6, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection6, ref sectionWidth6);
        }

        private void nudSection7_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection7, ref sectionWidth7, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection7, ref sectionWidth7);
        }

        private void nudSection8_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection8, ref sectionWidth8, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection8, ref sectionWidth8);
        }

        private void nudSection9_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection9, ref sectionWidth9, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection9, ref sectionWidth9);
        }

        private void nudSection10_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection10, ref sectionWidth10, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection10, ref sectionWidth10);
        }

        private void nudSection11_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection11, ref sectionWidth11, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection11, ref sectionWidth11);
        }

        private void nudSection12_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection12, ref sectionWidth12, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection12, ref sectionWidth12);
        }

        private void nudSection13_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection13, ref sectionWidth13, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection13, ref sectionWidth13);
        }

        private void nudSection14_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection14, ref sectionWidth14, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection14, ref sectionWidth14);
        }

        private void nudSection15_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection15, ref sectionWidth15, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection15, ref sectionWidth15);
        }

        private void nudSection16_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref nudSection16, ref sectionWidth16, 0.01, 50, 0, mf.mToUser, mf.userToM))
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection16, ref sectionWidth16);
        }

        private void nudSection1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudSectionWidth, gStr.gsHelp);
        }

        private void nudCutoffSpeed_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudCutoffSpeed, gStr.gsHelp);
        }

        private void nudCutoffSpeed_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudCutoffSpeed, ref cutoffSpeed, 0, 30, 1, mf.KMHToUser, mf.userToKMH);
        }

        private void nudMinCoverage_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudMinCoverage, gStr.gsHelp);
        }

        private void nudMinCoverage_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudMinCoverage, ref minCoverage, 0, 100);
        }

        private void cboxSectionResponse_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxSectionResponse, gStr.gsHelp);
        }

        private void cboxNumSections_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_cboxNumSections, gStr.gsHelp);
        }

        private void btnNumSections_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToButton(ref btnNumSections, ref numberOfSections, 0, FormGPS.MAXSECTIONS - 1))
            {
                double wide = defaultSectionWidth;

                if (numberOfSections * wide > 48)
                {
                    if (mf.isMetric)
                        mf.TimedMessageBox(3000, "Too Wide", "Max 48 Meters");
                    else
                        mf.TimedMessageBox(3000, "Too Wide", "Max 157.48 Feet");
                    wide = 48.0 / numberOfSections;
                }

                UpdateSpinners();

                sectionWidth1 = sectionWidth2 = sectionWidth3 = sectionWidth4 = wide;
                sectionWidth5 = sectionWidth6 = sectionWidth7 = sectionWidth8 = wide;
                sectionWidth9 = sectionWidth10 = sectionWidth11 = sectionWidth12 = wide;
                sectionWidth13 = sectionWidth14 = sectionWidth15 = sectionWidth16 = wide;

                //update in settings dialog ONLY total tool width
                SectionFeetInchesTotalWidthLabelUpdate(ref nudSection1, ref sectionWidth1);

                nudSection1.Text = (sectionWidth1 * mf.mToUser).ToString("0");
                nudSection2.Text = (sectionWidth2 * mf.mToUser).ToString("0");
                nudSection3.Text = (sectionWidth3 * mf.mToUser).ToString("0");
                nudSection4.Text = (sectionWidth4 * mf.mToUser).ToString("0");
                nudSection5.Text = (sectionWidth5 * mf.mToUser).ToString("0");
                nudSection6.Text = (sectionWidth6 * mf.mToUser).ToString("0");
                nudSection7.Text = (sectionWidth7 * mf.mToUser).ToString("0");
                nudSection8.Text = (sectionWidth8 * mf.mToUser).ToString("0");
                nudSection9.Text = (sectionWidth9 * mf.mToUser).ToString("0");
                nudSection10.Text = (sectionWidth10 * mf.mToUser).ToString("0");
                nudSection11.Text = (sectionWidth11 * mf.mToUser).ToString("0");
                nudSection12.Text = (sectionWidth12 * mf.mToUser).ToString("0");
                nudSection13.Text = (sectionWidth13 * mf.mToUser).ToString("0");
                nudSection14.Text = (sectionWidth14 * mf.mToUser).ToString("0");
                nudSection15.Text = (sectionWidth15 * mf.mToUser).ToString("0");
                nudSection16.Text = (sectionWidth16 * mf.mToUser).ToString("0");
            }
        }

        private void nudDefaultSectionWidth_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hc_nudDefaultSectionWidth, gStr.gsHelp);
        }

        private void nudDefaultSectionWidth_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudDefaultSectionWidth, ref defaultSectionWidth, 0.01, 50, 0, mf.mToUser, mf.userToM);
        }

        public void UpdateSpinners()
        {
            nudSection1.Enabled = nudSection1.Visible = numberOfSections > 0;
            nudSection2.Enabled = nudSection2.Visible = numberOfSections > 1;
            nudSection3.Enabled = nudSection3.Visible = numberOfSections > 2;
            nudSection4.Enabled = nudSection4.Visible = numberOfSections > 3;
            nudSection5.Enabled = nudSection5.Visible = numberOfSections > 4;
            nudSection6.Enabled = nudSection6.Visible = numberOfSections > 5;
            nudSection7.Enabled = nudSection7.Visible = numberOfSections > 6;
            nudSection8.Enabled = nudSection8.Visible = numberOfSections > 7;
            nudSection9.Enabled = nudSection9.Visible = numberOfSections > 8;
            nudSection10.Enabled = nudSection10.Visible = numberOfSections > 9;
            nudSection11.Enabled = nudSection11.Visible = numberOfSections > 10;
            nudSection12.Enabled = nudSection12.Visible = numberOfSections > 11;
            nudSection13.Enabled = nudSection13.Visible = numberOfSections > 12;
            nudSection14.Enabled = nudSection14.Visible = numberOfSections > 13;
            nudSection15.Enabled = nudSection15.Visible = numberOfSections > 14;
            nudSection16.Enabled = nudSection16.Visible = numberOfSections > 15;
        }

        //update tool width label at bottom of window
        private void SectionFeetInchesTotalWidthLabelUpdate(ref Button nudSection, ref double section)
        {
            somethingChanged = true;
            double width = 0;
            if (numberOfSections > 0)
                width += sectionWidth1;
            if (numberOfSections > 1)
                width += sectionWidth2;
            if (numberOfSections > 2)
                width += sectionWidth3;
            if (numberOfSections > 3)
                width += sectionWidth4;
            if (numberOfSections > 4)
                width += sectionWidth5;
            if (numberOfSections > 5)
                width += sectionWidth6;
            if (numberOfSections > 6)
                width += sectionWidth7;
            if (numberOfSections > 7)
                width += sectionWidth8;
            if (numberOfSections > 8)
                width += sectionWidth9;
            if (numberOfSections > 9)
                width += sectionWidth10;
            if (numberOfSections > 10)
                width += sectionWidth11;
            if (numberOfSections > 11)
                width += sectionWidth12;
            if (numberOfSections > 12)
                width += sectionWidth13;
            if (numberOfSections > 13)
                width += sectionWidth14;
            if (numberOfSections > 14)
                width += sectionWidth15;
            if (numberOfSections > 15)
                width += sectionWidth16;

            if (width > 48)
            {
                if (mf.isMetric)
                    mf.TimedMessageBox(3000, "Too Wide", "Max 50 Meters");
                else
                    mf.TimedMessageBox(3000, "Too Wide", "Max 164 Feet");

                section = 48 - (width - section);
                nudSection.Text = (section * mf.mToUser).ToString("0");
            }

            if (mf.isMetric)
            {
                lblSecTotalWidthFeet.Text = (width * mf.mToUserBig).ToString("0.00") + mf.unitsFtM;
            }
            else
            {
                double toFeet = (width * mf.mToUserBig);
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }
        }

        //Convert section width to positions along toolbar
        private void CalculateSectionPositions()
        {
            int i = numberOfSections;

            switch (i)
            {
                case 1:
                    {
                        sectionPosition2 = sectionWidth1 / 2.0;
                        sectionPosition1 = sectionPosition2 * -1;
                        sectionPosition3 = 0;
                        sectionPosition4 = 0;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 2:
                    {
                        sectionPosition1 = sectionWidth1 * -1;
                        sectionPosition2 = 0;
                        sectionPosition3 = sectionWidth2;
                        sectionPosition4 = 0;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 3:
                    {
                        sectionPosition3 = sectionWidth2 / 2.0;
                        sectionPosition2 = sectionPosition3 * -1;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition4 = sectionPosition3 + sectionWidth3;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 4:
                    {
                        sectionPosition2 = sectionWidth2 * -1;
                        sectionPosition3 = 0;
                        sectionPosition4 = sectionWidth3;
                        sectionPosition5 = sectionWidth3 + sectionWidth4;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 5:
                    {
                        sectionPosition4 = sectionWidth3 / 2.0;
                        sectionPosition3 = sectionPosition4 * -1;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition5 = sectionPosition4 + sectionWidth4;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 6:
                    {
                        sectionPosition4 = 0;
                        sectionPosition3 = sectionWidth3 * -1;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition5 = sectionWidth4;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 7:
                    {
                        sectionPosition5 = sectionWidth4 / 2.0;
                        sectionPosition4 = sectionPosition5 * -1;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = 0;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 8:
                    {
                        sectionPosition5 = 0;
                        sectionPosition4 = sectionWidth4 * -1;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = 0;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 9:
                    {
                        sectionPosition6 = sectionWidth5 / 2.0;
                        sectionPosition5 = sectionPosition6 * -1;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = 0;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 10:
                    {
                        sectionPosition6 = 0;
                        sectionPosition5 = sectionWidth5 * -1;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition7 = sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = 0;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 11:
                    {
                        sectionPosition7 = sectionWidth6 / 2.0;
                        sectionPosition6 = sectionPosition7 * -1;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = 0;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }
                case 12:
                    {
                        sectionPosition7 = 0;
                        sectionPosition6 = sectionWidth6 * -1;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition8 = sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = sectionPosition12 + sectionWidth12;
                        sectionPosition14 = 0;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;
                        break;
                    }
                case 13:
                    {
                        sectionPosition8 = sectionWidth7 / 2.0;
                        sectionPosition7 = sectionPosition8 * -1;
                        sectionPosition6 = sectionPosition7 - sectionWidth6;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = sectionPosition12 + sectionWidth12;
                        sectionPosition14 = sectionPosition13 + sectionWidth13;
                        sectionPosition15 = 0;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;

                        break;
                    }

                case 14:
                    {
                        sectionPosition8 = 0;
                        sectionPosition7 = sectionWidth7 * -1;
                        sectionPosition6 = sectionPosition7 - sectionWidth6;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition9 = sectionWidth8;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = sectionPosition12 + sectionWidth12;
                        sectionPosition14 = sectionPosition13 + sectionWidth13;
                        sectionPosition15 = sectionPosition14 + sectionWidth14;
                        sectionPosition16 = 0;
                        sectionPosition17 = 0;
                        break;
                    }
                case 15:
                    {
                        sectionPosition9 = sectionWidth8 / 2.0;
                        sectionPosition8 = sectionPosition9 * -1;
                        sectionPosition7 = sectionPosition8 - sectionWidth7;
                        sectionPosition6 = sectionPosition7 - sectionWidth6;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition10 = sectionPosition9 + sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = sectionPosition12 + sectionWidth12;
                        sectionPosition14 = sectionPosition13 + sectionWidth13;
                        sectionPosition15 = sectionPosition14 + sectionWidth14;
                        sectionPosition16 = sectionPosition15 + sectionWidth15;
                        sectionPosition17 = 0;

                        break;
                    }


                case 16:
                    {
                        sectionPosition9 = 0;
                        sectionPosition8 = sectionWidth8 * -1;
                        sectionPosition7 = sectionPosition8 - sectionWidth7;
                        sectionPosition6 = sectionPosition7 - sectionWidth6;
                        sectionPosition5 = sectionPosition6 - sectionWidth5;
                        sectionPosition4 = sectionPosition5 - sectionWidth4;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition10 = sectionWidth9;
                        sectionPosition11 = sectionPosition10 + sectionWidth10;
                        sectionPosition12 = sectionPosition11 + sectionWidth11;
                        sectionPosition13 = sectionPosition12 + sectionWidth12;
                        sectionPosition14 = sectionPosition13 + sectionWidth13;
                        sectionPosition15 = sectionPosition14 + sectionWidth14;
                        sectionPosition16 = sectionPosition15 + sectionWidth15;
                        sectionPosition17 = sectionPosition16 + sectionWidth16;
                        break;
                    }
            }
        }
    }
}
