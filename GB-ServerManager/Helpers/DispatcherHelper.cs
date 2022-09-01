using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GB_ServerManager.Helpers
{
    public static class DispatcherHelper
    {
        public static void RunServerProcessCheck(object sender, EventArgs e)
        {
            foreach (var server in ServerCache._ServerList.Servers)
            {
                if (server._ServerPID != 0)
                {
                    var status = ProcessHelper.GetServerStatus(server._ServerPID);
                    if (status == false)
                    {
                        if (!string.IsNullOrWhiteSpace(AppSettingsHelper.ReadSettings().SteamCMDPath))
                        {
                            var proc = SteamCMDHelper.DownloadUpdateNewServer(server);
                            while (Process.GetProcessById(proc).HasExited != true)
                            {
                                Thread.Sleep(5000);
                            }
                        }
                        ProcessHelper.StartServer(server);
                    }
                }
            }
        }
    }
}
