using GB_ServerManager.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GB_ServerManager.Helpers;
using System.Windows.Media;
using System.Net;
using System;
using System.IO.Compression;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private static string _LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GB-ServerManager";
        
        public Settings()
        {
            InitializeComponent();

            var settings = AppSettingsHelper.ReadSettings();
            if (settings != null)
            {
                tbxBaseSrvrPath.Text = settings.ServerBasePath;
                tbxStmCmdPath.Text = settings.SteamCMDPath;
            }

            if (!File.Exists(tbxStmCmdPath.Text))
            {
                lblSCPError.Visibility = Visibility.Visible;
                btnDownloadSteam.IsEnabled = true;

            } else
            {
                lblSCPError.Visibility = Visibility.Hidden;
                btnDownloadSteam.IsEnabled = false;
            }

            if (!Directory.Exists(tbxBaseSrvrPath.Text))
            {
                lblSBPError.Visibility = Visibility.Visible;
            }
            else
            {
                lblSBPError.Visibility = Visibility.Hidden;
            }
                
            
        }

        private void bttnOpenSteamCmd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tbxStmCmdPath.Text = OpenFilePickerAndSelectFile("Select SteamCMD Path");
        }

        private void bttnOpenServerPath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tbxBaseSrvrPath.Text = OpenFilePickerAndSelectFile("Select Path where servers will be installed");
        }

        private string OpenFilePickerAndSelectFile(string title)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = title;
            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                return ofd.FileName;
            }
            else
            {
                return "";
            }

        }

        private void bttnSaveSttngs_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxBaseSrvrPath.Text) && !string.IsNullOrEmpty(tbxStmCmdPath.Text))
            {
                if (File.Exists(tbxStmCmdPath.Text) && Directory.Exists(tbxBaseSrvrPath.Text))
                {
                    var result = AppSettingsHelper.SaveSetting(new EnvSettings
                    {
                        ServerBasePath = tbxBaseSrvrPath.Text,
                        SteamCMDPath = tbxStmCmdPath.Text
                    });                    
                    //((MainWindow)System.Windows.Application.Current.MainWindow).btnSettings.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;

                    if (!result)
                    {
                        MessageBox.Show("An error occured saving your settings." + System.Environment.NewLine + "If this error persists please contact the developer.", "Error saving settings", System.Windows.MessageBoxButton.OK);
                    }
                    else
                    {
                        lblSBPError.Visibility = Visibility.Hidden;
                        lblSCPError.Visibility = Visibility.Hidden;
                    }
                    //TODO: something similar to the servers page to show that it has saved.
                }
                else
                {
                    MessageBox.Show("Failed to save." + System.Environment.NewLine + "Check that SteamCMD exists in the selected location and that the base server path folder exists.", "File/Directory Error", System.Windows.MessageBoxButton.OK);
                }

            }
        }

        private void btnDownloadSteam_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((AppSettingsHelper.ReadSettings()).SteamCMDPath))
            {
                try
                {
                    if (!Directory.Exists(_LocalAppDataPath))
                    {
                        Directory.CreateDirectory(_LocalAppDataPath);
                    }
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", _LocalAppDataPath + "\\steamcmd.zip");
                    }
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Error downloading SteamCMD, please try doing it manually");                 
                }
                                
                ZipFile.ExtractToDirectory(_LocalAppDataPath + "\\steamcmd.zip", _LocalAppDataPath + "\\steamcommand", true);

                if (File.Exists(_LocalAppDataPath + "\\steamcommand\\steamcmd.exe"))
                {
                    var settings = new EnvSettings { SteamCMDPath = _LocalAppDataPath + "\\steamcommand\\steamcmd.exe" };
                    tbxStmCmdPath.Text = settings.SteamCMDPath;
                    var result = AppSettingsHelper.SaveSetting(settings);

                    if (result)
                    {
                        lblSCPError.Visibility = Visibility.Hidden;
                    }
                } 
            }
        }
    }
}
