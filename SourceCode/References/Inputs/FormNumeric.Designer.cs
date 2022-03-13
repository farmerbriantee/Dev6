namespace AgOpenGPS
{
    partial class FormNumeric
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
            this.tboxNumber = new System.Windows.Forms.TextBox();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnDn = new System.Windows.Forms.Button();
            this.Btn1 = new System.Windows.Forms.Button();
            this.Btn2 = new System.Windows.Forms.Button();
            this.Btn3 = new System.Windows.Forms.Button();
            this.Btn4 = new System.Windows.Forms.Button();
            this.Btn5 = new System.Windows.Forms.Button();
            this.Btn6 = new System.Windows.Forms.Button();
            this.Btn7 = new System.Windows.Forms.Button();
            this.Btn8 = new System.Windows.Forms.Button();
            this.Btn9 = new System.Windows.Forms.Button();
            this.Btn0 = new System.Windows.Forms.Button();
            this.BtnSeparator = new System.Windows.Forms.Button();
            this.BtnClear2 = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnPlus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tboxNumber
            // 
            this.tboxNumber.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxNumber.Location = new System.Drawing.Point(210, 10);
            this.tboxNumber.Name = "tboxNumber";
            this.tboxNumber.ReadOnly = true;
            this.tboxNumber.Size = new System.Drawing.Size(250, 50);
            this.tboxNumber.TabIndex = 1;
            this.tboxNumber.Text = "1234.567890";
            this.tboxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxNumber.Click += new System.EventHandler(this.TboxNumber_Click);
            // 
            // lblMax
            // 
            this.lblMax.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.Location = new System.Drawing.Point(335, 60);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(125, 30);
            this.lblMax.TabIndex = 6;
            this.lblMax.Text = "88.8";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMin
            // 
            this.lblMin.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.Location = new System.Drawing.Point(210, 60);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(125, 30);
            this.lblMin.TabIndex = 7;
            this.lblMin.Text = "-22.8";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnUp
            // 
            this.BtnUp.BackColor = System.Drawing.SystemColors.Control;
            this.BtnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnUp.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnUp.FlatAppearance.BorderSize = 2;
            this.BtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.BtnUp.Location = new System.Drawing.Point(110, 10);
            this.BtnUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(90, 90);
            this.BtnUp.TabIndex = 148;
            this.BtnUp.UseVisualStyleBackColor = false;
            this.BtnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDistanceUp_MouseDown);
            this.BtnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // BtnDn
            // 
            this.BtnDn.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnDn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnDn.FlatAppearance.BorderSize = 2;
            this.BtnDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDn.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.BtnDn.Location = new System.Drawing.Point(10, 10);
            this.BtnDn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnDn.Name = "BtnDn";
            this.BtnDn.Size = new System.Drawing.Size(90, 90);
            this.BtnDn.TabIndex = 147;
            this.BtnDn.UseVisualStyleBackColor = false;
            this.BtnDn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDistanceDn_MouseDown);
            this.BtnDn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // Btn1
            // 
            this.Btn1.BackColor = System.Drawing.SystemColors.Control;
            this.Btn1.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn1.FlatAppearance.BorderSize = 2;
            this.Btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn1.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn1.Location = new System.Drawing.Point(22, 122);
            this.Btn1.Name = "Btn1";
            this.Btn1.Size = new System.Drawing.Size(90, 90);
            this.Btn1.TabIndex = 150;
            this.Btn1.Text = "1";
            this.Btn1.UseVisualStyleBackColor = false;
            this.Btn1.Click += new System.EventHandler(this.Btn1_Click);
            // 
            // Btn2
            // 
            this.Btn2.BackColor = System.Drawing.SystemColors.Control;
            this.Btn2.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn2.FlatAppearance.BorderSize = 2;
            this.Btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn2.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn2.Location = new System.Drawing.Point(134, 122);
            this.Btn2.Name = "Btn2";
            this.Btn2.Size = new System.Drawing.Size(90, 90);
            this.Btn2.TabIndex = 151;
            this.Btn2.Text = "2";
            this.Btn2.UseVisualStyleBackColor = false;
            this.Btn2.Click += new System.EventHandler(this.Btn2_Click);
            // 
            // Btn3
            // 
            this.Btn3.BackColor = System.Drawing.SystemColors.Control;
            this.Btn3.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn3.FlatAppearance.BorderSize = 2;
            this.Btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn3.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn3.Location = new System.Drawing.Point(246, 122);
            this.Btn3.Name = "Btn3";
            this.Btn3.Size = new System.Drawing.Size(90, 90);
            this.Btn3.TabIndex = 152;
            this.Btn3.Text = "3";
            this.Btn3.UseVisualStyleBackColor = false;
            this.Btn3.Click += new System.EventHandler(this.Btn3_Click);
            // 
            // Btn4
            // 
            this.Btn4.BackColor = System.Drawing.SystemColors.Control;
            this.Btn4.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn4.FlatAppearance.BorderSize = 2;
            this.Btn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn4.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn4.Location = new System.Drawing.Point(22, 234);
            this.Btn4.Name = "Btn4";
            this.Btn4.Size = new System.Drawing.Size(90, 90);
            this.Btn4.TabIndex = 153;
            this.Btn4.Text = "4";
            this.Btn4.UseVisualStyleBackColor = false;
            this.Btn4.Click += new System.EventHandler(this.Btn4_Click);
            // 
            // Btn5
            // 
            this.Btn5.BackColor = System.Drawing.SystemColors.Control;
            this.Btn5.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn5.FlatAppearance.BorderSize = 2;
            this.Btn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn5.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn5.Location = new System.Drawing.Point(134, 234);
            this.Btn5.Name = "Btn5";
            this.Btn5.Size = new System.Drawing.Size(90, 90);
            this.Btn5.TabIndex = 154;
            this.Btn5.Text = "5";
            this.Btn5.UseVisualStyleBackColor = false;
            this.Btn5.Click += new System.EventHandler(this.Btn5_Click);
            // 
            // Btn6
            // 
            this.Btn6.BackColor = System.Drawing.SystemColors.Control;
            this.Btn6.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn6.FlatAppearance.BorderSize = 2;
            this.Btn6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn6.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn6.Location = new System.Drawing.Point(246, 234);
            this.Btn6.Name = "Btn6";
            this.Btn6.Size = new System.Drawing.Size(90, 90);
            this.Btn6.TabIndex = 155;
            this.Btn6.Text = "6";
            this.Btn6.UseVisualStyleBackColor = false;
            this.Btn6.Click += new System.EventHandler(this.Btn6_Click);
            // 
            // Btn7
            // 
            this.Btn7.BackColor = System.Drawing.SystemColors.Control;
            this.Btn7.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn7.FlatAppearance.BorderSize = 2;
            this.Btn7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn7.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn7.Location = new System.Drawing.Point(22, 346);
            this.Btn7.Name = "Btn7";
            this.Btn7.Size = new System.Drawing.Size(90, 90);
            this.Btn7.TabIndex = 156;
            this.Btn7.Text = "7";
            this.Btn7.UseVisualStyleBackColor = false;
            this.Btn7.Click += new System.EventHandler(this.Btn7_Click);
            // 
            // Btn8
            // 
            this.Btn8.BackColor = System.Drawing.SystemColors.Control;
            this.Btn8.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn8.FlatAppearance.BorderSize = 2;
            this.Btn8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn8.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn8.Location = new System.Drawing.Point(134, 346);
            this.Btn8.Name = "Btn8";
            this.Btn8.Size = new System.Drawing.Size(90, 90);
            this.Btn8.TabIndex = 157;
            this.Btn8.Text = "8";
            this.Btn8.UseVisualStyleBackColor = false;
            this.Btn8.Click += new System.EventHandler(this.Btn8_Click);
            // 
            // Btn9
            // 
            this.Btn9.BackColor = System.Drawing.SystemColors.Control;
            this.Btn9.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn9.FlatAppearance.BorderSize = 2;
            this.Btn9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn9.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn9.Location = new System.Drawing.Point(246, 346);
            this.Btn9.Name = "Btn9";
            this.Btn9.Size = new System.Drawing.Size(90, 90);
            this.Btn9.TabIndex = 158;
            this.Btn9.Text = "9";
            this.Btn9.UseVisualStyleBackColor = false;
            this.Btn9.Click += new System.EventHandler(this.Btn9_Click);
            // 
            // Btn0
            // 
            this.Btn0.BackColor = System.Drawing.SystemColors.Control;
            this.Btn0.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.Btn0.FlatAppearance.BorderSize = 2;
            this.Btn0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn0.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.Btn0.Location = new System.Drawing.Point(134, 458);
            this.Btn0.Name = "Btn0";
            this.Btn0.Size = new System.Drawing.Size(90, 90);
            this.Btn0.TabIndex = 159;
            this.Btn0.Text = "0";
            this.Btn0.UseVisualStyleBackColor = false;
            this.Btn0.Click += new System.EventHandler(this.Btn0_Click);
            // 
            // BtnSeparator
            // 
            this.BtnSeparator.BackColor = System.Drawing.SystemColors.Control;
            this.BtnSeparator.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnSeparator.FlatAppearance.BorderSize = 2;
            this.BtnSeparator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSeparator.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSeparator.Location = new System.Drawing.Point(246, 458);
            this.BtnSeparator.Name = "BtnSeparator";
            this.BtnSeparator.Size = new System.Drawing.Size(90, 90);
            this.BtnSeparator.TabIndex = 160;
            this.BtnSeparator.Text = ".";
            this.BtnSeparator.UseVisualStyleBackColor = false;
            this.BtnSeparator.Click += new System.EventHandler(this.BtnSeparator_Click);
            // 
            // BtnClear2
            // 
            this.BtnClear2.BackColor = System.Drawing.Color.Salmon;
            this.BtnClear2.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnClear2.FlatAppearance.BorderSize = 2;
            this.BtnClear2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClear2.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear2.Location = new System.Drawing.Point(358, 122);
            this.BtnClear2.Name = "BtnClear2";
            this.BtnClear2.Size = new System.Drawing.Size(90, 90);
            this.BtnClear2.TabIndex = 161;
            this.BtnClear2.Text = "C";
            this.BtnClear2.UseVisualStyleBackColor = false;
            this.BtnClear2.Click += new System.EventHandler(this.BtnClear2_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.Salmon;
            this.BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnClear.FlatAppearance.BorderSize = 2;
            this.BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClear.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.Location = new System.Drawing.Point(358, 234);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(90, 90);
            this.BtnClear.TabIndex = 162;
            this.BtnClear.Text = "CE";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnCancel.FlatAppearance.BorderSize = 2;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.BtnCancel.Location = new System.Drawing.Point(358, 346);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(90, 90);
            this.BtnCancel.TabIndex = 163;
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.SystemColors.Control;
            this.BtnOk.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnOk.FlatAppearance.BorderSize = 2;
            this.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOk.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnOk.Location = new System.Drawing.Point(358, 458);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(90, 90);
            this.BtnOk.TabIndex = 164;
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnPlus
            // 
            this.BtnPlus.BackColor = System.Drawing.SystemColors.Control;
            this.BtnPlus.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.BtnPlus.FlatAppearance.BorderSize = 2;
            this.BtnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlus.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPlus.Location = new System.Drawing.Point(22, 458);
            this.BtnPlus.Name = "BtnPlus";
            this.BtnPlus.Size = new System.Drawing.Size(90, 90);
            this.BtnPlus.TabIndex = 165;
            this.BtnPlus.Text = "+/-";
            this.BtnPlus.UseVisualStyleBackColor = false;
            this.BtnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // FormNumeric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(470, 570);
            this.ControlBox = false;
            this.Controls.Add(this.BtnPlus);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnClear2);
            this.Controls.Add(this.BtnSeparator);
            this.Controls.Add(this.Btn0);
            this.Controls.Add(this.Btn9);
            this.Controls.Add(this.Btn8);
            this.Controls.Add(this.Btn7);
            this.Controls.Add(this.Btn6);
            this.Controls.Add(this.Btn5);
            this.Controls.Add(this.Btn4);
            this.Controls.Add(this.Btn3);
            this.Controls.Add(this.Btn2);
            this.Controls.Add(this.Btn1);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.BtnDn);
            this.Controls.Add(this.tboxNumber);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblMax);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNumeric";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Enter a Value";
            this.Activated += new System.EventHandler(this.FormNumeric_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormNumeric_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tboxNumber;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Button BtnDn;
        private System.Windows.Forms.Button Btn1;
        private System.Windows.Forms.Button Btn2;
        private System.Windows.Forms.Button Btn3;
        private System.Windows.Forms.Button Btn4;
        private System.Windows.Forms.Button Btn5;
        private System.Windows.Forms.Button Btn6;
        private System.Windows.Forms.Button Btn7;
        private System.Windows.Forms.Button Btn8;
        private System.Windows.Forms.Button Btn9;
        private System.Windows.Forms.Button Btn0;
        private System.Windows.Forms.Button BtnSeparator;
        private System.Windows.Forms.Button BtnClear2;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnPlus;
    }
}