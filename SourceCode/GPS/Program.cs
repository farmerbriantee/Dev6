using AgOpenGPS.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{516-0AC5-B9A1-55fd-A8CE-72F04E6BDE8F}");

        [STAThread]
        private static void Main()
        {
            ////opening the subkey
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

            ////create default keys if not existing
            if (regKey == null)
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                //storing the values
                Key.SetValue("Language", "en");
                Key.Close();

                Settings.Default.setF_culture = "en";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.setF_culture = regKey.GetValue("Language").ToString();
                Settings.Default.Save();
                regKey.Close();
            }

            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                /*
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    String resourceName = "AgOpenGPS.References." + new AssemblyName(args.Name).Name + ".dll";
                    String assemblyName = Assembly.GetExecutingAssembly().FullName;
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            Byte[] assemblyData = new Byte[stream.Length];
                            stream.Read(assemblyData, 0, assemblyData.Length);
                            return Assembly.Load(assemblyData);
                        }
                    }
                    return null;
                };
                */

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Properties.Settings.Default.setF_culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.setF_culture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormGPS());
            }
            else
            {
                MessageBox.Show("AgOpenGPS is Already Running");
            }

        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();
    }
}