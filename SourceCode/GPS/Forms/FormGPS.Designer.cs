﻿namespace AgOpenGPS
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.btnResetSim = new System.Windows.Forms.Button();
            this.btnResetSteerAngle = new System.Windows.Forms.Button();
            this.timerSim = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hsbarSteerAngle = new System.Windows.Forms.HScrollBar();
            this.hsbarStepDistance = new System.Windows.Forms.HScrollBar();
            this.oglMain = new OpenTK.GLControl();
            this.panelSim = new System.Windows.Forms.Panel();
            this.btnSimSetSpeedToZero = new System.Windows.Forms.Button();
            this.oglBack = new OpenTK.GLControl();
            this.lblHz = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.TableLayoutPanel();
            this.btnRecPath = new System.Windows.Forms.Button();
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
            this.steerChartStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.webcamToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetFixToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.angleChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correctionToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.panelNavigation = new System.Windows.Forms.TableLayoutPanel();
            this.btnN3D = new System.Windows.Forms.Button();
            this.btnBrightnessDn = new System.Windows.Forms.Button();
            this.btnBrightnessUp = new System.Windows.Forms.Button();
            this.btn2D = new System.Windows.Forms.Button();
            this.btnDayNightMode = new System.Windows.Forms.Button();
            this.btnZoomIn = new ProXoft.WinForms.RepeatButton();
            this.btnZoomOut = new ProXoft.WinForms.RepeatButton();
            this.btnpTiltDown = new ProXoft.WinForms.RepeatButton();
            this.btnpTiltUp = new ProXoft.WinForms.RepeatButton();
            this.btn3D = new System.Windows.Forms.Button();
            this.btnN2D = new System.Windows.Forms.Button();
            this.lblFieldStatus = new System.Windows.Forms.Label();
            this.panelAB = new System.Windows.Forms.Panel();
            this.btnResetToolHeading = new System.Windows.Forms.Button();
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
            this.btnStanleyPure = new System.Windows.Forms.Button();
            this.btnAutoSteerConfig = new System.Windows.Forms.Button();
            this.btnMaximizeMainForm = new System.Windows.Forms.Button();
            this.btnMinimizeMainForm = new System.Windows.Forms.Button();
            this.pictureboxStart = new System.Windows.Forms.PictureBox();
            this.btnStartAgIO = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.panelMain = new AgOpenGPS.Panel1();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.toolStripDropDownButton4 = new System.Windows.Forms.Button();
            this.toolStripBtnField = new System.Windows.Forms.Button();
            this.stripBtnConfig = new System.Windows.Forms.Button();
            this.simplifyToolStrip = new System.Windows.Forms.Button();
            this.distanceToolBtn = new System.Windows.Forms.Label();
            this.paneltop = new AgOpenGPS.Panel1();
            this.btnMenu = new System.Windows.Forms.Button();
            this.panelTopRight = new System.Windows.Forms.Panel();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.boundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tramLinesMenuField = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBtnMakeBndContour = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolSteerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem.SuspendLayout();
            this.contextMenuStripFlag.SuspendLayout();
            this.panelSim.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.panelAB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxStart)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.paneltop.SuspendLayout();
            this.panelTopRight.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.fileToolStripMenuItem.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.fileToolStripMenuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(392, 500);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(388, 6);
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
            this.menustripLanguage.Size = new System.Drawing.Size(391, 46);
            this.menustripLanguage.Text = "Language";
            // 
            // menuLanguageDanish
            // 
            this.menuLanguageDanish.Name = "menuLanguageDanish";
            this.menuLanguageDanish.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageDanish.Text = "Dansk (Denmark)";
            this.menuLanguageDanish.Click += new System.EventHandler(this.menuLanguageDanish_Click);
            // 
            // menuLanguageDeutsch
            // 
            this.menuLanguageDeutsch.CheckOnClick = true;
            this.menuLanguageDeutsch.Name = "menuLanguageDeutsch";
            this.menuLanguageDeutsch.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageDeutsch.Text = "Deutsch (Germany)";
            this.menuLanguageDeutsch.Click += new System.EventHandler(this.menuLanguageDeutsch_Click);
            // 
            // menuLanguageEnglish
            // 
            this.menuLanguageEnglish.CheckOnClick = true;
            this.menuLanguageEnglish.Name = "menuLanguageEnglish";
            this.menuLanguageEnglish.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageEnglish.Text = "English (Canada)";
            this.menuLanguageEnglish.Click += new System.EventHandler(this.menuLanguageEnglish_Click);
            // 
            // menuLanguageSpanish
            // 
            this.menuLanguageSpanish.CheckOnClick = true;
            this.menuLanguageSpanish.Name = "menuLanguageSpanish";
            this.menuLanguageSpanish.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageSpanish.Text = "Español (Spanish)";
            this.menuLanguageSpanish.Click += new System.EventHandler(this.menuLanguageSpanish_Click);
            // 
            // menuLanguageFrench
            // 
            this.menuLanguageFrench.CheckOnClick = true;
            this.menuLanguageFrench.Name = "menuLanguageFrench";
            this.menuLanguageFrench.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageFrench.Text = "Français (France)";
            this.menuLanguageFrench.Click += new System.EventHandler(this.menuLanguageFrench_Click);
            // 
            // menuLanguageItalian
            // 
            this.menuLanguageItalian.Name = "menuLanguageItalian";
            this.menuLanguageItalian.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageItalian.Text = "Italiano (Italy)";
            this.menuLanguageItalian.Click += new System.EventHandler(this.menuLanguageItalian_Click);
            // 
            // menuLanguageDutch
            // 
            this.menuLanguageDutch.CheckOnClick = true;
            this.menuLanguageDutch.Name = "menuLanguageDutch";
            this.menuLanguageDutch.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageDutch.Text = "Nederlands (Holland)";
            this.menuLanguageDutch.Click += new System.EventHandler(this.menuLanguageDutch_Click);
            // 
            // menuLanguagePolish
            // 
            this.menuLanguagePolish.Name = "menuLanguagePolish";
            this.menuLanguagePolish.Size = new System.Drawing.Size(478, 46);
            this.menuLanguagePolish.Text = "Polski (Poland)";
            this.menuLanguagePolish.Click += new System.EventHandler(this.menuLanguagesPolski_Click);
            // 
            // menuLanguageSlovak
            // 
            this.menuLanguageSlovak.Name = "menuLanguageSlovak";
            this.menuLanguageSlovak.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageSlovak.Text = "Slovenčina (Slovakia)";
            this.menuLanguageSlovak.Click += new System.EventHandler(this.menuLanguageSlovak_Click);
            // 
            // menuLanguageUkranian
            // 
            this.menuLanguageUkranian.Name = "menuLanguageUkranian";
            this.menuLanguageUkranian.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageUkranian.Text = "Yкраїнська (Ukraine)";
            this.menuLanguageUkranian.Click += new System.EventHandler(this.menuLanguageUkranian_Click);
            // 
            // menuLanguageRussian
            // 
            this.menuLanguageRussian.CheckOnClick = true;
            this.menuLanguageRussian.Name = "menuLanguageRussian";
            this.menuLanguageRussian.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageRussian.Text = "русский (Russia)";
            this.menuLanguageRussian.Click += new System.EventHandler(this.menuLanguageRussian_Click);
            // 
            // menuLanguageTest
            // 
            this.menuLanguageTest.Name = "menuLanguageTest";
            this.menuLanguageTest.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageTest.Text = "Test";
            this.menuLanguageTest.Click += new System.EventHandler(this.menuLanguageTest_Click);
            // 
            // menuLanguageTurkish
            // 
            this.menuLanguageTurkish.CheckOnClick = true;
            this.menuLanguageTurkish.Name = "menuLanguageTurkish";
            this.menuLanguageTurkish.Size = new System.Drawing.Size(478, 46);
            this.menuLanguageTurkish.Text = "Turkish (Türk)";
            this.menuLanguageTurkish.Click += new System.EventHandler(this.menuLanguageTurkish_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(388, 6);
            // 
            // setWorkingDirectoryToolStripMenuItem
            // 
            this.setWorkingDirectoryToolStripMenuItem.Name = "setWorkingDirectoryToolStripMenuItem";
            this.setWorkingDirectoryToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.setWorkingDirectoryToolStripMenuItem.Text = "Directories";
            this.setWorkingDirectoryToolStripMenuItem.Click += new System.EventHandler(this.setWorkingDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(388, 6);
            // 
            // colorsToolStripMenuItem1
            // 
            this.colorsToolStripMenuItem1.Name = "colorsToolStripMenuItem1";
            this.colorsToolStripMenuItem1.Size = new System.Drawing.Size(391, 46);
            this.colorsToolStripMenuItem1.Text = "Colors";
            this.colorsToolStripMenuItem1.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // sectionColorToolStripMenuItem
            // 
            this.sectionColorToolStripMenuItem.Name = "sectionColorToolStripMenuItem";
            this.sectionColorToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.sectionColorToolStripMenuItem.Text = "Section Colors";
            this.sectionColorToolStripMenuItem.Click += new System.EventHandler(this.colorsSectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(388, 6);
            // 
            // topFieldViewToolStripMenuItem
            // 
            this.topFieldViewToolStripMenuItem.Name = "topFieldViewToolStripMenuItem";
            this.topFieldViewToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.topFieldViewToolStripMenuItem.Text = "Top Field View";
            this.topFieldViewToolStripMenuItem.Click += new System.EventHandler(this.topFieldViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(388, 6);
            // 
            // enterSimCoordsToolStripMenuItem
            // 
            this.enterSimCoordsToolStripMenuItem.Name = "enterSimCoordsToolStripMenuItem";
            this.enterSimCoordsToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.enterSimCoordsToolStripMenuItem.Text = "Enter Sim Coords";
            this.enterSimCoordsToolStripMenuItem.Click += new System.EventHandler(this.enterSimCoordsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(388, 6);
            // 
            // simulatorOnToolStripMenuItem
            // 
            this.simulatorOnToolStripMenuItem.Name = "simulatorOnToolStripMenuItem";
            this.simulatorOnToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.simulatorOnToolStripMenuItem.Text = "Simulator On";
            this.simulatorOnToolStripMenuItem.Click += new System.EventHandler(this.simulatorOnToolStripMenuItem_Click);
            // 
            // resetALLToolStripMenuItem
            // 
            this.resetALLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetEverythingToolStripMenuItem});
            this.resetALLToolStripMenuItem.Name = "resetALLToolStripMenuItem";
            this.resetALLToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.resetALLToolStripMenuItem.Text = "Reset All";
            // 
            // resetEverythingToolStripMenuItem
            // 
            this.resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
            this.resetEverythingToolStripMenuItem.Size = new System.Drawing.Size(394, 46);
            this.resetEverythingToolStripMenuItem.Text = "Reset To Default";
            this.resetEverythingToolStripMenuItem.Click += new System.EventHandler(this.resetALLToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(391, 46);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(391, 46);
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
            this.contextMenuStripFlag.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.toolStripMenuFlagForm.Size = new System.Drawing.Size(259, 70);
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
            this.cboxpRowWidth.Location = new System.Drawing.Point(875, 11);
            this.cboxpRowWidth.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.cboxpRowWidth.Name = "cboxpRowWidth";
            this.cboxpRowWidth.Size = new System.Drawing.Size(74, 49);
            this.cboxpRowWidth.TabIndex = 247;
            this.cboxpRowWidth.Visible = false;
            this.cboxpRowWidth.SelectedIndexChanged += new System.EventHandler(this.cboxpRowWidth_SelectedIndexChanged);
            this.cboxpRowWidth.Click += new System.EventHandler(this.cboxpRowWidth_Click);
            // 
            // oglZoom
            // 
            this.oglZoom.BackColor = System.Drawing.Color.Black;
            this.oglZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oglZoom.Location = new System.Drawing.Point(375, 100);
            this.oglZoom.Margin = new System.Windows.Forms.Padding(0);
            this.oglZoom.Name = "oglZoom";
            this.oglZoom.Size = new System.Drawing.Size(500, 500);
            this.oglZoom.TabIndex = 182;
            this.oglZoom.VSync = false;
            this.oglZoom.Load += new System.EventHandler(this.oglZoom_Load);
            this.oglZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.oglZoom_Paint);
            this.oglZoom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.oglZoom_MouseClick);
            this.oglZoom.Resize += new System.EventHandler(this.oglZoom_Resize);
            // 
            // btnResetSim
            // 
            this.btnResetSim.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSim.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSim.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSim.Location = new System.Drawing.Point(625, 6);
            this.btnResetSim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetSim.Name = "btnResetSim";
            this.btnResetSim.Size = new System.Drawing.Size(62, 38);
            this.btnResetSim.TabIndex = 164;
            this.btnResetSim.Text = "Reset";
            this.btnResetSim.UseVisualStyleBackColor = false;
            this.btnResetSim.Click += new System.EventHandler(this.btnResetSim_Click);
            // 
            // btnResetSteerAngle
            // 
            this.btnResetSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSteerAngle.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSteerAngle.Location = new System.Drawing.Point(562, 6);
            this.btnResetSteerAngle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetSteerAngle.Name = "btnResetSteerAngle";
            this.btnResetSteerAngle.Size = new System.Drawing.Size(62, 38);
            this.btnResetSteerAngle.TabIndex = 162;
            this.btnResetSteerAngle.Text = ">0<";
            this.btnResetSteerAngle.UseVisualStyleBackColor = false;
            this.btnResetSteerAngle.Click += new System.EventHandler(this.btnResetSteerAngle_Click);
            // 
            // timerSim
            // 
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
            this.hsbarSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarSteerAngle.LargeChange = 20;
            this.hsbarSteerAngle.Location = new System.Drawing.Point(312, 0);
            this.hsbarSteerAngle.Maximum = 819;
            this.hsbarSteerAngle.Name = "hsbarSteerAngle";
            this.hsbarSteerAngle.Size = new System.Drawing.Size(250, 40);
            this.hsbarSteerAngle.TabIndex = 179;
            this.hsbarSteerAngle.Value = 400;
            this.hsbarSteerAngle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSteerAngle_Scroll);
            // 
            // hsbarStepDistance
            // 
            this.hsbarStepDistance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarStepDistance.LargeChange = 2;
            this.hsbarStepDistance.Location = new System.Drawing.Point(0, 0);
            this.hsbarStepDistance.Maximum = 101;
            this.hsbarStepDistance.Minimum = -25;
            this.hsbarStepDistance.Name = "hsbarStepDistance";
            this.hsbarStepDistance.Size = new System.Drawing.Size(250, 40);
            this.hsbarStepDistance.TabIndex = 178;
            this.hsbarStepDistance.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarStepDistance_Scroll);
            // 
            // oglMain
            // 
            this.oglMain.BackColor = System.Drawing.Color.Black;
            this.oglMain.CausesValidation = false;
            this.oglMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oglMain.Location = new System.Drawing.Point(75, 75);
            this.oglMain.Margin = new System.Windows.Forms.Padding(0);
            this.oglMain.Name = "oglMain";
            this.oglMain.Size = new System.Drawing.Size(1100, 750);
            this.oglMain.TabIndex = 180;
            this.oglMain.VSync = false;
            this.oglMain.Load += new System.EventHandler(this.oglMain_Load);
            this.oglMain.Paint += new System.Windows.Forms.PaintEventHandler(this.oglMain_Paint);
            this.oglMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglMain_MouseDown);
            this.oglMain.Resize += new System.EventHandler(this.oglMain_Resize);
            // 
            // panelSim
            // 
            this.panelSim.BackColor = System.Drawing.Color.White;
            this.panelSim.Controls.Add(this.hsbarStepDistance);
            this.panelSim.Controls.Add(this.btnSimSetSpeedToZero);
            this.panelSim.Controls.Add(this.hsbarSteerAngle);
            this.panelSim.Controls.Add(this.btnResetSteerAngle);
            this.panelSim.Controls.Add(this.btnResetSim);
            this.panelSim.Location = new System.Drawing.Point(328, 762);
            this.panelSim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSim.Name = "panelSim";
            this.panelSim.Size = new System.Drawing.Size(688, 50);
            this.panelSim.TabIndex = 325;
            // 
            // btnSimSetSpeedToZero
            // 
            this.btnSimSetSpeedToZero.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSetSpeedToZero.BackgroundImage = global::AgOpenGPS.Properties.Resources.AutoStop;
            this.btnSimSetSpeedToZero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSetSpeedToZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSetSpeedToZero.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimSetSpeedToZero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimSetSpeedToZero.Location = new System.Drawing.Point(250, 6);
            this.btnSimSetSpeedToZero.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSimSetSpeedToZero.Name = "btnSimSetSpeedToZero";
            this.btnSimSetSpeedToZero.Size = new System.Drawing.Size(62, 38);
            this.btnSimSetSpeedToZero.TabIndex = 453;
            this.btnSimSetSpeedToZero.UseVisualStyleBackColor = false;
            this.btnSimSetSpeedToZero.Click += new System.EventHandler(this.btnSimSetSpeedToZero_Click);
            // 
            // oglBack
            // 
            this.oglBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.oglBack.BackColor = System.Drawing.Color.Black;
            this.oglBack.ForeColor = System.Drawing.Color.Transparent;
            this.oglBack.Location = new System.Drawing.Point(330, 84);
            this.oglBack.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.oglBack.Name = "oglBack";
            this.oglBack.Size = new System.Drawing.Size(625, 375);
            this.oglBack.TabIndex = 181;
            this.oglBack.VSync = false;
            this.oglBack.Load += new System.EventHandler(this.oglBack_Load);
            this.oglBack.Paint += new System.Windows.Forms.PaintEventHandler(this.oglBack_Paint);
            // 
            // lblHz
            // 
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHz.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHz.Location = new System.Drawing.Point(0, 25);
            this.lblHz.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(100, 25);
            this.lblHz.TabIndex = 249;
            this.lblHz.Text = "10.0 ~ 5.5";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelRight
            // 
            this.panelRight.ColumnCount = 1;
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.Controls.Add(this.btnRecPath, 0, 3);
            this.panelRight.Controls.Add(this.btnCurve, 0, 1);
            this.panelRight.Controls.Add(this.btnContour, 0, 0);
            this.panelRight.Controls.Add(this.btnABLine, 0, 2);
            this.panelRight.Controls.Add(this.btnAutoYouTurn, 0, 10);
            this.panelRight.Controls.Add(this.btnSectionOffAutoOn, 0, 8);
            this.panelRight.Controls.Add(this.btnManualOffOn, 0, 7);
            this.panelRight.Controls.Add(this.btnCycleLines, 0, 5);
            this.panelRight.Controls.Add(this.btnAutoSteer, 0, 12);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1175, 75);
            this.panelRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelRight.Name = "panelRight";
            this.panelRight.RowCount = 13;
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090502F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.545252F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.545252F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.545253F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.549715F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090504F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.panelRight.Size = new System.Drawing.Size(75, 825);
            this.panelRight.TabIndex = 320;
            this.panelRight.Visible = false;
            // 
            // btnRecPath
            // 
            this.btnRecPath.BackColor = System.Drawing.Color.Transparent;
            this.btnRecPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRecPath.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnRecPath.FlatAppearance.BorderSize = 0;
            this.btnRecPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecPath.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecPath.Image = global::AgOpenGPS.Properties.Resources.RecPathOn;
            this.btnRecPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRecPath.Location = new System.Drawing.Point(0, 222);
            this.btnRecPath.Margin = new System.Windows.Forms.Padding(0);
            this.btnRecPath.Name = "btnRecPath";
            this.btnRecPath.Size = new System.Drawing.Size(75, 74);
            this.btnRecPath.TabIndex = 252;
            this.btnRecPath.UseVisualStyleBackColor = false;
            this.btnRecPath.Click += new System.EventHandler(this.btnRecPath_Click);
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
            this.btnCurve.Location = new System.Drawing.Point(0, 74);
            this.btnCurve.Margin = new System.Windows.Forms.Padding(0);
            this.btnCurve.Name = "btnCurve";
            this.btnCurve.Size = new System.Drawing.Size(75, 74);
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
            this.btnContour.Size = new System.Drawing.Size(75, 74);
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
            this.btnABLine.Location = new System.Drawing.Point(0, 148);
            this.btnABLine.Margin = new System.Windows.Forms.Padding(0);
            this.btnABLine.Name = "btnABLine";
            this.btnABLine.Size = new System.Drawing.Size(75, 74);
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
            this.btnAutoYouTurn.Location = new System.Drawing.Point(0, 629);
            this.btnAutoYouTurn.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoYouTurn.Name = "btnAutoYouTurn";
            this.btnAutoYouTurn.Size = new System.Drawing.Size(75, 74);
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
            this.btnSectionOffAutoOn.Location = new System.Drawing.Point(0, 518);
            this.btnSectionOffAutoOn.Margin = new System.Windows.Forms.Padding(0);
            this.btnSectionOffAutoOn.Name = "btnSectionOffAutoOn";
            this.btnSectionOffAutoOn.Size = new System.Drawing.Size(75, 74);
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
            this.btnManualOffOn.Location = new System.Drawing.Point(0, 444);
            this.btnManualOffOn.Margin = new System.Windows.Forms.Padding(0);
            this.btnManualOffOn.Name = "btnManualOffOn";
            this.btnManualOffOn.Size = new System.Drawing.Size(75, 74);
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
            this.btnCycleLines.Location = new System.Drawing.Point(0, 333);
            this.btnCycleLines.Margin = new System.Windows.Forms.Padding(0);
            this.btnCycleLines.Name = "btnCycleLines";
            this.btnCycleLines.Size = new System.Drawing.Size(75, 74);
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
            this.btnAutoSteer.Location = new System.Drawing.Point(0, 740);
            this.btnAutoSteer.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteer.Name = "btnAutoSteer";
            this.btnAutoSteer.Size = new System.Drawing.Size(75, 85);
            this.btnAutoSteer.TabIndex = 128;
            this.btnAutoSteer.UseVisualStyleBackColor = false;
            this.btnAutoSteer.Click += new System.EventHandler(this.btnAutoSteer_Click);
            // 
            // deleteContourPathsToolStripMenuItem
            // 
            this.deleteContourPathsToolStripMenuItem.Enabled = false;
            this.deleteContourPathsToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ContourDelete;
            this.deleteContourPathsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteContourPathsToolStripMenuItem.Name = "deleteContourPathsToolStripMenuItem";
            this.deleteContourPathsToolStripMenuItem.Size = new System.Drawing.Size(478, 70);
            this.deleteContourPathsToolStripMenuItem.Text = "Hide Contour Paths";
            this.deleteContourPathsToolStripMenuItem.Click += new System.EventHandler(this.deleteContourPathsToolStripMenuItem_Click);
            // 
            // deleteAppliedAreaToolStripMenuItem
            // 
            this.deleteAppliedAreaToolStripMenuItem.Enabled = false;
            this.deleteAppliedAreaToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.skull;
            this.deleteAppliedAreaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteAppliedAreaToolStripMenuItem.Name = "deleteAppliedAreaToolStripMenuItem";
            this.deleteAppliedAreaToolStripMenuItem.Size = new System.Drawing.Size(478, 70);
            this.deleteAppliedAreaToolStripMenuItem.Text = "Delete Applied Area";
            this.deleteAppliedAreaToolStripMenuItem.Click += new System.EventHandler(this.toolStripAreYouSure_Click);
            // 
            // steerChartStripMenu
            // 
            this.steerChartStripMenu.Image = global::AgOpenGPS.Properties.Resources.Chart;
            this.steerChartStripMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.steerChartStripMenu.Name = "steerChartStripMenu";
            this.steerChartStripMenu.Size = new System.Drawing.Size(478, 70);
            this.steerChartStripMenu.Text = "Steer Chart";
            this.steerChartStripMenu.Click += new System.EventHandler(this.toolStripAutoSteerChart_Click);
            // 
            // webcamToolStrip
            // 
            this.webcamToolStrip.Image = global::AgOpenGPS.Properties.Resources.Webcam;
            this.webcamToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.webcamToolStrip.Name = "webcamToolStrip";
            this.webcamToolStrip.Size = new System.Drawing.Size(478, 70);
            this.webcamToolStrip.Text = "WebCam";
            this.webcamToolStrip.Click += new System.EventHandler(this.webcamToolStrip_Click);
            // 
            // offsetFixToolStrip
            // 
            this.offsetFixToolStrip.Image = global::AgOpenGPS.Properties.Resources.YouTurnReverse;
            this.offsetFixToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.offsetFixToolStrip.Name = "offsetFixToolStrip";
            this.offsetFixToolStrip.Size = new System.Drawing.Size(478, 70);
            this.offsetFixToolStrip.Text = "Offset Fix";
            this.offsetFixToolStrip.Click += new System.EventHandler(this.offsetFixToolStrip_Click);
            // 
            // angleChartToolStripMenuItem
            // 
            this.angleChartToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_SourcesHeading;
            this.angleChartToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.angleChartToolStripMenuItem.Name = "angleChartToolStripMenuItem";
            this.angleChartToolStripMenuItem.ShowShortcutKeys = false;
            this.angleChartToolStripMenuItem.Size = new System.Drawing.Size(478, 70);
            this.angleChartToolStripMenuItem.Text = "Heading Chart";
            this.angleChartToolStripMenuItem.Click += new System.EventHandler(this.headingChartToolStripMenuItem_Click);
            // 
            // correctionToolStrip
            // 
            this.correctionToolStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.correctionToolStrip.Name = "correctionToolStrip";
            this.correctionToolStrip.ShowShortcutKeys = false;
            this.correctionToolStrip.Size = new System.Drawing.Size(478, 70);
            this.correctionToolStrip.Text = "Roll & Easting";
            this.correctionToolStrip.Click += new System.EventHandler(this.correctionToolStrip_Click);
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelNavigation.ColumnCount = 2;
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.panelNavigation.Controls.Add(this.btnN3D, 0, 3);
            this.panelNavigation.Controls.Add(this.btnBrightnessDn, 0, 5);
            this.panelNavigation.Controls.Add(this.btnBrightnessUp, 0, 5);
            this.panelNavigation.Controls.Add(this.btn2D, 0, 0);
            this.panelNavigation.Controls.Add(this.btnDayNightMode, 0, 4);
            this.panelNavigation.Controls.Add(this.btnZoomIn, 1, 3);
            this.panelNavigation.Controls.Add(this.btnZoomOut, 1, 2);
            this.panelNavigation.Controls.Add(this.btnpTiltDown, 1, 1);
            this.panelNavigation.Controls.Add(this.btnpTiltUp, 1, 0);
            this.panelNavigation.Controls.Add(this.btn3D, 0, 1);
            this.panelNavigation.Controls.Add(this.btnN2D, 0, 2);
            this.panelNavigation.Location = new System.Drawing.Point(118, 120);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.RowCount = 6;
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelNavigation.Size = new System.Drawing.Size(189, 500);
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
            this.btnN3D.Location = new System.Drawing.Point(7, 253);
            this.btnN3D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnN3D.Name = "btnN3D";
            this.btnN3D.Size = new System.Drawing.Size(75, 75);
            this.btnN3D.TabIndex = 472;
            this.btnN3D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnN3D.UseVisualStyleBackColor = false;
            this.btnN3D.Click += new System.EventHandler(this.btnN3D_Click);
            // 
            // btnBrightnessDn
            // 
            this.btnBrightnessDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrightnessDn.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessDn.FlatAppearance.BorderSize = 0;
            this.btnBrightnessDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessDn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrightnessDn.Image = global::AgOpenGPS.Properties.Resources.BrightnessDn;
            this.btnBrightnessDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessDn.Location = new System.Drawing.Point(102, 420);
            this.btnBrightnessDn.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnBrightnessDn.Name = "btnBrightnessDn";
            this.btnBrightnessDn.Size = new System.Drawing.Size(75, 75);
            this.btnBrightnessDn.TabIndex = 474;
            this.btnBrightnessDn.Text = "20%";
            this.btnBrightnessDn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBrightnessDn.UseVisualStyleBackColor = false;
            this.btnBrightnessDn.Click += new System.EventHandler(this.btnBrightnessDn_Click);
            // 
            // btnBrightnessUp
            // 
            this.btnBrightnessUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrightnessUp.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessUp.FlatAppearance.BorderSize = 0;
            this.btnBrightnessUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessUp.Image = global::AgOpenGPS.Properties.Resources.BrightnessUp;
            this.btnBrightnessUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessUp.Location = new System.Drawing.Point(7, 420);
            this.btnBrightnessUp.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnBrightnessUp.Name = "btnBrightnessUp";
            this.btnBrightnessUp.Size = new System.Drawing.Size(75, 75);
            this.btnBrightnessUp.TabIndex = 473;
            this.btnBrightnessUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBrightnessUp.UseVisualStyleBackColor = false;
            this.btnBrightnessUp.Click += new System.EventHandler(this.btnBrightnessUp_Click);
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
            this.btn2D.Location = new System.Drawing.Point(7, 4);
            this.btn2D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn2D.Name = "btn2D";
            this.btn2D.Size = new System.Drawing.Size(75, 75);
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
            this.btnDayNightMode.Location = new System.Drawing.Point(7, 336);
            this.btnDayNightMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDayNightMode.Name = "btnDayNightMode";
            this.btnDayNightMode.Size = new System.Drawing.Size(75, 75);
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
            this.btnZoomIn.Location = new System.Drawing.Point(102, 253);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(75, 75);
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
            this.btnZoomOut.Location = new System.Drawing.Point(102, 170);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(75, 75);
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
            this.btnpTiltDown.Location = new System.Drawing.Point(102, 87);
            this.btnpTiltDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnpTiltDown.Name = "btnpTiltDown";
            this.btnpTiltDown.Size = new System.Drawing.Size(75, 75);
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
            this.btnpTiltUp.Location = new System.Drawing.Point(102, 4);
            this.btnpTiltUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnpTiltUp.Name = "btnpTiltUp";
            this.btnpTiltUp.Size = new System.Drawing.Size(75, 75);
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
            this.btn3D.Location = new System.Drawing.Point(7, 87);
            this.btn3D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn3D.Name = "btn3D";
            this.btn3D.Size = new System.Drawing.Size(75, 75);
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
            this.btnN2D.Location = new System.Drawing.Point(7, 170);
            this.btnN2D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnN2D.Name = "btnN2D";
            this.btnN2D.Size = new System.Drawing.Size(75, 75);
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
            this.lblFieldStatus.Location = new System.Drawing.Point(126, 42);
            this.lblFieldStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFieldStatus.Name = "lblFieldStatus";
            this.lblFieldStatus.Size = new System.Drawing.Size(82, 29);
            this.lblFieldStatus.TabIndex = 469;
            this.lblFieldStatus.Text = "25 Ha";
            this.lblFieldStatus.Visible = false;
            // 
            // panelAB
            // 
            this.panelAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelAB.Controls.Add(this.btnResetToolHeading);
            this.panelAB.Controls.Add(this.btnYouSkipEnable);
            this.panelAB.Controls.Add(this.btnHeadlandOnOff);
            this.panelAB.Controls.Add(this.btnHydLift);
            this.panelAB.Controls.Add(this.btnTramDisplayMode);
            this.panelAB.Controls.Add(this.btnFlag);
            this.panelAB.Controls.Add(this.btnChangeMappingColor);
            this.panelAB.Controls.Add(this.btnABDraw);
            this.panelAB.Controls.Add(this.btnSnapToPivot);
            this.panelAB.Controls.Add(this.btnEditAB);
            this.panelAB.Controls.Add(this.cboxpRowWidth);
            this.panelAB.Location = new System.Drawing.Point(75, 0);
            this.panelAB.Margin = new System.Windows.Forms.Padding(0);
            this.panelAB.Name = "panelAB";
            this.panelAB.Size = new System.Drawing.Size(950, 75);
            this.panelAB.TabIndex = 480;
            // 
            // btnResetToolHeading
            // 
            this.btnResetToolHeading.FlatAppearance.BorderSize = 0;
            this.btnResetToolHeading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetToolHeading.Image = global::AgOpenGPS.Properties.Resources.ResetTool;
            this.btnResetToolHeading.Location = new System.Drawing.Point(0, 0);
            this.btnResetToolHeading.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetToolHeading.Name = "btnResetToolHeading";
            this.btnResetToolHeading.Size = new System.Drawing.Size(75, 75);
            this.btnResetToolHeading.TabIndex = 492;
            this.btnResetToolHeading.UseVisualStyleBackColor = false;
            this.btnResetToolHeading.Click += new System.EventHandler(this.btnResetToolHeading_Click);
            // 
            // btnYouSkipEnable
            // 
            this.btnYouSkipEnable.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnYouSkipEnable.FlatAppearance.BorderSize = 0;
            this.btnYouSkipEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYouSkipEnable.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYouSkipEnable.Image = global::AgOpenGPS.Properties.Resources.YouSkipOff;
            this.btnYouSkipEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYouSkipEnable.Location = new System.Drawing.Point(788, 0);
            this.btnYouSkipEnable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnYouSkipEnable.Name = "btnYouSkipEnable";
            this.btnYouSkipEnable.Size = new System.Drawing.Size(75, 75);
            this.btnYouSkipEnable.TabIndex = 490;
            this.btnYouSkipEnable.UseVisualStyleBackColor = false;
            this.btnYouSkipEnable.Visible = false;
            this.btnYouSkipEnable.Click += new System.EventHandler(this.btnYouSkipEnable_Click);
            // 
            // btnHeadlandOnOff
            // 
            this.btnHeadlandOnOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeadlandOnOff.Image = global::AgOpenGPS.Properties.Resources.HeadlandOff;
            this.btnHeadlandOnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOnOff.Location = new System.Drawing.Point(88, 0);
            this.btnHeadlandOnOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHeadlandOnOff.Name = "btnHeadlandOnOff";
            this.btnHeadlandOnOff.Size = new System.Drawing.Size(75, 75);
            this.btnHeadlandOnOff.TabIndex = 447;
            this.btnHeadlandOnOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHeadlandOnOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOnOff.Visible = false;
            this.btnHeadlandOnOff.Click += new System.EventHandler(this.btnHeadlandOnOff_Click);
            // 
            // btnHydLift
            // 
            this.btnHydLift.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnHydLift.FlatAppearance.BorderSize = 0;
            this.btnHydLift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHydLift.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnHydLift.Image = global::AgOpenGPS.Properties.Resources.HydraulicLiftOff;
            this.btnHydLift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHydLift.Location = new System.Drawing.Point(175, 0);
            this.btnHydLift.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHydLift.Name = "btnHydLift";
            this.btnHydLift.Size = new System.Drawing.Size(75, 75);
            this.btnHydLift.TabIndex = 453;
            this.btnHydLift.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHydLift.UseVisualStyleBackColor = false;
            this.btnHydLift.Visible = false;
            this.btnHydLift.Click += new System.EventHandler(this.btnHydLift_Click);
            // 
            // btnTramDisplayMode
            // 
            this.btnTramDisplayMode.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnTramDisplayMode.FlatAppearance.BorderSize = 0;
            this.btnTramDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTramDisplayMode.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTramDisplayMode.Image = global::AgOpenGPS.Properties.Resources.TramOff;
            this.btnTramDisplayMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTramDisplayMode.Location = new System.Drawing.Point(262, 0);
            this.btnTramDisplayMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTramDisplayMode.Name = "btnTramDisplayMode";
            this.btnTramDisplayMode.Size = new System.Drawing.Size(75, 75);
            this.btnTramDisplayMode.TabIndex = 480;
            this.btnTramDisplayMode.UseVisualStyleBackColor = false;
            this.btnTramDisplayMode.Click += new System.EventHandler(this.btnTramDisplayMode_Click);
            // 
            // btnFlag
            // 
            this.btnFlag.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnFlag.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFlag.FlatAppearance.BorderSize = 0;
            this.btnFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlag.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlag.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.btnFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFlag.Location = new System.Drawing.Point(350, 0);
            this.btnFlag.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFlag.Name = "btnFlag";
            this.btnFlag.Size = new System.Drawing.Size(75, 75);
            this.btnFlag.TabIndex = 121;
            this.btnFlag.UseVisualStyleBackColor = false;
            this.btnFlag.Click += new System.EventHandler(this.btnFlag_Click);
            // 
            // btnChangeMappingColor
            // 
            this.btnChangeMappingColor.BackColor = System.Drawing.Color.SkyBlue;
            this.btnChangeMappingColor.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionMapping;
            this.btnChangeMappingColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChangeMappingColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChangeMappingColor.FlatAppearance.BorderSize = 0;
            this.btnChangeMappingColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeMappingColor.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeMappingColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeMappingColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeMappingColor.Location = new System.Drawing.Point(438, 5);
            this.btnChangeMappingColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeMappingColor.Name = "btnChangeMappingColor";
            this.btnChangeMappingColor.Size = new System.Drawing.Size(75, 75);
            this.btnChangeMappingColor.TabIndex = 476;
            this.btnChangeMappingColor.UseVisualStyleBackColor = false;
            this.btnChangeMappingColor.Click += new System.EventHandler(this.btnChangeMappingColor_Click);
            // 
            // btnABDraw
            // 
            this.btnABDraw.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnABDraw.FlatAppearance.BorderSize = 0;
            this.btnABDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnABDraw.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnABDraw.Image = global::AgOpenGPS.Properties.Resources.PointStart;
            this.btnABDraw.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnABDraw.Location = new System.Drawing.Point(525, 0);
            this.btnABDraw.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnABDraw.Name = "btnABDraw";
            this.btnABDraw.Size = new System.Drawing.Size(75, 75);
            this.btnABDraw.TabIndex = 250;
            this.btnABDraw.UseVisualStyleBackColor = false;
            this.btnABDraw.Visible = false;
            this.btnABDraw.Click += new System.EventHandler(this.btnABDraw_Click);
            // 
            // btnSnapToPivot
            // 
            this.btnSnapToPivot.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSnapToPivot.FlatAppearance.BorderSize = 0;
            this.btnSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapToPivot.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSnapToPivot.ForeColor = System.Drawing.Color.DarkGray;
            this.btnSnapToPivot.Image = global::AgOpenGPS.Properties.Resources.SnapToPivot;
            this.btnSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSnapToPivot.Location = new System.Drawing.Point(612, 0);
            this.btnSnapToPivot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSnapToPivot.Name = "btnSnapToPivot";
            this.btnSnapToPivot.Size = new System.Drawing.Size(75, 75);
            this.btnSnapToPivot.TabIndex = 477;
            this.btnSnapToPivot.UseVisualStyleBackColor = false;
            this.btnSnapToPivot.Visible = false;
            this.btnSnapToPivot.Click += new System.EventHandler(this.btnSnapToPivot_Click);
            // 
            // btnEditAB
            // 
            this.btnEditAB.FlatAppearance.BorderSize = 0;
            this.btnEditAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditAB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditAB.Image = global::AgOpenGPS.Properties.Resources.ABLineEdit;
            this.btnEditAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditAB.Location = new System.Drawing.Point(700, 0);
            this.btnEditAB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditAB.Name = "btnEditAB";
            this.btnEditAB.Size = new System.Drawing.Size(75, 75);
            this.btnEditAB.TabIndex = 489;
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
            this.lblSpeed.Location = new System.Drawing.Point(100, 6);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(150, 62);
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
            this.lblTopData.Location = new System.Drawing.Point(126, 0);
            this.lblTopData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTopData.Name = "lblTopData";
            this.lblTopData.Size = new System.Drawing.Size(200, 21);
            this.lblTopData.TabIndex = 483;
            this.lblTopData.Text = "Vehicle Name + Width";
            // 
            // lblInty
            // 
            this.lblInty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInty.BackColor = System.Drawing.Color.Transparent;
            this.lblInty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInty.Location = new System.Drawing.Point(0, 712);
            this.lblInty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInty.Name = "lblInty";
            this.lblInty.Size = new System.Drawing.Size(75, 25);
            this.lblInty.TabIndex = 485;
            this.lblInty.Text = "0";
            this.lblInty.Click += new System.EventHandler(this.lblInty_Click);
            // 
            // lblCurveLineName
            // 
            this.lblCurveLineName.AutoSize = true;
            this.lblCurveLineName.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveLineName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveLineName.Location = new System.Drawing.Point(484, 0);
            this.lblCurveLineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurveLineName.Name = "lblCurveLineName";
            this.lblCurveLineName.Size = new System.Drawing.Size(46, 21);
            this.lblCurveLineName.TabIndex = 486;
            this.lblCurveLineName.Text = "Line";
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.AutoSize = true;
            this.lblCurrentField.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentField.Location = new System.Drawing.Point(126, 21);
            this.lblCurrentField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(98, 21);
            this.lblCurrentField.TabIndex = 488;
            this.lblCurrentField.Text = "Fieldname";
            // 
            // lblFix
            // 
            this.lblFix.BackColor = System.Drawing.Color.Transparent;
            this.lblFix.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFix.Location = new System.Drawing.Point(0, 50);
            this.lblFix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(100, 25);
            this.lblFix.TabIndex = 489;
            this.lblFix.Text = "GPS single: ";
            this.lblFix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbludpWatchCounts
            // 
            this.lbludpWatchCounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbludpWatchCounts.BackColor = System.Drawing.Color.Transparent;
            this.lbludpWatchCounts.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbludpWatchCounts.Location = new System.Drawing.Point(0, 488);
            this.lbludpWatchCounts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbludpWatchCounts.Name = "lbludpWatchCounts";
            this.lbludpWatchCounts.Size = new System.Drawing.Size(75, 25);
            this.lbludpWatchCounts.TabIndex = 492;
            this.lbludpWatchCounts.Text = "0";
            this.lbludpWatchCounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbludpWatchCounts.Click += new System.EventHandler(this.lbludpWatchCounts_Click);
            // 
            // lblAge
            // 
            this.lblAge.BackColor = System.Drawing.Color.Transparent;
            this.lblAge.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.Location = new System.Drawing.Point(0, 0);
            this.lblAge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(100, 25);
            this.lblAge.TabIndex = 493;
            this.lblAge.Text = "Age: age";
            this.lblAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnStanleyPure.Location = new System.Drawing.Point(0, 750);
            this.btnStanleyPure.Margin = new System.Windows.Forms.Padding(0);
            this.btnStanleyPure.Name = "btnStanleyPure";
            this.btnStanleyPure.Size = new System.Drawing.Size(75, 75);
            this.btnStanleyPure.TabIndex = 490;
            this.btnStanleyPure.Text = "mo";
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
            this.btnAutoSteerConfig.Location = new System.Drawing.Point(0, 625);
            this.btnAutoSteerConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteerConfig.Name = "btnAutoSteerConfig";
            this.btnAutoSteerConfig.Size = new System.Drawing.Size(75, 75);
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
            this.btnMaximizeMainForm.Location = new System.Drawing.Point(400, 0);
            this.btnMaximizeMainForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMaximizeMainForm.Name = "btnMaximizeMainForm";
            this.btnMaximizeMainForm.Size = new System.Drawing.Size(75, 75);
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
            this.btnMinimizeMainForm.Location = new System.Drawing.Point(325, 0);
            this.btnMinimizeMainForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMinimizeMainForm.Name = "btnMinimizeMainForm";
            this.btnMinimizeMainForm.Size = new System.Drawing.Size(75, 75);
            this.btnMinimizeMainForm.TabIndex = 481;
            this.btnMinimizeMainForm.UseVisualStyleBackColor = false;
            this.btnMinimizeMainForm.Click += new System.EventHandler(this.btnMinimizeMainForm_Click);
            // 
            // pictureboxStart
            // 
            this.pictureboxStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureboxStart.BackgroundImage = global::AgOpenGPS.Properties.Resources.first;
            this.pictureboxStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureboxStart.Location = new System.Drawing.Point(960, 356);
            this.pictureboxStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureboxStart.Name = "pictureboxStart";
            this.pictureboxStart.Size = new System.Drawing.Size(92, 75);
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
            this.btnStartAgIO.Location = new System.Drawing.Point(0, 525);
            this.btnStartAgIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartAgIO.Name = "btnStartAgIO";
            this.btnStartAgIO.Size = new System.Drawing.Size(75, 75);
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
            this.btnShutdown.Location = new System.Drawing.Point(475, 0);
            this.btnShutdown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(75, 75);
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
            this.btnHelp.Location = new System.Drawing.Point(250, 0);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 75);
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
            this.panelMain.Controls.Add(this.paneltop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1250, 900);
            this.panelMain.TabIndex = 496;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelAB);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(75, 825);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 75);
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
            this.panelLeft.Location = new System.Drawing.Point(0, 75);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(75, 825);
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
            this.toolStripDropDownButton4.Location = new System.Drawing.Point(0, 275);
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(75, 75);
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
            this.toolStripBtnField.Location = new System.Drawing.Point(0, 375);
            this.toolStripBtnField.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnField.Name = "toolStripBtnField";
            this.toolStripBtnField.Size = new System.Drawing.Size(75, 75);
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
            this.stripBtnConfig.Location = new System.Drawing.Point(0, 175);
            this.stripBtnConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.stripBtnConfig.Name = "stripBtnConfig";
            this.stripBtnConfig.Size = new System.Drawing.Size(75, 75);
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
            this.simplifyToolStrip.Location = new System.Drawing.Point(0, 75);
            this.simplifyToolStrip.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.simplifyToolStrip.Name = "simplifyToolStrip";
            this.simplifyToolStrip.Size = new System.Drawing.Size(75, 75);
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
            this.distanceToolBtn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.distanceToolBtn.Name = "distanceToolBtn";
            this.distanceToolBtn.Size = new System.Drawing.Size(75, 50);
            this.distanceToolBtn.TabIndex = 493;
            this.distanceToolBtn.Text = "25 Ha";
            this.distanceToolBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.distanceToolBtn.Click += new System.EventHandler(this.toolStripDropDownButtonDistance_Click);
            // 
            // paneltop
            // 
            this.paneltop.Controls.Add(this.btnMenu);
            this.paneltop.Controls.Add(this.panelTopRight);
            this.paneltop.Controls.Add(this.lblCurveLineName);
            this.paneltop.Controls.Add(this.lblTopData);
            this.paneltop.Controls.Add(this.lblCurrentField);
            this.paneltop.Controls.Add(this.lblFieldStatus);
            this.paneltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneltop.Location = new System.Drawing.Point(0, 0);
            this.paneltop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.paneltop.Name = "paneltop";
            this.paneltop.Size = new System.Drawing.Size(1250, 75);
            this.paneltop.TabIndex = 498;
            // 
            // btnMenu
            // 
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Image = global::AgOpenGPS.Properties.Resources.fileMenu;
            this.btnMenu.Location = new System.Drawing.Point(0, 0);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(100, 75);
            this.btnMenu.TabIndex = 498;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // panelTopRight
            // 
            this.panelTopRight.Controls.Add(this.lblSpeed);
            this.panelTopRight.Controls.Add(this.btnHelp);
            this.panelTopRight.Controls.Add(this.btnMinimizeMainForm);
            this.panelTopRight.Controls.Add(this.btnMaximizeMainForm);
            this.panelTopRight.Controls.Add(this.btnShutdown);
            this.panelTopRight.Controls.Add(this.lblAge);
            this.panelTopRight.Controls.Add(this.lblFix);
            this.panelTopRight.Controls.Add(this.lblHz);
            this.panelTopRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelTopRight.Location = new System.Drawing.Point(700, 0);
            this.panelTopRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTopRight.Name = "panelTopRight";
            this.panelTopRight.Size = new System.Drawing.Size(550, 75);
            this.panelTopRight.TabIndex = 497;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.contextMenuStrip2.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.boundariesToolStripMenuItem,
            this.headlandToolStripMenuItem,
            this.tramLinesMenuField,
            this.toolStripBtnMakeBndContour});
            this.contextMenuStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStrip2.Name = "contextMenuStripFlag";
            this.contextMenuStrip2.Size = new System.Drawing.Size(457, 354);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Image = global::AgOpenGPS.Properties.Resources.JobClose;
            this.toolStripMenuItem9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMenuItem9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(456, 70);
            this.toolStripMenuItem9.Text = "Field";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripBtnField_Click);
            // 
            // boundariesToolStripMenuItem
            // 
            this.boundariesToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.MakeBoundary;
            this.boundariesToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.boundariesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.boundariesToolStripMenuItem.Name = "boundariesToolStripMenuItem";
            this.boundariesToolStripMenuItem.Size = new System.Drawing.Size(456, 70);
            this.boundariesToolStripMenuItem.Text = "Boundary";
            this.boundariesToolStripMenuItem.Click += new System.EventHandler(this.boundariesToolStripMenuItem_Click);
            // 
            // headlandToolStripMenuItem
            // 
            this.headlandToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.HeadlandMenu;
            this.headlandToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.headlandToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.headlandToolStripMenuItem.Name = "headlandToolStripMenuItem";
            this.headlandToolStripMenuItem.Size = new System.Drawing.Size(456, 70);
            this.headlandToolStripMenuItem.Text = "Headland";
            this.headlandToolStripMenuItem.Click += new System.EventHandler(this.headlandToolStripMenuItem_Click);
            // 
            // tramLinesMenuField
            // 
            this.tramLinesMenuField.Image = global::AgOpenGPS.Properties.Resources.ABTramLine;
            this.tramLinesMenuField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tramLinesMenuField.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tramLinesMenuField.Name = "tramLinesMenuField";
            this.tramLinesMenuField.Size = new System.Drawing.Size(456, 70);
            this.tramLinesMenuField.Text = "TramLines";
            this.tramLinesMenuField.Click += new System.EventHandler(this.tramLinesMenuField_Click);
            // 
            // toolStripBtnMakeBndContour
            // 
            this.toolStripBtnMakeBndContour.Image = global::AgOpenGPS.Properties.Resources.MakeBoundary;
            this.toolStripBtnMakeBndContour.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnMakeBndContour.Name = "toolStripBtnMakeBndContour";
            this.toolStripBtnMakeBndContour.Size = new System.Drawing.Size(456, 70);
            this.toolStripBtnMakeBndContour.Text = "Boundary Contour";
            this.toolStripBtnMakeBndContour.Click += new System.EventHandler(this.toolStripBtnMakeBndContour_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(479, 564);
            // 
            // toolSteerSettingsToolStripMenuItem
            // 
            this.toolSteerSettingsToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_ModulesSteer;
            this.toolSteerSettingsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSteerSettingsToolStripMenuItem.Name = "toolSteerSettingsToolStripMenuItem";
            this.toolSteerSettingsToolStripMenuItem.ShowShortcutKeys = false;
            this.toolSteerSettingsToolStripMenuItem.Size = new System.Drawing.Size(478, 70);
            this.toolSteerSettingsToolStripMenuItem.Text = "Tool Steer Settings";
            this.toolSteerSettingsToolStripMenuItem.Click += new System.EventHandler(this.toolSteerSettingsToolStripMenuItem_Click);
            // 
            // FormGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1250, 900);
            this.Controls.Add(this.pictureboxStart);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.oglZoom);
            this.Controls.Add(this.oglBack);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1250, 900);
            this.Name = "FormGPS";
            this.Text = "AgOpenGPS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPS_FormClosing);
            this.Load += new System.EventHandler(this.FormGPS_Load);
            this.fileToolStripMenuItem.ResumeLayout(false);
            this.contextMenuStripFlag.ResumeLayout(false);
            this.panelSim.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.panelAB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxStart)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.paneltop.ResumeLayout(false);
            this.paneltop.PerformLayout();
            this.panelTopRight.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip fileToolStripMenuItem;
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
        private System.Windows.Forms.Timer timerSim;
        private System.Windows.Forms.Button btnManualOffOn;
        private System.Windows.Forms.ToolStripMenuItem menustripLanguage;
        private System.Windows.Forms.Panel panelSim;
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
        private System.Windows.Forms.ToolStripMenuItem steerChartStripMenu;
        private System.Windows.Forms.ToolStripMenuItem webcamToolStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteAppliedAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offsetFixToolStrip;
        private System.Windows.Forms.TableLayoutPanel panelNavigation;
        private System.Windows.Forms.Label lblFieldStatus;
        private System.Windows.Forms.ToolStripMenuItem deleteContourPathsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureboxStart;
        private System.Windows.Forms.Panel panelAB;
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
        private System.Windows.Forms.Button btnAutoSteerConfig;
		public System.Windows.Forms.Button btnBrightnessDn;
        public System.Windows.Forms.Button btnBrightnessUp;

        private System.Windows.Forms.TableLayoutPanel panelRight;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageTurkish;
        private Panel1 panelMain;
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
        private System.Windows.Forms.Button toolStripBtnField;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button toolStripDropDownButton4;
        private System.Windows.Forms.Panel panelTopRight;
        private System.Windows.Forms.ToolStripMenuItem toolSteerSettingsToolStripMenuItem;
        private System.Windows.Forms.Button btnRecPath;
        private Panel1 paneltop;
        private System.Windows.Forms.Button btnResetToolHeading;
        private System.Windows.Forms.Button btnMenu;
    }
}