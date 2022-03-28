using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigSourceData : UserControl2
    {
        private readonly ConfigNTRIP mf;
        
        private readonly List<string> dataList = new List<string>();
        private readonly double lat, lon;
        private readonly string site;

        public ConfigSourceData(ConfigNTRIP callingForm, List<string> _dataList, double _lat, double _lon, string syte)
        {
            mf = callingForm;

            InitializeComponent();
            dataList = _dataList;
            lat = _lat;
            lon = _lon;
            site = syte;
        }

        private void ConfigSourceData_Load(object sender, System.EventArgs e)
        {
            ListViewItem itm;
            if (dataList.Count > 0)
            {
                double temp;

                for (int i = 0; i < dataList.Count; i++)
                {
                    string[] data = dataList[i].Split(',');
                    double.TryParse(data[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double cLat);
                    double.TryParse(data[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double cLon);

                    if (cLat == 0 || cLon == 0)
                    {
                        temp = double.MaxValue;
                    }
                    else
                    {
                        temp = GetDistance(cLon, cLat, lon, lat);
                        temp *= .001;
                    }
                    //load up the listview
                    string[] fieldNames = { (temp == double.MaxValue ? "******" : temp.ToString("#######").PadLeft(10)),data[0].Trim(), data[1].Trim(),
                                                    data[2].Trim(), data[3].Trim(), data[4].Trim() };
                    itm = new ListViewItem(fieldNames);
                    lvLines.Items.Add(itm);
                }
            }
        }

        private void btnUseMount_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                mf.tboxMount.Text = lvLines.SelectedItems[0].SubItems[1].Text;
                mf.Controls.Remove(mf.currentTab);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mf.Controls.Remove(mf.currentTab);
        }

        private void btnSite_Click(object sender, EventArgs e)
        {
            Process.Start(site);
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                tboxMount3.Text = (lvLines.SelectedItems[0].SubItems[1].Text);
            }
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            double d1 = latitude * (Math.PI / 180.0);
            double num1 = longitude * (Math.PI / 180.0);
            double d2 = otherLatitude * (Math.PI / 180.0);
            double num2 = otherLongitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
