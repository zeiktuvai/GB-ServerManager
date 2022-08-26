using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
