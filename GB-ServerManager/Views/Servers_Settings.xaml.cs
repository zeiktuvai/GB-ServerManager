using GB_ServerManager.Models;
using System.Windows.Controls;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Servers_Settings.xaml
    /// </summary>
    public partial class Servers_Settings : Page
    {
        public Servers_Settings(ServerSetting server)
        {
            InitializeComponent();

            this.DataContext = server;
        }
    }
}
