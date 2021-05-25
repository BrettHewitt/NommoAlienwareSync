using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using ViewLibrary.Model.Effects;

namespace ACLightingControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string targetDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folder = targetDirectory.Replace(Path.GetFileName(targetDirectory), "");
            Directory.SetCurrentDirectory(folder);

            SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
            Application.ApplicationExit += ApplicationOnApplicationExit;
            bool createdNew = false;
            string mutexName = System.Reflection.Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName, out createdNew))
            {
                if (!createdNew)
                {
                    // Only allow one instance
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    STAApplicationContext context = new STAApplicationContext();
                    Application.Run(context);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error");
                }
            }
        }

        private static void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            EffectManager.Instance.StopEffect();
            EffectManager.Instance.Uninitialise();
            Application.ApplicationExit -= ApplicationOnApplicationExit;
            SystemEvents.SessionEnding -= SystemEventsOnSessionEnding;
        }

        private static void ApplicationOnApplicationExit(object sender, EventArgs e)
        {
            EffectManager.Instance.StopEffect();
            EffectManager.Instance.Uninitialise();
            Application.ApplicationExit -= ApplicationOnApplicationExit;
            SystemEvents.SessionEnding -= SystemEventsOnSessionEnding;
        }
    }
}
