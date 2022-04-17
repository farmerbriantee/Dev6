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
            cboxFeatureTram.Checked = Properties.Settings.Default.setDisplayFeature_Tram;
            cboxFeatureHeadland.Checked = Properties.Settings.Default.setDisplayFeature_Headland;
            cboxFeatureBoundary.Checked = Properties.Settings.Default.setDisplayFeature_Boundary;
            cboxFeatureBoundaryContour.Checked = Properties.Settings.Default.setDisplayFeature_BoundaryContour;
            cboxFeatureRecPath.Checked = Properties.Settings.Default.setDisplayFeature_RecPath;
            cboxFeatureABSmooth.Checked = Properties.Settings.Default.setDisplayFeature_ABSmooth;
            cboxFeatureHideContour.Checked = Properties.Settings.Default.setDisplayFeature_HideContour;
            cboxFeatureWebcam.Checked = Properties.Settings.Default.setDisplayFeature_Webcam;
            cboxFeatureOffsetFix.Checked = Properties.Settings.Default.setDisplayFeature_OffsetFix;
            cboxFeatureContour.Checked = Properties.Settings.Default.setDisplayFeature_Contour;
            cboxFeatureYouTurn.Checked = Properties.Settings.Default.setDisplayFeature_YouTurn;
            cboxFeatureSteerMode.Checked = Properties.Settings.Default.setDisplayFeature_SteerMode;
            cboxFeatureAgIO.Checked = Properties.Settings.Default.setDisplayFeature_AgIO;
            cboxFeatureAutoSection.Checked = Properties.Settings.Default.setDisplayFeature_AutoSection;
            cboxFeatureManualSection.Checked = Properties.Settings.Default.setDisplayFeature_ManualSection;
            cboxFeatureCycleLines.Checked = Properties.Settings.Default.setDisplayFeature_CycleLines;
            cboxFeatureABLine.Checked = Properties.Settings.Default.setDisplayFeature_ABLine;
            cboxFeatureCurve.Checked = Properties.Settings.Default.setDisplayFeature_Curve;
            cboxFeatureAutoSteer.Checked = Properties.Settings.Default.setDisplayFeature_AutoSteer;
            cboxFeatureUTurn.Checked = Properties.Settings.Default.setDisplayFeature_UTurn;
            cboxFeatureLateral.Checked = Properties.Settings.Default.setDisplayFeature_Lateral;

            cboxTurnSound.Checked = Properties.Settings.Default.setSound_isUturnOn;
            cboxSteerSound.Checked = Properties.Settings.Default.setSound_isAutoSteerOn;
            cboxHydLiftSound.Checked = Properties.Settings.Default.setSound_isHydLiftOn;
        }

        public override void Close()
        {
            Properties.Settings.Default.setDisplayFeature_Tram = cboxFeatureTram.Checked;
            Properties.Settings.Default.setDisplayFeature_Headland = cboxFeatureHeadland.Checked;
            Properties.Settings.Default.setDisplayFeature_Boundary = cboxFeatureBoundary.Checked;
            Properties.Settings.Default.setDisplayFeature_BoundaryContour = cboxFeatureBoundaryContour.Checked;
            Properties.Settings.Default.setDisplayFeature_RecPath = cboxFeatureRecPath.Checked;
            Properties.Settings.Default.setDisplayFeature_ABSmooth = cboxFeatureABSmooth.Checked;
            Properties.Settings.Default.setDisplayFeature_HideContour = cboxFeatureHideContour.Checked;
            Properties.Settings.Default.setDisplayFeature_Webcam = cboxFeatureWebcam.Checked;
            Properties.Settings.Default.setDisplayFeature_OffsetFix = cboxFeatureOffsetFix.Checked;
            Properties.Settings.Default.setDisplayFeature_Contour = cboxFeatureContour.Checked;
            Properties.Settings.Default.setDisplayFeature_YouTurn = cboxFeatureYouTurn.Checked;
            Properties.Settings.Default.setDisplayFeature_SteerMode = cboxFeatureSteerMode.Checked;
            Properties.Settings.Default.setDisplayFeature_AgIO = cboxFeatureAgIO.Checked;
            Properties.Settings.Default.setDisplayFeature_AutoSection = cboxFeatureAutoSection.Checked;
            Properties.Settings.Default.setDisplayFeature_ManualSection = cboxFeatureManualSection.Checked;
            Properties.Settings.Default.setDisplayFeature_CycleLines = cboxFeatureCycleLines.Checked;
            Properties.Settings.Default.setDisplayFeature_ABLine = cboxFeatureABLine.Checked;
            Properties.Settings.Default.setDisplayFeature_Curve = cboxFeatureCurve.Checked;
            Properties.Settings.Default.setDisplayFeature_AutoSteer = cboxFeatureAutoSteer.Checked;
            Properties.Settings.Default.setDisplayFeature_UTurn = cboxFeatureUTurn.Checked;
            Properties.Settings.Default.setDisplayFeature_Lateral = cboxFeatureLateral.Checked;

            Properties.Settings.Default.setSound_isUturnOn = mf.sounds.isTurnSoundOn = cboxTurnSound.Checked;
            Properties.Settings.Default.setSound_isAutoSteerOn = mf.sounds.isSteerSoundOn = cboxSteerSound.Checked;
            Properties.Settings.Default.setSound_isHydLiftOn = mf.sounds.isHydLiftSoundOn = cboxHydLiftSound.Checked;

            Properties.Settings.Default.setDisplayFeature_SimpleCloseField = !(cboxFeatureTram.Checked | cboxFeatureHeadland.Checked | cboxFeatureBoundary.Checked | cboxFeatureBoundaryContour.Checked | cboxFeatureRecPath.Checked);

            Properties.Settings.Default.Save();

            mf.SetFeaturesOnOff();
        }
    }
}
