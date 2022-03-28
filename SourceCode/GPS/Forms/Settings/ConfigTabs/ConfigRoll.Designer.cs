namespace AgOpenGPS
{
    partial class ConfigRoll
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
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.lblRollZeroOffset = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.hsbarRollFilter = new System.Windows.Forms.HScrollBar();
            this.btnZeroRoll = new System.Windows.Forms.Button();
            this.btnRemoveZeroOffset = new System.Windows.Forms.Button();
            this.cboxDataInvertRoll = new System.Windows.Forms.CheckBox();
            this.lblRollFilterPercent = new System.Windows.Forms.Label();
            this.btnResetIMU = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConD_RollHelper;
            this.pictureBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox9.Location = new System.Drawing.Point(648, 172);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(154, 217);
            this.pictureBox9.TabIndex = 499;
            this.pictureBox9.TabStop = false;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.ForeColor = System.Drawing.Color.Black;
            this.label78.Location = new System.Drawing.Point(469, 226);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(55, 13);
            this.label78.TabIndex = 498;
            this.label78.Text = "Invert Roll";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.ForeColor = System.Drawing.Color.Black;
            this.label77.Location = new System.Drawing.Point(102, 227);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(50, 13);
            this.label77.TabIndex = 497;
            this.label77.Text = "Zero Roll";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.ForeColor = System.Drawing.Color.Black;
            this.label76.Location = new System.Drawing.Point(102, 71);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(78, 13);
            this.label76.TabIndex = 496;
            this.label76.Text = "Remove Offset";
            // 
            // lblRollZeroOffset
            // 
            this.lblRollZeroOffset.AutoSize = true;
            this.lblRollZeroOffset.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.lblRollZeroOffset.ForeColor = System.Drawing.Color.Black;
            this.lblRollZeroOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRollZeroOffset.Location = new System.Drawing.Point(237, 274);
            this.lblRollZeroOffset.Name = "lblRollZeroOffset";
            this.lblRollZeroOffset.Size = new System.Drawing.Size(100, 33);
            this.lblRollZeroOffset.TabIndex = 486;
            this.lblRollZeroOffset.Text = "label11";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(59, 446);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(45, 23);
            this.label24.TabIndex = 494;
            this.label24.Text = "Less";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(346, 446);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(52, 23);
            this.label26.TabIndex = 493;
            this.label26.Text = "More";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(177, 417);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(122, 52);
            this.label18.TabIndex = 492;
            this.label18.Text = "Roll Filter";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // hsbarRollFilter
            // 
            this.hsbarRollFilter.LargeChange = 1;
            this.hsbarRollFilter.Location = new System.Drawing.Point(63, 472);
            this.hsbarRollFilter.Maximum = 98;
            this.hsbarRollFilter.Name = "hsbarRollFilter";
            this.hsbarRollFilter.Size = new System.Drawing.Size(335, 43);
            this.hsbarRollFilter.TabIndex = 490;
            this.hsbarRollFilter.Value = 5;
            this.hsbarRollFilter.ValueChanged += new System.EventHandler(this.hsbarRollFilter_ValueChanged);
            // 
            // btnZeroRoll
            // 
            this.btnZeroRoll.BackColor = System.Drawing.Color.Transparent;
            this.btnZeroRoll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnZeroRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.btnZeroRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroRoll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroRoll.ForeColor = System.Drawing.Color.Black;
            this.btnZeroRoll.Image = global::AgOpenGPS.Properties.Resources.ConDa_RollSetZero;
            this.btnZeroRoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZeroRoll.Location = new System.Drawing.Point(94, 246);
            this.btnZeroRoll.Name = "btnZeroRoll";
            this.btnZeroRoll.Size = new System.Drawing.Size(130, 95);
            this.btnZeroRoll.TabIndex = 487;
            this.btnZeroRoll.UseVisualStyleBackColor = false;
            this.btnZeroRoll.Click += new System.EventHandler(this.btnZeroRoll_Click);
            this.btnZeroRoll.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnZeroRoll_HelpRequested);
            // 
            // btnRemoveZeroOffset
            // 
            this.btnRemoveZeroOffset.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveZeroOffset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRemoveZeroOffset.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.btnRemoveZeroOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveZeroOffset.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveZeroOffset.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveZeroOffset.Image = global::AgOpenGPS.Properties.Resources.ConDa_RemoveOffset;
            this.btnRemoveZeroOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemoveZeroOffset.Location = new System.Drawing.Point(94, 90);
            this.btnRemoveZeroOffset.Name = "btnRemoveZeroOffset";
            this.btnRemoveZeroOffset.Size = new System.Drawing.Size(130, 95);
            this.btnRemoveZeroOffset.TabIndex = 488;
            this.btnRemoveZeroOffset.UseVisualStyleBackColor = false;
            this.btnRemoveZeroOffset.Click += new System.EventHandler(this.btnRemoveZeroOffset_Click);
            this.btnRemoveZeroOffset.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnRemoveZeroOffset_HelpRequested);
            // 
            // cboxDataInvertRoll
            // 
            this.cboxDataInvertRoll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxDataInvertRoll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboxDataInvertRoll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxDataInvertRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxDataInvertRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxDataInvertRoll.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDataInvertRoll.ForeColor = System.Drawing.Color.Black;
            this.cboxDataInvertRoll.Image = global::AgOpenGPS.Properties.Resources.ConDa_InvertRoll;
            this.cboxDataInvertRoll.Location = new System.Drawing.Point(457, 245);
            this.cboxDataInvertRoll.Name = "cboxDataInvertRoll";
            this.cboxDataInvertRoll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxDataInvertRoll.Size = new System.Drawing.Size(130, 95);
            this.cboxDataInvertRoll.TabIndex = 495;
            this.cboxDataInvertRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxDataInvertRoll.UseVisualStyleBackColor = false;
            this.cboxDataInvertRoll.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.cboxDataInvertRoll_HelpRequested);
            // 
            // lblRollFilterPercent
            // 
            this.lblRollFilterPercent.AutoSize = true;
            this.lblRollFilterPercent.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRollFilterPercent.ForeColor = System.Drawing.Color.Black;
            this.lblRollFilterPercent.Location = new System.Drawing.Point(193, 523);
            this.lblRollFilterPercent.Name = "lblRollFilterPercent";
            this.lblRollFilterPercent.Size = new System.Drawing.Size(95, 39);
            this.lblRollFilterPercent.TabIndex = 491;
            this.lblRollFilterPercent.Text = "65%";
            // 
            // btnResetIMU
            // 
            this.btnResetIMU.BackColor = System.Drawing.Color.Transparent;
            this.btnResetIMU.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConDa_ResetIMU;
            this.btnResetIMU.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnResetIMU.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnResetIMU.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.btnResetIMU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetIMU.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetIMU.ForeColor = System.Drawing.Color.Black;
            this.btnResetIMU.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetIMU.Location = new System.Drawing.Point(457, 89);
            this.btnResetIMU.Name = "btnResetIMU";
            this.btnResetIMU.Size = new System.Drawing.Size(130, 95);
            this.btnResetIMU.TabIndex = 489;
            this.btnResetIMU.UseVisualStyleBackColor = false;
            this.btnResetIMU.Click += new System.EventHandler(this.btnResetIMU_Click);
            this.btnResetIMU.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnResetIMU_HelpRequested);
            // 
            // ConfigRoll
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.label78);
            this.Controls.Add(this.label77);
            this.Controls.Add(this.label76);
            this.Controls.Add(this.lblRollZeroOffset);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.lblRollFilterPercent);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.hsbarRollFilter);
            this.Controls.Add(this.btnResetIMU);
            this.Controls.Add(this.btnZeroRoll);
            this.Controls.Add(this.btnRemoveZeroOffset);
            this.Controls.Add(this.cboxDataInvertRoll);
            this.Name = "ConfigRoll";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigRoll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label lblRollZeroOffset;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.HScrollBar hsbarRollFilter;
        private System.Windows.Forms.Button btnZeroRoll;
        private System.Windows.Forms.Button btnRemoveZeroOffset;
        private System.Windows.Forms.CheckBox cboxDataInvertRoll;
        private System.Windows.Forms.Label lblRollFilterPercent;
        private System.Windows.Forms.Button btnResetIMU;
    }
}
