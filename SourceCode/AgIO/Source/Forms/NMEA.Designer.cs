using System;
using System.Globalization;

namespace AgIO
{
    public partial class FormLoop
    {
        public NmeaValues VehicleGPS = new NmeaValues(true);
        public NmeaValues ToolGPS = new NmeaValues(false);
        
        private string[] words;
        public double latitude, longitude;

        public string ggaSentence, vtgSentence, hdtSentence, avrSentence, paogiSentence,
            hpdSentence, rmcSentence, pandaSentence, ksxtSentence;

        private float rollK, Pc, G, Xp, Zp, XeRoll, P = 1.0f;
        private readonly float varRoll = 0.1f, varProcess = 0.0003f;
        private bool resized = false;
        //Convert Fix value to Text
        public string FixQuality(NmeaValues GPS)
        {
            if (GPS.fixQualityData == 0) return "Invalid: ";
            else if (GPS.fixQualityData == 1) return "GPS 1: ";
            else if (GPS.fixQualityData == 2) return "DGPS : ";
            else if (GPS.fixQualityData == 3) return "PPS : ";
            else if (GPS.fixQualityData == 4) return "RTK fix: ";
            else if (GPS.fixQualityData == 5) return "Float: ";
            else if (GPS.fixQualityData == 6) return "Estimate: ";
            else if (GPS.fixQualityData == 7) return "Man IP: ";
            else if (GPS.fixQualityData == 8) return "Sim: ";
            else return "Unknown: ";
        }

        public void ParseNMEA(ref NmeaValues GPS)
        {
            if (GPS.rawBuffer == null) return;

            //find end of a sentence
            int cr = GPS.rawBuffer.IndexOf("\r", StringComparison.Ordinal);
            if (cr == -1) return; // No end found, wait for more data

            // Find start of next sentence
            int dollar = GPS.rawBuffer.IndexOf("$", StringComparison.Ordinal);
            if (dollar == -1) return;

            //if the $ isn't first, get rid of the tail of corrupt sentence
            if (dollar >= cr) GPS.rawBuffer = GPS.rawBuffer.Substring(dollar);

            cr = GPS.rawBuffer.IndexOf("\r", StringComparison.Ordinal);
            if (cr == -1) return; // No end found, wait for more data
            dollar = GPS.rawBuffer.IndexOf("$", StringComparison.Ordinal);
            if (dollar == -1) return;

            //if the $ isn't first, get rid of the tail of corrupt sentence
            if (dollar >= cr) GPS.rawBuffer = GPS.rawBuffer.Substring(dollar);

            cr = GPS.rawBuffer.IndexOf("\r", StringComparison.Ordinal);
            dollar = GPS.rawBuffer.IndexOf("$", StringComparison.Ordinal);
            if (cr == -1 || dollar == -1) return;

            if (GPS.rawBuffer.Length > 301)
            {
                if (isLogNMEA)
                {
                    GPS.logNMEASentence.Append("\r\n" +
                        DateTime.UtcNow.ToString(" ->>  mm:ss.fff ", CultureInfo.InvariantCulture) + "\r\n" + GPS.rawBuffer + "\r\n");
                }
                GPS.rawBuffer = "";
                return;
            }

            //now we have a complete sentence or more somewhere in the portData
            while (true)
            {
                //extract the next NMEA single sentence
                string nextNMEASentence = Parse(ref GPS);
                if (nextNMEASentence == null) break;

                words = nextNMEASentence.Split(',');

                if (isLogNMEA)
                {
                    string timNow = DateTime.UtcNow.ToString("HHmmss.fff ", CultureInfo.InvariantCulture);
                    GPS.logNMEASentence.Append(timNow + " " + nextNMEASentence + "\r\n");
                }

                //parse them accordingly
                if (words.Length < 3) break;

                if ((words[0] == "$GPGGA" || words[0] == "$GNGGA") && words.Length > 13)
                {
                    ParseGGA(ref GPS);
                    if (isGPSSentencesOn) ggaSentence = nextNMEASentence;
                }

                else if ((words[0] == "$GPVTG" || words[0] == "$GNVTG") && words.Length > 7)
                {
                    ParseVTG(ref GPS);
                    if (isGPSSentencesOn) vtgSentence = nextNMEASentence;
                }

                //else if (words[0] == "$GPRMC" || words[0] == "$GNRMC")
                //{
                //    ParseRMC();
                //    if (isGPSSentencesOn) rmcSentence = nextNMEASentence;
                //}

                else if (words[0] == "$KSXT")
                {
                    ParseKSXT(ref GPS);
                    if (isGPSSentencesOn) ksxtSentence = nextNMEASentence;
                }

                else if (words[0] == "$GPHPD")
                {
                    ParseHPD(ref GPS);
                    if (isGPSSentencesOn) hpdSentence = nextNMEASentence;
                }

                else if (words[0] == "$PAOGI" && words.Length > 14)
                {
                    ParseOGI(ref GPS);
                    if (isGPSSentencesOn) paogiSentence = nextNMEASentence;
                }

                else if (words[0] == "$PANDA" && words.Length > 14)
                {
                    ParsePANDA(ref GPS);
                    if (isGPSSentencesOn) pandaSentence = nextNMEASentence;
                }

                else if (words[0] == "$GPHDT" || words[0] == "$GNHDT")
                {
                    ParseHDT(ref GPS);
                    if (isGPSSentencesOn) hdtSentence = nextNMEASentence;
                }

                else if (words[0] == "$PTNL" && words.Length > 8)
                {
                    ParseAVR(ref GPS);
                    if (isGPSSentencesOn) avrSentence = nextNMEASentence;
                }

                else if (words[0] == "$GNTRA")
                {
                    ParseTRA(ref GPS);
                }

                else if (words[0] == "$PSTI" && words[1] == "032")
                {
                    ParseSTI032(ref GPS); //there is also an $PSTI,030,... wich contains different data!
                }
            }// while still data


            if (GPS.vehicle && GPS.isNMEAToSend)
            {
                latitude = GPS.latitude;
                longitude = GPS.longitude;

                GPS.isNMEAToSend = false;

                byte[] nmeaPGN = new byte[57];

                nmeaPGN[0] = 0x80;
                nmeaPGN[1] = 0x81;
                nmeaPGN[2] = 0x7C;
                nmeaPGN[3] = 0xD6;
                nmeaPGN[4] = 0x33; // nmea total array count minus 6

                //longitude
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.longitudeSend), 0, nmeaPGN, 5, 8);
                GPS.longitudeSend = double.MaxValue;

