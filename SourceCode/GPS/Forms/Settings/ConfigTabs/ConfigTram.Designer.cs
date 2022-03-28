namespace AgOpenGPS
{
    partial class ConfigTram
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
            this.label23 = new System.Windows.Forms.Label();
            this.lblTramWidthUnits = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.nudTramWidth = new System.Windows.Forms.Button();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.cboxTramOnBackBuffer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.SuspendLayout();
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(290, 58);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(159, 16);
            this.label23.TabIndex = 487;
            this.label23.Text = "Tram Auto Control  On-Off";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTramWidthUnits
            // 
            this.lblTramWidthUnits.AutoSize = true;
            this.lblTramWidthUnits.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTramWidthUnits.ForeColor = System.Drawing.Color.Black;
            this.lblTramWidthUnits.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTramWidthUnits.Location = new System.Drawing.Point(588, 412);
            this.lblTramWidthUnits.Name = "lblTramWidthUnits";
            this.lblTramWidthUnits.Size = new System.Drawing.Size(23, 19);
            this.lblTramWidthUnits.TabIndex = 486;
            this.lblTramWidthUnits.Text = "m";
            this.lblTramWidthUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.ForeColor = System.Drawing.Color.Black;
            this.label75.Location = new System.Drawing.Point(462, 377);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(62, 13);
            this.label75.TabIndex = 485;
            this.label75.Text = "Tram Width";
            // 
            // nudTramWidth
            // 
            this.nudTramWidth.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTramWidth.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nudTramWidth.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTramWidth.Location = new System.Drawing.Point(427, 397);
            this.nudTramWidth.Name = "nudTramWidth";
            this.nudTramWidth.Size = new System.Drawing.Size(157, 52);
            this.nudTramWidth.TabIndex = 483;
            this.nudTramWidth.Text = "5000";
            this.nudTramWidth.UseVisualStyleBackColor = false;
            this.nudTramWidth.Click += new System.EventHandler(this.nudTramWidth_Click);
            this.nudTramWidth.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudTramWidth_HelpRequested);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::AgOpenGPS.Properties.Resources.ConT_TramSpacing;
            this.pictureBox8.Location = new System.Drawing.Point(250, 326);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(171, 195);
            this.pictureBox8.TabIndex = 484;
            this.pictureBox8.TabStop = false;
            // 
            // cboxTramOnBackBuffer
            // 
            this.cboxTramOnBackBuffer.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxTramOnBackBuffer.BackColor = System.Drawing.Color.Transparent;
            this.cboxTramOnBackBuffer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxTramOnBackBuffer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxTramOnBackBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxTramOnBackBuffer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxTramOnBackBuffer.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxTramOnBackBuffer.Image = global::AgOpenGPS.Properties.Resources.ConT_TramBBuffOn;
            this.cboxTramOnBackBuffer.Location = new System.Drawing.Point(242, 134);
            this.cboxTramOnBackBuffer.Name = "cboxTramOnBackBuffer";
            this.cboxTramOnBackBuffer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxTramOnBackBuffer.Size = new System.Drawing.Size(192, 102);
            this.cboxTramOnBackBuffer.TabIndex = 488;
            this.cboxTramOnBackBuffer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxTramOnBackBuffer.UseVisualStyleBackColor = false;
            this.cboxTramOnBackBuffer.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxTramOnBackBuffer_HelpRequested);
            // 
            // ConfigTram
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.cboxTramOnBackBuffer);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.lblTramWidthUnits);
            this.Controls.Add(this.label75);
            this.Controls.Add(this.nudTramWidth);
            this.Controls.Add(this.pictureBox8);
            this.Name = "ConfigTram";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigTram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblTramWidthUnits;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Button nudTramWidth;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.CheckBox cboxTramOnBackBuffer;
    }
}
