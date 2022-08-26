using GB_ServerManager.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using GB_ServerManager.Services;
using GB_ServerManager.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Threading;
using System;

namespace GB_ServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _ActiveButtonColor = "#626050";
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Tick += new EventHandler(DispatcherHelper.RunServerProcessCheck);
            dispatcherTimer.Start();
            ServerService.GetGBServers();

            frame.NavigationService.Navigate(new Home());
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            btnHome.Background = (new BrushConverter()).ConvertFrom(_ActiveButtonColor) as Brush;
            checkSettingsStatus();

            try
            {
                var ver = typeof(MainWindow).Assembly.GetName().Version;
                lblVer.Content = string.Format("v{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);
            }
            catch (System.Exception)
            {
                lblVer.Content = "v0.0.0";                
            }

            if (!checkRunAsAdmin())
            {
                MessageBox.Show("Warning: You are not running this application as administrator. You may have issues starting and stopping servers.", "Not running as admin", MessageBoxButton.OK);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Home());
            btnHome.Background = (new BrushConverter()).ConvertFrom(_ActiveButtonColor) as Brush;
            btnServer.Background = new SolidColorBrush(Colors.Transparent);
            btnSettings.Background = new SolidColorBrush(Colors.Transparent);
            checkSettingsStatus();
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Servers());
            btnServer.Background = (new BrushConverter()).ConvertFrom(_ActiveButtonColor) as Brush;
            btnHome.Background = new SolidColorBrush(Colors.Transparent);
            btnSettings.Background = new SolidColorBrush(Colors.Transparent);
            checkSettingsStatus();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Settings());
            btnSettings.Background = (new BrushConverter()).ConvertFrom(_ActiveButtonColor) as Brush;
            btnServer.Background = new SolidColorBrush(Colors.Transparent);
            btnHome.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void dragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            List<int> runningServers = new List<int>();

            foreach (var server in ServerCache._ServerList.Servers)
            {
                if (server._ServerPID != 0 && ProcessHelper.GetServerStatus(server._ServerPID))
                {
                    runningServers.Add(server._ServerPID);
                }
            }

            if (runningServers.Count > 0)
            {
                var test = MessageBox.Show("There are running servers, Are you sure you wish to quit and close the servers?", "Really quit bro?", MessageBoxButton.YesNo);
                if (test == MessageBoxResult.Yes)
                {
                    bool allKilled = false;

                    foreach (var server in runningServers)
                    {
                        allKilled = ProcessHelper.StopServer(server);
                    }

                    if (allKilled == true)
                    {
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to stop all servers, you will have to manually terminate server processes.");
                        Close();
                    }
                }
            }
            else
            {
                Close();
            }
        }

        private void checkSettingsStatus()
        {
            //if (AppSettingsHelper.ReadSettings() == null)
            //{
            //    btnSettings.Background = new SolidColorBrush(Colors.Red);
            //}
        }

        private bool checkRunAsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
