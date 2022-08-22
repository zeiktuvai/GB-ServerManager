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
        internal List<ServerSetting> _ServerList { get; set; }
        
        public Servers()
        {
            InitializeComponent();
            PopulateServerList();
        }

        public Servers(int index)
        {
            InitializeComponent();
            PopulateServerList();
            tbcServerList.SelectedIndex = index;
                        
        }

        private void PopulateServerList()
        {
            ServerCache._ServerList = JSONHelper.ReadServersFromFile();
            if (ServerCache._ServerList.Servers != null)
            {
                _ServerList = ServerCache._ServerList.Servers;
                //foreach (var server in _ServerList)
                //{
                //    tbcServerList.Items.Add(server);
                //}
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
                        var Server = GBServerHelper.RetrieveGBServerProperties(BasePath, ServerExePath);
                        var item = tbcServerList.Items.Add(Server);
                        tbcServerList.SelectedIndex = item;
                        JSONHelper.SaveServerToFile(Server);
                    }

                }
                catch (System.Exception)
                {
                    MessageBox.Show("An error occured finding the Ground Branch executable file." + System.Environment.NewLine + "Please make sure you selected the right directory for your Ground Branch server install, and that it is not corrupt.", "Error adding server", System.Windows.MessageBoxButton.OK);
                }
            }

           
        }

        private void btnSaveServer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var BaseServerSetting = ((sender as Button).DataContext as ServerSetting);
            //var parent = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as Button)));
            //var test = VisualTreeHelper.GetChild(parent, 1);
            //var test2 = VisualTreeHelper.GetChild(test, 3);
            //BaseServerSetting.MultiHome = tbxSrvrMulHome.Text;
        }
    }
}
