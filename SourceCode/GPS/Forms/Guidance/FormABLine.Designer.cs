﻿namespace AgOpenGPS
{
    partial class FormABLine
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
            this.lvLines = new System.Windows.Forms.ListView();
            this.chField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelAPlus = new System.Windows.Forms.Panel();
            this.btnPausePlay = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnCancel_APlus = new System.Windows.Forms.Button();
            this.btnEnter_APlus = new System.Windows.Forms.Button();
            this.nudHeading = new System.Windows.Forms.NumericUpDown();
            this.btnAPoint = new System.Windows.Forms.Button();
            this.btnBPoint = new System.Windows.Forms.Button();
            this.panelName = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel_Name = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panelPick = new System.Windows.Forms.Panel();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.btnEditName = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnNewABLine = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnListUse = new System.Windows.Forms.Button();
            this.btnListDelete = new System.Windows.Forms.Button();
            this.panelAPlus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading)).BeginInit();
            this.panelName.SuspendLayout();
            this.panelPick.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLines
            // 
            this.lvLines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chField});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.LabelWrap = false;
            this.lvLines.Location = new System.Drawing.Point(6, 5);
            this.lvLines.Margin = new System.Windows.Forms.Padding(0);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(350, 230);
            this.lvLines.TabIndex = 153;
            this.lvLines.TileSize = new System.Drawing.Size(350, 35);
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Tile;
            // 
            // chField
            // 
            this.chField.Text = "CurveLines";
            this.chField.Width = 399;
            // 
            // panelAPlus
            // 
            this.panelAPlus.BackColor = System.Drawing.Color.Transparent;
            this.panelAPlus.Controls.Add(this.btnPausePlay);
            this.panelAPlus.Controls.Add(this.btnManual);
            this.panelAPlus.Controls.Add(this.btnCancel_APlus);
            this.panelAPlus.Controls.Add(this.btnEnter_APlus);
            this.panelAPlus.Controls.Add(this.nudHeading);
            this.panelAPlus.Controls.Add(this.btnAPoint);
            this.panelAPlus.Controls.Add(this.btnBPoint);
            this.panelAPlus.Location = new System.Drawing.Point(448, 3);
            this.panelAPlus.Name = "panelAPlus";
            this.panelAPlus.Size = new System.Drawing.Size(233, 313);
            this.panelAPlus.TabIndex = 431;
            // 
            // btnPausePlay
            // 
            this.btnPausePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPausePlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPausePlay.Enabled = false;
            this.btnPausePlay.FlatAppearance.BorderSize = 0;
            this.btnPausePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPausePlay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausePlay.Image = global::AgOpenGPS.Properties.Resources.boundaryPause;
            this.btnPausePlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPausePlay.Location = new System.Drawing.Point(80, 232);
            this.btnPausePlay.Name = "btnPausePlay";
            this.btnPausePlay.Size = new System.Drawing.Size(82, 81);
            this.btnPausePlay.TabIndex = 430;
            this.btnPausePlay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPausePlay.UseVisualStyleBackColor = false;
            this.btnPausePlay.Click += new System.EventHandler(this.btnPausePlay_Click);
            this.btnPausePlay.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnPausePlay_HelpRequested);
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.Transparent;
            this.btnManual.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnManual.FlatAppearance.BorderSize = 0;
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManual.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.Image = global::AgOpenGPS.Properties.Resources.FileEditName;
            this.btnManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnManual.Location = new System.Drawing.Point(70, 162);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(80, 80);
            this.btnManual.TabIndex = 429;
            this.btnManual.UseVisualStyleBackColor = false;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            this.btnManual.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnManual_HelpRequested);
            // 
            // btnCancel_APlus
            // 
            this.btnCancel_APlus.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel_APlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel_APlus.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_APlus.FlatAppearance.BorderSize = 0;
            this.btnCancel_APlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel_APlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel_APlus.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel_APlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel_APlus.Location = new System.Drawing.Point(3, 230);
            this.btnCancel_APlus.Name = "btnCancel_APlus";
            this.btnCancel_APlus.Size = new System.Drawing.Size(80, 80);
            this.btnCancel_APlus.TabIndex = 428;
            this.btnCancel_APlus.UseVisualStyleBackColor = false;
            this.btnCancel_APlus.Click += new System.EventHandler(this.btnCancel_APlus_Click);
            this.btnCancel_APlus.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnCancel_APlus_HelpRequested);
            // 
            // btnEnter_APlus
            // 
            this.btnEnter_APlus.BackColor = System.Drawing.Color.Transparent;
            this.btnEnter_APlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEnter_APlus.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnEnter_APlus.FlatAppearance.BorderSize = 0;
            this.btnEnter_APlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnter_APlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnEnter_APlus.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnEnter_APlus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnter_APlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEnter_APlus.Location = new System.Drawing.Point(150, 230);
            this.btnEnter_APlus.Name = "btnEnter_APlus";
            this.btnEnter_APlus.Size = new System.Drawing.Size(80, 80);
            this.btnEnter_APlus.TabIndex = 427;
            this.btnEnter_APlus.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnEnter_APlus.UseVisualStyleBackColor = false;
            this.btnEnter_APlus.Click += new System.EventHandler(this.btnEnter_APlus_Click);
            this.btnEnter_APlus.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnEnter_APlus_HelpRequested);
            // 
            // nudHeading
            // 
            this.nudHeading.BackColor = System.Drawing.Color.AliceBlue;
            this.nudHeading.DecimalPlaces = 5;
            this.nudHeading.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHeading.InterceptArrowKeys = false;
            this.nudHeading.Location = new System.Drawing.Point(17, 109);
            this.nudHeading.Maximum = new decimal(new int[] {
            817405951,
            838,
            0,
            655360});
            this.nudHeading.Name = "nudHeading";
            this.nudHeading.ReadOnly = true;
            this.nudHeading.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudHeading.Size = new System.Drawing.Size(197, 46);
            this.nudHeading.TabIndex = 413;
            this.nudHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudHeading.Click += new System.EventHandler(this.nudHeading_Click);
            this.nudHeading.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudHeading_HelpRequested);
            // 
            // btnAPoint
            // 
            this.btnAPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAPoint.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAPoint.FlatAppearance.BorderSize = 0;
            this.btnAPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAPoint.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.btnAPoint.Location = new System.Drawing.Point(15, 15);
            this.btnAPoint.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAPoint.Name = "btnAPoint";
            this.btnAPoint.Size = new System.Drawing.Size(80, 80);
            this.btnAPoint.TabIndex = 57;
            this.btnAPoint.UseVisualStyleBackColor = true;
            this.btnAPoint.Click += new System.EventHandler(this.btnAPoint_Click);
            this.btnAPoint.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnAPoint_HelpRequested);
            // 
            // btnBPoint
            // 
            this.btnBPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBPoint.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBPoint.FlatAppearance.BorderSize = 0;
            this.btnBPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBPoint.Image = global::AgOpenGPS.Properties.Resources.LetterBBlue;
            this.btnBPoint.Location = new System.Drawing.Point(138, 15);
            this.btnBPoint.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnBPoint.Name = "btnBPoint";
            this.btnBPoint.Size = new System.Drawing.Size(80, 80);
            this.btnBPoint.TabIndex = 58;
            this.btnBPoint.UseVisualStyleBackColor = true;
            this.btnBPoint.Click += new System.EventHandler(this.btnBPoint_Click);
            this.btnBPoint.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnBPoint_HelpRequested);
            // 
            // panelName
            // 
            this.panelName.BackColor = System.Drawing.Color.Transparent;
            this.panelName.Controls.Add(this.label1);
            this.panelName.Controls.Add(this.btnAddTime);
            this.panelName.Controls.Add(this.btnAdd);
            this.panelName.Controls.Add(this.btnCancel_Name);
            this.panelName.Controls.Add(this.textBox1);
            this.panelName.Location = new System.Drawing.Point(684, 3);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(233, 313);
            this.panelName.TabIndex = 433;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 29);
            this.label1.TabIndex = 424;
            this.label1.Text = "+";
            // 
            // btnAddTime
            // 
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTime.Image = global::AgOpenGPS.Properties.Resources.Time;
            this.btnAddTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddTime.Location = new System.Drawing.Point(87, 133);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(88, 68);
            this.btnAddTime.TabIndex = 150;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            this.btnAddTime.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnAddTime_HelpRequested);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(138, 230);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 68);
            this.btnAdd.TabIndex = 150;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnAdd.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnAdd_HelpRequested);
            // 
            // btnCancel_Name
            // 
            this.btnCancel_Name.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel_Name.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel_Name.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_Name.FlatAppearance.BorderSize = 0;
            this.btnCancel_Name.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel_Name.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel_Name.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel_Name.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel_Name.Location = new System.Drawing.Point(6, 230);
            this.btnCancel_Name.Name = "btnCancel_Name";
            this.btnCancel_Name.Size = new System.Drawing.Size(88, 68);
            this.btnCancel_Name.TabIndex = 423;
            this.btnCancel_Name.UseVisualStyleBackColor = false;
            this.btnCancel_Name.Click += new System.EventHandler(this.btnCancel_APlus_Click);
            this.btnCancel_Name.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnCancel_Name_HelpRequested);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox1.CausesValidation = false;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(17, 37);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.MaxLength = 100;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(204, 77);
            this.textBox1.TabIndex = 145;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.textBox1_HelpRequested);
            // 
            // panelPick
            // 
            this.panelPick.BackColor = System.Drawing.Color.Transparent;
            this.panelPick.Controls.Add(this.btnSwapAB);
            this.panelPick.Controls.Add(this.btnEditName);
            this.panelPick.Controls.Add(this.btnDuplicate);
            this.panelPick.Controls.Add(this.lvLines);
            this.panelPick.Controls.Add(this.btnNewABLine);
            this.panelPick.Controls.Add(this.btnCancel);
            this.panelPick.Controls.Add(this.btnListUse);
            this.panelPick.Controls.Add(this.btnListDelete);
            this.panelPick.Location = new System.Drawing.Point(3, 3);
            this.panelPick.Name = "panelPick";
            this.panelPick.Size = new System.Drawing.Size(442, 313);
            this.panelPick.TabIndex = 434;
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(365, 155);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(73, 63);
            this.btnSwapAB.TabIndex = 424;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.btnSwapAB_Click);
            this.btnSwapAB.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnSwapAB_HelpRequested);
            // 
            // btnEditName
            // 
            this.btnEditName.BackColor = System.Drawing.Color.Transparent;
            this.btnEditName.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEditName.FlatAppearance.BorderSize = 0;
            this.btnEditName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditName.Image = global::AgOpenGPS.Properties.Resources.FileEditName;
            this.btnEditName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditName.Location = new System.Drawing.Point(365, 79);
            this.btnEditName.Name = "btnEditName";
            this.btnEditName.Size = new System.Drawing.Size(73, 63);
            this.btnEditName.TabIndex = 423;
            this.btnEditName.UseVisualStyleBackColor = false;
            this.btnEditName.Click += new System.EventHandler(this.btnEditName_Click);
            this.btnEditName.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnEditName_HelpRequested);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.BackColor = System.Drawing.Color.Transparent;
            this.btnDuplicate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDuplicate.FlatAppearance.BorderSize = 0;
            this.btnDuplicate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuplicate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDuplicate.Image = global::AgOpenGPS.Properties.Resources.FileCopy;
            this.btnDuplicate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDuplicate.Location = new System.Drawing.Point(365, 3);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(73, 63);
            this.btnDuplicate.TabIndex = 422;
            this.btnDuplicate.UseVisualStyleBackColor = false;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            this.btnDuplicate.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnDuplicate_HelpRequested);
            // 
            // btnNewABLine
            // 
            this.btnNewABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnNewABLine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnNewABLine.FlatAppearance.BorderSize = 0;
            this.btnNewABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewABLine.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewABLine.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.btnNewABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNewABLine.Location = new System.Drawing.Point(236, 243);
            this.btnNewABLine.Name = "btnNewABLine";
            this.btnNewABLine.Size = new System.Drawing.Size(73, 63);
            this.btnNewABLine.TabIndex = 149;
            this.btnNewABLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNewABLine.UseVisualStyleBackColor = false;
            this.btnNewABLine.Click += new System.EventHandler(this.BtnNewABLine_Click);
            this.btnNewABLine.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnNewABLine_HelpRequested);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(121, 243);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 63);
            this.btnCancel.TabIndex = 421;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnCancel_HelpRequested);
            // 
            // btnListUse
            // 
            this.btnListUse.BackColor = System.Drawing.Color.Transparent;
            this.btnListUse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnListUse.FlatAppearance.BorderSize = 0;
            this.btnListUse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListUse.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnListUse.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnListUse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnListUse.Location = new System.Drawing.Point(365, 243);
            this.btnListUse.Name = "btnListUse";
            this.btnListUse.Size = new System.Drawing.Size(73, 63);
            this.btnListUse.TabIndex = 86;
            this.btnListUse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnListUse.UseVisualStyleBackColor = false;
            this.btnListUse.Click += new System.EventHandler(this.btnListUse_Click);
            this.btnListUse.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnListUse_HelpRequested);
            // 
            // btnListDelete
            // 
            this.btnListDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnListDelete.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnListDelete.FlatAppearance.BorderSize = 0;
            this.btnListDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListDelete.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnListDelete.Image = global::AgOpenGPS.Properties.Resources.ABLineDelete;
            this.btnListDelete.Location = new System.Drawing.Point(6, 243);
            this.btnListDelete.Name = "btnListDelete";
            this.btnListDelete.Size = new System.Drawing.Size(73, 63);
            this.btnListDelete.TabIndex = 85;
            this.btnListDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnListDelete.UseVisualStyleBackColor = false;
            this.btnListDelete.Click += new System.EventHandler(this.btnListDelete_Click);
            this.btnListDelete.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.btnListDelete_HelpRequested);
            // 
            // FormABLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(920, 319);
            this.Controls.Add(this.panelPick);
            this.Controls.Add(this.panelName);
            this.Controls.Add(this.panelAPlus);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormABLine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AB Line";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormABLine_FormClosing);
            this.Load += new System.EventHandler(this.FormABLine_Load);
            this.panelAPlus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading)).EndInit();
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelPick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnBPoint;
        private System.Windows.Forms.Button btnAPoint;
        private System.Windows.Forms.Button btnListUse;
        private System.Windows.Forms.Button btnNewABLine;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chField;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelAPlus;
        private System.Windows.Forms.Button btnCancel_APlus;
        private System.Windows.Forms.Button btnEnter_APlus;
        private System.Windows.Forms.NumericUpDown nudHeading;
        private System.Windows.Forms.Panel panelName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel_Name;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnListDelete;
        private System.Windows.Forms.Panel panelPick;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnEditName;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnPausePlay;
    }
}