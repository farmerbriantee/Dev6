using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        private Socket NtripSocket;                      // Server connection
        private byte[] casterRecBuffer = new byte[256];    // Recieved data buffer

        private int NTRIP_Counter = 0;
        private int NTRIP_SendGGACounter, NTRIP_sendGGAInterval = 0;

        private int NTRIP_Watchdog = 100, NTRIP_UDPPort = 7777;
        private long NTRIP_byteCount = 100;

        public bool NTRIP_TurnedOn = false;
        private bool NTRIP_Connecting = false;
        private bool NTRIP_Authorized = false;
        private bool NTRIP_Connected = false;
        private bool NTRIP_Sending = false;

        public void UpdateNtripButton(bool state)
        {
            NTRIP_Counter = 15;
            NTRIP_Connecting = false;
            NTRIP_Authorized = false;
            NTRIP_Connected = false;
            NTRIP_Sending = false;
            NTRIP_TurnedOn = state;

            if (NtripSocket != null && NtripSocket.Connected)
            {
                NtripSocket.Shutdown(SocketShutdown.Both);
                NtripSocket.Close();
            }

            if (!NTRIP_TurnedOn)
            {
                btnStartAgIO.Text = "Off";
            }
        }

        //set up connection to Caster
        public void StartNTRIP()
        {
            NTRIP_sendGGAInterval = Properties.Settings.Default.NTRIP_GGAInterval;
            string broadCasterIP = Properties.Settings.Default.NTRIP_casterIP; //Select correct Address
            NTRIP_UDPPort = Properties.Settings.Default.NTRIP_UDPPort; //send rtcm to which udp port

            string actualIP = Properties.Settings.Default.NTRIP_casterURL.Trim();

            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                broadCasterIP = addresslist[0].ToString().Trim();
            }
            catch (Exception)
            {
            }

            try
            {
                // Close the socket if it is still open
                if (NtripSocket != null && NtripSocket.Connected)
                {
                    NtripSocket.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(100);
                    NtripSocket.Close();
                }

                // Create the socket object
                NtripSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Define the Server address and port
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(broadCasterIP), Properties.Settings.Default.NTRIP_casterPort);

                NtripSocket.Blocking = false;
                NTRIP_Connecting = true;
                NTRIP_Watchdog = 0;
                NTRIP_SendGGACounter = 0;
                NTRIP_byteCount = 0;
                NtripSocket.BeginConnect(epServer, new AsyncCallback(RecievedNtripData), null);

            }
            catch (Exception)
            {
            }
        }

        private void SendGGA()
        {
            //timer may have brought us here so return if not connected
            if (NTRIP_Connected && (NTRIP_Connecting || NTRIP_Authorized))
            {
                try
                {
                    NTRIP_SendGGACounter = 0;
                    NTRIP_Sending = true;
                    string str = BuildGGA().ToString();

                    byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                    NtripSocket.Send(byteDateLine, byteDateLine.Length, 0);
                }
                catch (Exception)
                {
                }
            }
        }

        public void RecievedNtripData(IAsyncResult asyncResult)
        {
            try
            {
                if (NtripSocket == null || !NtripSocket.Connected)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        UpdateNtripButton(false);
                    });
                    return;
                }
                else if (NTRIP_Connecting)
                {
                    NTRIP_Watchdog = 0;
                    NTRIP_Connected = true;

                    if (!Properties.Settings.Default.NTRIP_isTCP)
                    {
                        //encode user and password
                        byte[] byteArray = Encoding.ASCII.GetBytes(Properties.Settings.Default.NTRIP_userName + ":" + Properties.Settings.Default.NTRIP_Password);
                        string auth = Convert.ToBase64String(byteArray, 0, byteArray.Length);

                        //Build authorization string
                        string str = "GET /" + Properties.Settings.Default.NTRIP_Mount + " HTTP/" + (Properties.Settings.Default.NTRIP_isHTTP10 ? "1.0" : "1.1") + "\r\n";
                        str += "User-Agent: NTRIP LefebureNTRIPClient/20131124\r\n";
                        str += "Authorization: Basic " + auth + "\r\n"; //This line can be removed if no authorization is needed
                        //str += GGASentence; //this line can be removed if no position feedback is needed

                        str += "Accept: */*\r\nConnection: close\r\n";

                        str += "\r\n";

                        // Convert to byte array and send.
                        byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                        NtripSocket.Send(byteDateLine, byteDateLine.Length, 0);

                        //enable to send GGA sentence to server.
                        if (NTRIP_sendGGAInterval > 0)
                        {
                            SendGGA();
                        }
                    }

                    NTRIP_Connecting = false;

                    NtripSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(RecievedNtripData), null);
                }
                else if (NTRIP_Connected)
                {
                    int nBytesRec = NtripSocket.EndReceive(asyncResult);
                    if (nBytesRec > 0)
                    {
                        NTRIP_Watchdog = 0;

                        byte[] localMsg = new byte[nBytesRec];
                        Array.Copy(casterRecBuffer, localMsg, nBytesRec);

                        NTRIP_byteCount += nBytesRec;

                        if (!NTRIP_Authorized)
                        {
                            string Message = Encoding.Default.GetString(localMsg);
                            
                            if (Message.IndexOf("SOURCETABLE") > -1)
                            {
                                NTRIP_TurnedOn = false;
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    UpdateNtripButton(false);
                                });

                                MessageBox.Show(Message,
                                "Critical Settings Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                return;
                            }
                            else if (Message.IndexOf("200 OK") < 0)
                            {
                                if (Message.IndexOf("401 Unauthorized") > -1)
                                {
                                    NTRIP_TurnedOn = false;

                                    BeginInvoke((MethodInvoker)delegate
                                    {
                                        UpdateNtripButton(false);
                                    });

                                    MessageBox.Show(Message,
                                    "Critical Settings Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                    return;
                                }
                                else NTRIP_Authorized = true;//for reply that doesnt send ok or unautorized
                            }
                            else NTRIP_Authorized = true;

                            // If the connection is still usable restablish the callback
                            NtripSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(RecievedNtripData), null);
                        }
                        else
                        {
                            SendPgnToLoop(localMsg, NTRIP_UDPPort);

                            // If the connection is still usable restablish the callback
                            NtripSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(RecievedNtripData), null);
                        }
                    }
                    else
                    {
                        // If no data was recieved then the connection is probably dead
                        Console.WriteLine("Client {0}, disconnected", NtripSocket.RemoteEndPoint);
                        NtripSocket.Shutdown(SocketShutdown.Both);
                        NtripSocket.Close();
                        BeginInvoke((MethodInvoker)delegate
                        {
                            UpdateNtripButton(false);
                        });
                    }
                }
            }
            catch (Exception)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    UpdateNtripButton(false);
                });
            }
        }

        private StringBuilder BuildGGA()
        {
            double lat;
            double lon;
            if (Properties.Settings.Default.NTRIP_isGGAManual)
            {
                lat = Properties.Settings.Default.NTRIP_manualLat;
                lon = Properties.Settings.Default.NTRIP_manualLon;
            }
            else
            {
                lat = pn.latitude;
                lon = pn.longitude;
            }

            //convert to DMS from Degrees
            double latMinu = lat;
            double longMinu = lon;

            double latDeg = (int)lat;
            double longDeg = (int)lon;

            latMinu -= latDeg;
            longMinu -= longDeg;

            latMinu = Math.Round(latMinu * 60.0, 7);
            longMinu = Math.Round(longMinu * 60.0, 7);

            latDeg *= 100.0;
            longDeg *= 100.0;

            double latNMEA = latMinu + latDeg;
            double longNMEA = longMinu + longDeg;

            char NS = lat >= 0 ? 'N' : 'S';
            char EW = lon >= 0 ? 'E' : 'W';

            StringBuilder sbGGA = new StringBuilder();
            sbGGA.Append("$GPGGA,");
            sbGGA.Append(DateTime.Now.ToString("HHmmss.00,", CultureInfo.InvariantCulture));
            sbGGA.Append(Math.Abs(latNMEA).ToString("0000.000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbGGA.Append(Math.Abs(longNMEA).ToString("00000.000", CultureInfo.InvariantCulture)).Append(',').Append(EW);
            sbGGA.Append(",1,10,1,43.4,M,46.4,M,5,0*");

            sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
            sbGGA.Append("\r\n");
            /*
        $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,5,0*47
           0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
                Time      Lat       Lon     FixSatsOP Alt */
            return sbGGA;
        }

        //calculate the NMEA checksum to stuff at the end
        public string CalculateChecksum(string Sentence)
        {
            int sum = 0, inx;
            char[] sentence_chars = Sentence.ToCharArray();
            char tmp;
            // All character xor:ed results in the trailing hex checksum
            // The checksum calc starts after '$' and ends before '*'
            for (inx = 1; ; inx++)
            {
                tmp = sentence_chars[inx];
                // Indicates end of data and start of checksum
                if (tmp == '*')
                    break;
                sum ^= tmp;    // Build checksum
            }
            // Calculated checksum converted to a 2 digit hex string
            return string.Format("{0:X2}", sum);
        }
    }
}