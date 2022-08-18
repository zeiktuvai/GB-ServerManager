using System.Text.Json;
using System.IO;
using GB_ServerManager.Models;
using System;
using System.Collections.Generic;

namespace GB_ServerManager.Helpers
{
    internal static class JSONHelper
    {
        private const string _FileName = "ServerList.JSON";
        private static string _LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GB_ServerManager";
        internal static bool SaveServerToFile(ServerSetting ServerToSave)
        {
            string path = Path.Combine(_LocalAppDataPath, _FileName);
            ServerList serverList = new ServerList();

            if (ServerToSave != null)
            {
                if (File.Exists(path))
                {
                    serverList = ReadServersFromFile();
                }

                try
                {
                    if (serverList.Servers != null)
                    {
                        var serverFound = serverList.Servers.Find(p => p.ServerPath == ServerToSave.ServerPath);
                       
                        if (serverFound != null)
                        {
                            serverList.Servers[serverList.Servers.IndexOf(serverFound)] = ServerToSave;
                        }
                        else
                        {
                            serverList.Servers.Add(ServerToSave);
                        }
                       
                        string JSONString = JsonSerializer.Serialize(serverList);
                        File.WriteAllText(path, JSONString);
                    }
                    else
                    {
                        serverList.Servers = new List<ServerSetting>();
                        serverList.Servers.Add(ServerToSave);
                        string JSONString = JsonSerializer.Serialize(serverList);

                        if (!Directory.Exists(_LocalAppDataPath))
                        {
                            Directory.CreateDirectory(_LocalAppDataPath);
                        }
                        File.WriteAllText(path, JSONString);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;
        }

        internal static ServerList ReadServersFromFile()
        {
            string path = Path.Combine(_LocalAppDataPath, _FileName);
            ServerList serverList = new ServerList();

            try
            {
                string JSONString = File.ReadAllText(path);
                serverList = JsonSerializer.Deserialize<ServerList>(JSONString)!;
                return serverList;
            }
            catch (System.Exception)
            {
                return null;
            }

            
        }
    }
}
