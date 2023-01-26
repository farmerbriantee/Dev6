//Please, if you use this, share the improvements

using Microsoft.Win32;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;

namespace AgOpenGPS
{

    public enum TBrand { AGOpenGPS, Case, Claas, Deutz, Fendt, JDeere, Kubota, Massey, NewHolland, Same, Steyr, Ursus, Valtra }
    public enum HBrand { AGOpenGPS, Case, Claas, JDeere, NewHolland }
    public enum WDBrand { AGOpenGPS, Case, Challenger, JDeere, NewHolland }


    //the main form object
    public partial class FormGPS : Form
    {
        //To bring forward AgIO if running
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        #region // Class Props and instances

        //The base directory where AgOpenGPS will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of vehicle
        public string vehiclesDirectory, vehicleFileName = "";

        //current directory of tools
        public string toolsDirectory, toolFileName = "";

        //current directory of Environments
        public string envDirectory, envFileName = "";

        //current fields and field directory
        public string fieldsDirectory, currentFieldDirectory, displayFieldName;

        private bool leftMouseDownOnOpenGL; //mousedown event in opengl window
        public int flagNumberPicked = 0;

        //bool for whether or not a job is active
        public bool isJobStarted = false, isAutoSteerBtnOn;

        //if we are saving a file
        public bool isLogNMEA = false, isLogElevation = false;

        //texture holders
        public uint[] texture;

        public string currentVersionStr = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public double secondsSinceStart, lastSecondInField;

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swHz = new Stopwatch();

        //Time to do fix position update and draw routine
        public double frameTime = 0, rawHz = 0.5, HzTime = 5;

        //For field saving in background
        private int secondsCounter = 1;
        private int dayNightCounter = 1;

        //whether or not to use Stanley control
        public bool isStanleyUsed = true, isKeepOffsetsOn = false;

        private int threeSecondCounter = 0;
        private int oneSecondCounter = 0;
        private int oneHalfSecondCounter = 0;

        /// <summary>
        /// create the world manager
        /// </summary>
        public CWorldManager worldManager;

        /// <summary>
        /// TramLine class for boundary and settings
        /// </summary>
        public CTram tram;

        /// <summary>
        /// Our vehicle only
        /// </summary>
        public CVehicle vehicle;

        /// <summary>
        /// Just the tool attachment that includes the sections
        /// </summary>
        public CTool tool;

        /// <summary>
        /// All the structs for recv and send of information out ports
        /// </summary>
        public CModuleComm mc;

        /// <summary>
        /// The boundary object
        /// </summary>
        public CBoundary bnd;

        /// <summary>
        /// The internal simulator
        /// </summary>
        public CSim sim;

        ///// <summary>
        ///// Sound
        ///// </summary>
        public CSound sounds;

        /// <summary>
        /// The font class
        /// </summary>
        public CFont font;

        /// <summary>
        /// The new steer algorithms
        /// </summary>
        public CGuidance gyd;

        public ShapeFile shape;
		/// <summary>
        /// The new brightness code
        /// </summary>
        public CWindowsSettingsBrightnessController displayBrightness;

        #endregion // Class Props and instances

        // Constructor, Initializes a new instance of the "FormGPS" class.
        public FormGPS()
        {
            //winform initialization
            InitializeComponent();

            RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            object Path = Key.GetValue("WorkDir");
            if (Path == null)
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else
                baseDirectory = Path.ToString() + "\\AgOpenGPS\\";
            Key.Close();

            Portable.PortableSettingsProvider.ApplicationSettingsFile = baseDirectory + "AgOpenGPS.config";
            Portable.PortableSettingsProvider.ApplyProvider(Properties.Settings.Default, Properties.Vehicle.Default);

            ControlExtension.Draggable(oglZoom, true);

            //time keeper
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            //create the world grid
            worldManager = new CWorldManager(this);

            //our vehicle made with gl object and pointer of mainform
            vehicle = new CVehicle(this);

            tool = new CTool(this);

            // communication
            mc = new CModuleComm(this);

            //boundary object
            bnd = new CBoundary(this);

            //nmea simulator built in.
            sim = new CSim(this);

            //instance of tram
            tram = new CTram(this);

            //access to font class
            font = new CFont(this);

            //the new steer algorithms
            gyd = new CGuidance(this);

            //sounds class
            sounds = new CSound();

            shape = new ShapeFile(this);
            displayBrightness = new CWindowsSettingsBrightnessController(Properties.Settings.Default.setDisplay_isBrightnessOn);
        }

