﻿namespace AgOpenGPS
{
    public class CModuleComm
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //Critical Safety Properties
        public bool isOutOfBounds = true;

        // ---- Section control switches to AOG  ---------------------------------------------------------
        //PGN - 32736 - 127.249 0x7FF9
        public byte[] ss = new byte[9];
        public byte[] ssP = new byte[9];
        public int
            swHeader = 0,
            swMain = 1,
            swReserve = 2,
            swReserve2 = 3,
            swNumSections = 4,
            swOnGr0 = 5,
            swOffGr0 = 6,
            swOnGr1 = 7,
            swOffGr1 = 8;

        public int pwmDisplay = 0;
        public double actualSteerAngleDegrees = 0;
        public int actualSteerAngleChart = 0, sensorData = -1;

        //for the workswitch
        public bool isWorkSwitchActiveLow, isWorkSwitchEnabled, isWorkSwitchManual, isSteerControlsManual;

        public bool workSwitchHigh, oldWorkSwitchHigh, steerSwitchHigh, oldsteerSwitchHigh;

        //constructor
        public CModuleComm(FormGPS _f)
        {
            mf = _f;
            //WorkSwitch logic
            isWorkSwitchEnabled = false;

            //does a low, grounded out, mean on
            isWorkSwitchActiveLow = true;
        }

        //Called from "OpenGL.Designer.cs" when requied
        public void CheckWorkAndSteerSwitch()
        {
            //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
            if (mf.ahrs.isAutoSteerAuto && steerSwitchHigh != oldsteerSwitchHigh)
            {
                oldsteerSwitchHigh = steerSwitchHigh;
                //steerSwith is active low
                if (steerSwitchHigh == mf.isAutoSteerBtnOn)
                    mf.btnAutoSteer.PerformClick();
            }

            if (isSteerControlsManual) workSwitchHigh = steerSwitchHigh;

            if ((isWorkSwitchEnabled || isSteerControlsManual) && workSwitchHigh != oldWorkSwitchHigh)
            {
                oldWorkSwitchHigh = workSwitchHigh;

                if (workSwitchHigh != isWorkSwitchActiveLow)
                {
                    if (isWorkSwitchManual)
                    {
                        if (mf.manualBtnState != FormGPS.btnStates.On)
                            mf.btnManualOffOn.PerformClick();
                    }
                    else if (mf.autoBtnState != FormGPS.btnStates.Auto)
                        mf.btnSectionOffAutoOn.PerformClick();
                }
                else//Checks both on-screen buttons, performs click if button is not off
                {
                    if (mf.autoBtnState != FormGPS.btnStates.Off)
                        mf.btnSectionOffAutoOn.PerformClick();
                    if (mf.manualBtnState != FormGPS.btnStates.Off)
                        mf.btnManualOffOn.PerformClick();
                }
            }
        }
    }
}