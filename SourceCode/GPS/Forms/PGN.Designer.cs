﻿using System;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //AutoSteerData
        public class CPGN_FE
        {
            /// <summary>
            /// 8 bytes
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFE, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int speedLo = 5;
            public int speedHi = 6;
            public int status = 7;
            public int steerAngleLo = 8;
            public int steerAngleHi = 9;
            public int lineDistance  = 10;
            public int sc1to8 = 11;
            public int sc9to16 = 12;

            public void Reset()
            {
            }
        }

        public class CPGN_FD
        {
            /// <summary>
            /// From steer module
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFD, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int actualLo = 5;
            public int actualHi = 6;
            public int headLo = 7;
            public int headHi = 8;
            public int rollLo = 9;
            public int rollHi = 10;
            public int switchStatus = 11;
            public int pwm = 12;

            public void Reset()
            {
            }
        }


        //AutoSteer Settings
        public class CPGN_FC
        {
            /// <summary>
            /// PGN - 252 - FC gainProportional=5 HighPWM=6  LowPWM = 7 MinPWM = 8 
            /// CountsPerDegree = 9 wasOffsetHi = 10 wasOffsetLo = 11 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFC, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int gainProportional = 5;
            public int highPWM = 6;
            public int lowPWM = 7;
            public int minPWM = 8;
            public int countsPerDegree = 9;
            public int wasOffsetLo = 10;
            public int wasOffsetHi = 11;
            public int ackerman = 12;

            public CPGN_FC()
            {
                pgn[gainProportional] = Properties.Settings.Default.setAS_Kp;
                pgn[highPWM] = Properties.Settings.Default.setAS_highSteerPWM;
                pgn[lowPWM] = Properties.Settings.Default.setAS_lowSteerPWM;
                pgn[minPWM] = Properties.Settings.Default.setAS_minSteerPWM;
                pgn[countsPerDegree] = Properties.Settings.Default.setAS_countsPerDegree;
                pgn[wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));;
                pgn[wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_Kp));
                pgn[ackerman] = Properties.Settings.Default.setAS_ackerman;
            }

            public void Reset()
            {
            }
        }

        //Autosteer Board Config
        public class CPGN_FB
        {
            /// <summary>
            /// 
            /// PGN - 251 - FB 
            /// set0=5 maxPulse = 6 minSpeed = 7 ackermanFix = 8
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFB, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int set0 = 5;
            public int maxPulse = 6;
            public int minSpeed = 7;
            public int set1 = 8;
            //public int  = 10;
            //public int  = 11;
            //public int  = 12;

            public CPGN_FB()
            {
                pgn[set0] = 0;
                pgn[maxPulse] = 0;
                pgn[minSpeed] = 0;
                pgn[set1] = 0;
            }

            public void Reset()
            {
            }
        }

        //Machine Data
        public class CPGN_EF
        {
            /// <summary>
            /// PGN - 239 - EF 
            /// uturn=5  tree=6  hydLift = 8 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEF, 8, 0, 0, 0, 0, 0, 0, 0, 0,  0xCC };
            public int uturn = 5;
            public int speed = 6;
            public int hydLift = 7;
            public int tram = 8;
            public int geoStop = 9; //out of bounds etc
            //public int  = 10;
            public int  sc1to8= 11;
            public int  sc9to16= 12;

            public CPGN_EF()
            {
            }

            public void Reset()
            {
            }
        }

        //Machine Config
        public class CPGN_EE
        {
            /// <summary>
            /// PGN - 238 - EE 
            /// raiseTime=5  lowerTime=6   enableHyd= 7 set0 = 8
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEE, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int raiseTime = 5;
            public int lowerTime = 6;
            public int enableHyd = 7;
            public int set0 = 8;
            public int user1 = 9;
            public int user2 = 10;
            public int user3 = 11;
            public int user4  = 12;

            // PGN  - 127.239 0x7FEF
            int crc = 0;

            public CPGN_EE()
            {
                pgn[raiseTime] = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
                pgn[lowerTime] = Properties.Vehicle.Default.setArdMac_hydLowerTime;
                pgn[enableHyd] = Properties.Vehicle.Default.setArdMac_isHydEnabled;
                pgn[set0] = Properties.Vehicle.Default.setArdMac_setting0;

                pgn[user1] = Properties.Vehicle.Default.setArdMac_user1;
                pgn[user2] = Properties.Vehicle.Default.setArdMac_user2;
                pgn[user3] = Properties.Vehicle.Default.setArdMac_user3;
                pgn[user4] = Properties.Vehicle.Default.setArdMac_user4;
            }

            public void MakeCRC()
            {
                crc = 0;
                for (int i = 2; i < pgn.Length - 1; i++)
                {
                    crc += pgn[i];
                }
                pgn[pgn.Length - 1] = (byte)crc;
            }

            public void Reset()
            {
            }
        }

        //Relay Config
        public class CPGN_EC
        {
            /// <summary>
            /// PGN - 236 - EC
            /// Pin conifg 1 to 20
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEC, 24,
                                        0, 0, 0, 0, 0, 0, 0, 0, 
                                        0, 0, 0, 0, 0, 0, 0, 0, 
                                        0, 0, 0, 0, 0, 0, 0, 0, 0xCC };

            //where in the pgn is which pin
            public int pin0 = 5;
            public int pin1 = 6;
            public int pin2 = 7;
            public int pin3 = 8;
            public int pin4 = 9;
            public int pin5 = 10;
            public int pin6 = 11;
            public int pin7 = 12;
            public int pin8 = 13;
            public int pin9 = 14;

            public int pin10 = 15;
            public int pin11 = 16;
            public int pin12 = 17;
            public int pin13 = 18;
            public int pin14 = 19;
            public int pin15 = 20;
            public int pin16 = 21;

            public int pin17 = 22;
            public int pin18 = 23;
            public int pin19 = 24;
            public int pin20 = 25;
            public int pin21 = 26;
            public int pin22 = 27;
            public int pin23 = 28;

            // PGN  - 127.237 0x7FED
            int crc = 0;

            public CPGN_EC()
            {
                string [] words;

                words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

                pgn[pin0] = (byte)int.Parse(words[0]);
                pgn[pin1] = (byte)int.Parse(words[1]);
                pgn[pin2] = (byte)int.Parse(words[2]);
                pgn[pin3] = (byte)int.Parse(words[3]);
                pgn[pin4] = (byte)int.Parse(words[4]);
                pgn[pin5] = (byte)int.Parse(words[5]);
                pgn[pin6] = (byte)int.Parse(words[6]);
                pgn[pin7] = (byte)int.Parse(words[7]);
                pgn[pin8] = (byte)int.Parse(words[8]);
                pgn[pin9] = (byte)int.Parse(words[9]);

                pgn[pin10] = (byte)int.Parse(words[10]);
                pgn[pin11] = (byte)int.Parse(words[11]);
                pgn[pin12] = (byte)int.Parse(words[12]);
                pgn[pin13] = (byte)int.Parse(words[13]);
                pgn[pin14] = (byte)int.Parse(words[14]);
                pgn[pin15] = (byte)int.Parse(words[15]);
                pgn[pin16] = (byte)int.Parse(words[16]);
                pgn[pin17] = (byte)int.Parse(words[17]);
                pgn[pin18] = (byte)int.Parse(words[18]);
                pgn[pin19] = (byte)int.Parse(words[19]);

                pgn[pin20] = (byte)int.Parse(words[20]);
                pgn[pin21] = (byte)int.Parse(words[21]);
                pgn[pin22] = (byte)int.Parse(words[22]);
                pgn[pin23] = (byte)int.Parse(words[23]);

            }

            public void MakeCRC()
            {
                crc = 0;
                for (int i = 2; i < pgn.Length - 1; i++)
                {
                    crc += pgn[i];
                }
                pgn[pgn.Length - 1] = (byte)crc;
            }

            public void Reset()
            {
            }
        }

        //Tool Steer Data 233
        public class CPGN_E9
        {
            /// <summary>
            /// PGN - 233 - E9 - Tool Steer Data
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE9, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int lowXTE = 5;
            public int highXTE = 6;
            public int status = 7;
            public int lowVehXTE = 8;
            public int highVehXTE = 9;
            public int speed = 10;
            //public int sc1to8 = 11;
            //public int sc9to16 = 12;

            public CPGN_E9()
            {
            }
        }

        //Tool Steer Config 232
        public class CPGN_E8
        {
            /// <summary>
            /// PGN - 232 - E8 - Tool Steer Config
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE8, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int P = 5;
            public int I = 6;
            public int minPWM = 7;
            public int highPWM = 8;
            public int windup = 9;
            public int wasCounts = 10;
            public int wasOffset = 11;
            public int maxSteer = 12;

            public CPGN_E8()
            {
            }
        }

        //Tool steer Board Config 231
        public class CPGN_E7
        {
            /// <summary>
            /// 
            /// PGN - 231 - E7 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE7, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int set0 = 5;
            //public int maxPulse = 6;
            //public int minSpeed = 7;
            //public int set1 = 8;
            //public int angVel = 9;
            //public int  = 10;
            //public int  = 11;
            //public int  = 12;

            public CPGN_E7()
            {
                pgn[set0] = 0;
                //pgn[maxPulse] = 0;
                //pgn[minSpeed] = 0;
                //pgn[set1] = 0;
                //pgn[angVel] = 0;
            }
        }



        //pgn instances

        /// <summary>
        /// autoSteerData - FE - 254 - 
        /// </summary>
        public CPGN_FE p_254 = new CPGN_FE();

        /// <summary>
        /// autoSteerSettings PGN - 252 - FC
        /// </summary>
        public CPGN_FC p_252 = new CPGN_FC();

        /// <summary>
        /// autoSteerConfig PGN - 251 - FB
        /// </summary>
        public CPGN_FB p_251 = new CPGN_FB();


        /// <summary>
        /// machineData PGN - 239 - EF
        /// </summary>
        public CPGN_EF p_239 = new CPGN_EF();

        /// <summary>
        /// machineConfig PGN - 238 - EE
        /// </summary>
        public CPGN_EE p_238 = new CPGN_EE();

        /// <summary>
        /// relayConfig PGN - 236 - EC
        /// </summary>
        public CPGN_EC p_236 = new CPGN_EC();

        /// <summary>
        /// Tool Steer Data PGN - 235 - EB
        /// </summary>
        public CPGN_E9 p_233 = new CPGN_E9();

        /// <summary>
        /// ToolSteerConfig PGN - 233 - E9
        /// </summary>
        public CPGN_E8 p_232 = new CPGN_E8();

        /// <summary>
        /// ToolSteerSettings PGN - 232 - E8
        /// </summary>
        public CPGN_E7 p_231 = new CPGN_E7();
    }
}
    
