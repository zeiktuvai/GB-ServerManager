using System;
using System.IO;
using System.Windows.Controls;
using GB_ServerManager.Helpers;
using GB_ServerManager.Models;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace GB_ServerManager.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();

          
        }

        public void TestMethod(object sender, EventArgs e)
        {
            JSONHelper.ReadServersFromFile();
        }
       

    }
}
