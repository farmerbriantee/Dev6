using System;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigRelay : UserControl2
    {
        private readonly FormGPS mf;

        public ConfigRelay(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void cboxPin0_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;
        }

        private void btnSendRelayConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettingsRelay();
            SendRelaySettingsToMachineModule();

            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);

            pboxSendRelay.Visible = false;
        }

        private void SaveSettingsRelay()
        {
            StringBuilder bob = new StringBuilder();

            bob.Append(cboxPin0.SelectedIndex.ToString() + ",")
               .Append(cboxPin1.SelectedIndex.ToString() + ",")
               .Append(cboxPin2.SelectedIndex.ToString() + ",")
               .Append(cboxPin3.SelectedIndex.ToString() + ",")
               .Append(cboxPin4.SelectedIndex.ToString() + ",")
               .Append(cboxPin5.SelectedIndex.ToString() + ",")
               .Append(cboxPin6.SelectedIndex.ToString() + ",")
               .Append(cboxPin7.SelectedIndex.ToString() + ",")
               .Append(cboxPin8.SelectedIndex.ToString() + ",")
               .Append(cboxPin9.SelectedIndex.ToString() + ",")
               .Append(cboxPin10.SelectedIndex.ToString() + ",")
               .Append(cboxPin11.SelectedIndex.ToString() + ",")
               .Append(cboxPin12.SelectedIndex.ToString() + ",")
               .Append(cboxPin13.SelectedIndex.ToString() + ",")
               .Append(cboxPin14.SelectedIndex.ToString() + ",")
               .Append(cboxPin15.SelectedIndex.ToString() + ",")
               .Append(cboxPin16.SelectedIndex.ToString() + ",")
               .Append(cboxPin17.SelectedIndex.ToString() + ",")
               .Append(cboxPin18.SelectedIndex.ToString() + ",")
               .Append(cboxPin19.SelectedIndex.ToString() + ",")
               .Append(cboxPin20.SelectedIndex.ToString() + ",")
               .Append(cboxPin21.SelectedIndex.ToString() + ",")
               .Append(cboxPin22.SelectedIndex.ToString() + ",")
               .Append(cboxPin23.SelectedIndex.ToString());

            Properties.Settings.Default.setRelay_pinConfig = bob.ToString();

            //save settings
            Properties.Settings.Default.Save();
            pboxSendRelay.Visible = false;

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

        private void btnRelaySetDefaultConfig_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;

            cboxPin0.SelectedIndex = 1;
            cboxPin1.SelectedIndex = 2;
            cboxPin2.SelectedIndex = 3;
            cboxPin3.SelectedIndex = 0;
            cboxPin4.SelectedIndex = 0;
            cboxPin5.SelectedIndex = 0;
            cboxPin6.SelectedIndex = 0;
            cboxPin7.SelectedIndex = 0;
            cboxPin8.SelectedIndex = 0;
            cboxPin9.SelectedIndex = 0;
            cboxPin10.SelectedIndex = 0;
            cboxPin11.SelectedIndex = 0;
            cboxPin12.SelectedIndex = 0;
            cboxPin13.SelectedIndex = 0;
            cboxPin14.SelectedIndex = 0;
            cboxPin15.SelectedIndex = 0;
            cboxPin16.SelectedIndex = 0;
            cboxPin17.SelectedIndex = 0;
            cboxPin18.SelectedIndex = 0;
            cboxPin19.SelectedIndex = 0;
            cboxPin20.SelectedIndex = 0;
            cboxPin21.SelectedIndex = 0;
            cboxPin22.SelectedIndex = 0;
            cboxPin23.SelectedIndex = 0;
        }

        private void btnRelayResetConfigToNone_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;

            cboxPin0.SelectedIndex = 0;
            cboxPin1.SelectedIndex = 0;
            cboxPin2.SelectedIndex = 0;
            cboxPin3.SelectedIndex = 0;
            cboxPin4.SelectedIndex = 0;
            cboxPin5.SelectedIndex = 0;
            cboxPin6.SelectedIndex = 0;
            cboxPin7.SelectedIndex = 0;
            cboxPin8.SelectedIndex = 0;
            cboxPin9.SelectedIndex = 0;
            cboxPin10.SelectedIndex = 0;
            cboxPin11.SelectedIndex = 0;
            cboxPin12.SelectedIndex = 0;
            cboxPin13.SelectedIndex = 0;
            cboxPin14.SelectedIndex = 0;
            cboxPin15.SelectedIndex = 0;
            cboxPin16.SelectedIndex = 0;
            cboxPin17.SelectedIndex = 0;
            cboxPin18.SelectedIndex = 0;
            cboxPin19.SelectedIndex = 0;
            cboxPin20.SelectedIndex = 0;
            cboxPin21.SelectedIndex = 0;
            cboxPin22.SelectedIndex = 0;
            cboxPin23.SelectedIndex = 0;
        }

        private void ConfigRelay_Load(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = false;

            string[] wordsList = { "-","Section 1","Section 2","Section 3","Section 4","Section 5","Section 6","Section 7",
                    "Section 8","Section 9","Section 10","Section 11","Section 12","Section 13","Section 14","Section 15",
                    "Section 16","Hyd Up","Hyd Down","Tram Right","Tram Left", "Geo Stop"};

            //19 tram right and 20 tram left

            cboxPin0.Items.Clear(); cboxPin0.Items.AddRange(wordsList);
            cboxPin1.Items.Clear(); cboxPin1.Items.AddRange(wordsList);
            cboxPin2.Items.Clear(); cboxPin2.Items.AddRange(wordsList);
            cboxPin3.Items.Clear(); cboxPin3.Items.AddRange(wordsList);
            cboxPin4.Items.Clear(); cboxPin4.Items.AddRange(wordsList);
            cboxPin5.Items.Clear(); cboxPin5.Items.AddRange(wordsList);
            cboxPin6.Items.Clear(); cboxPin6.Items.AddRange(wordsList);
            cboxPin7.Items.Clear(); cboxPin7.Items.AddRange(wordsList);
            cboxPin8.Items.Clear(); cboxPin8.Items.AddRange(wordsList);
            cboxPin9.Items.Clear(); cboxPin9.Items.AddRange(wordsList);

            cboxPin10.Items.Clear(); cboxPin10.Items.AddRange(wordsList);
            cboxPin11.Items.Clear(); cboxPin11.Items.AddRange(wordsList);
            cboxPin12.Items.Clear(); cboxPin12.Items.AddRange(wordsList);
            cboxPin13.Items.Clear(); cboxPin13.Items.AddRange(wordsList);
            cboxPin14.Items.Clear(); cboxPin14.Items.AddRange(wordsList);
            cboxPin15.Items.Clear(); cboxPin15.Items.AddRange(wordsList);
            cboxPin16.Items.Clear(); cboxPin16.Items.AddRange(wordsList);
            cboxPin17.Items.Clear(); cboxPin17.Items.AddRange(wordsList);
            cboxPin18.Items.Clear(); cboxPin18.Items.AddRange(wordsList);
            cboxPin19.Items.Clear(); cboxPin19.Items.AddRange(wordsList);

            cboxPin20.Items.Clear(); cboxPin20.Items.AddRange(wordsList);
            cboxPin21.Items.Clear(); cboxPin21.Items.AddRange(wordsList);
            cboxPin22.Items.Clear(); cboxPin22.Items.AddRange(wordsList);
            cboxPin23.Items.Clear(); cboxPin23.Items.AddRange(wordsList);

            string[] words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

            cboxPin0.SelectedIndex = int.Parse(words[0]);
            cboxPin1.SelectedIndex = int.Parse(words[1]);
            cboxPin2.SelectedIndex = int.Parse(words[2]);
            cboxPin3.SelectedIndex = int.Parse(words[3]);
            cboxPin4.SelectedIndex = int.Parse(words[4]);
            cboxPin5.SelectedIndex = int.Parse(words[5]);
            cboxPin6.SelectedIndex = int.Parse(words[6]);
            cboxPin7.SelectedIndex = int.Parse(words[7]);
            cboxPin8.SelectedIndex = int.Parse(words[8]);
            cboxPin9.SelectedIndex = int.Parse(words[9]);
            cboxPin10.SelectedIndex = int.Parse(words[10]);
            cboxPin11.SelectedIndex = int.Parse(words[11]);
            cboxPin12.SelectedIndex = int.Parse(words[12]);
            cboxPin13.SelectedIndex = int.Parse(words[13]);
            cboxPin14.SelectedIndex = int.Parse(words[14]);
            cboxPin15.SelectedIndex = int.Parse(words[15]);
            cboxPin16.SelectedIndex = int.Parse(words[16]);
            cboxPin17.SelectedIndex = int.Parse(words[17]);
            cboxPin18.SelectedIndex = int.Parse(words[18]);
            cboxPin19.SelectedIndex = int.Parse(words[19]);
            cboxPin20.SelectedIndex = int.Parse(words[20]);
            cboxPin21.SelectedIndex = int.Parse(words[21]);
            cboxPin22.SelectedIndex = int.Parse(words[22]);
            cboxPin23.SelectedIndex = int.Parse(words[23]);
        }
    }
}
