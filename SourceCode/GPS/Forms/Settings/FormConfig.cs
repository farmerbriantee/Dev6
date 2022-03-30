using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        bool isSaving = false;
        UserControl2 currentTab;

        //constructor
        public FormConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            HideSubMenu();
            ShowTab(new ConfigSummary(mf));
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaving)
            {
                if (currentTab != null)
                    currentTab.Close();

                //save current vehicle
                SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentTab != null)
            {
                UserControl2 Tab = (UserControl2)Activator.CreateInstance(currentTab.GetType(), mf);

                panel1.Controls.Add(Tab);
                Tab.BringToFront();
                Tab.Dock = DockStyle.Fill;
                panel1.Controls.Remove(currentTab);
                currentTab = Tab;
            }
        }

        private void HideSubMenu()
        {
            panelVehicleSubMenu.Visible = false;
            panelToolSubMenu.Visible = false;
            panelDataSourcesSubMenu.Visible = false;
            panelArduinoSubMenu.Visible = false;
        }

        private void ShowSubMenu(Panel subMenu, Button btn)
        {
            if (subMenu.Visible == false)
            {
                HideSubMenu();
                subMenu.Visible = true;
                if (subMenu.Name == "panelVehicleSubMenu") ShowTab(new ConfigVehicleType(mf));
                else if (subMenu.Name == "panelToolSubMenu") ShowTab(new ConfigToolType(mf));
                else if (subMenu.Name == "panelDataSourcesSubMenu") ShowTab(new ConfigHeading(mf));
                else if (subMenu.Name == "panelArduinoSubMenu") ShowTab(new ConfigSteer(mf));
                else if (btn.Name == "btnUTurn") ShowTab(new ConfigUTurn(mf));
                else if (btn.Name == "btnFeatureHides") ShowTab(new ConfigBtns(mf));
            }
            else
            {
                ShowTab(new ConfigSummary(mf));
                subMenu.Visible = false;
            }
        }

        public void ShowTab(UserControl2 Tab)
        {
            if (currentTab != null)
            {
                if (currentTab.GetType() == Tab.GetType())
                {
                    return;
                }
                panel1.Controls.Remove(currentTab);
                currentTab.Close();
            }
            panel1.Controls.Add(Tab);
            Tab.BringToFront();
            Tab.Dock = DockStyle.Fill;
            currentTab = Tab;
        }

        #region Top Menu Btns

        private void btnTool_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelToolSubMenu, btnTool);
        }

        private void btnDataSources_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelDataSourcesSubMenu, btnDataSources);
        }

        private void btnVehicle_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelVehicleSubMenu, btnVehicle);
        }

        private void btnTram_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ShowTab(new ConfigTram(mf));
        }

        private void btnUTurn_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ShowTab(new ConfigUTurn(mf));
        }

        private void btnFeatureHides_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ShowTab(new ConfigBtns(mf));
        }

        private void btnArduino_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelArduinoSubMenu, btnArduino);
        }

        #endregion

        private void btnSubVehicleType_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigVehicleType(mf));
        }

        private void btnSubDimensions_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigDimensions(mf));
        }

        private void btnSubAntenna_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigAntenna(mf));
        }

        private void btnSubGuidance_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigGuidance(mf));
        }

        private void btnSubBrand_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigBrand(mf));
        }

        private void btnSubToolType_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigToolType(mf));
        }

        private void btnSubHitch_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigHitch(mf));
        }

        private void btnSubSections_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigSections(mf));
        }

        private void btnSubSwitches_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigRemoteSwitch(mf));
        }

        private void btnSubToolSettings_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigToolSettings(mf));
        }

        private void btnSubRoll_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigRoll(mf));
        }

        private void btnSubHeading_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigHeading(mf));
        }

        private void btnSteerModule_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigSteer(mf));
        }

        private void btnMachineModule_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigMachine(mf));
        }

        private void btnMachineRelay_Click(object sender, EventArgs e)
        {
            ShowTab(new ConfigRelay(mf));
        }
    }
}