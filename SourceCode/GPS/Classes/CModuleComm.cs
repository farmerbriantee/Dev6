using System.Text;

namespace AgOpenGPS
{
    public class CModuleComm
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //WGS84 Lat Long
        public double latitude, longitude, latitudeTool, longitudeTool;

        //our current fix
        public vec2 fix = new vec2(0, 0);
        public vec2 fixTool = new vec2(0, 0);

        //other GIS Info
        public double altitude, speed = double.MaxValue, avgSpeed, panicStopSpeed;

        public double headingTrueDual = double.MaxValue, headingTrueDualTool, headingTrue, hdop, age, hdopTool, ageTool, headingTrueDualOffset;

        public int fixQuality, fixQualityTool, ageAlarm;
        public int satellitesTracked, satellitesTrackedTool;

        public StringBuilder logNMEASentence = new StringBuilder();

        //Roll and heading from the IMU
        public double imuHeading = 99999, imuHeadingTool = 99999, imuRoll = 0, imuRollTool, imuPitch = 0, imuYawRate = 0;

        //actual value in degrees
        public double rollZero, rollZeroTool;

        //Roll Filter Value
        public double rollFilter;

        //is the auto steer in auto turn on mode or not
        public bool isRollInvert, isDualAsIMU, isReverseOn;

        //the factor for fusion of GPS and IMU
        public double forwardComp, reverseComp, fusionWeight, sideHillCompFactor;

        //Critical Safety Properties
        public bool isOutOfBounds = false;

        // ---- Section control switches to AOG  ---------------------------------------------------------
        //PGN - 32736 - 127.249 0x7FF9
        public byte[] ssP = new byte[3];

        public int
            swHeader = 0,
            swMain = 5,
            swReserve = 6,
            swReserve2 = 7,
            swNumSections = 8,
            swOnGr0 = 9,
            swOffGr0 = 10,
            swOnGr1 = 11,
            swOffGr1 = 12;

        public int pwmDisplay = 0;
        public double actualSteerAngleDegrees = 0;
        public int actualSteerAngleChart = 0, sensorData = -1;

        public int toolPWM = 0, toolStatus = 0;
        public double toolActualDistance = 0, toolError = 0;

        //for the workswitch
        public bool isWorkSwitchEnabled, isWorkSwitchManual, isWorkSwitchActiveLow;
        public bool isSteerSwitchEnabled, isSteerSwitchManual, isAutoSteerAuto;

        public bool workSwitchHigh, oldWorkSwitchHigh, steerSwitchHigh, oldSteerSwitchHigh;

        //constructor
        public CModuleComm(FormGPS _f)
        {
            mf = _f;
        }

        public void LoadSettings()
        {
            isWorkSwitchEnabled = Properties.Settings.Default.setF_IsWorkSwitchEnabled;
            isWorkSwitchManual = Properties.Settings.Default.setF_IsWorkSwitchManual;
            isWorkSwitchActiveLow = Properties.Settings.Default.setF_IsWorkSwitchActiveLow;

            isSteerSwitchEnabled = Properties.Settings.Default.setF_IsSteerSwitchEnabled;
            isSteerSwitchManual = Properties.Settings.Default.setF_steerControlsManual;
            isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn;

            ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
            rollZero = Properties.Settings.Default.setIMU_rollZero;
            rollFilter = Properties.Settings.Default.setIMU_rollFilter;
            fusionWeight = Properties.Settings.Default.setIMU_fusionWeight;
            forwardComp = Properties.Settings.Default.setGPS_forwardComp;
            reverseComp = Properties.Settings.Default.setGPS_reverseComp;
            isRollInvert = Properties.Settings.Default.setIMU_invertRoll;
            isDualAsIMU = Properties.Settings.Default.setIMU_isDualAsIMU;
            isReverseOn = Properties.Settings.Default.setIMU_isReverseOn;
            sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;
        }

        //Called from "OpenGL.Designer.cs" when requied
        public void CheckWorkAndSteerSwitch()
        {
            if (steerSwitchHigh != oldSteerSwitchHigh)
            {
                oldSteerSwitchHigh = steerSwitchHigh;

                if (isAutoSteerAuto)
                {
                    mf.setBtnAutoSteer(!steerSwitchHigh); //steerSwith is active low
                }

                if (isSteerSwitchEnabled)
                {
                    if ((mf.isAutoSteerBtnOn && isAutoSteerAuto) || !isAutoSteerAuto && !steerSwitchHigh)
                        mf.setSectionBtnState(isWorkSwitchManual ? btnStates.On : btnStates.Auto);
                    else
                        mf.setSectionBtnState(btnStates.Off);
                }
            }

            if (isWorkSwitchEnabled && workSwitchHigh != oldWorkSwitchHigh)
            {
                oldWorkSwitchHigh = workSwitchHigh;

                if (workSwitchHigh != isWorkSwitchActiveLow)
                    mf.setSectionBtnState(isWorkSwitchManual ? btnStates.On : btnStates.Auto);
                else//Checks both on-screen buttons, performs click if button is not off
                    mf.setSectionBtnState(btnStates.Off);
            }
        }
    }
}