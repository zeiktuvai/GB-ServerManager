using GB_ServerManager.Models;
using GB_ServerManager.Services;
using GB_ServerManager.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Servers.xaml
    /// </summary>

    public partial class Servers : Page
    {
        public Servers()
        {
            InitializeComponent();
            PopulateServerList();
            lvServers.SelectedIndex = 0;
            if (lvServers.Items.Count != 0)
            {
                NavigateServerSelection(lvServers.SelectedItem as ServerSetting);
            }
        }

        public Servers(int index)
        {
            InitializeComponent();
            PopulateServerList();
            lvServers.SelectedIndex = index;
            NavigateServerSelection(lvServers.SelectedItem as ServerSetting);
                        
        }

        private void PopulateServerList()
        {            
            lvServers.Items.Clear();
            foreach (var server in ServerCache._ServerList.Servers)
            {
                lvServers.Items.Add(server);
            }
        }

        private void btnAddExisting_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executable File (*.exe)|*.exe";
            bool? result = ofd.ShowDialog();

            if (result != null)
            {
                var serverPaths = Helpers.GBServerHelper.FindGBServerExecutable(ofd.FileName);

                if (string.IsNullOrEmpty(serverPaths.ServerBasePath))
                {
                    MessageBox.Show("An error occured finding the Ground Branch executable file." + System.Environment.NewLine + "Please make sure you selected the right directory for your Ground Branch server install, and that it is not corrupt.", "Error adding server", System.Windows.MessageBoxButton.OK);                    
                }
                else
                {
                    try
                    {
                        ServerService.AddGBServer(serverPaths.ServerBasePath, serverPaths.ServerPath);
                        PopulateServerList();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(string.Format("Error adding server. {0}", ex.Message), "Error adding server.", System.Windows.MessageBoxButton.OK);
                    }
                }
            }  
        }

        private void lvServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = (sender as ListView).SelectedItem as ServerSetting;
            NavigateServerSelection(val);
        }

        private void NavigateServerSelection(ServerSetting server)
        {
            frmServer.Navigate(new Servers_Settings(server));
        }

        private void btnAddNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddNewGBServer addWindow = new AddNewGBServer();
            var result = addWindow.ShowDialog().Value;

            if (result == true)
            {
                PopulateServerList();
            }

        }
    }
}
