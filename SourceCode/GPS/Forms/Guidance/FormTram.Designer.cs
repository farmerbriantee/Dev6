﻿namespace AgOpenGPS
{
    partial class FormTram
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSmallSnapRight = new System.Windows.Forms.Label();
            this.nudPasses = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMode = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdjLeft = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAdjRight = new System.Windows.Forms.Button();
            this.lblTrack = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblToolWidthHalf = new System.Windows.Forms.Label();
            this.lblTramWidth = new System.Windows.Forms.Label();
            this.nudFirstPass = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudOuterTramPasses = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudPasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirstPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterTramPasses)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSmallSnapRight
            // 
            this.lblSmallSnapRight.AutoSize = true;
            this.lblSmallSnapRight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSmallSnapRight.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmallSnapRight.ForeColor = System.Drawing.Color.Black;
            this.lblSmallSnapRight.Location = new System.Drawing.Point(102, 182);
            this.lblSmallSnapRight.Name = "lblSmallSnapRight";
            this.lblSmallSnapRight.Size = new System.Drawing.Size(47, 19);
            this.lblSmallSnapRight.TabIndex = 424;
            this.lblSmallSnapRight.Text = "Tram";
            this.lblSmallSnapRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudPasses
            // 
            this.nudPasses.BackColor = System.Drawing.Color.AliceBlue;
            this.nudPasses.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudPasses.InterceptArrowKeys = false;
            this.nudPasses.Location = new System.Drawing.Point(190, 293);
            this.nudPasses.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudPasses.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.nudPasses.Name = "nudPasses";
            this.nudPasses.ReadOnly = true;
            this.nudPasses.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudPasses.Size = new System.Drawing.Size(100, 46);
            this.nudPasses.TabIndex = 433;
            this.nudPasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPasses.Value = new decimal(new int[] {
            777,
            0,
            0,
            0});
            this.nudPasses.Click += new System.EventHandler(this.nudLastPass_Click);
            this.nudPasses.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudPasses_HelpRequested);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(170, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 48);
            this.label3.TabIndex = 435;
            this.label3.Text = "Passes";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(110, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 26);
            this.label2.TabIndex = 459;
            this.label2.Text = "10 cm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMode
            // 
            this.btnMode.BackColor = System.Drawing.Color.Transparent;
            this.btnMode.FlatAppearance.BorderSize = 0;
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMode.ForeColor = System.Drawing.Color.White;
            this.btnMode.Image = global::AgOpenGPS.Properties.Resources.TramOff;
            this.btnMode.Location = new System.Drawing.Point(249, 174);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(72, 62);
            this.btnMode.TabIndex = 460;
            this.btnMode.UseVisualStyleBackColor = false;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            this.btnMode.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnMode_HelpRequested);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnLeft.BackgroundImage = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeft.ForeColor = System.Drawing.Color.White;
            this.btnLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLeft.Location = new System.Drawing.Point(37, 86);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(64, 64);
            this.btnLeft.TabIndex = 456;
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            this.btnLeft.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnLeft_HelpRequested);
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.Color.Transparent;
            this.btnRight.BackgroundImage = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRight.ForeColor = System.Drawing.Color.White;
            this.btnRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRight.Location = new System.Drawing.Point(217, 87);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(64, 64);
            this.btnRight.TabIndex = 457;
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            this.btnRight.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnRight_HelpRequested);
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.ForeColor = System.Drawing.Color.White;
            this.btnSwapAB.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(9, 174);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(72, 62);
            this.btnSwapAB.TabIndex = 438;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.btnSwapAB_Click);
            this.btnSwapAB.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnSwapAB_HelpRequested);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(9, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 62);
            this.btnCancel.TabIndex = 421;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnCancel_HelpRequested);
            // 
            // btnAdjLeft
            // 
            this.btnAdjLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.BackgroundImage = global::AgOpenGPS.Properties.Resources.SnapLeftHalf;
            this.btnAdjLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdjLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjLeft.FlatAppearance.BorderSize = 0;
            this.btnAdjLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjLeft.ForeColor = System.Drawing.Color.White;
            this.btnAdjLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjLeft.Location = new System.Drawing.Point(11, 7);
            this.btnAdjLeft.Name = "btnAdjLeft";
            this.btnAdjLeft.Size = new System.Drawing.Size(76, 55);
            this.btnAdjLeft.TabIndex = 416;
            this.btnAdjLeft.UseVisualStyleBackColor = false;
            this.btnAdjLeft.Click += new System.EventHandler(this.btnAdjLeft_Click);
            this.btnAdjLeft.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnAdjLeft_HelpRequested);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(249, 377);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 62);
            this.btnExit.TabIndex = 234;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnExit_HelpRequested);
            // 
            // btnAdjRight
            // 
            this.btnAdjRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.BackgroundImage = global::AgOpenGPS.Properties.Resources.SnapRightHalf;
            this.btnAdjRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdjRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjRight.FlatAppearance.BorderSize = 0;
            this.btnAdjRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjRight.ForeColor = System.Drawing.Color.White;
            this.btnAdjRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjRight.Location = new System.Drawing.Point(246, 7);
            this.btnAdjRight.Name = "btnAdjRight";
            this.btnAdjRight.Size = new System.Drawing.Size(76, 55);
            this.btnAdjRight.TabIndex = 415;
            this.btnAdjRight.UseVisualStyleBackColor = false;
            this.btnAdjRight.Click += new System.EventHandler(this.btnAdjRight_Click);
            this.btnAdjRight.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnAdjRight_HelpRequested);
            // 
            // lblTrack
            // 
            this.lblTrack.AutoSize = true;
            this.lblTrack.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTrack.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrack.ForeColor = System.Drawing.Color.Black;
            this.lblTrack.Location = new System.Drawing.Point(155, 209);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(68, 23);
            this.lblTrack.TabIndex = 465;
            this.lblTrack.Text = "10 cm";
            this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(101, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 19);
            this.label6.TabIndex = 464;
            this.label6.Text = "Track";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToolWidthHalf
            // 
            this.lblToolWidthHalf.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblToolWidthHalf.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolWidthHalf.ForeColor = System.Drawing.Color.Black;
            this.lblToolWidthHalf.Location = new System.Drawing.Point(110, 15);
            this.lblToolWidthHalf.Name = "lblToolWidthHalf";
            this.lblToolWidthHalf.Size = new System.Drawing.Size(105, 32);
            this.lblToolWidthHalf.TabIndex = 463;
            this.lblToolWidthHalf.Text = "Tool";
            this.lblToolWidthHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTramWidth
            // 
            this.lblTramWidth.AutoSize = true;
            this.lblTramWidth.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTramWidth.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTramWidth.ForeColor = System.Drawing.Color.Black;
            this.lblTramWidth.Location = new System.Drawing.Point(155, 179);
            this.lblTramWidth.Name = "lblTramWidth";
            this.lblTramWidth.Size = new System.Drawing.Size(68, 23);
            this.lblTramWidth.TabIndex = 462;
            this.lblTramWidth.Text = "10 cm";
            this.lblTramWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFirstPass
            // 
            this.nudFirstPass.BackColor = System.Drawing.Color.AliceBlue;
            this.nudFirstPass.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudFirstPass.InterceptArrowKeys = false;
            this.nudFirstPass.Location = new System.Drawing.Point(40, 293);
            this.nudFirstPass.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudFirstPass.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.nudFirstPass.Name = "nudFirstPass";
            this.nudFirstPass.ReadOnly = true;
            this.nudFirstPass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudFirstPass.Size = new System.Drawing.Size(100, 46);
            this.nudFirstPass.TabIndex = 466;
            this.nudFirstPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudFirstPass.Value = new decimal(new int[] {
            777,
            0,
            0,
            0});
            this.nudFirstPass.Click += new System.EventHandler(this.nudFirstPass_Click);
            this.nudFirstPass.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudPasses_HelpRequested);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(20, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 48);
            this.label1.TabIndex = 467;
            this.label1.Text = "First Pass";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudOuterTramPasses
            // 
            this.nudOuterTramPasses.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOuterTramPasses.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOuterTramPasses.InterceptArrowKeys = false;
            this.nudOuterTramPasses.Location = new System.Drawing.Point(121, 393);
            this.nudOuterTramPasses.Name = "nudOuterTramPasses";
            this.nudOuterTramPasses.ReadOnly = true;
            this.nudOuterTramPasses.Size = new System.Drawing.Size(100, 46);
            this.nudOuterTramPasses.TabIndex = 468;
            this.nudOuterTramPasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudOuterTramPasses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOuterTramPasses.Click += new System.EventHandler(this.nudOuterTramPasses_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(101, 342);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 48);
            this.label4.TabIndex = 469;
            this.label4.Text = "Outer Tram Passes";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // FormTram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(330, 451);
            this.Controls.Add(this.nudOuterTramPasses);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudFirstPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudPasses);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTrack);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnAdjLeft);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSwapAB);
            this.Controls.Add(this.btnAdjRight);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblSmallSnapRight);
            this.Controls.Add(this.lblToolWidthHalf);
            this.Controls.Add(this.lblTramWidth);
            this.Controls.Add(this.btnMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTram";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AB Line Tramline";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTram_FormClosing);
            this.Load += new System.EventHandler(this.FormTram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirstPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterTramPasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAdjLeft;
        private System.Windows.Forms.Button btnAdjRight;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSmallSnapRight;
        private System.Windows.Forms.NumericUpDown nudPasses;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMode;
        private System.Windows.Forms.Label lblTramWidth;
        private System.Windows.Forms.Label lblToolWidthHalf;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudFirstPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudOuterTramPasses;
        private System.Windows.Forms.Label label4;
    }
}