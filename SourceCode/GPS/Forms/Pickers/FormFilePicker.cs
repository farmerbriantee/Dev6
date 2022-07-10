using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFilePicker : Form
    {
        private readonly FormGPS mf;

        private readonly List<string> fileList = new List<string>();
        private ListViewColumnSorter lvwColumnSorter;

        public FormFilePicker(Form callingForm, byte Idx, string _fileList)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnOpenExistingLv.Text = gStr.gsUseSelected;
            
            ColumnHeader chName = new ColumnHeader();

            if (Idx == 0)
            {
                ColumnHeader chDistance = new ColumnHeader();
                ColumnHeader chArea = new ColumnHeader();

                chName.Text = "Field Name";
                chName.Width = 680;

                chDistance.Text = "Distance";
                chDistance.Width = 140;

                chArea.Text = "Area";
                chArea.Width = 140;

                lvLines.Columns.AddRange(new ColumnHeader[] { chName, chDistance, chArea });

                LoadList();
            }
            else if (Idx == 1)
            {
                lvLines.Columns.AddRange(new ColumnHeader[] { chName });

                chName.Text = "Field Name";
                chName.Width = 960;


                string[] fileList = _fileList.Split(',');
                for (int i = 0; i < fileList.Length; i++)
                {
                    lvLines.Items.Add(new ListViewItem(fileList[i]));
                }
            }

            lvwColumnSorter = new ListViewColumnSorter();
            lvLines.ListViewItemSorter = lvwColumnSorter;
        }

        private void LoadList()
        {
            string[] dirs = Directory.GetDirectories(mf.fieldsDirectory);

            if (dirs == null || dirs.Length < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsCreateNewField, gStr.gsFileError);
                Close();
                return;
            }

            foreach (string dir in dirs)
            {
                double latStart = 0;
                double lonStart = 0;
                double distance = 0;
                string fieldDirectory = Path.GetFileName(dir);
                string filename = dir + "\\Field.txt";
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                latStart = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lonStart = (double.Parse(offs[1], CultureInfo.InvariantCulture));

                                distance = Math.Pow((latStart - mf.mc.latitude), 2) + Math.Pow((lonStart - mf.mc.longitude), 2);
                                distance = Math.Sqrt(distance);
                                distance *= 100;

                                fileList.Add(fieldDirectory);
                                fileList.Add(distance.ToString("0.000").PadLeft(10));
                            }
                            else
                            {
                                MessageBox.Show(fieldDirectory + " is Damaged, Please Delete This Field", gStr.gsFileError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                fileList.Add(fieldDirectory);
                                fileList.Add("Error");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(fieldDirectory + " is Damaged, Please Delete, Field.txt is Broken", gStr.gsFileError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                            fileList.Add(fieldDirectory);
                            fileList.Add("Error");
                        }
                    }

                    //grab the boundary area
                    filename = dir + "\\Boundary.txt";
                    if (File.Exists(filename))
                    {
                        List<vec2> pointList = new List<vec2>();
                        double area = 0;

                        using (StreamReader reader = new StreamReader(filename))
                        {
                            try
                            {
                                //read header
                                line = reader.ReadLine();//Boundary

                                if (!reader.EndOfStream)
                                {
                                    //True or False OR points from older boundary files
                                    line = reader.ReadLine();

                                    //Check for older boundary files, then above line string is num of points
                                    if (line == "True" || line == "False")
                                    {
                                        line = reader.ReadLine(); //number of points
                                    }

                                    //Check for latest boundary files, then above line string is num of points
                                    if (line == "True" || line == "False")
                                    {
                                        line = reader.ReadLine(); //number of points
                                    }

                                    int numPoints = int.Parse(line);

                                    if (numPoints > 0)
                                    {
                                        //load the line
                                        for (int i = 0; i < numPoints; i++)
                                        {
                                            line = reader.ReadLine();
                                            string[] words = line.Split(',');
                                            pointList.Add(new vec2(
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[1], CultureInfo.InvariantCulture)));
                                        }

                                        int ptCount = pointList.Count;
                                        if (ptCount > 5)
                                        {
                                            area = 0;         // Accumulates area in the loop
                                            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                                            for (int i = 0; i < ptCount; j = i++)
                                            {
                                                area += (pointList[j].easting + pointList[i].easting) * (pointList[j].northing - pointList[i].northing);
                                            }
                                            area = Math.Abs(area / 2) * glm.m2ToUser;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                area = 0;
                            }
                        }
                        if (area == 0) fileList.Add("No Bndry");
                        else fileList.Add(area.ToString("0.0").PadLeft(10));
                    }
                    else
                    {
                        fileList.Add("No Bndry");
                    }
                }
            }

            if (fileList == null || fileList.Count < 3)
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Close();
                return;
            }

            for (int i = 0; i < fileList.Count; i += 3)
            {
                string[] fieldNames = { fileList[i], fileList[i + 1], fileList[i + 2] };
                lvLines.Items.Add(new ListViewItem(fieldNames));
            }

            if (lvLines.Items.Count == 0)
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Close();
                return;
            }
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                if (lvLines.SelectedItems[0].SubItems[1].Text == "Error" || lvLines.SelectedItems[0].SubItems[2].Text == "Error")
                {
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.gsFileError,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    mf.currentFieldDirectory = lvLines.SelectedItems[0].SubItems[0].Text;
                    mf.FileOpenField();
                    Close();
                }
            }
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                string selectedRecord = lvLines.SelectedItems[0].SubItems[0].Text;
                string dir2Delete = mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text;

                if (new FormHelp(dir2Delete, gStr.gsDeleteForSure, true).ShowDialog(this) == DialogResult.Yes)
                {
                    Directory.Delete(dir2Delete, true);

                    lvLines.Items.Remove(lvLines.SelectedItems[0]);
                }
            }
        }

        private void lvLines_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvLines.Sort();
        }
    }
}