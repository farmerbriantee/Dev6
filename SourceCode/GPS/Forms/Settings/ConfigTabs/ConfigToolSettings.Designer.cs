namespace AgOpenGPS
{
    partial class ConfigToolSettings
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
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudLookAheadOff = new System.Windows.Forms.Button();
            this.nudOffset = new System.Windows.Forms.Button();
            this.nudTurnOffDelay = new System.Windows.Forms.Button();
            this.nudLookAhead = new System.Windows.Forms.Button();
            this.nudOverlap = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.ForeColor = System.Drawing.Color.Black;
            this.label66.Location = new System.Drawing.Point(636, 441);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(75, 13);
            this.label66.TabIndex = 488;
            this.label66.Text = "Overlap / Gap";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.BackColor = System.Drawing.Color.Transparent;
            this.label65.ForeColor = System.Drawing.Color.Black;
            this.label65.Location = new System.Drawing.Point(419, 517);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(35, 13);
            this.label65.TabIndex = 487;
            this.label65.Text = "Offset";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(43, 71);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 24);
            this.label16.TabIndex = 486;
            this.label16.Text = "On (secs)";
            this.label16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(43, 231);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 24);
            this.label14.TabIndex = 485;
            this.label14.Text = "Off (secs)";
            this.label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(43, 370);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 52);
            this.label10.TabIndex = 484;
            this.label10.Text = "Delay (secs)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudLookAheadOff
            // 
            this.nudLookAheadOff.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLookAheadOff.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLookAheadOff.Location = new System.Drawing.Point(44, 258);
            this.nudLookAheadOff.Name = "nudLookAheadOff";
            this.nudLookAheadOff.Size = new System.Drawing.Size(114, 52);
            this.nudLookAheadOff.TabIndex = 482;
            this.nudLookAheadOff.Text = "0.5";
            this.nudLookAheadOff.UseVisualStyleBackColor = false;
            this.nudLookAheadOff.Click += new System.EventHandler(this.nudLookAheadOff_Click);
            this.nudLookAheadOff.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudLookAheadOff_HelpRequested);
            // 
            // nudOffset
            // 
            this.nudOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOffset.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOffset.Location = new System.Drawing.Point(422, 462);
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.Size = new System.Drawing.Size(114, 52);
            this.nudOffset.TabIndex = 478;
            this.nudOffset.Text = "-8";
            this.nudOffset.UseVisualStyleBackColor = false;
            this.nudOffset.Click += new System.EventHandler(this.nudOffset_Click);
            this.nudOffset.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudOffset_HelpRequested);
            // 
            // nudTurnOffDelay
            // 
            this.nudTurnOffDelay.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTurnOffDelay.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTurnOffDelay.Location = new System.Drawing.Point(44, 425);
            this.nudTurnOffDelay.Name = "nudTurnOffDelay";
            this.nudTurnOffDelay.Size = new System.Drawing.Size(114, 52);
            this.nudTurnOffDelay.TabIndex = 481;
            this.nudTurnOffDelay.Text = "0.0";
            this.nudTurnOffDelay.UseVisualStyleBackColor = false;
            this.nudTurnOffDelay.Click += new System.EventHandler(this.nudTurnOffDelay_Click);
            this.nudTurnOffDelay.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudTurnOffDelay_HelpRequested);
            // 
            // nudLookAhead
            // 
            this.nudLookAhead.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLookAhead.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLookAhead.Location = new System.Drawing.Point(44, 98);
            this.nudLookAhead.Name = "nudLookAhead";
            this.nudLookAhead.Size = new System.Drawing.Size(114, 52);
            this.nudLookAhead.TabIndex = 480;
            this.nudLookAhead.Text = "1";
            this.nudLookAhead.UseVisualStyleBackColor = false;
            this.nudLookAhead.Click += new System.EventHandler(this.nudLookAhead_Click);
            this.nudLookAhead.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudLookAhead_HelpRequested);
            // 
            // nudOverlap
            // 
            this.nudOverlap.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOverlap.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOverlap.Location = new System.Drawing.Point(639, 386);
            this.nudOverlap.Name = "nudOverlap";
            this.nudOverlap.Size = new System.Drawing.Size(114, 52);
            this.nudOverlap.TabIndex = 479;
            this.nudOverlap.Text = "8";
            this.nudOverlap.UseVisualStyleBackColor = false;
            this.nudOverlap.Click += new System.EventHandler(this.nudOverlap_Click);
            this.nudOverlap.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudOverlap_HelpRequested);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::AgOpenGPS.Properties.Resources.ImplementSettings;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(153, 16);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(660, 534);
            this.pictureBox3.TabIndex = 483;
            this.pictureBox3.TabStop = false;
            // 
            // ConfigToolSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label66);
            this.Controls.Add(this.label65);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.nudLookAheadOff);
            this.Controls.Add(this.nudOffset);
            this.Controls.Add(this.nudTurnOffDelay);
            this.Controls.Add(this.nudLookAhead);
            this.Controls.Add(this.nudOverlap);
            this.Controls.Add(this.pictureBox3);
            this.Name = "ConfigToolSettings";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigToolSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button nudLookAheadOff;
        private System.Windows.Forms.Button nudOffset;
        private System.Windows.Forms.Button nudTurnOffDelay;
        private System.Windows.Forms.Button nudLookAhead;
        private System.Windows.Forms.Button nudOverlap;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}
