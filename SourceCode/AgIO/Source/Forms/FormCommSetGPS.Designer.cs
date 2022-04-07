namespace AgIO
{
    partial class FormCommSetGPS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCommSetGPS));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCloseRTCM = new System.Windows.Forms.Button();
            this.btnOpenRTCM = new System.Windows.Forms.Button();
            this.labelDifferentRtcmPort = new System.Windows.Forms.Label();
            this.cboxRtcmPort = new System.Windows.Forms.ComboBox();
            this.cboxRtcmBaud = new System.Windows.Forms.ComboBox();
            this.labelRtcmPort = new System.Windows.Forms.Label();
            this.labelRtcmBaud = new System.Windows.Forms.Label();
            this.cboxPort = new System.Windows.Forms.ComboBox();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurrentBaud = new System.Windows.Forms.Label();
            this.btnCloseSerial = new System.Windows.Forms.Button();
            this.textBoxRcv = new System.Windows.Forms.TextBox();
            this.btnOpenSerial = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGPS = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCloseRTCM);
            this.groupBox1.Controls.Add(this.btnOpenRTCM);
            this.groupBox1.Controls.Add(this.labelDifferentRtcmPort);
            this.groupBox1.Controls.Add(this.cboxRtcmPort);
            this.groupBox1.Controls.Add(this.cboxRtcmBaud);
            this.groupBox1.Controls.Add(this.labelRtcmPort);
            this.groupBox1.Controls.Add(this.labelRtcmBaud);
            this.groupBox1.Controls.Add(this.cboxPort);
            this.groupBox1.Controls.Add(this.cboxBaud);
            this.groupBox1.Controls.Add(this.lblCurrentPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblCurrentBaud);
            this.groupBox1.Controls.Add(this.btnCloseSerial);
            this.groupBox1.Controls.Add(this.textBoxRcv);
            this.groupBox1.Controls.Add(this.btnOpenSerial);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(173, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(740, 273);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(154, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 29);
            this.label3.TabIndex = 97;
            this.label3.Text = "RTCM";
            // 
            // btnCloseRTCM
            // 
            this.btnCloseRTCM.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseRTCM.FlatAppearance.BorderSize = 0;
            this.btnCloseRTCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseRTCM.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseRTCM.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseRTCM.Image")));
            this.btnCloseRTCM.Location = new System.Drawing.Point(673, 112);
            this.btnCloseRTCM.Name = "btnCloseRTCM";
            this.btnCloseRTCM.Size = new System.Drawing.Size(56, 58);
            this.btnCloseRTCM.TabIndex = 95;
            this.btnCloseRTCM.UseVisualStyleBackColor = false;
            this.btnCloseRTCM.Click += new System.EventHandler(this.btnCloseRTCM_Click);
            // 
            // btnOpenRTCM
            // 
            this.btnOpenRTCM.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenRTCM.FlatAppearance.BorderSize = 0;
            this.btnOpenRTCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenRTCM.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenRTCM.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenRTCM.Image")));
            this.btnOpenRTCM.Location = new System.Drawing.Point(572, 112);
            this.btnOpenRTCM.Name = "btnOpenRTCM";
            this.btnOpenRTCM.Size = new System.Drawing.Size(58, 58);
            this.btnOpenRTCM.TabIndex = 96;
            this.btnOpenRTCM.UseVisualStyleBackColor = false;
            this.btnOpenRTCM.Click += new System.EventHandler(this.btnOpenRTCM_Click);
            // 
            // labelDifferentRtcmPort
            // 
            this.labelDifferentRtcmPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifferentRtcmPort.Location = new System.Drawing.Point(22, 127);
            this.labelDifferentRtcmPort.Name = "labelDifferentRtcmPort";
            this.labelDifferentRtcmPort.Size = new System.Drawing.Size(128, 43);
            this.labelDifferentRtcmPort.TabIndex = 94;
            this.labelDifferentRtcmPort.Text = "Different Port Then GPS ?";
            this.labelDifferentRtcmPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxRtcmPort
            // 
            this.cboxRtcmPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxRtcmPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRtcmPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRtcmPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxRtcmPort.FormattingEnabled = true;
            this.cboxRtcmPort.Location = new System.Drawing.Point(244, 127);
            this.cboxRtcmPort.Name = "cboxRtcmPort";
            this.cboxRtcmPort.Size = new System.Drawing.Size(124, 37);
            this.cboxRtcmPort.TabIndex = 54;
            this.cboxRtcmPort.SelectedIndexChanged += new System.EventHandler(this.cboxRtcmPort_SelectedIndexChanged);
            // 
            // cboxRtcmBaud
            // 
            this.cboxRtcmBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxRtcmBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRtcmBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRtcmBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxRtcmBaud.FormattingEnabled = true;
            this.cboxRtcmBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000",
            "256000"});
            this.cboxRtcmBaud.Location = new System.Drawing.Point(395, 127);
            this.cboxRtcmBaud.Name = "cboxRtcmBaud";
            this.cboxRtcmBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxRtcmBaud.TabIndex = 53;
            this.cboxRtcmBaud.SelectedIndexChanged += new System.EventHandler(this.cboxRtcmBaud_SelectedIndexChanged);
            // 
            // labelRtcmPort
            // 
            this.labelRtcmPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRtcmPort.Location = new System.Drawing.Point(244, 106);
            this.labelRtcmPort.Name = "labelRtcmPort";
            this.labelRtcmPort.Size = new System.Drawing.Size(121, 18);
            this.labelRtcmPort.TabIndex = 52;
            this.labelRtcmPort.Text = "RTCM Port";
            this.labelRtcmPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRtcmBaud
            // 
            this.labelRtcmBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRtcmBaud.Location = new System.Drawing.Point(395, 106);
            this.labelRtcmBaud.Name = "labelRtcmBaud";
            this.labelRtcmBaud.Size = new System.Drawing.Size(124, 18);
            this.labelRtcmBaud.TabIndex = 51;
            this.labelRtcmBaud.Text = "RTCM Baud";
            this.labelRtcmBaud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxPort
            // 
            this.cboxPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxPort.FormattingEnabled = true;
            this.cboxPort.Location = new System.Drawing.Point(244, 37);
            this.cboxPort.Name = "cboxPort";
            this.cboxPort.Size = new System.Drawing.Size(124, 37);
            this.cboxPort.TabIndex = 50;
            this.cboxPort.SelectedIndexChanged += new System.EventHandler(this.cboxPort_SelectedIndexChanged_1);
            // 
            // cboxBaud
            // 
            this.cboxBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud.Location = new System.Drawing.Point(395, 37);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud.TabIndex = 49;
            this.cboxBaud.SelectedIndexChanged += new System.EventHandler(this.cboxBaud_SelectedIndexChanged_1);
            // 
            // lblCurrentPort
            // 
            this.lblCurrentPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort.Location = new System.Drawing.Point(244, 16);
            this.lblCurrentPort.Name = "lblCurrentPort";
            this.lblCurrentPort.Size = new System.Drawing.Size(121, 18);
            this.lblCurrentPort.TabIndex = 47;
            this.lblCurrentPort.Text = "GPS Port";
            this.lblCurrentPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(174, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 29);
            this.label2.TabIndex = 74;
            this.label2.Text = "GPS";
            // 
            // lblCurrentBaud
            // 
            this.lblCurrentBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud.Location = new System.Drawing.Point(395, 16);
            this.lblCurrentBaud.Name = "lblCurrentBaud";
            this.lblCurrentBaud.Size = new System.Drawing.Size(124, 18);
            this.lblCurrentBaud.TabIndex = 46;
            this.lblCurrentBaud.Text = "GPS Baud";
            this.lblCurrentBaud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCloseSerial
            // 
            this.btnCloseSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerial.FlatAppearance.BorderSize = 0;
            this.btnCloseSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerial.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseSerial.Image")));
            this.btnCloseSerial.Location = new System.Drawing.Point(673, 23);
            this.btnCloseSerial.Name = "btnCloseSerial";
            this.btnCloseSerial.Size = new System.Drawing.Size(56, 58);
            this.btnCloseSerial.TabIndex = 44;
            this.btnCloseSerial.UseVisualStyleBackColor = false;
            this.btnCloseSerial.Click += new System.EventHandler(this.btnCloseSerial_Click);
            // 
            // textBoxRcv
            // 
            this.textBoxRcv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxRcv.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxRcv.Location = new System.Drawing.Point(12, 185);
            this.textBoxRcv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv.Multiline = true;
            this.textBoxRcv.Name = "textBoxRcv";
            this.textBoxRcv.ReadOnly = true;
            this.textBoxRcv.Size = new System.Drawing.Size(718, 78);
            this.textBoxRcv.TabIndex = 40;
            // 
            // btnOpenSerial
            // 
            this.btnOpenSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerial.FlatAppearance.BorderSize = 0;
            this.btnOpenSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerial.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenSerial.Image")));
            this.btnOpenSerial.Location = new System.Drawing.Point(572, 23);
            this.btnOpenSerial.Name = "btnOpenSerial";
            this.btnOpenSerial.Size = new System.Drawing.Size(58, 58);
            this.btnOpenSerial.TabIndex = 45;
            this.btnOpenSerial.UseVisualStyleBackColor = false;
            this.btnOpenSerial.Click += new System.EventHandler(this.btnOpenSerial_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AgIO.Properties.Resources.satellite;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(27, 49);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(120, 120);
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
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
            this.btnRescan.Location = new System.Drawing.Point(650, 323);
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
            this.btnSerialOK.Location = new System.Drawing.Point(811, 322);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(91, 63);
            this.btnSerialOK.TabIndex = 59;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = false;
            this.btnSerialOK.Click += new System.EventHandler(this.btnSerialOK_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(303, 343);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 73;
            this.label1.Text = "GPS:";
            // 
            // lblGPS
            // 
            this.lblGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGPS.AutoSize = true;
            this.lblGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS.Location = new System.Drawing.Point(357, 343);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new System.Drawing.Size(48, 23);
            this.lblGPS.TabIndex = 79;
            this.lblGPS.Text = "GPS";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS.Location = new System.Drawing.Point(436, 341);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS.TabIndex = 172;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCommSetGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(927, 397);
            this.ControlBox = false;
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblGPS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSerialOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormCommSetGPS";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect GPS";
            this.Load += new System.EventHandler(this.FormCommSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrentPort;
        private System.Windows.Forms.Label lblCurrentBaud;
        private System.Windows.Forms.Button btnCloseSerial;
        private System.Windows.Forms.TextBox textBoxRcv;
        private System.Windows.Forms.Button btnOpenSerial;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.ComboBox cboxPort;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.ComboBox cboxRtcmPort;
        private System.Windows.Forms.ComboBox cboxRtcmBaud;
        private System.Windows.Forms.Label labelRtcmPort;
        private System.Windows.Forms.Label labelRtcmBaud;
        private System.Windows.Forms.Label labelDifferentRtcmPort;
        private System.Windows.Forms.Button btnCloseRTCM;
        private System.Windows.Forms.Button btnOpenRTCM;
        private System.Windows.Forms.Label label3;
    }
}