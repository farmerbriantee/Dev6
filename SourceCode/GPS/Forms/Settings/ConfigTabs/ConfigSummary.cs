using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigSummary : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigSummary(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigSum_Load(object sender, EventArgs e)
        {
            chkDisplaySky.Checked = mf.isSkyOn;
            chkDisplayFloor.Checked = mf.isTextureOn;
            chkDisplayGrid.Checked = mf.worldManager.isGridOn;
            chkDisplaySpeedo.Checked = mf.isSpeedoOn;
            chkDisplayDayNight.Checked = mf.isAutoDayNight;
            chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
            chkDisplayLogNMEA.Checked = mf.isLogNMEA;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayLightbar.Checked = mf.isLightbarOn;
            chkDisplayKeyboard.Checked = glm.isKeyboardOn;

            if (glm.isMetric) rbtnDisplayMetric.Checked = true;
            else rbtnDisplayImperial.Checked = true;

            UpdateVehicleListView();
            btnVehicleDelete.Enabled = btnVehicleLoad.Enabled = btnVehicleSaveAs.Enabled = lvVehicles.SelectedItems.Count > 0;
        }

        public override void Close()
        {
            Properties.Settings.Default.setMenu_isSkyOn = mf.isSkyOn = chkDisplaySky.Checked;
            Properties.Settings.Default.setDisplay_isTextureOn = mf.isTextureOn = chkDisplayFloor.Checked;
            Properties.Settings.Default.setMenu_isGridOn = mf.worldManager.isGridOn = chkDisplayGrid.Checked;
            Properties.Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn = chkDisplaySpeedo.Checked;
            Properties.Settings.Default.setDisplay_isAutoDayNight = mf.isAutoDayNight = chkDisplayDayNight.Checked;
            Properties.Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines = chkDisplayExtraGuides.Checked;
            mf.isLogNMEA = chkDisplayLogNMEA.Checked;
            mf.isDrawPolygons = chkDisplayPolygons.Checked;
            Properties.Settings.Default.setMenu_isLightbarOn = mf.isLightbarOn = chkDisplayLightbar.Checked;
            Properties.Settings.Default.setDisplay_isKeyboardOn = glm.isKeyboardOn = chkDisplayKeyboard.Checked;
            Properties.Settings.Default.setDisplay_isStartFullScreen = chkDisplayStartFullScreen.Checked;
            Properties.Settings.Default.setMenu_isMetric = glm.isMetric = rbtnDisplayMetric.Checked;

            glm.SetUserScales(glm.isMetric);

            Properties.Settings.Default.Save();
        }

        private void rbtnDisplayMetric_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Metric");
        }

        private void rbtnDisplayImperial_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Imperial");
        }

        private void tboxVehicleNameSave_Click(object sender, EventArgs e)
        {
            tboxVehicleNameSave.KeyboardToText();
        }

        private void btnVehicleLoad_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (lvVehicles.SelectedItems.Count > 0)
                {
                    DialogResult result3 = new FormHelp("Load: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML", gStr.gsSaveAndReturn, true).ShowDialog(this);
                    if (result3 == DialogResult.Yes)
                    {
                        SettingsIO.ImportAll(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                        mf.vehicleFileName = lvVehicles.SelectedItems[0].SubItems[0].Text;
                        Properties.Vehicle.Default.setVehicle_vehicleName = mf.vehicleFileName;
                        Properties.Vehicle.Default.Save();

                        //reset AOG
                        mf.LoadSettings();

                        chkDisplaySky.Checked = mf.isSkyOn;
                        chkDisplayFloor.Checked = mf.isTextureOn;
                        chkDisplayGrid.Checked = mf.worldManager.isGridOn;
                        chkDisplaySpeedo.Checked = mf.isSpeedoOn;
                        chkDisplayDayNight.Checked = mf.isAutoDayNight;
                        chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
                        chkDisplayLogNMEA.Checked = mf.isLogNMEA;
                        chkDisplayPolygons.Checked = mf.isDrawPolygons;
                        chkDisplayLightbar.Checked = mf.isLightbarOn;
                        chkDisplayKeyboard.Checked = glm.isKeyboardOn;
                        chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;

                        if (glm.isMetric) rbtnDisplayMetric.Checked = true;
                        else rbtnDisplayImperial.Checked = true;



                        //Form Steer Settings
                        mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
                        mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

                        mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
                        mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

                        mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
                        mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
                        mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
                        mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

                        mf.SendPgnToLoop(mf.p_252.pgn);

                        //machine module settings
                        mf.p_238.pgn[mf.p_238.set0] = Properties.Vehicle.Default.setArdMac_setting0;
                        mf.p_238.pgn[mf.p_238.raiseTime] = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
                        mf.p_238.pgn[mf.p_238.lowerTime] = Properties.Vehicle.Default.setArdMac_hydLowerTime;

                        mf.SendPgnToLoop(mf.p_238.pgn);

                        //steer config
                        mf.p_251.pgn[mf.p_251.set0] = Properties.Vehicle.Default.setArdSteer_setting0;
                        mf.p_251.pgn[mf.p_251.set1] = Properties.Vehicle.Default.setArdSteer_setting1;
                        mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Vehicle.Default.setArdSteer_maxPulseCounts;
                        mf.p_251.pgn[mf.p_251.minSpeed] = 5; //0.5 kmh

                        mf.SendPgnToLoop(mf.p_251.pgn);

                        //Send Pin configuration
                        SendRelaySettingsToMachineModule();

                        ///Remind the user
                        mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");
                    }

                    UpdateVehicleListView();

                }
            }
            else
            {
                this.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
            }
            //Close();
        }

        private void SendRelaySettingsToMachineModule()
        {
            string[] words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

            //load the pgn
            mf.p_236.pgn[mf.p_236.pin0] = (byte)int.Parse(words[0]);
            mf.p_236.pgn[mf.p_236.pin1] = (byte)int.Parse(words[1]);
            mf.p_236.pgn[mf.p_236.pin2] = (byte)int.Parse(words[2]);
            mf.p_236.pgn[mf.p_236.pin3] = (byte)int.Parse(words[3]);
            mf.p_236.pgn[mf.p_236.pin4] = (byte)int.Parse(words[4]);
            mf.p_236.pgn[mf.p_236.pin5] = (byte)int.Parse(words[5]);
            mf.p_236.pgn[mf.p_236.pin6] = (byte)int.Parse(words[6]);
            mf.p_236.pgn[mf.p_236.pin7] = (byte)int.Parse(words[7]);
            mf.p_236.pgn[mf.p_236.pin8] = (byte)int.Parse(words[8]);
            mf.p_236.pgn[mf.p_236.pin9] = (byte)int.Parse(words[9]);

            mf.p_236.pgn[mf.p_236.pin10] = (byte)int.Parse(words[10]);
            mf.p_236.pgn[mf.p_236.pin11] = (byte)int.Parse(words[11]);
            mf.p_236.pgn[mf.p_236.pin12] = (byte)int.Parse(words[12]);
            mf.p_236.pgn[mf.p_236.pin13] = (byte)int.Parse(words[13]);
            mf.p_236.pgn[mf.p_236.pin14] = (byte)int.Parse(words[14]);
            mf.p_236.pgn[mf.p_236.pin15] = (byte)int.Parse(words[15]);
            mf.p_236.pgn[mf.p_236.pin16] = (byte)int.Parse(words[16]);
            mf.p_236.pgn[mf.p_236.pin17] = (byte)int.Parse(words[17]);
            mf.p_236.pgn[mf.p_236.pin18] = (byte)int.Parse(words[18]);
            mf.p_236.pgn[mf.p_236.pin19] = (byte)int.Parse(words[19]);

            mf.p_236.pgn[mf.p_236.pin20] = (byte)int.Parse(words[20]);
            mf.p_236.pgn[mf.p_236.pin21] = (byte)int.Parse(words[21]);
            mf.p_236.pgn[mf.p_236.pin22] = (byte)int.Parse(words[22]);
            mf.p_236.pgn[mf.p_236.pin23] = (byte)int.Parse(words[23]);
            mf.SendPgnToLoop(mf.p_236.pgn);
        }

        private void btnVehicleSaveAs_Click(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                DialogResult result3 = new FormHelp("Overwrite: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML", gStr.gsSaveAndReturn, true).ShowDialog(this);
                if (result3 == DialogResult.Yes)
                {
                    SettingsIO.ExportAll(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                }
                UpdateVehicleListView();
            }
        }

        private void btnVehicleDelete_Click(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                DialogResult result3 = new FormHelp("Delete: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML", gStr.gsSaveAndReturn, true).ShowDialog(this);
                if (result3 == DialogResult.Yes)
                {
                    File.Delete(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                }
                UpdateVehicleListView();
            }
        }

        private void btnVehicleSave_Click(object sender, EventArgs e)
        {
            if (tboxVehicleNameSave.Text.Trim().Length > 0)
            {
                SettingsIO.ExportAll(mf.vehiclesDirectory + tboxVehicleNameSave.Text.Trim() + ".XML");
                UpdateVehicleListView();
                Properties.Vehicle.Default.setVehicle_vehicleName = tboxVehicleNameSave.Text.Trim();
                Properties.Vehicle.Default.Save();
                tboxVehicleNameSave.Text = "";
                btnVehicleSave.Enabled = false;
            }
        }

        private void tboxVehicleNameSave_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            btnVehicleSave.Enabled = !string.IsNullOrEmpty(tboxVehicleNameSave.Text.Trim());
        }

        private void UpdateVehicleListView()
        {
            DirectoryInfo dinfo = new DirectoryInfo(mf.vehiclesDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.XML");

            //load the listbox
            lvVehicles.Items.Clear();
            foreach (FileInfo file in Files)
            {
                lvVehicles.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            //deselect everything
            lvVehicles.SelectedItems.Clear();
        }

        private void chkDisplaySky_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplaySky, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayDayNight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayDayNight, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayLightbar_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayLightbar, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayPolygons_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayPolygons, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayGrid_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayGrid, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayStartFullScreen_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayStartFullScreen, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayKeyboard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayKeyboard, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayLogNMEA_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayLogNMEA, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplaySpeedo_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplaySpeedo, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayExtraGuides_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayExtraGuides, gStr.gsHelp).ShowDialog(this);
        }

        private void chkDisplayFloor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_chkDisplayFloor, gStr.gsHelp).ShowDialog(this);
        }

        private void rbtnDisplayMetric_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_rbtnDisplayMetric, gStr.gsHelp).ShowDialog(this);
        }

        private void rbtnDisplayImperial_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_rbtnDisplayImperial, gStr.gsHelp).ShowDialog(this);
        }

        private void lvVehicles_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_ListViewVehicles, gStr.gsHelp).ShowDialog(this);
        }

        private void btnVehicleLoad_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnVehicleLoad, gStr.gsHelp).ShowDialog(this);
        }

        private void btnVehicleSaveAs_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnVehicleSaveAs, gStr.gsHelp).ShowDialog(this);
        }

        private void btnVehicleDelete_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnVehicleDelete, gStr.gsHelp).ShowDialog(this);
        }

        private void tboxVehicleNameSave_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_tboxVehicleNameSave, gStr.gsHelp).ShowDialog(this);
        }

        private void btnVehicleSave_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            new FormHelp(gStr.hc_btnVehicleSave, gStr.gsHelp).ShowDialog(this);
        }

        private void lvVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnVehicleDelete.Enabled = btnVehicleLoad.Enabled = btnVehicleSaveAs.Enabled = lvVehicles.SelectedItems.Count > 0;
        }
    }
}
