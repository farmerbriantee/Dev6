using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AgOpenGPS
{
    public static class SettingsIO
    {
        /// <summary>
        /// Import an XML and save to 1 section of user.config
        /// </summary>
        /// <param name="settingFile">Either Settings or Vehicle or Tools</param>
        /// <param name="settingsFilePath">Usually Documents.Drive.Folder</param>
        internal static void ImportSingle(string settingFile, string settingsFilePath)
        {
            if (!File.Exists(settingsFilePath))
            {
                throw new FileNotFoundException();
            }

            //var appSettings = Properties.Settings.Default;
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

                string sectionName = "";

                if (settingFile == "Vehicle")
                {
                    sectionName = Properties.Vehicle.Default.Context["GroupName"].ToString();
                }
                else if (settingFile == "Settings")
                {
                    sectionName = Properties.Settings.Default.Context["GroupName"].ToString();
                }
                //else if (settingFile == "Tool")
                //{
                //    sectionName = Properties.Tool.Default.Context["GroupName"].ToString();
                //}
                //else if (settingFile == "DataSource")
                //{
                //    sectionName = Properties.Tool.Default.Context["GroupName"].ToString();
                //}

                XDocument document = XDocument.Load(Path.Combine(settingsFilePath));
                string settingsSection = document.XPathSelectElements($"//{sectionName}").Single().ToString();
                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsSection);
                config.Save(ConfigurationSaveMode.Modified);

                if (settingFile == "Vehicle")
                {
                    Properties.Vehicle.Default.Reload();
                }
                else if (settingFile == "Settings")
                {
                    Properties.Settings.Default.Reload();
                }
            }
            catch (Exception) // Should make this more specific
            {
                // Could not import settings.
                if (settingFile == "Vehicle")
                {
                    Properties.Vehicle.Default.Reload();
                }
                else if (settingFile == "Settings")
                {
                    Properties.Settings.Default.Reload();
                }
            }
        }

        internal static void ExportSingle(string settingsFilePath)
        {
            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //Export the entire settings as an xml
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            config.SaveAs(settingsFilePath);
        }

        internal static void ExportAll(string settingsFilePath)
        {
            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //Export the entire settings as an xml
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            config.SaveAs(settingsFilePath);
        }

        internal static void ImportAll(string settingsFilePath)
        {
            if (!File.Exists(settingsFilePath))
            {
                return;
            }

            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                string sectionName = Properties.Settings.Default.Context["GroupName"].ToString();

                XDocument document = XDocument.Load(Path.Combine(settingsFilePath));
                string settingsA = document.XPathSelectElements($"//{sectionName}").Single().ToString();

                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsA);
                config.Save(ConfigurationSaveMode.Modified);

                //ConfigurationManager.RefreshSection(sectionName);
                Properties.Settings.Default.Reload();


                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                sectionName = Properties.Vehicle.Default.Context["GroupName"].ToString();

                document = XDocument.Load(Path.Combine(settingsFilePath));
                settingsA = document.XPathSelectElements($"//{sectionName}").Single().ToString();

                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsA);
                config.Save(ConfigurationSaveMode.Modified);

                Properties.Vehicle.Default.Reload();
            }

            catch (Exception) // Should make this more specific
            {
                // Could not import settings.
                Properties.Settings.Default.Reload();
                Properties.Vehicle.Default.Reload();
            }
        }
    }
}
