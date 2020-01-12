using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings.Effects;
using ViewLibrary.Model.Settings.OpSettings;

namespace ViewLibrary.Model.Settings
{
    public static class SettingsManager
    {
        private const string DirectoryName = "AlienChromaSync";
        private const string FileName = "AlienChromaSettings.acs";

        private static string FilePath
        { 
            get;
            set;
        }
        static SettingsManager()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderLocation = Path.Combine(appDataPath, DirectoryName);

            if (!Directory.Exists(folderLocation))
            {
                Directory.CreateDirectory(folderLocation);
            }

            string filePath = Path.Combine(folderLocation, FileName);
            
            if (!File.Exists(filePath))
            {
                GlobalSettings settings = new GlobalSettings();
                settings.DeviceSettings = new DeviceSettings();
                settings.SpectrumSettings = new SpectrumSettings();
                settings.ScreenCaptureSettings = new ScreenCaptureSettings();
                settings.CustomSettings = new CustomSettings();
                ProgramSettings programSettings = new ProgramSettings();
                programSettings.GeneralSettings = new GeneralSettings();
                programSettings.LightingSettings = new LightingSettings();
                settings.ProgramSettings = programSettings;

                string json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(filePath, json);
            }

            FilePath = filePath;
        }

        public static GlobalSettings GetSettings()
        {
            try
            {
                string text = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<GlobalSettings>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool SaveSettings(GlobalSettings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(FilePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
