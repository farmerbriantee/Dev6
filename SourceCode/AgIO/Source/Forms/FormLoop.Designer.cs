
namespace AgIO
{
    partial class FormLoop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoop));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCurentLon = new System.Windows.Forms.Label();
            this.lblCurrentLat = new System.Windows.Forms.Label();
            this.lblWatch = new System.Windows.Forms.Label();
            this.lblNTRIPBytes = new System.Windows.Forms.Label();
            this.lblToAOG = new System.Windows.Forms.Label();
            this.lblFromAOG = new System.Windows.Forms.Label();
            this.lblToMachine = new System.Windows.Forms.Label();
            this.lblToSteer = new System.Windows.Forms.Label();
            this.lblFromMachine = new System.Windows.Forms.Label();
            this.lblFromSteer = new System.Windows.Forms.Label();
            this.lblToGPS = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.lblGPS1Comm = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.settingsMenuStrip = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripGPSData = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAgDiag = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.uDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnToolSteer = new System.Windows.Forms.Button();
            this.lblFromModule3 = new System.Windows.Forms.Label();
            this.lblToModule3 = new System.Windows.Forms.Label();
            this.lblFromGPS2 = new System.Windows.Forms.Label();
            this.lblToGPS2 = new System.Windows.Forms.Label();
            this.btnGPS2 = new System.Windows.Forms.Button();
            this.cboxLogNMEA = new System.Windows.Forms.CheckBox();
            this.btnStartStopNtrip = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnNTRIP = new System.Windows.Forms.Button();
            this.btnRunAOG = new System.Windows.Forms.Button();
            this.btnSteer = new System.Windows.Forms.Button();
            this.btnMachine = new System.Windows.Forms.Button();
            this.btnGPS = new System.Windows.Forms.Button();
            this.btnAOGButton = new System.Windows.Forms.Button();
            this.btnUDP = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntripMeterTimer = new System.Windows.Forms.Timer(this.components);
            this.lblCount = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 4000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(22, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 19);
            this.label6.TabIndex = 151;
            this.label6.Text = "Lat";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(17, 33);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 19);
            this.label8.TabIndex = 152;
            this.label8.Text = "Lon";
            // 
            // lblCurentLon
            // 
            this.lblCurentLon.AutoSize = true;
            this.lblCurentLon.BackColor = System.Drawing.Color.Transparent;
            this.lblCurentLon.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurentLon.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCurentLon.Location = new System.Drawing.Point(53, 34);
            this.lblCurentLon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurentLon.Name = "lblCurentLon";
            this.lblCurentLon.Size = new System.Drawing.Size(44, 18);
            this.lblCurentLon.TabIndex = 154;
            this.lblCurentLon.Text = "-111";
            // 
            // lblCurrentLat
            // 
            this.lblCurrentLat.AutoSize = true;
            this.lblCurrentLat.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentLat.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLat.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCurrentLat.Location = new System.Drawing.Point(54, 10);
            this.lblCurrentLat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentLat.Name = "lblCurrentLat";
            this.lblCurrentLat.Size = new System.Drawing.Size(28, 18);
            this.lblCurrentLat.TabIndex = 153;
            this.lblCurrentLat.Text = "53";
            // 
            // lblWatch
            // 
            this.lblWatch.BackColor = System.Drawing.Color.Transparent;
            this.lblWatch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWatch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWatch.Location = new System.Drawing.Point(5, 157);
            this.lblWatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWatch.Name = "lblWatch";
            this.lblWatch.Size = new System.Drawing.Size(117, 16);
            this.lblWatch.TabIndex = 146;
            this.lblWatch.Text = "Watch";
            this.lblWatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNTRIPBytes
            // 
            this.lblNTRIPBytes.BackColor = System.Drawing.Color.Transparent;
            this.lblNTRIPBytes.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNTRIPBytes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblNTRIPBytes.Location = new System.Drawing.Point(6, 176);
            this.lblNTRIPBytes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNTRIPBytes.Name = "lblNTRIPBytes";
            this.lblNTRIPBytes.Size = new System.Drawing.Size(112, 27);
            this.lblNTRIPBytes.TabIndex = 148;
            this.lblNTRIPBytes.Text = "999,999,999";
            this.lblNTRIPBytes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToAOG
            // 
            this.lblToAOG.BackColor = System.Drawing.Color.Transparent;
            this.lblToAOG.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToAOG.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblToAOG.Location = new System.Drawing.Point(158, 132);
            this.lblToAOG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToAOG.Name = "lblToAOG";
            this.lblToAOG.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblToAOG.Size = new System.Drawing.Size(64, 27);
            this.lblToAOG.TabIndex = 123;
            this.lblToAOG.Text = "0000";
            this.lblToAOG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromAOG
            // 
            this.lblFromAOG.BackColor = System.Drawing.Color.Transparent;
            this.lblFromAOG.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromAOG.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFromAOG.Location = new System.Drawing.Point(320, 132);
            this.lblFromAOG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromAOG.Name = "lblFromAOG";
            this.lblFromAOG.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFromAOG.Size = new System.Drawing.Size(64, 27);
            this.lblFromAOG.TabIndex = 126;
            this.lblFromAOG.Text = "0000";
            this.lblFromAOG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToMachine
            // 
            this.lblToMachine.BackColor = System.Drawing.Color.Transparent;
            this.lblToMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToMachine.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblToMachine.Location = new System.Drawing.Point(161, 389);
            this.lblToMachine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToMachine.Name = "lblToMachine";
            this.lblToMachine.Size = new System.Drawing.Size(64, 27);
            this.lblToMachine.TabIndex = 163;
            this.lblToMachine.Text = "---";
            this.lblToMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToSteer
            // 
            this.lblToSteer.BackColor = System.Drawing.Color.Transparent;
            this.lblToSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToSteer.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblToSteer.Location = new System.Drawing.Point(161, 299);
            this.lblToSteer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToSteer.Name = "lblToSteer";
            this.lblToSteer.Size = new System.Drawing.Size(64, 27);
            this.lblToSteer.TabIndex = 141;
            this.lblToSteer.Text = "---";
            this.lblToSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMachine
            // 
            this.lblFromMachine.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMachine.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFromMachine.Location = new System.Drawing.Point(320, 388);
            this.lblFromMachine.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMachine.Name = "lblFromMachine";
            this.lblFromMachine.Size = new System.Drawing.Size(64, 27);
            this.lblFromMachine.TabIndex = 164;
            this.lblFromMachine.Text = "---";
            this.lblFromMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromSteer
            // 
            this.lblFromSteer.BackColor = System.Drawing.Color.Transparent;
            this.lblFromSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromSteer.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFromSteer.Location = new System.Drawing.Point(320, 299);
            this.lblFromSteer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromSteer.Name = "lblFromSteer";
            this.lblFromSteer.Size = new System.Drawing.Size(64, 27);
            this.lblFromSteer.TabIndex = 142;
            this.lblFromSteer.Text = "---";
            this.lblFromSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToGPS
            // 
            this.lblToGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblToGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToGPS.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblToGPS.Location = new System.Drawing.Point(161, 201);
            this.lblToGPS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToGPS.Name = "lblToGPS";
            this.lblToGPS.Size = new System.Drawing.Size(64, 27);
            this.lblToGPS.TabIndex = 128;
            this.lblToGPS.Text = "---";
            this.lblToGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFromGPS.Location = new System.Drawing.Point(320, 216);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS.TabIndex = 130;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGPS1Comm
            // 
            this.lblGPS1Comm.BackColor = System.Drawing.Color.Transparent;
            this.lblGPS1Comm.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS1Comm.ForeColor = System.Drawing.Color.Black;
            this.lblGPS1Comm.Location = new System.Drawing.Point(77, 222);
            this.lblGPS1Comm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGPS1Comm.Name = "lblGPS1Comm";
            this.lblGPS1Comm.Size = new System.Drawing.Size(80, 22);
            this.lblGPS1Comm.TabIndex = 176;
            this.lblGPS1Comm.Text = "--";
            this.lblGPS1Comm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsMenuStrip});
            this.statusStrip1.Location = new System.Drawing.Point(108, 449);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(87, 65);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 149;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // settingsMenuStrip
            // 
            this.settingsMenuStrip.AutoSize = false;
            this.settingsMenuStrip.BackColor = System.Drawing.Color.Gainsboro;
            this.settingsMenuStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripGPSData,
            this.deviceManagerToolStripMenuItem,
            this.toolStripAgDiag,
            this.saveToolStrip,
            this.loadToolStrip,
            this.uDPToolStripMenuItem});
            this.settingsMenuStrip.Image = global::AgIO.Properties.Resources.Settings48;
            this.settingsMenuStrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.settingsMenuStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsMenuStrip.Name = "settingsMenuStrip";
            this.settingsMenuStrip.ShowDropDownArrow = false;
            this.settingsMenuStrip.Size = new System.Drawing.Size(64, 63);
            // 
            // toolStripGPSData
            // 
            this.toolStripGPSData.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripGPSData.Image = global::AgIO.Properties.Resources.RadioSettings;
            this.toolStripGPSData.Name = "toolStripGPSData";
            this.toolStripGPSData.Size = new System.Drawing.Size(276, 70);
            this.toolStripGPSData.Text = "Radio";
            this.toolStripGPSData.Click += new System.EventHandler(this.toolStripGPSData_Click);
            // 
            // deviceManagerToolStripMenuItem
            // 
            this.deviceManagerToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceManagerToolStripMenuItem.Image = global::AgIO.Properties.Resources.DeviceManager;
            this.deviceManagerToolStripMenuItem.Name = "deviceManagerToolStripMenuItem";
            this.deviceManagerToolStripMenuItem.Size = new System.Drawing.Size(276, 70);
            this.deviceManagerToolStripMenuItem.Text = "Device Manager";
            this.deviceManagerToolStripMenuItem.Click += new System.EventHandler(this.deviceManagerToolStripMenuItem_Click);
            // 
            // toolStripAgDiag
            // 
            this.toolStripAgDiag.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripAgDiag.Image = global::AgIO.Properties.Resources.AgDiagButton;
            this.toolStripAgDiag.Name = "toolStripAgDiag";
            this.toolStripAgDiag.Size = new System.Drawing.Size(276, 70);
            this.toolStripAgDiag.Text = "AgDiag";
            this.toolStripAgDiag.Click += new System.EventHandler(this.toolStripAgDiag_Click);
            // 
            // saveToolStrip
            // 
            this.saveToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToolStrip.Image = global::AgIO.Properties.Resources.VehFileSave;
            this.saveToolStrip.Name = "saveToolStrip";
            this.saveToolStrip.Size = new System.Drawing.Size(276, 70);
            this.saveToolStrip.Text = "Save";
            this.saveToolStrip.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // loadToolStrip
            // 
            this.loadToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadToolStrip.Image = global::AgIO.Properties.Resources.VehFileLoad;
            this.loadToolStrip.Name = "loadToolStrip";
            this.loadToolStrip.Size = new System.Drawing.Size(276, 70);
            this.loadToolStrip.Text = "Load";
            this.loadToolStrip.Click += new System.EventHandler(this.loadToolStrip_Click);
            // 
            // uDPToolStripMenuItem
            // 
            this.uDPToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uDPToolStripMenuItem.Image = global::AgIO.Properties.Resources.B_UDP;
            this.uDPToolStripMenuItem.Name = "uDPToolStripMenuItem";
            this.uDPToolStripMenuItem.Size = new System.Drawing.Size(276, 70);
            this.uDPToolStripMenuItem.Text = "UDP";
            this.uDPToolStripMenuItem.Click += new System.EventHandler(this.uDPToolStripMenuItem_Click);
            // 
            // btnToolSteer
            // 
            this.btnToolSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnToolSteer.BackgroundImage = global::AgIO.Properties.Resources.B_Autosteer2;
            this.btnToolSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnToolSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnToolSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolSteer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToolSteer.ForeColor = System.Drawing.Color.White;
            this.btnToolSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnToolSteer.Location = new System.Drawing.Point(463, 287);
            this.btnToolSteer.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolSteer.Name = "btnToolSteer";
            this.btnToolSteer.Size = new System.Drawing.Size(103, 59);
            this.btnToolSteer.TabIndex = 470;
            this.btnToolSteer.UseVisualStyleBackColor = false;
            // 
            // lblFromModule3
            // 
            this.lblFromModule3.BackColor = System.Drawing.Color.Transparent;
            this.lblFromModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromModule3.ForeColor = System.Drawing.Color.Black;
            this.lblFromModule3.Location = new System.Drawing.Point(570, 302);
            this.lblFromModule3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromModule3.Name = "lblFromModule3";
            this.lblFromModule3.Size = new System.Drawing.Size(64, 27);
            this.lblFromModule3.TabIndex = 468;
            this.lblFromModule3.Text = "---";
            this.lblFromModule3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToModule3
            // 
            this.lblToModule3.BackColor = System.Drawing.Color.Transparent;
            this.lblToModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToModule3.ForeColor = System.Drawing.Color.Black;
            this.lblToModule3.Location = new System.Drawing.Point(394, 301);
            this.lblToModule3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToModule3.Name = "lblToModule3";
            this.lblToModule3.Size = new System.Drawing.Size(64, 27);
            this.lblToModule3.TabIndex = 467;
            this.lblToModule3.Text = "---";
            this.lblToModule3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromGPS2
            // 
            this.lblFromGPS2.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS2.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS2.Location = new System.Drawing.Point(570, 218);
            this.lblFromGPS2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS2.Name = "lblFromGPS2";
            this.lblFromGPS2.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS2.TabIndex = 464;
            this.lblFromGPS2.Text = "---";
            this.lblFromGPS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToGPS2
            // 
            this.lblToGPS2.BackColor = System.Drawing.Color.Transparent;
            this.lblToGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToGPS2.ForeColor = System.Drawing.Color.Black;
            this.lblToGPS2.Location = new System.Drawing.Point(394, 217);
            this.lblToGPS2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToGPS2.Name = "lblToGPS2";
            this.lblToGPS2.Size = new System.Drawing.Size(64, 27);
            this.lblToGPS2.TabIndex = 463;
            this.lblToGPS2.Text = "---";
            this.lblToGPS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGPS2
            // 
            this.btnGPS2.BackColor = System.Drawing.Color.Transparent;
            this.btnGPS2.BackgroundImage = global::AgIO.Properties.Resources.B_GPS2;
            this.btnGPS2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGPS2.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnGPS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPS2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPS2.ForeColor = System.Drawing.Color.White;
            this.btnGPS2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPS2.Location = new System.Drawing.Point(463, 201);
            this.btnGPS2.Margin = new System.Windows.Forms.Padding(4);
            this.btnGPS2.Name = "btnGPS2";
            this.btnGPS2.Size = new System.Drawing.Size(103, 59);
            this.btnGPS2.TabIndex = 466;
            this.btnGPS2.UseVisualStyleBackColor = false;
            // 
            // cboxLogNMEA
            // 
            this.cboxLogNMEA.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxLogNMEA.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxLogNMEA.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxLogNMEA.FlatAppearance.BorderSize = 0;
            this.cboxLogNMEA.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxLogNMEA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxLogNMEA.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxLogNMEA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxLogNMEA.Image = global::AgIO.Properties.Resources.NtripBlank;
            this.cboxLogNMEA.Location = new System.Drawing.Point(7, 293);
            this.cboxLogNMEA.Name = "cboxLogNMEA";
            this.cboxLogNMEA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxLogNMEA.Size = new System.Drawing.Size(97, 46);
            this.cboxLogNMEA.TabIndex = 461;
            this.cboxLogNMEA.Text = "Log NMEA";
            this.cboxLogNMEA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxLogNMEA.UseVisualStyleBackColor = false;
            this.cboxLogNMEA.CheckedChanged += new System.EventHandler(this.cboxLogNMEA_CheckedChanged);
            // 
            // btnStartStopNtrip
            // 
            this.btnStartStopNtrip.BackColor = System.Drawing.Color.Transparent;
            this.btnStartStopNtrip.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnStartStopNtrip.FlatAppearance.BorderSize = 0;
            this.btnStartStopNtrip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopNtrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStopNtrip.ForeColor = System.Drawing.Color.Black;
            this.btnStartStopNtrip.Image = global::AgIO.Properties.Resources.NtripBlank;
            this.btnStartStopNtrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartStopNtrip.Location = new System.Drawing.Point(14, 104);
            this.btnStartStopNtrip.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopNtrip.Name = "btnStartStopNtrip";
            this.btnStartStopNtrip.Size = new System.Drawing.Size(97, 46);
            this.btnStartStopNtrip.TabIndex = 147;
            this.btnStartStopNtrip.Text = "StartStop";
            this.btnStartStopNtrip.UseVisualStyleBackColor = false;
            this.btnStartStopNtrip.Click += new System.EventHandler(this.btnStartStopNtrip_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::AgIO.Properties.Resources.SwitchOff;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(8, 452);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 58);
            this.btnExit.TabIndex = 192;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnNTRIP
            // 
            this.btnNTRIP.BackColor = System.Drawing.Color.Transparent;
            this.btnNTRIP.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnNTRIP.FlatAppearance.BorderSize = 0;
            this.btnNTRIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNTRIP.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNTRIP.ForeColor = System.Drawing.Color.White;
            this.btnNTRIP.Image = global::AgIO.Properties.Resources.NtripSettings;
            this.btnNTRIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNTRIP.Location = new System.Drawing.Point(228, 452);
            this.btnNTRIP.Margin = new System.Windows.Forms.Padding(4);
            this.btnNTRIP.Name = "btnNTRIP";
            this.btnNTRIP.Size = new System.Drawing.Size(63, 58);
            this.btnNTRIP.TabIndex = 191;
            this.btnNTRIP.UseVisualStyleBackColor = false;
            this.btnNTRIP.Click += new System.EventHandler(this.btnNTRIP_Click);
            // 
            // btnRunAOG
            // 
            this.btnRunAOG.BackColor = System.Drawing.Color.Transparent;
            this.btnRunAOG.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnRunAOG.FlatAppearance.BorderSize = 0;
            this.btnRunAOG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAOG.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAOG.ForeColor = System.Drawing.Color.White;
            this.btnRunAOG.Image = global::AgIO.Properties.Resources.AgIOBtn;
            this.btnRunAOG.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRunAOG.Location = new System.Drawing.Point(331, 452);
            this.btnRunAOG.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunAOG.Name = "btnRunAOG";
            this.btnRunAOG.Size = new System.Drawing.Size(63, 58);
            this.btnRunAOG.TabIndex = 190;
            this.btnRunAOG.UseVisualStyleBackColor = false;
            this.btnRunAOG.Click += new System.EventHandler(this.btnRunAOG_Click);
            // 
            // btnSteer
            // 
            this.btnSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnSteer.BackgroundImage = global::AgIO.Properties.Resources.B_Autosteer;
            this.btnSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteer.ForeColor = System.Drawing.Color.White;
            this.btnSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSteer.Location = new System.Drawing.Point(228, 287);
            this.btnSteer.Margin = new System.Windows.Forms.Padding(4);
            this.btnSteer.Name = "btnSteer";
            this.btnSteer.Size = new System.Drawing.Size(88, 59);
            this.btnSteer.TabIndex = 189;
            this.btnSteer.UseVisualStyleBackColor = false;
            // 
            // btnMachine
            // 
            this.btnMachine.BackColor = System.Drawing.Color.Transparent;
            this.btnMachine.BackgroundImage = global::AgIO.Properties.Resources.B_Machine;
            this.btnMachine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMachine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMachine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachine.ForeColor = System.Drawing.Color.White;
            this.btnMachine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMachine.Location = new System.Drawing.Point(228, 373);
            this.btnMachine.Margin = new System.Windows.Forms.Padding(4);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(88, 59);
            this.btnMachine.TabIndex = 188;
            this.btnMachine.UseVisualStyleBackColor = false;
            // 
            // btnGPS
            // 
            this.btnGPS.BackColor = System.Drawing.Color.Transparent;
            this.btnGPS.BackgroundImage = global::AgIO.Properties.Resources.B_GPS;
            this.btnGPS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGPS.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnGPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPS.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPS.ForeColor = System.Drawing.Color.White;
            this.btnGPS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPS.Location = new System.Drawing.Point(228, 201);
            this.btnGPS.Margin = new System.Windows.Forms.Padding(4);
            this.btnGPS.Name = "btnGPS";
            this.btnGPS.Size = new System.Drawing.Size(88, 59);
            this.btnGPS.TabIndex = 187;
            this.btnGPS.UseVisualStyleBackColor = false;
            this.btnGPS.Click += new System.EventHandler(this.btnBringUpCommSettings_Click);
            // 
            // btnAOGButton
            // 
            this.btnAOGButton.BackColor = System.Drawing.Color.Orange;
            this.btnAOGButton.BackgroundImage = global::AgIO.Properties.Resources.B_AoG;
            this.btnAOGButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAOGButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAOGButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAOGButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAOGButton.ForeColor = System.Drawing.Color.White;
            this.btnAOGButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAOGButton.Location = new System.Drawing.Point(228, 115);
            this.btnAOGButton.Margin = new System.Windows.Forms.Padding(4);
            this.btnAOGButton.Name = "btnAOGButton";
            this.btnAOGButton.Size = new System.Drawing.Size(88, 59);
            this.btnAOGButton.TabIndex = 186;
            this.btnAOGButton.UseVisualStyleBackColor = false;
            // 
            // btnUDP
            // 
            this.btnUDP.BackColor = System.Drawing.Color.Transparent;
            this.btnUDP.BackgroundImage = global::AgIO.Properties.Resources.B_UDP;
            this.btnUDP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUDP.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnUDP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUDP.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUDP.ForeColor = System.Drawing.Color.White;
            this.btnUDP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUDP.Location = new System.Drawing.Point(228, 44);
            this.btnUDP.Margin = new System.Windows.Forms.Padding(4);
            this.btnUDP.Name = "btnUDP";
            this.btnUDP.Size = new System.Drawing.Size(88, 59);
            this.btnUDP.TabIndex = 184;
            this.btnUDP.UseVisualStyleBackColor = false;
            this.btnUDP.Click += new System.EventHandler(this.btnUDP_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AgIO.Properties.Resources.InOut;
            this.pictureBox2.Location = new System.Drawing.Point(178, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(186, 36);
            this.pictureBox2.TabIndex = 183;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AgIO.Properties.Resources.FirstAgiO;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(120, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 35);
            this.pictureBox1.TabIndex = 182;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(39, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 471;
            this.label1.Text = "NTRIP";
            // 
            // ntripMeterTimer
            // 
            this.ntripMeterTimer.Enabled = true;
            this.ntripMeterTimer.Interval = 50;
            this.ntripMeterTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCount.Location = new System.Drawing.Point(161, 232);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(64, 27);
            this.lblCount.TabIndex = 472;
            this.lblCount.Text = "---";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLoop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(404, 521);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxLogNMEA);
            this.Controls.Add(this.btnToolSteer);
            this.Controls.Add(this.btnStartStopNtrip);
            this.Controls.Add(this.lblFromModule3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblGPS1Comm);
            this.Controls.Add(this.lblNTRIPBytes);
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblCurrentLat);
            this.Controls.Add(this.lblToModule3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblWatch);
            this.Controls.Add(this.btnSteer);
            this.Controls.Add(this.lblCurentLon);
            this.Controls.Add(this.lblFromGPS2);
            this.Controls.Add(this.lblToGPS);
            this.Controls.Add(this.btnRunAOG);
            this.Controls.Add(this.btnMachine);
            this.Controls.Add(this.lblToGPS2);
            this.Controls.Add(this.btnGPS);
            this.Controls.Add(this.btnAOGButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnGPS2);
            this.Controls.Add(this.btnUDP);
            this.Controls.Add(this.btnNTRIP);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblToAOG);
            this.Controls.Add(this.lblToMachine);
            this.Controls.Add(this.lblToSteer);
            this.Controls.Add(this.lblFromAOG);
            this.Controls.Add(this.lblFromMachine);
            this.Controls.Add(this.lblFromSteer);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(660, 560);
            this.MinimumSize = new System.Drawing.Size(420, 560);
            this.Name = "FormLoop";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "AgIO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLoop_FormClosing);
            this.Load += new System.EventHandler(this.FormLoop_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCurentLon;
        private System.Windows.Forms.Label lblCurrentLat;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblWatch;
        private System.Windows.Forms.Button btnStartStopNtrip;
        private System.Windows.Forms.Label lblNTRIPBytes;
        private System.Windows.Forms.Label lblToAOG;
        private System.Windows.Forms.Label lblFromAOG;
        private System.Windows.Forms.Label lblToMachine;
        private System.Windows.Forms.Label lblToSteer;
        private System.Windows.Forms.Label lblFromMachine;
        private System.Windows.Forms.Label lblFromSteer;
        private System.Windows.Forms.Label lblToGPS;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.Label lblGPS1Comm;
        private System.Windows.Forms.ToolStripDropDownButton settingsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deviceManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStrip;
        private System.Windows.Forms.ToolStripMenuItem loadToolStrip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnUDP;
        private System.Windows.Forms.Button btnAOGButton;
        private System.Windows.Forms.Button btnGPS;
        private System.Windows.Forms.Button btnMachine;
        private System.Windows.Forms.Button btnSteer;
        private System.Windows.Forms.Button btnRunAOG;
        private System.Windows.Forms.Button btnNTRIP;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolStripMenuItem uDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripGPSData;
        private System.Windows.Forms.ToolStripMenuItem toolStripAgDiag;
        private System.Windows.Forms.CheckBox cboxLogNMEA;
        private System.Windows.Forms.Label lblFromGPS2;
        private System.Windows.Forms.Label lblToGPS2;
        private System.Windows.Forms.Button btnGPS2;
        private System.Windows.Forms.Button btnToolSteer;
        private System.Windows.Forms.Label lblFromModule3;
        private System.Windows.Forms.Label lblToModule3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer ntripMeterTimer;
        private System.Windows.Forms.Label lblCount;
    }
}

