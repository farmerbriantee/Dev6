using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEnterAB : Form
    {
        private readonly FormGPS mf = null;

        private bool isAB = true;
        public string headingText = string.Empty;
        public FormEnterAB(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = gStr.gsEditABLine;
            nudLatitude.Controls[0].Enabled = false;
            nudLongitude.Controls[0].Enabled = false;
            nudLatitudeB.Controls[0].Enabled = false;
            nudLatitudeB.Controls[0].Enabled = false;
            nudHeading.Controls[0].Enabled = false;

            nudLatitude.Value = (decimal)mf.mc.latitude;
            nudLatitudeB.Value = (decimal)mf.mc.latitude + 0.000001m;
            nudLongitude.Value = (decimal)mf.mc.longitude;
            nudLongitudeB.Value = (decimal)mf.mc.longitude + 0.000001m;
        }

        private void FormEnterAB_Load(object sender, EventArgs e)
        {
            btnEnterManual.Focus();
            headingText = textBox1.Text = "Create A New Line";

        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnEnterManual.Focus();
            CalcHeading();
        }

        private void btnChooseType_Click(object sender, EventArgs e)
        {
            isAB = !isAB;
            if (isAB)
            {
                nudLatitudeB.Enabled = true;
                nudLongitudeB.Enabled = true;
                nudHeading.Enabled = false;
                nudLatitudeB.Visible = true;
                nudLongitudeB.Visible = true;
                nudHeading.Visible = false;
                label4.Visible = false;
                label5.Visible = true;
            }
            else
            {
                nudLatitudeB.Enabled = false;
                nudLongitudeB.Enabled = false;
                nudHeading.Enabled = true;
                nudLatitudeB.Visible = false;
                nudLongitudeB.Visible = false;
                nudHeading.Visible = true;
                label4.Visible = true;
                label5.Visible = false;
            }
            CalcHeading();
        }

        private void nudLatitude_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnEnterManual.Focus();
            CalcHeading();
        }

        private void nudLongitude_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnEnterManual.Focus();
            CalcHeading();
        }

        private void nudLatitudeB_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnEnterManual.Focus();
            CalcHeading();
        }

        private void nudLongitudeB_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnEnterManual.Focus();
            CalcHeading();
        }

        private void btnEnterManual_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Create A New Line")
                this.DialogResult = DialogResult.Cancel;
            Close();
        }

        public void CalcHeading()
        {
            if (mf.gyd.EditGuidanceLine != null)
            {
                mf.worldManager.ConvertWGS84ToLocal((double)nudLatitude.Value, (double)nudLongitude.Value, out double nort, out double east);
                double heading;
                double nort2, east2;

                if (isAB)
                {
                    mf.worldManager.ConvertWGS84ToLocal((double)nudLatitudeB.Value, (double)nudLongitudeB.Value, out nort2, out east2);

                    // heading based on AB points
                    heading = Math.Atan2(east2 - east, nort2 - nort);
                    if (heading < 0) heading += glm.twoPI;

                    nudHeading.Value = (decimal)(glm.toDegrees(heading));
                }
                else
                {
                    heading = glm.toRadians((double)nudHeading.Value);
                    nort2 = nort + Math.Cos(heading);
                    east2 = east + Math.Sin(heading);
                }

                if (mf.gyd.EditGuidanceLine.points.Count > 1)
                {
                    mf.gyd.EditGuidanceLine.points[0] = new vec2(east, nort);
                    mf.gyd.EditGuidanceLine.points[1] = new vec2(east2, nort2);
                }
                else if (mf.gyd.EditGuidanceLine.points.Count > 0)
                {
                    mf.gyd.EditGuidanceLine.points[0] = new vec2(east, nort);
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(east2, nort2));
                }
                else
                {
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(east, nort));
                    mf.gyd.EditGuidanceLine.points.Add(new vec2(east2, nort2));
                }

                headingText = textBox1.Text = "Manual AB " +
                    (Math.Round(glm.toDegrees(heading), 1)).ToString(CultureInfo.InvariantCulture) +
                    "\u00B0 " + mf.FindDirection(heading);

                if (textBox1.Text != "Create A New Line") btnEnterManual.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
