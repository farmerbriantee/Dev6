namespace AgOpenGPS
{
    partial class ConfigDimensions
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
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.nudVehicleTrack = new System.Windows.Forms.Button();
            this.nudWheelbase = new System.Windows.Forms.Button();
            this.nudMinTurnRadius = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label60.Location = new System.Drawing.Point(566, 383);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(65, 13);
            this.label60.TabIndex = 484;
            this.label60.Text = "Turn Radius";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label53.Location = new System.Drawing.Point(641, 80);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(35, 13);
            this.label53.TabIndex = 483;
            this.label53.Text = "Track";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label44.Location = new System.Drawing.Point(78, 320);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(61, 13);
            this.label44.TabIndex = 482;
            this.label44.Text = "Wheelbase";
            // 
            // nudVehicleTrack
            // 
            this.nudVehicleTrack.BackColor = System.Drawing.Color.AliceBlue;
            this.nudVehicleTrack.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudVehicleTrack.Location = new System.Drawing.Point(580, 99);
            this.nudVehicleTrack.Name = "nudVehicleTrack";
            this.nudVehicleTrack.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudVehicleTrack.Size = new System.Drawing.Size(152, 52);
            this.nudVehicleTrack.TabIndex = 481;
            this.nudVehicleTrack.Text = "301";
            this.nudVehicleTrack.Click += new System.EventHandler(this.nudVehicleTrack_Click);
            // 
            // nudWheelbase
            // 
            this.nudWheelbase.BackColor = System.Drawing.Color.AliceBlue;
            this.nudWheelbase.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudWheelbase.Location = new System.Drawing.Point(81, 339);
            this.nudWheelbase.Name = "nudWheelbase";
            this.nudWheelbase.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudWheelbase.Size = new System.Drawing.Size(152, 52);
            this.nudWheelbase.TabIndex = 480;
            this.nudWheelbase.Text = "499";
            this.nudWheelbase.Click += new System.EventHandler(this.nudWheelbase_Click);
            // 
            // nudMinTurnRadius
            // 
            this.nudMinTurnRadius.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMinTurnRadius.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinTurnRadius.Location = new System.Drawing.Point(569, 402);
            this.nudMinTurnRadius.Name = "nudMinTurnRadius";
            this.nudMinTurnRadius.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudMinTurnRadius.Size = new System.Drawing.Size(152, 52);
            this.nudMinTurnRadius.TabIndex = 479;
            this.nudMinTurnRadius.Text = "301";
            this.nudMinTurnRadius.Click += new System.EventHandler(this.nudMinTurnRadius_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AgOpenGPS.Properties.Resources.RadiusWheelBase;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(176, 69);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(469, 415);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 478;
            this.pictureBox1.TabStop = false;
            // 
            // ConfigDimensions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label60);
            this.Controls.Add(this.label53);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.nudVehicleTrack);
            this.Controls.Add(this.nudWheelbase);
            this.Controls.Add(this.nudMinTurnRadius);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ConfigDimensions";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigDimensions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button nudVehicleTrack;
        private System.Windows.Forms.Button nudWheelbase;
        private System.Windows.Forms.Button nudMinTurnRadius;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
