namespace AgOpenGPS
{
    partial class FormGPS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGPS));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menustripLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDanish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDeutsch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSpanish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageFrench = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageItalian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageDutch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguagePolish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSlovak = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageUkranian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageRussian = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageTurkish = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.setWorkingDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.colorsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.topFieldViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.enterSimCoordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.simulatorOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetEverythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrWatchdog = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripFlag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFlagRed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagGrn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagYel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cboxpRowWidth = new System.Windows.Forms.ComboBox();
            this.oglZoom = new OpenTK.GLControl();
            this.panelDrag = new System.Windows.Forms.TableLayoutPanel();
            this.btnPathGoStop = new System.Windows.Forms.Button();
            this.btnPickPath = new System.Windows.Forms.Button();
            this.btnPathRecordStop = new System.Windows.Forms.Button();
            this.btnResumePath = new System.Windows.Forms.Button();
            this.btnResetSim = new System.Windows.Forms.Button();
            this.btnResetSteerAngle = new System.Windows.Forms.Button();
            this.timerSim = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hsbarSteerAngle = new System.Windows.Forms.HScrollBar();
            this.hsbarStepDistance = new System.Windows.Forms.HScrollBar();
            this.oglMain = new OpenTK.GLControl();
            this.panelSim = new System.Windows.Forms.TableLayoutPanel();
            this.btnSimSetSpeedToZero = new System.Windows.Forms.Button();
            this.oglBack = new OpenTK.GLControl();
            this.lblHz = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.TableLayoutPanel();
            this.btnCurve = new System.Windows.Forms.Button();
            this.btnContour = new System.Windows.Forms.Button();
            this.btnABLine = new System.Windows.Forms.Button();
            this.btnAutoYouTurn = new System.Windows.Forms.Button();
            this.btnSectionOffAutoOn = new System.Windows.Forms.Button();
            this.btnManualOffOn = new System.Windows.Forms.Button();
            this.btnCycleLines = new System.Windows.Forms.Button();
            this.btnAutoSteer = new System.Windows.Forms.Button();
            this.deleteContourPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAppliedAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteForSureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.steerChartStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.webcamToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetFixToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.angleChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correctionToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.panelNavigation = new System.Windows.Forms.TableLayoutPanel();
            this.btnN3D = new System.Windows.Forms.Button();
            this.btn2D = new System.Windows.Forms.Button();
            this.btnDayNightMode = new System.Windows.Forms.Button();
            this.btnZoomIn = new ProXoft.WinForms.RepeatButton();
            this.btnZoomOut = new ProXoft.WinForms.RepeatButton();
            this.btnpTiltDown = new ProXoft.WinForms.RepeatButton();
            this.btnpTiltUp = new ProXoft.WinForms.RepeatButton();
            this.btn3D = new System.Windows.Forms.Button();
            this.btnN2D = new System.Windows.Forms.Button();
            this.lblFieldStatus = new System.Windows.Forms.Label();
            this.panelAB = new System.Windows.Forms.TableLayoutPanel();
            this.btnYouSkipEnable = new System.Windows.Forms.Button();
            this.btnHeadlandOnOff = new System.Windows.Forms.Button();
            this.btnHydLift = new System.Windows.Forms.Button();
            this.btnTramDisplayMode = new System.Windows.Forms.Button();
            this.btnFlag = new System.Windows.Forms.Button();
            this.btnChangeMappingColor = new System.Windows.Forms.Button();
            this.btnABDraw = new System.Windows.Forms.Button();
            this.btnSnapToPivot = new System.Windows.Forms.Button();
            this.btnEditAB = new System.Windows.Forms.Button();
            this.lblSpeed = new System.Windows.Forms.Button();
            this.lblTopData = new System.Windows.Forms.Label();
            this.lblInty = new System.Windows.Forms.Label();
            this.lblCurveLineName = new System.Windows.Forms.Label();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.lblFix = new System.Windows.Forms.Label();
            this.lbludpWatchCounts = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStanleyPure = new System.Windows.Forms.Button();
            this.btnAutoSteerConfig = new System.Windows.Forms.Button();
            this.btnMaximizeMainForm = new System.Windows.Forms.Button();
            this.btnMinimizeMainForm = new System.Windows.Forms.Button();
            this.pictureboxStart = new System.Windows.Forms.PictureBox();
            this.btnStartAgIO = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.toolStripDropDownButton4 = new System.Windows.Forms.Button();
            this.toolStripBtnField = new System.Windows.Forms.Button();
            this.stripBtnConfig = new System.Windows.Forms.Button();
            this.simplifyToolStrip = new System.Windows.Forms.Button();
            this.distanceToolBtn = new System.Windows.Forms.Label();
            this.panelCaptionBar = new System.Windows.Forms.Panel();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.boundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tramLinesMenuField = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBtnMakeBndContour = new System.Windows.Forms.ToolStripMenuItem();
            this.recordedPathStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolSteerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripFlag.SuspendLayout();
            this.panelDrag.SuspendLayout();
            this.panelSim.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.panelAB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxStart)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelCaptionBar.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 22F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(96, 55);
            this.menuStrip1.TabIndex = 49;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator9,
            this.menustripLanguage,
            this.toolStripSeparator11,
            this.setWorkingDirectoryToolStripMenuItem,
            this.toolStripSeparator10,
            this.colorsToolStripMenuItem1,
            this.sectionColorToolStripMenuItem,
            this.toolStripSeparator8,
            this.topFieldViewToolStripMenuItem,
            this.toolStripSeparator3,
            this.enterSimCoordsToolStripMenuItem,
            this.toolStripSeparator4,
            this.simulatorOnToolStripMenuItem,
            this.resetALLToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.helpMenuItem});
            this.fileToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.fileMenu;
            this.fileToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(88, 55);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(316, 6);
            // 
            // menustripLanguage
            // 
            this.menustripLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLanguageDanish,
            this.menuLanguageDeutsch,
            this.menuLanguageEnglish,
            this.menuLanguageSpanish,
            this.menuLanguageFrench,
            this.menuLanguageItalian,
            this.menuLanguageDutch,
            this.menuLanguagePolish,
            this.menuLanguageSlovak,
            this.menuLanguageUkranian,
            this.menuLanguageRussian,
            this.menuLanguageTest,
            this.menuLanguageTurkish});
            this.menustripLanguage.Name = "menustripLanguage";
            this.menustripLanguage.Size = new System.Drawing.Size(319, 40);
            this.menustripLanguage.Text = "Language";
            // 
            // menuLanguageDanish
            // 
            this.menuLanguageDanish.Name = "menuLanguageDanish";
            this.menuLanguageDanish.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageDanish.Text = "Dansk (Denmark)";
            this.menuLanguageDanish.Click += new System.EventHandler(this.menuLanguageDanish_Click);
            // 
            // menuLanguageDeutsch
            // 
            this.menuLanguageDeutsch.CheckOnClick = true;
            this.menuLanguageDeutsch.Name = "menuLanguageDeutsch";
            this.menuLanguageDeutsch.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageDeutsch.Text = "Deutsch (Germany)";
            this.menuLanguageDeutsch.Click += new System.EventHandler(this.menuLanguageDeutsch_Click);
            // 
            // menuLanguageEnglish
            // 
            this.menuLanguageEnglish.CheckOnClick = true;
            this.menuLanguageEnglish.Name = "menuLanguageEnglish";
            this.menuLanguageEnglish.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageEnglish.Text = "English (Canada)";
            this.menuLanguageEnglish.Click += new System.EventHandler(this.menuLanguageEnglish_Click);
            // 
            // menuLanguageSpanish
            // 
            this.menuLanguageSpanish.CheckOnClick = true;
            this.menuLanguageSpanish.Name = "menuLanguageSpanish";
            this.menuLanguageSpanish.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageSpanish.Text = "Español (Spanish)";
            this.menuLanguageSpanish.Click += new System.EventHandler(this.menuLanguageSpanish_Click);
            // 
            // menuLanguageFrench
            // 
            this.menuLanguageFrench.CheckOnClick = true;
            this.menuLanguageFrench.Name = "menuLanguageFrench";
            this.menuLanguageFrench.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageFrench.Text = "Français (France)";
            this.menuLanguageFrench.Click += new System.EventHandler(this.menuLanguageFrench_Click);
            // 
            // menuLanguageItalian
            // 
            this.menuLanguageItalian.Name = "menuLanguageItalian";
            this.menuLanguageItalian.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageItalian.Text = "Italiano (Italy)";
            this.menuLanguageItalian.Click += new System.EventHandler(this.menuLanguageItalian_Click);
            // 
            // menuLanguageDutch
            // 
            this.menuLanguageDutch.CheckOnClick = true;
            this.menuLanguageDutch.Name = "menuLanguageDutch";
            this.menuLanguageDutch.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageDutch.Text = "Nederlands (Holland)";
            this.menuLanguageDutch.Click += new System.EventHandler(this.menuLanguageDutch_Click);
            // 
            // menuLanguagePolish
            // 
            this.menuLanguagePolish.Name = "menuLanguagePolish";
            this.menuLanguagePolish.Size = new System.Drawing.Size(372, 40);
            this.menuLanguagePolish.Text = "Polski (Poland)";
            this.menuLanguagePolish.Click += new System.EventHandler(this.menuLanguagesPolski_Click);
            // 
            // menuLanguageSlovak
            // 
            this.menuLanguageSlovak.Name = "menuLanguageSlovak";
            this.menuLanguageSlovak.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageSlovak.Text = "Slovenčina (Slovakia)";
            this.menuLanguageSlovak.Click += new System.EventHandler(this.menuLanguageSlovak_Click);
            // 
            // menuLanguageUkranian
            // 
            this.menuLanguageUkranian.Name = "menuLanguageUkranian";
            this.menuLanguageUkranian.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageUkranian.Text = "Yкраїнська (Ukraine)";
            this.menuLanguageUkranian.Click += new System.EventHandler(this.menuLanguageUkranian_Click);
            // 
            // menuLanguageRussian
            // 
            this.menuLanguageRussian.CheckOnClick = true;
            this.menuLanguageRussian.Name = "menuLanguageRussian";
            this.menuLanguageRussian.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageRussian.Text = "русский (Russia)";
            this.menuLanguageRussian.Click += new System.EventHandler(this.menuLanguageRussian_Click);
            // 
            // menuLanguageTest
            // 
            this.menuLanguageTest.Name = "menuLanguageTest";
            this.menuLanguageTest.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageTest.Text = "Test";
            this.menuLanguageTest.Click += new System.EventHandler(this.menuLanguageTest_Click);
            // 
            // menuLanguageTurkish
            // 
            this.menuLanguageTurkish.CheckOnClick = true;
            this.menuLanguageTurkish.Name = "menuLanguageTurkish";
            this.menuLanguageTurkish.Size = new System.Drawing.Size(372, 40);
            this.menuLanguageTurkish.Text = "Turkish (Türk)";
            this.menuLanguageTurkish.Click += new System.EventHandler(this.menuLanguageTurkish_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(316, 6);
            // 
            // setWorkingDirectoryToolStripMenuItem
            // 
            this.setWorkingDirectoryToolStripMenuItem.Name = "setWorkingDirectoryToolStripMenuItem";
            this.setWorkingDirectoryToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.setWorkingDirectoryToolStripMenuItem.Text = "Directories";
            this.setWorkingDirectoryToolStripMenuItem.Click += new System.EventHandler(this.setWorkingDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(316, 6);
            // 
            // colorsToolStripMenuItem1
            // 
            this.colorsToolStripMenuItem1.Name = "colorsToolStripMenuItem1";
            this.colorsToolStripMenuItem1.Size = new System.Drawing.Size(319, 40);
            this.colorsToolStripMenuItem1.Text = "Colors";
            this.colorsToolStripMenuItem1.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // sectionColorToolStripMenuItem
            // 
            this.sectionColorToolStripMenuItem.Name = "sectionColorToolStripMenuItem";
            this.sectionColorToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.sectionColorToolStripMenuItem.Text = "Section Colors";
            this.sectionColorToolStripMenuItem.Click += new System.EventHandler(this.colorsSectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(316, 6);
            // 
            // topFieldViewToolStripMenuItem
            // 
            this.topFieldViewToolStripMenuItem.Name = "topFieldViewToolStripMenuItem";
            this.topFieldViewToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.topFieldViewToolStripMenuItem.Text = "Top Field View";
            this.topFieldViewToolStripMenuItem.Click += new System.EventHandler(this.topFieldViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(316, 6);
            // 
            // enterSimCoordsToolStripMenuItem
            // 
            this.enterSimCoordsToolStripMenuItem.Name = "enterSimCoordsToolStripMenuItem";
            this.enterSimCoordsToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.enterSimCoordsToolStripMenuItem.Text = "Enter Sim Coords";
            this.enterSimCoordsToolStripMenuItem.Click += new System.EventHandler(this.enterSimCoordsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(316, 6);
            // 
            // simulatorOnToolStripMenuItem
            // 
            this.simulatorOnToolStripMenuItem.CheckOnClick = true;
            this.simulatorOnToolStripMenuItem.Name = "simulatorOnToolStripMenuItem";
            this.simulatorOnToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.simulatorOnToolStripMenuItem.Text = "Simulator On";
            this.simulatorOnToolStripMenuItem.Click += new System.EventHandler(this.simulatorOnToolStripMenuItem_Click);
            // 
            // resetALLToolStripMenuItem
            // 
            this.resetALLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetEverythingToolStripMenuItem});
            this.resetALLToolStripMenuItem.Name = "resetALLToolStripMenuItem";
            this.resetALLToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.resetALLToolStripMenuItem.Text = "Reset All";
            // 
            // resetEverythingToolStripMenuItem
            // 
            this.resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
            this.resetEverythingToolStripMenuItem.Size = new System.Drawing.Size(312, 40);
            this.resetEverythingToolStripMenuItem.Text = "Reset To Default";
            this.resetEverythingToolStripMenuItem.Click += new System.EventHandler(this.resetALLToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(319, 40);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(319, 40);
            this.helpMenuItem.Text = "Help";
            this.helpMenuItem.Click += new System.EventHandler(this.helpMenuItem_Click);
            // 
            // tmrWatchdog
            // 
            this.tmrWatchdog.Interval = 250;
            this.tmrWatchdog.Tick += new System.EventHandler(this.tmrWatchdog_tick);
            // 
            // contextMenuStripFlag
            // 
            this.contextMenuStripFlag.AutoSize = false;
            this.contextMenuStripFlag.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStripFlag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFlagRed,
            this.toolStripMenuFlagGrn,
            this.toolStripMenuFlagYel,
            this.toolStripMenuFlagForm,
            this.toolStripSeparator1});
            this.contextMenuStripFlag.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStripFlag.Name = "contextMenuStripFlag";
            this.contextMenuStripFlag.Size = new System.Drawing.Size(202, 312);
            // 
            // toolStripMenuItemFlagRed
            // 
            this.toolStripMenuItemFlagRed.AutoSize = false;
            this.toolStripMenuItemFlagRed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuItemFlagRed.Image = global::AgOpenGPS.Properties.Resources.FlagYel;
            this.toolStripMenuItemFlagRed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMenuItemFlagRed.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemFlagRed.Name = "toolStripMenuItemFlagRed";
            this.toolStripMenuItemFlagRed.Size = new System.Drawing.Size(200, 70);
            this.toolStripMenuItemFlagRed.Text = ".ffdfdf";
            this.toolStripMenuItemFlagRed.Click += new System.EventHandler(this.toolStripMenuItemFlagRed_Click);
            // 
            // toolStripMenuFlagGrn
            // 
            this.toolStripMenuFlagGrn.AutoSize = false;
            this.toolStripMenuFlagGrn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagGrn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagGrn.Image = global::AgOpenGPS.Properties.Resources.FlagGrn;
            this.toolStripMenuFlagGrn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagGrn.Name = "toolStripMenuFlagGrn";
            this.toolStripMenuFlagGrn.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagGrn.Text = ".";
            this.toolStripMenuFlagGrn.Click += new System.EventHandler(this.toolStripMenuGrn_Click);
            // 
            // toolStripMenuFlagYel
            // 
            this.toolStripMenuFlagYel.AutoSize = false;
            this.toolStripMenuFlagYel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagYel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagYel.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.toolStripMenuFlagYel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagYel.Name = "toolStripMenuFlagYel";
            this.toolStripMenuFlagYel.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagYel.Text = ".";
            this.toolStripMenuFlagYel.Click += new System.EventHandler(this.toolStripMenuYel_Click);
            // 
            // toolStripMenuFlagForm
            // 
            this.toolStripMenuFlagForm.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.toolStripMenuFlagForm.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagForm.Name = "toolStripMenuFlagForm";
            this.toolStripMenuFlagForm.Size = new System.Drawing.Size(228, 70);
            this.toolStripMenuFlagForm.Text = "toolStripMenuItem3";
            this.toolStripMenuFlagForm.Click += new System.EventHandler(this.toolStripMenuFlagForm_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // cboxpRowWidth
            // 
            this.cboxpRowWidth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxpRowWidth.BackColor = System.Drawing.Color.Lavender;
            this.cboxpRowWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxpRowWidth.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxpRowWidth.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxpRowWidth.FormattingEnabled = true;
            this.cboxpRowWidth.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboxpRowWidth.Location = new System.Drawing.Point(724, 9);
            this.cboxpRowWidth.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxpRowWidth.Name = "cboxpRowWidth";
            this.cboxpRowWidth.Size = new System.Drawing.Size(44, 41);
            this.cboxpRowWidth.TabIndex = 247;
            this.cboxpRowWidth.Visible = false;
            this.cboxpRowWidth.SelectedIndexChanged += new System.EventHandler(this.cboxpRowWidth_SelectedIndexChanged);
            this.cboxpRowWidth.Click += new System.EventHandler(this.cboxpRowWidth_Click);
            // 
            // oglZoom
            // 
            this.oglZoom.BackColor = System.Drawing.Color.Black;
            this.oglZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oglZoom.Location = new System.Drawing.Point(300, 80);
            this.oglZoom.Margin = new System.Windows.Forms.Padding(0);
            this.oglZoom.Name = "oglZoom";
            this.oglZoom.Size = new System.Drawing.Size(400, 400);
            this.oglZoom.TabIndex = 182;
            this.oglZoom.VSync = false;
            this.oglZoom.Load += new System.EventHandler(this.oglZoom_Load);
            this.oglZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.oglZoom_Paint);
            this.oglZoom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.oglZoom_MouseClick);
            this.oglZoom.Resize += new System.EventHandler(this.oglZoom_Resize);
            // 
            // panelDrag
            // 
            this.panelDrag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelDrag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelDrag.ColumnCount = 1;
            this.panelDrag.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelDrag.Controls.Add(this.btnPathGoStop, 0, 0);
            this.panelDrag.Controls.Add(this.btnPickPath, 0, 3);
            this.panelDrag.Controls.Add(this.btnPathRecordStop, 0, 2);
            this.panelDrag.Controls.Add(this.btnResumePath, 0, 1);
            this.panelDrag.Location = new System.Drawing.Point(323, 122);
            this.panelDrag.Name = "panelDrag";
            this.panelDrag.RowCount = 5;
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelDrag.Size = new System.Drawing.Size(66, 320);
            this.panelDrag.TabIndex = 445;
            this.panelDrag.Visible = false;
            // 
            // btnPathGoStop
            // 
            this.btnPathGoStop.BackColor = System.Drawing.Color.Transparent;
            this.btnPathGoStop.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPathGoStop.FlatAppearance.BorderSize = 0;
            this.btnPathGoStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathGoStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPathGoStop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPathGoStop.Image = global::AgOpenGPS.Properties.Resources.boundaryPlay;
            this.btnPathGoStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPathGoStop.Location = new System.Drawing.Point(3, 3);
            this.btnPathGoStop.Name = "btnPathGoStop";
            this.btnPathGoStop.Size = new System.Drawing.Size(60, 60);
            this.btnPathGoStop.TabIndex = 468;
            this.btnPathGoStop.UseVisualStyleBackColor = false;
            this.btnPathGoStop.Click += new System.EventHandler(this.btnPathGoStop_Click);
            // 
            // btnPickPath
            // 
            this.btnPickPath.BackColor = System.Drawing.Color.Transparent;
            this.btnPickPath.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPickPath.FlatAppearance.BorderSize = 0;
            this.btnPickPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPickPath.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickPath.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPickPath.Image = global::AgOpenGPS.Properties.Resources.FileExplorerWindows;
            this.btnPickPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPickPath.Location = new System.Drawing.Point(3, 201);
            this.btnPickPath.Name = "btnPickPath";
            this.btnPickPath.Size = new System.Drawing.Size(60, 60);
            this.btnPickPath.TabIndex = 471;
            this.btnPickPath.UseVisualStyleBackColor = false;
            this.btnPickPath.Click += new System.EventHandler(this.btnPickPath_Click);
            // 
            // btnPathRecordStop
            // 
            this.btnPathRecordStop.BackColor = System.Drawing.Color.Transparent;
            this.btnPathRecordStop.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPathRecordStop.FlatAppearance.BorderSize = 0;
            this.btnPathRecordStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathRecordStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPathRecordStop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPathRecordStop.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnPathRecordStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPathRecordStop.Location = new System.Drawing.Point(3, 135);
            this.btnPathRecordStop.Name = "btnPathRecordStop";
            this.btnPathRecordStop.Size = new System.Drawing.Size(60, 60);
            this.btnPathRecordStop.TabIndex = 470;
            this.btnPathRecordStop.UseVisualStyleBackColor = false;
            this.btnPathRecordStop.Click += new System.EventHandler(this.btnPathRecordStop_Click);
            // 
            // btnResumePath
            // 
            this.btnResumePath.BackColor = System.Drawing.Color.Transparent;
            this.btnResumePath.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnResumePath.FlatAppearance.BorderSize = 0;
            this.btnResumePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumePath.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResumePath.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnResumePath.Image = global::AgOpenGPS.Properties.Resources.pathResumeStart;
            this.btnResumePath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResumePath.Location = new System.Drawing.Point(3, 69);
            this.btnResumePath.Name = "btnResumePath";
            this.btnResumePath.Size = new System.Drawing.Size(60, 60);
            this.btnResumePath.TabIndex = 472;
            this.btnResumePath.UseVisualStyleBackColor = false;
            this.btnResumePath.Click += new System.EventHandler(this.btnResumePath_Click);
            // 
            // btnResetSim
            // 
            this.btnResetSim.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSim.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnResetSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSim.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSim.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSim.Location = new System.Drawing.Point(528, 4);
            this.btnResetSim.Name = "btnResetSim";
            this.btnResetSim.Size = new System.Drawing.Size(50, 30);
            this.btnResetSim.TabIndex = 164;
            this.btnResetSim.Text = "Reset";
            this.btnResetSim.UseVisualStyleBackColor = false;
            this.btnResetSim.Click += new System.EventHandler(this.btnResetSim_Click);
            // 
            // btnResetSteerAngle
            // 
            this.btnResetSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSteerAngle.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnResetSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSteerAngle.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSteerAngle.Location = new System.Drawing.Point(471, 4);
            this.btnResetSteerAngle.Name = "btnResetSteerAngle";
            this.btnResetSteerAngle.Size = new System.Drawing.Size(50, 30);
            this.btnResetSteerAngle.TabIndex = 162;
            this.btnResetSteerAngle.Text = ">0<";
            this.btnResetSteerAngle.UseVisualStyleBackColor = false;
            this.btnResetSteerAngle.Click += new System.EventHandler(this.btnResetSteerAngle_Click);
            // 
            // timerSim
            // 
            this.timerSim.Enabled = true;
            this.timerSim.Interval = 94;
            this.timerSim.Tick += new System.EventHandler(this.timerSim_Tick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(334, 62);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // hsbarSteerAngle
            // 
            this.hsbarSteerAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsbarSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarSteerAngle.LargeChange = 20;
            this.hsbarSteerAngle.Location = new System.Drawing.Point(263, 1);
            this.hsbarSteerAngle.Maximum = 800;
            this.hsbarSteerAngle.Name = "hsbarSteerAngle";
            this.hsbarSteerAngle.Size = new System.Drawing.Size(204, 36);
            this.hsbarSteerAngle.TabIndex = 179;
            this.hsbarSteerAngle.Value = 400;
            this.hsbarSteerAngle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSteerAngle_Scroll);
            // 
            // hsbarStepDistance
            // 
            this.hsbarStepDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsbarStepDistance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarStepDistance.LargeChange = 2;
            this.hsbarStepDistance.Location = new System.Drawing.Point(1, 1);
            this.hsbarStepDistance.Maximum = 101;
            this.hsbarStepDistance.Minimum = -25;
            this.hsbarStepDistance.Name = "hsbarStepDistance";
            this.hsbarStepDistance.Size = new System.Drawing.Size(204, 36);
            this.hsbarStepDistance.TabIndex = 178;
            this.hsbarStepDistance.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarStepDistance_Scroll);
            // 
            // oglMain
            // 
            this.oglMain.BackColor = System.Drawing.Color.Black;
            this.oglMain.CausesValidation = false;
            this.oglMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oglMain.Location = new System.Drawing.Point(60, 0);
            this.oglMain.Margin = new System.Windows.Forms.Padding(0);
            this.oglMain.Name = "oglMain";
            this.oglMain.Size = new System.Drawing.Size(880, 600);
            this.oglMain.TabIndex = 180;
            this.oglMain.VSync = false;
            this.oglMain.Load += new System.EventHandler(this.oglMain_Load);
            this.oglMain.Paint += new System.Windows.Forms.PaintEventHandler(this.oglMain_Paint);
            this.oglMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglMain_MouseDown);
            this.oglMain.Resize += new System.EventHandler(this.oglMain_Resize);
            // 
            // panelSim
            // 
            this.panelSim.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelSim.BackColor = System.Drawing.Color.White;
            this.panelSim.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panelSim.ColumnCount = 5;
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.panelSim.Controls.Add(this.hsbarStepDistance, 0, 0);
            this.panelSim.Controls.Add(this.btnSimSetSpeedToZero, 1, 0);
            this.panelSim.Controls.Add(this.hsbarSteerAngle, 2, 0);
            this.panelSim.Controls.Add(this.btnResetSteerAngle, 3, 0);
            this.panelSim.Controls.Add(this.btnResetSim, 4, 0);
            this.panelSim.Location = new System.Drawing.Point(262, 550);
            this.panelSim.Name = "panelSim";
            this.panelSim.RowCount = 1;
            this.panelSim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSim.Size = new System.Drawing.Size(600, 38);
            this.panelSim.TabIndex = 325;
            // 
            // btnSimSetSpeedToZero
            // 
            this.btnSimSetSpeedToZero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSimSetSpeedToZero.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSetSpeedToZero.BackgroundImage = global::AgOpenGPS.Properties.Resources.AutoStop;
            this.btnSimSetSpeedToZero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSetSpeedToZero.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnSimSetSpeedToZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSetSpeedToZero.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimSetSpeedToZero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimSetSpeedToZero.Location = new System.Drawing.Point(209, 4);
            this.btnSimSetSpeedToZero.Name = "btnSimSetSpeedToZero";
            this.btnSimSetSpeedToZero.Size = new System.Drawing.Size(50, 30);
            this.btnSimSetSpeedToZero.TabIndex = 453;
            this.btnSimSetSpeedToZero.UseVisualStyleBackColor = false;
            this.btnSimSetSpeedToZero.Click += new System.EventHandler(this.btnSimSetSpeedToZero_Click);
            // 
            // oglBack
            // 
            this.oglBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.oglBack.BackColor = System.Drawing.Color.Black;
            this.oglBack.ForeColor = System.Drawing.Color.Transparent;
            this.oglBack.Location = new System.Drawing.Point(264, 67);
            this.oglBack.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.oglBack.Name = "oglBack";
            this.oglBack.Size = new System.Drawing.Size(500, 300);
            this.oglBack.TabIndex = 181;
            this.oglBack.VSync = false;
            this.oglBack.Load += new System.EventHandler(this.oglBack_Load);
            this.oglBack.Paint += new System.Windows.Forms.PaintEventHandler(this.oglBack_Paint);
            // 
            // lblHz
            // 
            this.lblHz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.lblHz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHz.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHz.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHz.Location = new System.Drawing.Point(75, 312);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(73, 82);
            this.lblHz.TabIndex = 249;
            this.lblHz.Text = "5 Hz 32\r\nPPS";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHz.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // panelRight
            // 
            this.panelRight.ColumnCount = 1;
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.Controls.Add(this.btnCurve, 0, 1);
            this.panelRight.Controls.Add(this.btnContour, 0, 0);
            this.panelRight.Controls.Add(this.btnABLine, 0, 2);
            this.panelRight.Controls.Add(this.btnAutoYouTurn, 0, 8);
            this.panelRight.Controls.Add(this.btnSectionOffAutoOn, 0, 6);
            this.panelRight.Controls.Add(this.btnManualOffOn, 0, 5);
            this.panelRight.Controls.Add(this.btnCycleLines, 0, 3);
            this.panelRight.Controls.Add(this.btnAutoSteer, 0, 9);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(940, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.RowCount = 10;
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.554738F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.10946F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.55973F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1096F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11849F));
            this.panelRight.Size = new System.Drawing.Size(60, 660);
            this.panelRight.TabIndex = 320;
            this.panelRight.Visible = false;
            // 
            // btnCurve
            // 
            this.btnCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCurve.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCurve.FlatAppearance.BorderSize = 0;
            this.btnCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurve.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurve.Image = global::AgOpenGPS.Properties.Resources.CurveOff;
            this.btnCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCurve.Location = new System.Drawing.Point(0, 73);
            this.btnCurve.Margin = new System.Windows.Forms.Padding(0);
            this.btnCurve.Name = "btnCurve";
            this.btnCurve.Size = new System.Drawing.Size(60, 73);
            this.btnCurve.TabIndex = 173;
            this.btnCurve.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCurve.UseVisualStyleBackColor = false;
            this.btnCurve.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // btnContour
            // 
            this.btnContour.BackColor = System.Drawing.Color.Transparent;
            this.btnContour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnContour.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnContour.FlatAppearance.BorderSize = 0;
            this.btnContour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContour.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContour.Image = global::AgOpenGPS.Properties.Resources.ContourOff;
            this.btnContour.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContour.Location = new System.Drawing.Point(0, 0);
            this.btnContour.Margin = new System.Windows.Forms.Padding(0);
            this.btnContour.Name = "btnContour";
            this.btnContour.Size = new System.Drawing.Size(60, 73);
            this.btnContour.TabIndex = 105;
            this.btnContour.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContour.UseVisualStyleBackColor = false;
            this.btnContour.Click += new System.EventHandler(this.btnContour_Click);
            // 
            // btnABLine
            // 
            this.btnABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnABLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnABLine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnABLine.FlatAppearance.BorderSize = 0;
            this.btnABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnABLine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOff;
            this.btnABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnABLine.Location = new System.Drawing.Point(0, 146);
            this.btnABLine.Margin = new System.Windows.Forms.Padding(0);
            this.btnABLine.Name = "btnABLine";
            this.btnABLine.Size = new System.Drawing.Size(60, 73);
            this.btnABLine.TabIndex = 0;
            this.btnABLine.UseVisualStyleBackColor = false;
            this.btnABLine.Click += new System.EventHandler(this.btnABLine_Click);
            // 
            // btnAutoYouTurn
            // 
            this.btnAutoYouTurn.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoYouTurn.Enabled = false;
            this.btnAutoYouTurn.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoYouTurn.FlatAppearance.BorderSize = 0;
            this.btnAutoYouTurn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoYouTurn.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAutoYouTurn.Image = global::AgOpenGPS.Properties.Resources.YouTurnNo;
            this.btnAutoYouTurn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoYouTurn.Location = new System.Drawing.Point(0, 510);
            this.btnAutoYouTurn.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoYouTurn.Name = "btnAutoYouTurn";
            this.btnAutoYouTurn.Size = new System.Drawing.Size(60, 73);
            this.btnAutoYouTurn.TabIndex = 132;
            this.btnAutoYouTurn.UseVisualStyleBackColor = false;
            this.btnAutoYouTurn.Click += new System.EventHandler(this.btnAutoYouTurn_Click);
            // 
            // btnSectionOffAutoOn
            // 
            this.btnSectionOffAutoOn.BackColor = System.Drawing.Color.Transparent;
            this.btnSectionOffAutoOn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSectionOffAutoOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSectionOffAutoOn.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSectionOffAutoOn.FlatAppearance.BorderSize = 0;
            this.btnSectionOffAutoOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionOffAutoOn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionOffAutoOn.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.btnSectionOffAutoOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSectionOffAutoOn.Location = new System.Drawing.Point(0, 401);
            this.btnSectionOffAutoOn.Margin = new System.Windows.Forms.Padding(0);
            this.btnSectionOffAutoOn.Name = "btnSectionOffAutoOn";
            this.btnSectionOffAutoOn.Size = new System.Drawing.Size(60, 73);
            this.btnSectionOffAutoOn.TabIndex = 152;
            this.btnSectionOffAutoOn.UseVisualStyleBackColor = false;
            this.btnSectionOffAutoOn.Click += new System.EventHandler(this.btnSectionOffAutoOn_Click);
            // 
            // btnManualOffOn
            // 
            this.btnManualOffOn.BackColor = System.Drawing.Color.Transparent;
            this.btnManualOffOn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnManualOffOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManualOffOn.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnManualOffOn.FlatAppearance.BorderSize = 0;
            this.btnManualOffOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualOffOn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualOffOn.Image = global::AgOpenGPS.Properties.Resources.ManualOff;
            this.btnManualOffOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnManualOffOn.Location = new System.Drawing.Point(0, 328);
            this.btnManualOffOn.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOffOn.Name = "btnManualOffOn";
            this.btnManualOffOn.Size = new System.Drawing.Size(60, 73);
            this.btnManualOffOn.TabIndex = 98;
            this.btnManualOffOn.UseVisualStyleBackColor = false;
            this.btnManualOffOn.Click += new System.EventHandler(this.btnManualOffOn_Click);
            // 
            // btnCycleLines
            // 
            this.btnCycleLines.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCycleLines.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCycleLines.FlatAppearance.BorderSize = 0;
            this.btnCycleLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleLines.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleLines.Image = global::AgOpenGPS.Properties.Resources.ABLineCycle;
            this.btnCycleLines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleLines.Location = new System.Drawing.Point(0, 219);
            this.btnCycleLines.Margin = new System.Windows.Forms.Padding(0);
            this.btnCycleLines.Name = "btnCycleLines";
            this.btnCycleLines.Size = new System.Drawing.Size(60, 73);
            this.btnCycleLines.TabIndex = 251;
            this.btnCycleLines.UseVisualStyleBackColor = false;
            this.btnCycleLines.Click += new System.EventHandler(this.btnCycleLines_Click);
            // 
            // btnAutoSteer
            // 
            this.btnAutoSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoSteer.FlatAppearance.BorderSize = 0;
            this.btnAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteer.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnAutoSteer.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.btnAutoSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteer.Location = new System.Drawing.Point(0, 583);
            this.btnAutoSteer.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteer.Name = "btnAutoSteer";
            this.btnAutoSteer.Size = new System.Drawing.Size(60, 77);
            this.btnAutoSteer.TabIndex = 128;
            this.btnAutoSteer.UseVisualStyleBackColor = false;
            this.btnAutoSteer.Click += new System.EventHandler(this.btnAutoSteer_Click);
            // 
            // deleteContourPathsToolStripMenuItem
            // 
            this.deleteContourPathsToolStripMenuItem.Enabled = false;
            this.deleteContourPathsToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.HideContour;
            this.deleteContourPathsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteContourPathsToolStripMenuItem.Name = "deleteContourPathsToolStripMenuItem";
            this.deleteContourPathsToolStripMenuItem.Size = new System.Drawing.Size(404, 70);
            this.deleteContourPathsToolStripMenuItem.Text = "Hide Contour Paths";
            this.deleteContourPathsToolStripMenuItem.Click += new System.EventHandler(this.deleteContourPathsToolStripMenuItem_Click);
            // 
            // deleteAppliedAreaToolStripMenuItem
            // 
            this.deleteAppliedAreaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteForSureToolStripMenuItem});
            this.deleteAppliedAreaToolStripMenuItem.Enabled = false;
            this.deleteAppliedAreaToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.skull;
            this.deleteAppliedAreaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteAppliedAreaToolStripMenuItem.Name = "deleteAppliedAreaToolStripMenuItem";
            this.deleteAppliedAreaToolStripMenuItem.Size = new System.Drawing.Size(404, 70);
            this.deleteAppliedAreaToolStripMenuItem.Text = "Delete Applied Area";
            // 
            // deleteForSureToolStripMenuItem
            // 
            this.deleteForSureToolStripMenuItem.Name = "deleteForSureToolStripMenuItem";
            this.deleteForSureToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.deleteForSureToolStripMenuItem.Text = "Delete For Sure";
            this.deleteForSureToolStripMenuItem.Click += new System.EventHandler(this.toolStripAreYouSure_Click);
            // 
            // steerChartStripMenu
            // 
            this.steerChartStripMenu.Image = global::AgOpenGPS.Properties.Resources.Chart;
            this.steerChartStripMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.steerChartStripMenu.Name = "steerChartStripMenu";
            this.steerChartStripMenu.Size = new System.Drawing.Size(404, 70);
            this.steerChartStripMenu.Text = "Steer Chart";
            this.steerChartStripMenu.Click += new System.EventHandler(this.toolStripAutoSteerChart_Click);
            // 
            // webcamToolStrip
            // 
            this.webcamToolStrip.Image = global::AgOpenGPS.Properties.Resources.Webcam;
            this.webcamToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.webcamToolStrip.Name = "webcamToolStrip";
            this.webcamToolStrip.Size = new System.Drawing.Size(404, 70);
            this.webcamToolStrip.Text = "WebCam";
            this.webcamToolStrip.Click += new System.EventHandler(this.webcamToolStrip_Click);
            // 
            // offsetFixToolStrip
            // 
            this.offsetFixToolStrip.Image = global::AgOpenGPS.Properties.Resources.YouTurnReverse;
            this.offsetFixToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.offsetFixToolStrip.Name = "offsetFixToolStrip";
            this.offsetFixToolStrip.Size = new System.Drawing.Size(404, 70);
            this.offsetFixToolStrip.Text = "Offset Fix";
            this.offsetFixToolStrip.Click += new System.EventHandler(this.offsetFixToolStrip_Click);
            // 
            // angleChartToolStripMenuItem
            // 
            this.angleChartToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_SourcesHeading;
            this.angleChartToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.angleChartToolStripMenuItem.Name = "angleChartToolStripMenuItem";
            this.angleChartToolStripMenuItem.ShowShortcutKeys = false;
            this.angleChartToolStripMenuItem.Size = new System.Drawing.Size(404, 70);
            this.angleChartToolStripMenuItem.Text = "Heading Chart";
            this.angleChartToolStripMenuItem.Click += new System.EventHandler(this.headingChartToolStripMenuItem_Click);
            // 
            // correctionToolStrip
            // 
            this.correctionToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.correctionToolStrip.Name = "correctionToolStrip";
            this.correctionToolStrip.ShowShortcutKeys = false;
            this.correctionToolStrip.Size = new System.Drawing.Size(404, 70);
            this.correctionToolStrip.Text = "Roll & Easting";
            this.correctionToolStrip.Click += new System.EventHandler(this.correctionToolStrip_Click);
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelNavigation.ColumnCount = 2;
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.panelNavigation.Controls.Add(this.btnN3D, 0, 3);
            this.panelNavigation.Controls.Add(this.btn2D, 0, 0);
            this.panelNavigation.Controls.Add(this.btnDayNightMode, 0, 4);
            this.panelNavigation.Controls.Add(this.btnZoomIn, 1, 3);
            this.panelNavigation.Controls.Add(this.btnZoomOut, 1, 2);
            this.panelNavigation.Controls.Add(this.btnpTiltDown, 1, 1);
            this.panelNavigation.Controls.Add(this.btnpTiltUp, 1, 0);
            this.panelNavigation.Controls.Add(this.btn3D, 0, 1);
            this.panelNavigation.Controls.Add(this.btnN2D, 0, 2);
            this.panelNavigation.Controls.Add(this.lblHz, 1, 4);
            this.panelNavigation.Location = new System.Drawing.Point(140, 7);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.RowCount = 5;
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelNavigation.Size = new System.Drawing.Size(151, 394);
            this.panelNavigation.TabIndex = 468;
            this.panelNavigation.Visible = false;
            // 
            // btnN3D
            // 
            this.btnN3D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnN3D.BackColor = System.Drawing.Color.Transparent;
            this.btnN3D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnN3D.FlatAppearance.BorderSize = 0;
            this.btnN3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnN3D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnN3D.Image = global::AgOpenGPS.Properties.Resources.CameraNorth64;
            this.btnN3D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnN3D.Location = new System.Drawing.Point(6, 243);
            this.btnN3D.Name = "btnN3D";
            this.btnN3D.Size = new System.Drawing.Size(60, 60);
            this.btnN3D.TabIndex = 472;
            this.btnN3D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnN3D.UseVisualStyleBackColor = false;
            this.btnN3D.Click += new System.EventHandler(this.btnN3D_Click);
            // 
            // btn2D
            // 
            this.btn2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn2D.BackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn2D.FlatAppearance.BorderSize = 0;
            this.btn2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2D.Image = global::AgOpenGPS.Properties.Resources.Camera2D64;
            this.btn2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn2D.Location = new System.Drawing.Point(6, 9);
            this.btn2D.Name = "btn2D";
            this.btn2D.Size = new System.Drawing.Size(60, 60);
            this.btn2D.TabIndex = 469;
            this.btn2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn2D.UseVisualStyleBackColor = false;
            this.btn2D.Click += new System.EventHandler(this.btn2D_Click);
            // 
            // btnDayNightMode
            // 
            this.btnDayNightMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDayNightMode.BackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatAppearance.BorderSize = 0;
            this.btnDayNightMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayNightMode.Image = global::AgOpenGPS.Properties.Resources.WindowNightMode;
            this.btnDayNightMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDayNightMode.Location = new System.Drawing.Point(6, 323);
            this.btnDayNightMode.Name = "btnDayNightMode";
            this.btnDayNightMode.Size = new System.Drawing.Size(60, 60);
            this.btnDayNightMode.TabIndex = 452;
            this.btnDayNightMode.UseVisualStyleBackColor = false;
            this.btnDayNightMode.Click += new System.EventHandler(this.btnDayNightMode_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnZoomIn.Image = global::AgOpenGPS.Properties.Resources.ZoomIn48;
            this.btnZoomIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZoomIn.Location = new System.Drawing.Point(81, 243);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(60, 60);
            this.btnZoomIn.TabIndex = 120;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZoomIn_MouseDown);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnZoomOut.Image = global::AgOpenGPS.Properties.Resources.ZoomOut48;
            this.btnZoomOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZoomOut.Location = new System.Drawing.Point(81, 165);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(60, 60);
            this.btnZoomOut.TabIndex = 119;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZoomOut_MouseDown);
            // 
            // btnpTiltDown
            // 
            this.btnpTiltDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnpTiltDown.BackColor = System.Drawing.Color.Transparent;
            this.btnpTiltDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnpTiltDown.FlatAppearance.BorderSize = 0;
            this.btnpTiltDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpTiltDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnpTiltDown.Image = global::AgOpenGPS.Properties.Resources.TiltDown;
            this.btnpTiltDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnpTiltDown.Location = new System.Drawing.Point(81, 87);
            this.btnpTiltDown.Name = "btnpTiltDown";
            this.btnpTiltDown.Size = new System.Drawing.Size(60, 60);
            this.btnpTiltDown.TabIndex = 446;
            this.btnpTiltDown.UseVisualStyleBackColor = false;
            this.btnpTiltDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnpTiltDown_MouseDown);
            // 
            // btnpTiltUp
            // 
            this.btnpTiltUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnpTiltUp.BackColor = System.Drawing.Color.Transparent;
            this.btnpTiltUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnpTiltUp.FlatAppearance.BorderSize = 0;
            this.btnpTiltUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpTiltUp.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnpTiltUp.Image = global::AgOpenGPS.Properties.Resources.TiltUp;
            this.btnpTiltUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnpTiltUp.Location = new System.Drawing.Point(81, 9);
            this.btnpTiltUp.Name = "btnpTiltUp";
            this.btnpTiltUp.Size = new System.Drawing.Size(60, 60);
            this.btnpTiltUp.TabIndex = 447;
            this.btnpTiltUp.UseVisualStyleBackColor = false;
            this.btnpTiltUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnpTiltUp_MouseDown);
            // 
            // btn3D
            // 
            this.btn3D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn3D.BackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn3D.FlatAppearance.BorderSize = 0;
            this.btn3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3D.Image = global::AgOpenGPS.Properties.Resources.Camera3D64;
            this.btn3D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn3D.Location = new System.Drawing.Point(6, 87);
            this.btn3D.Name = "btn3D";
            this.btn3D.Size = new System.Drawing.Size(60, 60);
            this.btn3D.TabIndex = 471;
            this.btn3D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn3D.UseVisualStyleBackColor = false;
            this.btn3D.Click += new System.EventHandler(this.btn3D_Click);
            // 
            // btnN2D
            // 
            this.btnN2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnN2D.BackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnN2D.FlatAppearance.BorderSize = 0;
            this.btnN2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnN2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnN2D.Image = global::AgOpenGPS.Properties.Resources.CameraNorth2D;
            this.btnN2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnN2D.Location = new System.Drawing.Point(6, 165);
            this.btnN2D.Name = "btnN2D";
            this.btnN2D.Size = new System.Drawing.Size(60, 60);
            this.btnN2D.TabIndex = 470;
            this.btnN2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnN2D.UseVisualStyleBackColor = false;
            this.btnN2D.Click += new System.EventHandler(this.btnN2D_Click);
            // 
            // lblFieldStatus
            // 
            this.lblFieldStatus.AutoSize = true;
            this.lblFieldStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblFieldStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFieldStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFieldStatus.Location = new System.Drawing.Point(101, 34);
            this.lblFieldStatus.Name = "lblFieldStatus";
            this.lblFieldStatus.Size = new System.Drawing.Size(66, 23);
            this.lblFieldStatus.TabIndex = 469;
            this.lblFieldStatus.Text = "25 Ha";
            this.lblFieldStatus.Visible = false;
            // 
            // panelAB
            // 
            this.panelAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelAB.BackColor = System.Drawing.Color.Transparent;
            this.panelAB.ColumnCount = 10;
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.278351F));
            this.panelAB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.247422F));
            this.panelAB.Controls.Add(this.btnYouSkipEnable, 8, 0);
            this.panelAB.Controls.Add(this.btnHeadlandOnOff, 0, 0);
            this.panelAB.Controls.Add(this.btnHydLift, 1, 0);
            this.panelAB.Controls.Add(this.btnTramDisplayMode, 2, 0);
            this.panelAB.Controls.Add(this.btnFlag, 3, 0);
            this.panelAB.Controls.Add(this.btnChangeMappingColor, 4, 0);
            this.panelAB.Controls.Add(this.btnABDraw, 5, 0);
            this.panelAB.Controls.Add(this.btnSnapToPivot, 6, 0);
            this.panelAB.Controls.Add(this.btnEditAB, 7, 0);
            this.panelAB.Controls.Add(this.cboxpRowWidth, 9, 0);
            this.panelAB.Location = new System.Drawing.Point(50, 0);
            this.panelAB.Margin = new System.Windows.Forms.Padding(0);
            this.panelAB.Name = "panelAB";
            this.panelAB.RowCount = 1;
            this.panelAB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelAB.Size = new System.Drawing.Size(780, 60);
            this.panelAB.TabIndex = 480;
            // 
            // btnYouSkipEnable
            // 
            this.btnYouSkipEnable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnYouSkipEnable.BackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnYouSkipEnable.FlatAppearance.BorderSize = 0;
            this.btnYouSkipEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYouSkipEnable.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYouSkipEnable.Image = global::AgOpenGPS.Properties.Resources.YouSkipOff;
            this.btnYouSkipEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYouSkipEnable.Location = new System.Drawing.Point(646, 0);
            this.btnYouSkipEnable.Margin = new System.Windows.Forms.Padding(0);
            this.btnYouSkipEnable.Name = "btnYouSkipEnable";
            this.btnYouSkipEnable.Size = new System.Drawing.Size(60, 60);
            this.btnYouSkipEnable.TabIndex = 490;
            this.btnYouSkipEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYouSkipEnable.UseVisualStyleBackColor = false;
            this.btnYouSkipEnable.Visible = false;
            this.btnYouSkipEnable.Click += new System.EventHandler(this.btnYouSkipEnable_Click);
            // 
            // btnHeadlandOnOff
            // 
            this.btnHeadlandOnOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOnOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHeadlandOnOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeadlandOnOff.Image = global::AgOpenGPS.Properties.Resources.HeadlandOff;
            this.btnHeadlandOnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOnOff.Location = new System.Drawing.Point(10, 0);
            this.btnHeadlandOnOff.Margin = new System.Windows.Forms.Padding(0);
            this.btnHeadlandOnOff.Name = "btnHeadlandOnOff";
            this.btnHeadlandOnOff.Size = new System.Drawing.Size(60, 60);
            this.btnHeadlandOnOff.TabIndex = 447;
            this.btnHeadlandOnOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHeadlandOnOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOnOff.Visible = false;
            this.btnHeadlandOnOff.Click += new System.EventHandler(this.btnHeadlandOnOff_Click);
            // 
            // btnHydLift
            // 
            this.btnHydLift.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHydLift.BackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHydLift.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnHydLift.FlatAppearance.BorderSize = 0;
            this.btnHydLift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHydLift.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnHydLift.Image = global::AgOpenGPS.Properties.Resources.HydraulicLiftOff;
            this.btnHydLift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHydLift.Location = new System.Drawing.Point(90, 0);
            this.btnHydLift.Margin = new System.Windows.Forms.Padding(0);
            this.btnHydLift.Name = "btnHydLift";
            this.btnHydLift.Size = new System.Drawing.Size(60, 60);
            this.btnHydLift.TabIndex = 453;
            this.btnHydLift.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHydLift.UseVisualStyleBackColor = false;
            this.btnHydLift.Visible = false;
            this.btnHydLift.Click += new System.EventHandler(this.btnHydLift_Click);
            // 
            // btnTramDisplayMode
            // 
            this.btnTramDisplayMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTramDisplayMode.BackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnTramDisplayMode.FlatAppearance.BorderSize = 0;
            this.btnTramDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTramDisplayMode.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTramDisplayMode.Image = global::AgOpenGPS.Properties.Resources.TramOff;
            this.btnTramDisplayMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTramDisplayMode.Location = new System.Drawing.Point(170, 0);
            this.btnTramDisplayMode.Margin = new System.Windows.Forms.Padding(0);
            this.btnTramDisplayMode.Name = "btnTramDisplayMode";
            this.btnTramDisplayMode.Size = new System.Drawing.Size(60, 60);
            this.btnTramDisplayMode.TabIndex = 480;
            this.btnTramDisplayMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTramDisplayMode.UseVisualStyleBackColor = false;
            this.btnTramDisplayMode.Click += new System.EventHandler(this.btnTramDisplayMode_Click);
            // 
            // btnFlag
            // 
            this.btnFlag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFlag.BackColor = System.Drawing.Color.Transparent;
            this.btnFlag.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnFlag.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFlag.FlatAppearance.BorderSize = 0;
            this.btnFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlag.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlag.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.btnFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFlag.Location = new System.Drawing.Point(250, 0);
            this.btnFlag.Margin = new System.Windows.Forms.Padding(0);
            this.btnFlag.Name = "btnFlag";
            this.btnFlag.Size = new System.Drawing.Size(60, 60);
            this.btnFlag.TabIndex = 121;
            this.btnFlag.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFlag.UseVisualStyleBackColor = false;
            this.btnFlag.Click += new System.EventHandler(this.btnFlag_Click);
            // 
            // btnChangeMappingColor
            // 
            this.btnChangeMappingColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChangeMappingColor.BackColor = System.Drawing.Color.SkyBlue;
            this.btnChangeMappingColor.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionMapping;
            this.btnChangeMappingColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChangeMappingColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChangeMappingColor.FlatAppearance.BorderSize = 0;
            this.btnChangeMappingColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeMappingColor.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeMappingColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeMappingColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeMappingColor.Location = new System.Drawing.Point(333, 4);
            this.btnChangeMappingColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeMappingColor.Name = "btnChangeMappingColor";
            this.btnChangeMappingColor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnChangeMappingColor.Size = new System.Drawing.Size(54, 52);
            this.btnChangeMappingColor.TabIndex = 476;
            this.btnChangeMappingColor.UseVisualStyleBackColor = false;
            this.btnChangeMappingColor.Click += new System.EventHandler(this.btnChangeMappingColor_Click);
            // 
            // btnABDraw
            // 
            this.btnABDraw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnABDraw.BackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnABDraw.FlatAppearance.BorderSize = 0;
            this.btnABDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnABDraw.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnABDraw.Image = global::AgOpenGPS.Properties.Resources.PointStart;
            this.btnABDraw.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnABDraw.Location = new System.Drawing.Point(410, 0);
            this.btnABDraw.Margin = new System.Windows.Forms.Padding(0);
            this.btnABDraw.Name = "btnABDraw";
            this.btnABDraw.Size = new System.Drawing.Size(60, 60);
            this.btnABDraw.TabIndex = 250;
            this.btnABDraw.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnABDraw.UseVisualStyleBackColor = false;
            this.btnABDraw.Visible = false;
            this.btnABDraw.Click += new System.EventHandler(this.btnABDraw_Click);
            // 
            // btnSnapToPivot
            // 
            this.btnSnapToPivot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSnapToPivot.BackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSnapToPivot.FlatAppearance.BorderSize = 0;
            this.btnSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapToPivot.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSnapToPivot.ForeColor = System.Drawing.Color.DarkGray;
            this.btnSnapToPivot.Image = global::AgOpenGPS.Properties.Resources.SnapToPivot;
            this.btnSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSnapToPivot.Location = new System.Drawing.Point(490, 0);
            this.btnSnapToPivot.Margin = new System.Windows.Forms.Padding(0);
            this.btnSnapToPivot.Name = "btnSnapToPivot";
            this.btnSnapToPivot.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSnapToPivot.Size = new System.Drawing.Size(60, 60);
            this.btnSnapToPivot.TabIndex = 477;
            this.btnSnapToPivot.UseVisualStyleBackColor = false;
            this.btnSnapToPivot.Visible = false;
            this.btnSnapToPivot.Click += new System.EventHandler(this.btnSnapToPivot_Click);
            // 
            // btnEditAB
            // 
            this.btnEditAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditAB.BackColor = System.Drawing.Color.Transparent;
            this.btnEditAB.FlatAppearance.BorderSize = 0;
            this.btnEditAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditAB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditAB.Image = global::AgOpenGPS.Properties.Resources.ABLineEdit;
            this.btnEditAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditAB.Location = new System.Drawing.Point(570, 0);
            this.btnEditAB.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditAB.Name = "btnEditAB";
            this.btnEditAB.Size = new System.Drawing.Size(60, 60);
            this.btnEditAB.TabIndex = 489;
            this.btnEditAB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditAB.UseVisualStyleBackColor = false;
            this.btnEditAB.Visible = false;
            this.btnEditAB.Click += new System.EventHandler(this.btnEditAB_Click);
            // 
            // lblSpeed
            // 
            this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSpeed.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Black;
            this.lblSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSpeed.Location = new System.Drawing.Point(0, 5);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(120, 50);
            this.lblSpeed.TabIndex = 116;
            this.lblSpeed.Text = "888.8";
            this.lblSpeed.UseVisualStyleBackColor = false;
            this.lblSpeed.Click += new System.EventHandler(this.lblSpeed_Click);
            // 
            // lblTopData
            // 
            this.lblTopData.AutoSize = true;
            this.lblTopData.BackColor = System.Drawing.Color.Transparent;
            this.lblTopData.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTopData.Location = new System.Drawing.Point(101, 0);
            this.lblTopData.Name = "lblTopData";
            this.lblTopData.Size = new System.Drawing.Size(149, 16);
            this.lblTopData.TabIndex = 483;
            this.lblTopData.Text = "Vehicle Name + Width";
            // 
            // lblInty
            // 
            this.lblInty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInty.BackColor = System.Drawing.Color.Transparent;
            this.lblInty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInty.Location = new System.Drawing.Point(0, 570);
            this.lblInty.Name = "lblInty";
            this.lblInty.Size = new System.Drawing.Size(60, 20);
            this.lblInty.TabIndex = 485;
            this.lblInty.Text = "0";
            this.lblInty.Click += new System.EventHandler(this.lblInty_Click);
            // 
            // lblCurveLineName
            // 
            this.lblCurveLineName.AutoSize = true;
            this.lblCurveLineName.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveLineName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveLineName.Location = new System.Drawing.Point(387, 0);
            this.lblCurveLineName.Name = "lblCurveLineName";
            this.lblCurveLineName.Size = new System.Drawing.Size(33, 16);
            this.lblCurveLineName.TabIndex = 486;
            this.lblCurveLineName.Text = "Line";
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.AutoSize = true;
            this.lblCurrentField.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentField.Location = new System.Drawing.Point(101, 17);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(70, 16);
            this.lblCurrentField.TabIndex = 488;
            this.lblCurrentField.Text = "Fieldname";
            // 
            // lblFix
            // 
            this.lblFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFix.BackColor = System.Drawing.Color.Transparent;
            this.lblFix.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFix.Location = new System.Drawing.Point(560, 40);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(80, 20);
            this.lblFix.TabIndex = 489;
            this.lblFix.Text = "GPS single: ";
            this.lblFix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbludpWatchCounts
            // 
            this.lbludpWatchCounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbludpWatchCounts.BackColor = System.Drawing.Color.Transparent;
            this.lbludpWatchCounts.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbludpWatchCounts.Location = new System.Drawing.Point(0, 390);
            this.lbludpWatchCounts.Name = "lbludpWatchCounts";
            this.lbludpWatchCounts.Size = new System.Drawing.Size(60, 20);
            this.lbludpWatchCounts.TabIndex = 492;
            this.lbludpWatchCounts.Text = "0";
            this.lbludpWatchCounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbludpWatchCounts.Click += new System.EventHandler(this.lbludpWatchCounts_Click);
            // 
            // lblAge
            // 
            this.lblAge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAge.BackColor = System.Drawing.Color.Transparent;
            this.lblAge.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.Location = new System.Drawing.Point(600, 0);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(40, 20);
            this.lblAge.TabIndex = 493;
            this.lblAge.Text = "age";
            this.lblAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(560, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 494;
            this.label1.Text = "Age:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStanleyPure
            // 
            this.btnStanleyPure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStanleyPure.BackColor = System.Drawing.Color.Transparent;
            this.btnStanleyPure.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnStanleyPure.FlatAppearance.BorderSize = 0;
            this.btnStanleyPure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStanleyPure.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStanleyPure.ForeColor = System.Drawing.Color.Black;
            this.btnStanleyPure.Image = global::AgOpenGPS.Properties.Resources.ModeStanley;
            this.btnStanleyPure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStanleyPure.Location = new System.Drawing.Point(0, 600);
            this.btnStanleyPure.Margin = new System.Windows.Forms.Padding(0);
            this.btnStanleyPure.Name = "btnStanleyPure";
            this.btnStanleyPure.Size = new System.Drawing.Size(60, 60);
            this.btnStanleyPure.TabIndex = 490;
            this.btnStanleyPure.UseVisualStyleBackColor = false;
            this.btnStanleyPure.Click += new System.EventHandler(this.btnStanleyPure_Click);
            // 
            // btnAutoSteerConfig
            // 
            this.btnAutoSteerConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAutoSteerConfig.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.BackgroundImage = global::AgOpenGPS.Properties.Resources.AutoSteerConf;
            this.btnAutoSteerConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAutoSteerConfig.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown;
            this.btnAutoSteerConfig.FlatAppearance.BorderSize = 0;
            this.btnAutoSteerConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteerConfig.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoSteerConfig.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSteerConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteerConfig.Location = new System.Drawing.Point(0, 500);
            this.btnAutoSteerConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteerConfig.Name = "btnAutoSteerConfig";
            this.btnAutoSteerConfig.Size = new System.Drawing.Size(60, 60);
            this.btnAutoSteerConfig.TabIndex = 475;
            this.btnAutoSteerConfig.Text = "-38.8.";
            this.btnAutoSteerConfig.UseVisualStyleBackColor = false;
            this.btnAutoSteerConfig.Click += new System.EventHandler(this.btnAutoSteerConfig_Click);
            // 
            // btnMaximizeMainForm
            // 
            this.btnMaximizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMaximizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMaximizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximizeMainForm.Image = global::AgOpenGPS.Properties.Resources.WindowMaximize;
            this.btnMaximizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMaximizeMainForm.Location = new System.Drawing.Point(240, 0);
            this.btnMaximizeMainForm.Name = "btnMaximizeMainForm";
            this.btnMaximizeMainForm.Size = new System.Drawing.Size(60, 60);
            this.btnMaximizeMainForm.TabIndex = 482;
            this.btnMaximizeMainForm.UseVisualStyleBackColor = false;
            this.btnMaximizeMainForm.Click += new System.EventHandler(this.btnMaximizeMainForm_Click);
            // 
            // btnMinimizeMainForm
            // 
            this.btnMinimizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMinimizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizeMainForm.Image = global::AgOpenGPS.Properties.Resources.WindowMinimize;
            this.btnMinimizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMinimizeMainForm.Location = new System.Drawing.Point(180, 0);
            this.btnMinimizeMainForm.Name = "btnMinimizeMainForm";
            this.btnMinimizeMainForm.Size = new System.Drawing.Size(60, 60);
            this.btnMinimizeMainForm.TabIndex = 481;
            this.btnMinimizeMainForm.UseVisualStyleBackColor = false;
            this.btnMinimizeMainForm.Click += new System.EventHandler(this.btnMinimizeMainForm_Click);
            // 
            // pictureboxStart
            // 
            this.pictureboxStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureboxStart.BackgroundImage = global::AgOpenGPS.Properties.Resources.first;
            this.pictureboxStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureboxStart.Location = new System.Drawing.Point(768, 285);
            this.pictureboxStart.Name = "pictureboxStart";
            this.pictureboxStart.Size = new System.Drawing.Size(74, 60);
            this.pictureboxStart.TabIndex = 473;
            this.pictureboxStart.TabStop = false;
            // 
            // btnStartAgIO
            // 
            this.btnStartAgIO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartAgIO.BackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnStartAgIO.FlatAppearance.BorderSize = 0;
            this.btnStartAgIO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAgIO.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartAgIO.ForeColor = System.Drawing.Color.Black;
            this.btnStartAgIO.Image = global::AgOpenGPS.Properties.Resources.AgIO;
            this.btnStartAgIO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartAgIO.Location = new System.Drawing.Point(0, 420);
            this.btnStartAgIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartAgIO.Name = "btnStartAgIO";
            this.btnStartAgIO.Size = new System.Drawing.Size(60, 60);
            this.btnStartAgIO.TabIndex = 467;
            this.btnStartAgIO.UseVisualStyleBackColor = false;
            this.btnStartAgIO.Click += new System.EventHandler(this.btnStartAgIO_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.FlatAppearance.BorderSize = 0;
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShutdown.Image = global::AgOpenGPS.Properties.Resources.WindowClose;
            this.btnShutdown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShutdown.Location = new System.Drawing.Point(300, 0);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(60, 60);
            this.btnShutdown.TabIndex = 447;
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.Transparent;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Image = global::AgOpenGPS.Properties.Resources.Help;
            this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHelp.Location = new System.Drawing.Point(120, 0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(60, 60);
            this.btnHelp.TabIndex = 495;
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelNavigation);
            this.panelMain.Controls.Add(this.panelSim);
            this.panelMain.Controls.Add(this.oglMain);
            this.panelMain.Controls.Add(this.panelBottom);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Location = new System.Drawing.Point(0, 60);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1000, 660);
            this.panelMain.TabIndex = 496;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelAB);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(60, 600);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(880, 60);
            this.panelBottom.TabIndex = 325;
            this.panelBottom.Visible = false;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.toolStripDropDownButton4);
            this.panelLeft.Controls.Add(this.toolStripBtnField);
            this.panelLeft.Controls.Add(this.stripBtnConfig);
            this.panelLeft.Controls.Add(this.simplifyToolStrip);
            this.panelLeft.Controls.Add(this.distanceToolBtn);
            this.panelLeft.Controls.Add(this.btnStartAgIO);
            this.panelLeft.Controls.Add(this.btnAutoSteerConfig);
            this.panelLeft.Controls.Add(this.lblInty);
            this.panelLeft.Controls.Add(this.btnStanleyPure);
            this.panelLeft.Controls.Add(this.lbludpWatchCounts);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(60, 660);
            this.panelLeft.TabIndex = 326;
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDropDownButton4.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.toolStripDropDownButton4.FlatAppearance.BorderSize = 0;
            this.toolStripDropDownButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolStripDropDownButton4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton4.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripDropDownButton4.Image = global::AgOpenGPS.Properties.Resources.SpecialFunctions;
            this.toolStripDropDownButton4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.toolStripDropDownButton4.Location = new System.Drawing.Point(0, 220);
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(60, 60);
            this.toolStripDropDownButton4.TabIndex = 497;
            this.toolStripDropDownButton4.UseVisualStyleBackColor = false;
            this.toolStripDropDownButton4.Click += new System.EventHandler(this.toolStripDropDownButton4_Click);
            // 
            // toolStripBtnField
            // 
            this.toolStripBtnField.BackColor = System.Drawing.Color.Transparent;
            this.toolStripBtnField.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.toolStripBtnField.FlatAppearance.BorderSize = 0;
            this.toolStripBtnField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolStripBtnField.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripBtnField.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripBtnField.Image = global::AgOpenGPS.Properties.Resources.JobActive;
            this.toolStripBtnField.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.toolStripBtnField.Location = new System.Drawing.Point(0, 300);
            this.toolStripBtnField.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnField.Name = "toolStripBtnField";
            this.toolStripBtnField.Size = new System.Drawing.Size(60, 60);
            this.toolStripBtnField.TabIndex = 496;
            this.toolStripBtnField.UseVisualStyleBackColor = false;
            this.toolStripBtnField.Click += new System.EventHandler(this.toolStripBtnFieldOpen_Click);
            // 
            // stripBtnConfig
            // 
            this.stripBtnConfig.BackColor = System.Drawing.Color.Transparent;
            this.stripBtnConfig.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.stripBtnConfig.FlatAppearance.BorderSize = 0;
            this.stripBtnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stripBtnConfig.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stripBtnConfig.Image = global::AgOpenGPS.Properties.Resources.Settings48;
            this.stripBtnConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stripBtnConfig.Location = new System.Drawing.Point(0, 140);
            this.stripBtnConfig.Name = "stripBtnConfig";
            this.stripBtnConfig.Size = new System.Drawing.Size(60, 60);
            this.stripBtnConfig.TabIndex = 495;
            this.stripBtnConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.stripBtnConfig.UseVisualStyleBackColor = false;
            this.stripBtnConfig.Click += new System.EventHandler(this.stripBtnConfig_Click);
            // 
            // simplifyToolStrip
            // 
            this.simplifyToolStrip.BackColor = System.Drawing.Color.Transparent;
            this.simplifyToolStrip.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.simplifyToolStrip.FlatAppearance.BorderSize = 0;
            this.simplifyToolStrip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.simplifyToolStrip.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simplifyToolStrip.Image = global::AgOpenGPS.Properties.Resources.NavigationSettings;
            this.simplifyToolStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.simplifyToolStrip.Location = new System.Drawing.Point(0, 60);
            this.simplifyToolStrip.Name = "simplifyToolStrip";
            this.simplifyToolStrip.Size = new System.Drawing.Size(60, 60);
            this.simplifyToolStrip.TabIndex = 494;
            this.simplifyToolStrip.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.simplifyToolStrip.UseVisualStyleBackColor = false;
            this.simplifyToolStrip.Click += new System.EventHandler(this.navPanelToolStrip_Click);
            // 
            // distanceToolBtn
            // 
            this.distanceToolBtn.BackColor = System.Drawing.Color.Transparent;
            this.distanceToolBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.distanceToolBtn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.distanceToolBtn.Location = new System.Drawing.Point(0, 0);
            this.distanceToolBtn.Name = "distanceToolBtn";
            this.distanceToolBtn.Size = new System.Drawing.Size(60, 40);
            this.distanceToolBtn.TabIndex = 493;
            this.distanceToolBtn.Text = "25 Ha";
            this.distanceToolBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.distanceToolBtn.Click += new System.EventHandler(this.toolStripDropDownButtonDistance_Click);
            // 
            // panelCaptionBar
            // 
            this.panelCaptionBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCaptionBar.Controls.Add(this.lblSpeed);
            this.panelCaptionBar.Controls.Add(this.btnHelp);
            this.panelCaptionBar.Controls.Add(this.btnMinimizeMainForm);
            this.panelCaptionBar.Controls.Add(this.btnMaximizeMainForm);
            this.panelCaptionBar.Controls.Add(this.btnShutdown);
            this.panelCaptionBar.Location = new System.Drawing.Point(640, 0);
            this.panelCaptionBar.Name = "panelCaptionBar";
            this.panelCaptionBar.Size = new System.Drawing.Size(360, 60);
            this.panelCaptionBar.TabIndex = 497;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.contextMenuStrip2.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.boundariesToolStripMenuItem,
            this.headlandToolStripMenuItem,
            this.tramLinesMenuField,
            this.toolStripBtnMakeBndContour,
            this.recordedPathStripMenu});
            this.contextMenuStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStrip2.Name = "contextMenuStripFlag";
            this.contextMenuStrip2.Size = new System.Drawing.Size(387, 424);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Image = global::AgOpenGPS.Properties.Resources.JobClose;
            this.toolStripMenuItem9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMenuItem9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(386, 70);
            this.toolStripMenuItem9.Text = "Field";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripBtnField_Click);
            // 
            // boundariesToolStripMenuItem
            // 
            this.boundariesToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.MakeBoundary;
            this.boundariesToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.boundariesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.boundariesToolStripMenuItem.Name = "boundariesToolStripMenuItem";
            this.boundariesToolStripMenuItem.Size = new System.Drawing.Size(386, 70);
            this.boundariesToolStripMenuItem.Text = "Boundary";
            this.boundariesToolStripMenuItem.Click += new System.EventHandler(this.boundariesToolStripMenuItem_Click);
            // 
            // headlandToolStripMenuItem
            // 
            this.headlandToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.HeadlandMenu;
            this.headlandToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.headlandToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.headlandToolStripMenuItem.Name = "headlandToolStripMenuItem";
            this.headlandToolStripMenuItem.Size = new System.Drawing.Size(386, 70);
            this.headlandToolStripMenuItem.Text = "Headland";
            this.headlandToolStripMenuItem.Click += new System.EventHandler(this.headlandToolStripMenuItem_Click);
            // 
            // tramLinesMenuField
            // 
            this.tramLinesMenuField.Image = global::AgOpenGPS.Properties.Resources.ABTramLine;
            this.tramLinesMenuField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tramLinesMenuField.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tramLinesMenuField.Name = "tramLinesMenuField";
            this.tramLinesMenuField.Size = new System.Drawing.Size(386, 70);
            this.tramLinesMenuField.Text = "TramLines";
            this.tramLinesMenuField.Click += new System.EventHandler(this.tramLinesMenuField_Click);
            // 
            // toolStripBtnMakeBndContour
            // 
            this.toolStripBtnMakeBndContour.Image = global::AgOpenGPS.Properties.Resources.MakeBoundary;
            this.toolStripBtnMakeBndContour.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnMakeBndContour.Name = "toolStripBtnMakeBndContour";
            this.toolStripBtnMakeBndContour.Size = new System.Drawing.Size(386, 70);
            this.toolStripBtnMakeBndContour.Text = "Boundary Contour";
            this.toolStripBtnMakeBndContour.Click += new System.EventHandler(this.toolStripBtnMakeBndContour_Click);
            // 
            // recordedPathStripMenu
            // 
            this.recordedPathStripMenu.Image = global::AgOpenGPS.Properties.Resources.RecPath;
            this.recordedPathStripMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.recordedPathStripMenu.Name = "recordedPathStripMenu";
            this.recordedPathStripMenu.Size = new System.Drawing.Size(386, 70);
            this.recordedPathStripMenu.Text = "Record Path";
            this.recordedPathStripMenu.Click += new System.EventHandler(this.recordedPathStripMenu_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSteerSettingsToolStripMenuItem,
            this.deleteContourPathsToolStripMenuItem,
            this.deleteAppliedAreaToolStripMenuItem,
            this.steerChartStripMenu,
            this.webcamToolStrip,
            this.offsetFixToolStrip,
            this.angleChartToolStripMenuItem,
            this.correctionToolStrip});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(405, 564);
            // 
            // toolSteerSettingsToolStripMenuItem
            // 
            this.toolSteerSettingsToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_ModulesSteer;
            this.toolSteerSettingsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSteerSettingsToolStripMenuItem.Name = "toolSteerSettingsToolStripMenuItem";
            this.toolSteerSettingsToolStripMenuItem.ShowShortcutKeys = false;
            this.toolSteerSettingsToolStripMenuItem.Size = new System.Drawing.Size(404, 70);
            this.toolSteerSettingsToolStripMenuItem.Text = "Tool Steer Settings";
            this.toolSteerSettingsToolStripMenuItem.Click += new System.EventHandler(this.toolSteerSettingsToolStripMenuItem_Click);
            // 
            // FormGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 720);
            this.Controls.Add(this.panelCaptionBar);
            this.Controls.Add(this.pictureboxStart);
            this.Controls.Add(this.lblFix);
            this.Controls.Add(this.panelDrag);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCurrentField);
            this.Controls.Add(this.lblCurveLineName);
            this.Controls.Add(this.lblTopData);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.oglZoom);
            this.Controls.Add(this.lblFieldStatus);
            this.Controls.Add(this.oglBack);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 720);
            this.Name = "FormGPS";
            this.Text = "AgOpenGPS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPS_FormClosing);
            this.Load += new System.EventHandler(this.FormGPS_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripFlag.ResumeLayout(false);
            this.panelDrag.ResumeLayout(false);
            this.panelSim.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.panelAB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxStart)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelCaptionBar.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer tmrWatchdog;
        private System.Windows.Forms.Button lblSpeed;
        private ProXoft.WinForms.RepeatButton btnZoomOut;
        private ProXoft.WinForms.RepeatButton btnZoomIn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFlag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFlagRed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagGrn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagYel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.Button btnFlag;
        private System.Windows.Forms.Button btnResetSteerAngle;
        private System.Windows.Forms.Button btnResetSim;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button btnSectionOffAutoOn;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageEnglish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDeutsch;
        private System.Windows.Forms.ToolStripMenuItem setWorkingDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageRussian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDutch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSpanish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageFrench;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageItalian;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnCurve;
        private System.Windows.Forms.ToolStripMenuItem enterSimCoordsToolStripMenuItem;
        private System.Windows.Forms.Button btnABLine;
        private System.Windows.Forms.HScrollBar hsbarStepDistance;
        private System.Windows.Forms.HScrollBar hsbarSteerAngle;
        private OpenTK.GLControl oglZoom;
        private OpenTK.GLControl oglMain;
        private OpenTK.GLControl oglBack;
        private System.Windows.Forms.ComboBox cboxpRowWidth;
        private System.Windows.Forms.Label lblHz;
        public System.Windows.Forms.Timer timerSim;
        private System.Windows.Forms.Button btnManualOffOn;
        public System.Windows.Forms.ToolStripMenuItem menustripLanguage;
        private System.Windows.Forms.TableLayoutPanel panelSim;
        public System.Windows.Forms.TableLayoutPanel panelDrag;
        private ProXoft.WinForms.RepeatButton btnpTiltDown;
        private ProXoft.WinForms.RepeatButton btnpTiltUp;
        private System.Windows.Forms.Button btnHeadlandOnOff;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Button btnSimSetSpeedToZero;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageUkranian;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSlovak;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagForm;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageTest;
        private System.Windows.Forms.ToolStripMenuItem menuLanguagePolish;
        public System.Windows.Forms.ToolStripMenuItem steerChartStripMenu;
        private System.Windows.Forms.ToolStripMenuItem webcamToolStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteAppliedAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteForSureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offsetFixToolStrip;
        private System.Windows.Forms.TableLayoutPanel panelNavigation;
        private System.Windows.Forms.Label lblFieldStatus;
        private System.Windows.Forms.ToolStripMenuItem deleteContourPathsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureboxStart;
        private System.Windows.Forms.TableLayoutPanel panelAB;
        private System.Windows.Forms.ToolStripMenuItem colorsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem topFieldViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem simulatorOnToolStripMenuItem;
        private System.Windows.Forms.Button btnMinimizeMainForm;
        private System.Windows.Forms.Button btnMaximizeMainForm;
        private System.Windows.Forms.Label lblTopData;
        private System.Windows.Forms.ToolStripMenuItem resetALLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetEverythingToolStripMenuItem;
        private System.Windows.Forms.Label lblInty;
        private System.Windows.Forms.Label lblCurveLineName;
        private System.Windows.Forms.Label lblCurrentField;
        private System.Windows.Forms.Label lblFix;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageDanish;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.Label lbludpWatchCounts;
        private System.Windows.Forms.ToolStripMenuItem angleChartToolStripMenuItem;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem sectionColorToolStripMenuItem;
        private System.Windows.Forms.Button btnEditAB;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ToolStripMenuItem correctionToolStrip;
        private System.Windows.Forms.Button btnCycleLines;
        private System.Windows.Forms.Button btnContour;
        private System.Windows.Forms.Button btnAutoYouTurn;
        private System.Windows.Forms.Button btnAutoSteer;
        private System.Windows.Forms.Button btnStartAgIO;
        private System.Windows.Forms.Button btnStanleyPure;
        private System.Windows.Forms.Button btnABDraw;
        private System.Windows.Forms.Button btnHydLift;
        private System.Windows.Forms.Button btnChangeMappingColor;
        private System.Windows.Forms.Button btnSnapToPivot;
        private System.Windows.Forms.Button btnTramDisplayMode;
        private System.Windows.Forms.Button btnYouSkipEnable;
        private System.Windows.Forms.Button btnDayNightMode;
        private System.Windows.Forms.Button btnN3D;
        private System.Windows.Forms.Button btn2D;
        private System.Windows.Forms.Button btn3D;
        private System.Windows.Forms.Button btnN2D;
        public System.Windows.Forms.Button btnPickPath;
        public System.Windows.Forms.Button btnPathRecordStop;
        public System.Windows.Forms.Button btnPathGoStop;
        public System.Windows.Forms.Button btnResumePath;
        private System.Windows.Forms.Button btnAutoSteerConfig;
        public System.Windows.Forms.TableLayoutPanel panelRight;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageTurkish;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button simplifyToolStrip;
        private System.Windows.Forms.Label distanceToolBtn;
        private System.Windows.Forms.Button stripBtnConfig;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem boundariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headlandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tramLinesMenuField;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripBtnMakeBndContour;
        private System.Windows.Forms.ToolStripMenuItem recordedPathStripMenu;
        private System.Windows.Forms.Button toolStripBtnField;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button toolStripDropDownButton4;
        private System.Windows.Forms.Panel panelCaptionBar;
        private System.Windows.Forms.ToolStripMenuItem toolSteerSettingsToolStripMenuItem;
    }
}