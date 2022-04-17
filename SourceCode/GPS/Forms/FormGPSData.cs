//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGPSData : Form
    {
        private readonly FormGPS mf = null;

        public FormGPSData(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            lblSunrise.Text = mf.sunrise.ToString("HH:mm");
            lblSunset.Text = mf.sunset.ToString("HH:mm");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTram.Text = mf.tram.controlByte.ToString();

            lblHz.Text = mf.fixUpdateHz + " ~ " + mf.frameTime.ToString("0.0");

            lblEastingField.Text = mf.pivotAxlePos.easting.ToString("0.00");
            lblNorthingField.Text = mf.pivotAxlePos.northing.ToString("0.00");

            lblLatitude.Text = mf.mc.latitude.ToString("0.0000000");
            lblLongitude.Text = mf.mc.longitude.ToString("0.0000000");

            //other sat and GPS info
            lblFixQuality.Text = mf.FixQuality;
            lblSatsTracked.Text = mf.SatsTracked;
            lblHDOP.Text = mf.HDOP;
            lblSpeed.Text = mf.mc.avgSpeed.ToString("0.0");

            //lblUturnByte.Text = Convert.ToString(mf.mc.machineData[mf.mc.mdUTurn], 2).PadLeft(6, '0');

            lblRoll.Text = mf.RollInDegrees;
            lblYawHeading.Text = mf.GyroInDegrees;
            lblGPSHeading.Text = mf.GPSHeading;
            lblFixHeading.Text = (mf.fixHeading * 57.2957795).ToString("0.0");

            lblAltitude.Text = (mf.mc.altitude * mf.mToUserBig).ToString("0.0");
        }
    }
}


//lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ac).ToString("0.00"));
//lblAreaMinusActualApplied.Text = (((mf.fd.areaBoundaryOuterLessInner - mf.fd.actualAreaCovered) * glm.m2ac).ToString("0.00"));
//lblOverlapPercent.Text = (fd.overlapPercent.ToString("0.00")) + "%";
//lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ac).ToString("0.000"));

//lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ha).ToString("0.00"));
//lblAreaMinusActualApplied.Text = (((mf.fd.areaBoundaryOuterLessInner - mf.fd.actualAreaCovered) * glm.m2ha).ToString("0.00"));
//lblOverlapPercent.Text = (fd.overlapPercent.ToString("0.00")) + "%";
//lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ha).ToString("0.000"));


//lblLookOnLeft.Text = mf.tool.lookAheadDistanceOnPixelsLeft.ToString("0.0");
//lblLookOnRight.Text = mf.tool.lookAheadDistanceOnPixelsRight.ToString("0.0");
//lblLookOffLeft.Text = mf.tool.lookAheadDistanceOffPixelsLeft.ToString("0.0");
//lblLookOffRight.Text = mf.tool.lookAheadDistanceOffPixelsRight.ToString("0.0");

//lblLeftToolSpd.Text = (mf.tool.toolFarLeftSpeed*3.6).ToString("0.0");
//lblRightToolSpd.Text = (mf.tool.toolFarRightSpeed*3.6).ToString("0.0");

//lblSectSpdLeft.Text = (mf.section[0].speedPixels*0.36).ToString("0.0");
//lblSectSpdRight.Text = (mf.section[mf.tool.numOfSections-1].speedPixels*0.36).ToString("0.0");