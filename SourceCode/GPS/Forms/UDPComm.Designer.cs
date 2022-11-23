using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        // - App Sockets  -----------------------------------------------------
        private Socket loopBackSocket;

        //endpoints of modules
        private IPEndPoint epAgVR = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 16666);
        private IPEndPoint epAgIO = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 17777);

        // Initialise the IPEndPoint for async listener!
        private EndPoint epSender = new IPEndPoint(IPAddress.Any, 0);

        // Data stream
        private byte[] loopBuffer = new byte[1024];

        // Status delegate
        private int udpWatchCounts = 0;
        public int udpWatchLimit = 70, toolGPSWatchdog = 20, vehicleGPSWatchdog = 20;

        private bool EnableHeadRoll;

        private void ReceiveFromAgIO(byte[] data)
        {
            #region F9p_UBX_Message
            //for testing purposes only will be deleted on release!
            if (data.Length > 7 && data[0] == 0xB5 && data[1] == 0x62 && data[2] == 0x01)//Daniel P
            {
                if (data[3] == 0x06 && data.Length > 8)
                {
                    int CK_A = 0;
                    int CK_B = 0;

                    for (int j = 2; j < 7; j += 1)// start with Class and end by Checksum
                    {
                        CK_A = (CK_A + data[j]) & 0xFF;
                        CK_B = (CK_B + CK_A) & 0xFF;
                    }

                    if (data[7] == CK_A && data[8] == CK_B)
                    {
                        if ((data[6] & 0x81) == 0x81)
                        {
                            mc.fixQualityTool = 4;
                        }
                        else if ((data[6] & 0x41) == 0x41)
                        {
                            mc.fixQualityTool = 5;
                        }
                        else
                        {
                            mc.fixQualityTool = 1;
                        }
                    }
                }
                else if (data[3] == 0x07 && data.Length > 99)//UBX-NAV-PVT
                {
                    int CK_A = 0;
                    int CK_B = 0;

                    for (int j = 2; j < 98; j += 1)// start with Class and end by Checksum
                    {
                        CK_A = (CK_A + data[j]) & 0xFF;
                        CK_B = (CK_B + CK_A) & 0xFF;
                    }

                    if (data[98] == CK_A && data[99] == CK_B)
                    {
                        long itow = data[6] | (data[7] << 8) | (data[8] << 16) | (data[9] << 24);

                        if ((data[27] & 0x81) == 0x81)
                        {
                            mc.fixQuality = 4;
                            EnableHeadRoll = true;
                        }
                        else if ((data[27] & 0x41) == 0x41)
                        {
                            mc.fixQuality = 5;
                            EnableHeadRoll = true;
                        }
                        else
                        {
                            mc.fixQuality = 1;
                            EnableHeadRoll = false;
                        }

                        mc.satellitesTracked = data[29];

                        mc.longitude = (data[30] | (data[31] << 8) | (data[32] << 16) | (data[33] << 24)) * 0.0000001;//to deg
                        mc.latitude = (data[34] | (data[35] << 8) | (data[36] << 16) | (data[37] << 24)) * 0.0000001;//to deg

                        //Height above ellipsoid
                        mc.altitude = (data[38] | (data[39] << 8) | (data[40] << 16) | (data[41] << 24)) * 0.001;//to meters
                        // Height above mean sea level
                        mc.altitude = (data[42] | (data[43] << 8) | (data[44] << 16) | (data[45] << 24)) * 0.001;//to meters

                        mc.hdop = (data[82] | (data[83] << 8) | (data[84] << 16) | (data[85] << 24)) * 0.01;

                        if (mc.longitude != 0)
                        {
                            vehicleGPSWatchdog = 0;
                            if (toolGPSWatchdog < 20) toolGPSWatchdog++;

                            mc.speed = (data[66] | (data[67] << 8) | (data[68] << 16) | (data[69] << 24)) * 0.0036;// mm/s to km/h
                            worldManager.ConvertWGS84ToLocal(mc.latitude, mc.longitude, out mc.fix.northing, out mc.fix.easting);

                            sentenceCounter = 0;
                            UpdateFixPosition();
                        }
                        else
                        {
                            EnableHeadRoll = false;
                            mc.fixQuality = 0;
                        }
                    }
                }
                else if (data[3] == 0x3C && data.Length > 71)//Daniel P
                {
                    int CK_A = 0;
                    int CK_B = 0;
                    for (int j = 2; j < 70; j += 1)// start with Class and end by Checksum
                    {
                        CK_A = (CK_A + data[j]) & 0xFF;
                        CK_B = (CK_B + CK_A) & 0xFF;
                    }

                    if (data[70] == CK_A && data[71] == CK_B)
                    {
                        long itow = data[10] | (data[11] << 8) | (data[12] << 16) | (data[13] << 24);

                        if ((true || EnableHeadRoll) && (((data[66] & 0x2F) == 0x2F) || ((data[66] & 0x37) == 0x37)))
                        {
                            int relposlength = data[26] | (data[27] << 8) | (data[28] << 16) | (data[29] << 24);//in cm!


                            //double DualAntennaDistance = 146.5;


                            //if ((DualAntennaDistance - 5) < relposlength && relposlength < (DualAntennaDistance + 5))
                            {
                                double RelPosN = ((data[14] | (data[15] << 8) | (data[16] << 16) | (data[17] << 24)) + (sbyte)data[38] * 0.01);
                                double RelPosE = ((data[18] | (data[19] << 8) | (data[20] << 16) | (data[21] << 24)) + (sbyte)data[39] * 0.01);
                                double relPosD = ((data[22] | (data[23] << 8) | (data[24] << 16) | (data[25] << 24)) + (sbyte)data[40] * 0.01);

                                double rollK = glm.toDegrees(Math.Atan2(relPosD, Math.Sqrt(RelPosN * RelPosN + RelPosE * RelPosE)));
                                //ahrs.imuPitch = glm.toDegrees(Math.Atan2(relPosD, DualAntennaDistance));
                                //if (ahrs.isRollInvert)
                                //    ahrs.imuRoll *= -1;

                                if (mc.isRollInvert) rollK *= -1.0;
                                rollK -= mc.rollZero;
                                mc.imuRoll = mc.imuRoll * mc.rollFilter + rollK * (1 - mc.rollFilter);

                                //D = d / 2 * cos(roll) + h * sin(roll) * cos(pitch)
                            }

                            mc.headingTrueDual = (data[30] | (data[31] << 8) | (data[32] << 16) | (data[33] << 24)) * 0.00001 + mc.headingTrueDualOffset;

                            if (mc.isDualAsIMU)
                            {
                                mc.imuHeading = mc.headingTrueDual;
                                mc.headingTrueDual = double.MaxValue;
                            }
                        }
                        else //Bad Quality
                        {
                            mc.imuRoll = 88888;
                            mc.headingTrueDual = double.MaxValue;
                        }
                    }
                }

                return;
            }
            #endregion
            int Length;

            if (data.Length > 4 && data[0] == 0x80 && data[1] == 0x81 && data.Length > (Length = data[4] + 5))
            {
                byte CK_A = 0;
                for (int j = 2; j < Length; j++)
                {
                    CK_A += data[j];
                }

                if (data[Length] == (byte)CK_A)
                {
                    switch (data[3])
                    {
                        case 0xD6:// 214
                            {
                                if (startCounter > 0 && swHz.ElapsedMilliseconds < udpWatchLimit)
                                {
                                    udpWatchCounts++;
                                    if (isLogNMEA) mc.logNMEASentence.Append("*** "
                                        + DateTime.UtcNow.ToString("ss.ff -> ", CultureInfo.InvariantCulture)
                                        + swHz.ElapsedMilliseconds + "\r\n");
                                    return;
                                }

                                double Lon = BitConverter.ToDouble(data, 5);
                                double Lat = BitConverter.ToDouble(data, 13);

                                if (Lon != double.MaxValue && Lat != double.MaxValue)
                                {
                                    //make sure tool gps is either off or resetting watchdog
                                    vehicleGPSWatchdog = 0;
                                    if (toolGPSWatchdog < 20) toolGPSWatchdog++;

                                    if (glm.isSimEnabled)
                                        SetSimStatus(false);

                                    mc.longitude = Lon;
                                    mc.latitude = Lat;

                                    worldManager.ConvertWGS84ToLocal(Lat, Lon, out mc.fix.northing, out mc.fix.easting);

                                    //From dual antenna heading sentences
                                    float temp = BitConverter.ToSingle(data, 21);
                                    if (temp != float.MaxValue)
                                    {
                                        mc.headingTrueDual = temp + mc.headingTrueDualOffset;
                                        if (mc.headingTrueDual < 0) mc.headingTrueDual += 360;
                                        if (mc.isDualAsIMU)
                                        {
                                            mc.imuHeading = temp;
                                            mc.headingTrueDual = double.MaxValue;
                                        }
                                    }
                                    else
                                    {
                                        mc.headingTrueDual = double.MaxValue;
                                    }

                                    //from single antenna sentences (VTG,RMC)
                                    mc.headingTrue = BitConverter.ToSingle(data, 25);

                                    temp = BitConverter.ToSingle(data, 29);
                                    mc.speed = temp == float.MaxValue ? double.MaxValue : temp;

                                    //roll in degrees
                                    temp = BitConverter.ToSingle(data, 33);
                                    if (temp == float.MinValue)
                                        mc.imuRoll = 0;
                                    else if (temp != float.MaxValue)
                                    {
                                        if (mc.isRollInvert) temp *= -1;
                                        mc.imuRoll = temp - mc.rollZero;
                                    }

                                    //altitude in meters
                                    temp = BitConverter.ToSingle(data, 37);
                                    if (temp != float.MaxValue)
                                        mc.altitude = temp;

                                    ushort sats = BitConverter.ToUInt16(data, 41);
                                    if (sats != ushort.MaxValue)
                                        mc.satellitesTracked = sats;

                                    byte fix = data[43];
                                    if (fix != byte.MaxValue)
                                        mc.fixQuality = fix;

                                    ushort hdop = BitConverter.ToUInt16(data, 44);
                                    if (hdop != ushort.MaxValue)
                                        mc.hdop = hdop * 0.01;

                                    ushort age = BitConverter.ToUInt16(data, 46);
                                    if (age != ushort.MaxValue)
                                        mc.age = age * 0.01;

                                    ushort imuHead = BitConverter.ToUInt16(data, 48);
                                    if (imuHead != ushort.MaxValue)
                                    {
                                        mc.imuHeading = imuHead;
                                        mc.imuHeading *= 0.1;
                                    }

                                    short imuRol = BitConverter.ToInt16(data, 50);
                                    if (imuRol != short.MaxValue)
                                    {
                                        double rollK = imuRol;
                                        if (mc.isRollInvert) rollK *= -0.1;
                                        else rollK *= 0.1;
                                        rollK -= mc.rollZero;
                                        mc.imuRoll = mc.imuRoll * mc.rollFilter + rollK * (1 - mc.rollFilter);
                                    }

                                    short imuPich = BitConverter.ToInt16(data, 52);
                                    if (imuPich != short.MaxValue)
                                    {
                                        mc.imuPitch = imuPich;
                                    }

                                    short imuYaw = BitConverter.ToInt16(data, 54);
                                    if (imuYaw != short.MaxValue)
                                    {
                                        mc.imuYawRate = imuYaw;
                                    }

                                    sentenceCounter = 0;

                                    if (isLogNMEA)
                                        mc.logNMEASentence.Append(
                                            DateTime.UtcNow.ToString("mm:ss.ff", CultureInfo.InvariantCulture) + " " +
                                            Lat.ToString("0.0000000") + " " + Lon.ToString("0.0000000") + " " +
                                            mc.headingTrueDual.ToString("0.0") + "\r\n"
                                            );

                                    UpdateFixPosition();
                                }
                            }
                            break;

                        case 0xD7: //Tool Antenna
                            {
                                double Lon = BitConverter.ToDouble(data, 5);
                                double Lat = BitConverter.ToDouble(data, 13);

                                if (tool.isSteering && Lon != double.MaxValue && Lat != double.MaxValue)
                                {
                                    if (vehicleGPSWatchdog < 20) vehicleGPSWatchdog++;
                                    toolGPSWatchdog = 0;

                                    mc.longitudeTool = Lon;
                                    mc.latitudeTool = Lat;
                                    worldManager.ConvertWGS84ToLocal(Lat, Lon, out mc.fixTool.northing, out mc.fixTool.easting);

                                    ushort sats = BitConverter.ToUInt16(data, 21);
                                    if (sats != ushort.MaxValue)
                                        mc.satellitesTrackedTool = sats;

                                    byte fix = data[23];
                                    if (fix != byte.MaxValue)
                                        mc.fixQualityTool = fix;

                                    ushort hdop = BitConverter.ToUInt16(data, 24);
                                    if (hdop != ushort.MaxValue)
                                        mc.hdopTool = hdop * 0.01;

                                    ushort age = BitConverter.ToUInt16(data, 26);
                                    if (age != ushort.MaxValue)
                                        mc.ageTool = age * 0.01;

                                    short imuRoll = BitConverter.ToInt16(data, 28);
                                    if (imuRoll != short.MaxValue)
                                    {
                                        double rollK = imuRoll;
                                        if (mc.isRollInvert) rollK *= -0.1;
                                        else rollK *= 0.1;
                                        rollK -= mc.rollZeroTool;
                                        mc.imuRollTool = rollK;
                                    }

                                    //From dual antenna heading sentences
                                    float temp = BitConverter.ToSingle(data, 30);
                                    if (temp != float.MaxValue)
                                    {
                                        mc.headingTrueDualTool = temp;// + pn.headingTrueDualOffset;
                                        if (mc.headingTrueDualTool < 0) mc.headingTrueDualTool += 360;
                                        if (mc.isDualAsIMU) mc.imuHeadingTool = temp;
                                    }

                                    sentenceCounter = 0;//or when vehicleGPSWatchdog > 10 only?

                                    if (vehicleGPSWatchdog > 10)
                                        UpdateFixPosition();
                                }
                            }
                            break;

                        case 0xFD:// 253    return from autosteer module
                            {
                                //Steer angle actual
                                if (data.Length != 14)
                                    break;
                                mc.actualSteerAngleChart = (Int16)((data[6] << 8) + data[5]);
                                mc.actualSteerAngleDegrees = (double)mc.actualSteerAngleChart * 0.01;

                                //Heading
                                double head253 = (Int16)((data[8] << 8) + data[7]);
                                if (head253 != 9999)
                                {
                                    mc.imuHeading = head253 * 0.1;
                                }

                                //Roll
                                double rollK = (Int16)((data[10] << 8) + data[9]);
                                if (rollK != 8888)
                                {
                                    if (mc.isRollInvert) rollK *= -0.1;
                                    else rollK *= 0.1;
                                    rollK -= mc.rollZero;
                                    mc.imuRoll = mc.imuRoll * mc.rollFilter + rollK * (1 - mc.rollFilter);
                                }
                                //else ahrs.imuRoll = 88888;

                                //switch status
                                mc.workSwitchHigh = (data[11] & 1) == 1;
                                mc.steerSwitchHigh = (data[11] & 2) == 2;

                                //the pink steer dot reset
                                steerModuleConnectedCounter = 0;

                                //Actual PWM
                                mc.pwmDisplay = data[12];

                                if (isLogNMEA)
                                    mc.logNMEASentence.Append(
                                        DateTime.UtcNow.ToString("mm:ss.ff", CultureInfo.InvariantCulture) + " AS " +
                                        //Lat.ToString("0.0000000") + " " + Lon.ToString("0.0000000") + " " +
                                        //pn.speed.ToString("0.0") + " " + ahrs.imuRoll.ToString("0.0") + " " +
                                        mc.actualSteerAngleDegrees.ToString("0.0") + "\r\n"
                                        );

                                break;
                            }

                        case 0xFA: // 250
                            {
                                if (data.Length != 14)
                                    break;
                                mc.sensorData = data[5];
                                break;
                            }

                        case 0xE6:// 230    return from tool steer module
                            {
                                //Steer angle actual
                                if (data.Length != 14)
                                    break;
                                mc.toolActualDistance = (Int16)((data[6] << 8) + data[5]);
                                mc.toolActualDistance *= 0.1;
                                mc.toolError = (Int16)((data[8] << 8) + data[7]);
                                mc.toolError *= 0.1;

                                mc.toolPWM = (Int16)((data[9]));

                                mc.toolStatus = (Int16)((data[10]));

                                break;
                            }

                        #region Remote Switches
                        case 0xEA: // 234    MTZ8302 Feb 2020
                            {
                                //MTZ8302 Feb 2020 
                                if (data.Length != 14 && isJobStarted)
                                {
                                    //MainSW was used
                                    if (data[mc.swMain] != mc.ssP[0])
                                    {
                                        //Main SW pressed
                                        if ((data[mc.swMain] & 1) == 1)
                                        {
                                            setSectionBtnState(btnStates.On);
                                        } // if Main SW ON
                                          //if Main SW in Arduino is pressed OFF
                                        if ((data[mc.swMain] & 2) == 2)
                                        {
                                            setSectionBtnState(btnStates.Off);
                                        } // if Main SW OFF

                                        mc.ssP[0] = data[mc.swMain];
                                    }  //Main or Rate SW

                                    int set = 1;
                                    int idx1 = mc.swOnGr0;
                                    int idx2 = mc.swOffGr0;
                                    int idx3 = 1;

                                    for (int j = 0; j < tool.numOfSections; j++)
                                    {
                                        if (j == 8)
                                        {
                                            set = 1;
                                            idx1 = mc.swOnGr1;
                                            idx2 = mc.swOffGr1;
                                            idx3 = 2;
                                        }

                                        //do nothing if bit isn't set [only works fully when BBBBB is deleted]
                                        btnStates status = tool.sections[j].sectionState;

                                        if ((data[idx1] & set) == set)
                                        {
                                            if (autoBtnState == btnStates.Auto && (data[idx2] & set) == set)//AAAAA
                                                status = btnStates.Auto;//not sure if we want to force on when auto is off!
                                            else if (autoBtnState != btnStates.Off)
                                                status = btnStates.On;
                                            else
                                                status = btnStates.Off;
                                        }
                                        else if ((data[idx2] & set) == set)
                                            status = btnStates.Off;
                                        else if ((data[idx2] & set) != (mc.ssP[idx3] & set) && status == btnStates.Off)//BBBBB
                                            status = btnStates.Auto;//should change to AAAAA (Both on [so that it doesnt change when you send 0])

                                        set <<= 1;

                                        tool.sections[j].UpdateButton(status);
                                    }

                                    //only needed for BBBBB
                                    mc.ssP[1] = data[mc.swOffGr0];
                                    mc.ssP[2] = data[mc.swOffGr1];

                                }//if serial or udp port open

                                break;
                            }
                            #endregion
                    }
                }
            }
        }

        //start the UDP server
        public void StartLoopbackServer()
        {
            try
            {
                // Initialise the socket
                loopBackSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                loopBackSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                loopBackSocket.Bind(new IPEndPoint(IPAddress.Any, 15555));
                loopBackSocket.BeginReceiveFrom(loopBuffer, 0, loopBuffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveAppData), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSimStatus(bool enabled)
        {
            isFirstFixPositionSet = false;
            isGPSPositionInitialized = false;
            isFirstHeadingSet = false;
            startCounter = 0;
            panelSim.Visible = timerSim.Enabled = enabled;
            simulatorOnToolStripMenuItem.Checked = glm.isSimEnabled = enabled;

            if (Properties.Settings.Default.setMenu_isSimulatorOn != simulatorOnToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void ReceiveAppData(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = loopBackSocket.EndReceiveFrom(asyncResult, ref epSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(loopBuffer, localMsg, msgLen);

                // Listen for more connections again...
                loopBackSocket.BeginReceiveFrom(loopBuffer, 0, loopBuffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveAppData), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromAgIO(localMsg)));
            }
            catch (Exception)
            {
                // MessageBox.Show("ReceiveData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendAsyncLoopData(IAsyncResult asyncResult)
        {
            try
            {
                loopBackSocket.EndSend(asyncResult);
            }
            catch (Exception ex)
            { 
                if (!glm.isSimEnabled)
                    MessageBox.Show("SendData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendPgnToLoop(byte[] byteData)
        {
            try
            {
                if (loopBackSocket != null && byteData.Length > 2)
                {
                    int crc = 0;
                    for (int i = 2; i + 1 < byteData.Length; i++)
                    {
                        crc += byteData[i];
                    }
                    byteData[byteData.Length - 1] = (byte)crc;
                    
                    loopBackSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, epAgIO, new AsyncCallback(SendAsyncLoopData), null);
                }
            }
            catch (Exception)
            {
                //WriteErrorLog("Sending UDP Message" + e.ToString());
                //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //for moving and sizing borderless window
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0083:
                    if (m.WParam.ToInt32() == 1)
                        return;
                    else break;

                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= NormalPadding.Top)
                        {
                            if (clientPoint.X <= NormalPadding.Left)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - NormalPadding.Right))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - NormalPadding.Bottom))
                        {
                            if (clientPoint.X <= NormalPadding.Left)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - NormalPadding.Right))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= NormalPadding.Left)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - NormalPadding.Right))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }

        private static Padding MaximizedPadding = new System.Windows.Forms.Padding(8, 8, 8, 8);
        private static Padding NormalPadding = new System.Windows.Forms.Padding(5, 5, 5, 5);

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
               if (this.Padding != MaximizedPadding)
                this.Padding = MaximizedPadding;
            }
            else if (this.Padding != NormalPadding)
            {
                this.Padding = NormalPadding;
            }
            base.OnLayout(levent);
        }

        #region keystrokes
        //keystrokes for easy and quick startup
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //reset Sim
            if (keyData == Keys.L)
            {
                btnResetSim.PerformClick();
                return true;
            }

            //speed up
            if (keyData == Keys.Up)
            {
                sim.stepDistance += 0.055555555554;
                if (sim.stepDistance > 27.7778) sim.stepDistance = 27.7778;
                hsbarStepDistance.Value = (int)(sim.stepDistance * 3.6);
                return true;
            }

            //slow down
            if (keyData == Keys.Down)
            {
                sim.stepDistance -= 0.055555555554;
                if (sim.stepDistance < -6.94444) sim.stepDistance = -6.94444;
                hsbarStepDistance.Value = (int)(sim.stepDistance * 3.6);
                return true;
            }

            //Stop
            if (keyData == Keys.OemPeriod)
            {
                sim.stepDistance = 0;
                hsbarStepDistance.Value = 0;
                return true;
            }

            //turn right
            if (keyData == Keys.Right)
            {
                sim.steerAngle += 2;
                if (sim.steerAngle > 40) sim.steerAngle = 40;
                if (sim.steerAngle < -40) sim.steerAngle = -40;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            //turn left
            if (keyData == Keys.Left)
            {
                sim.steerAngle -= 2;
                if (sim.steerAngle > 40) sim.steerAngle = 40;
                if (sim.steerAngle < -40) sim.steerAngle = -40;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            //zero steering
            if (keyData == Keys.OemQuestion)
            {
                sim.steerAngle = 0.0;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            if (keyData == (Keys.F))
            {
                CloseCurrentJob(false);
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.A)) //autosteer button on off
            {
                setBtnAutoSteer(!isAutoSteerBtnOn);
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.C)) //open the steer chart
            {
                steerChartStripMenu.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.NumPad1) || keyData == (Keys.N)) //auto section on off
            {
                btnSectionOffAutoOn.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.NumPad0) || keyData == (Keys.M)) //auto section on off
            {
                btnManualOffOn.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.G)) // Flag click
            {
                btnFlag.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.P)) // Snap/Prioritu click
            {
                btnSnapToPivot.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.F11)) // Full Screen click
            {
                btnMaximizeMainForm.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Gesture

        // Private variables used to maintain the state of gestures
        ////private DrawingObject _dwo = new DrawingObject();
        //private Point _ptFirst = new Point();

        //private Point _ptSecond = new Point();
        //private int _iArguments = 0;

        //// One of the fields in GESTUREINFO structure is type of Int64 (8 bytes).
        //// The relevant gesture information is stored in lower 4 bytes. This
        //// bit mask is used to get 4 lower bytes from this argument.
        //private const Int64 ULL_ARGUMENTS_BIT_MASK = 0x00000000FFFFFFFF;

        ////-----------------------------------------------------------------------
        //// Multitouch/Touch glue (from winuser.h file)
        //// Since the managed layer between C# and WinAPI functions does not
        //// exist at the moment for multi-touch related functions this part of
        //// code is required to replicate definitions from winuser.h file.
        ////-----------------------------------------------------------------------
        //// Touch event window message constants [winuser.h]
        //private const int WM_GESTURENOTIFY = 0x011A;

        //private const int WM_GESTURE = 0x0119;

        //private const int GC_ALLGESTURES = 0x00000001;

        //// Gesture IDs
        //private const int GID_BEGIN = 1;

        //private const int GID_END = 2;
        //private const int GID_ZOOM = 3;
        //private const int GID_PAN = 4;
        //private const int GID_ROTATE = 5;
        //private const int GID_TWOFINGERTAP = 6;


        //private const int GID_PRESSANDTAP = 7;

        //// Gesture flags - GESTUREINFO.dwFlags
        //private const int GF_BEGIN = 0x00000001;

        //private const int GF_INERTIA = 0x00000002;
        //private const int GF_END = 0x00000004;

        ////
        //// Gesture configuration structure
        ////   - Used in SetGestureConfig and GetGestureConfig
        ////   - Note that any setting not included in either GESTURECONFIG.dwWant
        ////     or GESTURECONFIG.dwBlock will use the parent window's preferences
        ////     or system defaults.
        ////
        //// Touch API defined structures [winuser.h]
        //[StructLayout(LayoutKind.Sequential)]
        //private struct GESTURECONFIG
        //{
        //    public int dwID;    // gesture ID
        //    public int dwWant;  // settings related to gesture ID that are to be

        //    // turned on
        //    public int dwBlock; // settings related to gesture ID that are to be

        //    // turned off
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //private struct POINTS
        //{
        //    public short x;
        //    public short y;
        //}

        ////
        //// Gesture information structure
        ////   - Pass the HGESTUREINFO received in the WM_GESTURE message lParam
        ////     into the GetGestureInfo function to retrieve this information.
        ////   - If cbExtraArgs is non-zero, pass the HGESTUREINFO received in
        ////     the WM_GESTURE message lParam into the GetGestureExtraArgs
        ////     function to retrieve extended argument information.
        ////
        //[StructLayout(LayoutKind.Sequential)]
        //private struct GESTUREINFO
        //{
        //    public int cbSize;           // size, in bytes, of this structure

        //    // (including variable length Args
        //    // field)
        //    public int dwFlags;          // see GF_* flags

        //    public int dwID;             // gesture ID, see GID_* defines
        //    public IntPtr hwndTarget;    // handle to window targeted by this

        //    // gesture
        //    [MarshalAs(UnmanagedType.Struct)]
        //    internal POINTS ptsLocation; // current location of this gesture

        //    public int dwInstanceID;     // internally used
        //    public int dwSequenceID;     // internally used
        //    public Int64 ullArguments;   // arguments for gestures whose

        //    // arguments fit in 8 BYTES
        //    public int cbExtraArgs;      // size, in bytes, of extra arguments,

        //    // if any, that accompany this gesture
        //}

        //// Currently touch/multitouch access is done through unmanaged code
        //// We must p/invoke into user32 [winuser.h]
        //[DllImport("user32")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, ref GESTURECONFIG pGestureConfig, int cbSize);

        //[DllImport("user32")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

        //// size of GESTURECONFIG structure
        //private int _gestureConfigSize;

        //// size of GESTUREINFO structure
        //private int _gestureInfoSize;

        //[SecurityPermission(SecurityAction.Demand)]
        //private void SetupStructSizes()
        //{
        //    // Both GetGestureCommandInfo and GetTouchInputInfo need to be
        //    // passed the size of the structure they will be filling
        //    // we get the sizes upfront so they can be used later.
        //    _gestureConfigSize = Marshal.SizeOf(new GESTURECONFIG());
        //    _gestureInfoSize = Marshal.SizeOf(new GESTUREINFO());
        //}

        ////-------------------------------------------------------------
        //// Since there is no managed layer at the moment that supports
        //// event handlers for WM_GESTURENOTIFY and WM_GESTURE
        //// messages we have to override WndProc function
        ////
        //// in
        ////   m - Message object
        ////-------------------------------------------------------------

        //// Drag form without border definitions
        //private const int WM_NCHITTEST = 0x84;
        ////private const int HT_CAPTION = 0x2;

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //protected override void WndProc(ref Message m)
        //{
        //    bool handled = false;
        //    const int RESIZE_HANDLE_SIZE = 10;

        //    switch (m.Msg)
        //    {
        //        //case WM_GESTURENOTIFY:
        //        //    {
        //        //        // This is the right place to define the list of gestures
        //        //        // that this application will support. By populating
        //        //        // GESTURECONFIG structure and calling SetGestureConfig
        //        //        // function. We can choose gestures that we want to
        //        //        // handle in our application. In this app we decide to
        //        //        // handle all gestures.
        //        //        GESTURECONFIG gc = new GESTURECONFIG
        //        //        {
        //        //            dwID = 0,                // gesture ID
        //        //            dwWant = GC_ALLGESTURES, // settings related to gesture
        //        //                                     // ID that are to be turned on
        //        //            dwBlock = 0 // settings related to gesture ID that are
        //        //        };
        //        //        // to be

        //        //        // We must p/invoke into user32 [winuser.h]
        //        //        bool bResult = SetGestureConfig(
        //        //            Handle, // window for which configuration is specified
        //        //            0,      // reserved, must be 0
        //        //            1,      // count of GESTURECONFIG structures
        //        //            ref gc, // array of GESTURECONFIG structures, dwIDs
        //        //                    // will be processed in the order specified
        //        //                    // and repeated occurances will overwrite
        //        //                    // previous ones
        //        //            _gestureConfigSize // sizeof(GESTURECONFIG)
        //        //        );

        //        //        if (!bResult)
        //        //        {
        //        //            throw new Exception("Error in execution of SetGestureConfig");
        //        //        }
        //        //    }
        //        //    handled = true;
        //        //    break;

        //        //case WM_GESTURE:
        //        //    // The gesture processing code is implemented in
        //        //    // the DecodeGesture method
        //        //    handled = DecodeGesture(ref m);
        //        //    break;

        //        case WM_NCHITTEST:

        //            base.WndProc(ref m);

        //            if ((int)m.Result == 0x01/*HTCLIENT*/)
        //            {
        //                Point screenPoint = new Point(m.LParam.ToInt32());
        //                Point clientPoint = this.PointToClient(screenPoint);
        //                if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
        //                {
        //                    if (clientPoint.X <= RESIZE_HANDLE_SIZE)
        //                        m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
        //                    else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
        //                        m.Result = (IntPtr)12/*HTTOP*/ ;
        //                    else
        //                        m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
        //                }
        //                else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
        //                {
        //                    if (clientPoint.X <= RESIZE_HANDLE_SIZE)
        //                        m.Result = (IntPtr)10/*HTLEFT*/ ;
        //                    else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
        //                        m.Result = (IntPtr)2/*HTCAPTION*/ ;
        //                    else
        //                        m.Result = (IntPtr)11/*HTRIGHT*/ ;
        //                }
        //                else
        //                {
        //                    if (clientPoint.X <= RESIZE_HANDLE_SIZE)
        //                        m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
        //                    else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
        //                        m.Result = (IntPtr)15/*HTBOTTOM*/ ;
        //                    else
        //                        m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
        //                }
        //                return;

        //            }

        //            handled = false;
        //            //base.WndProc(ref m);

        //            // For window move
        //            //m.Result = (IntPtr)(HT_CAPTION);

        //            //return;

        //            break;

        //        default:
        //            handled = false;
        //            break;
        //    }

        //    //// Filter message back up to parents.
        //    //base.WndProc(ref m);

        //    //if (handled)
        //    //{
        //    //    // Acknowledge event if handled.
        //    //    try
        //    //    {
        //    //        m.Result = new System.IntPtr(1);
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //    }
        //    //}
        //}

        //// Taken from GCI_ROTATE_ANGLE_FROM_ARGUMENT.
        //// Converts from "binary radians" to traditional radians.
        //static protected double ArgToRadians(Int64 arg)
        //{
        //    return (arg / 65535.0 * 4.0 * 3.14159265) - (2.0 * 3.14159265);
        //}

        //// Handler of gestures
        ////in:
        ////  m - Message object
        //private bool DecodeGesture(ref Message m)
        //{
        //    GESTUREINFO gi;

        //    try
        //    {
        //        gi = new GESTUREINFO();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    gi.cbSize = _gestureInfoSize;

        //    // Load the gesture information.
        //    // We must p/invoke into user32 [winuser.h]
        //    if (!GetGestureInfo(m.LParam, ref gi))
        //    {
        //        return false;
        //    }

        //    switch (gi.dwID)
        //    {
        //        case GID_BEGIN:
        //        case GID_END:
        //            break;

        //        case GID_ZOOM:
        //            switch (gi.dwFlags)
        //            {
        //                case GF_BEGIN:
        //                    _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
        //                    _ptFirst.X = gi.ptsLocation.x;
        //                    _ptFirst.Y = gi.ptsLocation.y;
        //                    _ptFirst = PointToClient(_ptFirst);
        //                    break;

        //                default:
        //                    // We read here the second point of the gesture. This
        //                    // is middle point between fingers in this new
        //                    // position.
        //                    _ptSecond.X = gi.ptsLocation.x;
        //                    _ptSecond.Y = gi.ptsLocation.y;
        //                    _ptSecond = PointToClient(_ptSecond);
        //                    {
        //                        // The zoom factor is the ratio of the new
        //                        // and the old distance. The new distance
        //                        // between two fingers is stored in
        //                        // gi.ullArguments (lower 4 bytes) and the old
        //                        // distance is stored in _iArguments.
        //                        double k = (double)(_iArguments)
        //                                    / (double)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
        //                        //lblX.Text = k.ToString();
        //                        camera.zoomValue *= k;
        //                        if (camera.zoomValue < 6.0) camera.zoomValue = 6;
        //                        camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
        //                        SetZoom();
        //                    }

        //                    // Now we have to store new information as a starting
        //                    // information for the next step in this gesture.
        //                    _ptFirst = _ptSecond;
        //                    _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
        //                    break;
        //            }
        //            break;

        //        //case GID_PAN:
        //        //    switch (gi.dwFlags)
        //        //    {
        //        //        case GF_BEGIN:
        //        //            _ptFirst.X = gi.ptsLocation.x;
        //        //            _ptFirst.Y = gi.ptsLocation.y;
        //        //            _ptFirst = PointToClient(_ptFirst);
        //        //            break;

        //        //        default:
        //        //            // We read the second point of this gesture. It is a
        //        //            // middle point between fingers in this new position
        //        //            _ptSecond.X = gi.ptsLocation.x;
        //        //            _ptSecond.Y = gi.ptsLocation.y;
        //        //            _ptSecond = PointToClient(_ptSecond);

        //        //            // We apply move operation of the object
        //        //            _dwo.Move(_ptSecond.X - _ptFirst.X, _ptSecond.Y - _ptFirst.Y);

        //        //            Invalidate();

        //        //            // We have to copy second point into first one to
        //        //            // prepare for the next step of this gesture.
        //        //            _ptFirst = _ptSecond;
        //        //            break;
        //        //    }
        //        //    break;

        //        case GID_ROTATE:
        //            switch (gi.dwFlags)
        //            {
        //                case GF_BEGIN:
        //                    _iArguments = 32768;
        //                    break;

        //                default:
        //                    // Gesture handler returns cumulative rotation angle. However we
        //                    // have to pass the delta angle to our function responsible
        //                    // to process the rotation gesture.
        //                    double k = ((int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK) - _iArguments) * 0.01;
        //                    camera.camPitch -= k;
        //                    if (camera.camPitch < -74) camera.camPitch = -74;
        //                    if (camera.camPitch > 0) camera.camPitch = 0;
        //                    _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
        //                    break;
        //            }
        //            break;

        //            //case GID_TWOFINGERTAP:
        //            //    // Toggle drawing of diagonals
        //            //    _dwo.ToggleDrawDiagonals();
        //            //    Invalidate();
        //            //    break;

        //            //case GID_PRESSANDTAP:
        //            //    if (gi.dwFlags == GF_BEGIN)
        //            //    {
        //            //        // Shift drawing color
        //            //        _dwo.ShiftColor();
        //            //        Invalidate();
        //            //    }
        //            //    break;
        //    }

        //    return true;
        //}

        #endregion Gesture

    }
}
