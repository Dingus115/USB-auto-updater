using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Distroflex_device_update2._0.views
{
    /// <summary>
    /// Interaction logic for sdLoader.xaml
    /// </summary>
    public partial class sdLoader : UserControl
    {
        DirectoryInfo di = new DirectoryInfo("C:\\\\");
        public sdLoader()
        {
            InitializeComponent();
            locationChanged();
        }

        private void locationChanged()
        {
            sdLocation.Items.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo i in allDrives)
            {
                if (i.IsReady == true && !i.Name.Contains("C"))
                {
                    drives drives = new drives();
                    drives.driveSize = (double.Parse(i.TotalSize.ToString()) / 1000000000).ToString("N1").ToString() + " GB";
                    drives.name = i.Name;
                    drives.driveDescription = $"{drives.name} {drives.driveSize}";
                    sdLocation.Items.Add(drives);
                }
            }
        }

        private void deleteExistingFiles(object sender, RoutedEventArgs e)
        {
            drives drive = sdLocation.SelectedItem as drives;
            var result = MessageBox.Show($"You are about to delte all files located on Drive:{drive.name}\nAre you sure?", "Delete All Files", MessageBoxButton.YesNo);
            
            if(result == MessageBoxResult.Yes)
            {
                di = new DirectoryInfo(drive.name);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            else
            {
                MessageBox.Show("Operation Canceled", "Delete All Files", MessageBoxButton.OK);
            }
        }

        private void loadSdCard(object sender, RoutedEventArgs e)
        {
            drives drive = sdLocation.SelectedItem as drives;
            File.WriteAllText($"{drive.name}RunRemote!.mcf", "run u:");
        }

        private void refreshDrives(object sender, RoutedEventArgs e)
        {
            locationChanged();
        }
    }

    public class drives
    {
        public string name { get; set; } = string.Empty;
        public string driveSize { get; set; } = string.Empty;
        public string driveDescription { get; set; } = string.Empty;
    }
}
