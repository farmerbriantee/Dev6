namespace AgOpenGPS
{
    partial class FormKeyboard
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
            this.keyboardString = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // keyboardString
            // 
            this.keyboardString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboardString.Font = new System.Drawing.Font("Tahoma", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyboardString.Location = new System.Drawing.Point(82, 9);
            this.keyboardString.Name = "keyboardString";
            this.keyboardString.Size = new System.Drawing.Size(698, 62);
            this.keyboardString.TabIndex = 0;
            // 
            // FormKeyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(862, 208);
            this.ControlBox = false;
            this.Controls.Add(this.keyboardString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(250, 250);
            this.Name = "FormKeyboard";
            this.Text = "Keyboard";
            this.Activated += new System.EventHandler(this.FormKeyboard_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyboard_KeyUpDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormNumeric_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormKeyboard_KeyUpDown);
            this.MouseEnter += new System.EventHandler(this.FormKeyboard_MouseEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox keyboardString;
    }
}