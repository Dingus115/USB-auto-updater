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
    /// Interaction logic for password_confirmation_window.xaml
    /// </summary>
    public partial class password_confirmation_window : Window
    {
        public string password;
        public password_confirmation_window()
        {
            InitializeComponent();
            adminAccessComboBox.SelectedIndex = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(initialPasswordtextbox.Text == confirmationPasswordtextbox.Text && initialPasswordtextbox.Text != "")
            {
                MessageBoxResult result = MessageBoxResult.No;
                if (adminAccessComboBox.SelectedIndex == 0)
                {
                    result = MessageBox.Show("You are about to give this user\naccess to editing the database.\nAre you sure?", "Adding admin", MessageBoxButton.YesNo);
                }
                if(result == MessageBoxResult.No)
                {
                    adminAccessComboBox.SelectedIndex = 1;
                }
                password = initialPasswordtextbox.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK);
                initialPasswordtextbox.Text = "";
                confirmationPasswordtextbox.Text = "";
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(password != null)
            {

            }
            else
            {
                password = null;
            }
            
        }
    }
}
