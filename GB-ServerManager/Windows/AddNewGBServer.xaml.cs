using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System.Windows;
using System;

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
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //Open a new window to get information or something
            //XXCall to get directory
            //run steamcmd process to download
            //XXprobably create a server INI
            //use add server stuff to import the server dir into servercache including ports and stuff
            //refresh server list
            ServerSetting server = new ServerSetting
            {
                ServerName = "[YDL] Discord Member Server",
                ServerMOTD = "<h1>Welcome to the YDL Private Server",
                MaxPlayers = 16,
                MaxSpectators = 0,
                GameRules = "((\"AllowCheats\", False),(\"AllowDeadChat\", True),(\"AllowUnrestrictedRadio\", False),(\"AllowUnrestrictedVoice\", False),(\"SpectateEnemies\", True),(\"SpectateForceFirstPerson\", False),(\"SpectateFreeCam\", True),(\"UseTeamRestrictions\", False))",
                ServerPassword = "Password"
            };
            server.ServerBasePath = GBServerHelper.GetNewGBServerDirectory();
            GBServerHelper.CreateServerINIFile(server);
            var test = GBServerHelper.GetServerINIFile(server);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ServerSetting addServer = new ServerSetting();



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
    }
}
