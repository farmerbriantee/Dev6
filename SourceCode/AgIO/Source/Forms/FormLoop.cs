﻿using AgIO.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormLoop : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //key event to restore window
        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;

        //Stringbuilder
        public StringBuilder logNMEASentence = new StringBuilder();
        private StringBuilder sbRTCM = new StringBuilder();

        public bool isKeyboardOn = true;

        public bool isSendToSerial = true, isSendToUDP = false;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        //timer variables
        public double secondsSinceStart, twoSecondTimer, tenSecondTimer, threeMinuteTimer;

        public string lastSentence;

        public bool isPluginUsed;

        //usually 256 - send ntrip to serial in chunks
        public int packetSizeNTRIP;

        public bool lastHelloGPS, lastHelloAutoSteer, lastHelloMachine, lastHelloIMU;
        public bool isConnectedIMU, isConnectedSteer, isConnectedMachine;

        //is the fly out displayed
        public bool isViewAdvanced = false;
        public bool isLogNMEA;

        //used to hide the window and not update text fields and most counters
        public bool isAppInFocus = true, isLostFocus;
        public int focusSkipCounter = 310;

        //The base directory where Drive will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of Comm storage
        public string commDirectory, commFileName = "";

        public FormLoop()
        {
            InitializeComponent();

            RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            object Path = Key.GetValue("WorkDir");
            if (Path == null)
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else
                baseDirectory = Path.ToString() + "\\AgOpenGPS\\";
            Key.Close();

            Portable.PortableSettingsProvider.ApplicationSettingsFile = baseDirectory + "AgOpenGPS.config";
            Portable.PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
        }

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            //get the fields directory, if not exist, create
            commDirectory = baseDirectory + "AgIO\\";
            string dir = Path.GetDirectoryName(commDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            if (Settings.Default.setUDP_isOn)
            {
                LoadUDPNetwork();
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label9.Visible = false;

                lblSteerAngle.Visible = false;
                lblWASCounts.Visible = false;
                lblSwitchStatus.Visible = false;
                lblWorkSwitchStatus.Visible = false;

                label10.Visible = false;
                label12.Visible = false;
                lbl1To8.Visible = false;
                lbl9To16.Visible = false;

                btnRelayTest.Visible = false;

                btnUDP.BackColor = Color.Gainsboro;
                lblIP.Text = "Off";
            }

            //small view
            this.Width = 428;

            LoadLoopback();

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;
            isPluginUsed = Properties.Settings.Default.setUDP_isUsePluginApp;

            packetSizeNTRIP = Properties.Settings.Default.setNTRIP_packetSize;

            isSendToSerial = Settings.Default.setNTRIP_sendToSerial;
            isSendToUDP = Settings.Default.setNTRIP_sendToUDP;

            //lblMount.Text = Properties.Settings.Default.setNTRIP_mount;

            lblGPS1Comm.Text = "---";
            lblIMUComm.Text = "---";
            lblMod1Comm.Text = "---";
            lblMod2Comm.Text = "---";

            //set baud and port from last time run
            baudRateGPS = Settings.Default.setPort_baudRateGPS;
            portNameGPS = Settings.Default.setPort_portNameGPS;
            wasGPSConnectedLastRun = Settings.Default.setPort_wasGPSConnected;
            if (wasGPSConnectedLastRun)
            {
                OpenGPSPort();
                if (spGPS.IsOpen) lblGPS1Comm.Text = portNameGPS;
            }

            // set baud and port for rtcm from last time run
            baudRateRtcm = Settings.Default.setPort_baudRateRtcm;
            portNameRtcm = Settings.Default.setPort_portNameRtcm;
            wasRtcmConnectedLastRun = Settings.Default.setPort_wasRtcmConnected;

            if (wasRtcmConnectedLastRun)
            {
                OpenRtcmPort();
            }

            //Open IMU
            portNameIMU = Settings.Default.setPort_portNameIMU;
            wasIMUConnectedLastRun = Settings.Default.setPort_wasIMUConnected;
            if (wasIMUConnectedLastRun)
            {
                OpenIMUPort();
                if (spIMU.IsOpen) lblIMUComm.Text = portNameIMU;
            }


            //same for SteerModule port
            portNameSteerModule = Settings.Default.setPort_portNameSteer;
            wasSteerModuleConnectedLastRun = Settings.Default.setPort_wasSteerModuleConnected;
            if (wasSteerModuleConnectedLastRun)
            {
                OpenSteerModulePort();
                if (spSteerModule.IsOpen) lblMod1Comm.Text = portNameSteerModule;
            }

            //same for MachineModule port
            portNameMachineModule = Settings.Default.setPort_portNameMachine;
            wasMachineModuleConnectedLastRun = Settings.Default.setPort_wasMachineModuleConnected;
            if (wasMachineModuleConnectedLastRun)
            {
                OpenMachineModulePort();
                if (spMachineModule.IsOpen) lblMod2Comm.Text = portNameMachineModule;
            }

            ConfigureNTRIP();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + "\r\n";
                }
            }

            isConnectedIMU = cboxIsIMUModule.Checked = Properties.Settings.Default.setMod_isIMUConnected;
            isConnectedSteer = cboxIsSteerModule.Checked = Properties.Settings.Default.setMod_isSteerConnected;
            isConnectedMachine = cboxIsMachineModule.Checked = Properties.Settings.Default.setMod_isMachineConnected;
            
            SetModulesOnOff();

            oneSecondLoopTimer.Enabled = true;
            pictureBox1.Visible = true;
            pictureBox1.BringToFront();
            pictureBox1.Width = 430;
            pictureBox1.Height = 500;
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            //pictureBox1.Dock = DockStyle.Fill;

            //On or off the module rows
            SetModulesOnOff();
        }

        public void SetModulesOnOff()
        {
            if (isConnectedIMU)
            {
                btnIMU.Visible = true; 
                lblIMUComm.Visible = true;
                lblFromMU.Visible = true;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnIMU.Visible = false;
                lblIMUComm.Visible = false;
                lblFromMU.Visible = false;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedMachine)
            {
                btnMachine.Visible = true;
                lblFromMachine.Visible = true;
                lblToMachine.Visible = true;
                lblMod2Comm.Visible = true;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnMachine.Visible = false;
                lblFromMachine.Visible = false;
                lblToMachine.Visible = false;
                lblMod2Comm.Visible = false;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedSteer)
            {
                btnSteer.Visible = true;
                lblFromSteer.Visible = true;
                lblToSteer.Visible = true; 
                lblMod1Comm.Visible = true;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnSteer.Visible = false;
                lblFromSteer.Visible = false;
                lblToSteer.Visible = false;
                lblMod1Comm.Visible = false;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.AddNew;
            }

            Properties.Settings.Default.setMod_isIMUConnected = isConnectedIMU;
            Properties.Settings.Default.setMod_isSteerConnected = isConnectedSteer;
            Properties.Settings.Default.setMod_isMachineConnected = isConnectedMachine;

            Properties.Settings.Default.Save();
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { loopBackSocket.Close(); }
            }

            if (UDPSocket != null)
            {
                try
                {
                    UDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { UDPSocket.Close(); }
            }
        }

        private void oneSecondLoopTimer_Tick(object sender, EventArgs e)
        {
            if (oneSecondLoopTimer.Interval > 1200)
            {
                Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
                oneSecondLoopTimer.Interval = 1000;
                this.Width = 428;
                this.Height = 500;
                return;
            }

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            //Hello Alarm logic
            DoHelloAlarmLogic();

            DoTraffic();

            if (focusSkipCounter != 0)
            {
                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

            //send a hello to modules
            SendUDPMessage(helloFromAgIO, epModule);
            helloFromAgIO[7] = 0;

            #region Sleep

            //is this the active window
            isAppInFocus = FormLoop.ActiveForm != null;

            //start counting down to minimize
            if (!isAppInFocus && !isLostFocus)
            {
                focusSkipCounter = 310;
                isLostFocus = true;
            }

            // Is active window again
            if (isAppInFocus && isLostFocus)
            {
                isLostFocus = false;
                focusSkipCounter = int.MaxValue;
            }

            if (isLostFocus && focusSkipCounter !=0)
            {
                if (focusSkipCounter == 1)
                {
                    WindowState = FormWindowState.Minimized;
                }

                focusSkipCounter-- ;
            }

            #endregion

            //every couple or so seconds
            if ((secondsSinceStart - twoSecondTimer) > 2)
            {
                TwoSecondLoop();
                twoSecondTimer = secondsSinceStart;
            }

            //every 10 seconds
            if ((secondsSinceStart - tenSecondTimer) > 9.5)
            {
                TenSecondLoop();
                tenSecondTimer = secondsSinceStart;
            }

            //3 minute egg timer
            if ((secondsSinceStart - threeMinuteTimer) > 180)
            {
                ThreeMinuteLoop();
                threeMinuteTimer = secondsSinceStart;
            }

            // 1 Second Loop Part2 
            if (isViewAdvanced)
            {
                if (isNTRIP_RequiredOn)
                {
                    sbRTCM.Append(".");
                    lblMessages.Text = sbRTCM.ToString();
                }
                btnResetTimer.Text = ((int)(180 - (secondsSinceStart - threeMinuteTimer))).ToString();
            }
        }

        private void TwoSecondLoop()
        {
            if (isLogNMEA)
            {
                using (StreamWriter writer = new StreamWriter("zAgIO_log.txt", true))
                {
                    writer.Write(logNMEASentence.ToString());
                }
                logNMEASentence.Clear();
            }

            if (focusSkipCounter < 310) lblSkipCounter.Text = focusSkipCounter.ToString();
            else lblSkipCounter.Text = "On";

        }

        private void TenSecondLoop()
        {
            if (focusSkipCounter != 0 && WindowState == FormWindowState.Minimized)
            {
                focusSkipCounter = 0;
                isLostFocus = true;
            }

            if (focusSkipCounter != 0)
            {
                if (isViewAdvanced && isNTRIP_RequiredOn)
                {
                    try
                    {
                        //add the uniques messages to all the new ones
                        foreach (var item in aList)
                        {
                                rList.Add(item);
                        }

                        //sort and group using Linq
                        sbRTCM.Clear();

                        var g = rList.GroupBy(i => i)
                            .OrderBy(grp => grp.Key);
                        int count = 0;
                        aList.Clear();

                        //Create the text box of unique message numbers
                        foreach (var grp in g)
                        {
                            aList.Add(grp.Key);
                            sbRTCM.AppendLine(grp.Key + " - " + (grp.Count()-1));
                            count++;
                        }

                        rList?.Clear();

                        //too many messages or trash
                        if (count > 25)
                        {
                            aList?.Clear();
                            sbRTCM.Clear();
                            sbRTCM.Append("Reset..");
                        }

                        lblMessagesFound.Text = count.ToString();
                    }

                    catch
                    {
                        sbRTCM.Clear();
                        sbRTCM.Append("Error");
                    }
                }

                #region Serial update

                if (wasIMUConnectedLastRun)
                {
                    if (!spIMU.IsOpen)
                    {
                        byte[] imuClose = new byte[] { 0x80, 0x81, 0x7C, 0xD4, 2, 1, 0, 83 };

                        //tell AOG IMU is disconnected
                        SendToLoopBackMessageAOG(imuClose);
                        wasIMUConnectedLastRun = false;
                        lblIMUComm.Text = "---";
                    }
                }

                if (wasGPSConnectedLastRun)
                {
                    if (!spGPS.IsOpen)
                    {
                        wasGPSConnectedLastRun = false;
                        lblGPS1Comm.Text = "---";
                    }
                }

                if (wasSteerModuleConnectedLastRun)
                {
                    if (!spSteerModule.IsOpen)
                    {
                        wasSteerModuleConnectedLastRun = false;
                        lblMod1Comm.Text = "---";
                    }
                }

                if (wasMachineModuleConnectedLastRun)
                {
                    if (!spMachineModule.IsOpen)
                    {
                        wasMachineModuleConnectedLastRun = false;
                        lblMod2Comm.Text = "---";
                    }
                }

                #endregion
            }
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (this.Width < 600)
            {
                this.Width = 700;
                isViewAdvanced = true;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnLeft;
                sbRTCM.Clear();
                lblMessages.Text = "Reading...";
                threeMinuteTimer = secondsSinceStart;
                lblMessagesFound.Text = "-";
                aList.Clear();  
                rList.Clear();

            }
            else
            {
                this.Width = 428;
                isViewAdvanced = false;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnRight;
                aList.Clear();
                rList.Clear();
                lblMessages.Text = "Reading...";
                lblMessagesFound.Text = "-";
                aList.Clear();
                rList.Clear();
            }
        }

        private void ThreeMinuteLoop()
        {
            if (isViewAdvanced)
            {
                btnSlide.PerformClick();
            }
        }

        private void DoHelloAlarmLogic()
        {
            bool currentHello;

            if (isConnectedMachine)
            {
                currentHello = traffic.helloFromMachine < 3;

                if (currentHello != lastHelloMachine)
                {
                    if (currentHello) btnMachine.BackColor = Color.LimeGreen;
                    else btnMachine.BackColor = Color.Red;
                    lastHelloMachine = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedSteer)
            {
                currentHello = traffic.helloFromAutoSteer < 3;

                if (currentHello != lastHelloAutoSteer)
                {
                    if (currentHello) btnSteer.BackColor = Color.LimeGreen;
                    else btnSteer.BackColor = Color.Red;
                    lastHelloAutoSteer = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedIMU)
            {
                currentHello = traffic.helloFromIMU < 3;

                if (currentHello != lastHelloIMU)
                {
                    if (currentHello) btnIMU.BackColor = Color.LimeGreen;
                    else btnIMU.BackColor = Color.Red;
                    lastHelloIMU = currentHello;
                    ShowAgIO();
                }
            }

            currentHello = traffic.cntrGPSOut != 0;

            if (currentHello != lastHelloGPS)
            {
                if (currentHello) btnGPS.BackColor = Color.LimeGreen;
                else btnGPS.BackColor = Color.Red;
                lastHelloGPS = currentHello;
                ShowAgIO();
            }
        }

        private void FormLoop_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                if (isViewAdvanced) btnSlide.PerformClick();
                isLostFocus = true;
                focusSkipCounter = 0;
            }
        }

        private void ShowAgIO()
        {
            Process[] processName = Process.GetProcessesByName("AgIO");
            
            if (processName.Length != 0)
            {
                // Guard: check if window already has focus.
                if (processName[0].MainWindowHandle == GetForegroundWindow()) return;

                // Show window maximized.
                ShowWindow(processName[0].MainWindowHandle, 9);

                // Simulate an "ALT" key press.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

                // Simulate an "ALT" key release.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

                // Show window in forground.
                SetForegroundWindow(processName[0].MainWindowHandle);
            }  
            
            //{
            //    //Set foreground window
            //    if (IsIconic(processName[0].MainWindowHandle))
            //    {
            //        ShowWindow(processName[0].MainWindowHandle, 9);
            //    }
            //    SetForegroundWindow(processName[0].MainWindowHandle);
            //}
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;
            traffic.helloFromIMU++;

            if (focusSkipCounter != 0)
            {

                lblFromGPS.Text = traffic.cntrGPSOut == 0 ? "--" : (traffic.cntrGPSOut).ToString();

                if (isConnectedSteer)
                {
                    lblToSteer.Text = traffic.cntrSteerIn == 0 ? "--" : (traffic.cntrSteerIn).ToString();
                    lblFromSteer.Text = traffic.cntrSteerOut == 0 ? "--" : (traffic.cntrSteerOut).ToString();
                }

                if (isConnectedMachine)
                {
                    lblToMachine.Text = traffic.cntrMachineIn == 0 ? "--" : (traffic.cntrMachineIn).ToString();
                    lblFromMachine.Text = traffic.cntrMachineOut == 0 ? "--" : (traffic.cntrMachineOut).ToString();
                }

                if (isConnectedIMU)
                lblFromMU.Text = traffic.cntrIMUOut == 0 ? "--" : (traffic.cntrIMUOut).ToString();

                //reset all counters
                traffic.cntrPGNToAOG = traffic.cntrPGNFromAOG = traffic.cntrGPSOut =
                    traffic.cntrIMUOut = traffic.cntrSteerIn = traffic.cntrSteerOut =
                    traffic.cntrMachineOut = traffic.cntrMachineIn = 0;

                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }
        }

        // Buttons, Checkboxes and Clicks  ***************************************************

        private void RescanPorts()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + " ";
                }
            }
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void cboxIsSteerModule_Click(object sender, EventArgs e)
        {
            isConnectedSteer = cboxIsSteerModule.Checked;
            SetModulesOnOff();  
        }

        private void cboxIsMachineModule_Click(object sender, EventArgs e)
        {
            isConnectedMachine = cboxIsMachineModule.Checked;
            SetModulesOnOff();
        }

        private void lblMessages_Click(object sender, EventArgs e)
        {
            aList?.Clear();
            sbRTCM.Clear();
            sbRTCM.Append("Reset..");
        }

        private void btnResetTimer_Click(object sender, EventArgs e)
        {
            threeMinuteTimer = secondsSinceStart;
        }

        private void serialPassThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRadio_RequiredOn)
            {
                TimedMessageBox(2000, "Radio NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            if (isNTRIP_RequiredOn)
            {
                TimedMessageBox(2000, "Air NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            using (var form = new FormSerialPass(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ////Clicked Save
                    //Application.Restart();
                    //Environment.Exit(0);
                }
            }
        }

        private void btnRelayTest_Click(object sender, EventArgs e)
        {
                helloFromAgIO[7] = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void lblNTRIPBytes_Click(object sender, EventArgs e)
        {
            tripBytes = 0;
        }

        private void cboxIsIMUModule_Click(object sender, EventArgs e)
        {
            isConnectedIMU = cboxIsIMUModule.Checked;
            SetModulesOnOff();
        }

        private void btnBringUpCommSettings_Click(object sender, EventArgs e)
        {
            SettingsCommunicationGPS();
            RescanPorts();
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            SettingsUDP();
        }

        private void btnRunAOG_Click(object sender, EventArgs e)
        {
            StartAOG();
        }

        private void btnNTRIP_Click(object sender, EventArgs e)
        {
            SettingsNTRIP();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void btnRadio_Click_1(object sender, EventArgs e)
        {
            SettingsRadio();
        }

        private void btnWindowsShutDown_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Shutdown Windows For Realz ?",
                "For Sure For Sure ?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private void toolStripGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void cboxLogNMEA_CheckedChanged(object sender, EventArgs e)
        {
            isLogNMEA = cboxLogNMEA.Checked;
        }

    }
}

