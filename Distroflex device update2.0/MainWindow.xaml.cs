using Distroflex_device_update2._0;
using Distroflex_device_update2._0.Properties;
using Microsoft.Win32.SafeHandles;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Distroflex_device_update_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            updateDeviceTabButton.IsEnabled = false;
        }
        private void updateDeviceTab(object sender, RoutedEventArgs e)
        {
            if(deviceUpdateView.Visibility == Visibility.Collapsed)
            {
                deviceUpdateView.Visibility = Visibility.Visible;
                serverManagerView.Visibility = Visibility.Collapsed;
                sdLoaderView.Visibility = Visibility.Collapsed;

                updateDeviceTabButton.IsEnabled = false;
                serverManagerTabTabButton.IsEnabled = true;
                sdLoaderTabButton.IsEnabled = true;
            }
        }

        private void serverManagerTab(object sender, RoutedEventArgs e)
        {
            if (serverManagerView.Visibility == Visibility.Collapsed)
            {
                serverManagerView.Visibility = Visibility.Visible;
                deviceUpdateView.Visibility = Visibility.Collapsed;
                sdLoaderView.Visibility = Visibility.Collapsed;

                updateDeviceTabButton.IsEnabled = true;
                serverManagerTabTabButton.IsEnabled = false;
                sdLoaderTabButton.IsEnabled = true;
            }
        }

        private void sdLoaderTab(object sender, RoutedEventArgs e)
        {
            if (sdLoaderView.Visibility == Visibility.Collapsed)
            {
                serverManagerView.Visibility = Visibility.Collapsed;
                deviceUpdateView.Visibility = Visibility.Collapsed;
                sdLoaderView.Visibility = Visibility.Visible;

                updateDeviceTabButton.IsEnabled = true;
                serverManagerTabTabButton.IsEnabled = true;
                sdLoaderTabButton.IsEnabled = false;
            }
        }

        public void CloseButton_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void SignOut(object sender, RoutedEventArgs e)
        {
            Settings.Default.User = "";
            Settings.Default.password = "";
            Settings.Default.staySignedIn = 0;
            Settings.Default.Save();
            companySignInWindow temp = new companySignInWindow();
            temp.Show();
            this.Close();
        }
    }
}
