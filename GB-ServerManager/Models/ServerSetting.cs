﻿using System.Windows.Media;
using System.Text.Json.Serialization;

namespace GB_ServerManager.Models
{
    internal class ServerSetting
    {
        public string Header { get; set; }
        public string ServerBasePath { get; set; }
        public string ServerName { get; set; }
        public string ServerPath { get; set; }
        public string ServerMOTD { get; set; }
        public string ServerPassword { get; set; }
        public int Port { get; set; }
        public int QueryPort { get; set; }
        public int RestartTime { get; set; }
        public int MaxPlayers { get; set; }
        public int MaxSpectators { get; set; }
        public string GameRules { get; set; }
        [JsonIgnore]
        public SolidColorBrush _Status { get; set; }
        [JsonIgnore]
        public string _PlayerStats { get; set; }
        [JsonIgnore]
        public int _ServerPID { get; set; }
        [JsonIgnore]
        public bool _LaunchSepLogWindow { get; set; }
    }
}
