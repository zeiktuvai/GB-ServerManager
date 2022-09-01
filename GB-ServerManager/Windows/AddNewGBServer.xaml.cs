using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System.Windows;
using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace GB_ServerManager.Windows
{
    /// <summary>
    /// Interaction logic for AddNewGBServer.xaml
    /// </summary>
    public partial class AddNewGBServer : Window
    {
        private const string GBServerAppID = "476400";        
        public AddNewGBServer()
        {
            InitializeComponent();

            if (ServerCache._ServerList.Servers != null)
            {
                var ports = FindPorts();
                tbxPort.Text = ports.Item1.ToString();
                tbxQueryPort.Text = ports.Item2.ToString();
            }
            else
            {
                tbxPort.Text = "7777";
                tbxQueryPort.Text = "27015";
            }

            if (!string.IsNullOrWhiteSpace(AppSettingsHelper.ReadSettings().ServerBasePath))
            {
                tbxServerPath.Text =  GBServerHelper.GetNewGBServerDirectory();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {           
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AppSettingsHelper.ReadSettings().ServerBasePath))
            {
                var backupPorts = FindPorts();

                ServerSetting addServer = new ServerSetting
                {
                    ServerId = Guid.NewGuid(),
                    ServerName = IsValidStringInput(tbxServerName.Text) ? tbxServerName.Text : "Unnamed Ground Branch Server",                    
                    ServerBasePath = tbxServerPath.Text,
                    ServerMOTD = String.IsNullOrWhiteSpace(tbxServerMOTD.Text) ? "Welcome!" : tbxServerMOTD.Text,
                    GameRules = "((\"AllowCheats\", False),(\"AllowDeadChat\", True),(\"AllowUnrestrictedRadio\", False),(\"AllowUnrestrictedVoice\", False),(\"SpectateEnemies\", True),(\"SpectateForceFirstPerson\", False),(\"SpectateFreeCam\", True),(\"UseTeamRestrictions\", False))",
                    MaxPlayers = IsValidIntegerRange(tbxMaxPlayer.Text, 1, 16) ? int.Parse(tbxMaxPlayer.Text) : 16,
                    MaxSpectators = IsValidIntegerRange(tbxMaxSpectator.Text, 0, 16) ? int.Parse(tbxMaxSpectator.Text) : 0,
                    RestartTime = IsValidIntegerRange(tbxRestartTime.Text, 1, 24) ? int.Parse(tbxRestartTime.Text) : 12,
                    MultiHome = Regex.IsMatch(tbxMultiHome.Text, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$") ? tbxMultiHome.Text : "0.0.0.0",
                    Port = IsValidInteger(tbxPort.Text) ? int.Parse(tbxPort.Text) : backupPorts.Item1,
                    QueryPort = IsValidInteger(tbxQueryPort.Text) ? int.Parse(tbxQueryPort.Text) : backupPorts.Item2,
                    ServerPassword = tbxPassword.Text,
                    SpectatorOnlyPassword = tbxSpectatorPass.Text
                };

                int proc = SteamCMDHelper.DownloadUpdateNewServer(addServer);

                while (Process.GetProcessById(proc).HasExited == false)
                {
                    Thread.Sleep(10000);
                }

                try
                {
                    addServer = GBServerHelper.RetrieveGBServerProperties(addServer);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error creating server, Please check your steamCMD path and base server path and try again.  If this persists contact the app developer");
                }

                if (ProcessHelper.InitialServerStart(addServer))
                {
                    GBServerHelper.CreateServerINIFile(addServer);
                }

                ServerService.AddGBServer(addServer);

                this.DialogResult = true;                
                this.Close();
            }
            else
            {
                MessageBox.Show("Please set the server base path in settings to continue.");
            }

            

        }

        private Tuple<int, int> FindPorts()
        {
            bool portFound = false;
            int port = 7777;
            int queryPort = 27015;
            

            while (portFound == false)
            {
                var search =  ServerCache._ServerList.Servers.Find(s => s.Port == port);

                if (search != null)
                {
                    port++;
                }
                else
                {
                    portFound = true;
                }
            }

            portFound = false;
            while (portFound == false)
            {
                var search = ServerCache._ServerList.Servers.Find(s => s.QueryPort == queryPort);

                if (search != null)
                {
                    queryPort++;
                }
                else
                {
                    portFound = true;
                }
            }


            Tuple<int, int> ports = new Tuple<int, int>(port, queryPort);
            return ports;
        }

        private bool IsValidStringInput(string value)
        {           
            if (!string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        private bool IsValidIntegerRange(string value, int min, int max)
        {
            bool isValidNumber = false;
            bool isInValidRange = false;
            int val = 0;

            try
            {
                val = int.Parse(value);
                isValidNumber = true;
            }
            catch (Exception)
            {
                isValidNumber = false;
            }

            if (val != 0)
            {
                if (val >= min && val <= max)
                {
                    isInValidRange = true;
                }
                else
                {
                    isInValidRange = false;
                }
            }

            return (isValidNumber && isInValidRange) ? true : false;
        }

        private bool IsValidInteger(string value)
        {
            try
            {
                int.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;                
            }
        }
    }
}
