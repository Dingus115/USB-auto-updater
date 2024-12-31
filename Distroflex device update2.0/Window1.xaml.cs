using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Distroflex_device_update2._0
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class serverEditor : Window
    {
        public serverEditor()
        {
            InitializeComponent();
        }

        private void deviceCreatorWindow(object sender, RoutedEventArgs e)
        {
            viewHider(1);
        }

        private void deviceEditorWindow(object sender, RoutedEventArgs e)
        {
            viewHider(2);
        }

        private void osEditorWindow(object sender, RoutedEventArgs e)
        {
            viewHider(3);
        }

        private void engineEditorWindow(object sender, RoutedEventArgs e)
        {
            viewHider(4);
        }

        private void userEditorWindow(object sender, RoutedEventArgs e)
        {
            viewHider(5);
        }

        private void viewHider(int viewNumber)
        {
            switch (viewNumber)
            {
                case 1:
                    deviceCreatorView.Visibility = Visibility.Visible;
                    deviceEditorView.Visibility = Visibility.Collapsed;
                    osEditorView.Visibility = Visibility.Collapsed;
                    engineEditorView.Visibility = Visibility.Collapsed;
                    userEditorView.Visibility = Visibility.Collapsed;

                    deviceCreatorButton.IsEnabled = false;
                    deviceEditorButton.IsEnabled = true;
                    osEditorButton.IsEnabled = true;
                    enginesEditorButton.IsEnabled = true;
                    userEditorButton.IsEnabled = true;
                    break;

                case 2:
                    deviceCreatorView.Visibility = Visibility.Collapsed;
                    deviceEditorView.Visibility = Visibility.Visible;
                    osEditorView.Visibility = Visibility.Collapsed;
                    engineEditorView.Visibility = Visibility.Collapsed;
                    userEditorView.Visibility = Visibility.Collapsed;

                    deviceCreatorButton.IsEnabled = true;
                    deviceEditorButton.IsEnabled = false;
                    osEditorButton.IsEnabled = true;
                    enginesEditorButton.IsEnabled = true;
                    userEditorButton.IsEnabled = true;
                    break;

                case 3:
                    deviceCreatorView.Visibility = Visibility.Collapsed;
                    deviceEditorView.Visibility = Visibility.Collapsed;
                    osEditorView.Visibility = Visibility.Visible;
                    engineEditorView.Visibility = Visibility.Collapsed;
                    userEditorView.Visibility = Visibility.Collapsed;

                    deviceCreatorButton.IsEnabled = true;
                    deviceEditorButton.IsEnabled = true;
                    osEditorButton.IsEnabled = false;
                    enginesEditorButton.IsEnabled = true;
                    userEditorButton.IsEnabled = true;
                    break;

                case 4:
                    deviceCreatorView.Visibility = Visibility.Collapsed;
                    deviceEditorView.Visibility = Visibility.Collapsed;
                    osEditorView.Visibility = Visibility.Collapsed;
                    engineEditorView.Visibility = Visibility.Visible;
                    userEditorView.Visibility = Visibility.Collapsed;

                    deviceCreatorButton.IsEnabled = true;
                    deviceEditorButton.IsEnabled = true;
                    osEditorButton.IsEnabled = true;
                    enginesEditorButton.IsEnabled = false;
                    userEditorButton.IsEnabled = true;
                    break;

                case 5:
                    deviceCreatorView.Visibility = Visibility.Collapsed;
                    deviceEditorView.Visibility = Visibility.Collapsed;
                    osEditorView.Visibility = Visibility.Collapsed;
                    engineEditorView.Visibility = Visibility.Collapsed;
                    userEditorView.Visibility = Visibility.Visible;

                    deviceCreatorButton.IsEnabled = true;
                    deviceEditorButton.IsEnabled = true;
                    osEditorButton.IsEnabled = true;
                    enginesEditorButton.IsEnabled = true;
                    userEditorButton.IsEnabled = false;
                    break;
            }
        }

        
    } 
}
