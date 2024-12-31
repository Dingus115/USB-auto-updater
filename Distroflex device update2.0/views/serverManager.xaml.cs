using Distroflex_device_update_2._0;
using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class serverManager : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        private SqlCommand commandOperator = new SqlCommand();
        public serverManager()
        {
            InitializeComponent();
            if(Settings.Default.userIsAdmin == 1)
            {
                userName_textbox.Text = Settings.Default.User;
                password_textBox.Password = Settings.Default.password;
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            try
            { 
                if(sqlConnector.State != ConnectionState.Open)
                {
                    sqlConnector.Open();
                }
                if(userName_textbox.Text != "" && userName_textbox.Text != null)
                {
                    if (password_textBox.Password != "" && password_textBox.Password != null)
                    {
                        if (sqlConnector.State == System.Data.ConnectionState.Open)
                        {
                            commandOperator = new SqlCommand("adminCreds", sqlConnector);
                            commandOperator.CommandType = CommandType.StoredProcedure;
                            commandOperator.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = userName_textbox.Text });
                            commandOperator.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = password_textBox.Password });

                            object result = commandOperator.ExecuteScalar();
                            if (result != null && result != DBNull.Value && (int)result == 1)
                            {
                                serverEditor serverEditor = new serverEditor();
                                serverEditor.Show();
                                serverEditor.Closing += serverEditor_closing;
                                button_login.IsEnabled = false;
                            }
                            else
                            {
                                MessageBox.Show("Username OR password is incorrect", "Error: invalid credentials.", MessageBoxButton.OK);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a password!", "Error: no password", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a Username!", "Error: no username", MessageBoxButton.OK);
                }
                sqlConnector.Close();
            }
            catch { MessageBox.Show("There was an error connecting to the database.\nPlease contact your IT admin.", "Error: Network", MessageBoxButton.OK); }

            
        }
        private void serverEditor_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            button_login.IsEnabled = true;
        }
    }
}
