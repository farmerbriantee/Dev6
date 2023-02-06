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

        private void button1_Click(object sender, EventArgs e)
        {
            lblHeading.Text += mf.shape.dbfFieldCount.ToString();
            for(int i = 0; i < mf.shape.dbfFieldCount; i++)
            {
                var lvi = new ListViewItem
                {
                    Text = mf.shape.dbfFieldNames[i],
                    BackColor = Color.Red
                };
                lbColors.Items.Add(lvi);
            }

        }
    }
}
