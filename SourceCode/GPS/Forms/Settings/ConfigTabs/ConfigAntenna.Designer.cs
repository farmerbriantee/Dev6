namespace AgOpenGPS
{
    partial class ConfigAntenna
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
            this.nudAntennaHeight = new System.Windows.Forms.Button();
            this.nudAntennaPivot = new System.Windows.Forms.Button();
            this.nudAntennaOffset = new System.Windows.Forms.Button();
            this.pboxAntenna = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAntenna)).BeginInit();
            this.SuspendLayout();
            // 
            // nudAntennaHeight
            // 
            this.nudAntennaHeight.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaHeight.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaHeight.Location = new System.Drawing.Point(510, 199);
            this.nudAntennaHeight.Name = "nudAntennaHeight";
            this.nudAntennaHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudAntennaHeight.Size = new System.Drawing.Size(144, 52);
            this.nudAntennaHeight.TabIndex = 51;
            this.nudAntennaHeight.Text = "300";
            this.nudAntennaHeight.UseVisualStyleBackColor = false;
            this.nudAntennaHeight.Click += new System.EventHandler(this.nudAntennaHeight_Click);
            // 
            // nudAntennaPivot
            // 
            this.nudAntennaPivot.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaPivot.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaPivot.Location = new System.Drawing.Point(244, 34);
            this.nudAntennaPivot.Name = "nudAntennaPivot";
            this.nudAntennaPivot.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudAntennaPivot.Size = new System.Drawing.Size(144, 52);
            this.nudAntennaPivot.TabIndex = 49;
            this.nudAntennaPivot.Text = "111";
            this.nudAntennaPivot.UseVisualStyleBackColor = false;
            this.nudAntennaPivot.Click += new System.EventHandler(this.nudAntennaPivot_Click);
            // 
            // nudAntennaOffset
            // 
            this.nudAntennaOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaOffset.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaOffset.Location = new System.Drawing.Point(584, 405);
            this.nudAntennaOffset.Name = "nudAntennaOffset";
            this.nudAntennaOffset.Size = new System.Drawing.Size(144, 52);
            this.nudAntennaOffset.TabIndex = 50;
            this.nudAntennaOffset.Text = "0";
            this.nudAntennaOffset.UseVisualStyleBackColor = false;
            this.nudAntennaOffset.Click += new System.EventHandler(this.nudAntennaOffset_Click);
            // 
            // pboxAntenna
            // 
            this.pboxAntenna.BackgroundImage = global::AgOpenGPS.Properties.Resources.AntennaTractor;
            this.pboxAntenna.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pboxAntenna.Location = new System.Drawing.Point(140, 97);
            this.pboxAntenna.Name = "pboxAntenna";
            this.pboxAntenna.Size = new System.Drawing.Size(514, 446);
            this.pboxAntenna.TabIndex = 52;
            this.pboxAntenna.TabStop = false;
            // 
            // ConfigAntenna
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.nudAntennaHeight);
            this.Controls.Add(this.nudAntennaPivot);
            this.Controls.Add(this.nudAntennaOffset);
            this.Controls.Add(this.pboxAntenna);
            this.Name = "ConfigAntenna";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigAntenna_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pboxAntenna)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button nudAntennaHeight;
        private System.Windows.Forms.Button nudAntennaPivot;
        private System.Windows.Forms.Button nudAntennaOffset;
        private System.Windows.Forms.PictureBox pboxAntenna;
    }
}
