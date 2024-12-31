using Distroflex_device_update_2._0;
using Distroflex_device_update2._0.Properties;
using Distroflex_device_update2._0.views;
using Microsoft.Data.SqlClient;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Distroflex_device_update2._0
{
    /// <summary>
    /// Interaction logic for companySignInWindow.xaml
    /// </summary>
    public partial class companySignInWindow : Window
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        private SqlCommand commandOperator = new SqlCommand();
        public companySignInWindow()
        {
            InitializeComponent();
            if (Settings.Default.staySignedIn == 1)
            {
                userCredentials user = new userCredentials();
                user.username = Settings.Default.User;
                user.password = Settings.Default.password;
                validateConnection(user);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool connectionstate = serverCheck();
            if (connectionstate) 
            {
                sqlConnector.Open();
                userCredentials user  = new userCredentials();
                
                user.username = userNameTextbox.Text;
                user.password = passwordTextbox.Text;
                if (user.username != "" && user.password != "")
                {
                    bool validUser = validateConnection(user);
                }
                else
                {
                    MessageBox.Show("Please enter valid credentials!", "Error: Invalid credentials", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Error: Please check your network connection.\nCould not connect to database.", "Error: Could not connect to Database", MessageBoxButton.OK);
            }
        }

        private bool serverCheck()//method to check connection to server
        {
            bool connected = false;
            try
            {
                if(sqlConnector.State != ConnectionState.Open)
                {
                    sqlConnector.Open(); //open server
                }
                sqlConnector.Close(); //close server to not keep networking going
                connected = true;
            }
            catch
            {
                connected = false;
                //MessageBox.Show("There was an issue connecting to the database.\nPlease contact your IT admin", "Error");
            }

            return connected;
        }
        private bool validateConnection(userCredentials user)
        {

            bool validUser = false;
            if(sqlConnector.State != ConnectionState.Open)
            {
                sqlConnector.Open();
            }
            
            commandOperator = new SqlCommand("validateUser", sqlConnector);
            commandOperator.CommandType = System.Data.CommandType.StoredProcedure;
            commandOperator.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar) { Value = user.username });
            commandOperator.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = user.password });
            string valid = string.Empty;

            try
            {
                valid = (string)commandOperator.ExecuteScalar();
            }
            catch { valid = string.Empty; }

            int adminAccess = 0;
            commandOperator = new SqlCommand("adminCreds", sqlConnector);
            commandOperator.CommandType = System.Data.CommandType.StoredProcedure;
            commandOperator.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar) { Value = user.username });
            commandOperator.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = user.password });
            adminAccess = (int)commandOperator.ExecuteScalar();

            if (valid == user.username)
            {
                if (keepSignInCheckBox.IsChecked == true)
                {
                    Settings.Default.User = user.username;
                    Settings.Default.password = user.password;
                    Settings.Default.staySignedIn = 1;
                    Settings.Default.userIsAdmin = adminAccess;
                    Settings.Default.Save();
                }
                else
                {
                    Settings.Default.User = user.username;
                    Settings.Default.Save();
                }
                
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Credentials. Please try again", "Error: invalid user", MessageBoxButton.OK);
            }
            return validUser;
        }
    }
}
