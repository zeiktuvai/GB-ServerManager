using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace GB_ServerManager.Helpers
{
    internal static class ProcessHelper
    {
        internal static bool StartServer(ServerSetting server)
        {
            if (!string.IsNullOrEmpty(server.MultiHome)
                & server.Port != 0
                & server.QueryPort != 0
                & server.RestartTime != 0)
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = server.ServerPath,
                        Arguments = String.Format("Multihome={0} Port={1} QueryPort={2} ScheduledShutdownTime={3} -LOCALLOGTIMES -log", server.MultiHome, server.Port, server.QueryPort, server.RestartTime),
                        //WindowStyle = ProcessWindowStyle.Hidden,
                        //RedirectStandardOutput = true
                    }

                };
                var process = proc.Start();
                ServerCache._ServerList.Servers.Find(s => s.ServerName == server.ServerName)._ServerPID = proc.Id;
    
                //TODO: redirected log stuff
                //proc.BeginOutputReadLine();
            
                return process;
            }
            else
            {
                MessageBox.Show("An error occured starting the server." + System.Environment.NewLine + "Please make sure all required information is set first (Multihome address, Port, QueryPort and Restart Time.", "Error starting server", System.Windows.MessageBoxButton.OK);
                return false;
            }
            

        }

        internal static bool StopServer(int serverPID)
        {
            var serverProc = Process.GetProcessById(serverPID);
            try
            {
                serverProc.Kill();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to stop server, please make sure you are running as administrator", "Error stopping", System.Windows.MessageBoxButton.OK);
                return false;
            }

            if (Process.GetProcessById(serverPID).HasExited)
            {
                ServerCache._ServerList.Servers.Find(p => p._ServerPID == serverPID)._ServerPID = 0;
                return true;
            }
            return false;
            
        }

        public static bool GetServerStatus(int serverPID)
        {
            Process proc = null;

            try
            {
                proc = Process.GetProcessById(serverPID);
            }
            catch (Exception)
            {
                return false;                
            }
         
            
            if (proc != null && !proc.HasExited)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
