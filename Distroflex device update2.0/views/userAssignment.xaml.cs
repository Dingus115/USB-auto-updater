using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
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
    /// Interaction logic for userAssignment.xaml
    /// </summary>
    public partial class userAssignment : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);

        private sqlServerConnectionMethods sqlconnectorclass = new sqlServerConnectionMethods();

        private DataSet users = new DataSet();
        private List<string> currentUsers = new List<string>();

        private string newUserPassword;
        private int adminAccess;
        public userAssignment()
        {
            InitializeComponent();
            serverCheck();
        }

        private void serverCheck()//method to check connection to server
        {

            try
            {
                sqlConnector.Open(); //open server
                serverStatus.Content = "connected"; //show connection
                sqlConnector.Close(); //close server to not keep networking going
                loadTables();
            }
            catch
            {
                MessageBox.Show("There was an issue connecting to the database.\nPlease contact your IT admin", "Error");
            }
        }

        private void loadTables()
        {

            //refresh user datagrid and user combobox since both populate the same items
            users.Clear();
            users = retrieveItems("selectUsers");
            existingUsers.Items.Clear();
            userCombobox.Items.Clear();
            
            foreach (DataRow row in users.Tables[0].Rows)
            {
                string userName = row[0].ToString();
                int adminAccess = Convert.ToInt32(row[1].ToString());

                userCreds user = new userCreds();
                user.userName = userName;
                user.adminAccess = adminAccess;

                existingUsers.Items.Add(user);
                userCombobox.Items.Add(user.userName);
                currentUsers.Add(user.userName);
            }
            existingUsers.Items.Refresh();


            //refresh device comboBox
            List<string> devices = new List<string>();
            users = retrieveItems("selectDevices");
            deviceComboBox.Items.Clear();
            foreach (DataRow row in users.Tables[0].Rows)
            {
                deviceComboBox.Items.Add(row[0]);
            }
        }

        private DataSet retrieveItems(string procedure)
        {
            DataSet dataSet = sqlconnectorclass.sqlOperatorsSelect(procedure, sqlConnector);
            return dataSet;
        }

        private void addUser(object sender, RoutedEventArgs e)
        {
            bool matchFound = false;
            foreach(userCreds i in existingUsers.Items)
            {
                if(i.userName == addUsertextbox.Text)
                {
                    matchFound = true;
                    break;
                }
            }
            if (!matchFound) 
            {
                if (addUsertextbox.Text != "" || addUsertextbox.Text != null)
                {
                    password_confirmation_window passwordBox = new password_confirmation_window();
                    passwordBox.ShowDialog();
                    newUserPassword = passwordBox.password;

                    if(passwordBox.adminAccessComboBox.SelectedIndex == 0)
                    {
                        adminAccess = 1;
                    }
                    else if(passwordBox.adminAccessComboBox.SelectedIndex == 1)
                    {
                        adminAccess = 0;
                    }
                    else
                    {
                        adminAccess = -1;
                    }
                    
                    if (newUserPassword != null && newUserPassword != "")
                    {
                        if(adminAccess != -1)
                        {
                            alterUsers("add");
                            loadTables();
                            addUsertextbox.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Admin access was not selected. Please select \"Yes\" or \"No\".", "Error: invalid Admin Access", MessageBoxButton.OK);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Password was null! User not added.", "Invalid Password", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show("No Name found! Please type a valid Username.", "Add User", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show($"User:{addUsertextbox.Text} already exists! Please choose a different name.", "Add User", MessageBoxButton.OK);
            }
            
        }

        private void removeUser(object sender, RoutedEventArgs e)
        {
            if (removeUserTextbox.Text != "" || addUsertextbox.Text != null)
            {
                bool matchFound = false;
                foreach(userCreds i in existingUsers.Items)
                {
                    if(i.userName == removeUserTextbox.Text)
                    {
                        matchFound = true;
                        break;
                    }
                }
                if (matchFound) 
                {
                    var result = MessageBox.Show($"You are going to delete user: \"{removeUserTextbox.Text}\". Are you sure?", "Remove user", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        List<string> adminCheck = adminUserCheck();
                        if (adminCheck.Count == 1 && adminCheck[0].ToString() == removeUserTextbox.Text)
                        {
                            MessageBox.Show("Error: Cannot remove last Admin User!", "Error: Single Admin Detected", MessageBoxButton.OK);
                            removeUserTextbox.Text = "";
                        }
                        else
                        {
                            alterUsers("remove");
                            loadTables();
                            removeUserTextbox.Text = "";
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("User was not removed", "Remove user", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show($"User: \"{removeUserTextbox.Text}\" was not found. Please check spelling", "Remove user", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("No Name detected! Please type a valid Username.", "Remove User", MessageBoxButton.OK);
            }
        }

        private List<string> adminUserCheck()
        {
            List<string> result = new List<string>();
            foreach (userCreds i in existingUsers.Items)
            {
                if (i.adminAccess == 1)
                {
                    result.Add(i.userName);
                }
            }
            return result;
        }

        private void alterUsers(string procedureType)
        {
            if(sqlConnector.State != ConnectionState.Open)
            {
                
            }
            //@statementType = procedureType aka add/remove/change


            List<SqlParameter> sqlParameters = null;
            switch (procedureType)
            {
                case "add":
                    sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@statementType", SqlDbType.VarChar) { Value = procedureType },
                        new SqlParameter("@userName", SqlDbType.VarChar) { Value = addUsertextbox.Text },
                        new SqlParameter("@password", SqlDbType.VarChar) { Value = newUserPassword },
                        new SqlParameter("@adminAccess", SqlDbType.VarChar) { Value = adminAccess },
                    };
                    break;

                case "remove":
                    sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@statementType", SqlDbType.VarChar) { Value = procedureType },
                        new SqlParameter("@userName", SqlDbType.VarChar) { Value = removeUserTextbox.Text }
                    };
                    break;

                case "change":
                    sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@statementType", SqlDbType.VarChar) { Value = procedureType },
                        new SqlParameter("@userName", SqlDbType.VarChar) { Value = initialUsername.Text },
                        new SqlParameter("@newUserName", SqlDbType.VarChar) { Value = newUsername.Text }
                    };
                    break;
            }
            sqlconnectorclass.sqlOperatorsEdit("crudUsers", sqlConnector, sqlParameters);
        }

        private void changeUserName(object sender, RoutedEventArgs e)
        {
            bool matchFound = false;
            foreach (userCreds i in existingUsers.Items)
            {
                if (i.userName == initialUsername.Text)
                {
                    matchFound = true;
                    break;
                }
            }
            if (!matchFound)
            {
                MessageBox.Show("No User found! Please type a valid Username.", "Change User Name", MessageBoxButton.OK);
            }
            else
            {
                if(newUsername.Text == "" || newUsername.Text == null)
                {
                    MessageBox.Show($"No \"NEW USERNAME\" found!\nPlease Type in a new username for user {initialUsername.Text}.", "Chane User Name", MessageBoxButton.OK);
                }
                else
                {
                    var result = MessageBox.Show($"You are about to change User Name: {initialUsername.Text} to {newUsername.Text}.\nAre you sure?", "Change User Name", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        alterUsers("change");
                        loadTables();
                        initialUsername.Text = "";
                        newUsername.Text = "";
                    }
                }
                

            }
        }

        private void refreshGrids(object sender, RoutedEventArgs e)
        {
            loadTables();
        }

        private void userCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = -1;
            string actualSelectedUser = string.Empty;
            if (userCombobox.SelectedIndex < 0)
            {
                usertoDevicegrid.Items.Clear();
            }
            else
            {
                index = userCombobox.SelectedIndex;
                actualSelectedUser = currentUsers[index].ToString();
            }

            

            if (index >= 0)
            {
                userDatagridLoad(actualSelectedUser);
            }
        }

        private void userDatagridLoad(string actualSelectedUser)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = "select" },
                new SqlParameter("@user", SqlDbType.VarChar) { Value = actualSelectedUser }
            };
            DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("crudDevicetoUser", sqlConnector, sqlParameters);
            usertoDevicegrid.Items.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                usertoDevicegrid.Items.Add(row[0].ToString());
            }
            usertoDevicegrid.Items.Refresh();
        }

        private void addDeviceToUser_Click(object sender, RoutedEventArgs e)
        {
            string temp = deviceComboBox.Text;
             if(deviceComboBox.Text != null && deviceComboBox.Text != string.Empty && deviceComboBox.Text != "")
            {
                bool deviceAssigned = false;

                foreach (string i in usertoDevicegrid.Items)
                {
                    if (i == deviceComboBox.Text)
                    {
                        deviceAssigned = true;
                        break;
                    }
                }

                if (deviceAssigned == true)
                {
                    MessageBox.Show("Device is already assigned!", "Error: Device already assigned", MessageBoxButton.OK);
                }
                else
                {

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@statementType", SqlDbType.VarChar) { Value = "add" },
                    new SqlParameter("@device", SqlDbType.VarChar) { Value = deviceComboBox.Text },
                    new SqlParameter("@user", SqlDbType.VarChar) { Value = userCombobox.Text }
                };
                    sqlconnectorclass.sqlOperatorsEdit("crudDevicetoUser", sqlConnector, sqlParameters);
                    userDatagridLoad(userCombobox.Text);
                }
            }
            else
            {
                MessageBox.Show("Please select a valid device!", "Error: no device selected", MessageBoxButton.OK);
            }
        }

        private void removeDevicefromUser_Click(object sender, RoutedEventArgs e)
        {
            string deviceToRemove = (string)usertoDevicegrid.SelectedItem;
            if (deviceToRemove != null || deviceToRemove != "")
            {
                bool deviceAssigned = false;
                foreach (string i in usertoDevicegrid.Items)
                {
                    if (i == deviceToRemove)
                    {
                        deviceAssigned = true;
                        break;
                    }
                }
                if (deviceAssigned == true)
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@statementType", SqlDbType.VarChar) { Value = "remove" },
                        new SqlParameter("@device", SqlDbType.VarChar) { Value = deviceComboBox.Text },
                        new SqlParameter("@user", SqlDbType.VarChar) { Value = userCombobox.Text }
                    };
                    sqlconnectorclass.sqlOperatorsEdit("crudDevicetoUser", sqlConnector, sqlParameters);
                    userDatagridLoad(userCombobox.Text);
                }
                else
                {
                    MessageBox.Show("No device selected! Please select a device!", "Error: No Device selected", MessageBoxButton.OK);
                    
                }
            }
            else
            {
                MessageBox.Show("Device is not assigned to this user!", "Error: Device not found", MessageBoxButton.OK);
                usertoDevicegrid.SelectedItem = null;
            }
        }

        private void refreshDevices_Click(object sender, RoutedEventArgs e)
        {
            loadTables();
        }
    }

    public class userCreds
    {
        public string userName { get; set; } = string.Empty;
        public int adminAccess { get; set; }
    }
}
