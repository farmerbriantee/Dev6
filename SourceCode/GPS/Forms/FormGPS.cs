//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net.Sockets;
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

        //maximum sections available
        public const int MAXSECTIONS = 17;

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

        //the currentversion of software
        public string currentVersionStr, inoVersionStr;
        public int inoVersionInt;

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swFrame = new Stopwatch();

        public double secondsSinceStart;

        //Time to do fix position update and draw routine
        public double frameTime = 0;

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swHz = new Stopwatch();

        //Time to do fix position update and draw routine
        private double HzTime = 5;

        //For field saving in background
        private int secondsCounter = 1;
        private int dayNightCounter = 1;

        //whether or not to use Stanley control
        public bool isStanleyUsed = true, isKeepOffsetsOn = false;

        private int threeSecondCounter = 0;
        private int oneSecondCounter = 0;
        private int oneHalfSecondCounter = 0;

        public double mToUser, userToM, mToUserBig, userBigToM, m2ToUser, KMHToUser, userToKMH;

        public string unitsFtM, unitsInCm, unitsHaAc;

        /// <summary>
        /// create the world manager
        /// </summary>
        public CWorldManager worldManager;

        /// <summary>
        /// The NMEA class that decodes it
        /// </summary>
        public CNMEA pn;

        /// <summary>
        /// an array of sections, so far 16 section + 1 fullWidth Section
        /// </summary>
        public CSection[] section;

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

        /// <summary>
        /// Heading, Roll, Pitch, GPS, Properties
        /// </summary>
        public CAHRS ahrs;

        /// <summary>
        /// Most of the displayed field data for GUI
        /// </summary>
        public CFieldData fd;

        ///// <summary>
        ///// Sound
        ///// </summary>
        public CSound sounds;

        /// <summary>
        /// Sound for approaching boundary
        /// </summary>
        public SoundPlayer sndHydraulicLift;

        /// <summary>
        /// Sound for approaching boundary
        /// </summary>
        public SoundPlayer sndHydraulicLower;

        /// <summary>
        /// The font class
        /// </summary>
        public CFont font;

        /// <summary>
        /// The new steer algorithms
        /// </summary>
        public CGuidance gyd;

        public ShapeFile shape;

        #endregion // Class Props and instances

        // Constructor, Initializes a new instance of the "FormGPS" class.
        public FormGPS()
        {
            //winform initialization
            InitializeComponent();

            if (Settings.Default.setFeatures == null)
            {
                Settings.Default.setFeatures = new CFeatureSettings();
            }

            ControlExtension.Draggable(oglZoom, true);
            ControlExtension.Draggable(panelDrag, true);

            //time keeper
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            //create the world grid
            worldManager = new CWorldManager(this);

            //our vehicle made with gl object and pointer of mainform
            vehicle = new CVehicle(this);

            tool = new CTool(this);

            //create a new section and set left and right positions
            //created whether used or not, saves restarting program

            section = new CSection[MAXSECTIONS];
            for (int j = 0; j < MAXSECTIONS; j++)
            {
                section[j] = new CSection(this, j);
                Controls.Add(section[j].button);
                section[j].button.BringToFront();
            }

            //our NMEA parser
            pn = new CNMEA(this);

            //module communication
            mc = new CModuleComm(this);

            //boundary object
            bnd = new CBoundary(this);

            //nmea simulator built in.
            sim = new CSim(this);

            ////all the attitude, heading, roll, pitch reference system
            ahrs = new CAHRS();

            //fieldData all in one place
            fd = new CFieldData(this);

            //instance of tram
            tram = new CTram(this);

            //access to font class
            font = new CFont(this);

            //the new steer algorithms
            gyd = new CGuidance(this);

            //sounds class
            sounds = new CSound();

            shape = new ShapeFile(this);
        }

        //Initialize items before the form Loads or is visible
        private void FormGPS_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            MouseWheel += ZoomByMouseWheel;

            //start udp server is required
            StartLoopbackServer();

            pictureboxStart.BringToFront();
            pictureboxStart.Dock = System.Windows.Forms.DockStyle.Fill;
            tmrWatchdog.Enabled = true;

            //set the language to last used
            SetLanguage(Settings.Default.setF_culture);

            currentVersionStr = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            string[] fullVers = currentVersionStr.Split('.');
            int inoV = int.Parse(fullVers[0], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[1], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[2], CultureInfo.InvariantCulture);
            inoVersionInt = inoV;
            inoVersionStr = inoV.ToString();

            if (Settings.Default.setF_workingDirectory == "Default")
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else baseDirectory = Settings.Default.setF_workingDirectory + "\\AgOpenGPS\\";

            //get the fields directory, if not exist, create
            fieldsDirectory = baseDirectory + "Fields\\";
            string dir = Path.GetDirectoryName(fieldsDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //get the fields directory, if not exist, create
            vehiclesDirectory = baseDirectory + "Vehicles\\";
            dir = Path.GetDirectoryName(vehiclesDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //make sure current field directory exists, null if not
            currentFieldDirectory = Settings.Default.setF_CurrentDir;

            // load all the gui elements in gui.designer.cs
            LoadSettings();

            //nmea limiter
            udpWatch.Start();

            Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            FixPanelsAndMenus();

            this.Resize += new System.EventHandler(this.FormGPS_Resize); 
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
            deleteForSureToolStripMenuItem.Text = gStr.gsAreYouSure;
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
        }

        //called everytime window is resized, clean up button positions
        private void FormGPS_Resize(object sender, EventArgs e)
        {
            FixPanelsAndMenus();
        }

        // Load Bitmaps And Convert To Textures

        public enum textures : uint
        {
            SkyDay, Floor, Font,
            Turn, TurnCancel, TurnManual,
            Compass, Speedo, SpeedoNeedle,
            Lift, SkyNight, SteerPointer,
            SteerDot, Tractor, QuestionMark,
            FrontWheels, FourWDFront, FourWDRear,
            Harvester, Lateral
        }

        public void LoadGLTextures()
        {
            GL.Enable(EnableCap.Texture2D);

            Bitmap[] oglTextures = new Bitmap[]
            {
                Properties.Resources.z_SkyDay,Properties.Resources.z_Floor,Properties.Resources.z_Font,
                Properties.Resources.z_Turn,Properties.Resources.z_TurnCancel,Properties.Resources.z_TurnManual,
                Properties.Resources.z_Compass,Properties.Resources.z_Speedo,Properties.Resources.z_SpeedoNeedle,
                Properties.Resources.z_Lift,Properties.Resources.z_SkyNight,Properties.Resources.z_SteerPointer,
                Properties.Resources.z_SteerDot,GetTractorBrand(Settings.Default.setBrand_TBrand),Properties.Resources.z_QuestionMark,
                Properties.Resources.z_FrontWheels,Get4WDBrandFront(Settings.Default.setBrand_WDBrand), Get4WDBrandRear(Settings.Default.setBrand_WDBrand),
                GetHarvesterBrand(Settings.Default.setBrand_HBrand), Properties.Resources.z_LateralManual, Properties.Resources.z_bingMap
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
                bitmap = Resources.z_TractorCase;
            else if (brand == TBrand.Claas)
                bitmap = Resources.z_TractorClaas;
            else if (brand == TBrand.Deutz)
                bitmap = Resources.z_TractorDeutz;
            else if (brand == TBrand.Fendt)
                bitmap = Resources.z_TractorFendt;
            else if (brand == TBrand.JDeere)
                bitmap = Resources.z_TractorJDeere;
            else if (brand == TBrand.Kubota)
                bitmap = Resources.z_TractorKubota;
            else if (brand == TBrand.Massey)
                bitmap = Resources.z_TractorMassey;
            else if (brand == TBrand.NewHolland)
                bitmap = Resources.z_TractorNH;
            else if (brand == TBrand.Same)
                bitmap = Resources.z_TractorSame;
            else if (brand == TBrand.Steyr)
                bitmap = Resources.z_TractorSteyr;
            else if (brand == TBrand.Ursus)
                bitmap = Resources.z_TractorUrsus;
            else if (brand == TBrand.Valtra)
                bitmap = Resources.z_TractorValtra;
            else
                bitmap = Resources.z_TractorAoG;

            return bitmap;
        }

        public Bitmap GetHarvesterBrand(HBrand brandH)
        {
            Bitmap harvesterbitmap;
            if (brandH == HBrand.Case)
                harvesterbitmap = Resources.z_HarvesterCase;
            else if (brandH == HBrand.Claas)
                harvesterbitmap = Resources.z_HarvesterClaas;
            else if (brandH == HBrand.JDeere)
                harvesterbitmap = Resources.z_HarvesterJD;
            else if (brandH == HBrand.NewHolland)
                harvesterbitmap = Resources.z_HarvesterNH;
            else
                harvesterbitmap = Resources.z_HarvesterAoG;

            return harvesterbitmap;
        }

        public Bitmap Get4WDBrandFront(WDBrand brandWDF)
        {
            Bitmap bitmap4WDFront;
            if (brandWDF == WDBrand.Case)
                bitmap4WDFront = Resources.z_4WDFrontCase;
            else if (brandWDF == WDBrand.Challenger)
                bitmap4WDFront = Resources.z_4WDFrontChallenger;
            else if (brandWDF == WDBrand.JDeere)
                bitmap4WDFront = Resources.z_4WDFrontJDeere;
            else if (brandWDF == WDBrand.NewHolland)
                bitmap4WDFront = Resources.z_4WDFrontNH;
            else
                bitmap4WDFront = Resources.z_4WDFrontAoG;

            return bitmap4WDFront;
        }

        public Bitmap Get4WDBrandRear(WDBrand brandWDR)
        {
            Bitmap bitmap4WDRear;
            if (brandWDR == WDBrand.Case)
                bitmap4WDRear = Resources.z_4WDRearCase;
            else if (brandWDR == WDBrand.Challenger)
                bitmap4WDRear = Resources.z_4WDRearChallenger;
            else if (brandWDR == WDBrand.JDeere)
                bitmap4WDRear = Resources.z_4WDRearJDeere;
            else if (brandWDR == WDBrand.NewHolland)
                bitmap4WDRear = Resources.z_4WDRearNH;
            else
                bitmap4WDRear = Resources.z_4WDRearAoG;

            return bitmap4WDRear;
        }

        private void BuildMachineByte()
        {
            int set = 1;
            int reset = 2046;
            p_254.pgn[p_254.sc1to8] = 0;
            p_254.pgn[p_254.sc9to16] = 0;

            int machine = 0;

            //check if super section is on
            if (section[tool.numOfSections].isSectionOn)
            {
                for (int j = 0; j < tool.numOfSections; j++)
                {
                    //all the sections are on, so set them
                    machine |= set;
                    set <<= 1;
                }
            }
            else
            {
                for (int j = 0; j < MAXSECTIONS; j++)
                {
                    //set if on, reset bit if off
                    if (section[j].isSectionOn) machine |= set;
                    else machine &= reset;

                    //move set and reset over 1 bit left
                    set <<= 1;
                    reset <<= 1;
                    reset += 1;
                }
            }

            //sections in autosteer
            p_254.pgn[p_254.sc9to16] = unchecked((byte)(machine >> 8));
            p_254.pgn[p_254.sc1to8] = unchecked((byte)machine);

            //machine pgn
            p_239.pgn[p_239.sc9to16] = p_254.pgn[p_254.sc9to16];
            p_239.pgn[p_239.sc1to8] = p_254.pgn[p_254.sc1to8];
            p_239.pgn[p_239.speed] = unchecked((byte)(avgSpeed*10));
            p_239.pgn[p_239.tram] = unchecked((byte)tram.controlByte);

            //out serial to autosteer module  //indivdual classes load the distance and heading deltas 
        }

        public bool KeypadToNUD(NumericUpDown sender, Form owner)
        {
            sender.BackColor = Color.Red;
            sender.Value = Math.Round(sender.Value, sender.DecimalPlaces);

            using (FormNumeric form = new FormNumeric((double)sender.Minimum, (double)sender.Maximum, (double)sender.Value, 2))
            {
                DialogResult result = form.ShowDialog(owner);
                if (result == DialogResult.OK)
                {
                    sender.Value = (decimal)form.ReturnValue;
                    sender.BackColor = Color.AliceBlue;
                    return true;
                }
                else if (result == DialogResult.Cancel)
                {
                    sender.BackColor = Color.AliceBlue;
                }
                return false;
            }
        }

        public bool KeypadToButton(ref Button sender, ref double value, double minimum, double maximum, int decimals, bool changetoculture = false, double Mtr2Unit = 1.0, double unit2meter = 1.0, decimal divisible = -1)
        {
            using (FormNumeric form = new FormNumeric(minimum, maximum, value, decimals, changetoculture, unit2meter, Mtr2Unit, divisible))
            {
                if (form.ShowDialog(sender) == DialogResult.OK)
                {
                    value = form.ReturnValue;

                    string guifix = "0";
                    if (decimals > 0)
                    {
                        guifix = "0.0";
                        while (--decimals > 0)
                            guifix += "#";
                    }

                    sender.Text = (value * Mtr2Unit).ToString(guifix);
                    return true;
                }
            }
            return false;
        }

        public bool KeypadToButton(ref Button sender, ref int value, double minimum, double maximum, double Mtr2Unit = 1.0, double unit2meter = 1.0, decimal divisible = -1)
        {
            using (FormNumeric form = new FormNumeric(minimum, maximum, value, 0, true, unit2meter, Mtr2Unit, divisible))
            {
                if (form.ShowDialog(sender) == DialogResult.OK)
                {
                    value = (int)form.ReturnValue;
                    sender.Text = (value * Mtr2Unit).ToString("0");
                    return true;
                }
            }
            return false;
        }

        public void KeyboardToText(TextBox sender, IWin32Window owner)
        {
            sender.BackColor = Color.Red;
            using (FormKeyboard form = new FormKeyboard(sender.Text))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    sender.Text = form.ReturnString;
                }
            }
            sender.BackColor = Color.AliceBlue;
        }

        //function to set section positions
        public void SectionSetPosition()
        {
            section[0].positionLeft = Vehicle.Default.setSection_position1 + Vehicle.Default.setVehicle_toolOffset;
            section[0].positionRight = Vehicle.Default.setSection_position2 + Vehicle.Default.setVehicle_toolOffset;

            section[1].positionLeft = Vehicle.Default.setSection_position2 + Vehicle.Default.setVehicle_toolOffset;
            section[1].positionRight = Vehicle.Default.setSection_position3 + Vehicle.Default.setVehicle_toolOffset;

            section[2].positionLeft = Vehicle.Default.setSection_position3 + Vehicle.Default.setVehicle_toolOffset;
            section[2].positionRight = Vehicle.Default.setSection_position4 + Vehicle.Default.setVehicle_toolOffset;

            section[3].positionLeft = Vehicle.Default.setSection_position4 + Vehicle.Default.setVehicle_toolOffset;
            section[3].positionRight = Vehicle.Default.setSection_position5 + Vehicle.Default.setVehicle_toolOffset;

            section[4].positionLeft = Vehicle.Default.setSection_position5 + Vehicle.Default.setVehicle_toolOffset;
            section[4].positionRight = Vehicle.Default.setSection_position6 + Vehicle.Default.setVehicle_toolOffset;

            section[5].positionLeft = Vehicle.Default.setSection_position6 + Vehicle.Default.setVehicle_toolOffset;
            section[5].positionRight = Vehicle.Default.setSection_position7 + Vehicle.Default.setVehicle_toolOffset;

            section[6].positionLeft = Vehicle.Default.setSection_position7 + Vehicle.Default.setVehicle_toolOffset;
            section[6].positionRight = Vehicle.Default.setSection_position8 + Vehicle.Default.setVehicle_toolOffset;

            section[7].positionLeft = Vehicle.Default.setSection_position8 + Vehicle.Default.setVehicle_toolOffset;
            section[7].positionRight = Vehicle.Default.setSection_position9 + Vehicle.Default.setVehicle_toolOffset;

            section[8].positionLeft = Vehicle.Default.setSection_position9 + Vehicle.Default.setVehicle_toolOffset;
            section[8].positionRight = Vehicle.Default.setSection_position10 + Vehicle.Default.setVehicle_toolOffset;

            section[9].positionLeft = Vehicle.Default.setSection_position10 + Vehicle.Default.setVehicle_toolOffset;
            section[9].positionRight = Vehicle.Default.setSection_position11 + Vehicle.Default.setVehicle_toolOffset;

            section[10].positionLeft = Vehicle.Default.setSection_position11 + Vehicle.Default.setVehicle_toolOffset;
            section[10].positionRight = Vehicle.Default.setSection_position12 + Vehicle.Default.setVehicle_toolOffset;

            section[11].positionLeft = Vehicle.Default.setSection_position12 + Vehicle.Default.setVehicle_toolOffset;
            section[11].positionRight = Vehicle.Default.setSection_position13 + Vehicle.Default.setVehicle_toolOffset;

            section[12].positionLeft = Vehicle.Default.setSection_position13 + Vehicle.Default.setVehicle_toolOffset;
            section[12].positionRight = Vehicle.Default.setSection_position14 + Vehicle.Default.setVehicle_toolOffset;

            section[13].positionLeft = Vehicle.Default.setSection_position14 + Vehicle.Default.setVehicle_toolOffset;
            section[13].positionRight = Vehicle.Default.setSection_position15 + Vehicle.Default.setVehicle_toolOffset;

            section[14].positionLeft = Vehicle.Default.setSection_position15 + Vehicle.Default.setVehicle_toolOffset;
            section[14].positionRight = Vehicle.Default.setSection_position16 + Vehicle.Default.setVehicle_toolOffset;

            section[15].positionLeft = Vehicle.Default.setSection_position16 + Vehicle.Default.setVehicle_toolOffset;
            section[15].positionRight = Vehicle.Default.setSection_position17 + Vehicle.Default.setVehicle_toolOffset;

            //Calculate total width and each section width
            SectionCalcWidths();
        }

        //function to calculate the width of each section and update
        public void SectionCalcWidths()
        {
            for (int j = 0; j < MAXSECTIONS; j++)
            {
                section[j].sectionWidth = section[j].positionRight - section[j].positionLeft;
                section[j].rpSectionPosition = 250 + (int)Math.Round(section[j].positionLeft * 10, 0, MidpointRounding.AwayFromZero);
                section[j].rpSectionWidth = (int)Math.Round(section[j].sectionWidth * 10, 0, MidpointRounding.AwayFromZero);
            }

            //calculate tool width based on extreme right and left values
            tool.toolWidth = section[tool.numOfSections - 1].positionRight - section[0].positionLeft;

            Vehicle.Default.setVehicle_toolWidth = tool.toolWidth;
            Vehicle.Default.Save();

            //left and right tool position
            tool.toolFarLeftPosition = section[0].positionLeft;
            tool.toolFarRightPosition = section[tool.numOfSections - 1].positionRight;

            //now do the full width section
            section[tool.numOfSections].sectionWidth = tool.toolWidth;
            section[tool.numOfSections].positionLeft = tool.toolFarLeftPosition;
            section[tool.numOfSections].positionRight = tool.toolFarRightPosition;
            
            //find the right side pixel position
            tool.rpXPosition = 250 + (int)(Math.Round((tool.toolFarLeftPosition - tool.toolOffset) * 10, 0, MidpointRounding.AwayFromZero));
            tool.rpWidth = (int)(Math.Round(tool.toolWidth * 10, 0, MidpointRounding.AwayFromZero));
        }

        //request a new job
        public void JobNew()
        {
            if (Settings.Default.setMenu_isOGLZoomOn == 1)
            {
                oglZoom.BringToFront();
                oglZoom.Width = 300;
                oglZoom.Height = 300;
            }

            isJobStarted = true;

            for (int j = 0; j < MAXSECTIONS; j++)
            {
                section[j].SetButtonStatus(true);
            }

            FieldMenuButtonEnableDisable(true);

            if (Debugger.IsAttached && Directory.Exists(fieldsDirectory + currentFieldDirectory))
            {
                foreach (string file in Directory.GetFiles(fieldsDirectory + currentFieldDirectory, "*.shp", SearchOption.AllDirectories))
                {
                    shape.Main(fieldsDirectory + currentFieldDirectory + "\\" + Path.GetFileNameWithoutExtension(file));
                }
            }
        }

        public void FieldMenuButtonEnableDisable(bool isOn)
        {
            deleteContourPathsToolStripMenuItem.Enabled = isOn;
            deleteAppliedAreaToolStripMenuItem.Enabled = isOn;

            panelBottom.Visible = isOn;
            panelRight.Visible = isOn;

            lblFieldStatus.Visible = isOn;

            LineUpManualBtns();
        }

        //close the current job
        public void JobClose()
        {
            //reset field offsets
            if (!isKeepOffsetsOn)
            {
                pn.fixOffset = new vec2();
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

            //turn section buttons all OFF
            for (int j = 0; j < MAXSECTIONS; j++)
            {
                section[j].sectionOnRequest = false;
                section[j].isSectionOn = false;
                if (section[j].isMappingOn)
                    section[j].TurnMappingOff();

                section[j].SetButtonStatus(false);
                section[j].triangleList.Clear();
            }

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
            gyd.curList.Clear();
            gyd.curveArr.Clear();

            gyd.resumeState = 0;
            btnResumePath.Image = Resources.pathResumeStart;
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
            fd.workedAreaTotal = 0;

            //reset GUI areas
            CalculateMinMax();

            displayFieldName = gStr.gsNone;
            FixTramModeButton();

            worldManager.isGeoMap = false;

            shape.Polygons.Clear();
        }

        //Does the logic to process section on off requests
        private void ProcessSectionOnOffRequests()
        {
            double timer = HzTime / (isFastSections ? 1 : 2);
            for (int j = 0; j < tool.numOfSections; j++)
            {
                //SECTIONS - 
                if (section[j].sectionOnRequest)
                {
                    section[j].isSectionOn = true;

                    section[j].sectionOverlapTimer = (int)Math.Max(timer * tool.turnOffDelay, 1);

                    if (!section[j].isMappingOn && section[j].mappingOnTimer == 0)
                        section[j].mappingOnTimer = (int)Math.Max(timer * tool.lookAheadOnSetting + 1, 1);//tool.mappingOnDelay

                    section[j].mappingOffTimer = (int)(timer * tool.lookAheadOffSetting + 2);//tool.mappingOffDelay
                }
                else if (section[j].sectionOverlapTimer > 0)
                {
                    section[j].sectionOverlapTimer--;
                    if (section[j].isSectionOn && section[j].sectionOverlapTimer == 0)
                        section[j].isSectionOn = false;
                    else
                        section[j].mappingOffTimer = (int)(timer * tool.lookAheadOffSetting + 2);//tool.mappingOffDelay
                }

                //MAPPING -
                if (section[tool.numOfSections].sectionOnRequest)
                {
                    section[j].mappingOnTimer = 2;
                    if (section[j].isMappingOn)
                        section[j].TurnMappingOff();
                }
                if (section[j].mappingOnTimer > 0 && section[j].mappingOffTimer > 1)
                {
                    section[j].mappingOnTimer--;
                    if (!section[j].isMappingOn && section[j].mappingOnTimer == 0)
                        section[j].TurnMappingOn();
                }
                if (section[j].mappingOffTimer > 0)
                {
                    section[j].mappingOffTimer--;
                    if (section[j].mappingOffTimer == 0)
                    {
                        section[j].mappingOnTimer = 0;
                        if (section[j].isMappingOn)
                            section[j].TurnMappingOff();
                    }
                }
            }

            if (section[tool.numOfSections].sectionOnRequest)
            {
                if (!section[tool.numOfSections].isMappingOn)
                    section[tool.numOfSections].TurnMappingOn();
            }
            else if (section[tool.numOfSections].isMappingOn)
                section[tool.numOfSections].TurnMappingOff();
        }

        //take the distance from object and convert to camera data
        public void SetZoom(double Delta)
        {
            worldManager.zoomValue += Delta * worldManager.zoomValue * (worldManager.zoomValue <= 20 ? 0.04 : 0.02);

            if (worldManager.zoomValue > 220) worldManager.zoomValue = 220;
            if (worldManager.zoomValue < 6.0) worldManager.zoomValue = 6.0;

            worldManager.camSetDistance = worldManager.zoomValue * worldManager.zoomValue * -1;

            //match grid to cam distance and redo perspective
            if (worldManager.camSetDistance > -50) worldManager.gridZoom = 10;
            else if (worldManager.camSetDistance > -150) worldManager.gridZoom = 20;
            else if (worldManager.camSetDistance > -250) worldManager.gridZoom = 40;
            else if (worldManager.camSetDistance > -500) worldManager.gridZoom = 80;
            else if (worldManager.camSetDistance > -1000) worldManager.gridZoom = 160;
            else if (worldManager.camSetDistance > -2000) worldManager.gridZoom = 320;
            else if (worldManager.camSetDistance > -5000) worldManager.gridZoom = 640;
            else if (worldManager.camSetDistance > -10000) worldManager.gridZoom = 1280;
            else if (worldManager.camSetDistance > -20000) worldManager.gridZoom = 2560;

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

        //message box pops up with info then goes away
        public void TimedMessageBox(int timeout, string s1, string s2)
        {
            FormTimedMessage form = new FormTimedMessage(timeout, s1, s2);
            form.Show(this);
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