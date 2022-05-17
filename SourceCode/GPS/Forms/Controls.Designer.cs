//Please, if you use this, share the improvements

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using Microsoft.Win32;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        public bool isTT;

        #region Right Menu
        private void btnContour_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnContour, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            Form f = Application.OpenForms["FormABLine"];
            if (f != null)
            {
                f.Close();
            }

            SetGuidanceMode(gyd.CurrentGMode == Mode.Contour ? Mode.None : Mode.Contour);
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnCurve, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            Form f = Application.OpenForms["FormABLine"];
            if (f != null)
            {
                f.Close();
            }

            if (gyd.CurrentGMode != Mode.Curve)
            {
                SetGuidanceMode(Mode.Curve);

                gyd.currentGuidanceLine = gyd.currentCurveLine;
                if (gyd.currentGuidanceLine != null)
                    return;
            }

            Form form = new FormABLine(this, false);
            form.Show(this);
        }

        private void btnABLine_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnABLine, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            Form f = Application.OpenForms["FormABLine"];
            if (f != null)
            {
                f.Close();
            }

            if (gyd.CurrentGMode != Mode.AB)
            {
                SetGuidanceMode(Mode.AB);

                gyd.currentGuidanceLine = gyd.currentABLine;
                if (gyd.currentGuidanceLine != null)
                    return;
            }

            var form = new FormABLine(this, true);
            form.Show(this);
        }

        public void SetGuidanceMode(Mode newmode)
        {
            gyd.isValid = false;
            gyd.moveDistance = 0;
            gyd.currentGuidanceLine = null;
            gyd.curList = new Polyline();

            panelDrag.Visible = newmode == Mode.RecPath;
            if (gyd.CurrentGMode == Mode.RecPath)
                gyd.StopDrivingRecordedPath();

            setBtnAutoSteer(false);

            btnContour.Image = newmode == Mode.Contour ? Properties.Resources.ContourOn : Properties.Resources.ContourOff;
            btnCycleLines.Image = newmode == Mode.Contour ? Properties.Resources.ColorLocked : Properties.Resources.ABLineCycle;
            btnCycleLines.Enabled = (newmode == Mode.Curve || newmode == Mode.AB || newmode == Mode.Contour);

            btnCurve.Image = newmode == Mode.Curve ? Properties.Resources.CurveOn : Properties.Resources.CurveOff;
            btnABLine.Image = newmode == Mode.AB ? Properties.Resources.ABLineOn : Properties.Resources.ABLineOff;

            btnAutoSteer.Enabled = newmode != Mode.None;
            
            if (newmode == Mode.Curve || newmode == Mode.AB)
            {
                if (!btnEditAB.Visible)
                {
                    btnEditAB.Visible = true;
                    btnSnapToPivot.Visible = true;
                    cboxpRowWidth.Visible = true;
                    btnYouSkipEnable.Visible = true;
                    btnAutoYouTurn.Enabled = bnd.bndList.Count > 0;
                }
            }
            else if (btnEditAB.Visible)
            {
                btnEditAB.Visible = false;
                btnSnapToPivot.Visible = false;
                cboxpRowWidth.Visible = false;
                btnYouSkipEnable.Visible = false;
                btnAutoYouTurn.Enabled = false;
            }

            gyd.CurrentGMode = newmode;
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                if (gyd.CurrentGMode == Mode.Contour)
                    new FormHelp(gStr.h_btnLockToContour, gStr.gsHelp).ShowDialog(this);
                else
                    new FormHelp(gStr.h_btnCycleLines, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
            }
            else if (gyd.CurrentGMode == Mode.Contour)
            {
                if (gyd.curList.points.Count > 5) gyd.isLocked = !gyd.isLocked;
            }
            else if (gyd.currentGuidanceLine != null)
            {
                gyd.isValid = false;
                gyd.moveDistance = 0;
                gyd.ResetYouTurn();

                bool found = !(gyd.currentGuidanceLine.mode.HasFlag(gyd.CurrentGMode));
                bool loop = true;
                for (int i = 0; i < gyd.curveArr.Count || loop; i++)
                {
                    if (i >= gyd.curveArr.Count)
                    {
                        loop = false;
                        i = -1;
                        if (!found) break;
                        else continue;
                    }
                    if (gyd.currentGuidanceLine.Name == gyd.curveArr[i].Name)
                        found = true;
                    else if (found && gyd.curveArr[i].mode.HasFlag(gyd.CurrentGMode))
                    {
                        gyd.currentGuidanceLine = new CGuidanceLine(gyd.curveArr[i]);

                        if (gyd.currentGuidanceLine.mode.HasFlag(Mode.AB))
                            gyd.currentABLine = gyd.currentGuidanceLine;
                        else
                            gyd.currentCurveLine = gyd.currentGuidanceLine;

                        lblCurveLineName.Text = (gyd.currentGuidanceLine.mode.HasFlag(Mode.AB) ? "AB-" : "Cur-") + gyd.currentGuidanceLine.Name.Trim();
                        break;
                    }
                }
                if (!found)
                {
                    gyd.currentGuidanceLine = null;
                    lblCurveLineName.Text = string.Empty;
                }
            }
        }

        //Section Manual and Auto
        private void btnManualOffOn_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnManualOffOn, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            
            setSectionBtnState(autoBtnState == btnStates.On ? btnStates.Off : btnStates.On);
        }

        private void btnSectionOffAutoOn_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnSectionOffAutoOn, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            setSectionBtnState(autoBtnState == btnStates.Auto ? btnStates.Off : btnStates.Auto);
        }

        public void setSectionBtnState(btnStates status)
        {
            if (!isJobStarted) status = btnStates.Off;

            if (autoBtnState != status)
            {
                if (status == btnStates.On || (autoBtnState == btnStates.On && status == btnStates.Off))
                    System.Media.SystemSounds.Asterisk.Play();
                else if (status == btnStates.Auto || (autoBtnState == btnStates.Auto && status == btnStates.Off))
                    System.Media.SystemSounds.Exclamation.Play();

                autoBtnState = status;
                btnManualOffOn.Image = status == btnStates.On ? Properties.Resources.ManualOn : Properties.Resources.ManualOff;
                btnSectionOffAutoOn.Image = status == btnStates.Auto ? Properties.Resources.SectionMasterOn : Properties.Resources.SectionMasterOff;

                //set sections buttons to status
                for (int j = 0; j < tool.sections.Count; j++)
                    tool.sections[j].UpdateButton(status);
            }
        }

        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnAutoSteer, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            //new direction so reset where to put turn diagnostic
            gyd.ResetYouTurn();
            setBtnAutoSteer(!isAutoSteerBtnOn);
        }

        public void setBtnAutoSteer(bool status)
        {
            if (gyd.CurrentGMode == Mode.None) status = false;
            if (status != isAutoSteerBtnOn)
            {
                isAutoSteerBtnOn = status;
                btnAutoSteer.Image = status ? Properties.Resources.AutoSteerOn : Properties.Resources.AutoSteerOff;

                if (!isAutoSteerBtnOn && gyd.isYouTurnBtnOn)
                    btnAutoYouTurn.PerformClick();

                if (sounds.isSteerSoundOn)
                {
                    if (status)
                        sounds.sndAutoSteerOn.Play();
                    else
                        sounds.sndAutoSteerOff.Play();
                }
            }
        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnAutoYouTurn, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            gyd.ResetYouTurn();

            p_239.pgn[p_239.uturn] = 0;

            if (!gyd.isYouTurnBtnOn)
            {
                if (!isAutoSteerBtnOn)
                    setBtnAutoSteer(true);

                gyd.isYouTurnBtnOn = true;
                btnAutoYouTurn.Image = Properties.Resources.Youturn80;
            }
            else
            {
                gyd.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
                gyd.Set_Alternate_skips();

                gyd.isYouTurnBtnOn = false;
                btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
            }
        }

        #endregion

        #region Left Panel Menu
        private void toolStripDropDownButtonDistance_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnDistanceArea, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            fd.distanceUser = 0;
            fd.workedAreaTotalUser = 0;
        }        
        private void navPanelToolStrip_Click(object sender, EventArgs e)
        {
            //buttonPanelCounter = 0;

            if (panelNavigation.Visible)
            {
                panelNavigation.Visible = false;
            }
            else
            {
                panelNavigation.Visible = true;
                navPanelCounter = 2;
            }
        }
        private void toolStripMenuItemFlagRed_Click(object sender, EventArgs e)
        {
            flagColor = 0;
            btnFlag.Image = Properties.Resources.FlagRed;
        }
        private void toolStripMenuGrn_Click(object sender, EventArgs e)
        {
            flagColor = 1;
            btnFlag.Image = Properties.Resources.FlagGrn;
        }
        private void toolStripMenuYel_Click(object sender, EventArgs e)
        {
            flagColor = 2;
            btnFlag.Image = Properties.Resources.FlagYel;
        }
        private void toolStripMenuFlagForm_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = 1;
                Form form = new FormFlags(this);
                form.Show(this);
            }            
        }

        private void btnFlag_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnFlag, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            int nextflag = flagPts.Count + 1;
            CFlag flagPt = new CFlag(mc.latitude, mc.longitude, pivotAxlePos.easting, pivotAxlePos.northing, fixHeading, flagColor, (nextflag).ToString());
            flagPts.Add(flagPt);
            FileSaveFlags();

            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = nextflag;
                Form form = new FormFlags(this);
                form.Show(this);
            }
        }

        private void btnStartAgIO_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnStartAgIO, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            Process[] processName = Process.GetProcessesByName("AgIO");
            if (processName.Length == 0)
            {
                //Start application here
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                string strPath = di.ToString();
                strPath += "\\AgIO.exe";
                try
                {
                    //TimedMessageBox(2000, "Please Wait", "Starting AgIO");
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    //processInfo.ErrorDialog = true;
                    //processInfo.UseShellExecute = false;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgIO");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }
        private void btnAutoSteerConfig_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnAutoSteerConfig, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
                return;
            }

            //
            Form form = new FormSteer(this);
            form.Show(this);

        }

        private void stripBtnConfig_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnConfig, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            using (FormConfig form = new FormConfig(this))
            {
                form.ShowDialog(this);
            }
        }

        private void btnStanleyPure_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnStanleyPure, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            isStanleyUsed = !isStanleyUsed;

            btnStanleyPure.Image = isStanleyUsed ? Resources.ModeStanley : Resources.ModePurePursuit;
            
            Properties.Vehicle.Default.setVehicle_isStanleyUsed = isStanleyUsed;
            Properties.Vehicle.Default.Save();
        }
        #endregion

        #region Top Panel
        private void lblSpeed_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_lblSpeed, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                return;
            }

            Form form = new FormGPSData(this);
            form.Show(this);

        }
        private void btnShutdown_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnMinimizeMainForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnMaximizeMainForm_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                Padding = new System.Windows.Forms.Padding(5,5,5,5);
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        #endregion

        #region File Menu

        //File drop down items
        private void setWorkingDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Currently: " + new DirectoryInfo(baseDirectory).Parent.FullName;

            fbd.SelectedPath = new DirectoryInfo(baseDirectory).Parent.FullName;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                if (fbd.SelectedPath != Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                {
                    Key.SetValue("WorkDir", fbd.SelectedPath);
                }
                else
                {
                    Key.DeleteValue("WorkDir", false);
                }
                Key.Close();
                //restart program
                new FormHelp(gStr.gsProgramWillExitPleaseRestart, "").ShowDialog(this);
                Close();
            }
        }

        private void enterSimCoordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSimCoords(this))
            {
                form.ShowDialog(this);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Form_About())
            {
                form.ShowDialog(this);
            }
        }

        private void resetALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                MessageBox.Show(gStr.gsCloseFieldFirst);
            }
            else
            {
                if (new FormHelp(gStr.gsReallyResetEverything, gStr.gsResetAll, true).ShowDialog(this) == DialogResult.Yes)
                {
                    //deleting "WorkDir" subkey
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    Key.DeleteValue("WorkDir", false);
                    Key.Close();

                    Properties.Settings.Default.Reset();
                    Properties.Vehicle.Default.Reset();

                    LoadSettings();
                }
            }
        }
        private void topFieldViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.setMenu_isOGLZoomOn == 1)
            {
                Settings.Default.setMenu_isOGLZoomOn = 0;
                Settings.Default.Save();
                topFieldViewToolStripMenuItem.Checked = false;
                oglZoom.Width = 400;
                oglZoom.Height = 400;
                oglZoom.SendToBack();
            }
            else
            {
                Settings.Default.setMenu_isOGLZoomOn = 1;
                Settings.Default.Save();
                topFieldViewToolStripMenuItem.Checked = true;
                oglZoom.Width = 300;
                oglZoom.Height = 300;
                oglZoom.Left = 80;
                oglZoom.Top = 80;
                if (isJobStarted) oglZoom.BringToFront();
            }
        }

        private void helpMenuItem_Click(object sender, EventArgs e)
        {

             using (var form = new Form_Help(this))
            {
                form.ShowDialog(this);
            }
        }

        private void simulatorOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!panelSim.Visible && isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                simulatorOnToolStripMenuItem.Checked = panelSim.Visible;
            }
            else if (!panelSim.Visible && sentenceCounter < 299)
            {
                TimedMessageBox(2000, "Conected", "GPS");
                simulatorOnToolStripMenuItem.Checked = panelSim.Visible;
            }
            else
            {
                simulatorOnToolStripMenuItem.Checked = panelSim.Visible = timerSim.Enabled = !panelSim.Visible;
                isFirstFixPositionSet = false;
                isGPSPositionInitialized = false;
                isFirstHeadingSet = false;
                startCounter = 0;
            }
            if (Properties.Settings.Default.setMenu_isSimulatorOn != simulatorOnToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void colorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormColor(this))
            {
                form.ShowDialog(this);
            }
            SettingsIO.ExportAll(vehiclesDirectory + vehicleFileName + ".XML");
        }

        private void colorsSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSectionColor(this))
            {
                form.ShowDialog(this);
            }
            SettingsIO.ExportAll(vehiclesDirectory + vehicleFileName + ".XML");
        }

        //Languages
        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            SetLanguage("en");
        }
        private void menuLanguageDanish_Click(object sender, EventArgs e)
        {
            SetLanguage("da");
        }
        private void menuLanguageTurkish_Click(object sender, EventArgs e)
        {
            SetLanguage("tr");
        }
        private void menuLanguageDeutsch_Click(object sender, EventArgs e)
        {
            SetLanguage("de");
        }
        private void menuLanguageRussian_Click(object sender, EventArgs e)
        {
            SetLanguage("ru");
        }
        private void menuLanguageDutch_Click(object sender, EventArgs e)
        {
            SetLanguage("nl");
        }
        private void menuLanguageSpanish_Click(object sender, EventArgs e)
        {
            SetLanguage("es");
        }
        private void menuLanguageFrench_Click(object sender, EventArgs e)
        {
            SetLanguage("fr");
        }
        private void menuLanguageItalian_Click(object sender, EventArgs e)
        {
            SetLanguage("it");
        }
        private void menuLanguageUkranian_Click(object sender, EventArgs e)
        {
            SetLanguage("uk");
        }
        private void menuLanguageSlovak_Click(object sender, EventArgs e)
        {
            SetLanguage("sk");
        }
        private void menuLanguagesPolski_Click(object sender, EventArgs e)
        {
            SetLanguage("pl");
        }
        private void menuLanguageTest_Click(object sender, EventArgs e)
        {
            SetLanguage("af");
        }

        private void SetLanguage(string lang)
        {
            //reset them all to false
            menuLanguageEnglish.Checked = false;
            menuLanguageDeutsch.Checked = false;
            menuLanguageRussian.Checked = false;
            menuLanguageDutch.Checked = false;
            menuLanguageSpanish.Checked = false;
            menuLanguageFrench.Checked = false;
            menuLanguageItalian.Checked = false;
            menuLanguageUkranian.Checked = false;
            menuLanguageSlovak.Checked = false;
            menuLanguagePolish.Checked = false;
            menuLanguageDanish.Checked = false;
            menuLanguageTurkish.Checked = false;

            menuLanguageTest.Checked = false;

            switch (lang)
            {
                case "en":
                    menuLanguageEnglish.Checked = true;
                    break;

                case "ru":
                    menuLanguageRussian.Checked = true;
                    break;

                case "da":
                    menuLanguageDanish.Checked = true;
                    break;

                case "de":
                    menuLanguageDeutsch.Checked = true;
                    break;

                case "nl":
                    menuLanguageDutch.Checked = true;
                    break;

                case "it":
                    menuLanguageItalian.Checked = true;
                    break;

                case "es":
                    menuLanguageSpanish.Checked = true;
                    break;

                case "fr":
                    menuLanguageFrench.Checked = true;
                    break;

                case "uk":
                    menuLanguageUkranian.Checked = true;
                    break;

                case "sk":
                    menuLanguageSlovak.Checked = true;
                    break;

                case "pl":
                    menuLanguagePolish.Checked = true;
                    break;

                case "tr":
                    menuLanguageTurkish.Checked = true;
                    break;

                case "af":
                    menuLanguageTest.Checked = true;
                    break;

                default:
                    menuLanguageEnglish.Checked = true;
                    lang = "en";
                    break;
            }

            if (Properties.Settings.Default.setF_culture != lang)
            {
                Properties.Settings.Default.setF_culture = lang;
                Properties.Settings.Default.Save();
            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);

            SetGuiText();
        }

        #endregion

        #region Bottom Menu

        private void btnEditAB_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnEditAB, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            Form fc = Application.OpenForms["FormEditAB"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (gyd.currentGuidanceLine != null)
            {
                Form form = new FormEditAB(this, gyd.currentGuidanceLine);
                form.Show(this);
            }
            else
            {
                var form = new FormTimedMessage(1500, gStr.gsNoABLineActive, gStr.gsPleaseEnterABLine);
                return;
            }
        }

        public void CloseTopMosts()
        {
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

            fc = Application.OpenForms["FormSteerGraph"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

            fc = Application.OpenForms["FormGPSData"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
            }

        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            using (var form = new FormConfig(this))
            {
                form.ShowDialog(this);
            }
        }

        private void btnTramDisplayMode_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnTramDisplayMode, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            tram.displayMode++;
            if (tram.displayMode > 3) tram.displayMode = 0;

            FixTramModeButton();
        }

        private void btnChangeMappingColor_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnChangeMappingColor, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            using (var form = new FormColorPicker(this, sectionColor))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    sectionColor = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorSectionsDay = sectionColor;
            Settings.Default.Save();

            btnChangeMappingColor.BackColor = sectionColor;

        }

        //Snaps
        private void btnSnapToPivot_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnSnapToPivot, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            if (gyd.currentGuidanceLine != null)
            {
                gyd.MoveGuidanceLine(gyd.currentGuidanceLine, gyd.distanceFromCurrentLinePivot);
            }
            else
            {
                var form = new FormTimedMessage(2000, (gStr.gsNoGuidanceLines), (gStr.gsTurnOnContourOrMakeABLine));
                form.Show(this);
            }
        }

        private void btnABDraw_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnABDraw, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            using (var form = new FormABDraw(this))
            {
                form.ShowDialog(this);
            }
        }

        private void btnYouSkipEnable_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnYouSkipEnable, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            
            gyd.alternateSkips = !gyd.alternateSkips;
            if (gyd.alternateSkips)
            {
                btnYouSkipEnable.Image = Resources.YouSkipOn;
                //make sure at least 1
                if (gyd.rowSkipsWidth < 2)
                {
                    gyd.rowSkipsWidth = 2;
                    cboxpRowWidth.Text = "1";
                }
                gyd.Set_Alternate_skips();
                gyd.ResetCreatedYouTurn();
                if (!gyd.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
            }
            else
            {
                btnYouSkipEnable.Image = Resources.YouSkipOff;
            }
        }

        private void cboxpRowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gyd.rowSkipsWidth = cboxpRowWidth.SelectedIndex + 1;
            gyd.Set_Alternate_skips();
            gyd.ResetCreatedYouTurn();
            Properties.Vehicle.Default.set_youSkipWidth = gyd.rowSkipsWidth;
            Properties.Vehicle.Default.Save();
        }

        private void btnResetToolHeading_Click(object sender, EventArgs e)
        {
            tankPos.heading = fixHeading;
            tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (tool.toolTankTrailingHitchLength));
            tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (tool.toolTankTrailingHitchLength));

            tool.Pos.heading = tankPos.heading;
            tool.Pos.easting = tankPos.easting + (Math.Sin(tool.Pos.heading) * (tool.toolTrailingHitchLength));
            tool.Pos.northing = tankPos.northing + (Math.Cos(tool.Pos.heading) * (tool.toolTrailingHitchLength));
        }

        private void btnHeadlandOnOff_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnHeadlandOnOff, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = !bnd.isHeadlandOn;
                if (bnd.isHeadlandOn)
                {
                    btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                    btnHydLift.Visible = true;
                }
                else
                {
                    btnHydLift.Visible = false;
                    btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                }
            }
            else bnd.isHeadlandOn = false;

            if (!bnd.isHeadlandOn)
            {
                p_239.pgn[p_239.hydLift] = 0;
                vehicle.isHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                btnHydLift.Visible = false;
            }

        }

        private void btnHydLift_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnHydLift, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            if (bnd.isHeadlandOn)
            {
                vehicle.isHydLiftOn = !vehicle.isHydLiftOn;
                if (vehicle.isHydLiftOn)
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOn;
                }
                else
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                    p_239.pgn[p_239.hydLift] = 0;
                }
            }
            else
            {
                p_239.pgn[p_239.hydLift] = 0;
                vehicle.isHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                btnHydLift.Visible = false;
            }
        }


        #endregion

        #region Tools Menu

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(btnSender.Width, 0);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }

        private void deleteContourPathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileCreateContour();
            gyd.ResetContour();
            contourSaveList.Clear();
        }

        private void toolStripAreYouSure_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                if (autoBtnState == btnStates.Off)
                {
                    if (new FormHelp(gStr.gsDeleteAllContoursAndSections, gStr.gsDeleteForSure, true).ShowDialog(this) == DialogResult.Yes)
                    {
                        //clear out the contour Lists
                        gyd.ResetContour();
                        fd.workedAreaTotal = 0;

                        for (int j = 0; j < patchList.Count; j++)
                        {
                            patchList[j].RemoveHandle();
                        }
                        patchList.Clear();
                        patchSaveList.Clear();

                        FileCreateContour();
                        FileCreateSections();
                    }
                }
                else
                {
                   TimedMessageBox(1500, "Sections are on", "Turn Auto or Manual Off First");
                }
            }
        }
        private void headingChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcg = Application.OpenForms["FormHeadingGraph"];

            if (fcg != null)
            {
                fcg.Focus();
                return;
            }

            //
            Form formG = new FormHeadingGraph(this);
            formG.Show(this);
        }
        private void toolStripAutoSteerChart_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcg = Application.OpenForms["FormSteerGraph"];

            if (fcg != null)
            {
                fcg.Focus();
                return;
            }

            //
            Form formG = new FormSteerGraph(this);
            formG.Show(this);
        }
        private void webcamToolStrip_Click(object sender, EventArgs e)
        {
            Form form = new FormWebCam();
            form.Show(this);
        }
        private void offsetFixToolStrip_Click(object sender, EventArgs e)
        {
            using (var form = new FormShiftPos(this))
            {
                form.ShowDialog(this);
            }
        }

        private void toolSteerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FormToolSteer"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //
            Form form = new FormToolSteer(this);
            form.Show(this);
        }

        private void correctionToolStrip_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcc = Application.OpenForms["FormCorrection"];

            if (fcc != null)
            {
                fcc.Focus();
                return;
            }

            //
            Form formC = new FormCorrection(this);
            formC.Show(this);
        }


        #endregion

        #region Nav Panel
        private void btn2D_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btn2D, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

            worldManager.camFollowing = true;
            worldManager.camPitch = 0;
            navPanelCounter = 2;
        }

        private void btn3D_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btn3D, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            worldManager.camFollowing = true;
            worldManager.camPitch = -60;
            navPanelCounter = 2;
        }

        private void btnN2D_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnN2D, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            worldManager.camFollowing = false;
            worldManager.camPitch = 0;
            navPanelCounter = 2;
        }

        private void btnN3D_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnN3D, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            worldManager.camPitch = -60;
            worldManager.camFollowing = false;
            navPanelCounter = 2;
        }

        private void btnDayNightMode_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnDayNightMode, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
            SwapDayNightMode();
            navPanelCounter = 2;
        }

        //The zoom tilt buttons
        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            SetZoom(-2.5);
            navPanelCounter = 2;
        }
        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            SetZoom(2.5);
            navPanelCounter = 2;
        }
        private void btnpTiltUp_MouseDown(object sender, MouseEventArgs e)
        {
            worldManager.camPitch -= ((worldManager.camPitch * 0.012) - 1);
            if (worldManager.camPitch > -40) worldManager.camPitch = 0;
            navPanelCounter = 2;
        }
        private void btnpTiltDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (worldManager.camPitch > -40) worldManager.camPitch = -40;
            worldManager.camPitch += ((worldManager.camPitch * 0.012) - 1);
            if (worldManager.camPitch < -80) worldManager.camPitch = -80;
            navPanelCounter = 2;
        }

        #endregion

        #region Field Menu
        private void toolStripBtnFieldOpen_Click(object sender, EventArgs e)
        {
            //bring up dialog if no job active, close job if one is
            if (!isJobStarted)
            {
                using (var form = new FormJob(this))
                {
                    if (!isFirstFixPositionSet || sentenceCounter > 299 || mc.latitude == 0 || mc.longitude == 0)
                    {
                        TimedMessageBox(2500, "No GPS", "You are lost with no GPS, Fix that First");
                        return;
                    }

                    var result = form.ShowDialog(this);
                    if (result == DialogResult.Yes)
                    {
                        //ask for a directory name
                        using (var form2 = new FormFieldKML(this, false))
                        { form2.ShowDialog(this); }
                    }

                    //load from  KML
                    else if (result == DialogResult.No)
                    {
                        //ask for a directory name
                        using (var form2 = new FormFieldKML(this, true))
                        { form2.ShowDialog(this); }
                    }
                }
            }
            else if (Properties.Settings.Default.setDisplayFeature_SimpleCloseField)
            {
                CloseCurrentJob(false);
            }
            else
            {
                Button btnSender = (Button)sender;
                Point ptLowerLeft = new Point(btnSender.Width, 0);
                ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
                contextMenuStrip2.Show(ptLowerLeft);
            }
        }
        private void toolStripBtnField_Click(object sender, EventArgs e)
        {
            CloseCurrentJob(false);
        }

        private bool CloseCurrentJob(bool closing)
        {
            //close the current job and ask how to or if to save
            if (isJobStarted)
            {
                if (!panelSim.Visible && autoBtnState != btnStates.Off)
                {
                    TimedMessageBox(2000, "Safe Shutdown", "Turn off Auto Section Control");
                    return true;
                }

                CloseTopMosts();

                int choice = 0;
                using (FormSaveOrNot form = new FormSaveOrNot(closing))
                {
                    DialogResult result = form.ShowDialog(this);

                    if (result == DialogResult.Ignore) choice = 0;   //Ignore
                    else if (result == DialogResult.OK) choice = 1;      //Save and Exit
                    else if (result == DialogResult.Yes) choice = 2;      //Save As
                    else choice = 0;  // oops something is really busted
                }

                if (choice > 0)
                {
                    Settings.Default.setF_CurrentDir = currentFieldDirectory;
                    Settings.Default.Save();

                    //turn off contour line if on
                    gyd.StopContourLine();

                    //FileSaveHeadland();
                    //FileSaveBoundary();
                    FileSaveSections();
                    FileSaveContour();
                    FileSaveFieldKML();

                    JobClose();
                    Text = "AgOpenGPS";

                    if (choice > 1)
                    {
                        //ask for a directory name
                        using (var form2 = new FormSaveAs(this))
                        {
                            form2.ShowDialog(this);
                        }
                    }
                    else
                        displayFieldName = gStr.gsNone;
                }
                else return true;
            }
            return false;
        }
        private void toolStripBtnMakeBndContour_Click(object sender, EventArgs e)
        {
            //build all the contour guidance lines from boundaries, all of them.
            using (var form = new FormMakeBndCon(this))
            {
                form.ShowDialog(this);
            }
        }
        private void tramLinesMenuField_Click(object sender, EventArgs e)
        {
            if ((gyd.CurrentGMode == Mode.AB || gyd.CurrentGMode == Mode.Curve) && gyd.currentGuidanceLine != null)
            {
                Form form97 = new FormTram(this);
                form97.Show(this);
                form97.Left = Width - 275;
                form97.Top = 100;
            }
            else
            {
                var form = new FormTimedMessage(1500, gStr.gsNoABLineActive, gStr.gsPleaseEnterABLine);
                form.Show(this);
                return;
            }
        }
        private void headlandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bnd.bndList.Count == 0)
            {
                TimedMessageBox(2000, gStr.gsNoBoundary, gStr.gsCreateABoundaryFirst);
                return;
            }

            GetHeadland();
        }
        public void GetHeadland()
        {
            using (var form = new FormHeadland(this))
            {
                form.ShowDialog(this);
            }

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
                btnHydLift.Visible = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                btnHydLift.Visible = false;
            }
        }

        private void boundariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                var form = new FormBoundary(this);
                form.Show(this);
            }
            else { TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsStartNewField); }
        }

        //Recorded Path
        private void btnPathGoStop_Click(object sender, EventArgs e)
        {
            //already running?
            if (gyd.isDrivingRecordedPath)
            {
                gyd.StopDrivingRecordedPath();
                btnPathGoStop.Image = Properties.Resources.boundaryPlay;
                btnPathRecordStop.Enabled = true;
                btnPickPath.Enabled = true;
                btnResumePath.Enabled = true;
                return;
            }

            //start the recorded path driving process
            if (!gyd.StartDrivingRecordedPath())
            {
                //Cancel the recPath - something went seriously wrong
                gyd.StopDrivingRecordedPath();
                TimedMessageBox(1500, gStr.gsProblemMakingPath, gStr.gsCouldntGenerateValidPath);
                btnPathGoStop.Image = Properties.Resources.boundaryPlay;
                btnPathRecordStop.Enabled = true;
                btnPickPath.Enabled = true;
                btnResumePath.Enabled = true;
                return;
            }
            else
            {
                btnPathGoStop.Image = Properties.Resources.boundaryStop;
                btnPathRecordStop.Enabled = false;
                btnPickPath.Enabled = false;
                btnResumePath.Enabled = false;
            }
        }

        private void btnPathRecordStop_Click(object sender, EventArgs e)
        {
            if (gyd.isRecordOn)
            {
                gyd.isRecordOn = false;
                btnPathRecordStop.Image = Properties.Resources.BoundaryRecord;
                btnPathGoStop.Enabled = true;
                btnPickPath.Enabled = true;
                btnResumePath.Enabled = true;

                using (var form = new FormRecordName(this))
                {
                    form.ShowDialog(this);
                    if(form.DialogResult == DialogResult.OK) 
                    {
                        String filename = form.filename + ".rec";
                        FileSaveRecPath();
                        FileSaveRecPath(filename);
                    }
                }                
            }
            else if (isJobStarted)
            {
                gyd.recList.Clear();
                gyd.isRecordOn = true;
                btnPathRecordStop.Image = Properties.Resources.boundaryStop;
                btnPathGoStop.Enabled = false;
                btnPickPath.Enabled = false;
                btnResumePath.Enabled = false;
            }
        }

        private void btnResumePath_Click(object sender, EventArgs e)
        {
            if (gyd.resumeState == 0)
            {
                gyd.resumeState++;
                btnResumePath.Image = Properties.Resources.pathResumeLast;
                TimedMessageBox(1500, "Resume Style", "Last Stopped Position");
            }

            else if (gyd.resumeState == 1)
            {
                gyd.resumeState++;
                btnResumePath.Image = Properties.Resources.pathResumeClose;
                TimedMessageBox(1500, "Resume Style", "Closest Point");
            }
            else
            {
                gyd.resumeState = 0;
                btnResumePath.Image = Properties.Resources.pathResumeStart;
                TimedMessageBox(1500, "Resume Style", "Start At Beginning");
            }
        }

        private void btnPickPath_Click(object sender, EventArgs e)
        {
            gyd.resumeState = 0;
            btnResumePath.Image = Properties.Resources.pathResumeStart;
            gyd.currentPositonIndex = 0;

            using (FormFilePicker form = new FormFilePicker(this, 2, ""))
            {
                form.ShowDialog(this);
            }
        }

        private void recordedPathStripMenu_Click(object sender, EventArgs e)
        {
            SetGuidanceMode(gyd.CurrentGMode == Mode.RecPath ? Mode.None : Mode.RecPath);
        }

        #endregion

        #region Sim controls
        private void timerSim_Tick(object sender, EventArgs e)
        {
            if ((gyd.isDrivingRecordedPath || isAutoSteerBtnOn) && guidanceLineDistanceOff != 32000)
                sim.DoSimTick(guidanceLineSteerAngle * 0.01);
            else sim.DoSimTick(sim.steerAngleScrollBar);
        }

        private void hsbarSteerAngle_Scroll(object sender, ScrollEventArgs e)
        {
            sim.steerAngleScrollBar = (hsbarSteerAngle.Value - 400) * 0.1;
            btnResetSteerAngle.Text = sim.steerAngleScrollBar.ToString("0.0");
        }
        private void hsbarStepDistance_Scroll(object sender, ScrollEventArgs e)
        {
            sim.stepDistance = ((double)(hsbarStepDistance.Value)) / 3.6;
        }
        private void btnResetSteerAngle_Click(object sender, EventArgs e)
        {
            sim.steerAngleScrollBar = 0;
            hsbarSteerAngle.Value = 400;
            btnResetSteerAngle.Text = sim.steerAngleScrollBar.ToString("0.0");
        }
        private void btnResetSim_Click(object sender, EventArgs e)
        {
            sim.resetSim();
        }
        private void btnSimSetSpeedToZero_Click(object sender, EventArgs e)
        {
            sim.stepDistance = 0;
            hsbarStepDistance.Value = 0;
        }
        #endregion


        private void lbludpWatchCounts_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_lbludpWatchCounts, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
        }

        private void lblInty_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_lblIntegral, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }
        }

        private void cboxpRowWidth_Click(object sender, EventArgs e)
        {
            if (isTT)
            {
                new FormHelp(gStr.h_btnRowWidthSkips, gStr.gsHelp).ShowDialog(this);
                ResetHelpBtn();
                return;
            }

        }

        public void FixTramModeButton()
        {
            if (tram.tramList.Count > 0 || tram.tramBoundary.Count > 0)
                btnTramDisplayMode.Visible = true;
            else btnTramDisplayMode.Visible = false;

            switch (tram.displayMode)
            {
                case 0:
                    btnTramDisplayMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnTramDisplayMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnTramDisplayMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnTramDisplayMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            isTT = !isTT;
            if (isTT)
                btnHelp.Image = Resources.HelpCancel;
            else
            {
                btnHelp.Image = Resources.Help;
                isTT = false;
            }
        }

        private void ResetHelpBtn()
        {
            isTT = false;
            btnHelp.Image = Resources.Help;
        }
    }//end class
}//end namespace