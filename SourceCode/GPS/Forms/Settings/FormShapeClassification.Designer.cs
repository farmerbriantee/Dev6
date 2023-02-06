
namespace AgOpenGPS.Forms.Settings
{
    partial class FormShapeClassification
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
            this.lblHeading = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblColorRange = new System.Windows.Forms.Label();
            this.lbColorRanges = new System.Windows.Forms.ListBox();
            this.lbColors = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeading.AutoSize = true;
            this.lblHeading.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblHeading.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.Cyan;
            this.lblHeading.Location = new System.Drawing.Point(13, 9);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(363, 36);
            this.lblHeading.TabIndex = 252;
            this.lblHeading.Text = "Shapefile Classification";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(427, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 253;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblColorRange
            // 
            this.lblColorRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorRange.AutoSize = true;
            this.lblColorRange.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblColorRange.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorRange.ForeColor = System.Drawing.Color.Cyan;
            this.lblColorRange.Location = new System.Drawing.Point(623, 8);
            this.lblColorRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorRange.Name = "lblColorRange";
            this.lblColorRange.Size = new System.Drawing.Size(191, 36);
            this.lblColorRange.TabIndex = 254;
            this.lblColorRange.Text = "Color range";
            this.lblColorRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbColorRanges
            // 
            this.lbColorRanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbColorRanges.FormattingEnabled = true;
            this.lbColorRanges.ItemHeight = 36;
            this.lbColorRanges.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.lbColorRanges.Location = new System.Drawing.Point(841, 8);
            this.lbColorRanges.Name = "lbColorRanges";
            this.lbColorRanges.Size = new System.Drawing.Size(323, 184);
            this.lbColorRanges.TabIndex = 255;
            // 
            // lbColors
            // 
            this.lbColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbColors.HideSelection = false;
            this.lbColors.Location = new System.Drawing.Point(19, 105);
            this.lbColors.Name = "lbColors";
            this.lbColors.Size = new System.Drawing.Size(795, 609);
            this.lbColors.TabIndex = 256;
            this.lbColors.TileSize = new System.Drawing.Size(75, 75);
            this.lbColors.UseCompatibleStateImageBehavior = false;
            this.lbColors.View = System.Windows.Forms.View.Tile;
            // 
            // FormShapeClassification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1176, 738);
            this.Controls.Add(this.lbColors);
            this.Controls.Add(this.lbColorRanges);
            this.Controls.Add(this.lblColorRange);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblHeading);
            this.Name = "FormShapeClassification";
            this.Text = "Shape Classification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblColorRange;
        private System.Windows.Forms.ListBox lbColorRanges;
        private System.Windows.Forms.ListView lbColors;
    }
}