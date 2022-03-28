namespace AgOpenGPS
{
    partial class ConfigRemoteSwitch
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
            this.chkRemoteSwitchEnable = new System.Windows.Forms.CheckBox();
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.chkSetAutoSections = new System.Windows.Forms.CheckBox();
            this.chkSetManualSections = new System.Windows.Forms.CheckBox();
            this.grpSwitch = new System.Windows.Forms.GroupBox();
            this.chkSelectSteerSwitch = new System.Windows.Forms.CheckBox();
            this.chkSelectWorkSwitch = new System.Windows.Forms.CheckBox();
            this.chkWorkSwActiveLow = new System.Windows.Forms.CheckBox();
            this.grpControls.SuspendLayout();
            this.grpSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkRemoteSwitchEnable
            // 
            this.chkRemoteSwitchEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRemoteSwitchEnable.AutoSize = true;
            this.chkRemoteSwitchEnable.BackColor = System.Drawing.Color.AliceBlue;
            this.chkRemoteSwitchEnable.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkRemoteSwitchEnable.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkRemoteSwitchEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRemoteSwitchEnable.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoteSwitchEnable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkRemoteSwitchEnable.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.chkRemoteSwitchEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkRemoteSwitchEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkRemoteSwitchEnable.Location = new System.Drawing.Point(241, 18);
            this.chkRemoteSwitchEnable.Name = "chkRemoteSwitchEnable";
            this.chkRemoteSwitchEnable.Size = new System.Drawing.Size(377, 70);
            this.chkRemoteSwitchEnable.TabIndex = 458;
            this.chkRemoteSwitchEnable.Text = "         Remote Section Control";
            this.chkRemoteSwitchEnable.UseVisualStyleBackColor = false;
            this.chkRemoteSwitchEnable.CheckedChanged += new System.EventHandler(this.chkRemoteSwitchEnable_CheckedChanged);
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.chkSetAutoSections);
            this.grpControls.Controls.Add(this.chkSetManualSections);
            this.grpControls.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpControls.Location = new System.Drawing.Point(57, 423);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(730, 139);
            this.grpControls.TabIndex = 460;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "Activates";
            // 
            // chkSetAutoSections
            // 
            this.chkSetAutoSections.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetAutoSections.AutoSize = true;
            this.chkSetAutoSections.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetAutoSections.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetAutoSections.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetAutoSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetAutoSections.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSetAutoSections.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSetAutoSections.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.chkSetAutoSections.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetAutoSections.Location = new System.Drawing.Point(367, 51);
            this.chkSetAutoSections.Name = "chkSetAutoSections";
            this.chkSetAutoSections.Size = new System.Drawing.Size(250, 70);
            this.chkSetAutoSections.TabIndex = 456;
            this.chkSetAutoSections.Text = "        Auto Sections";
            this.chkSetAutoSections.UseVisualStyleBackColor = false;
            this.chkSetAutoSections.Click += new System.EventHandler(this.chkSetAutoSections_Click);
            // 
            // chkSetManualSections
            // 
            this.chkSetManualSections.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetManualSections.AutoSize = true;
            this.chkSetManualSections.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetManualSections.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetManualSections.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetManualSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetManualSections.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSetManualSections.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSetManualSections.Image = global::AgOpenGPS.Properties.Resources.ManualOff;
            this.chkSetManualSections.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetManualSections.Location = new System.Drawing.Point(59, 51);
            this.chkSetManualSections.Name = "chkSetManualSections";
            this.chkSetManualSections.Size = new System.Drawing.Size(280, 70);
            this.chkSetManualSections.TabIndex = 0;
            this.chkSetManualSections.Text = "        Manual Sections";
            this.chkSetManualSections.UseVisualStyleBackColor = false;
            this.chkSetManualSections.Click += new System.EventHandler(this.chkSetManualSections_Click);
            // 
            // grpSwitch
            // 
            this.grpSwitch.BackColor = System.Drawing.Color.Transparent;
            this.grpSwitch.Controls.Add(this.chkSelectSteerSwitch);
            this.grpSwitch.Controls.Add(this.chkSelectWorkSwitch);
            this.grpSwitch.Controls.Add(this.chkWorkSwActiveLow);
            this.grpSwitch.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSwitch.ForeColor = System.Drawing.Color.Black;
            this.grpSwitch.Location = new System.Drawing.Point(57, 112);
            this.grpSwitch.Name = "grpSwitch";
            this.grpSwitch.Size = new System.Drawing.Size(730, 279);
            this.grpSwitch.TabIndex = 459;
            this.grpSwitch.TabStop = false;
            this.grpSwitch.Text = "Work or Steer Switch";
            // 
            // chkSelectSteerSwitch
            // 
            this.chkSelectSteerSwitch.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectSteerSwitch.AutoSize = true;
            this.chkSelectSteerSwitch.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSelectSteerSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSelectSteerSwitch.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSelectSteerSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectSteerSwitch.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectSteerSwitch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSelectSteerSwitch.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.chkSelectSteerSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectSteerSwitch.Location = new System.Drawing.Point(439, 70);
            this.chkSelectSteerSwitch.Name = "chkSelectSteerSwitch";
            this.chkSelectSteerSwitch.Size = new System.Drawing.Size(236, 70);
            this.chkSelectSteerSwitch.TabIndex = 2;
            this.chkSelectSteerSwitch.Text = "        Steer Switch";
            this.chkSelectSteerSwitch.UseVisualStyleBackColor = false;
            this.chkSelectSteerSwitch.Click += new System.EventHandler(this.chkSelectSteerSwitch_Click);
            // 
            // chkSelectWorkSwitch
            // 
            this.chkSelectWorkSwitch.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectWorkSwitch.AutoSize = true;
            this.chkSelectWorkSwitch.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSelectWorkSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSelectWorkSwitch.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSelectWorkSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectWorkSwitch.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectWorkSwitch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSelectWorkSwitch.Image = global::AgOpenGPS.Properties.Resources.HydraulicLiftOff;
            this.chkSelectWorkSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectWorkSwitch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSelectWorkSwitch.Location = new System.Drawing.Point(59, 70);
            this.chkSelectWorkSwitch.Name = "chkSelectWorkSwitch";
            this.chkSelectWorkSwitch.Size = new System.Drawing.Size(236, 70);
            this.chkSelectWorkSwitch.TabIndex = 1;
            this.chkSelectWorkSwitch.Text = "        Work Switch";
            this.chkSelectWorkSwitch.UseVisualStyleBackColor = false;
            this.chkSelectWorkSwitch.Click += new System.EventHandler(this.chkSelectWorkSwitch_Click);
            // 
            // chkWorkSwActiveLow
            // 
            this.chkWorkSwActiveLow.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWorkSwActiveLow.BackColor = System.Drawing.Color.AliceBlue;
            this.chkWorkSwActiveLow.Checked = true;
            this.chkWorkSwActiveLow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWorkSwActiveLow.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkWorkSwActiveLow.FlatAppearance.CheckedBackColor = System.Drawing.Color.AliceBlue;
            this.chkWorkSwActiveLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWorkSwActiveLow.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWorkSwActiveLow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWorkSwActiveLow.Image = global::AgOpenGPS.Properties.Resources.SwitchActiveClosed;
            this.chkWorkSwActiveLow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkWorkSwActiveLow.Location = new System.Drawing.Point(59, 163);
            this.chkWorkSwActiveLow.Name = "chkWorkSwActiveLow";
            this.chkWorkSwActiveLow.Size = new System.Drawing.Size(236, 76);
            this.chkWorkSwActiveLow.TabIndex = 0;
            this.chkWorkSwActiveLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWorkSwActiveLow.UseVisualStyleBackColor = false;
            this.chkWorkSwActiveLow.Click += new System.EventHandler(this.chkWorkSwActiveLow_Click);
            // 
            // ConfigRemoteSwitch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.chkRemoteSwitchEnable);
            this.Controls.Add(this.grpControls);
            this.Controls.Add(this.grpSwitch);
            this.Name = "ConfigRemoteSwitch";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigRemoteSwitch_Load);
            this.grpControls.ResumeLayout(false);
            this.grpControls.PerformLayout();
            this.grpSwitch.ResumeLayout(false);
            this.grpSwitch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkRemoteSwitchEnable;
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.CheckBox chkSetAutoSections;
        private System.Windows.Forms.CheckBox chkSetManualSections;
        private System.Windows.Forms.GroupBox grpSwitch;
        private System.Windows.Forms.CheckBox chkSelectSteerSwitch;
        private System.Windows.Forms.CheckBox chkSelectWorkSwitch;
        private System.Windows.Forms.CheckBox chkWorkSwActiveLow;
    }
}
