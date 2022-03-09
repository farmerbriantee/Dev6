namespace AgIO
{
    partial class FormCommTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCommTool));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblCurrentModule3Port = new System.Windows.Forms.Label();
            this.cboxModule3Port = new System.Windows.Forms.ComboBox();
            this.btnOpenSerialModule3 = new System.Windows.Forms.Button();
            this.btnCloseSerialModule3 = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIMU = new System.Windows.Forms.Label();
            this.lblSteer = new System.Windows.Forms.Label();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblGPS = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.lblFromMU = new System.Windows.Forms.Label();
            this.lblFromModule1 = new System.Windows.Forms.Label();
            this.lblFromModule2 = new System.Windows.Forms.Label();
            this.lblFromGPS2 = new System.Windows.Forms.Label();
            this.lblGPS2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnOpenGPS2 = new System.Windows.Forms.Button();
            this.btnCloseGPS2 = new System.Windows.Forms.Button();
            this.cboxBaud2 = new System.Windows.Forms.ComboBox();
            this.lblCurrentBaud2 = new System.Windows.Forms.Label();
            this.cboxPort2 = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort2 = new System.Windows.Forms.Label();
            this.textBoxRcv2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblToolSteer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFromToolSteer = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Controls.Add(this.lblCurrentModule3Port);
            this.groupBox5.Controls.Add(this.cboxModule3Port);
            this.groupBox5.Controls.Add(this.btnOpenSerialModule3);
            this.groupBox5.Controls.Add(this.btnCloseSerialModule3);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox5.Location = new System.Drawing.Point(116, 212);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(406, 118);
            this.groupBox5.TabIndex = 68;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Tool Steer Module";
            // 
            // lblCurrentModule3Port
            // 
            this.lblCurrentModule3Port.AutoSize = true;
            this.lblCurrentModule3Port.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentModule3Port.Location = new System.Drawing.Point(58, 40);
            this.lblCurrentModule3Port.Name = "lblCurrentModule3Port";
            this.lblCurrentModule3Port.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentModule3Port.TabIndex = 71;
            this.lblCurrentModule3Port.Text = "Port";
            // 
            // cboxModule3Port
            // 
            this.cboxModule3Port.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxModule3Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxModule3Port.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxModule3Port.FormattingEnabled = true;
            this.cboxModule3Port.Location = new System.Drawing.Point(39, 61);
            this.cboxModule3Port.Name = "cboxModule3Port";
            this.cboxModule3Port.Size = new System.Drawing.Size(124, 37);
            this.cboxModule3Port.TabIndex = 64;
            this.cboxModule3Port.SelectedIndexChanged += new System.EventHandler(this.cboxModule3Port_SelectedIndexChanged);
            // 
            // btnOpenSerialModule3
            // 
            this.btnOpenSerialModule3.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerialModule3.FlatAppearance.BorderSize = 0;
            this.btnOpenSerialModule3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerialModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialModule3.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenSerialModule3.Image")));
            this.btnOpenSerialModule3.Location = new System.Drawing.Point(186, 42);
            this.btnOpenSerialModule3.Name = "btnOpenSerialModule3";
            this.btnOpenSerialModule3.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerialModule3.TabIndex = 53;
            this.btnOpenSerialModule3.UseVisualStyleBackColor = false;
            this.btnOpenSerialModule3.Click += new System.EventHandler(this.btnOpenSerialModule3_Click);
            // 
            // btnCloseSerialModule3
            // 
            this.btnCloseSerialModule3.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerialModule3.FlatAppearance.BorderSize = 0;
            this.btnCloseSerialModule3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerialModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialModule3.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseSerialModule3.Image")));
            this.btnCloseSerialModule3.Location = new System.Drawing.Point(293, 40);
            this.btnCloseSerialModule3.Name = "btnCloseSerialModule3";
            this.btnCloseSerialModule3.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerialModule3.TabIndex = 52;
            this.btnCloseSerialModule3.UseVisualStyleBackColor = false;
            this.btnCloseSerialModule3.Click += new System.EventHandler(this.btnCloseSerialModule3_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRescan.BackColor = System.Drawing.Color.Transparent;
            this.btnRescan.FlatAppearance.BorderSize = 0;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescan.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRescan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRescan.Image = global::AgIO.Properties.Resources.ScanPorts;
            this.btnRescan.Location = new System.Drawing.Point(555, 364);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(89, 63);
            this.btnRescan.TabIndex = 58;
            this.btnRescan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // btnSerialOK
            // 
            this.btnSerialOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialOK.BackColor = System.Drawing.Color.Transparent;
            this.btnSerialOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialOK.FlatAppearance.BorderSize = 0;
            this.btnSerialOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialOK.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSerialOK.Image = global::AgIO.Properties.Resources.OK64;
            this.btnSerialOK.Location = new System.Drawing.Point(667, 366);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(91, 63);
            this.btnSerialOK.TabIndex = 59;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = false;
            this.btnSerialOK.Click += new System.EventHandler(this.btnSerialOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(590, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 73;
            this.label1.Text = "GPS:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(549, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 76;
            this.label4.Text = "Machine:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(578, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 23);
            this.label5.TabIndex = 77;
            this.label5.Text = "Steer:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(588, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 23);
            this.label6.TabIndex = 78;
            this.label6.Text = "IMU:";
            // 
            // lblIMU
            // 
            this.lblIMU.AutoSize = true;
            this.lblIMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMU.Location = new System.Drawing.Point(639, 236);
            this.lblIMU.Name = "lblIMU";
            this.lblIMU.Size = new System.Drawing.Size(50, 23);
            this.lblIMU.TabIndex = 83;
            this.lblIMU.Text = "IMU";
            // 
            // lblSteer
            // 
            this.lblSteer.AutoSize = true;
            this.lblSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteer.Location = new System.Drawing.Point(639, 271);
            this.lblSteer.Name = "lblSteer";
            this.lblSteer.Size = new System.Drawing.Size(60, 23);
            this.lblSteer.TabIndex = 82;
            this.lblSteer.Text = "Steer";
            // 
            // lblMachine
            // 
            this.lblMachine.AutoSize = true;
            this.lblMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMachine.Location = new System.Drawing.Point(639, 309);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(60, 23);
            this.lblMachine.TabIndex = 81;
            this.lblMachine.Text = "Mach";
            // 
            // lblGPS
            // 
            this.lblGPS.AutoSize = true;
            this.lblGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS.Location = new System.Drawing.Point(639, 201);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new System.Drawing.Size(48, 23);
            this.lblGPS.TabIndex = 79;
            this.lblGPS.Text = "GPS";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS.Location = new System.Drawing.Point(694, 199);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS.TabIndex = 172;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMU
            // 
            this.lblFromMU.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMU.ForeColor = System.Drawing.Color.Black;
            this.lblFromMU.Location = new System.Drawing.Point(694, 233);
            this.lblFromMU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMU.Name = "lblFromMU";
            this.lblFromMU.Size = new System.Drawing.Size(64, 27);
            this.lblFromMU.TabIndex = 175;
            this.lblFromMU.Text = "---";
            this.lblFromMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromModule1
            // 
            this.lblFromModule1.BackColor = System.Drawing.Color.Transparent;
            this.lblFromModule1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromModule1.ForeColor = System.Drawing.Color.Black;
            this.lblFromModule1.Location = new System.Drawing.Point(694, 269);
            this.lblFromModule1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromModule1.Name = "lblFromModule1";
            this.lblFromModule1.Size = new System.Drawing.Size(64, 27);
            this.lblFromModule1.TabIndex = 173;
            this.lblFromModule1.Text = "---";
            this.lblFromModule1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromModule2
            // 
            this.lblFromModule2.BackColor = System.Drawing.Color.Transparent;
            this.lblFromModule2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromModule2.ForeColor = System.Drawing.Color.Black;
            this.lblFromModule2.Location = new System.Drawing.Point(694, 307);
            this.lblFromModule2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromModule2.Name = "lblFromModule2";
            this.lblFromModule2.Size = new System.Drawing.Size(64, 27);
            this.lblFromModule2.TabIndex = 174;
            this.lblFromModule2.Text = "---";
            this.lblFromModule2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromGPS2
            // 
            this.lblFromGPS2.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS2.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS2.Location = new System.Drawing.Point(246, 354);
            this.lblFromGPS2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS2.Name = "lblFromGPS2";
            this.lblFromGPS2.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS2.TabIndex = 178;
            this.lblFromGPS2.Text = "---";
            this.lblFromGPS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGPS2
            // 
            this.lblGPS2.AutoSize = true;
            this.lblGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS2.Location = new System.Drawing.Point(190, 356);
            this.lblGPS2.Name = "lblGPS2";
            this.lblGPS2.Size = new System.Drawing.Size(48, 23);
            this.lblGPS2.TabIndex = 177;
            this.lblGPS2.Text = "GPS";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label9.Location = new System.Drawing.Point(131, 356);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 23);
            this.label9.TabIndex = 176;
            this.label9.Text = "GPS2:";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AgIO.Properties.Resources.satellite;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(2, 57);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(87, 90);
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
            // 
            // btnOpenGPS2
            // 
            this.btnOpenGPS2.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenGPS2.FlatAppearance.BorderSize = 0;
            this.btnOpenGPS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenGPS2.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenGPS2.Image")));
            this.btnOpenGPS2.Location = new System.Drawing.Point(471, 39);
            this.btnOpenGPS2.Name = "btnOpenGPS2";
            this.btnOpenGPS2.Size = new System.Drawing.Size(56, 58);
            this.btnOpenGPS2.TabIndex = 45;
            this.btnOpenGPS2.UseVisualStyleBackColor = false;
            this.btnOpenGPS2.Click += new System.EventHandler(this.btnOpenGPS2_Click);
            // 
            // btnCloseGPS2
            // 
            this.btnCloseGPS2.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseGPS2.FlatAppearance.BorderSize = 0;
            this.btnCloseGPS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseGPS2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseGPS2.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseGPS2.Image")));
            this.btnCloseGPS2.Location = new System.Drawing.Point(572, 39);
            this.btnCloseGPS2.Name = "btnCloseGPS2";
            this.btnCloseGPS2.Size = new System.Drawing.Size(56, 58);
            this.btnCloseGPS2.TabIndex = 44;
            this.btnCloseGPS2.UseVisualStyleBackColor = false;
            this.btnCloseGPS2.Click += new System.EventHandler(this.btnCloseGPS2_Click);
            // 
            // cboxBaud2
            // 
            this.cboxBaud2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud2.FormattingEnabled = true;
            this.cboxBaud2.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud2.Location = new System.Drawing.Point(257, 49);
            this.cboxBaud2.Name = "cboxBaud2";
            this.cboxBaud2.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud2.TabIndex = 49;
            this.cboxBaud2.SelectedIndexChanged += new System.EventHandler(this.cboxBaud2_SelectedIndexChanged);
            // 
            // lblCurrentBaud2
            // 
            this.lblCurrentBaud2.AutoSize = true;
            this.lblCurrentBaud2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud2.Location = new System.Drawing.Point(299, 28);
            this.lblCurrentBaud2.Name = "lblCurrentBaud2";
            this.lblCurrentBaud2.Size = new System.Drawing.Size(45, 18);
            this.lblCurrentBaud2.TabIndex = 46;
            this.lblCurrentBaud2.Text = "Baud";
            // 
            // cboxPort2
            // 
            this.cboxPort2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxPort2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxPort2.FormattingEnabled = true;
            this.cboxPort2.Location = new System.Drawing.Point(102, 49);
            this.cboxPort2.Name = "cboxPort2";
            this.cboxPort2.Size = new System.Drawing.Size(124, 37);
            this.cboxPort2.TabIndex = 50;
            this.cboxPort2.SelectedIndexChanged += new System.EventHandler(this.cboxPort2_SelectedIndexChanged);
            // 
            // lblCurrentPort2
            // 
            this.lblCurrentPort2.AutoSize = true;
            this.lblCurrentPort2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort2.Location = new System.Drawing.Point(141, 28);
            this.lblCurrentPort2.Name = "lblCurrentPort2";
            this.lblCurrentPort2.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentPort2.TabIndex = 47;
            this.lblCurrentPort2.Text = "Port";
            // 
            // textBoxRcv2
            // 
            this.textBoxRcv2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxRcv2.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxRcv2.Location = new System.Drawing.Point(21, 108);
            this.textBoxRcv2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv2.Multiline = true;
            this.textBoxRcv2.Name = "textBoxRcv2";
            this.textBoxRcv2.ReadOnly = true;
            this.textBoxRcv2.Size = new System.Drawing.Size(627, 38);
            this.textBoxRcv2.TabIndex = 40;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.textBoxRcv2);
            this.groupBox1.Controls.Add(this.lblCurrentPort2);
            this.groupBox1.Controls.Add(this.cboxPort2);
            this.groupBox1.Controls.Add(this.lblCurrentBaud2);
            this.groupBox1.Controls.Add(this.cboxBaud2);
            this.groupBox1.Controls.Add(this.btnCloseGPS2);
            this.groupBox1.Controls.Add(this.btnOpenGPS2);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(95, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(663, 154);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tool GPS";
            // 
            // lblToolSteer
            // 
            this.lblToolSteer.AutoSize = true;
            this.lblToolSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolSteer.Location = new System.Drawing.Point(190, 391);
            this.lblToolSteer.Name = "lblToolSteer";
            this.lblToolSteer.Size = new System.Drawing.Size(48, 23);
            this.lblToolSteer.TabIndex = 180;
            this.lblToolSteer.Text = "GPS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(131, 391);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 23);
            this.label3.TabIndex = 179;
            this.label3.Text = "Steer:";
            // 
            // lblFromToolSteer
            // 
            this.lblFromToolSteer.BackColor = System.Drawing.Color.Transparent;
            this.lblFromToolSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromToolSteer.ForeColor = System.Drawing.Color.Black;
            this.lblFromToolSteer.Location = new System.Drawing.Point(246, 389);
            this.lblFromToolSteer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromToolSteer.Name = "lblFromToolSteer";
            this.lblFromToolSteer.Size = new System.Drawing.Size(64, 27);
            this.lblFromToolSteer.TabIndex = 181;
            this.lblFromToolSteer.Text = "---";
            this.lblFromToolSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCommSetModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(772, 440);
            this.ControlBox = false;
            this.Controls.Add(this.lblToolSteer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFromToolSteer);
            this.Controls.Add(this.lblGPS2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblIMU);
            this.Controls.Add(this.lblSteer);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.lblGPS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSerialOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblFromGPS2);
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblFromMU);
            this.Controls.Add(this.lblFromModule1);
            this.Controls.Add(this.lblFromModule2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormCommSetModules";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect GPS";
            this.Load += new System.EventHandler(this.FormCommSet_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboxModule3Port;
        private System.Windows.Forms.Button btnOpenSerialModule3;
        private System.Windows.Forms.Button btnCloseSerialModule3;
        private System.Windows.Forms.Label lblCurrentModule3Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblIMU;
        private System.Windows.Forms.Label lblSteer;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.Label lblFromMU;
        private System.Windows.Forms.Label lblFromModule1;
        private System.Windows.Forms.Label lblFromModule2;
        private System.Windows.Forms.Label lblFromGPS2;
        private System.Windows.Forms.Label lblGPS2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnOpenGPS2;
        private System.Windows.Forms.Button btnCloseGPS2;
        private System.Windows.Forms.ComboBox cboxBaud2;
        private System.Windows.Forms.Label lblCurrentBaud2;
        private System.Windows.Forms.ComboBox cboxPort2;
        private System.Windows.Forms.Label lblCurrentPort2;
        private System.Windows.Forms.TextBox textBoxRcv2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblToolSteer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFromToolSteer;
    }
}