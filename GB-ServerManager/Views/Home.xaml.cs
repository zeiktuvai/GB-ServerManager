using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList);
          
        }

        public void TestMethod(object sender, EventArgs e)
        {
            //var test = SteamA2SHelper.A2S_INFO.GetA2SInformation(new IPEndPoint(IPAddress.Parse("75.15.0.21"),27016));
            //ProcessHelper.StartServer(new ServerSetting());
            //ProcessHelper.StopServer();
        }
       
        private List<ServerSetting> UpdateServerStatus(ServerList list)
        {
            if (list != null && list.Servers != null)
            {
                foreach (var item in list.Servers)
                {
                    item._Status = new SolidColorBrush(Colors.Red);

                    if (item._ServerPID != 0)
                    {   //TODO: set this to localhost
                        var PlayerStats = SteamA2SHelper.A2S_INFO.GetA2SInformation(new IPEndPoint(IPAddress.Parse("75.15.0.21"), 27016));
                        item._PlayerStats = string.Format("Players: {0}/{1}", PlayerStats.Players, PlayerStats.MaxPlayers);
                        if (PlayerStats.MaxPlayers != 0)
                        {
                            item._Status = new SolidColorBrush(Colors.Green);
                        }
                    }
                    else
                    {
                        item._PlayerStats = "Players: 0/0";
                    }
                }

                return list.Servers;
            }

            return new List<ServerSetting>();
        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var server = ((sender as Button).DataContext as ServerSetting);
            var index = ServerCache._ServerList.Servers.IndexOf(server);
            
            NavigationService.Navigate(new Servers(index));
        }

        private void btnStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var server = ((sender as Button).DataContext as ServerSetting);
            ProcessHelper.StartServer(server);
            UpdateServerStatus(ServerCache._ServerList);
        }

        private void btnStop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var server = ((sender as Button).DataContext as ServerSetting);
            ProcessHelper.StopServer(server._ServerPID);
            UpdateServerStatus(ServerCache._ServerList);
        }

    }
}
