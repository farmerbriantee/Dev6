namespace AgOpenGPS
{
    partial class ConfigVehicleType
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
            this.rbtnHarvester = new System.Windows.Forms.RadioButton();
            this.rbtn4WD = new System.Windows.Forms.RadioButton();
            this.rbtnTractor = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbtnHarvester
            // 
            this.rbtnHarvester.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnHarvester.FlatAppearance.BorderSize = 0;
            this.rbtnHarvester.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnHarvester.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnHarvester.Image = global::AgOpenGPS.Properties.Resources.vehiclePageHarvester;
            this.rbtnHarvester.Location = new System.Drawing.Point(135, 300);
            this.rbtnHarvester.Name = "rbtnHarvester";
            this.rbtnHarvester.Size = new System.Drawing.Size(300, 200);
            this.rbtnHarvester.TabIndex = 253;
            this.rbtnHarvester.UseVisualStyleBackColor = true;
            // 
            // rbtn4WD
            // 
            this.rbtn4WD.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn4WD.FlatAppearance.BorderSize = 0;
            this.rbtn4WD.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtn4WD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn4WD.Image = global::AgOpenGPS.Properties.Resources.vehiclePage4WD;
            this.rbtn4WD.Location = new System.Drawing.Point(135, 80);
            this.rbtn4WD.Name = "rbtn4WD";
            this.rbtn4WD.Size = new System.Drawing.Size(300, 200);
            this.rbtn4WD.TabIndex = 252;
            this.rbtn4WD.UseVisualStyleBackColor = true;
            // 
            // rbtnTractor
            // 
            this.rbtnTractor.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTractor.Checked = true;
            this.rbtnTractor.FlatAppearance.BorderSize = 0;
            this.rbtnTractor.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnTractor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTractor.Image = global::AgOpenGPS.Properties.Resources.vehiclePageTractor;
            this.rbtnTractor.Location = new System.Drawing.Point(455, 80);
            this.rbtnTractor.Name = "rbtnTractor";
            this.rbtnTractor.Size = new System.Drawing.Size(300, 200);
            this.rbtnTractor.TabIndex = 112;
            this.rbtnTractor.TabStop = true;
            this.rbtnTractor.UseVisualStyleBackColor = true;
            // 
            // ConfigVehicleType
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.rbtnHarvester);
            this.Controls.Add(this.rbtnTractor);
            this.Controls.Add(this.rbtn4WD);
            this.Name = "ConfigVehicleType";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigVehicleType_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rbtnHarvester;
        private System.Windows.Forms.RadioButton rbtn4WD;
        private System.Windows.Forms.RadioButton rbtnTractor;
    }
}
