//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormGPSData : Form
    {
        private readonly FormLoop mf = null;

        public FormGPSData(Form callingForm)
        {
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NmeaValues GPS = mf.VehicleGPS;

            lblLatitude.Text = GPS.latitude.ToString("N7");
            lblLongitude.Text = GPS.longitude.ToString("N7");

            ////other sat and GPS info
            lblFixQuality.Text = mf.FixQuality(GPS);
            lblSatsTracked.Text = GPS.satellitesData.ToString();
            lblHDOP.Text = GPS.hdopData.ToString();
            lblSpeed.Text = GPS.speedData.ToString("N1");

            lblRoll.Text = GPS.rollData.ToString("N2");
            lblIMURoll.Text = GPS.imuRollData.ToString();
            lblIMUPitch.Text = GPS.imuPitchData.ToString();
            lblIMUYawRate.Text = GPS.imuYawRateData.ToString();
            lblIMUHeading.Text = GPS.imuHeadingData.ToString();

            lblAge.Text = GPS.ageData.ToString("N1");

            lblGPSHeading.Text = GPS.headingTrueData.ToString("N2");
            lblDualHeading.Text = GPS.headingTrueDualData.ToString("N2");

            lblAltitude.Text = GPS.altitudeData.ToString("N1");

            tboxVTG.Text = mf.vtgSentence;
            tboxGGA.Text = mf.ggaSentence;
            tboxPAOGI.Text = mf.paogiSentence;
            tboxAVR.Text = mf.avrSentence;
            tboxHDT.Text = mf.hdtSentence;
            //tboxRMC.Text = mf.rmcSentence;
            tboxHPD.Text = mf.hpdSentence;
            tboxPANDA.Text = mf.pandaSentence;
            tboxKSXT.Text = mf.ksxtSentence;
        }

        private void FormGPSData_Load(object sender, EventArgs e)
        {
            tboxGGA.Text = "";
            tboxVTG.Text = "";
            tboxHDT.Text = "";
            tboxAVR.Text = "";
            tboxPAOGI.Text = "";
            tboxHPD.Text = "";
            tboxPANDA.Text = "";
            tboxKSXT.Text = "";
        }

        private void FormGPSData_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isGPSSentencesOn = false;
        }
    }
}


//lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ac).ToString("N2"));
//lblAreaMinusActualApplied.Text = (((mf.fd.areaBoundaryOuterLessInner - mf.fd.actualAreaCovered) * glm.m2ac).ToString("N2"));
//lblOverlapPercent.Text = (fd.overlapPercent.ToString("N2")) + "%";
//lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ac).ToString("N3"));

//lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ha).ToString("N2"));
//lblAreaMinusActualApplied.Text = (((mf.fd.areaBoundaryOuterLessInner - mf.fd.actualAreaCovered) * glm.m2ha).ToString("N2"));
//lblOverlapPercent.Text = (fd.overlapPercent.ToString("N2")) + "%";
//lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ha).ToString("N3"));


//lblLookOnLeft.Text = mf.tool.lookAheadDistanceOnPixelsLeft.ToString("N0");
//lblLookOnRight.Text = mf.tool.lookAheadDistanceOnPixelsRight.ToString("N0");
//lblLookOffLeft.Text = mf.tool.lookAheadDistanceOffPixelsLeft.ToString("N0");
//lblLookOffRight.Text = mf.tool.lookAheadDistanceOffPixelsRight.ToString("N0");

//lblLeftToolSpd.Text = (mf.tool.toolFarLeftSpeed*3.6).ToString("N1");
//lblRightToolSpd.Text = (mf.tool.toolFarRightSpeed*3.6).ToString("N1");

//lblSectSpdLeft.Text = (mf.section[0].speedPixels*0.36).ToString("N1");
//lblSectSpdRight.Text = (mf.section[mf.tool.numOfSections-1].speedPixels*0.36).ToString("N1");