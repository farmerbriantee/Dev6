using System;
using System.Collections.Generic;
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
            int scaleMin = 50, scaleRange = 200;
            double scale = r / mf.shape.scaleUpper;
            return (int)((scaleRange * scale) + scaleMin);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < mf.shape.dbfFieldCount; i++)
            //{
            //    var lvi = new ListViewItem
            //    {
            //        Text = mf.shape.dbfFieldNames[i],
            //        BackColor = Color.Red
            //    };
            //    lbColors.Items.Add(lvi);
            //}
        }

        private void FormShapeClassification_Load(object sender, EventArgs e)
        {
            lbColorRanges.SelectedIndex = 0;
            SetColors();
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        void SetColors()
        {
            lbColors.Items.Clear();
            Color bc = Color.FromArgb(50, 50, 50, 50);
            if (lbColorRanges.SelectedIndex < 0)
            {
                return;
            }
            string colorRanges = lbColorRanges.SelectedItem.ToString();
            foreach (var key in mf.shape.dbfUniqueRates)
            {
                switch (colorRanges)
                {
                    case "Red":
                        bc = Color.FromArgb(50, MapColor(key.Key), 0, 0);
                        break;
                    case "Green":
                        bc = Color.FromArgb(50, 0, MapColor(key.Key), 0);
                        break;
                    case "Blue":
                        bc = Color.FromArgb(50, 0, 0, MapColor(key.Key));
                        break;
                }
                var lvi = new ListViewItem
                {
                    Text = key.Key.ToString(),
                    BackColor = bc
                };
                if (key.Key > 0)
                {
                    lbColors.Items.Add(lvi);
                }

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
    }
}

