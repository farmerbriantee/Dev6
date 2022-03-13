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
        private byte mode = 0;

        public FormFilePicker(Form callingForm, byte Idx, string _fileList)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            mode = Idx;

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
            else if (Idx == 2)
            {
                btnDeleteField.Image = Properties.Resources.Trash;
                btnReturn.Image = Properties.Resources.SwitchOff;

                lvLines.Columns.AddRange(new ColumnHeader[] { chName });

                chName.Text = "Record Name";
                chName.Width = 960;

                string fieldDir = mf.fieldsDirectory + mf.currentFieldDirectory;

                string[] files = Directory.GetFiles(fieldDir);

                // Here we use the filename of all .rec files in the current field dir.
                // The path and postfix is stripped off.

                foreach (string file in files)
                {
                    if (file.EndsWith(".rec"))
                    {
                        string recordName = file.Replace(".rec", "").Replace(fieldDir, "").Replace("\\", "");
                        lvLines.Items.Add(new ListViewItem(recordName));
                    }
                }

                if (lvLines.Items.Count == 0)
                {
                    MessageBox.Show("No Recorded Paths", "Create A Path First",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
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

                                distance = Math.Pow((latStart - mf.pn.latitude), 2) + Math.Pow((lonStart - mf.pn.longitude), 2);
                                distance = Math.Sqrt(distance);
                                distance *= 100;

                                fileList.Add(fieldDirectory);
                                fileList.Add(Math.Round(distance, 3).ToString().PadLeft(10));
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
                }
                else continue;

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
                                        if (mf.isMetric)
                                        {
                                            area = (Math.Abs(area / 2)) * 0.0001;
                                        }
                                        else
                                        {
                                            area = (Math.Abs(area / 2)) * 0.00024711;
                                        }
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
                    else fileList.Add(Math.Round(area, 1).ToString().PadLeft(10));
                }

                else
                {
                    fileList.Add("Error");
                    MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                        "               \r\n Delete Field or Fix ", gStr.gsFileError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                filename = dir + "\\Field.txt";
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
                if (mode == 2)
                {
                    int count = lvLines.SelectedItems.Count;
                    if (count > 0)
                    {
                        string selectedRecordPath = mf.fieldsDirectory + mf.currentFieldDirectory + "\\" + lvLines.SelectedItems[0].SubItems[0].Text + ".rec";

                        // Copy the selected record file to the original record name inside the field dir:
                        // ( this will load the last selected path automatically when this field is opened again)
                        File.Copy(selectedRecordPath, mf.fieldsDirectory + mf.currentFieldDirectory + "\\RecPath.txt", true);
                        // and load the selected path into the recPath object:
                        string line;
                        if (File.Exists(selectedRecordPath))
                        {
                            using (StreamReader reader = new StreamReader(selectedRecordPath))
                            {
                                try
                                {
                                    //read header
                                    line = reader.ReadLine();
                                    line = reader.ReadLine();
                                    int numPoints = int.Parse(line);
                                    mf.gyd.recList.Clear();

                                    while (!reader.EndOfStream)
                                    {
                                        for (int v = 0; v < numPoints; v++)
                                        {
                                            line = reader.ReadLine();
                                            string[] words = line.Split(',');
                                            CRecPathPt point = new CRecPathPt(
                                                double.Parse(words[0], CultureInfo.InvariantCulture),
                                                double.Parse(words[1], CultureInfo.InvariantCulture),
                                                double.Parse(words[2], CultureInfo.InvariantCulture),
                                                double.Parse(words[3], CultureInfo.InvariantCulture),
                                                bool.Parse(words[4]));

                                            //add the point
                                            mf.gyd.recList.Add(point);
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    var form = new FormTimedMessage(2000, gStr.gsRecordedPathFileIsCorrupt, gStr.gsButFieldIsLoaded);
                                    form.Show(this);
                                    mf.WriteErrorLog("Load Recorded Path" + ex.ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (lvLines.SelectedItems[0].SubItems[1].Text == "Error" || lvLines.SelectedItems[0].SubItems[2].Text == "Error")
                    {
                        MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.gsFileError,
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        mf.filePickerFileAndDirectory = mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text + "\\Field.txt";
                        Close();
                    }
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (mode == 2)
            {
                mf.gyd.StopDrivingRecordedPath();
                mf.gyd.recList.Clear();
                mf.FileSaveRecPath();
                mf.panelDrag.Visible = false;
                Close();
            }
            else
                mf.filePickerFileAndDirectory = "";
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                string selectedRecord = lvLines.SelectedItems[0].SubItems[0].Text;
                string dir2Delete = mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text;
                if (mode == 2)
                    dir2Delete = mf.fieldsDirectory + mf.currentFieldDirectory + "\\" + selectedRecord + ".rec";


                DialogResult result3 = MessageBox.Show(
                    dir2Delete,
                    gStr.gsDeleteForSure,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    if (mode == 2)
                        File.Delete(dir2Delete);
                    else
                        Directory.Delete(dir2Delete, true);

                    lvLines.Items.Remove(lvLines.SelectedItems[0]);
                }
            }
        }

        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;

            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor. Initializes various elements
            /// </summary>
            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
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