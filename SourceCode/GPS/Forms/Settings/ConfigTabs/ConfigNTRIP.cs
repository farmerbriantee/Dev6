using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigNTRIP : UserControl2
    {
        private readonly FormGPS mf;

        internal UserControl2 currentTab;
        private int casterPort = 2101, UDPPort = 7777, GGAInterval = 15;
        private double Longitude = 0, Latitude = 0;

        public ConfigNTRIP(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, System.EventArgs e)
        {
            string hostName = Dns.GetHostName(); // Retrieve the Name of HOST
            tboxHostName.Text = hostName;
            tboxThisIP.Text = GetIP4Address();

            tboxCasterIP.Text = Properties.Settings.Default.NTRIP_casterIP;
            tboxEnterURL.Text = Properties.Settings.Default.NTRIP_casterURL;

            casterPort = Properties.Settings.Default.NTRIP_casterPort;
            nudCasterPort.Text = casterPort.ToString();
            UDPPort = Properties.Settings.Default.NTRIP_UDPPort;
            nudSendToUDPPort.Text = UDPPort.ToString();
            GGAInterval = Properties.Settings.Default.NTRIP_GGAInterval;
            nudGGAInterval.Text = GGAInterval.ToString();
            Latitude = Properties.Settings.Default.NTRIP_manualLat;
            nudLatitude.Text = Latitude.ToString();
            Longitude = Properties.Settings.Default.NTRIP_manualLon;
            nudLongitude.Text = Longitude.ToString();

            tboxUserName.Text = Properties.Settings.Default.NTRIP_userName;
            tboxUserPassword.Text = Properties.Settings.Default.NTRIP_Password;
            tboxMount.Text = Properties.Settings.Default.NTRIP_Mount;
            checkBoxusetcp.Checked = Properties.Settings.Default.NTRIP_isTCP;

            if (Properties.Settings.Default.NTRIP_isGGAManual) cboxGGAManual.Text = "Use Manual Fix";
            else cboxGGAManual.Text = "Use GPS Fix";

            if (Properties.Settings.Default.NTRIP_isHTTP10) cboxHTTP.Text = "1.0";
            else cboxHTTP.Text = "1.1";
        }

        public override void Close()
        {
            Properties.Settings.Default.NTRIP_casterURL = tboxEnterURL.Text;
            Properties.Settings.Default.NTRIP_casterIP = tboxCasterIP.Text;
            Properties.Settings.Default.NTRIP_casterPort = casterPort;
            Properties.Settings.Default.NTRIP_UDPPort = UDPPort;

            Properties.Settings.Default.NTRIP_userName = tboxUserName.Text;
            Properties.Settings.Default.NTRIP_Password = tboxUserPassword.Text;
            Properties.Settings.Default.NTRIP_Mount = tboxMount.Text;

            Properties.Settings.Default.NTRIP_GGAInterval = GGAInterval;
            Properties.Settings.Default.NTRIP_isGGAManual = cboxGGAManual.Text == "Use Manual Fix";
            Properties.Settings.Default.NTRIP_manualLat = Latitude;
            Properties.Settings.Default.NTRIP_manualLon = Longitude;

            Properties.Settings.Default.NTRIP_isTCP = checkBoxusetcp.Checked;
            Properties.Settings.Default.NTRIP_isHTTP10 = cboxHTTP.Text == "1.0";
            Properties.Settings.Default.Save();

            mf.UpdateNtripButton(mf.NTRIP_TurnedOn);//reset
        }

        private void tboxEnterURL_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                using (FormKeyboard form = new FormKeyboard(tboxEnterURL.Text))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tboxEnterURL.Text = form.ReturnString;
                    }
                }
            }
        }

        private void tboxMount_TextChanged(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                using (FormKeyboard form = new FormKeyboard(tboxMount.Text))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tboxMount.Text = form.ReturnString;
                    }
                }
            }
        }

        private void btnGetSourceTable_Click(object sender, EventArgs e)
        {
            IPAddress casterIP = IPAddress.Parse(tboxCasterIP.Text.Trim()); //Select correct Address

            Socket sckt;
            List<string> dataList = new List<string>();

            try
            {
                sckt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    Blocking = true
                };
                sckt.Connect(new IPEndPoint(casterIP, casterPort));

                string msg = "GET / HTTP/1.0\r\n" + "User-Agent: NTRIP iter.dk\r\n" +
                                    "Accept: */*\r\nConnection: close\r\n" + "\r\n";

                //Send request
                byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                sckt.Send(data);
                int bytes = 0;
                byte[] bytesReceived = new byte[1024];
                string page = string.Empty;
                Thread.Sleep(200);

                do
                {
                    bytes = sckt.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                    page += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);

                if (page.Length > 0)
                {
                    string[] words = page.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < words.Length; i++)
                    {
                        string[] words2 = words[i].Split(';');

                        if (words2[0] == "STR")
                        {
                            dataList.Add(words2[1].Trim().ToString() + "," + words2[9].ToString() + "," + words2[10].ToString()
                          + "," + words2[3].Trim().ToString() + "," + words2[6].Trim().ToString()
                                );
                        }
                    }
                }
            }
            catch (SocketException)
            {
                mf.TimedMessageBox(2000, "Socket Exception", "Invalid IP:Port");
                return;
            }
            catch (Exception)
            {
                mf.TimedMessageBox(2000, "Exception", "Get Source Table Error");
                return;
            }

            if (dataList.Count > 0)
            {
                string syte = "http://monitor.use-snip.com/?hostUrl=" + tboxCasterIP.Text + "&port=" + casterPort.ToString();

                currentTab = new ConfigSourceData(this, dataList, mf.pn.latitude, mf.pn.longitude, syte);
                Controls.Add(currentTab);
                currentTab.BringToFront();
                currentTab.Dock = DockStyle.Fill;
            }
            else
            {
                mf.TimedMessageBox(2000, "Error", "No Source Data");
            }
        }

        private void tboxUserName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                using (FormKeyboard form = new FormKeyboard(tboxUserName.Text))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tboxUserName.Text = form.ReturnString;
                    }
                }
            }
        }

        private void tboxUserPassword_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                using (FormKeyboard form = new FormKeyboard(tboxUserPassword.Text))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tboxUserPassword.Text = form.ReturnString;
                    }
                }
            }
        }

        private void btnPassPassword_Click(object sender, EventArgs e)
        {
            if (tboxUserPassword.PasswordChar == '*') tboxUserPassword.PasswordChar = '\0';
            else tboxUserPassword.PasswordChar = '*';
            tboxUserPassword.Invalidate();
        }

        private void btnPassUsername_Click(object sender, EventArgs e)
        {
            if (tboxUserName.PasswordChar == '*') tboxUserName.PasswordChar = '\0';
            else tboxUserName.PasswordChar = '*';
            tboxUserName.Invalidate();
        }

        private void btnGetIP_Click(object sender, EventArgs e)
        {
            string actualIP = tboxEnterURL.Text.Trim();
            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                tboxCasterIP.Text = "";
                tboxCasterIP.Text = addresslist[0].ToString().Trim();
            }
            catch (Exception)
            {
                mf.TimedMessageBox(1500, "No IP Located", "Can't Find: " + actualIP);
            }
        }

        private void nudCasterPort_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudCasterPort, ref casterPort, 1, 65535);
        }

        private void nudSendToUDPPort_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudSendToUDPPort, ref UDPPort, 1, 65535);
        }

        private void nudGGAInterval_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudGGAInterval, ref UDPPort, 0, 600);
        }

        private void tboxCasterIP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckIPValid(tboxCasterIP.Text))
            {
                tboxCasterIP.Text = "127.0.0.1";
                tboxCasterIP.Focus();
                mf.TimedMessageBox(2000, "Invalid IP Address", "Set to Default Local 127.0.0.1");
            }
        }

        private void btnSetManualPosition_Click(object sender, EventArgs e)
        {
            Latitude = Math.Round(mf.pn.latitude, 7);
            nudLatitude.Text = Latitude.ToString();
            Longitude = Math.Round(mf.pn.longitude, 7);
            nudLongitude.Text = Longitude.ToString();
        }

        private void nudLongitude_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudLongitude, ref Longitude, -180, 180, 7);
        }

        private void nudLatitude_Click(object sender, EventArgs e)
        {
            mf.KeypadToButton(ref nudLatitude, ref Latitude, -90, 90, 7);
        }

        //get the ipv4 address only
        public static string GetIP4Address()
        {
            string IP4Address = string.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        public bool CheckIPValid(string strIP)
        {
            // Return true for COM Port
            if (strIP.Contains("COM"))
            {
                return true;
            }

            //  Split string by ".", check that array length is 3
            string[] arrOctets = strIP.Split('.');

            //at least 4 groups in the IP
            if (arrOctets.Length != 4) return false;

            //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
            const short MAXVALUE = 255;
            int temp; // Parse returns Int32
            foreach (string strOctet in arrOctets)
            {
                //check if at least 3 digits but not more OR 0 length
                if (strOctet.Length > 3 || strOctet.Length == 0) return false;

                //make sure all digits
                if (!int.TryParse(strOctet, out int temp2)) return false;

                //make sure not more then 255
                temp = int.Parse(strOctet);
                if (temp > MAXVALUE || temp < 0) return false;
            }
            return true;
        }
    }
}
