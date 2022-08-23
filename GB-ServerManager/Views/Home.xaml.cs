using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

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

       
        private List<ServerSetting> UpdateServerStatus(ServerList list, bool initial = false)
        {
            if (list != null && list.Servers != null)
            {
                foreach (var item in list.Servers)
                {
                    item._Status = new SolidColorBrush(Colors.Red);

                    if (item._ServerPID != 0)
                    {
                        item._Status = new SolidColorBrush(Colors.Green);

                        //TODO: find a way to update this.
                        PlayerCountCallback callback = new PlayerCountCallback(ReturnPlayerStats);
                        Thread playerWorker = new Thread(() => ReturnPlayerStats(new IPEndPoint(IPAddress.Parse("127.0.0.1"), item.QueryPort), item, initial));
                        playerWorker.Start();
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
            if (server._ServerPID == 0)
            {
                ProcessHelper.StartServer(server);
                lvServers.ItemsSource = null;
                lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList, true);
            }
        }

        private void btnStop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var server = ((sender as Button).DataContext as ServerSetting);
            if (server._ServerPID != 0)
            {
                ProcessHelper.StopServer(server._ServerPID);
                lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList);
            }
        }

        private void ReturnPlayerStats(IPEndPoint ip, ServerSetting server, bool initial)
        {
            var PlayerStats = SteamA2SHelper.A2S_INFO.GetA2SInformation(ip, initial);
            
            if (PlayerStats.MaxPlayers != 0)
            {
                var updateServer = ServerCache._ServerList.Servers.Find(s => s.ServerId == server.ServerId);
                updateServer._PlayerStats = string.Format("Players: {0}/{1}", PlayerStats.Players, PlayerStats.MaxPlayers);                
            }
        }

        private delegate void PlayerCountCallback(IPEndPoint ip, ServerSetting server, bool initial);
    }
}
