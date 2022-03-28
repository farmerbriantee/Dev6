namespace AgOpenGPS
{
    partial class ConfigHeading
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label12 = new System.Windows.Forms.Label();
            this.nudDualHeadingOffset = new System.Windows.Forms.Button();
            this.label118 = new System.Windows.Forms.Label();
            this.cboxIsRTK_KillAutoSteer = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudAgeAlarm = new System.Windows.Forms.Button();
            this.label104 = new System.Windows.Forms.Label();
            this.nudMinimumFrameTime = new System.Windows.Forms.Button();
            this.cboxIsRTK = new System.Windows.Forms.CheckBox();
            this.gboxSingle = new System.Windows.Forms.GroupBox();
            this.lblFusionIMU = new System.Windows.Forms.Label();
            this.lblIMU = new System.Windows.Forms.Label();
            this.lblGPS = new System.Windows.Forms.Label();
            this.hsbarFusion = new System.Windows.Forms.HScrollBar();
            this.lblFusion = new System.Windows.Forms.Label();
            this.lblIMUFusion = new System.Windows.Forms.Label();
            this.label117 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudForwardComp = new System.Windows.Forms.Button();
            this.cboxIsDualAsIMU = new System.Windows.Forms.CheckBox();
            this.nudReverseComp = new System.Windows.Forms.Button();
            this.cboxIsReverseOn = new System.Windows.Forms.CheckBox();
            this.nudStartSpeed = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudMinFixStepDistance = new System.Windows.Forms.Button();
            this.headingGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.rbtnHeadingHDT = new System.Windows.Forms.RadioButton();
            this.rbtnHeadingGPS = new System.Windows.Forms.RadioButton();
            this.rbtnHeadingFix = new System.Windows.Forms.RadioButton();
            this.gboxSingle.SuspendLayout();
            this.headingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(16, 290);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(178, 56);
            this.label12.TabIndex = 480;
            this.label12.Text = "Dual Heading Offset (Degree)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudDualHeadingOffset
            // 
            this.nudDualHeadingOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudDualHeadingOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudDualHeadingOffset.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDualHeadingOffset.Location = new System.Drawing.Point(200, 290);
            this.nudDualHeadingOffset.Name = "nudDualHeadingOffset";
            this.nudDualHeadingOffset.Size = new System.Drawing.Size(141, 52);
            this.nudDualHeadingOffset.TabIndex = 481;
            this.nudDualHeadingOffset.Text = "-99.9";
            this.nudDualHeadingOffset.UseVisualStyleBackColor = false;
            this.nudDualHeadingOffset.Click += new System.EventHandler(this.nudDualHeadingOffset_Click);
            // 
            // label118
            // 
            this.label118.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label118.AutoSize = true;
            this.label118.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label118.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label118.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label118.Location = new System.Drawing.Point(174, 518);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(38, 25);
            this.label118.TabIndex = 479;
            this.label118.Text = "->";
            this.label118.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboxIsRTK_KillAutoSteer
            // 
            this.cboxIsRTK_KillAutoSteer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxIsRTK_KillAutoSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsRTK_KillAutoSteer.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxIsRTK_KillAutoSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsRTK_KillAutoSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.cboxIsRTK_KillAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsRTK_KillAutoSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsRTK_KillAutoSteer.ForeColor = System.Drawing.Color.Black;
            this.cboxIsRTK_KillAutoSteer.Location = new System.Drawing.Point(222, 503);
            this.cboxIsRTK_KillAutoSteer.Name = "cboxIsRTK_KillAutoSteer";
            this.cboxIsRTK_KillAutoSteer.Size = new System.Drawing.Size(124, 65);
            this.cboxIsRTK_KillAutoSteer.TabIndex = 478;
            this.cboxIsRTK_KillAutoSteer.Text = "Kill Auto Steer";
            this.cboxIsRTK_KillAutoSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsRTK_KillAutoSteer.UseVisualStyleBackColor = false;
            this.cboxIsRTK_KillAutoSteer.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxIsRTK_KillAutoSteer_HelpRequested);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(190, 355);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 56);
            this.label2.TabIndex = 477;
            this.label2.Text = "Differential Age Alarm (secs)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAgeAlarm
            // 
            this.nudAgeAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudAgeAlarm.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAgeAlarm.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAgeAlarm.Location = new System.Drawing.Point(222, 413);
            this.nudAgeAlarm.Name = "nudAgeAlarm";
            this.nudAgeAlarm.Size = new System.Drawing.Size(124, 52);
            this.nudAgeAlarm.TabIndex = 476;
            this.nudAgeAlarm.Text = "20";
            this.nudAgeAlarm.UseVisualStyleBackColor = false;
            this.nudAgeAlarm.Click += new System.EventHandler(this.nudAgeAlarm_Click);
            this.nudAgeAlarm.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudAgeAlarm_HelpRequested);
            // 
            // label104
            // 
            this.label104.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label104.BackColor = System.Drawing.Color.Transparent;
            this.label104.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label104.ForeColor = System.Drawing.Color.Black;
            this.label104.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label104.Location = new System.Drawing.Point(6, 355);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(178, 56);
            this.label104.TabIndex = 474;
            this.label104.Text = "Minimum Frame Pause (msec)";
            this.label104.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudMinimumFrameTime
            // 
            this.nudMinimumFrameTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudMinimumFrameTime.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMinimumFrameTime.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinimumFrameTime.Location = new System.Drawing.Point(40, 413);
            this.nudMinimumFrameTime.Name = "nudMinimumFrameTime";
            this.nudMinimumFrameTime.Size = new System.Drawing.Size(125, 52);
            this.nudMinimumFrameTime.TabIndex = 475;
            this.nudMinimumFrameTime.Text = "70";
            this.nudMinimumFrameTime.UseVisualStyleBackColor = false;
            this.nudMinimumFrameTime.Click += new System.EventHandler(this.nudMinimumFrameTime_Click);
            this.nudMinimumFrameTime.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudMinimumFrameTime_HelpRequested);
            // 
            // cboxIsRTK
            // 
            this.cboxIsRTK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxIsRTK.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsRTK.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxIsRTK.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsRTK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.cboxIsRTK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsRTK.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsRTK.ForeColor = System.Drawing.Color.Black;
            this.cboxIsRTK.Location = new System.Drawing.Point(41, 503);
            this.cboxIsRTK.Name = "cboxIsRTK";
            this.cboxIsRTK.Size = new System.Drawing.Size(124, 65);
            this.cboxIsRTK.TabIndex = 473;
            this.cboxIsRTK.Text = "RTK Alarm";
            this.cboxIsRTK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsRTK.UseVisualStyleBackColor = false;
            this.cboxIsRTK.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxIsRTK_HelpRequested);
            // 
            // gboxSingle
            // 
            this.gboxSingle.Controls.Add(this.lblFusionIMU);
            this.gboxSingle.Controls.Add(this.lblIMU);
            this.gboxSingle.Controls.Add(this.lblGPS);
            this.gboxSingle.Controls.Add(this.hsbarFusion);
            this.gboxSingle.Controls.Add(this.lblFusion);
            this.gboxSingle.Controls.Add(this.lblIMUFusion);
            this.gboxSingle.Controls.Add(this.label117);
            this.gboxSingle.Controls.Add(this.label116);
            this.gboxSingle.Controls.Add(this.label6);
            this.gboxSingle.Controls.Add(this.nudForwardComp);
            this.gboxSingle.Controls.Add(this.cboxIsDualAsIMU);
            this.gboxSingle.Controls.Add(this.nudReverseComp);
            this.gboxSingle.Controls.Add(this.cboxIsReverseOn);
            this.gboxSingle.Controls.Add(this.nudStartSpeed);
            this.gboxSingle.Controls.Add(this.label15);
            this.gboxSingle.Controls.Add(this.label9);
            this.gboxSingle.Controls.Add(this.label8);
            this.gboxSingle.Controls.Add(this.nudMinFixStepDistance);
            this.gboxSingle.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxSingle.Location = new System.Drawing.Point(400, 12);
            this.gboxSingle.Name = "gboxSingle";
            this.gboxSingle.Size = new System.Drawing.Size(446, 544);
            this.gboxSingle.TabIndex = 472;
            this.gboxSingle.TabStop = false;
            this.gboxSingle.Text = "Single Antenna Settings";
            // 
            // lblFusionIMU
            // 
            this.lblFusionIMU.AutoSize = true;
            this.lblFusionIMU.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFusionIMU.ForeColor = System.Drawing.Color.Black;
            this.lblFusionIMU.Location = new System.Drawing.Point(12, 479);
            this.lblFusionIMU.Name = "lblFusionIMU";
            this.lblFusionIMU.Size = new System.Drawing.Size(91, 39);
            this.lblFusionIMU.TabIndex = 476;
            this.lblFusionIMU.Text = "-888";
            // 
            // lblIMU
            // 
            this.lblIMU.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMU.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblIMU.Location = new System.Drawing.Point(59, 442);
            this.lblIMU.Name = "lblIMU";
            this.lblIMU.Size = new System.Drawing.Size(76, 23);
            this.lblIMU.TabIndex = 475;
            this.lblIMU.Text = "IMU <";
            this.lblIMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGPS
            // 
            this.lblGPS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS.ForeColor = System.Drawing.Color.Red;
            this.lblGPS.Location = new System.Drawing.Point(319, 442);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new System.Drawing.Size(69, 23);
            this.lblGPS.TabIndex = 474;
            this.lblGPS.Text = "> GPS";
            this.lblGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarFusion
            // 
            this.hsbarFusion.LargeChange = 1;
            this.hsbarFusion.Location = new System.Drawing.Point(109, 472);
            this.hsbarFusion.Minimum = 1;
            this.hsbarFusion.Name = "hsbarFusion";
            this.hsbarFusion.Size = new System.Drawing.Size(235, 58);
            this.hsbarFusion.TabIndex = 471;
            this.hsbarFusion.Value = 25;
            this.hsbarFusion.ValueChanged += new System.EventHandler(this.hsbarFusion_ValueChanged);
            this.hsbarFusion.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.hsbarFusion_HelpRequested);
            // 
            // lblFusion
            // 
            this.lblFusion.AutoSize = true;
            this.lblFusion.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFusion.ForeColor = System.Drawing.Color.Black;
            this.lblFusion.Location = new System.Drawing.Point(349, 479);
            this.lblFusion.Name = "lblFusion";
            this.lblFusion.Size = new System.Drawing.Size(91, 39);
            this.lblFusion.TabIndex = 473;
            this.lblFusion.Text = "-888";
            // 
            // lblIMUFusion
            // 
            this.lblIMUFusion.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMUFusion.ForeColor = System.Drawing.Color.Black;
            this.lblIMUFusion.Location = new System.Drawing.Point(170, 424);
            this.lblIMUFusion.Name = "lblIMUFusion";
            this.lblIMUFusion.Size = new System.Drawing.Size(109, 48);
            this.lblIMUFusion.TabIndex = 472;
            this.lblIMUFusion.Text = "IMU GPS Fusion";
            this.lblIMUFusion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label117
            // 
            this.label117.AutoSize = true;
            this.label117.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label117.ForeColor = System.Drawing.Color.Black;
            this.label117.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label117.Location = new System.Drawing.Point(105, 252);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(255, 23);
            this.label117.TabIndex = 470;
            this.label117.Text = "IMU/GPS Turn Compensation";
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label116.ForeColor = System.Drawing.Color.Black;
            this.label116.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label116.Location = new System.Drawing.Point(298, 166);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(76, 23);
            this.label116.TabIndex = 469;
            this.label116.Text = "Reverse";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(58, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 23);
            this.label6.TabIndex = 468;
            this.label6.Text = "Forward";
            // 
            // nudForwardComp
            // 
            this.nudForwardComp.BackColor = System.Drawing.Color.AliceBlue;
            this.nudForwardComp.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudForwardComp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudForwardComp.Location = new System.Drawing.Point(35, 192);
            this.nudForwardComp.Name = "nudForwardComp";
            this.nudForwardComp.Size = new System.Drawing.Size(142, 52);
            this.nudForwardComp.TabIndex = 467;
            this.nudForwardComp.Text = "0.2";
            this.nudForwardComp.UseVisualStyleBackColor = false;
            this.nudForwardComp.Click += new System.EventHandler(this.nudForwardComp_Click);
            this.nudForwardComp.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudForwardComp_HelpRequested);
            // 
            // cboxIsDualAsIMU
            // 
            this.cboxIsDualAsIMU.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsDualAsIMU.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxIsDualAsIMU.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsDualAsIMU.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.cboxIsDualAsIMU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsDualAsIMU.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsDualAsIMU.ForeColor = System.Drawing.Color.Black;
            this.cboxIsDualAsIMU.Location = new System.Drawing.Point(253, 328);
            this.cboxIsDualAsIMU.Name = "cboxIsDualAsIMU";
            this.cboxIsDualAsIMU.Size = new System.Drawing.Size(161, 65);
            this.cboxIsDualAsIMU.TabIndex = 310;
            this.cboxIsDualAsIMU.Text = "Dual As IMU";
            this.cboxIsDualAsIMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsDualAsIMU.UseVisualStyleBackColor = false;
            this.cboxIsDualAsIMU.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxIsDualAsIMU_HelpRequested);
            // 
            // nudReverseComp
            // 
            this.nudReverseComp.BackColor = System.Drawing.Color.AliceBlue;
            this.nudReverseComp.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudReverseComp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudReverseComp.Location = new System.Drawing.Point(272, 192);
            this.nudReverseComp.Name = "nudReverseComp";
            this.nudReverseComp.Size = new System.Drawing.Size(142, 52);
            this.nudReverseComp.TabIndex = 466;
            this.nudReverseComp.Text = "0.5";
            this.nudReverseComp.UseVisualStyleBackColor = false;
            this.nudReverseComp.Click += new System.EventHandler(this.nudReverseComp_Click);
            this.nudReverseComp.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudReverseComp_HelpRequested);
            // 
            // cboxIsReverseOn
            // 
            this.cboxIsReverseOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsReverseOn.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxIsReverseOn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsReverseOn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.cboxIsReverseOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsReverseOn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsReverseOn.ForeColor = System.Drawing.Color.Black;
            this.cboxIsReverseOn.Location = new System.Drawing.Point(35, 327);
            this.cboxIsReverseOn.Name = "cboxIsReverseOn";
            this.cboxIsReverseOn.Size = new System.Drawing.Size(161, 66);
            this.cboxIsReverseOn.TabIndex = 465;
            this.cboxIsReverseOn.Text = "Reverse Detection";
            this.cboxIsReverseOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsReverseOn.UseVisualStyleBackColor = false;
            this.cboxIsReverseOn.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxIsReverseOn_HelpRequested);
            // 
            // nudStartSpeed
            // 
            this.nudStartSpeed.BackColor = System.Drawing.Color.AliceBlue;
            this.nudStartSpeed.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudStartSpeed.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudStartSpeed.Location = new System.Drawing.Point(35, 79);
            this.nudStartSpeed.Name = "nudStartSpeed";
            this.nudStartSpeed.Size = new System.Drawing.Size(142, 52);
            this.nudStartSpeed.TabIndex = 3;
            this.nudStartSpeed.Text = "0.5";
            this.nudStartSpeed.UseVisualStyleBackColor = false;
            this.nudStartSpeed.Click += new System.EventHandler(this.nudStartSpeed_Click);
            this.nudStartSpeed.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudStartSpeed_HelpRequested);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(293, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 307;
            this.label15.Text = "Fix Trigger";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(44, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 23);
            this.label9.TabIndex = 306;
            this.label9.Text = "Start Speed (kmh)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(283, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 23);
            this.label8.TabIndex = 305;
            this.label8.Text = "Distance (m)";
            // 
            // nudMinFixStepDistance
            // 
            this.nudMinFixStepDistance.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMinFixStepDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinFixStepDistance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudMinFixStepDistance.Location = new System.Drawing.Point(272, 79);
            this.nudMinFixStepDistance.Name = "nudMinFixStepDistance";
            this.nudMinFixStepDistance.Size = new System.Drawing.Size(142, 52);
            this.nudMinFixStepDistance.TabIndex = 2;
            this.nudMinFixStepDistance.Text = "1.0";
            this.nudMinFixStepDistance.UseVisualStyleBackColor = false;
            this.nudMinFixStepDistance.Click += new System.EventHandler(this.nudMinFixStepDistance_Click);
            this.nudMinFixStepDistance.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudMinFixStepDistance_HelpRequested);
            // 
            // headingGroupBox
            // 
            this.headingGroupBox.Controls.Add(this.pictureBox13);
            this.headingGroupBox.Controls.Add(this.rbtnHeadingHDT);
            this.headingGroupBox.Controls.Add(this.rbtnHeadingGPS);
            this.headingGroupBox.Controls.Add(this.rbtnHeadingFix);
            this.headingGroupBox.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingGroupBox.ForeColor = System.Drawing.Color.Black;
            this.headingGroupBox.Location = new System.Drawing.Point(34, 12);
            this.headingGroupBox.Name = "headingGroupBox";
            this.headingGroupBox.Size = new System.Drawing.Size(329, 275);
            this.headingGroupBox.TabIndex = 471;
            this.headingGroupBox.TabStop = false;
            this.headingGroupBox.Text = "Antenna Heading Type";
            // 
            // pictureBox13
            // 
            this.pictureBox13.Image = global::AgOpenGPS.Properties.Resources.Con_SourcesGPS;
            this.pictureBox13.Location = new System.Drawing.Point(191, 48);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(110, 204);
            this.pictureBox13.TabIndex = 3;
            this.pictureBox13.TabStop = false;
            // 
            // rbtnHeadingHDT
            // 
            this.rbtnHeadingHDT.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnHeadingHDT.BackColor = System.Drawing.Color.AliceBlue;
            this.rbtnHeadingHDT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rbtnHeadingHDT.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.rbtnHeadingHDT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnHeadingHDT.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnHeadingHDT.ForeColor = System.Drawing.Color.Black;
            this.rbtnHeadingHDT.Location = new System.Drawing.Point(24, 206);
            this.rbtnHeadingHDT.Name = "rbtnHeadingHDT";
            this.rbtnHeadingHDT.Size = new System.Drawing.Size(117, 43);
            this.rbtnHeadingHDT.TabIndex = 2;
            this.rbtnHeadingHDT.Text = "Dual";
            this.rbtnHeadingHDT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnHeadingHDT.UseVisualStyleBackColor = false;
            this.rbtnHeadingHDT.Click += new System.EventHandler(this.rbtnHeadingFix_Click);
            this.rbtnHeadingHDT.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.rbtnHeadingHDT_HelpRequested);
            // 
            // rbtnHeadingGPS
            // 
            this.rbtnHeadingGPS.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnHeadingGPS.BackColor = System.Drawing.Color.AliceBlue;
            this.rbtnHeadingGPS.Checked = true;
            this.rbtnHeadingGPS.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rbtnHeadingGPS.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.rbtnHeadingGPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnHeadingGPS.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnHeadingGPS.ForeColor = System.Drawing.Color.Black;
            this.rbtnHeadingGPS.Location = new System.Drawing.Point(24, 106);
            this.rbtnHeadingGPS.Name = "rbtnHeadingGPS";
            this.rbtnHeadingGPS.Size = new System.Drawing.Size(117, 43);
            this.rbtnHeadingGPS.TabIndex = 1;
            this.rbtnHeadingGPS.TabStop = true;
            this.rbtnHeadingGPS.Text = "VTG";
            this.rbtnHeadingGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnHeadingGPS.UseVisualStyleBackColor = false;
            this.rbtnHeadingGPS.Click += new System.EventHandler(this.rbtnHeadingFix_Click);
            this.rbtnHeadingGPS.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.rbtnHeadingGPS_HelpRequested);
            // 
            // rbtnHeadingFix
            // 
            this.rbtnHeadingFix.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnHeadingFix.BackColor = System.Drawing.Color.AliceBlue;
            this.rbtnHeadingFix.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rbtnHeadingFix.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.rbtnHeadingFix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnHeadingFix.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnHeadingFix.ForeColor = System.Drawing.Color.Black;
            this.rbtnHeadingFix.Location = new System.Drawing.Point(23, 42);
            this.rbtnHeadingFix.Name = "rbtnHeadingFix";
            this.rbtnHeadingFix.Size = new System.Drawing.Size(117, 43);
            this.rbtnHeadingFix.TabIndex = 0;
            this.rbtnHeadingFix.Text = "Fix";
            this.rbtnHeadingFix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnHeadingFix.UseVisualStyleBackColor = false;
            this.rbtnHeadingFix.Click += new System.EventHandler(this.rbtnHeadingFix_Click);
            this.rbtnHeadingFix.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.rbtnHeadingFix_HelpRequested);
            // 
            // ConfigHeading
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.nudDualHeadingOffset);
            this.Controls.Add(this.label118);
            this.Controls.Add(this.cboxIsRTK_KillAutoSteer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudAgeAlarm);
            this.Controls.Add(this.label104);
            this.Controls.Add(this.nudMinimumFrameTime);
            this.Controls.Add(this.cboxIsRTK);
            this.Controls.Add(this.gboxSingle);
            this.Controls.Add(this.headingGroupBox);
            this.Name = "ConfigHeading";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigHeading_Load);
            this.gboxSingle.ResumeLayout(false);
            this.gboxSingle.PerformLayout();
            this.headingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button nudDualHeadingOffset;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.CheckBox cboxIsRTK_KillAutoSteer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button nudAgeAlarm;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.Button nudMinimumFrameTime;
        private System.Windows.Forms.CheckBox cboxIsRTK;
        private System.Windows.Forms.GroupBox gboxSingle;
        private System.Windows.Forms.Label lblFusionIMU;
        private System.Windows.Forms.Label lblIMU;
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.HScrollBar hsbarFusion;
        private System.Windows.Forms.Label lblFusion;
        private System.Windows.Forms.Label lblIMUFusion;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button nudForwardComp;
        private System.Windows.Forms.CheckBox cboxIsDualAsIMU;
        private System.Windows.Forms.Button nudReverseComp;
        private System.Windows.Forms.CheckBox cboxIsReverseOn;
        private System.Windows.Forms.Button nudStartSpeed;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button nudMinFixStepDistance;
        private System.Windows.Forms.GroupBox headingGroupBox;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.RadioButton rbtnHeadingHDT;
        private System.Windows.Forms.RadioButton rbtnHeadingGPS;
        private System.Windows.Forms.RadioButton rbtnHeadingFix;
    }
}
