using GB_ServerManager.Models;
using System;
using System.Runtime.ConstrainedExecution;

namespace GB_ServerManager.Helpers
{
    internal static class AppSettingsHelper
    {
        internal static bool SaveSetting(EnvSettings settings)
        {
            try
            {
                Properties.Settings.Default.BaseSrvrPath = settings.ServerBasePath;
                Properties.Settings.Default.SteamCMDPath = settings.SteamCMDPath;
                Properties.Settings.Default.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static EnvSettings ReadSettings()
        {
            EnvSettings Settings = new EnvSettings();

            try
            {
                Settings.SteamCMDPath = Properties.Settings.Default.SteamCMDPath;
                Settings.ServerBasePath = Properties.Settings.Default.BaseSrvrPath;
            }
            catch (Exception)
            {

            }

            if (string.IsNullOrEmpty(Settings.ServerBasePath))
            {
                Settings.ServerBasePath = "";
            }

            if (string.IsNullOrEmpty(Settings.SteamCMDPath))
            {
                Settings.SteamCMDPath = "";
            }

            return Settings;            
        }

        internal static void SetDBMigration()
        {
            Properties.Settings.Default.DatabaseMigrate = true;
            Properties.Settings.Default.Save();
        }

        internal static bool ReadDBMigration()
        {
            return Properties.Settings.Default.DatabaseMigrate;
        }

        internal static bool NewVersionSettingsMigration()
        {
            var ver = typeof(MainWindow).Assembly.GetName().Version;                
            
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.SettingsVersion))
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.SettingsVersion = string.Format("v{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);
                Properties.Settings.Default.Save();
            }
            else
            {
                if (Properties.Settings.Default.SettingsVersion != string.Format("v{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build))
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.SettingsVersion = string.Format("v{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);
                    Properties.Settings.Default.Save();
                }
            }
            return false;
        }
    }
}