        //Initialize items before the form Loads or is visible
        private void FormGPS_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.setDisplay_isTermsAccepted)
            {
                using (var form = new Form_First())
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                }
            }
            
            FormBorderStyle = FormBorderStyle.Sizable;
            MouseWheel += ZoomByMouseWheel;

            SetZoom(0);
            //start udp server is required
            StartLoopbackServer();

            pictureboxStart.BringToFront();
            pictureboxStart.Dock = System.Windows.Forms.DockStyle.Fill;
            tmrWatchdog.Enabled = true;

            Location = Properties.Settings.Default.setWindow_Location;
            Size = Properties.Settings.Default.setWindow_Size;

            if (Properties.Settings.Default.setDisplay_isStartFullScreen)
                this.WindowState = FormWindowState.Maximized;

            // load all the gui elements in gui.designer.cs
            LoadSettings();

            SetGuidanceMode(Mode.None);

            this.Resize += new System.EventHandler(this.FormGPS_Resize);

            if (!glm.isSimEnabled && !Debugger.IsAttached)
            {
                btnStartAgIO_Click(null, EventArgs.Empty);
            }
        }

        private void SetGuiText()
        {
            setWorkingDirectoryToolStripMenuItem.Text = gStr.gsDirectories;
            enterSimCoordsToolStripMenuItem.Text = gStr.gsEnterSimCoords;
            aboutToolStripMenuItem.Text = gStr.gsAbout;
            menustripLanguage.Text = gStr.gsLanguage;

            simulatorOnToolStripMenuItem.Text = gStr.gsSimulatorOn;

            resetALLToolStripMenuItem.Text = gStr.gsResetAll;
            colorsToolStripMenuItem1.Text = gStr.gsColors;
            topFieldViewToolStripMenuItem.Text = gStr.gsTopFieldView;

            resetEverythingToolStripMenuItem.Text = gStr.gsResetAllForSure;

            steerChartStripMenu.Text = gStr.gsSteerChart;

            //Tools Menu
            //toolStripBtnMakeBndContour.Text = gStr.gsMakeBoundaryContours;
            boundariesToolStripMenuItem.Text = gStr.gsBoundary;
            headlandToolStripMenuItem.Text = gStr.gsHeadland;
            deleteContourPathsToolStripMenuItem.Text = gStr.gsDeleteContourPaths;
            deleteAppliedAreaToolStripMenuItem.Text = gStr.gsDeleteAppliedArea;
            webcamToolStrip.Text = gStr.gsWebCam;
            //googleEarthFlagsToolStrip.Text = gStr.gsGoogleEarth;
            offsetFixToolStrip.Text = gStr.gsOffsetFix;

            btnChangeMappingColor.Text = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
        }

        //form is closing so tidy up and save settings
        private void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseCurrentJob(true))
            {
                e.Cancel = true;
                return;
            }

            SaveFormGPSWindowSettings();

            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally { loopBackSocket.Close(); }
            }

            //save current vehicle
            SettingsIO.ExportAll(vehiclesDirectory + vehicleFileName + ".XML");

			if (displayBrightness.isWmiMonitor)
                displayBrightness.SetBrightness(Properties.Settings.Default.setDisplay_brightnessSystem);
        }

        //called everytime window is resized, clean up button positions
        private void FormGPS_Resize(object sender, EventArgs e)
        {
            LineUpManualBtns();
        }

        // Load Bitmaps And Convert To Textures
        public void LoadGLTextures()
        {
            GL.Enable(EnableCap.Texture2D);

            Bitmap[] oglTextures = new Bitmap[]
            {
                Properties.Resources.z_SkyDay,Properties.Resources.z_Floor,Properties.Resources.z_Font,
                Properties.Resources.z_Turn,Properties.Resources.z_TurnCancel,Properties.Resources.z_TurnManual,
                Properties.Resources.z_Compass,Properties.Resources.z_Speedo,Properties.Resources.z_SpeedoNeedle,
                Properties.Resources.z_Lift,Properties.Resources.z_SkyNight,Properties.Resources.z_SteerPointer,
                Properties.Resources.z_SteerDot,GetTractorBrand(Properties.Settings.Default.setBrand_TBrand),Properties.Resources.z_QuestionMark,
                Properties.Resources.z_FrontWheels,Get4WDBrandFront(Properties.Settings.Default.setBrand_WDBrand), Get4WDBrandRear(Properties.Settings.Default.setBrand_WDBrand),
                GetHarvesterBrand(Properties.Settings.Default.setBrand_HBrand), Properties.Resources.z_LateralManual, Properties.Resources.z_bingMap, Properties.Resources.z_NoGPS
            };

            texture = new uint[oglTextures.Length];

            for (int h = 0; h < oglTextures.Length; h++)
            {
                using (Bitmap bitmap = oglTextures[h])
                {
                    GL.GenTextures(1, out texture[h]);
                    GL.BindTexture(TextureTarget.Texture2D, texture[h]);
                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                    bitmap.UnlockBits(bitmapData);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                }
            }
        }

        //Load Bitmaps brand
        public Bitmap GetTractorBrand(TBrand brand)
        {
            Bitmap bitmap;
            if (brand == TBrand.Case)
                bitmap = Properties.Resources.z_TractorCase;
            else if (brand == TBrand.Claas)
                bitmap = Properties.Resources.z_TractorClaas;
            else if (brand == TBrand.Deutz)
                bitmap = Properties.Resources.z_TractorDeutz;
            else if (brand == TBrand.Fendt)
                bitmap = Properties.Resources.z_TractorFendt;
            else if (brand == TBrand.JDeere)
                bitmap = Properties.Resources.z_TractorJDeere;
            else if (brand == TBrand.Kubota)
                bitmap = Properties.Resources.z_TractorKubota;
            else if (brand == TBrand.Massey)
                bitmap = Properties.Resources.z_TractorMassey;
            else if (brand == TBrand.NewHolland)
                bitmap = Properties.Resources.z_TractorNH;
            else if (brand == TBrand.Same)
                bitmap = Properties.Resources.z_TractorSame;
            else if (brand == TBrand.Steyr)
                bitmap = Properties.Resources.z_TractorSteyr;
            else if (brand == TBrand.Ursus)
                bitmap = Properties.Resources.z_TractorUrsus;
            else if (brand == TBrand.Valtra)
                bitmap = Properties.Resources.z_TractorValtra;
            else
                bitmap = Properties.Resources.z_TractorAoG;

            return bitmap;
        }

        public Bitmap GetHarvesterBrand(HBrand brandH)
        {
            Bitmap harvesterbitmap;
            if (brandH == HBrand.Case)
                harvesterbitmap = Properties.Resources.z_HarvesterCase;
            else if (brandH == HBrand.Claas)
                harvesterbitmap = Properties.Resources.z_HarvesterClaas;
            else if (brandH == HBrand.JDeere)
                harvesterbitmap = Properties.Resources.z_HarvesterJD;
            else if (brandH == HBrand.NewHolland)
                harvesterbitmap = Properties.Resources.z_HarvesterNH;
            else
                harvesterbitmap = Properties.Resources.z_HarvesterAoG;

            return harvesterbitmap;
        }

        public Bitmap Get4WDBrandFront(WDBrand brandWDF)
        {
            Bitmap bitmap4WDFront;
            if (brandWDF == WDBrand.Case)
                bitmap4WDFront = Properties.Resources.z_4WDFrontCase;
            else if (brandWDF == WDBrand.Challenger)
                bitmap4WDFront = Properties.Resources.z_4WDFrontChallenger;
            else if (brandWDF == WDBrand.JDeere)
                bitmap4WDFront = Properties.Resources.z_4WDFrontJDeere;
            else if (brandWDF == WDBrand.NewHolland)
                bitmap4WDFront = Properties.Resources.z_4WDFrontNH;
            else
                bitmap4WDFront = Properties.Resources.z_4WDFrontAoG;

            return bitmap4WDFront;
        }

        public Bitmap Get4WDBrandRear(WDBrand brandWDR)
        {
            Bitmap bitmap4WDRear;
            if (brandWDR == WDBrand.Case)
                bitmap4WDRear = Properties.Resources.z_4WDRearCase;
            else if (brandWDR == WDBrand.Challenger)
                bitmap4WDRear = Properties.Resources.z_4WDRearChallenger;
            else if (brandWDR == WDBrand.JDeere)
                bitmap4WDRear = Properties.Resources.z_4WDRearJDeere;
            else if (brandWDR == WDBrand.NewHolland)
                bitmap4WDRear = Properties.Resources.z_4WDRearNH;
            else
                bitmap4WDRear = Properties.Resources.z_4WDRearAoG;

            return bitmap4WDRear;
        }

        private void BuildMachineByte()
        {
            int set = 1;
            p_254.pgn[p_254.sc1to8] = 0;
            p_254.pgn[p_254.sc9to16] = 0;

            int machine = 0;

            //check if super section is on
            for (int j = 0; j < tool.numOfSections; j++)
            {
                //set if on, reset bit if off
                if (tool.sections[tool.numOfSections].isSectionOn || tool.sections[j].isSectionOn)
                    machine |= set;

                //move set and reset over 1 bit left
                set <<= 1;
            }

            //sections in autosteer
            p_254.pgn[p_254.sc9to16] = unchecked((byte)(machine >> 8));
            p_254.pgn[p_254.sc1to8] = unchecked((byte)machine);

            //machine pgn
            p_239.pgn[p_239.sc9to16] = p_254.pgn[p_254.sc9to16];
            p_239.pgn[p_239.sc1to8] = p_254.pgn[p_254.sc1to8];
            p_239.pgn[p_239.speed] = unchecked((byte)(mc.avgSpeed*10));
            p_239.pgn[p_239.tram] = unchecked((byte)tram.controlByte);

            //out serial to autosteer module  //indivdual classes load the distance and heading deltas 
        }
        
        //request a new job
        public void JobNew()
        {
            if (Properties.Settings.Default.setMenu_isOGLZoomOn == 1)
            {
                oglZoom.BringToFront();
                oglZoom.Width = 300;
                oglZoom.Height = 300;
            }

            isJobStarted = true;

            FieldMenuButtonEnableDisable(true);
        }

        public void FieldMenuButtonEnableDisable(bool isOn)
        {
            deleteContourPathsToolStripMenuItem.Enabled = isOn;
            deleteAppliedAreaToolStripMenuItem.Enabled = isOn;

            panelBottom.Visible = isOn;
            panelRight.Visible = isOn;

            lblFieldStatus.Visible = isOn;

            for (int j = 0; j < tool.numOfSections; j++)
            {
                if (!isOn)
                {
                    tool.sections[j].sectionOnRequest = 0;
                    tool.sections[j].isSectionOn = false;
                    if (tool.sections[j].isMappingOn)
                        tool.sections[j].TurnMappingOff();

                    tool.sections[j].triangleList.Clear();
                }
                tool.sections[j].SetButtonStatus(isOn);
            }
            LineUpManualBtns();

            toolStripBtnField.Image = (isOn && Properties.Settings.Default.setDisplayFeature_SimpleCloseField) ? Properties.Resources.JobClose : Properties.Resources.JobActive;
        }

        public void PanelRightEnabled(bool status)
        {
            panelRight.Enabled = status;
        }

        public void LoadDriveInFields()
        {
            string[] dirs = Directory.GetDirectories(fieldsDirectory);

            Fields.Clear();

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string filename = dir + "\\Field.txt";
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    CAutoLoadField New = new CAutoLoadField();
                    double mPerDegreeLat = 0;
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

                                New.LatStart = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                New.LonStart = (double.Parse(offs[1], CultureInfo.InvariantCulture));
                                New.Dir = fieldDirectory;


                                double LatRad = New.LatStart * 0.01745329251994329576923690766743;

                                mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * LatRad) + 1.175 * Math.Cos(4.0 * LatRad) - 0.0023 * Math.Cos(6.0 * LatRad);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    //grab the boundary area
                    filename = dir + "\\Boundary.txt";
                    if (File.Exists(filename))
                    {
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

                                            double east = double.Parse(words[0], CultureInfo.InvariantCulture);
                                            double nort = double.Parse(words[1], CultureInfo.InvariantCulture);

                                            worldManager.ConvertLocalToLocal(nort, east, New.LatStart, New.LonStart, mPerDegreeLat, out double northing, out double easting);

                                            New.points.Add(new vec2(easting, northing));
                                        }

                                        int j = New.points.Count - 1;  // The last vertex is the 'previous' one to the first
                                        double Area = 0;
                                        for (int i = 0; i < New.points.Count; j = i++)
                                        {
                                            Area += (New.points[j].easting + New.points[i].easting) * (New.points[j].northing - New.points[i].northing);
                                        }

                                        New.Area = Math.Abs(Area / 2);

                                        Fields.Add(New);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        public void JobClose()
        {
            //reset field offsets
            if (!isKeepOffsetsOn)
            {
                worldManager.fixOffset = new vec2();
            }

            //turn off headland
            bnd.isHeadlandOn = false;
            btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            btnHeadlandOnOff.Visible = false;

            //make sure hydraulic lift is off
            p_239.pgn[p_239.hydLift] = 0;
            vehicle.isHydLiftOn = false;
            btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            btnHydLift.Visible = false;

            //zoom gone
            oglZoom.SendToBack();

            //clean all the lines
            for (int i = bnd.bndList.Count - 1; i >= 0; i--)
                bnd.RemoveHandles(i);

            FieldMenuButtonEnableDisable(false);

            isJobStarted = false;
            
            //fix auto button
            setSectionBtnState(btnStates.Off);

            for (int j = 0; j < patchList.Count; j++)
            {
                patchList[j].RemoveHandle();
            }
            patchList.Clear();

            //clear the flags
            flagPts.Clear();

            SetGuidanceMode(Mode.None);
            gyd.currentCurveLine = null;
            gyd.currentABLine = null;

            gyd.howManyPathsAway = 0;

            gyd.creatingContour = null;
            gyd.curList = new Polyline();
            gyd.curveArr.Clear();

            gyd.resumeState = 0;
            gyd.currentPositonIndex = 0;


            //clean up tram
            tram.displayMode = 0;

            for (int j = 0; j < tram.tramList.Count; j++)
                tram.tramList[j].RemoveHandle();
            tram.tramList.Clear();

            for (int i = 0; i < tram.tramBoundary.Count; i++)
            {
                for (int j = 0; j < tram.tramBoundary[i].Count; j++)
                    tram.tramBoundary[i][j].RemoveHandle();
            }
            tram.tramBoundary.Clear();

            contourSaveList.Clear();

            //reset acre and distance counters
            bnd.workedAreaTotal = 0;

            for (int i = 0; i < bnd.Rate.Count; i++)
                bnd.Rate[i].RemoveHandle();
            bnd.Rate.Clear();

            //reset GUI areas
            CalculateMinMax();

            displayFieldName = gStr.gsNone;
            FixTramModeButton();

            worldManager.isGeoMap = false;
        }

        //take the distance from object and convert to camera data
        public void SetZoom(double Delta)
        {
            worldManager.zoomValue += Delta * worldManager.zoomValue * (worldManager.zoomValue <= 20 ? 0.04 : 0.02);

            if (worldManager.zoomValue > 220) worldManager.zoomValue = 220;
            if (worldManager.zoomValue < 6.0) worldManager.zoomValue = 6.0;

            worldManager.camSetDistance = worldManager.zoomValue * worldManager.zoomValue;

            //match grid to cam distance and redo perspective
            if (worldManager.camSetDistance < 50) worldManager.gridZoom = 5;
            else if (worldManager.camSetDistance < 150) worldManager.gridZoom = 10;
            else if (worldManager.camSetDistance < 250) worldManager.gridZoom = 20;
            else if (worldManager.camSetDistance < 500) worldManager.gridZoom = 40;
            else if (worldManager.camSetDistance < 1000) worldManager.gridZoom = 80;
            else if (worldManager.camSetDistance < 2000) worldManager.gridZoom = 160;
            else if (worldManager.camSetDistance < 5000) worldManager.gridZoom = 320;
            else if (worldManager.camSetDistance < 10000) worldManager.gridZoom = 640;
            else if (worldManager.camSetDistance < 20000) worldManager.gridZoom = 1280;

            oglMain_Resize(null, null);
        }

        //an error log called by all try catches
        public void WriteErrorLog(string strErrorText)
        {
            try
            {
                //set up file and folder if it doesn't exist
                const string strFileName = "Error Log.txt";
                //string strPath = Application.StartupPath;

                //Write out the error appending to existing
                File.AppendAllText(baseDirectory + "\\" + strFileName, strErrorText + " - " +
                    DateTime.Now.ToString() + "\r\n\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WriteErrorLog: " + ex.Message, "Error Logging", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }//class FormGPS
}//namespace AgOpenGPS

/*The order is:
 *
 * The watchdog timer times out and runs this function tmrWatchdog_tick().
 * 50 times per second so statusUpdateCounter counts to 25 and updates strip menu etc at 2 hz
 * it also makes sure there is new sentences showing up otherwise it shows **** No GGA....
 * saveCounter ticks 2 x per second, used at end of draw routine every minute to save a backup of field
 * then ScanForNMEA function checks for a complete sentence if contained in pn.rawbuffer
 * if not it comes right back and waits for next watchdog trigger and starts all over
 * if a new sentence is there, UpdateFix() is called
 * Right away CalculateLookAhead(), no skips, is called to determine lookaheads and trigger distances to save triangles plotted
 * Then UpdateFix() continues.
 * Hitch, pivot, antenna locations etc and directions are figured out if trigDistance is triggered
 * When that is done, DoRender() is called on the visible OpenGL screen and its draw routine _draw is run
 * before triangles are drawn, frustum cull figures out how many of the triangles should be drawn
 * When its all the way thru, it triggers the sectioncontrol Draw, its frustum cull, and determines if sections should be on
 * ProcessSectionOnOffRequests() runs and that does the section on off magic
 * SectionControlToArduino() runs and spits out the port machine control based on sections on or off
 * If field needs saving (1.5 minute since last time) field is saved
 * Now the program is "Done" and waits for the next watchdog trigger, determines if a new sentence is available etc
 * and starts all over from the top.
 */