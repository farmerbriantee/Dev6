//Please, if you use this, share the improvements

using System.IO.Ports;
using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormLoop
    {
        public static string portNameGPS = "***";
        public  static int baudRateGPS = 4800;

        public  static string portNameRtcm = "***";
        public  static int baudRateRtcm = 4800;

        //used to decide to autoconnect section arduino this run
        public string recvGPSSentence = "GPS";

        public bool isGPSCommOpen = false;
        public bool isGPSCommToolOpen = false;

        //used to decide to autoconnect autosteer arduino this run
        public bool wasGPSConnectedLastRun = false;
        public bool wasRtcmConnectedLastRun = false;

        //serial port gps is connected to
        public SerialPort spGPS = new SerialPort(portNameGPS, baudRateGPS, Parity.None, 8, StopBits.One);

        //serial port gps is connected to
        public SerialPort spRtcm = new SerialPort(portNameRtcm, baudRateRtcm, Parity.None, 8, StopBits.One);

        #region GPS SerialPort --------------------------------------------------------------------------

        public void SendGPSPort(byte[] data)
        {
            try
            {
                if (spRtcm.IsOpen)
                {
                    spRtcm.Write(data, 0, data.Length);
                }

                else if (spGPS.IsOpen)
                {
                    spGPS.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        public void OpenGPSPort()
        {

            if (spGPS.IsOpen)
            {
                //close it first
                CloseGPSPort();
            }


            if (!spGPS.IsOpen)
            {
                spGPS.PortName = portNameGPS;
                spGPS.BaudRate = baudRateGPS;
                spGPS.DataReceived += sp_DataReceivedGPS;
                spGPS.WriteTimeout = 1000;
            }

            try { spGPS.Open(); }
            catch (Exception)
            {
            }

            if (spGPS.IsOpen)
            {
                //discard any stuff in the buffers
                spGPS.DiscardOutBuffer();
                spGPS.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS = portNameGPS;
                Properties.Settings.Default.setPort_baudRateGPS = baudRateGPS;
                Properties.Settings.Default.setPort_wasGPSConnected = true;
                wasGPSConnectedLastRun = true;
                Properties.Settings.Default.Save();
                lblGPS1Comm.Text = portNameGPS;
                wasGPSConnectedLastRun = true;
            }
        }
        public void CloseGPSPort()
        {
            //if (sp.IsOpen)
            {
                spGPS.DataReceived -= sp_DataReceivedGPS;
                try { spGPS.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS.Text = " * * " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;
                //stripOnlineGPS.Value = 1;
                spGPS.Dispose();
            }
            lblGPS1Comm.Text = "---";
            Properties.Settings.Default.setPort_wasGPSConnected = false;
            wasGPSConnectedLastRun=false;
            Properties.Settings.Default.Save();

        }

        //called by the GPS delegate every time a chunk is rec'd
        private void ReceiveGPSPort(string sentence)
        {
            VehicleGPS.rawBuffer += sentence;
            ParseNMEA(ref VehicleGPS);

            //SendToLoopBackMessageAOG(sentence);
            traffic.cntrFromGPS += sentence.Length;
            if (isGPSCommOpen) recvGPSSentence = sentence;
        }

        //serial port receive in its own thread
        private void sp_DataReceivedGPS(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS.IsOpen)
            {
                try
                {
                    string sentence = spGPS.ReadExisting();
                    BeginInvoke((MethodInvoker)(() => ReceiveGPSPort(sentence)));
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion SerialPortGPS

        public void OpenRtcmPort()
        {
            if (spRtcm.IsOpen)
            {
                //close it first
                CloseRtcmPort();
            }

            if (!spRtcm.IsOpen)
            {
                spRtcm.PortName = portNameRtcm;
                spRtcm.BaudRate = baudRateRtcm;
                spRtcm.WriteTimeout = 1000;
            }

            try { spRtcm.Open(); }
            catch (Exception)
            {
            }

            if (spRtcm.IsOpen)
            {
                //discard any stuff in the buffers
                spRtcm.DiscardOutBuffer();
                spRtcm.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameRtcm = portNameRtcm;
                Properties.Settings.Default.setPort_baudRateRtcm = baudRateRtcm;
                Properties.Settings.Default.setPort_wasRtcmConnected = true;
                wasRtcmConnectedLastRun = true;
                Properties.Settings.Default.Save();
            }
        }

        public void CloseRtcmPort()
        {
            {
                try { spRtcm.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }
            }

            Properties.Settings.Default.setPort_wasRtcmConnected = false;
            wasRtcmConnectedLastRun = false;
            Properties.Settings.Default.Save();
        }
    }//end class
}//end namespace