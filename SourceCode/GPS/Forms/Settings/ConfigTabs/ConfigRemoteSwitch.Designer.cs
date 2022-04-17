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
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.chkSetAutoSectionsSteer = new System.Windows.Forms.CheckBox();
            this.chkSelectSteerSwitch = new System.Windows.Forms.CheckBox();
            this.chkSetManualSectionsSteer = new System.Windows.Forms.CheckBox();
            this.chkSetAutoSections = new System.Windows.Forms.CheckBox();
            this.chkSetManualSections = new System.Windows.Forms.CheckBox();
            this.grpSwitch = new System.Windows.Forms.GroupBox();
            this.chkSelectWorkSwitch = new System.Windows.Forms.CheckBox();
            this.chkWorkSwActiveLow = new System.Windows.Forms.CheckBox();
            this.cboxAutoSteerAuto = new System.Windows.Forms.CheckBox();
            this.grpControls.SuspendLayout();
            this.grpSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.cboxAutoSteerAuto);
            this.grpControls.Controls.Add(this.chkSetAutoSectionsSteer);
            this.grpControls.Controls.Add(this.chkSelectSteerSwitch);
            this.grpControls.Controls.Add(this.chkSetManualSectionsSteer);
            this.grpControls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpControls.Location = new System.Drawing.Point(470, 100);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(320, 420);
            this.grpControls.TabIndex = 460;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "SteerSwitch";
            // 
            // chkSetAutoSectionsSteer
            // 
            this.chkSetAutoSectionsSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetAutoSectionsSteer.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetAutoSectionsSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetAutoSectionsSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetAutoSectionsSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetAutoSectionsSteer.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.chkSetAutoSectionsSteer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetAutoSectionsSteer.Location = new System.Drawing.Point(10, 240);
            this.chkSetAutoSectionsSteer.Name = "chkSetAutoSectionsSteer";
            this.chkSetAutoSectionsSteer.Size = new System.Drawing.Size(300, 70);
            this.chkSetAutoSectionsSteer.TabIndex = 462;
            this.chkSetAutoSectionsSteer.Text = "        Auto Sections";
            this.chkSetAutoSectionsSteer.UseVisualStyleBackColor = false;
            this.chkSetAutoSectionsSteer.Click += new System.EventHandler(this.chkSetAutoSectionsSteer_Click);
            // 
            // chkSelectSteerSwitch
            // 
            this.chkSelectSteerSwitch.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectSteerSwitch.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSelectSteerSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSelectSteerSwitch.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSelectSteerSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectSteerSwitch.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.chkSelectSteerSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectSteerSwitch.Location = new System.Drawing.Point(10, 60);
            this.chkSelectSteerSwitch.Name = "chkSelectSteerSwitch";
            this.chkSelectSteerSwitch.Size = new System.Drawing.Size(300, 70);
            this.chkSelectSteerSwitch.TabIndex = 2;
            this.chkSelectSteerSwitch.Text = "        Steer Switch";
            this.chkSelectSteerSwitch.UseVisualStyleBackColor = false;
            this.chkSelectSteerSwitch.Click += new System.EventHandler(this.chkSelectSteerSwitch_Click);
            // 
            // chkSetManualSectionsSteer
            // 
            this.chkSetManualSectionsSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetManualSectionsSteer.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetManualSectionsSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetManualSectionsSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetManualSectionsSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetManualSectionsSteer.Image = global::AgOpenGPS.Properties.Resources.ManualOff;
            this.chkSetManualSectionsSteer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetManualSectionsSteer.Location = new System.Drawing.Point(10, 160);
            this.chkSetManualSectionsSteer.Name = "chkSetManualSectionsSteer";
            this.chkSetManualSectionsSteer.Size = new System.Drawing.Size(300, 70);
            this.chkSetManualSectionsSteer.TabIndex = 461;
            this.chkSetManualSectionsSteer.Text = "        Manual Sections";
            this.chkSetManualSectionsSteer.UseVisualStyleBackColor = false;
            this.chkSetManualSectionsSteer.Click += new System.EventHandler(this.chkSetManualSectionsSteer_Click);
            // 
            // chkSetAutoSections
            // 
            this.chkSetAutoSections.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetAutoSections.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetAutoSections.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetAutoSections.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetAutoSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetAutoSections.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.chkSetAutoSections.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetAutoSections.Location = new System.Drawing.Point(10, 240);
            this.chkSetAutoSections.Name = "chkSetAutoSections";
            this.chkSetAutoSections.Size = new System.Drawing.Size(300, 70);
            this.chkSetAutoSections.TabIndex = 456;
            this.chkSetAutoSections.Text = "        Auto Sections";
            this.chkSetAutoSections.UseVisualStyleBackColor = false;
            this.chkSetAutoSections.Click += new System.EventHandler(this.chkSetAutoSections_Click);
            // 
            // chkSetManualSections
            // 
            this.chkSetManualSections.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSetManualSections.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSetManualSections.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSetManualSections.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSetManualSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSetManualSections.Image = global::AgOpenGPS.Properties.Resources.ManualOff;
            this.chkSetManualSections.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSetManualSections.Location = new System.Drawing.Point(10, 160);
            this.chkSetManualSections.Name = "chkSetManualSections";
            this.chkSetManualSections.Size = new System.Drawing.Size(300, 70);
            this.chkSetManualSections.TabIndex = 0;
            this.chkSetManualSections.Text = "        Manual Sections";
            this.chkSetManualSections.UseVisualStyleBackColor = false;
            this.chkSetManualSections.Click += new System.EventHandler(this.chkSetManualSections_Click);
            // 
            // grpSwitch
            // 
            this.grpSwitch.BackColor = System.Drawing.Color.Transparent;
            this.grpSwitch.Controls.Add(this.chkSelectWorkSwitch);
            this.grpSwitch.Controls.Add(this.chkWorkSwActiveLow);
            this.grpSwitch.Controls.Add(this.chkSetAutoSections);
            this.grpSwitch.Controls.Add(this.chkSetManualSections);
            this.grpSwitch.ForeColor = System.Drawing.Color.Black;
            this.grpSwitch.Location = new System.Drawing.Point(100, 100);
            this.grpSwitch.Name = "grpSwitch";
            this.grpSwitch.Size = new System.Drawing.Size(320, 420);
            this.grpSwitch.TabIndex = 459;
            this.grpSwitch.TabStop = false;
            this.grpSwitch.Text = "WorkSwitch";
            // 
            // chkSelectWorkSwitch
            // 
            this.chkSelectWorkSwitch.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectWorkSwitch.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSelectWorkSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSelectWorkSwitch.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSelectWorkSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectWorkSwitch.Image = global::AgOpenGPS.Properties.Resources.HydraulicLiftOff;
            this.chkSelectWorkSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectWorkSwitch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSelectWorkSwitch.Location = new System.Drawing.Point(10, 60);
            this.chkSelectWorkSwitch.Name = "chkSelectWorkSwitch";
            this.chkSelectWorkSwitch.Size = new System.Drawing.Size(300, 70);
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
            this.chkWorkSwActiveLow.Image = global::AgOpenGPS.Properties.Resources.SwitchActiveClosed;
            this.chkWorkSwActiveLow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkWorkSwActiveLow.Location = new System.Drawing.Point(10, 340);
            this.chkWorkSwActiveLow.Name = "chkWorkSwActiveLow";
            this.chkWorkSwActiveLow.Size = new System.Drawing.Size(300, 70);
            this.chkWorkSwActiveLow.TabIndex = 0;
            this.chkWorkSwActiveLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWorkSwActiveLow.UseVisualStyleBackColor = false;
            this.chkWorkSwActiveLow.Click += new System.EventHandler(this.chkWorkSwActiveLow_Click);
            // 
            // cboxAutoSteerAuto
            // 
            this.cboxAutoSteerAuto.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxAutoSteerAuto.BackColor = System.Drawing.Color.Firebrick;
            this.cboxAutoSteerAuto.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxAutoSteerAuto.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGreen;
            this.cboxAutoSteerAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxAutoSteerAuto.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOn;
            this.cboxAutoSteerAuto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboxAutoSteerAuto.Location = new System.Drawing.Point(10, 340);
            this.cboxAutoSteerAuto.Name = "cboxAutoSteerAuto";
            this.cboxAutoSteerAuto.Size = new System.Drawing.Size(300, 70);
            this.cboxAutoSteerAuto.TabIndex = 495;
            this.cboxAutoSteerAuto.Text = "        Manual";
            this.cboxAutoSteerAuto.UseVisualStyleBackColor = false;
            this.cboxAutoSteerAuto.CheckedChanged += new System.EventHandler(this.cboxAutoSteerAuto_CheckedChanged);
            this.cboxAutoSteerAuto.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxAutoSteerAuto_HelpRequested);
            // 
            // ConfigRemoteSwitch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.grpControls);
            this.Controls.Add(this.grpSwitch);
            this.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConfigRemoteSwitch";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigRemoteSwitch_Load);
            this.grpControls.ResumeLayout(false);
            this.grpSwitch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpControls;
        private System.Windows.Forms.CheckBox chkSetAutoSections;
        private System.Windows.Forms.CheckBox chkSetManualSections;
        private System.Windows.Forms.GroupBox grpSwitch;
        private System.Windows.Forms.CheckBox chkSelectSteerSwitch;
        private System.Windows.Forms.CheckBox chkSelectWorkSwitch;
        private System.Windows.Forms.CheckBox chkWorkSwActiveLow;
        private System.Windows.Forms.CheckBox chkSetAutoSectionsSteer;
        private System.Windows.Forms.CheckBox chkSetManualSectionsSteer;
        private System.Windows.Forms.CheckBox cboxAutoSteerAuto;
    }
}
