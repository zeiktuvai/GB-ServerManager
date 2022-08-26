using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        ProcessHelper.StartServer(server);
                    }
                }
            }
        }
    }
}