                //latitude
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.latitudeSend), 0, nmeaPGN, 13, 8);
                GPS.latitudeSend = double.MaxValue;

                //the different dual antenna headings
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.headingTrueDual), 0, nmeaPGN, 21, 4);
                GPS.headingTrueDual = float.MaxValue;

                //single antenna heading in degrees
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.headingTrue), 0, nmeaPGN, 25, 4);
                GPS.headingTrue = float.MaxValue;

                //speed converted to kmh from knots
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.speed), 0, nmeaPGN, 29, 4);
                GPS.speed = float.MaxValue;

                //roll value in degrees
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.roll), 0, nmeaPGN, 33, 4);
                GPS.roll = float.MaxValue;

                //altitude in meters
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.altitude), 0, nmeaPGN, 37, 4);
                GPS.altitude = float.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.satellitesTracked), 0, nmeaPGN, 41, 2);
                GPS.satellitesTracked = ushort.MaxValue;

                nmeaPGN[43] = (byte)GPS.fixQuality;
                GPS.fixQuality = byte.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.hdopX100), 0, nmeaPGN, 44, 2);
                GPS.hdopX100 = ushort.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.ageX100), 0, nmeaPGN, 46, 2);
                GPS.ageX100 = ushort.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.imuHeading), 0, nmeaPGN, 48, 2);
                GPS.imuHeading = ushort.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.imuRoll), 0, nmeaPGN, 50, 2);
                GPS.imuRoll = short.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.imuPitch), 0, nmeaPGN, 52, 2);
                GPS.imuPitch = short.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.imuYawRate), 0, nmeaPGN, 54, 2);
                GPS.imuYawRate = short.MaxValue;


                int CK_A = 0;
                for (int j = 2; j < nmeaPGN.Length; j++)
                {
                    CK_A += nmeaPGN[j];
                }

                //checksum
                nmeaPGN[56] = (byte)CK_A;

                //Send nmea to AgOpenGPS
                traffic.cntrPGNToAOG += nmeaPGN.Length;
                SendDataToLoopBack(nmeaPGN, epAgOpen);

                //Send nmea to autosteer module 8888
                if (isSendNMEAToUDP) SendUDPMessage(nmeaPGN, epModule);
            }
            else if (GPS.isNMEAToSend)
            {
                if (!resized && this.Width == 420)
                {
                    resized = true;
                    this.Width = 660;
                }

                GPS.isNMEAToSend = false;

                byte[] nmeaPGN = new byte[35];

                nmeaPGN[0] = 0x80;
                nmeaPGN[1] = 0x81;
                nmeaPGN[2] = 0x7C;
                nmeaPGN[3] = 0xD7;
                nmeaPGN[4] = 29; // nmea total array count minus 6

                //longitude
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.longitudeSend), 0, nmeaPGN, 5, 8);
                GPS.longitudeSend = double.MaxValue;

                //latitude
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.latitudeSend), 0, nmeaPGN, 13, 8);
                GPS.latitudeSend = double.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.satellitesTracked), 0, nmeaPGN, 21, 2);
                GPS.satellitesTracked = ushort.MaxValue;

                nmeaPGN[23] = (byte)GPS.fixQuality;
                GPS.fixQuality = byte.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.hdopX100), 0, nmeaPGN, 24, 2);
                GPS.hdopX100 = ushort.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.ageX100), 0, nmeaPGN, 26, 2);
                GPS.ageX100 = ushort.MaxValue;

                Buffer.BlockCopy(BitConverter.GetBytes(GPS.imuRoll), 0, nmeaPGN, 28, 2);
                GPS.imuRoll = short.MaxValue;

                //the different dual antenna headings
                Buffer.BlockCopy(BitConverter.GetBytes(GPS.headingTrueDual), 0, nmeaPGN, 30, 4);
                GPS.headingTrueDual = float.MaxValue;

                int CK_A = 0;
                for (int j = 2; j < nmeaPGN.Length; j++)
                {
                    CK_A += nmeaPGN[j];
                }

                //checksum
                nmeaPGN[34] = (byte)CK_A;

                //Send nmea to AgOpenGPS
                traffic.cntrPGNToAOG += nmeaPGN.Length;
                SendDataToLoopBack(nmeaPGN, epAgOpen);

                //Send nmea to autosteer module 8888
                if (isSendNMEAToUDP) SendUDPMessage(nmeaPGN, epModule);
            }
        }

        public string Parse(ref NmeaValues GPS)
        {
            string sentence;
            do
            {
                //double check for valid sentence
                // Find start of next sentence
                int start = GPS.rawBuffer.IndexOf("$", StringComparison.Ordinal);
                if (start == -1) return null;
                GPS.rawBuffer = GPS.rawBuffer.Substring(start);

                // Find end of sentence
                int end = GPS.rawBuffer.IndexOf("\r", StringComparison.Ordinal);
                if (end == -1) return null;

                //the NMEA sentence to be parsed
                sentence = GPS.rawBuffer.Substring(0, end + 1);

                //remove the processed sentence from the rawBuffer
                GPS.rawBuffer = GPS.rawBuffer.Substring(end + 1);
            }

            //if sentence has valid checksum, its all good
            while (!ValidateChecksum(sentence));

            // Remove trailing checksum and \r\n and return
            sentence = sentence.Substring(0, sentence.IndexOf("*", StringComparison.Ordinal));

            return sentence;
        }

        private void ParseGGA(ref NmeaValues GPS)
        {
            #region GGA Message
            //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47

            //GGA          Global Positioning System Fix Data
            //123519       Fix taken at 12:35:19 UTC
            //4807.038,N   Latitude 48 deg 07.038' N
            //01131.000,E  Longitude 11 deg 31.000' E
            //1            Fix quality: 0 = invalid
            //                          1 = GPS fix (SPS)
            //                          2 = DGPS fix
            //                          3 = PPS fix
            //                          4 = Real Time Kinematic
            //                          5 = Float RTK
            //                          6 = estimated (dead reckoning) (2.3 feature)
            //                          7 = Manual input mode
            //                          8 = Simulation mode
            //08           Number of satellites being tracked
            //0.9          Horizontal dilution of position
            //545.4,M      Altitude, Meters, above mean sea level
            //46.9,M       Height of geoid (mean sea level) above WGS84 ellipsoid
            //(empty field) time in seconds since last DGPS update
            //(empty field) DGPS station ID number
            //*47          the checksum data, always begins with *
            #endregion GGA Message

            if (!string.IsNullOrEmpty(words[1]) && !string.IsNullOrEmpty(words[2]) && !string.IsNullOrEmpty(words[3])
                && !string.IsNullOrEmpty(words[4]) && !string.IsNullOrEmpty(words[5]))
            {
                //FixQuality
                byte.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.fixQuality);
                GPS.fixQualityData = GPS.fixQuality;

                //satellites tracked
                ushort.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.satellitesTracked);
                GPS.satellitesData = GPS.satellitesTracked;

                //hdop
                float.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.hdopData);
                GPS.hdopX100 = (ushort)(GPS.hdopData * 100.0);

                //altitude
                float.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.altitude);
                GPS.altitudeData = GPS.altitude;

                //age
                float.TryParse(words[13], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.ageData);
                GPS.ageX100 = (ushort)(GPS.ageData * 100.0);

                //get latitude and convert to decimal degrees
                int decim = words[2].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[2] += ".00";
                    decim = words[2].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[2].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude);
                double.TryParse(words[2].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                temp *= 0.01666666666667;
                latitude += temp;
                if (words[3] == "S")
                    latitude *= -1;
                GPS.latitude = GPS.latitudeSend = latitude;

                //get longitude and convert to decimal degrees
                decim = words[4].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[4] += ".00";
                    decim = words[4].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[4].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude);
                double.TryParse(words[4].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                longitude += temp * 0.0166666666667;

                { if (words[5] == "W") longitude *= -1; }
                GPS.longitude = GPS.longitudeSend = longitude;

                if (lastSentence == "GGA") GPS.isNMEAToSend = true;
            }
        }

        private void ParseVTG(ref NmeaValues GPS)
        {
            #region VTG Message
            //$GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48

            //VTG          Track made good and ground speed
            //054.7,T      True track made good (degrees)
            //034.4,M      Magnetic track made good
            //005.5,N      Ground speed, knots
            //010.2,K      Ground speed, Kilometers per hour
            //*48          Checksum
            #endregion VTG Message


            if (!string.IsNullOrEmpty(words[7]))
            {
                //kph for speed
                float.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.speed);
                GPS.speedData = GPS.speed;
            }
            else
            {
                GPS.speed = GPS.speedData = 0;
            }

            if (!string.IsNullOrEmpty(words[1]))
            {
                //True heading
                float.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrue);
                GPS.headingTrueData = GPS.headingTrue;
            }

            if (lastSentence == "VTG") GPS.isNMEAToSend = true;
        }

        private void ParsePANDA(ref NmeaValues GPS)
        {
            #region PANDA Message
            /*
            $PANDA
            (1) Time of fix

            position
            (2,3) 4807.038,N Latitude 48 deg 07.038' N
            (4,5) 01131.000,E Longitude 11 deg 31.000' E

            (6) 1 Fix quality: 
                0 = invalid
                1 = GPS fix(SPS)
                2 = DGPS fix
                3 = PPS fix
                4 = Real Time Kinematic
                5 = Float RTK
                6 = estimated(dead reckoning)(2.3 feature)
                7 = Manual input mode
                8 = Simulation mode
            (7) Number of satellites being tracked
            (8) 0.9 Horizontal dilution of position
            (9) 545.4 Altitude (ALWAYS in Meters, above mean sea level)
            (10) 1.2 time in seconds since last DGPS update
            (11) Speed in knots

            FROM IMU:
            (12) Heading in degrees
            (13) Roll angle in degrees(positive roll = right leaning - right down, left up)
            
            (14) Pitch angle in degrees(Positive pitch = nose up)
            (15) Yaw Rate in Degrees / second

            * CHKSUM
            */
            #endregion PANDA Message

            if (!string.IsNullOrEmpty(words[1]) && !string.IsNullOrEmpty(words[2]) && !string.IsNullOrEmpty(words[3])
                && !string.IsNullOrEmpty(words[3]) && !string.IsNullOrEmpty(words[0]))
            {

                //get latitude and convert to decimal degrees
                int decim = words[2].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[2] += ".00";
                    decim = words[2].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[2].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude);
                double.TryParse(words[2].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                temp *= 0.01666666666666666666666666666667;
                latitude += temp;
                if (words[3] == "S")
                    latitude *= -1;

                GPS.latitude = GPS.latitudeSend = latitude;

                //get longitude and convert to decimal degrees
                decim = words[4].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[4] += ".00";
                    decim = words[4].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[4].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude);
                double.TryParse(words[4].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                longitude += temp * 0.01666666666666666666666666666667;

                { if (words[5] == "W") longitude *= -1; }
                GPS.longitude = GPS.longitudeSend = longitude;

                //FixQuality
                byte.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.fixQuality);
                GPS.fixQualityData = GPS.fixQuality;

                //satellites tracked
                ushort.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.satellitesTracked);
                GPS.satellitesData = GPS.satellitesTracked;

                //hdop
                float.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.hdopData);
                GPS.hdopX100 = (ushort)(GPS.hdopData * 100.0);

                //altitude
                float.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.altitude);
                GPS.altitudeData = GPS.altitude;

                //age
                float.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.ageData);
                GPS.ageX100 = (ushort)(GPS.ageData * 100.0);

                //kph for speed - knots read
                float.TryParse(words[11], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.speed);
                GPS.speed *= 1.852f;
                GPS.speedData = GPS.speed;

                //imu heading
                ushort.TryParse(words[12], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.imuHeading);
                GPS.imuHeadingData = GPS.imuHeading;

                //roll
                short.TryParse(words[13], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.imuRoll);
                GPS.imuRollData = GPS.imuRoll;

                //Pitch
                short.TryParse(words[14], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.imuPitch);
                GPS.imuPitchData = GPS.imuPitch;

                //YawRate
                short.TryParse(words[15], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.imuYawRate);
                GPS.imuYawRateData = GPS.imuYawRate;

                //always send because its probably the only one.
                GPS.isNMEAToSend = true;
                //}
            }
        }

        private void ParseKSXT(ref NmeaValues GPS)
        {
            if (!string.IsNullOrEmpty(words[1]) && !string.IsNullOrEmpty(words[2]) && !string.IsNullOrEmpty(words[3])
                && !string.IsNullOrEmpty(words[4]) && !string.IsNullOrEmpty(words[5]))
            {
                double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.longitude);
                GPS.longitudeSend = GPS.longitude;

                double.TryParse(words[3], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.latitude);
                GPS.latitudeSend = GPS.latitude;

                float.TryParse(words[4], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.altitude);
                GPS.altitudeData = GPS.altitude;

                float.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrueDual);
                GPS.headingTrueDualData = GPS.headingTrueDual;

                float.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out rollK);

                float.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.speed);
                GPS.speedData = GPS.speed;

                byte.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out byte fixQuality);
                if (fixQuality == 0) GPS.fixQualityData = 0;
                else if (fixQuality == 1) GPS.fixQualityData = 1;
                else if (fixQuality == 2) GPS.fixQualityData = 5;
                else if (fixQuality == 3) GPS.fixQualityData = 4;

                GPS.fixQuality = GPS.fixQualityData;

                int headingQuality;

                int.TryParse(words[11], NumberStyles.Float, CultureInfo.InvariantCulture, out headingQuality);

                if (headingQuality == 3)   // roll only when rtk 
                {
                    GPS.roll = (float)(rollK);
                    GPS.rollData = rollK;
                }
                else
                {
                    GPS.roll = float.MinValue;
                    GPS.rollData = 0;
                }

                ushort.TryParse(words[13], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.satellitesTracked);
                GPS.satellitesData = GPS.satellitesTracked;


                float.TryParse(words[20], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.ageData);
                GPS.ageX100 = (ushort)(GPS.ageData * 100.0);

                GPS.isNMEAToSend = true;
            }
        }

        private void ParseAVR(ref NmeaValues GPS)
        {
            #region AVR Message
            // $PTNL,AVR,145331.50,+35.9990,Yaw,-7.8209,Tilt,-0.4305,Roll,444.232,3,1.2,17 * 03

            //0 Message ID $PTNL,AVR
            //1 UTC of vector fix
            //2 Yaw angle, in degrees
            //3 Yaw
            //4 Tilt angle, in degrees
            //5 Tilt
            //6 Roll angle, in degrees
            //7 Roll
            //8 Range, in meters
            //9 GPS quality indicator:
            // 0: Fix not available or invalid
            // 1: Autonomous GPS fix
            // 2: Differential carrier phase solution RTK(Float)
            // 3: Differential carrier phase solution RTK(Fix)
            // 4: Differential code-based solution, DGPS
            //10 PDOP
            //11 Number of satellites used in solution
            //12 The checksum data, always begins with *
            #endregion AVR Message

            if (!string.IsNullOrEmpty(words[1]))
            {
                float.TryParse(words[8] == "Roll" ? words[7] : words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out rollK);

                //Kalman filter
                Pc = P + varProcess;
                G = Pc / (Pc + varRoll);
                P = (1 - G) * Pc;
                Xp = XeRoll;
                Zp = Xp;
                XeRoll = (G * (rollK - Zp)) + Xp;
                GPS.rollData = XeRoll;

                GPS.roll = (float)(XeRoll);

                if (lastSentence == "AVR") GPS.isNMEAToSend = true;
            }
        }

        private void ParseHPD(ref NmeaValues GPS)
        {
            if (!string.IsNullOrEmpty(words[1]))
            {
                //Dual heading
                float.TryParse(words[3], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrueDual);
                GPS.headingTrueDualData = GPS.headingTrueDual;

                float.TryParse(words[4], NumberStyles.Float, CultureInfo.InvariantCulture, out rollK);

                double.TryParse(words[18], NumberStyles.Float, CultureInfo.InvariantCulture, out double baseline);

                //int.TryParse(words[21], NumberStyles.Float, CultureInfo.InvariantCulture, out int solheading);

                if (baseline > 0)   // roll only when rtk and baseline
                {
                    GPS.roll = (float)(rollK);
                    GPS.rollData = rollK;
                }
                else
                {
                    GPS.roll = float.MinValue;
                    GPS.rollData = 0;
                }

                if (lastSentence == "HPD") GPS.isNMEAToSend = true;
            }
        }

        private void ParseOGI(ref NmeaValues GPS)
        {
            #region PAOGI Message
            /*
            $PAOGI
            (1) 123519 Fix taken at 1219 UTC

            Roll corrected position
            (2,3) 4807.038,N Latitude 48 deg 07.038' N
            (4,5) 01131.000,E Longitude 11 deg 31.000' E

            (6) 1 Fix quality: 
                0 = invalid
                1 = GPS fix(SPS)
                2 = DGPS fix
                3 = PPS fix
                4 = Real Time Kinematic
                5 = Float RTK
                6 = estimated(dead reckoning)(2.3 feature)
                7 = Manual input mode
                8 = Simulation mode
            (7) Number of satellites being tracked
            (8) 0.9 Horizontal dilution of position
            (9) 545.4 Altitude (ALWAYS in Meters, above mean sea level)
            (10) 1.2 time in seconds since last DGPS update

            (11) 022.4 Speed over the ground in knots - can be positive or negative

            FROM AHRS:
            (12) Heading in degrees
            (13) Roll angle in degrees(positive roll = right leaning - right down, left up)
            (14) Pitch angle in degrees(Positive pitch = nose up)
            (15) Yaw Rate in Degrees / second

            * CHKSUM
            */
            #endregion PAOGI Message

            if (!string.IsNullOrEmpty(words[1]) && !string.IsNullOrEmpty(words[2]) && !string.IsNullOrEmpty(words[3])
                && !string.IsNullOrEmpty(words[4]) && !string.IsNullOrEmpty(words[5]))
            {
                //double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double UTC);
                //if ((UTC < LastUpdateUTC ? 240000 + UTC : UTC) - LastUpdateUTC > 0.045)
                //{

                //FixQuality
                byte.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.fixQuality);
                GPS.fixQualityData = GPS.fixQuality;

                //satellites tracked
                ushort.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.satellitesTracked);
                GPS.satellitesData = GPS.satellitesTracked;

                //hdop
                float.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.hdopData);
                GPS.hdopX100 = (ushort)(GPS.hdopData * 100.0);

                //altitude
                float.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.altitude);
                GPS.altitudeData = GPS.altitude;

                //kph for speed - knots read
                float.TryParse(words[11], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.speed);
                GPS.speed *= 1.852f;
                GPS.speedData = GPS.speed;

                //Dual antenna derived heading
                float.TryParse(words[12], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrueDual);
                GPS.headingTrueDualData = GPS.headingTrueDual;

                //roll
                float.TryParse(words[13], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.roll);
                GPS.rollData = GPS.roll;

                //get latitude and convert to decimal degrees
                int decim = words[2].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[2] += ".00";
                    decim = words[2].IndexOf(".", StringComparison.Ordinal);
                }

                //age
                float.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.ageData);
                GPS.ageX100 = (ushort)(GPS.ageData * 100.0);

                decim -= 2;
                double.TryParse(words[2].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude);
                double.TryParse(words[2].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                temp *= 0.01666666666666666666666666666667;
                latitude += temp;
                if (words[3] == "S")
                    latitude *= -1;

                GPS.latitude = GPS.latitudeSend = latitude;

                //get longitude and convert to decimal degrees
                decim = words[4].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[4] += ".00";
                    decim = words[4].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[4].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude);
                double.TryParse(words[4].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                longitude += temp * 0.01666666666666666666666666666667;

                { if (words[5] == "W") longitude *= -1; }
                GPS.longitude = GPS.longitudeSend = longitude;

                //always send because its probably the only one.
                GPS.isNMEAToSend = true;
                //}
            }
        }

        private void ParseHDT(ref NmeaValues GPS)
        {
            #region HDT Message
            //$GNHDT,123.456,T * 00

            //(0)   Message ID $GNHDT
            //(1)   Heading in degrees
            //(2)   T: Indicates heading relative to True North
            //(3)   The checksum data, always begins with *
            #endregion HDT Message

            if (!string.IsNullOrEmpty(words[1]))
            {
                //True heading
                float.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrueDual);
                GPS.headingTrueDualData = GPS.headingTrueDual;

                if (lastSentence == "HDT") GPS.isNMEAToSend = true;
            }
        }

        private void ParseSTI032(ref NmeaValues GPS) //heading and roll from SkyTraQ receiver
        {
            #region STI0 Message
            //$PSTI,032,033010.000,111219,A,R,‐4.968,‐10.817,‐1.849,12.046,204.67,,,,,*39

            //(1) 032 Baseline Data indicator
            //(2) UTC time hhmmss.sss
            //(3) UTC date ddmmyy
            //(4) Status:
            //    V = Void
            //    A = Active
            //(5) Mode Indicator:
            //    F = Float RTK
            //    R = fixed RTK
            //(6) East-projection of baseline, meters
            //(7) North-projection of baseline, meters
            //(8) Up-projection of baseline, meters
            //(9) Baseline length, meters
            //(10) Baseline course: angle between baseline vector and north direction, degrees
            //(11) - (15) Reserved
            //(16) * Checksum
            #endregion STI0 Message

            if (!string.IsNullOrEmpty(words[10]))
            {
                //baselineCourse: angle between baseline vector (from kinematic base to rover) and north direction, degrees
                float.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out float baselineCourse);
                GPS.headingTrueDual = ((baselineCourse < 270.0f) ? (baselineCourse + 90.0f) : (baselineCourse - 270.0f)); //Rover Antenna on the left, kinematic base on the right!!!
            }

            if (!string.IsNullOrEmpty(words[8]) && !string.IsNullOrEmpty(words[9]))
            {
                double.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out double upProjection); //difference in hight of both antennas (rover - kinematic base)
                double.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out double baselineLength); //distance between kinematic base and rover
                rollK = (float)glm.toDegrees(Math.Atan(upProjection / baselineLength)); //roll to the right is positiv (rover left, kinematic base right!)

                //Kalman filter
                Pc = P + varProcess;
                G = Pc / (Pc + varRoll);
                P = (1 - G) * Pc;
                Xp = XeRoll;
                Zp = Xp;
                XeRoll = (G * (rollK - Zp)) + Xp;
                GPS.rollData = XeRoll;

                GPS.roll = (float)(XeRoll);

                if (lastSentence == "STI") GPS.isNMEAToSend = true;
            }
        }

        private void ParseTRA(ref NmeaValues GPS)  //tra contains hdt and roll for the ub482 receiver
        {
            if (!string.IsNullOrEmpty(words[1]))
            {
                float.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out GPS.headingTrueDual);
                GPS.headingTrueDualData = GPS.headingTrueDual;

                //  Console.WriteLine(HeadingForced);
                float.TryParse(words[3], NumberStyles.Float, CultureInfo.InvariantCulture, out rollK);
                // Console.WriteLine(nRoll);

                int.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out int trasolution);
                if (trasolution != 4) rollK = 0;
                GPS.rollData = rollK;
                GPS.roll = (float)(rollK);

                if (lastSentence == "TRA") GPS.isNMEAToSend = true;

            }
        }

        private void ParseRMC()
        {
            #region RMC Message
            //$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A

            //RMC          Recommended Minimum sentence C
            //123519       Fix taken at 12:35:19 UTC
            //A            Status A=active or V=Void.
            //4807.038,N   Latitude 48 deg 07.038' N
            //01131.000,E  Longitude 11 deg 31.000' E
            //022.4        Speed over the ground in knots
            //084.4        Track angle in degrees True
            //230394       Date - 23rd of March 1994
            //003.1,W      Magnetic Variation
            //*6A          * Checksum
            #endregion RMC Message
            /*
            if (!string.IsNullOrEmpty(words[1]) && !string.IsNullOrEmpty(words[3]) && !string.IsNullOrEmpty(words[4])
                && !string.IsNullOrEmpty(words[5]) && !string.IsNullOrEmpty(words[6]))
            {
                //Convert from knots to kph for speed
                float.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out speed);
                speed *= 1.852f;
                speedData = speed;

                //True heading
                float.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out headingTrueDual);
                headingTrueDualData = headingTrueDual;

                //get latitude and convert to decimal degrees
                int decim = words[3].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[3] += ".00";
                    decim = words[3].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[3].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out latitude);
                double.TryParse(words[3].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                latitude += temp * 0.01666666666666666666666666666667;

                if (words[4] == "S")
                    latitude *= -1;
                latitudeSend = latitude;

                //get longitude and convert to decimal degrees
                decim = words[5].IndexOf(".", StringComparison.Ordinal);
                if (decim == -1)
                {
                    words[5] += ".00";
                    decim = words[5].IndexOf(".", StringComparison.Ordinal);
                }

                decim -= 2;
                double.TryParse(words[5].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out longitude);
                double.TryParse(words[5].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                longitude += temp * 0.01666666666666666666666666666667;

                if (words[6] == "W") longitude *= -1;
                longitudeSend = longitude;

                isNMEAToSend = true;
            }
            */
        }

        public bool ValidateChecksum(string Sentence)
        {
            int sum = 0;
            try
            {
                char[] sentenceChars = Sentence.ToCharArray();
                // All character xor:ed results in the trailing hex checksum
                // The checksum calc starts after '$' and ends before '*'

                int inx = Sentence.IndexOf("*", StringComparison.Ordinal);

                if (sentenceChars.Length - inx == 4)
                {

                    for (inx = 1; ; inx++)
                    {
                        if (inx >= sentenceChars.Length) // No checksum found
                            return false;
                        var tmp = sentenceChars[inx];
                        // Indicates end of data and start of checksum
                        if (tmp == '*') break;
                        sum ^= tmp;    // Build checksum
                    }

                    // Calculated checksum converted to a 2 digit hex string
                    string sumStr = string.Format("{0:X2}", sum);

                    // Compare to checksum in sentence
                    return sumStr.Equals(Sentence.Substring(inx + 1, 2));
                }
                else
                {
                    //CRC code goes here - return true for now if $KS
                    if (sentenceChars[0] == 36 && sentenceChars[1] == 75 && sentenceChars[2] == 83) return true;
                    else return false;
                }
            }
            catch (Exception)
            {
                //mf.WriteErrorLog("Validate Checksum" + e);
                return false;
            }
        }
    }
}



