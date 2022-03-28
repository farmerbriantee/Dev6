namespace AgOpenGPS
{
    partial class ConfigHitch
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
            this.nudTrailingHitchLength = new System.Windows.Forms.Button();
            this.nudDrawbarLength = new System.Windows.Forms.Button();
            this.nudTankHitch = new System.Windows.Forms.Button();
            this.picboxToolHitch = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picboxToolHitch)).BeginInit();
            this.SuspendLayout();
            // 
            // nudTrailingHitchLength
            // 
            this.nudTrailingHitchLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTrailingHitchLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTrailingHitchLength.Location = new System.Drawing.Point(169, 84);
            this.nudTrailingHitchLength.Name = "nudTrailingHitchLength";
            this.nudTrailingHitchLength.Size = new System.Drawing.Size(124, 52);
            this.nudTrailingHitchLength.TabIndex = 7;
            this.nudTrailingHitchLength.Text = "3000";
            this.nudTrailingHitchLength.Click += new System.EventHandler(this.nudTrailingHitchLength_Click);
            // 
            // nudDrawbarLength
            // 
            this.nudDrawbarLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudDrawbarLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDrawbarLength.Location = new System.Drawing.Point(359, 84);
            this.nudDrawbarLength.Name = "nudDrawbarLength";
            this.nudDrawbarLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudDrawbarLength.Size = new System.Drawing.Size(124, 52);
            this.nudDrawbarLength.TabIndex = 5;
            this.nudDrawbarLength.Text = "51";
            this.nudDrawbarLength.Click += new System.EventHandler(this.nudDrawbarLength_Click);
            // 
            // nudTankHitch
            // 
            this.nudTankHitch.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTankHitch.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTankHitch.Location = new System.Drawing.Point(450, 84);
            this.nudTankHitch.Name = "nudTankHitch";
            this.nudTankHitch.Size = new System.Drawing.Size(124, 52);
            this.nudTankHitch.TabIndex = 6;
            this.nudTankHitch.Text = "75";
            this.nudTankHitch.Click += new System.EventHandler(this.nudTankHitch_Click);
            // 
            // picboxToolHitch
            // 
            this.picboxToolHitch.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolHitchPageTrailing;
            this.picboxToolHitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picboxToolHitch.Location = new System.Drawing.Point(15, 142);
            this.picboxToolHitch.Name = "picboxToolHitch";
            this.picboxToolHitch.Size = new System.Drawing.Size(857, 407);
            this.picboxToolHitch.TabIndex = 4;
            this.picboxToolHitch.TabStop = false;
            // 
            // ConfigHitch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.nudTrailingHitchLength);
            this.Controls.Add(this.nudDrawbarLength);
            this.Controls.Add(this.nudTankHitch);
            this.Controls.Add(this.picboxToolHitch);
            this.Name = "ConfigHitch";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigHitch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picboxToolHitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button nudTrailingHitchLength;
        private System.Windows.Forms.Button nudDrawbarLength;
        private System.Windows.Forms.Button nudTankHitch;
        private System.Windows.Forms.PictureBox picboxToolHitch;
    }
}
