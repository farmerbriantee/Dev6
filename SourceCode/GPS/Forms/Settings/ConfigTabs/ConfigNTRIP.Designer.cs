namespace AgOpenGPS
{
    partial class ConfigNTRIP
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
            this.label158 = new System.Windows.Forms.Label();
            this.cboxGGAManual = new System.Windows.Forms.ComboBox();
            this.label159 = new System.Windows.Forms.Label();
            this.nudLongitude = new System.Windows.Forms.Button();
            this.label160 = new System.Windows.Forms.Label();
            this.nudLatitude = new System.Windows.Forms.Button();
            this.btnSetManualPosition = new System.Windows.Forms.Button();
            this.label154 = new System.Windows.Forms.Label();
            this.nudSendToUDPPort = new System.Windows.Forms.Button();
            this.label155 = new System.Windows.Forms.Label();
            this.label157 = new System.Windows.Forms.Label();
            this.nudGGAInterval = new System.Windows.Forms.Button();
            this.tboxUserPassword = new System.Windows.Forms.TextBox();
            this.btnGetSourceTable = new System.Windows.Forms.Button();
            this.label55 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.tboxUserName = new System.Windows.Forms.TextBox();
            this.tboxMount = new System.Windows.Forms.TextBox();
            this.cboxHTTP = new System.Windows.Forms.ComboBox();
            this.label62 = new System.Windows.Forms.Label();
            this.checkBoxusetcp = new System.Windows.Forms.CheckBox();
            this.btnPassPassword = new System.Windows.Forms.Button();
            this.label63 = new System.Windows.Forms.Label();
            this.btnPassUsername = new System.Windows.Forms.Button();
            this.btnGetIP = new System.Windows.Forms.Button();
            this.tboxCasterIP = new System.Windows.Forms.TextBox();
            this.nudCasterPort = new System.Windows.Forms.Button();
            this.label64 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.tboxThisIP = new System.Windows.Forms.TextBox();
            this.label153 = new System.Windows.Forms.Label();
            this.tboxHostName = new System.Windows.Forms.TextBox();
            this.tboxEnterURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label158
            // 
            this.label158.AutoSize = true;
            this.label158.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label158.Location = new System.Drawing.Point(28, 485);
            this.label158.Name = "label158";
            this.label158.Size = new System.Drawing.Size(42, 23);
            this.label158.TabIndex = 202;
            this.label158.Text = "Lat:";
            // 
            // cboxGGAManual
            // 
            this.cboxGGAManual.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxGGAManual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxGGAManual.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxGGAManual.FormattingEnabled = true;
            this.cboxGGAManual.Items.AddRange(new object[] {
            "Use Manual Fix",
            "Use GPS Fix"});
            this.cboxGGAManual.Location = new System.Drawing.Point(344, 494);
            this.cboxGGAManual.Name = "cboxGGAManual";
            this.cboxGGAManual.Size = new System.Drawing.Size(192, 33);
            this.cboxGGAManual.TabIndex = 208;
            // 
            // label159
            // 
            this.label159.AutoSize = true;
            this.label159.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label159.Location = new System.Drawing.Point(23, 540);
            this.label159.Name = "label159";
            this.label159.Size = new System.Drawing.Size(47, 23);
            this.label159.TabIndex = 203;
            this.label159.Text = "Lon:";
            // 
            // nudLongitude
            // 
            this.nudLongitude.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLongitude.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nudLongitude.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLongitude.Location = new System.Drawing.Point(81, 540);
            this.nudLongitude.Name = "nudLongitude";
            this.nudLongitude.Size = new System.Drawing.Size(224, 33);
            this.nudLongitude.TabIndex = 204;
            this.nudLongitude.UseVisualStyleBackColor = false;
            this.nudLongitude.Click += new System.EventHandler(this.nudLongitude_Click);
            // 
            // label160
            // 
            this.label160.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label160.Location = new System.Drawing.Point(50, 449);
            this.label160.Name = "label160";
            this.label160.Size = new System.Drawing.Size(269, 30);
            this.label160.TabIndex = 207;
            this.label160.Text = "Manual Fix:";
            this.label160.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudLatitude
            // 
            this.nudLatitude.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLatitude.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nudLatitude.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLatitude.Location = new System.Drawing.Point(81, 485);
            this.nudLatitude.Name = "nudLatitude";
            this.nudLatitude.Size = new System.Drawing.Size(224, 33);
            this.nudLatitude.TabIndex = 205;
            this.nudLatitude.UseVisualStyleBackColor = false;
            this.nudLatitude.Click += new System.EventHandler(this.nudLatitude_Click);
            // 
            // btnSetManualPosition
            // 
            this.btnSetManualPosition.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetManualPosition.Location = new System.Drawing.Point(333, 533);
            this.btnSetManualPosition.Name = "btnSetManualPosition";
            this.btnSetManualPosition.Size = new System.Drawing.Size(225, 37);
            this.btnSetManualPosition.TabIndex = 206;
            this.btnSetManualPosition.Text = "Send Location To Manual Fix";
            this.btnSetManualPosition.UseVisualStyleBackColor = true;
            this.btnSetManualPosition.Click += new System.EventHandler(this.btnSetManualPosition_Click);
            // 
            // label154
            // 
            this.label154.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label154.Location = new System.Drawing.Point(651, 454);
            this.label154.Name = "label154";
            this.label154.Size = new System.Drawing.Size(180, 31);
            this.label154.TabIndex = 197;
            this.label154.Text = "To UDP Port";
            this.label154.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudSendToUDPPort
            // 
            this.nudSendToUDPPort.BackColor = System.Drawing.Color.AliceBlue;
            this.nudSendToUDPPort.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSendToUDPPort.Location = new System.Drawing.Point(681, 496);
            this.nudSendToUDPPort.Name = "nudSendToUDPPort";
            this.nudSendToUDPPort.Size = new System.Drawing.Size(121, 40);
            this.nudSendToUDPPort.TabIndex = 196;
            this.nudSendToUDPPort.Text = "36666";
            this.nudSendToUDPPort.UseVisualStyleBackColor = false;
            this.nudSendToUDPPort.Click += new System.EventHandler(this.nudSendToUDPPort_Click);
            // 
            // label155
            // 
            this.label155.AutoSize = true;
            this.label155.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label155.Location = new System.Drawing.Point(536, 399);
            this.label155.Name = "label155";
            this.label155.Size = new System.Drawing.Size(81, 25);
            this.label155.TabIndex = 201;
            this.label155.Text = "0 = Off";
            // 
            // label157
            // 
            this.label157.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label157.Location = new System.Drawing.Point(215, 400);
            this.label157.Name = "label157";
            this.label157.Size = new System.Drawing.Size(196, 33);
            this.label157.TabIndex = 199;
            this.label157.Text = "GGA Interval (secs)";
            this.label157.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudGGAInterval
            // 
            this.nudGGAInterval.BackColor = System.Drawing.Color.AliceBlue;
            this.nudGGAInterval.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGGAInterval.Location = new System.Drawing.Point(417, 399);
            this.nudGGAInterval.Name = "nudGGAInterval";
            this.nudGGAInterval.Size = new System.Drawing.Size(113, 40);
            this.nudGGAInterval.TabIndex = 198;
            this.nudGGAInterval.Text = "15";
            this.nudGGAInterval.UseVisualStyleBackColor = false;
            this.nudGGAInterval.Click += new System.EventHandler(this.nudGGAInterval_Click);
            // 
            // tboxUserPassword
            // 
            this.tboxUserPassword.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxUserPassword.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxUserPassword.Location = new System.Drawing.Point(426, 330);
            this.tboxUserPassword.Name = "tboxUserPassword";
            this.tboxUserPassword.PasswordChar = '*';
            this.tboxUserPassword.Size = new System.Drawing.Size(252, 33);
            this.tboxUserPassword.TabIndex = 183;
            this.tboxUserPassword.Click += new System.EventHandler(this.tboxUserPassword_Click);
            // 
            // btnGetSourceTable
            // 
            this.btnGetSourceTable.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetSourceTable.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetSourceTable.Location = new System.Drawing.Point(443, 96);
            this.btnGetSourceTable.Name = "btnGetSourceTable";
            this.btnGetSourceTable.Size = new System.Drawing.Size(235, 37);
            this.btnGetSourceTable.TabIndex = 190;
            this.btnGetSourceTable.Text = "Get Source Table";
            this.btnGetSourceTable.UseVisualStyleBackColor = false;
            this.btnGetSourceTable.Click += new System.EventHandler(this.btnGetSourceTable_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.Location = new System.Drawing.Point(267, 261);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(62, 23);
            this.label55.TabIndex = 195;
            this.label55.Text = "HTTP:";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.Location = new System.Drawing.Point(430, 174);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(70, 25);
            this.label61.TabIndex = 187;
            this.label61.Text = "Mount";
            // 
            // tboxUserName
            // 
            this.tboxUserName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxUserName.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxUserName.Location = new System.Drawing.Point(426, 266);
            this.tboxUserName.Name = "tboxUserName";
            this.tboxUserName.PasswordChar = '*';
            this.tboxUserName.Size = new System.Drawing.Size(252, 33);
            this.tboxUserName.TabIndex = 182;
            this.tboxUserName.Click += new System.EventHandler(this.tboxUserName_Click);
            // 
            // tboxMount
            // 
            this.tboxMount.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxMount.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxMount.Location = new System.Drawing.Point(426, 202);
            this.tboxMount.Name = "tboxMount";
            this.tboxMount.Size = new System.Drawing.Size(341, 33);
            this.tboxMount.TabIndex = 186;
            this.tboxMount.TextChanged += new System.EventHandler(this.tboxMount_TextChanged);
            // 
            // cboxHTTP
            // 
            this.cboxHTTP.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxHTTP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxHTTP.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxHTTP.FormattingEnabled = true;
            this.cboxHTTP.Items.AddRange(new object[] {
            "1.0",
            "1.1"});
            this.cboxHTTP.Location = new System.Drawing.Point(258, 287);
            this.cboxHTTP.Name = "cboxHTTP";
            this.cboxHTTP.Size = new System.Drawing.Size(80, 33);
            this.cboxHTTP.TabIndex = 194;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.Location = new System.Drawing.Point(430, 238);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(106, 25);
            this.label62.TabIndex = 184;
            this.label62.Text = "Username";
            // 
            // checkBoxusetcp
            // 
            this.checkBoxusetcp.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxusetcp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBoxusetcp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.checkBoxusetcp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxusetcp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxusetcp.Location = new System.Drawing.Point(72, 336);
            this.checkBoxusetcp.Name = "checkBoxusetcp";
            this.checkBoxusetcp.Size = new System.Drawing.Size(129, 41);
            this.checkBoxusetcp.TabIndex = 191;
            this.checkBoxusetcp.Text = "Only TCP:Port";
            this.checkBoxusetcp.UseVisualStyleBackColor = true;
            // 
            // btnPassPassword
            // 
            this.btnPassPassword.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassPassword.Location = new System.Drawing.Point(684, 325);
            this.btnPassPassword.Name = "btnPassPassword";
            this.btnPassPassword.Size = new System.Drawing.Size(63, 40);
            this.btnPassPassword.TabIndex = 193;
            this.btnPassPassword.Text = "(o)";
            this.btnPassPassword.UseVisualStyleBackColor = true;
            this.btnPassPassword.Click += new System.EventHandler(this.btnPassPassword_Click);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label63.Location = new System.Drawing.Point(430, 302);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(100, 25);
            this.label63.TabIndex = 185;
            this.label63.Text = "Password";
            // 
            // btnPassUsername
            // 
            this.btnPassUsername.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassUsername.Location = new System.Drawing.Point(684, 261);
            this.btnPassUsername.Name = "btnPassUsername";
            this.btnPassUsername.Size = new System.Drawing.Size(63, 40);
            this.btnPassUsername.TabIndex = 192;
            this.btnPassUsername.Text = "(o)";
            this.btnPassUsername.UseVisualStyleBackColor = true;
            this.btnPassUsername.Click += new System.EventHandler(this.btnPassUsername_Click);
            // 
            // btnGetIP
            // 
            this.btnGetIP.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnGetIP.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetIP.Location = new System.Drawing.Point(36, 218);
            this.btnGetIP.Name = "btnGetIP";
            this.btnGetIP.Size = new System.Drawing.Size(157, 40);
            this.btnGetIP.TabIndex = 189;
            this.btnGetIP.Text = "Confirm IP";
            this.btnGetIP.UseVisualStyleBackColor = false;
            this.btnGetIP.Click += new System.EventHandler(this.btnGetIP_Click);
            // 
            // tboxCasterIP
            // 
            this.tboxCasterIP.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxCasterIP.Location = new System.Drawing.Point(72, 284);
            this.tboxCasterIP.Name = "tboxCasterIP";
            this.tboxCasterIP.ReadOnly = true;
            this.tboxCasterIP.Size = new System.Drawing.Size(157, 33);
            this.tboxCasterIP.TabIndex = 175;
            this.tboxCasterIP.Text = "192.168.188.255";
            this.tboxCasterIP.Validating += new System.ComponentModel.CancelEventHandler(this.tboxCasterIP_Validating);
            // 
            // nudCasterPort
            // 
            this.nudCasterPort.BackColor = System.Drawing.Color.AliceBlue;
            this.nudCasterPort.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCasterPort.Location = new System.Drawing.Point(258, 218);
            this.nudCasterPort.Name = "nudCasterPort";
            this.nudCasterPort.Size = new System.Drawing.Size(119, 40);
            this.nudCasterPort.TabIndex = 176;
            this.nudCasterPort.Text = "8888";
            this.nudCasterPort.UseVisualStyleBackColor = false;
            this.nudCasterPort.Click += new System.EventHandler(this.nudCasterPort_Click);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.Location = new System.Drawing.Point(199, 226);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(57, 25);
            this.label64.TabIndex = 177;
            this.label64.Text = "Port:";
            // 
            // label68
            // 
            this.label68.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.Location = new System.Drawing.Point(31, 116);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(334, 56);
            this.label68.TabIndex = 179;
            this.label68.Text = "Enter Broadcaster URL or IP ";
            this.label68.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.Location = new System.Drawing.Point(30, 287);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(39, 25);
            this.label70.TabIndex = 178;
            this.label70.Text = "IP:";
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label105.Location = new System.Drawing.Point(51, 46);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(27, 23);
            this.label105.TabIndex = 174;
            this.label105.Text = "IP";
            // 
            // tboxThisIP
            // 
            this.tboxThisIP.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxThisIP.Location = new System.Drawing.Point(84, 47);
            this.tboxThisIP.Name = "tboxThisIP";
            this.tboxThisIP.ReadOnly = true;
            this.tboxThisIP.Size = new System.Drawing.Size(221, 30);
            this.tboxThisIP.TabIndex = 173;
            this.tboxThisIP.Text = "192.168.1.255";
            // 
            // label153
            // 
            this.label153.AutoSize = true;
            this.label153.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label153.Location = new System.Drawing.Point(31, 15);
            this.label153.Name = "label153";
            this.label153.Size = new System.Drawing.Size(47, 23);
            this.label153.TabIndex = 180;
            this.label153.Text = "Host";
            // 
            // tboxHostName
            // 
            this.tboxHostName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxHostName.Location = new System.Drawing.Point(84, 12);
            this.tboxHostName.Name = "tboxHostName";
            this.tboxHostName.ReadOnly = true;
            this.tboxHostName.Size = new System.Drawing.Size(221, 30);
            this.tboxHostName.TabIndex = 181;
            this.tboxHostName.Text = "HostName";
            // 
            // tboxEnterURL
            // 
            this.tboxEnterURL.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxEnterURL.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxEnterURL.Location = new System.Drawing.Point(36, 175);
            this.tboxEnterURL.Name = "tboxEnterURL";
            this.tboxEnterURL.Size = new System.Drawing.Size(341, 33);
            this.tboxEnterURL.TabIndex = 188;
            this.tboxEnterURL.Text = "RTK2Go.com";
            this.tboxEnterURL.Click += new System.EventHandler(this.tboxEnterURL_Click);
            // 
            // ConfigNTRIP
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label158);
            this.Controls.Add(this.cboxGGAManual);
            this.Controls.Add(this.label159);
            this.Controls.Add(this.nudLongitude);
            this.Controls.Add(this.label160);
            this.Controls.Add(this.nudLatitude);
            this.Controls.Add(this.btnSetManualPosition);
            this.Controls.Add(this.label154);
            this.Controls.Add(this.nudSendToUDPPort);
            this.Controls.Add(this.label155);
            this.Controls.Add(this.label157);
            this.Controls.Add(this.nudGGAInterval);
            this.Controls.Add(this.tboxUserPassword);
            this.Controls.Add(this.btnGetSourceTable);
            this.Controls.Add(this.label55);
            this.Controls.Add(this.label61);
            this.Controls.Add(this.tboxUserName);
            this.Controls.Add(this.tboxMount);
            this.Controls.Add(this.cboxHTTP);
            this.Controls.Add(this.label62);
            this.Controls.Add(this.checkBoxusetcp);
            this.Controls.Add(this.btnPassPassword);
            this.Controls.Add(this.label63);
            this.Controls.Add(this.btnPassUsername);
            this.Controls.Add(this.btnGetIP);
            this.Controls.Add(this.tboxCasterIP);
            this.Controls.Add(this.tboxEnterURL);
            this.Controls.Add(this.nudCasterPort);
            this.Controls.Add(this.label64);
            this.Controls.Add(this.label68);
            this.Controls.Add(this.label70);
            this.Controls.Add(this.label105);
            this.Controls.Add(this.tboxThisIP);
            this.Controls.Add(this.label153);
            this.Controls.Add(this.tboxHostName);
            this.Name = "ConfigNTRIP";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label158;
        private System.Windows.Forms.ComboBox cboxGGAManual;
        private System.Windows.Forms.Label label159;
        private System.Windows.Forms.Button nudLongitude;
        private System.Windows.Forms.Label label160;
        private System.Windows.Forms.Button nudLatitude;
        private System.Windows.Forms.Button btnSetManualPosition;
        private System.Windows.Forms.Label label154;
        private System.Windows.Forms.Button nudSendToUDPPort;
        private System.Windows.Forms.Label label155;
        private System.Windows.Forms.Label label157;
        private System.Windows.Forms.Button nudGGAInterval;
        private System.Windows.Forms.TextBox tboxUserPassword;
        private System.Windows.Forms.Button btnGetSourceTable;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox tboxUserName;
        public System.Windows.Forms.TextBox tboxMount;
        private System.Windows.Forms.ComboBox cboxHTTP;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.CheckBox checkBoxusetcp;
        private System.Windows.Forms.Button btnPassPassword;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button btnPassUsername;
        private System.Windows.Forms.Button btnGetIP;
        private System.Windows.Forms.TextBox tboxCasterIP;
        private System.Windows.Forms.Button nudCasterPort;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.TextBox tboxThisIP;
        private System.Windows.Forms.Label label153;
        private System.Windows.Forms.TextBox tboxHostName;
        private System.Windows.Forms.TextBox tboxEnterURL;
    }
}
