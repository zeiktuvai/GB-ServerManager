using System.Windows.Controls;
using GB_ServerManager.Models;
using GB_ServerManager.Helpers;
using GB_ServerManager.Services;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Collections.Generic;

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
            string GBFolderName = "GroundBranch";
            string BasePath = "";
            string ServerExePath = "";
            bool? result = ofd.ShowDialog();

            if (result != null)
            {
                try
                {
                    string DirPath = Path.GetDirectoryName(ofd.FileName);
                    
                    if (ofd.FileName.Contains("GroundBranchServer-Win64-Shipping.exe"))
                    {
                        string path = Path.GetDirectoryName(ofd.FileName);
                        BasePath = path.Substring(0, path.IndexOf(GBFolderName));
                        ServerExePath = ofd.FileName;
                    }

                    if (ofd.FileName.Contains("GroundBranchServer.exe"))
                    {
                        BasePath = Path.GetDirectoryName(ofd.FileName);
                        var exeFiles = Directory.GetFiles(BasePath, "*.exe", SearchOption.AllDirectories);
                        foreach (var item in exeFiles)
                        {
                            if (item.Contains("GroundBranchServer-Win64-Shipping.exe"))
                            {
                                ServerExePath = item;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(BasePath))
                    {
                        throw new FileNotFoundException();
                    }
                    else
                    {
                        try
                        {
                            ServerService.AddGBServer(BasePath, ServerExePath);
                            PopulateServerList();
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(string.Format("Error adding server. {0}", ex.Message), "Error adding server.", System.Windows.MessageBoxButton.OK);                            
                        }
                    }

                }
                catch (System.Exception)
                {
                    MessageBox.Show("An error occured finding the Ground Branch executable file." + System.Environment.NewLine + "Please make sure you selected the right directory for your Ground Branch server install, and that it is not corrupt.", "Error adding server", System.Windows.MessageBoxButton.OK);
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
    }
}
