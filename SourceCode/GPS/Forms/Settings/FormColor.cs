﻿//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColor : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool daySet;

        //constructor
        public FormColor(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = gStr.gsColors;
        }
        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            daySet = mf.isDay;
            hsbarOpacity.Value = Properties.Settings.Default.setDisplay_vehicleOpacity;
            hsbarSmooth.Value = Properties.Settings.Default.setDisplay_camSmooth;
            cboxIsImage.Checked = mf.isVehicleImage;
            lblOpacityPercent.Text = hsbarOpacity.Value.ToString() + "%";
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";

        }
        private void bntOK_Click(object sender, EventArgs e)
        {
            if (daySet != mf.isDay) mf.SwapDayNightMode();
            Properties.Settings.Default.setDisplay_vehicleOpacity = hsbarOpacity.Value;
            mf.vehicleOpacity = (hsbarOpacity.Value * 0.01);
            mf.vehicleOpacityByte = (byte)(255 * (hsbarOpacity.Value * 0.01));
            Properties.Settings.Default.setDisplay_camSmooth = hsbarSmooth.Value;

            mf.worldManager.camSmoothFactor = ((double)(hsbarSmooth.Value) * 0.004) + 0.15;

            mf.isVehicleImage = cboxIsImage.Checked;
            Properties.Settings.Default.setDisplay_isVehicleImage = cboxIsImage.Checked;
            Settings.Default.Save();
            Close();
        }

        private void btnVehicleColor_Click(object sender, EventArgs e)
        {
            using (FormColorPicker form = new FormColorPicker(mf, mf.vehicleColor))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.vehicleColor = form.useThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;
            Settings.Default.Save();
        }

        private void btnFrameDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.Default.setDisplay_colorDayFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.setDisplay_colorDayFrame = form.useThisColor;
                    Settings.Default.Save();
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFrameNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.Default.setDisplay_colorNightFrame))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.setDisplay_colorNightFrame = form.useThisColor;
                    Settings.Default.Save();
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.fieldColorDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.fieldColorDay = form.useThisColor;
                }
            }


            Settings.Default.setDisplay_colorFieldDay = mf.fieldColorDay;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.fieldColorNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.fieldColorNight = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorFieldNight = mf.fieldColorNight;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            mf.SwapDayNightMode();
        }

        private void btnNightText_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.Default.setDisplay_colorTextNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.setDisplay_colorTextNight = form.useThisColor;
                    Settings.Default.Save();
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnDayText_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, Settings.Default.setDisplay_colorTextDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.setDisplay_colorTextDay = form.useThisColor;
                    Settings.Default.Save();
                }
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void hsbarOpacity_ValueChanged(object sender, EventArgs e)
        {
            lblOpacityPercent.Text = hsbarOpacity.Value.ToString() + "%";
        }

        private void hsbarSmooth_ValueChanged(object sender, EventArgs e)
        {
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";
        }
    }
}