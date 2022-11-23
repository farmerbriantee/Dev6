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
            this.nudHitchLength = new System.Windows.Forms.Button();
            this.nudTankHitchLength = new System.Windows.Forms.Button();
            this.picboxToolHitch = new System.Windows.Forms.PictureBox();
            this.nudTankAxleLength = new System.Windows.Forms.Button();
            this.nudTrailingAxleLength = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picboxToolHitch)).BeginInit();
            this.SuspendLayout();
            // 
            // nudTrailingHitchLength
            // 
            this.nudTrailingHitchLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTrailingHitchLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTrailingHitchLength.Location = new System.Drawing.Point(50, 80);
            this.nudTrailingHitchLength.Name = "nudTrailingHitchLength";
            this.nudTrailingHitchLength.Size = new System.Drawing.Size(124, 52);
            this.nudTrailingHitchLength.TabIndex = 7;
            this.nudTrailingHitchLength.Text = "75";
            this.nudTrailingHitchLength.UseVisualStyleBackColor = false;
            this.nudTrailingHitchLength.Click += new System.EventHandler(this.nudTrailingHitchLength_Click);
            // 
            // nudHitchLength
            // 
            this.nudHitchLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudHitchLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHitchLength.Location = new System.Drawing.Point(650, 80);
            this.nudHitchLength.Name = "nudHitchLength";
            this.nudHitchLength.Size = new System.Drawing.Size(124, 52);
            this.nudHitchLength.TabIndex = 5;
            this.nudHitchLength.Text = "75";
            this.nudHitchLength.UseVisualStyleBackColor = false;
            this.nudHitchLength.Click += new System.EventHandler(this.nudHitchLength_Click);
            // 
            // nudTankHitchLength
            // 
            this.nudTankHitchLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTankHitchLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTankHitchLength.Location = new System.Drawing.Point(350, 80);
            this.nudTankHitchLength.Name = "nudTankHitchLength";
            this.nudTankHitchLength.Size = new System.Drawing.Size(124, 52);
            this.nudTankHitchLength.TabIndex = 6;
            this.nudTankHitchLength.Text = "75";
            this.nudTankHitchLength.UseVisualStyleBackColor = false;
            this.nudTankHitchLength.Click += new System.EventHandler(this.nudTankHitch_Click);
            // 
            // picboxToolHitch
            // 
            this.picboxToolHitch.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolHitchPageTBT;
            this.picboxToolHitch.Location = new System.Drawing.Point(45, 150);
            this.picboxToolHitch.Name = "picboxToolHitch";
            this.picboxToolHitch.Size = new System.Drawing.Size(800, 400);
            this.picboxToolHitch.TabIndex = 4;
            this.picboxToolHitch.TabStop = false;
            // 
            // nudTankAxleLength
            // 
            this.nudTankAxleLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTankAxleLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTankAxleLength.Location = new System.Drawing.Point(500, 80);
            this.nudTankAxleLength.Name = "nudTankAxleLength";
            this.nudTankAxleLength.Size = new System.Drawing.Size(124, 52);
            this.nudTankAxleLength.TabIndex = 8;
            this.nudTankAxleLength.Text = "3000";
            this.nudTankAxleLength.UseVisualStyleBackColor = false;
            this.nudTankAxleLength.Click += new System.EventHandler(this.nudTankAxleLength_Click);
            // 
            // nudTrailingAxleLength
            // 
            this.nudTrailingAxleLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTrailingAxleLength.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTrailingAxleLength.Location = new System.Drawing.Point(200, 80);
            this.nudTrailingAxleLength.Name = "nudTrailingAxleLength";
            this.nudTrailingAxleLength.Size = new System.Drawing.Size(124, 52);
            this.nudTrailingAxleLength.TabIndex = 9;
            this.nudTrailingAxleLength.Text = "3000";
            this.nudTrailingAxleLength.UseVisualStyleBackColor = false;
            this.nudTrailingAxleLength.Click += new System.EventHandler(this.nudTrailingAxleLength_Click);
            // 
            // ConfigHitch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.nudTrailingAxleLength);
            this.Controls.Add(this.nudTankAxleLength);
            this.Controls.Add(this.nudTrailingHitchLength);
            this.Controls.Add(this.nudHitchLength);
            this.Controls.Add(this.nudTankHitchLength);
            this.Controls.Add(this.picboxToolHitch);
            this.Name = "ConfigHitch";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigHitch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picboxToolHitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button nudTrailingHitchLength;
        private System.Windows.Forms.Button nudHitchLength;
        private System.Windows.Forms.Button nudTankHitchLength;
        private System.Windows.Forms.PictureBox picboxToolHitch;
        private System.Windows.Forms.Button nudTankAxleLength;
        private System.Windows.Forms.Button nudTrailingAxleLength;
    }
}
