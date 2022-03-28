namespace AgOpenGPS
{
    partial class ConfigSourceData
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
            this.lvLines = new System.Windows.Forms.ListView();
            this.chDistance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNetwork = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.btnUseMount = new System.Windows.Forms.Button();
            this.btnSite = new System.Windows.Forms.Button();
            this.lblCurrentAutoSteerPort = new System.Windows.Forms.Label();
            this.tboxMount3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lvLines
            // 
            this.lvLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLines.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDistance,
            this.chName,
            this.chLat,
            this.chLon,
            this.chFormat,
            this.chNetwork});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.LabelWrap = false;
            this.lvLines.Location = new System.Drawing.Point(0, 0);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(890, 500);
            this.lvLines.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLines.TabIndex = 102;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            this.lvLines.SelectedIndexChanged += new System.EventHandler(this.lvLines_SelectedIndexChanged);
            // 
            // chDistance
            // 
            this.chDistance.Text = "Distance";
            this.chDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDistance.Width = 100;
            // 
            // chName
            // 
            this.chName.Text = "Mount Point";
            this.chName.Width = 250;
            // 
            // chLat
            // 
            this.chLat.Text = "Lat";
            this.chLat.Width = 80;
            // 
            // chLon
            // 
            this.chLon.Text = "Lon";
            this.chLon.Width = 90;
            // 
            // chFormat
            // 
            this.chFormat.Text = "Format";
            this.chFormat.Width = 110;
            // 
            // chNetwork
            // 
            this.chNetwork.Text = "Network";
            this.chNetwork.Width = 200;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.button1.Location = new System.Drawing.Point(730, 513);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 64);
            this.button1.TabIndex = 107;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnUseMount
            // 
            this.btnUseMount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUseMount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUseMount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUseMount.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseMount.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnUseMount.Location = new System.Drawing.Point(404, 513);
            this.btnUseMount.Name = "btnUseMount";
            this.btnUseMount.Size = new System.Drawing.Size(157, 64);
            this.btnUseMount.TabIndex = 105;
            this.btnUseMount.UseVisualStyleBackColor = true;
            this.btnUseMount.Click += new System.EventHandler(this.btnUseMount_Click);
            // 
            // btnSite
            // 
            this.btnSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSite.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSite.Location = new System.Drawing.Point(567, 513);
            this.btnSite.Name = "btnSite";
            this.btnSite.Size = new System.Drawing.Size(157, 64);
            this.btnSite.TabIndex = 106;
            this.btnSite.UseVisualStyleBackColor = true;
            this.btnSite.Click += new System.EventHandler(this.btnSite_Click);
            // 
            // lblCurrentAutoSteerPort
            // 
            this.lblCurrentAutoSteerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentAutoSteerPort.AutoSize = true;
            this.lblCurrentAutoSteerPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAutoSteerPort.Location = new System.Drawing.Point(38, 537);
            this.lblCurrentAutoSteerPort.Name = "lblCurrentAutoSteerPort";
            this.lblCurrentAutoSteerPort.Size = new System.Drawing.Size(59, 18);
            this.lblCurrentAutoSteerPort.TabIndex = 104;
            this.lblCurrentAutoSteerPort.Text = "Mount:";
            this.lblCurrentAutoSteerPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tboxMount3
            // 
            this.tboxMount3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tboxMount3.BackColor = System.Drawing.SystemColors.Window;
            this.tboxMount3.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxMount3.Location = new System.Drawing.Point(120, 528);
            this.tboxMount3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxMount3.Name = "tboxMount3";
            this.tboxMount3.ReadOnly = true;
            this.tboxMount3.Size = new System.Drawing.Size(244, 33);
            this.tboxMount3.TabIndex = 103;
            // 
            // ConfigSourceData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lvLines);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnUseMount);
            this.Controls.Add(this.btnSite);
            this.Controls.Add(this.lblCurrentAutoSteerPort);
            this.Controls.Add(this.tboxMount3);
            this.Name = "ConfigSourceData";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigSourceData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chLat;
        private System.Windows.Forms.ColumnHeader chLon;
        private System.Windows.Forms.ColumnHeader chFormat;
        private System.Windows.Forms.ColumnHeader chNetwork;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnUseMount;
        private System.Windows.Forms.Button btnSite;
        private System.Windows.Forms.Label lblCurrentAutoSteerPort;
        private System.Windows.Forms.TextBox tboxMount3;
    }
}
