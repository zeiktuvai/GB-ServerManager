using System.IO;
using System.Text.RegularExpressions;
using GB_ServerManager.Models;
using System;
using System.Text;

namespace GB_ServerManager.Helpers
{
    internal static class GBServerHelper
    {
        internal static ServerSetting RetrieveGBServerProperties(string BasePath, string ServerExePath)
        {
            string ServerConfigFile = "";
            string ServerIniPath = "";
            ServerSetting NewServer = new ServerSetting();

            NewServer.ServerId = Guid.NewGuid();

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
        
        internal static string GetNewGBServerDirectory()
        {
            if (!string.IsNullOrWhiteSpace((AppSettingsHelper.ReadSettings()).ServerBasePath))
            {
                var _basePath = AppSettingsHelper.ReadSettings().ServerBasePath;
                var _serverFolderName = "Server_";

                if (Directory.Exists(_basePath))
                {
                    int dirNumber = 1;
                    bool emptyDirFound = false;
                    string? path = Path.Combine(_basePath, _serverFolderName + dirNumber);

                    while (emptyDirFound == false)
                    {
                        var dirs = Directory.GetDirectories(_basePath);

                        if (dirs.Length > 0)
                        {
                            foreach (var item in dirs)
                            {
                                path = Path.Combine(_basePath, _serverFolderName + dirNumber);
                            
                                if (item.Contains(path))
                                {
                                    dirNumber++;                                   
                                }                                
                            }

                            path = Path.Combine(_basePath, _serverFolderName + dirNumber);
                            if (!Directory.Exists(path))
                            {
                                emptyDirFound = true;
                                Directory.CreateDirectory(path);
                            }
                        }
                        else
                        {
                            emptyDirFound = true;
                            Directory.CreateDirectory(path);
                        }

                    }
                    return path;
                }
            }

            return null;
        }

        internal static void CreateUpdateServerINIFile(ServerSetting server)
        {
            StringBuilder file = new StringBuilder();
            var INIPath = Path.Combine(server.ServerBasePath, "GroundBranch\\ServerConfig");

            file.AppendLine("[/Script/RBZooKeeper.ZKServer]");
            file.AppendLine(String.Format("ServerName={0}", server.ServerName));
            file.AppendLine(String.Format("ServerMOTD={0}", server.ServerMOTD));
            file.AppendLine(String.Format("MaxPlayers={0}", server.MaxPlayers));
            file.AppendLine(String.Format("MaxSpectators={0}", server.MaxSpectators));
            file.AppendLine(String.Format("GameRules={0}", server.GameRules));
            file.AppendLine(String.Format("ServerPassword={0}", server.ServerPassword));
            file.AppendLine(String.Format("SpectatorOnlyPassword={0}", server.SpectatorOnlyPassword));

            if (!Directory.Exists(INIPath))
            {
                Directory.CreateDirectory(INIPath);
            }
            File.WriteAllText(INIPath + "\\Server.ini", file.ToString());
        }
    }
}