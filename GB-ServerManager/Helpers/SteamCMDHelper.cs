using GB_ServerManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace GB_ServerManager.Helpers
{
    internal static class SteamCMDHelper
    {
        internal static int DownloadUpdateNewServer(ServerSetting server)
        {
            if (!string.IsNullOrWhiteSpace(server.ServerBasePath))
            {
                if (!Directory.Exists(server.ServerBasePath))
                {
                    Directory.CreateDirectory(server.ServerBasePath);
                }

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = AppSettingsHelper.ReadSettings().SteamCMDPath,
                        Arguments = string.Format("+force_install_dir {0} +login anonymous +app_update 476400 +quit", server.ServerBasePath)
                    }
                };
                proc.Start();

                return proc.Id;
            }

            return 0;
        }
    }
}
