using System.Windows.Controls;
using GB_ServerManager.Models;
using GB_ServerManager.Helpers;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
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
            var serverList = JSONHelper.ReadServersFromFile();
            if (serverList.Servers != null)
            {
                foreach (var server in serverList.Servers)
                {
                    tbcServerList.Items.Add(server);
                }
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
                        tbcServerList.Items.Add(Server);
                        JSONHelper.SaveServerToFile(Server);
                    }

                }
                catch (System.Exception)
                {
                    MessageBox.Show("An error occured finding the Ground Branch executable file." + System.Environment.NewLine + "Please make sure you selected the right directory for your Ground Branch server install, and that it is not corrupt.", "Error adding server", System.Windows.MessageBoxButton.OK);
                }
            }

           
        }
    }
}
