using GB_ServerManager.Models;
using System;

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
    }
}
