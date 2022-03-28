namespace AgOpenGPS
{
    partial class ConfigToolType
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
            this.rbtnTBT = new System.Windows.Forms.RadioButton();
            this.rbtnFixedRear = new System.Windows.Forms.RadioButton();
            this.rbtnFront = new System.Windows.Forms.RadioButton();
            this.rbtnTrailing = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbtnTBT
            // 
            this.rbtnTBT.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTBT.BackColor = System.Drawing.Color.Transparent;
            this.rbtnTBT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnTBT.FlatAppearance.BorderSize = 0;
            this.rbtnTBT.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnTBT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTBT.Image = global::AgOpenGPS.Properties.Resources.ToolChkTBT;
            this.rbtnTBT.Location = new System.Drawing.Point(455, 80);
            this.rbtnTBT.Name = "rbtnTBT";
            this.rbtnTBT.Size = new System.Drawing.Size(300, 200);
            this.rbtnTBT.TabIndex = 112;
            this.rbtnTBT.UseVisualStyleBackColor = false;
            // 
            // rbtnFixedRear
            // 
            this.rbtnFixedRear.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnFixedRear.BackColor = System.Drawing.Color.Transparent;
            this.rbtnFixedRear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnFixedRear.FlatAppearance.BorderSize = 0;
            this.rbtnFixedRear.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnFixedRear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnFixedRear.Image = global::AgOpenGPS.Properties.Resources.ToolChkRear;
            this.rbtnFixedRear.Location = new System.Drawing.Point(135, 80);
            this.rbtnFixedRear.Name = "rbtnFixedRear";
            this.rbtnFixedRear.Size = new System.Drawing.Size(300, 200);
            this.rbtnFixedRear.TabIndex = 111;
            this.rbtnFixedRear.UseVisualStyleBackColor = false;
            // 
            // rbtnFront
            // 
            this.rbtnFront.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnFront.BackColor = System.Drawing.Color.Transparent;
            this.rbtnFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnFront.FlatAppearance.BorderSize = 0;
            this.rbtnFront.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnFront.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnFront.Image = global::AgOpenGPS.Properties.Resources.ToolChkFront;
            this.rbtnFront.Location = new System.Drawing.Point(135, 300);
            this.rbtnFront.Name = "rbtnFront";
            this.rbtnFront.Size = new System.Drawing.Size(300, 200);
            this.rbtnFront.TabIndex = 110;
            this.rbtnFront.UseVisualStyleBackColor = false;
            // 
            // rbtnTrailing
            // 
            this.rbtnTrailing.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTrailing.BackColor = System.Drawing.Color.Transparent;
            this.rbtnTrailing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnTrailing.Checked = true;
            this.rbtnTrailing.FlatAppearance.BorderSize = 0;
            this.rbtnTrailing.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.rbtnTrailing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTrailing.Image = global::AgOpenGPS.Properties.Resources.ToolChkTrailing;
            this.rbtnTrailing.Location = new System.Drawing.Point(455, 300);
            this.rbtnTrailing.Name = "rbtnTrailing";
            this.rbtnTrailing.Size = new System.Drawing.Size(300, 200);
            this.rbtnTrailing.TabIndex = 109;
            this.rbtnTrailing.TabStop = true;
            this.rbtnTrailing.UseVisualStyleBackColor = false;
            // 
            // ConfigToolType
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.rbtnFront);
            this.Controls.Add(this.rbtnTBT);
            this.Controls.Add(this.rbtnTrailing);
            this.Controls.Add(this.rbtnFixedRear);
            this.Name = "ConfigToolType";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigAttachStyle_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rbtnTBT;
        private System.Windows.Forms.RadioButton rbtnFixedRear;
        private System.Windows.Forms.RadioButton rbtnFront;
        private System.Windows.Forms.RadioButton rbtnTrailing;
    }
}
