using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using Xceed.Wpf.Toolkit;
using System.Windows;
using System.Linq;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private DispatcherTimer UITimer = new DispatcherTimer();

        public Home()
        {
            InitializeComponent();
            lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList);
            UITimer.Interval = new TimeSpan(0, 0, 30);
            UITimer.Tick += new EventHandler(RunServerListUpdate);
            UITimer.Start();
        }


        private List<ServerSetting> UpdateServerStatus(ServerList list, bool initial = false)
        {
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            if (list != null && list.Servers != null)
            {
                foreach (var item in list.Servers)
                {
                    item._Status = new SolidColorBrush(Colors.Red);

                    if (item._ServerPID != 0)
                    {
                        if (ProcessHelper.GetServerStatus(item._ServerPID))
                        {
                            item._Status = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            item._Status = new SolidColorBrush(Colors.Red);
                        }

                        Task.Run(() =>
                        {

                            var ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), item.QueryPort);
                            var PlayerStats = SteamA2SHelper.A2S_INFO.GetA2SInformation(ip, initial);

                            if (PlayerStats.MaxPlayers != 0)
                            {
                                var updateServer = ServerCache._ServerList.Servers.Find(s => s.ServerId == item.ServerId);
                                updateServer._PlayerStats = string.Format("Players: {0}/{1}", PlayerStats.Players, PlayerStats.MaxPlayers);
                            }
                        }).ContinueWith((t) =>
                        {
                            lvServers.ItemsSource = null;
                            lvServers.ItemsSource = ServerCache._ServerList.Servers;
                        }, scheduler);
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
            if (server._ServerPID == 0 || (server._ServerPID != 0 && !ProcessHelper.GetServerStatus(server._ServerPID))) 
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
                lvServers.ItemsSource = null;
                lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList);
            }
        }

        private void RunServerListUpdate(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show("tick");
            if (ServerCache._ServerList.Servers.FindAll(s => s._ServerPID != 0).Count > 0)
            {
                lvServers.ItemsSource = null;
                lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList, false);
            }
        }

    }
}
