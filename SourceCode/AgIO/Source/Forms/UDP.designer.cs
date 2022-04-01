using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgIO
{
    public class CTraffic
    {
        public int cntrPGNFromAOG = 0;
        public int cntrPGNToAOG = 0;
     
        public int cntrUDPOut = 0;
        public int cntrUDPIn = 0;

        public int cntrGPSIn = 0;
        public int cntrGPSOut = 0;

        public int cntrGPS2In = 0;
        public int cntrGPS2Out = 0;

        public int cntrIMUIn = 0;
        public int cntrIMUOut = 0;

        public int cntrModule1In = 0;
        public int cntrModule1Out = 0;

        public int cntrModule2In = 0;
        public int cntrModule2Out = 0;

        public int cntrModule3In = 0;
        public int cntrModule3Out = 0;

        public bool isTrafficOn = true;
    }

    public partial class FormLoop
    {
        // Sockets
        private Socket loopBackSocket;
        private Socket UDPSocket;
        private bool isUDPNetworkConnected;

        //2 endpoints for local and udp
        private IPEndPoint epAgOpen = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 15555);
        private IPEndPoint epAgVR = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 16666);
        private IPEndPoint epModule;
        private IPEndPoint epNtrip;

        // Initialise the IPEndPoint for async listener!
        EndPoint epUDPSender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint epLoopSender = new IPEndPoint(IPAddress.Loopback, 0);

        //class for counting bytes
        public CTraffic traffic = new CTraffic();

        // Data stream
        private byte[] buffer = new byte[1024];

        //IP address and port of Auto Steer server
        IPAddress epIP = IPAddress.Parse(Properties.Settings.Default.setIP_autoSteerIP);

        //initialize loopback and udp network
        private void LoadLoopback()
        {
            try //loopback
            {
                // Initialise the socket
                loopBackSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                loopBackSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                loopBackSocket.Bind(new IPEndPoint(IPAddress.Loopback, 17777));
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epLoopSender, new AsyncCallback(ReceiveDataLoopAsync), null);
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error";
                MessageBox.Show("Load Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUDPNetwork()
        {
            try //udp network
            {
                // Initialise the socket
                UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                UDPSocket.Bind(new IPEndPoint(IPAddress.Any, 9999));
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epUDPSender, new AsyncCallback(ReceiveDataUDPAsync), null);

                // AgIO sends to this endpoint - usually 192.168.1.255:8888
                epModule = new IPEndPoint(epIP, 8888);

                isUDPNetworkConnected = true;
            }
            catch (Exception e)
            {
                //WriteErrorLog("UDP Server" + e);
                MessageBox.Show("Load Error: " + e.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //loopback functions
        #region Send And Receive

        private void SendToLoopBackMessageAOG(byte[] byteData)
        {
            traffic.cntrPGNToAOG += byteData.Length;
            SendToLoopBack(byteData, epAgOpen);
        }

        private void SendToLoopBackMessageVR(byte[] byteData)
        {
            SendToLoopBack(byteData, epAgVR);
        }

        private void SendToLoopBack(byte[] byteData, IPEndPoint endPoint)
        {
            try
            {
                if (byteData.Length != 0)
                {
                    loopBackSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, endPoint, new AsyncCallback(SendDataLoopAsync), null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message, "UDP LoopBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                loopBackSocket.EndSend(asyncResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = loopBackSocket.EndReceiveFrom(asyncResult, ref epLoopSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epLoopSender, new AsyncCallback(ReceiveDataLoopAsync), null);

                // Update status through a delegate
                int port = ((IPEndPoint)epLoopSender).Port;
                BeginInvoke((MethodInvoker)(() => ReceiveFromLoopBack(port, localMsg)));
            }
            catch (Exception)
            {
                //MessageBox.Show("ReceiveData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        private void ReceiveFromLoopBack(int port, byte[] data)
        {
            traffic.cntrPGNFromAOG += data.Length;

            //Send out to udp network
            SendUDPMessage(data, epModule);

            if (data[0] == 0x80 && data[1] == 0x81)
            {
                switch (data[3])
                {
                    case 0xFE: //254 AutoSteer Data
                        {
                            //serList.AddRange(data);
                            SendModule1Port(data, data.Length);
                            SendModule2Port(data, data.Length);
                            break;
                        }
                    case 0xFC: //252 steer settings
                        {
                            SendModule1Port(data, data.Length);
                            break;
                        }
                    case 0xFB: //251 steer config
                        {
                            SendModule1Port(data, data.Length);
                            break;
                        }
                    case 0xEF: //239 machine pgn
                        {
                            SendModule2Port(data, data.Length);
                            SendModule1Port(data, data.Length);
                            break;
                        }

                    case 0xEE: //238 machine config
                        {
                            SendModule2Port(data, data.Length);
                            SendModule1Port(data, data.Length);
                            break;
                        }

                    case 0xEC: //236 machine config
                        {
                            SendModule2Port(data, data.Length);
                            SendModule1Port(data, data.Length);
                            break;
                        }
                }
            }
        }

        #endregion

        //sends byte array
        public void SendUDPMessage(byte[] byteData, IPEndPoint endPoint)
        {
            try
            {
                // Send packet to the zero
                if (isUDPNetworkConnected && byteData.Length != 0)
                {
                    traffic.cntrUDPOut += byteData.Length;
                    UDPSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, endPoint, new AsyncCallback(SendDataUDPAsync), null);
                }
            }
            catch (Exception)
            {
                //WriteErrorLog("Sending UDP Message" + e.ToString());
                //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        private void SendDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                UDPSocket.EndSend(asyncResult);
            }
            catch (Exception)
            {
                //WriteErrorLog(" UDP Send Data" + e.ToString());
                //MessageBox.Show("SendData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        private void ReceiveDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = UDPSocket.EndReceiveFrom(asyncResult, ref epUDPSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epUDPSender, new AsyncCallback(ReceiveDataUDPAsync), null);

                int port = ((IPEndPoint)epUDPSender).Port;
                BeginInvoke((MethodInvoker)(() => ReceiveFromUDP(port, localMsg)));
            }
            catch (Exception)
            {
                //WriteErrorLog("UDP Recv data " + e.ToString());
                //MessageBox.Show("ReceiveData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        private void ReceiveFromUDP(int port, byte[] data)
        {
            if (data[0] == 0x80 && data[1] == 0x81)
            {
                //module return via udp sent to AOG
                SendToLoopBackMessageAOG(data);

                //module data also sent to VR
                SendToLoopBackMessageVR(data);
            }

            else if (data[0] == 36 && (data[1] == 71 || data[1] == 80))
            {
                //if (timerSim.Enabled) DisableSim();
                traffic.cntrGPSIn += data.Length;
                rawBuffer += Encoding.ASCII.GetString(data);
                ParseNMEA(ref rawBuffer);
            }

            traffic.cntrUDPIn += data.Length;
        }
    }
}
