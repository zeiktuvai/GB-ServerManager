using GB_ServerManager.Models;
using GB_ServerManager.Services;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using GB_ServerManager.Helpers;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Servers_Settings.xaml
    /// </summary>
    public partial class Servers_Settings : Page
    {
        private Brush _ActiveButtonColor = new BrushConverter().ConvertFrom("#626050") as Brush;
        public Servers_Settings(ServerSetting server)
        {
            InitializeComponent();
            this.DataContext = server;

            btnSettings.Background = _ActiveButtonColor;
        }


        private void btnSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void btnSaveServer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var server = (ServerSetting)this.DataContext;
            bool _validConfig = false;
            int _srvrPort = 0;
            int _queryPort = 0;

            //server.ServerPath = tbxSrvrPath.Text;
            //server.ServerName = 
            
            if (!string.IsNullOrEmpty(tbxSrvrMulHome.Text))
            {
                server.MultiHome = tbxSrvrMulHome.Text;
                _validConfig = true;
            }
            else
            {
                _validConfig = false;
            }
            
            if (int.TryParse(tbxServerPort.Text, out _srvrPort))
            {
                server.Port = _srvrPort;
                _validConfig = true;
            }
            else
            {
                MessageBox.Show("Error setting Server Port; Please make sure the port field contains only numbers", "Port Error", System.Windows.MessageBoxButton.OK);
                _validConfig = false;
            }

            if (int.TryParse(tbxQueryPort.Text, out _queryPort))
            {
                server.QueryPort = _queryPort;
                _validConfig = true;
            }
            else
            {
                MessageBox.Show("Error setting Query Port; Please make sure the query port field contains only numbers", "Port Error", System.Windows.MessageBoxButton.OK);
                _validConfig = false;
            }

            if (!(tbxRestartTime.Value < 1) && !(tbxRestartTime.Value > 24))
            {
                server.RestartTime = (int)tbxRestartTime.Value;
                _validConfig = true;
            }
            else
            {
                MessageBox.Show("Error setting Restart Time; Please make sure the restart time is between 1 and 24", "Restart Time Error", System.Windows.MessageBoxButton.OK);
                _validConfig = false;
            }

            server.LaunchSeperateLogWindow = cbxOpenLogWindow.IsChecked ?? false;

            if (_validConfig == true)
            {
                if (StatusLabel.Visibility == System.Windows.Visibility.Visible)
                {
                    StatusLabel.Visibility = System.Windows.Visibility.Hidden;
                }

                try
                {
                    ServerService.UpdateGBServer(server);
                    StatusLabel.Visibility = System.Windows.Visibility.Visible;
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Error saving settings", "Error", System.Windows.MessageBoxButton.OK);
                }
                
            }
        }
    }
}
