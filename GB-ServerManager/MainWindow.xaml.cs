using GB_ServerManager.Helpers;
using GB_ServerManager.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace GB_ServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Home());
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            btnHome.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;

            if (AppSettingsHelper.ReadSettings() == null)
            {
                btnSettings.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Home());
            btnHome.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;
            btnServer.Background = new SolidColorBrush(Colors.Transparent);
            btnSettings.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Servers());
            btnServer.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;
            btnHome.Background = new SolidColorBrush(Colors.Transparent);
            btnSettings.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Settings());
            btnSettings.Background = (new BrushConverter()).ConvertFrom("#778c2d") as Brush;
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
            //TODO: add code to check if servers are running and ask if you want them to be closed.
            Close();
        }

    }
}
