﻿
using MechanikaDesign.WinForms.UI.ColorPicker;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColorPicker : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        private readonly Color inColor;
        public Color useThisColor { get; set; }

        private bool isUse = true;

        private HslColor colorHsl = HslColor.FromAhsl(0xff);
        private Color colorRgb = Color.Empty;


        public FormColorPicker(Form callingForm, Color _inColor)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            inColor = _inColor;

            btnNight.BackColor = inColor;
            btnDay.BackColor = inColor;

            useThisColor = inColor;

            UpdateColor(inColor);


            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void colorBox2D_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            this.colorRgb = colorBox2D.ColorRGB;
            HslColor colorHSL = HslColor.FromColor(colorRgb);

            this.colorHsl = colorHSL;
            this.colorSlider.ColorHSL = this.colorHsl;

            useThisColor = colorRgb;
            btnNight.BackColor = colorRgb;
            btnDay.BackColor = colorRgb;
        }

        private void colorSlider_ColorChanged(object sender, MechanikaDesign.WinForms.UI.ColorPicker.ColorChangedEventArgs args)
        {
            HslColor colorHSL = this.colorSlider.ColorHSL;
            this.colorHsl = colorHSL;
            this.colorRgb = this.colorHsl.RgbValue;
            this.colorBox2D.ColorHSL = this.colorHsl;

            useThisColor = colorRgb;
            btnNight.BackColor = colorRgb;
            btnDay.BackColor = colorRgb;
        }

        private void UpdateColor(Color col)
        {
            this.colorHsl = HslColor.FromColor(col);
            this.colorRgb = col;
            this.colorSlider.ColorHSL = this.colorHsl;
            this.colorBox2D.ColorHSL = this.colorHsl;

            useThisColor = col;
            btnNight.BackColor = col;
            btnDay.BackColor = col;
        }

        private void FormColorPicker_Load(object sender, EventArgs e)
        {
            btn00.BackColor = Color.FromArgb(mf.customColorsList[0]);
            btn01.BackColor = Color.FromArgb(mf.customColorsList[1]);
            btn02.BackColor = Color.FromArgb(mf.customColorsList[2]);
            btn03.BackColor = Color.FromArgb(mf.customColorsList[3]);
            btn04.BackColor = Color.FromArgb(mf.customColorsList[4]);
            btn05.BackColor = Color.FromArgb(mf.customColorsList[5]);
            btn06.BackColor = Color.FromArgb(mf.customColorsList[6]);
            btn07.BackColor = Color.FromArgb(mf.customColorsList[7]);
            btn08.BackColor = Color.FromArgb(mf.customColorsList[8]);
            btn09.BackColor = Color.FromArgb(mf.customColorsList[9]);
            btn10.BackColor = Color.FromArgb(mf.customColorsList[10]);
            btn11.BackColor = Color.FromArgb(mf.customColorsList[11]);
            btn12.BackColor = Color.FromArgb(mf.customColorsList[12]);
            btn13.BackColor = Color.FromArgb(mf.customColorsList[13]);
            btn14.BackColor = Color.FromArgb(mf.customColorsList[14]);
            btn15.BackColor = Color.FromArgb(mf.customColorsList[15]);
            
            //make sure no colors stored have 255
            for (int i = 0; i < 16; i++)
            {
                Color test = Color.FromArgb(mf.customColorsList[i]);
                int iCol = (test.A << 24) | (test.R << 16) | (test.G << 8) | test.B;
                mf.customColorsList[i] = iCol;
            }

            //save them just in case
            SaveCustomColor();
        }

        private void SaveCustomColor()
        {
            Properties.Settings.Default.setDisplay_customColors = "";
            for (int i = 0; i < 15; i++)
                Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[i].ToString() + ",";
            Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[15].ToString();

            Properties.Settings.Default.Save();
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            if (isUse)
            {
                UpdateColor(butt.BackColor);
            }
            else
            {

                int.TryParse(butt.Name.Substring(3, 2), out int buttNumber);

                int iCol = (useThisColor.A << 24) | (useThisColor.R << 16) | (useThisColor.G << 8) | useThisColor.B;
                mf.customColorsList[buttNumber] = iCol;
                butt.BackColor = useThisColor;

                SaveCustomColor();
            }
        }

        private void chkUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse.Checked)
            {
                groupBox1.Text = "Pick New Color and Select Square Below to Save Preset";
                chkUse.Image = Properties.Resources.ColorUnlocked;
                isUse = false;
            }
            else
            {
                isUse = true;
                groupBox1.Text = "Select Preset Color";
                chkUse.Image = Properties.Resources.ColorLocked;
            }
        }
    }
}