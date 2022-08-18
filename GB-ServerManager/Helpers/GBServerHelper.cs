using System.IO;
using System.Text.RegularExpressions;
using GB_ServerManager.Models;

namespace GB_ServerManager.Helpers
{
    internal static class GBServerHelper
    {
        internal static ServerSetting RetrieveGBServerProperties(string BasePath, string ServerExePath)
        {
            string ServerConfigFile = "";
            string ServerIniPath = "";
            ServerSetting NewServer = new ServerSetting();

            try
            {
                var IniFiles = Directory.GetFiles(BasePath, "*.ini", SearchOption.AllDirectories);

                foreach (var File in IniFiles)
                {
                    if (File.Contains("Server.ini"))
                    {
                        ServerIniPath = File;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(ServerIniPath))
                {
                    string _ReadFile = File.ReadAllText(ServerIniPath);

                    if (!string.IsNullOrEmpty(_ReadFile))
                    {
                        ServerConfigFile = _ReadFile;
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            if (!string.IsNullOrEmpty(ServerConfigFile))
            {
                var configFile = ServerConfigFile.Split(System.Environment.NewLine);
                foreach (var item in configFile)
                {
                    if (item.Contains("ServerName="))
                    {
                        NewServer.ServerName = item.Substring(item.IndexOf('=') + 1).Trim();
                    }
                    if (item.Contains("ServerMOTD="))
                    {
                        NewServer.ServerMOTD = item.Substring(item.IndexOf('=') + 1).Trim();
                    }
                    if (item.Contains("MaxPlayers="))
                    {
                        string value = item.Substring(item.IndexOf('=') + 1);
                        NewServer.MaxPlayers = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
                    }
                    if (item.Contains("MaxSpectators="))
                    {
                        string value = item.Substring(item.IndexOf('=') + 1);
                        NewServer.MaxSpectators = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
                    }
                    if (item.Contains("GameRules="))
                    {
                        NewServer.GameRules = item.Substring(item.IndexOf('=') + 1).Trim();
                    }
                    if (item.Contains("ServerPassword="))
                    {
                        NewServer.ServerPassword = item.Substring(item.IndexOf('=') + 1).Trim();
                    }
                }
            } else
            {
                NewServer.ServerName = "New Server";
                NewServer.ServerMOTD = "";
                NewServer.MaxPlayers = 16;
                NewServer.MaxSpectators = 0;
                NewServer.GameRules = "((\"AllowCheats\", False),(\"AllowDeadChat\", True),(\"AllowUnrestrictedRadio\", False),(\"AllowUnrestrictedVoice\", False),(\"SpectateEnemies\", True),(\"SpectateForceFirstPerson\", False),(\"SpectateFreeCam\", True),(\"UseTeamRestrictions\", False))";
                NewServer.ServerPassword = "";

            }

            NewServer.ServerBasePath = BasePath;
            NewServer.ServerPath = ServerExePath;
            NewServer.Header = NewServer.ServerName.Substring(0, 15);
            NewServer.RestartTime = 24;

            return NewServer;
        }       
    }
}