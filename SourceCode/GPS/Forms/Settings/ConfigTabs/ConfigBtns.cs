using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigBtns : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigBtns(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigBtns_Load(object sender, EventArgs e)
        {
            cboxFeatureTram.Checked = Properties.Settings.Default.setFeatures.isTramOn;
            cboxFeatureHeadland.Checked = Properties.Settings.Default.setFeatures.isHeadlandOn;
            cboxFeatureBoundary.Checked = Properties.Settings.Default.setFeatures.isBoundaryOn;
            cboxFeatureBoundaryContour.Checked = Properties.Settings.Default.setFeatures.isBndContourOn;
            cboxFeatureRecPath.Checked = Properties.Settings.Default.setFeatures.isRecPathOn;
            cboxFeatureABSmooth.Checked = Properties.Settings.Default.setFeatures.isABSmoothOn;
            cboxFeatureHideContour.Checked = Properties.Settings.Default.setFeatures.isHideContourOn;
            cboxFeatureWebcam.Checked = Properties.Settings.Default.setFeatures.isWebCamOn;
            cboxFeatureOffsetFix.Checked = Properties.Settings.Default.setFeatures.isOffsetFixOn;
            cboxFeatureContour.Checked = Properties.Settings.Default.setFeatures.isContourOn;
            cboxFeatureYouTurn.Checked = Properties.Settings.Default.setFeatures.isYouTurnOn;
            cboxFeatureSteerMode.Checked = Properties.Settings.Default.setFeatures.isSteerModeOn;
            cboxFeatureAgIO.Checked = Properties.Settings.Default.setFeatures.isAgIOOn;
            cboxFeatureAutoSection.Checked = Properties.Settings.Default.setFeatures.isAutoSectionOn;
            cboxFeatureManualSection.Checked = Properties.Settings.Default.setFeatures.isManualSectionOn;
            cboxFeatureCycleLines.Checked = Properties.Settings.Default.setFeatures.isCycleLinesOn;
            cboxFeatureABLine.Checked = Properties.Settings.Default.setFeatures.isABLineOn;
            cboxFeatureCurve.Checked = Properties.Settings.Default.setFeatures.isCurveOn;
            cboxFeatureAutoSteer.Checked = Properties.Settings.Default.setFeatures.isAutoSteerOn;
            cboxFeatureUTurn.Checked = Properties.Settings.Default.setFeatures.isUTurnOn;
            cboxFeatureLateral.Checked = Properties.Settings.Default.setFeatures.isLateralOn;

            cboxTurnSound.Checked = Properties.Settings.Default.setSound_isUturnOn;
            cboxSteerSound.Checked = Properties.Settings.Default.setSound_isAutoSteerOn;
            cboxHydLiftSound.Checked = Properties.Settings.Default.setSound_isHydLiftOn;
        }

        public override void Close()
        {
            Properties.Settings.Default.setFeatures.isTramOn = cboxFeatureTram.Checked;
            Properties.Settings.Default.setFeatures.isHeadlandOn = cboxFeatureHeadland.Checked;
            Properties.Settings.Default.setFeatures.isBoundaryOn = cboxFeatureBoundary.Checked;
            Properties.Settings.Default.setFeatures.isBndContourOn = cboxFeatureBoundaryContour.Checked;
            Properties.Settings.Default.setFeatures.isRecPathOn = cboxFeatureRecPath.Checked;
            Properties.Settings.Default.setFeatures.isABSmoothOn = cboxFeatureABSmooth.Checked;
            Properties.Settings.Default.setFeatures.isHideContourOn = cboxFeatureHideContour.Checked;
            Properties.Settings.Default.setFeatures.isWebCamOn = cboxFeatureWebcam.Checked;
            Properties.Settings.Default.setFeatures.isOffsetFixOn = cboxFeatureOffsetFix.Checked;
            Properties.Settings.Default.setFeatures.isContourOn = cboxFeatureContour.Checked;
            Properties.Settings.Default.setFeatures.isYouTurnOn = cboxFeatureYouTurn.Checked;
            Properties.Settings.Default.setFeatures.isSteerModeOn = cboxFeatureSteerMode.Checked;
            Properties.Settings.Default.setFeatures.isAgIOOn = cboxFeatureAgIO.Checked;
            Properties.Settings.Default.setFeatures.isAutoSectionOn = cboxFeatureAutoSection.Checked;
            Properties.Settings.Default.setFeatures.isManualSectionOn = cboxFeatureManualSection.Checked;
            Properties.Settings.Default.setFeatures.isCycleLinesOn = cboxFeatureCycleLines.Checked;
            Properties.Settings.Default.setFeatures.isABLineOn = cboxFeatureABLine.Checked;
            Properties.Settings.Default.setFeatures.isCurveOn = cboxFeatureCurve.Checked;
            Properties.Settings.Default.setFeatures.isAutoSteerOn = cboxFeatureAutoSteer.Checked;
            Properties.Settings.Default.setFeatures.isLateralOn = cboxFeatureLateral.Checked;
            Properties.Settings.Default.setFeatures.isUTurnOn = cboxFeatureUTurn.Checked;
            mf.SetFeaturesOnOff();

            Properties.Settings.Default.setSound_isUturnOn = mf.sounds.isTurnSoundOn = cboxTurnSound.Checked;
            Properties.Settings.Default.setSound_isAutoSteerOn = mf.sounds.isSteerSoundOn = cboxSteerSound.Checked;
            Properties.Settings.Default.setSound_isHydLiftOn = mf.sounds.isHydLiftSoundOn = cboxHydLiftSound.Checked;

            Properties.Settings.Default.Save();
        }
    }
}
