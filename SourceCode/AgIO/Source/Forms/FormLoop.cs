using AgIO.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Speech.Recognition;
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

        //used to send communication check pgn= C8 or 200
        private byte[] helloFromAgIO = { 0x80, 0x81, 0x7F, 200, 1, 1, 0x47 };

        public bool isLogNMEA;
        public bool isKeyboardOn = true;

        public bool isSendToSerial = false, isSendToUDP = false;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        public double secondsSinceStart, lastSecond;

        public string lastSentence;

        public bool isPluginUsed;

        public int packetSizeNTRIP;

        //The base directory where Drive will be stored and fields and vehicles branch from
        public string baseDirectory;

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            //get the fields directory, if not exist, create
            commDirectory = baseDirectory + "AgIO\\";
            string dir = Path.GetDirectoryName(commDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            if (Settings.Default.setUDP_isOn) LoadUDPNetwork();
            LoadLoopback();

            packetSizeNTRIP = Properties.Settings.Default.setNTRIP_packetSize;

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;
            isPluginUsed = Properties.Settings.Default.setUDP_isUsePluginApp;

            isSendToSerial = Settings.Default.setNTRIP_sendToSerial;
            isSendToUDP = Settings.Default.setNTRIP_sendToUDP;

            lblGPS1Comm.Text = "---";

            //set baud and port from last time run
            baudRateGPS = Settings.Default.setPort_baudRateGPS;
            portNameGPS = Settings.Default.setPort_portNameGPS;
            wasGPSConnectedLastRun = Settings.Default.setPort_wasGPSConnected;
            if (wasGPSConnectedLastRun)
            {
                OpenGPSPort();

                if (spGPS.IsOpen)
                {
                    lblGPS1Comm.Text = portNameGPS;
                }
            }

            // set baud and port for rtcm from last time run
            baudRateRtcm = Settings.Default.setPort_baudRateRtcm;
            portNameRtcm = Settings.Default.setPort_portNameRtcm;
            wasRtcmConnectedLastRun = Settings.Default.setPort_wasRtcmConnected;
            
            if (wasRtcmConnectedLastRun)
            {
                OpenRtcmPort();
            }

            ConfigureNTRIP();

            lastSentence = Properties.Settings.Default.setGPS_lastSentence;

            timer1.Enabled = true;
            //panel1.Visible = false;
            pictureBox1.BringToFront();
            pictureBox1.Dock = DockStyle.Fill;
        }

        //current directory of Comm storage
        public string commDirectory, commFileName = "";

        private void btnDeviceManager_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval > 1000)
            {
                Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
                //panel1.Visible = true;
                timer1.Interval = 1000;
                return;
            }

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            DoTraffic();

            //send a hello to modules
            SendUDPMessage(helloFromAgIO, epModule);

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

            //send back to Drive proof of life
            //every 3 seconds
            if ((secondsSinceStart - lastSecond) > 2)
            {
                if (traffic.helloFromMachine < 3) btnMachine.BackColor = Color.LightGreen;
                else btnMachine.BackColor = Color.Orange;

                if (traffic.helloFromAutoSteer < 3) btnSteer.BackColor = Color.LightGreen;
                else btnSteer.BackColor = Color.Orange;

                if (isLogNMEA)
                {
                    using (StreamWriter writer = new StreamWriter("zAgIO_log.txt", true))
                    {
                        writer.Write(VehicleGPS.logNMEASentence.ToString());
                    }
                    VehicleGPS.logNMEASentence.Clear();
                    using (StreamWriter writer = new StreamWriter("zAgIO_Tool_log.txt", true))
                    {
                        writer.Write(ToolGPS.logNMEASentence.ToString());
                    }
                    ToolGPS.logNMEASentence.Clear();
                }

                lastSecond = secondsSinceStart;

                if (wasGPSConnectedLastRun)
                {
                    if (!spGPS.IsOpen)
                    {
                        wasGPSConnectedLastRun = false;
                        lblGPS1Comm.Text = "---";
                    }
                }
            }
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;

            lblToAOG.Text = traffic.cntrPGNToAOG == 0 ? "--" : (traffic.cntrPGNToAOG).ToString();
            lblFromAOG.Text = traffic.cntrPGNFromAOG == 0 ? "--" : (traffic.cntrPGNFromAOG).ToString();

            lblFromGPS.Text = traffic.cntrFromGPS == 0 ? "--" : (traffic.cntrFromGPS).ToString();

            lblToGPS2.Text = traffic.cntrToGPS2 == 0 ? "--" : (traffic.cntrToGPS2).ToString();
            lblFromGPS2.Text = traffic.cntrFromGPS2 == 0 ? "--" : (traffic.cntrFromGPS2).ToString();

            lblToSteer.Text = traffic.cntrToSteer == 0 ? "--" : (traffic.cntrToSteer).ToString();
            lblFromSteer.Text = traffic.cntrFromSteer == 0 ? "--" : (traffic.cntrFromSteer).ToString();

            lblToMachine.Text = traffic.cntrToMachine == 0 ? "--" : (traffic.cntrToMachine).ToString();
            lblFromMachine.Text = traffic.cntrFromMachine == 0 ? "--" : (traffic.cntrFromMachine).ToString();

            lblToModule3.Text = traffic.cntrToModule3 == 0 ? "--" : (traffic.cntrToModule3).ToString();
            lblFromModule3.Text = traffic.cntrFromModule3 == 0 ? "--" : (traffic.cntrFromModule3).ToString();

            if (traffic.cntrFromGPS > 0) btnGPS.BackColor = Color.LightGreen;
            else btnGPS.BackColor = Color.Orange;

            if (traffic.cntrPGNFromAOG > 0 && traffic.cntrPGNToAOG > 0) btnAOGButton.BackColor = Color.LightGreen;
            else btnAOGButton.BackColor = Color.Orange;

            traffic.cntrPGNToAOG = traffic.cntrPGNFromAOG = //traffic.cntrUDPIn = traffic.cntrUDPOut =
                traffic.cntrFromGPS = traffic.cntrFromGPS2 = traffic.cntrToGPS2 =
                traffic.cntrToModule3 = traffic.cntrFromModule3 =
                traffic.cntrToSteer = traffic.cntrFromSteer =
                traffic.cntrFromMachine = traffic.cntrToMachine = 0;

            lblCurentLon.Text = VehicleGPS.longitude.ToString("N7");
            lblCurrentLat.Text = VehicleGPS.latitude.ToString("N7");

            if (traffic.cntrToGPSMessages > 9999) traffic.cntrToGPSMessages = 0;
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void btnBringUpCommSettings_Click(object sender, EventArgs e)
        {
            SettingsCommunicationGPS();
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

        public void ConfigureNTRIP()
        {
            lblWatch.Text = "Wait GPS";

            //start NTRIP if required
            isNTRIP_RequiredOn = Settings.Default.setNTRIP_isOn;
            isRadio_RequiredOn = Settings.Default.setRadio_isOn;

            if (isRadio_RequiredOn)
            {
                // Immediatly connect radio
                ntripCounter = 20;
            }

            if (isNTRIP_RequiredOn || isRadio_RequiredOn)
            {
                btnStartStopNtrip.Visible = true;
                btnStartStopNtrip.Visible = true;
                lblWatch.Visible = true;
                lblNTRIPBytes.Visible = true;
                lblToGPS.Visible = true;
            }
            else
            {
                btnStartStopNtrip.Visible = false;
                btnStartStopNtrip.Visible = false;
                lblWatch.Visible = false;
                lblNTRIPBytes.Visible = false;
                lblToGPS.Visible = false;
            }

            btnStartStopNtrip.Text = "Off";
        }

        private void uDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsUDP();
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

        private void toolStripGPSData_Click(object sender, EventArgs e)
        {
            SettingsRadio();
        }

        private void toolStripAgDiag_Click(object sender, EventArgs e)
        {
            Process[] processName = Process.GetProcessesByName("AgDiag");
            if (processName.Length == 0)
            {
                //Start application here
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                string strPath = di.ToString();
                strPath += "\\AgDiag.exe";
                //TimedMessageBox(8000, "No File Found", strPath);

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = strPath,
                        //processInfo.ErrorDialog = true;
                        //processInfo.UseShellExecute = false;
                        WorkingDirectory = Path.GetDirectoryName(strPath)
                    };
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgDiag");
                }
            }
            else
            {
                //Set foreground window

                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);

            }

        }

        private void cboxLogNMEA_CheckedChanged(object sender, EventArgs e)
        {
            isLogNMEA = cboxLogNMEA.Checked;
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setPort_wasGPSConnected = wasGPSConnectedLastRun;
            Properties.Settings.Default.setPort_wasRtcmConnected = wasRtcmConnectedLastRun;
            Properties.Settings.Default.Save();

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
    }
}

