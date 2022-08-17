﻿using GB_ServerManager.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GB_ServerManager.Helpers;
using System.Windows.Media;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();

            var settings = AppSettingsHelper.ReadSettings();
            if (settings != null)
            {
                tbxBaseSrvrPath.Text = settings.ServerBasePath;
                tbxStmCmdPath.Text = settings.SteamCMDPath;
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
                    ((MainWindow)System.Windows.Application.Current.MainWindow).btnSettings.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;

                    if (!result)
                    {
                        MessageBox.Show("An error occured saving your settings." + System.Environment.NewLine + "If this error persists please contact the developer.", "Error saving settings", System.Windows.MessageBoxButton.OK);
                    }
                    //TODO: system bar at bottom to show setting saved
                }
                else
                {
                    MessageBox.Show("Failed to save." + System.Environment.NewLine + "Check that SteamCMD exists in the selected location and that the base server path folder exists.", "File/Directory Error", System.Windows.MessageBoxButton.OK);
                }

            }
        }
    }
}
