using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AgOpenGPS.Forms.Settings
{
    public partial class FormShapeClassification : Form
    {
        private readonly FormGPS mf = null;
        public FormShapeClassification(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        public int MapColor(double r)
        {
            if (r == 0)
            {
                return 50;
            }
            // more rate == darker output
            int scaleMin = 50, scaleRange = 155;
            // or inverted
            //double scale = r / mf.shape.scaleUpper;
            double scale = mf.shape.scaleLower / r;
            return (int)((scaleRange * scale) + scaleMin);
        }


        private void bntOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        void SetColors()
        {
            lbColors.Items.Clear();
            if (lbColorRanges.SelectedIndex < 0)
            {
                return;
            }
            string colorRanges = lbColorRanges.SelectedItem.ToString();
            int a = 50;
            Hashtable h = new Hashtable();
            Color bc = Color.FromArgb(255, 0, 0, 0); // meh
            foreach (var key in mf.shape.dbfUniqueRates)
            {
                switch (colorRanges)
                {
                    case "Red":
                        bc = Color.FromArgb(a, MapColor(key.Key), 0, 0);
                        break;
                    case "Green":
                        bc = Color.FromArgb(a, 0, MapColor(key.Key), 0);
                        break;
                    case "Blue":
                        bc = Color.FromArgb(a, 0, 0, MapColor(key.Key));
                        break;
                }
                h[key.Key] = bc;
                var lvi = new ListViewItem
                {
                    Text = key.Key.ToString(),
                    BackColor = bc
                };
                if (key.Key > 0)
                    lbColors.Items.Add(lvi);
            }
            // override the zero value
            h[0] = Color.FromArgb(255,50,50,50);

            // colours drawn, let's change the polygons according to lookup value
            foreach (RatePolyline r in mf.bnd.Rate)
            {
                if (r.rate > 0) 
                    r.color = (Color)h[r.rate];
                //Console.WriteLine($"Rate is {r.rate} and {h[r.rate]} ");
            }
        }
        private void lbColorRanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mf.shape.dbfFieldCount > 0)
            {
                SetColors();
            }
        }

        private void lbColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbColors.SelectedItems.Count > 0)
            {
                MessageBox.Show(lbColors.SelectedItems[0].Text + lbColors.Items[lbColors.SelectedIndices[0]].BackColor);
            }
        }
        private void FormShapeClassification_Load(object sender, EventArgs e)
        {
            lbColorRanges.SelectedIndex = 0;
            SetColors();
        }
    }
}

