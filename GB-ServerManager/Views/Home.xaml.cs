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

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        DispatcherTimer dispatchTimer = new DispatcherTimer();

        public Home()
        {
            InitializeComponent();
            lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList);
            dispatchTimer.Interval = new TimeSpan(0, 0, 15);
            //dispatchTimer.Tick += new EventHandler(RunServerListUpdate);
            dispatchTimer.Tick += new EventHandler(RunServerProcessCheck);
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
                        item._Status = new SolidColorBrush(Colors.Green);

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
            if (server._ServerPID == 0)
            {
                ProcessHelper.StartServer(server);
                lvServers.ItemsSource = null;
                lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList, true);

                if (dispatchTimer.IsEnabled == false)
                {
                    dispatchTimer.Start();
                }
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

                if (ServerCache._ServerList.Servers.Count > 1)
                {
                    var runningServers = ServerCache._ServerList.Servers.FindAll(s => s._ServerPID != 0);

                    if (runningServers.Count == 0)
                    {
                        dispatchTimer.Stop();
                    }
                }
                else
                {
                    dispatchTimer.Stop();
                }
            }
        }

        private void RunServerListUpdate(bool initial = false)
        {
            lvServers.ItemsSource = null;
            lvServers.ItemsSource = UpdateServerStatus(ServerCache._ServerList, initial);
        }

        private void RunServerProcessCheck(object sender, EventArgs e)
        {
            foreach (var server in ServerCache._ServerList.Servers)
            {
                if (server._ServerPID != 0)
                {
                    var status = ProcessHelper.GetServerStatus(server._ServerPID);
                    if (status == false)
                    {
                        ProcessHelper.StartServer(server);
                        RunServerListUpdate(true);
                    }
                    else
                    {
                        RunServerListUpdate(false);
                    }
                }
            }
        }
    }
}
