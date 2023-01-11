using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Diagnostics;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for that entire section activity
        public List<Polyline2> patchList = new List<Polyline2>();

        public List<CAutoLoadField> Fields = new List<CAutoLoadField>();

        //list of the list of patch data individual triangles for field sections
        public List<List<vec2>> patchSaveList = new List<List<vec2>>();

        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec2>> contourSaveList = new List<List<vec2>>();

        public void FileSaveCurveLines()
        {
            gyd.moveDistance = 0;

            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            int cnt = gyd.curveArr.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    writer.WriteLine("$CurveLines," + currentVersionStr);

                    for (int i = 0; i < gyd.curveArr.Count; i++)
                    {
                        if (gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                        {
                            //write out the Name
                            writer.WriteLine(gyd.curveArr[i].Name.Trim());

                            //write out the aveheading
                            writer.WriteLine(0.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = gyd.curveArr[i].points.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (gyd.curveArr[i].points.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(gyd.curveArr[i].points[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(gyd.curveArr[i].points[j].northing, 3).ToString(CultureInfo.InvariantCulture) + ",0");
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    WriteErrorLog("Saving Curve Line" + er.ToString());

                    return;
                }
            }
        }

        public void FileLoadCurveLines()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$CurveLines," + currentVersionStr);
                }
            }

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                this.TimedMessageBox(2000, gStr.gsFileError, "Missing Curve File");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            CGuidanceLine New = new CGuidanceLine(Mode.Curve);

                            //read header $CurveLine
                            string text = reader.ReadLine();
                            while (gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                                text += " ";
                            New.Name = text;

                            if (New.Name == "Boundary Curve")
                                New.loop = true;

                            // get the average heading
                            line = reader.ReadLine();
                            //aveHeading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 1)
                            {
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    New.points.Add(new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture)));
                                }
                                gyd.curveArr.Add(New);
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        this.TimedMessageBox(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        WriteErrorLog("Load Curve Line" + er.ToString());
                    }
                }
            }
        }

        public void FileSaveABLines()
        {
            gyd.moveDistance = 0;

            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                foreach (var item in gyd.curveArr)
                {
                    if (item.mode.HasFlag(Mode.AB) && item.points.Count > 1)
                    {
                        double heading = Math.Atan2(item.points[1].easting - item.points[0].easting, item.points[1].northing - item.points[0].northing);

                        //make it culture invariant
                        string line = item.Name.Trim()
                            + ',' + (Math.Round(glm.toDegrees(heading), 8)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.points[0].easting, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.points[0].northing, 3)).ToString(CultureInfo.InvariantCulture);

                        //write out to file
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public void FileLoadABLines()
        {
            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                }
            }

            if (!File.Exists(filename))
            {
                this.TimedMessageBox(2000, gStr.gsFileError, gStr.gsMissingABLinesFile);
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {
                            line = reader.ReadLine();
                            string[] words = line.Split(',');

                            if (words.Length != 4) break;

                            CGuidanceLine New = new CGuidanceLine(Mode.AB);

                            string text = words[0];
                            while (gyd.curveArr.Exists(L => L.Name == text))//generate unique name!
                                text += " ";
                            New.Name = text;

                            double heading = glm.toRadians(double.Parse(words[1], CultureInfo.InvariantCulture));

                            New.points.Add(new vec2(double.Parse(words[2], CultureInfo.InvariantCulture), double.Parse(words[3], CultureInfo.InvariantCulture)));
                            New.points.Add(new vec2(New.points[0].easting + Math.Sin(heading), New.points[0].northing + Math.Cos(heading)));

                            gyd.curveArr.Add(New);
                        }
                    }
                    catch (Exception er)
                    {
                        this.TimedMessageBox(2000, "AB Line Corrupt", "Please delete it!!!");
                        WriteErrorLog("FieldOpen, Loading ABLine, Corrupt ABLine File" + er);
                    }
                }
            }
        }

        //function to open a previously saved field, resume, open exisiting, open named field
        public void FileOpenField()
        {
            string fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Field.txt";

            if (!File.Exists(fileAndDirectory)) return;

            //and open a new job
            this.JobNew();

            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //start to read the file
            string line;
            using (StreamReader reader = new StreamReader(fileAndDirectory))
            {
                try
                {
                    //Date time line
                    line = reader.ReadLine();

                    //dir header $FieldDir
                    line = reader.ReadLine();

                    //read field directory
                    line = reader.ReadLine();

                    displayFieldName = line.Trim();

                    //Offset header
                    line = reader.ReadLine();

                    //read the Offsets 
                    line = reader.ReadLine();
                    string[] offs = line.Split(',');

                    //convergence angle update
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                    }

                    //start positions
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        offs = line.Split(',');

                        worldManager.latStart = double.Parse(offs[0], CultureInfo.InvariantCulture);
                        worldManager.lonStart = double.Parse(offs[1], CultureInfo.InvariantCulture);

                        if (glm.isSimEnabled)
                        {
                            Properties.Settings.Default.setGPS_SimLatitude = worldManager.latStart;
                            Properties.Settings.Default.setGPS_SimLongitude = worldManager.lonStart;
                            Properties.Settings.Default.Save();

                            sim.resetSim();
                        }

                        worldManager.SetLocalMetersPerDegree();
                    }
                }

                catch (Exception e)
                {
                    WriteErrorLog("While Opening Field" + e.ToString());

                    this.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                    JobClose();
                    return;
                }
            }

            FileLoadABLines();
            FileLoadCurveLines();

            gyd.currentCurveLine = gyd.curveArr.Find(x => x.mode.HasFlag(Mode.Curve));
            gyd.currentABLine = gyd.curveArr.Find(x => x.mode.HasFlag(Mode.AB));

            //section patches
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Sections.txt";
            if (!File.Exists(fileAndDirectory))
            {
                this.TimedMessageBox(2000, gStr.gsMissingSectionFile, gStr.gsButFieldIsLoaded);
                //return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        bnd.workedAreaTotal = 0;
                        bnd.distanceUser = 0;

                        //read header
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            Polyline2 New = new Polyline2();

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                New.points.Add(new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture)));

                                if (v == 0)
                                {
                                    if (words.Length == 4)
                                    {
                                        New.points.Add(new vec2(double.Parse(words[2], CultureInfo.InvariantCulture),
                                            int.Parse(words[3], CultureInfo.InvariantCulture)));
                                    }
                                    else
                                    {
                                        New.points.Add(new vec2(double.Parse(words[2], CultureInfo.InvariantCulture), 0));
                                    }
                                }
                            }

                            //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                            verts = New.points.Count - 2;
                            if (verts >= 2)
                            {
                                for (int j = 2; j < verts; j++)
                                {
                                    double temp = 0;
                                    temp = New.points[j].easting * (New.points[j + 1].northing - New.points[j + 2].northing) +
                                              New.points[j + 1].easting * (New.points[j + 2].northing - New.points[j].northing) +
                                                  New.points[j + 2].easting * (New.points[j].northing - New.points[j + 1].northing);

                                    bnd.workedAreaTotal += Math.Abs((temp * 0.5));
                                }
                            }
                            if (New.points.Count > 4)
                                patchList.Add(New);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Section file" + e.ToString());

                        this.TimedMessageBox(2000, "Section File is Corrupt", gStr.gsButFieldIsLoaded);
                    }

                }
            }

            // Contour points ----------------------------------------------------------------------------

            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Contour.txt";
            if (!File.Exists(fileAndDirectory))
            {
                this.TimedMessageBox(2000, gStr.gsMissingContourFile, gStr.gsButFieldIsLoaded);
                //return;
            }

            //Points in Patch followed by easting, heading, northing, altitude
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            //read how many vertices in the following patch
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            CGuidanceLine New = new CGuidanceLine(Mode.Contour);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                New.points.Add(new vec2(double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture)));
                            }

                            gyd.curveArr.Add(New);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Loading Contour file" + e.ToString());

                        this.TimedMessageBox(2000, gStr.gsContourFileIsCorrupt, gStr.gsButFieldIsLoaded);
                    }
                }
            }


            // Flags -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Flags.txt";
            if (!File.Exists(fileAndDirectory))
            {
                this.TimedMessageBox(2000, gStr.gsMissingFlagsFile, gStr.gsButFieldIsLoaded);
            }

            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        //number of flags
                        line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            double lat;
                            double longi;
                            double east;
                            double nort;
                            double head;
                            int color;
                            string notes;

                            for (int v = 0; v < points; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                if (words.Length == 8)
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = double.Parse(words[4], CultureInfo.InvariantCulture);
                                    color = int.Parse(words[5]);
                                    notes = words[7].Trim();
                                }
                                else
                                {
                                    lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                    head = 0;
                                    color = int.Parse(words[4]);
                                    notes = "";
                                }

                                CFlag flagPt = new CFlag(lat, longi, east, nort, head, color, notes);
                                flagPts.Add(flagPt);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        this.TimedMessageBox(2000, gStr.gsFlagFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                    }
                }
            }

            //Boundaries
            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Boundary.txt";
            if (!File.Exists(fileAndDirectory))
            {
                this.TimedMessageBox(2000, gStr.gsMissingBoundaryFile, gStr.gsButFieldIsLoaded);
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();//Boundary

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            CBoundaryList New = new CBoundaryList();

                            //True or False OR points from older boundary files
                            line = reader.ReadLine();

                            //Check for older boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                New.isDriveThru = bool.Parse(line);
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

                                    New.fenceLine.points.Add(new vec2(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture)));
                                }
                                New.CalculateFenceArea();

                                bnd.bndList.Add(New);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.TimedMessageBox(2000, gStr.gsBoundaryLineFilesAreCorrupt, gStr.gsButFieldIsLoaded);
                        WriteErrorLog("Load Boundary Line" + e.ToString());
                    }
                }
            }

            CalculateMinMax();
            bnd.BuildTurnLines();

            // Headland  -------------------------------------------------------------------------------------------------
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Headland.txt";

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        Version currentFileVersion = new Version();
                        Version FileVersion6 = new Version(6, 0);

                        //read header
                        line = reader.ReadLine();

                        string[] words = line.Split(',');
                        if (words.Length > 1)
                        {
                            currentFileVersion = Version.Parse(words[1]);
                        }

                        for (int i = 0; true; i++)//bndList.Count
                        {
                            if (i >= bnd.bndList.Count || reader.EndOfStream) break;

                            //read the number of points
                            line = reader.ReadLine();//endless loop fix if no boundary

                            int hdLineCount = int.Parse(line);
                            for (int j = 0; j < hdLineCount; j++)
                            {
                                if (currentFileVersion < FileVersion6)
                                {
                                    hdLineCount = 1;
                                }
                                else
                                {
                                    line = reader.ReadLine();
                                }
                                int numPoints = int.Parse(line);

                                if (numPoints > 0)
                                {
                                    Polyline2 New = new Polyline2();
                                    //load the line
                                    for (int k = 0; k < numPoints; k++)
                                    {
                                        line = reader.ReadLine();
                                        words = line.Split(',');
                                        New.points.Add(new vec2(
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[1], CultureInfo.InvariantCulture)));
                                    }
                                    New.loop = true;
                                    bnd.bndList[i].hdLine.Add(New);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.TimedMessageBox(2000, "Headland File is Corrupt", "But Field is Loaded");
                        WriteErrorLog("Load Headland Loop" + e.ToString());
                    }
                }
            }

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;

            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
                btnHydLift.Visible = false;
            }

            //trams ---------------------------------------------------------------------------------
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Tram.txt";

            btnTramDisplayMode.Visible = false;

            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();//$Tram

                        if (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            int tramBoundaryCount = int.Parse(line);

                            for (int i = 0; i < tramBoundaryCount; i++)
                            {
                                line = reader.ReadLine();
                                int numPolyLines = int.Parse(line);
                                if (numPolyLines > 0)
                                {
                                    List<Polyline2> BoundaryArr = new List<Polyline2>();
                                    for (int j = 0; j < numPolyLines; j++)
                                    {
                                        line = reader.ReadLine();
                                        int numPoints = int.Parse(line);

                                        if (numPoints > 0)
                                        {
                                            Polyline2 tramArr = new Polyline2();

                                            line = reader.ReadLine();
                                            tramArr.loop = bool.Parse(line);

                                            for (int k = 0; k < numPoints; k++)
                                            {
                                                line = reader.ReadLine();
                                                string[] words = line.Split(',');
                                                vec2 vecPt = new vec2(
                                                double.Parse(words[0], CultureInfo.InvariantCulture),
                                                double.Parse(words[1], CultureInfo.InvariantCulture));

                                                tramArr.points.Add(vecPt);
                                            }

                                            tram.BuildTramLeftRightOffset(tramArr);
                                            BoundaryArr.Add(tramArr);
                                        }
                                    }
                                    tram.tramBoundary.Add(BoundaryArr);
                                }
                            }
                        }

                        if (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            int numLines = int.Parse(line);

                            for (int k = 0; k < numLines; k++)
                            {
                                line = reader.ReadLine();
                                int numPoints = int.Parse(line);
                                if (numPoints > 0)
                                {
                                    Polyline2 tramArr = new Polyline2();

                                    line = reader.ReadLine();
                                    tramArr.loop = bool.Parse(line);
                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec2 vecPt = new vec2(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture));

                                        tramArr.points.Add(vecPt);
                                    }

                                    tram.BuildTramLeftRightOffset(tramArr);
                                    tram.tramList.Add(tramArr);
                                }
                            }
                        }

                        FixTramModeButton();
                    }

                    catch (Exception e)
                    {
                        this.TimedMessageBox(2000, "Tram is corrupt", gStr.gsButFieldIsLoaded);
                        WriteErrorLog("Load Boundary Line" + e.ToString());
                    }
                }
            }

            if (Directory.Exists(fieldsDirectory + currentFieldDirectory))
            {
                foreach (string file in Directory.GetFiles(fieldsDirectory + currentFieldDirectory, "*.shp", SearchOption.TopDirectoryOnly))
                {
                    shapefile.Main(fieldsDirectory + currentFieldDirectory + "\\" + Path.GetFileNameWithoutExtension(file));
                }
            }
            //Recorded Path
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            CGuidanceRecPath New = new CGuidanceRecPath(Mode.RecPath);

                            line = reader.ReadLine();
                            while (gyd.curveArr.Exists(L => L.Name == line))//generate unique name!
                                line += " ";
                            New.Name = line;

                            line = reader.ReadLine();
                            if (line == "True" || line == "False")
                            {
                                New.loop = bool.Parse(line);
                                line = reader.ReadLine(); //number of points
                            }

                            int numPoints = int.Parse(line);

                            if (numPoints > 1)
                            {
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    New.points.Add(new vec2(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture)));
                                    New.Status.Add(new CRecPathPt(double.Parse(words[2], CultureInfo.InvariantCulture),
                                        (btnStates)Enum.Parse(typeof(btnStates), words[3], true)));
                                }
                                gyd.curveArr.Add(New);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.TimedMessageBox(2000, gStr.gsRecordedPathFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        WriteErrorLog("Load Recorded Path" + e.ToString());
                    }
                }
            }
        }//end of open file

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isJobStarted)
            {
                this.TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField);
                return;
            }
            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir," + currentVersionStr);
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(mc.latitude.ToString(CultureInfo.InvariantCulture) + "," + mc.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void FileCreateElevation()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //if (!isJobStarted)
            //{
            //    using (var form = new FormTimedMessage(3000, "Ooops, Job Not Started", "Start a Job First"))
            //    { form.Show(this); }
            //    return;
            //}

            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Elevation.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir," + currentVersionStr);
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");

                writer.WriteLine("Convergence");
                writer.WriteLine("0");

                writer.WriteLine("StartFix");
                writer.WriteLine(mc.latitude.ToString(CultureInfo.InvariantCulture) + "," + mc.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        //save field Patches
        public void FileSaveSections()
        {
            //make sure there is something to save
            if (patchSaveList.Count > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in patchSaveList)
                    {
                        int count2 = triList.Count;
                        writer.WriteLine((count2 - 1).ToString(CultureInfo.InvariantCulture));

                        if (count2 > 1)
                        {
                            writer.WriteLine((Math.Round(triList[0].easting, 3)).ToString(CultureInfo.InvariantCulture) +
                                "," + (Math.Round(triList[0].northing, 3)).ToString(CultureInfo.InvariantCulture) +
                                 "," + (Math.Round(triList[1].easting, 3)).ToString(CultureInfo.InvariantCulture) + "," + ((int)triList[1].northing).ToString(CultureInfo.InvariantCulture));

                            for (int i = 2; i < count2; i++)
                                writer.WriteLine((Math.Round(triList[i].easting, 3)).ToString(CultureInfo.InvariantCulture) +
                                    "," + (Math.Round(triList[i].northing, 3)).ToString(CultureInfo.InvariantCulture) +
                                     ",0");
                        }
                    }
                }

                //clear out that patchList and begin adding new ones for next save
                patchSaveList.Clear();
            }
        }

        //Create contour file
        public void FileCreateSections()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Sections.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                writer.WriteLine("$Contour," + currentVersionStr);
            }
        }

        //save the contour points which include elevation values
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in contourSaveList)
                    {
                        int count2 = triList.Count;

                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                        {
                            writer.WriteLine(Math.Round((triList[i].easting), 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].northing, 3).ToString(CultureInfo.InvariantCulture) + ",0");
                        }
                    }
                }

                contourSaveList.Clear();

            }
        }

        //save the boundary
        public void FileSaveBoundary()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Boundary.Txt"))
            {
                writer.WriteLine("$Boundary," + currentVersionStr);
                for (int i = 0; i < bnd.bndList.Count; i++)
                {
                    writer.WriteLine(bnd.bndList[i].isDriveThru);
                    //writer.WriteLine(bnd.bndList[i].isDriveAround);

                    writer.WriteLine(bnd.bndList[i].fenceLine.points.Count.ToString(CultureInfo.InvariantCulture));
                    if (bnd.bndList[i].fenceLine.points.Count > 0)
                    {
                        for (int j = 0; j < bnd.bndList[i].fenceLine.points.Count; j++)
                            writer.WriteLine(Math.Round(bnd.bndList[i].fenceLine.points[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(bnd.bndList[i].fenceLine.points[j].northing, 3).ToString(CultureInfo.InvariantCulture) + ",0");
                    }
                }
            }
        }

        //save tram
        public void FileSaveTram()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Tram.Txt"))
            {
                writer.WriteLine("$Tram," + currentVersionStr);

                writer.WriteLine(tram.tramBoundary.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < tram.tramBoundary.Count; i++)
                {
                    writer.WriteLine(tram.tramBoundary[i].Count.ToString(CultureInfo.InvariantCulture));

                    for (int j = 0; j < tram.tramBoundary[i].Count; j++)
                    {
                        writer.WriteLine(tram.tramBoundary[i][j].points.Count.ToString(CultureInfo.InvariantCulture));
                        writer.WriteLine(tram.tramBoundary[i][j].loop.ToString(CultureInfo.InvariantCulture));

                        for (int h = 0; h < tram.tramBoundary[i][j].points.Count; h++)
                        {
                            writer.WriteLine(Math.Round(tram.tramBoundary[i][j].points[h].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(tram.tramBoundary[i][j].points[h].northing, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }

                writer.WriteLine(tram.tramList.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < tram.tramList.Count; i++)
                {
                    writer.WriteLine(tram.tramList[i].points.Count.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(tram.tramList[i].loop.ToString(CultureInfo.InvariantCulture));
                    for (int h = 0; h < tram.tramList[i].points.Count; h++)
                    {
                        writer.WriteLine(Math.Round(tram.tramList[i].points[h].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(tram.tramList[i].points[h].northing, 3).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        //save the headland
        public void FileSaveHeadland()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Headland.Txt"))
            {
                writer.WriteLine("$Headland," + currentVersionStr);

                for (int i = 0; i < bnd.bndList.Count; i++)
                {
                    writer.WriteLine(bnd.bndList[i].hdLine.Count.ToString(CultureInfo.InvariantCulture));

                    for (int j = 0; j < bnd.bndList[i].hdLine.Count; j++)
                    {
                        writer.WriteLine(bnd.bndList[i].hdLine[j].points.Count.ToString(CultureInfo.InvariantCulture));

                        for (int k = 0; k < bnd.bndList[i].hdLine[j].points.Count; k++)
                        {
                            writer.WriteLine(bnd.bndList[i].hdLine[j].points[k].easting.ToString("0.000", CultureInfo.InvariantCulture) + "," +
                                             bnd.bndList[i].hdLine[j].points[k].northing.ToString("0.000", CultureInfo.InvariantCulture) + ",0");
                        }
                    }
                }
            }
        }

        public void FileSaveRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter((dirField + "RecPath.txt")))
            {
                try
                {
                    writer.WriteLine("$RecPath," + currentVersionStr);

                    for (int i = 0; i < gyd.curveArr.Count; i++)
                    {
                        if (gyd.curveArr[i].mode.HasFlag(Mode.RecPath))
                        {
                            if (gyd.curveArr[i] is CGuidanceRecPath RecPath)
                            {
                                //write out the points of ref line
                                writer.WriteLine(RecPath.Name);
                                writer.WriteLine(RecPath.loop);
                                writer.WriteLine(RecPath.Status.Count.ToString(CultureInfo.InvariantCulture));

                                for (int j = 0; j < RecPath.Status.Count; j++)
                                {
                                    writer.WriteLine(
                                    Math.Round(RecPath.points[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                    Math.Round(RecPath.points[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                    Math.Round(RecPath.Status[j].speed, 1).ToString(CultureInfo.InvariantCulture) + "," +
                                    (RecPath.Status[j].autoBtnState).ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    WriteErrorLog("Saving RecPath" + er.ToString());
                    return;
                }
            }
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(dirField + "Flags.txt"))
            {
                try
                {
                    writer.WriteLine("$Flags," + currentVersionStr);

                    int count2 = flagPts.Count;
                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].heading.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].color.ToString(CultureInfo.InvariantCulture) + ",0," +
                            flagPts[i].notes);
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving Flags" + e.ToString());
                    return;
                }
            }
        }

        //save nmea sentences
        public void FileSaveNMEA()
        {
            using (StreamWriter writer = new StreamWriter("zAOG_log.txt", true))
            {
                writer.Write(mc.logNMEASentence.ToString());
            }
            mc.logNMEASentence.Clear();
        }

        //save nmea sentences
        public void FileSaveElevation()
        {
            using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Elevation.txt"), true))
            {
                writer.Write(sbFix.ToString());
            }
            sbFix.Clear();
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML2(int flagNumber)
        {
            double lat = 0;
            double lon = 0;

            worldManager.ConvertLocalToWGS84(flagPts[flagNumber - 1].northing, flagPts[flagNumber - 1].easting, out lat, out lon);

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position


                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                flagPts[flagNumber - 1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber - 1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileMakeKMLFromCurrentPosition(double lat, double lon)
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }


            using (StreamWriter writer = new StreamWriter(dirField + "CurrentPosition.kml"))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flags
        public void FileSaveFieldKML()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Field.kml";

            XmlTextWriter kml = new XmlTextWriter(dirField + myFileName, Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            //Boundary  ----------------------------------------------------------------------
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndList.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0) kml.WriteElementString("name", currentFieldDirectory);

                //lineStyle
                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0) kml.WriteElementString("color", "ffdd00dd");
                else kml.WriteElementString("color", "ff4d3ffd");
                kml.WriteElementString("width", "4");
                kml.WriteEndElement(); // <LineStyle>

                kml.WriteStartElement("PolyStyle");
                if (i == 0) kml.WriteElementString("color", "407f3f55");
                else kml.WriteElementString("color", "703f38f1");
                kml.WriteEndElement(); // <PloyStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                //coords
                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndList[i].fenceLine.points.Count > 3)
                    bndPts = GetBoundaryPointsLatLon(i);
                kml.WriteRaw(bndPts);
                kml.WriteEndElement(); // <coordinates>

                kml.WriteEndElement(); // <Linear>
                kml.WriteEndElement(); // <OuterBoundary>
                kml.WriteEndElement(); // <Polygon>
                kml.WriteEndElement(); // <Placemark>
            }

            kml.WriteEndElement(); // <Folder>  
            //End of Boundary

            //guidance lines AB
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "AB_Lines");
            kml.WriteElementString("visibility", "0");

            for (int i = 0; i < gyd.curveArr.Count; i++)
            {
                if (gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("visibility", "0");

                    kml.WriteElementString("name", gyd.curveArr[i].Name.Trim());
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", "ff0000ff");
                    kml.WriteElementString("width", "2");
                    kml.WriteEndElement(); // <LineStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("LineString");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("coordinates");

                    string linePts = "";
                    if (gyd.curveArr[i].points.Count > 1)
                    {
                        for (int j = 0; j < gyd.curveArr[i].points.Count; j++)
                        {
                            if (j == 0)
                            {
                                double lineHeading = Math.Atan2(gyd.curveArr[i].points[j + 1].easting - gyd.curveArr[i].points[j].easting, gyd.curveArr[i].points[j + 1].northing - gyd.curveArr[i].points[j].northing);

                                linePts += worldManager.GetLocalToWSG84_KML(gyd.curveArr[i].points[j].easting - (Math.Sin(lineHeading) * gyd.abLength),
                                    gyd.curveArr[i].points[j].northing - (Math.Cos(lineHeading) * gyd.abLength));
                            }

                            linePts += worldManager.GetLocalToWSG84_KML(gyd.curveArr[i].points[j].easting, gyd.curveArr[i].points[j].northing);

                            if (j == gyd.curveArr[i].points.Count - 1)
                            {
                                double lineHeading = Math.Atan2(gyd.curveArr[i].points[j].easting - gyd.curveArr[i].points[j - 1].easting, gyd.curveArr[i].points[j].northing - gyd.curveArr[i].points[j - 1].northing);

                                linePts += worldManager.GetLocalToWSG84_KML(gyd.curveArr[i].points[j].easting + (Math.Sin(lineHeading) * gyd.abLength),
                                        gyd.curveArr[i].points[j].northing + (Math.Cos(lineHeading) * gyd.abLength));
                            }
                        }
                    }

                    kml.WriteRaw(linePts);

                    kml.WriteEndElement(); // <coordinates>
                    kml.WriteEndElement(); // <LineString>

                    kml.WriteEndElement(); // <Placemark>
                }
            }
            kml.WriteEndElement(); // <Folder>   

            //guidance lines Curve
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Curve_Lines");
            kml.WriteElementString("visibility", "0");

            for (int i = 0; i < gyd.curveArr.Count; i++)
            {
                if (gyd.curveArr[i].mode.HasFlag(Mode.Curve))
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("visibility", "0");

                    kml.WriteElementString("name", gyd.curveArr[i].Name.Trim());
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", "ff6699ff");
                    kml.WriteElementString("width", "2");
                    kml.WriteEndElement(); // <LineStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("LineString");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("coordinates");

                    string linePts = "";
                    for (int j = 0; j < gyd.curveArr[i].points.Count; j++)
                    {
                        linePts += worldManager.GetLocalToWSG84_KML(gyd.curveArr[i].points[j].easting, gyd.curveArr[i].points[j].northing);
                    }
                    kml.WriteRaw(linePts);

                    kml.WriteEndElement(); // <coordinates>
                    kml.WriteEndElement(); // <LineString>

                    kml.WriteEndElement(); // <Placemark>
                }
            }
            kml.WriteEndElement(); // <Folder>   

            //Recorded Path
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Recorded Path");
            kml.WriteElementString("visibility", "1");

            for (int i = 0; i < gyd.curveArr.Count; i++)
            {
                if (gyd.curveArr[i].mode.HasFlag(Mode.RecPath))
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("visibility", "0");

                    kml.WriteElementString("name", gyd.curveArr[i].Name.Trim());
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", "ff44ffff");
                    kml.WriteElementString("width", "2");
                    kml.WriteEndElement(); // <LineStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("LineString");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("coordinates");

                    string linePts = "";
                    for (int j = 0; j < gyd.curveArr[i].points.Count; j++)
                    {
                        linePts += worldManager.GetLocalToWSG84_KML(gyd.curveArr[i].points[j].easting, gyd.curveArr[i].points[j].northing);
                    }
                    kml.WriteRaw(linePts);

                    kml.WriteEndElement(); // <coordinates>
                    kml.WriteEndElement(); // <LineString>

                    kml.WriteEndElement(); // <Placemark>
                }
            }

            kml.WriteEndElement(); // <Folder>

            //flags  *************************************************************************
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_" + i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");

                if (flagPts[i].color == 0)  //red - xbgr
                    kml.WriteElementString("color", "ff4400ff");
                if (flagPts[i].color == 1)  //grn - xbgr
                    kml.WriteElementString("color", "ff44ff00");
                if (flagPts[i].color == 2)  //yel - xbgr
                    kml.WriteElementString("color", "ff44ffff");

                kml.WriteEndElement(); //IconStyle
                kml.WriteEndElement(); //Style

                kml.WriteElementString("name", ((i + 1).ToString() + " " + flagPts[i].notes));
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement(); //Point
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   
            //End of Flags

            //Sections  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");

            string secPts = "";
            int cntr = 0;

            //for every new chunk of patch
            foreach (Polyline2 triList in patchList)
            {
                if (triList.points.Count > 4)
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("name", "Sections_" + cntr.ToString());
                    cntr++;

                    string collor = "F0" + ((byte)(triList.points[1].easting)).ToString("X2") +
                        ((byte)(triList.points[0].northing)).ToString("X2") + ((byte)(triList.points[0].easting)).ToString("X2");

                    //lineStyle
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", collor);
                    //kml.WriteElementString("width", "6");
                    kml.WriteEndElement(); // <LineStyle>

                    kml.WriteStartElement("PolyStyle");
                    kml.WriteElementString("color", collor);
                    kml.WriteEndElement(); // <PloyStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("Polygon");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("outerBoundaryIs");
                    kml.WriteStartElement("LinearRing");

                    //coords
                    kml.WriteStartElement("coordinates");
                    secPts = "";
                    for (int i = 1; i < triList.points.Count; i += 2)
                    {
                        secPts += worldManager.GetLocalToWSG84_KML(triList.points[i].easting, triList.points[i].northing);
                    }
                    for (int i = triList.points.Count - 1; i > 1; i -= 2)
                    {
                        secPts += worldManager.GetLocalToWSG84_KML(triList.points[i].easting, triList.points[i].northing);
                    }
                    secPts += worldManager.GetLocalToWSG84_KML(triList.points[1].easting, triList.points[1].northing);

                    kml.WriteRaw(secPts);
                    kml.WriteEndElement(); // <coordinates>

                    kml.WriteEndElement(); // <LinearRing>
                    kml.WriteEndElement(); // <outerBoundaryIs>
                    kml.WriteEndElement(); // <Polygon>

                    kml.WriteEndElement(); // <Placemark>
                }
            }

            kml.WriteEndElement(); // <Folder>
            //End of sections

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();
        }

        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bnd.bndList[bndNum].fenceLine.points.Count; i++)
            {
                double lat = 0;
                double lon = 0;

                worldManager.ConvertLocalToWGS84(bnd.bndList[bndNum].fenceLine.points[i].northing, bnd.bndList[bndNum].fenceLine.points[i].easting, out lat, out lon);

                sb.Append(lon.ToString("0.0000000") + ',' + lat.ToString("0.0000000") + ",0 ");
            }
            return sb.ToString();
        }
    }
}