﻿namespace AgOpenGPS
{
    partial class FormCorrection 
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rollChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblCorrectionDistance = new System.Windows.Forms.Label();
            this.lblEast = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOst = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRollDegrees = new System.Windows.Forms.Label();
            this.lblEastOnGraph = new System.Windows.Forms.Label();
            this.btnScroll = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rollChart)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rollChart
            // 
            this.rollChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rollChart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            this.rollChart.BackColor = System.Drawing.Color.Black;
            chartArea2.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisY.LineWidth = 2;
            chartArea2.AxisY.MajorGrid.Interval = 2D;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisY.Maximum = 10D;
            chartArea2.AxisY.Minimum = -10D;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.BorderWidth = 0;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 100F;
            chartArea2.Position.Width = 100F;
            this.rollChart.ChartAreas.Add(chartArea2);
            this.rollChart.Location = new System.Drawing.Point(-30, 9);
            this.rollChart.Margin = new System.Windows.Forms.Padding(0);
            this.rollChart.Name = "rollChart";
            this.rollChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series4.Color = System.Drawing.Color.Red;
            series4.Name = "Ro";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series5.Color = System.Drawing.Color.Lime;
            series5.Name = "Ze";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series6.Color = System.Drawing.Color.Cyan;
            series6.Name = "Oe";
            this.rollChart.Series.Add(series4);
            this.rollChart.Series.Add(series5);
            this.rollChart.Series.Add(series6);
            this.rollChart.Size = new System.Drawing.Size(826, 569);
            this.rollChart.TabIndex = 241;
            this.rollChart.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // lblCorrectionDistance
            // 
            this.lblCorrectionDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCorrectionDistance.AutoSize = true;
            this.lblCorrectionDistance.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblCorrectionDistance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorrectionDistance.ForeColor = System.Drawing.Color.Red;
            this.lblCorrectionDistance.Location = new System.Drawing.Point(529, 583);
            this.lblCorrectionDistance.Name = "lblCorrectionDistance";
            this.lblCorrectionDistance.Size = new System.Drawing.Size(85, 29);
            this.lblCorrectionDistance.TabIndex = 245;
            this.lblCorrectionDistance.Text = "label5";
            this.lblCorrectionDistance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEast
            // 
            this.lblEast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEast.AutoSize = true;
            this.lblEast.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblEast.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEast.ForeColor = System.Drawing.Color.Lime;
            this.lblEast.Location = new System.Drawing.Point(305, 583);
            this.lblEast.Name = "lblEast";
            this.lblEast.Size = new System.Drawing.Size(85, 29);
            this.lblEast.TabIndex = 247;
            this.lblEast.Text = "label5";
            this.lblEast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoEllipsis = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Chartreuse;
            this.label5.Location = new System.Drawing.Point(210, 569);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 55);
            this.label5.TabIndex = 249;
            this.label5.Text = "Corrected Easting";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(434, 572);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 46);
            this.label1.TabIndex = 250;
            this.label1.Text = "Correction Distance";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOst
            // 
            this.lblOst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOst.AutoSize = true;
            this.lblOst.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblOst.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOst.ForeColor = System.Drawing.Color.Cyan;
            this.lblOst.Location = new System.Drawing.Point(98, 584);
            this.lblOst.Name = "lblOst";
            this.lblOst.Size = new System.Drawing.Size(85, 29);
            this.lblOst.TabIndex = 251;
            this.lblOst.Text = "label5";
            this.lblOst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoEllipsis = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Cyan;
            this.label3.Location = new System.Drawing.Point(18, 584);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 29);
            this.label3.TabIndex = 252;
            this.label3.Text = "Antenna";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Cyan;
            this.label2.Location = new System.Drawing.Point(5, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 29);
            this.label2.TabIndex = 253;
            this.label2.Text = "0.5";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoEllipsis = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Cyan;
            this.label4.Location = new System.Drawing.Point(5, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 29);
            this.label4.TabIndex = 254;
            this.label4.Text = "-0.5";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoEllipsis = true;
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.LightSalmon;
            this.label6.Location = new System.Drawing.Point(638, 573);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 47);
            this.label6.TabIndex = 256;
            this.label6.Text = "Roll Degrees";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRollDegrees
            // 
            this.lblRollDegrees.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRollDegrees.AutoSize = true;
            this.lblRollDegrees.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblRollDegrees.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRollDegrees.ForeColor = System.Drawing.Color.LightSalmon;
            this.lblRollDegrees.Location = new System.Drawing.Point(721, 581);
            this.lblRollDegrees.Name = "lblRollDegrees";
            this.lblRollDegrees.Size = new System.Drawing.Size(85, 29);
            this.lblRollDegrees.TabIndex = 255;
            this.lblRollDegrees.Text = "label5";
            this.lblRollDegrees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEastOnGraph
            // 
            this.lblEastOnGraph.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEastOnGraph.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblEastOnGraph.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEastOnGraph.ForeColor = System.Drawing.Color.Lime;
            this.lblEastOnGraph.Location = new System.Drawing.Point(736, 267);
            this.lblEastOnGraph.Name = "lblEastOnGraph";
            this.lblEastOnGraph.Size = new System.Drawing.Size(60, 29);
            this.lblEastOnGraph.TabIndex = 257;
            this.lblEastOnGraph.Text = "-88";
            this.lblEastOnGraph.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnScroll
            // 
            this.btnScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScroll.BackColor = System.Drawing.Color.Transparent;
            this.btnScroll.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnScroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScroll.ForeColor = System.Drawing.Color.Lime;
            this.btnScroll.Location = new System.Drawing.Point(701, 507);
            this.btnScroll.Name = "btnScroll";
            this.btnScroll.Size = new System.Drawing.Size(92, 60);
            this.btnScroll.TabIndex = 258;
            this.btnScroll.Text = "Scroll On Off";
            this.btnScroll.UseVisualStyleBackColor = false;
            this.btnScroll.Click += new System.EventHandler(this.btnScroll_Click_1);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoEllipsis = true;
            this.label7.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Cyan;
            this.label7.Location = new System.Drawing.Point(1, 266);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 29);
            this.label7.TabIndex = 259;
            this.label7.Text = "0";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(805, 621);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnScroll);
            this.Controls.Add(this.lblEastOnGraph);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblRollDegrees);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblOst);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblEast);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblCorrectionDistance);
            this.Controls.Add(this.rollChart);
            this.Location = new System.Drawing.Point(30, 30);
            this.Name = "FormCorrection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Roll Correction Graph";
            this.Load += new System.EventHandler(this.FormSteerGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rollChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart rollChart;
        private System.Windows.Forms.Label lblCorrectionDistance;
        private System.Windows.Forms.Label lblEast;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRollDegrees;
        private System.Windows.Forms.Label lblEastOnGraph;
        private System.Windows.Forms.Button btnScroll;
        private System.Windows.Forms.Label label7;
    }
}