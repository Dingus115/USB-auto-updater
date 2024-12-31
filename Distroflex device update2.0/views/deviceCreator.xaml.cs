using MySqlConnector;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Data.SqlClient;
using Distroflex_device_update2._0.Properties;
using System.ComponentModel.Design;

namespace Distroflex_device_update2._0.views
{
    /// <summary>
    /// Interaction logic for deviceCreator.xaml
    /// </summary>
    public partial class deviceCreator : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        private sqlServerConnectionMethods sqlconnectorclass = new sqlServerConnectionMethods();
        public deviceCreator()
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
            List<string> devices = new List<string>();
            retrieveDevices("selectDevices", devices);
            existingDevices.Items.Clear();
            foreach(string i in devices)
            {
                existingDevices.Items.Add(i.ToString());
            }
            existingDevices.Items.Refresh();
        }
        

        private void refreshDevices(object sender, RoutedEventArgs e)
        {
            loadTables();
        }
        private void addDevice_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_addDevice.Text != "")
            {
                bool deviceExistsCheck = false;
                for (int i = 0; i < existingDevices.Items.Count; i++)
                {
                    if (existingDevices.Items[i].ToString().Contains(textbox_addDevice.Text.ToUpper()))
                    {
                        deviceExistsCheck = true;
                        break;
                    }
                }
                if (deviceExistsCheck)
                {
                    MessageBox.Show("Error: Device already exists");
                }
                else
                {
                    var result = MessageBox.Show("You are creating a new Device called \"" + textbox_addDevice.Text + "\". Are you sure?", "Create new Device", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            alterDB("crudDevices", "add", textbox_addDevice.Text, null);
                            loadTables();
                        }
                        catch
                        {
                            MessageBox.Show("There was an error communicating with the database");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Operation Canceled", "Create new Device", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: Please enter valid text");
            }
        }

        private void removeDevice(object sender, RoutedEventArgs e)
        {
            bool deviceExistsCheck = false;
            for (int i = 0; i < existingDevices.Items.Count; i++)
            {
                if (existingDevices.Items[i].ToString().Contains(textbox_removeDevice.Text))
                {
                    deviceExistsCheck = true;
                    break;
                }
            }
            if (deviceExistsCheck == false)
            {
                MessageBox.Show("Error: Could not find device called \"" + textbox_removeDevice.Text + "\"", "Remove Device",  MessageBoxButton.OK);
            }
            else
            {
                if(textbox_removeDevice.Text == "")
                {
                    MessageBox.Show("Please enter valid text!", "Remove Device", MessageBoxButton.OK);
                }
                else
                {
                    var result = MessageBox.Show("You are going to permanently delete Device \"" + textbox_removeDevice.Text + "\" \n\nThis will make any data associated with it impossible to see!\n(Data for files will persist. Please contact your DBA for questions or concerns)", "Remove Device", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            alterDB("crudDevices", "remove", textbox_removeDevice.Text, null);
                            loadTables();
                        }
                        catch
                        {
                            MessageBox.Show("There was an error communicating with the database");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Operation Canceled", "Remove Device", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void editDeviceName(object sender, RoutedEventArgs e)
        {
            bool deviceExistsCheck = false;
            for (int i = 0; i < existingDevices.Items.Count; i++)
            {
                if (existingDevices.Items[i].ToString().Contains(textbox_initialDeviceName.Text))
                {
                    deviceExistsCheck = true;
                    break;
                }
            }
            if (deviceExistsCheck == false)
            {
                MessageBox.Show("Error: Could not find device called \"" + textbox_initialDeviceName.Text + "\" Please check your spelling", "Change Device Name", MessageBoxButton.OK);
            }
            else
            {
                var result = MessageBox.Show("You are about to change device table name \"" + textbox_initialDeviceName.Text + "\" to \"" + textbox_newDeviceName.Text + "\" Are you sure?", "Change Device Name", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) 
                {
                    try
                    {
                        alterDB("crudDevices", "change", textbox_initialDeviceName.Text, textbox_newDeviceName.Text);
                        loadTables();
                    }
                    catch
                    {
                        MessageBox.Show("There was an error communicating with the database");
                    }
                }
                else
                {
                    MessageBox.Show("Operation Canceled", "Change Device Name", MessageBoxButton.OK);
                }
            }
        }

        private List<string> retrieveDevices(string procedure, List<string> List)
        {
            DataSet dataSet = sqlconnectorclass.sqlOperatorsSelect("selectDevices", sqlConnector);
            existingDevices.Items.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                List.Add((string)row[0]);
            }
            sqlConnector.Close();

            return List;
        }

        private void alterDB(string procedureType, string func, string input1, string input2)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = func }
            };

            switch (func)
            {
                case "add":
                    sqlParameters.Add(new SqlParameter("@addDevice", SqlDbType.VarChar) { Value = input1 });
                    break;

                case "remove":
                    sqlParameters.Add(new SqlParameter("@removeDevice", SqlDbType.VarChar) { Value = input1 });
                    break;

                case "change":
                    sqlParameters.Add(new SqlParameter("@initialDevice", SqlDbType.VarChar) { Value = input1 });
                    sqlParameters.Add(new SqlParameter("@changeDevice", SqlDbType.VarChar) { Value = input2 });
                    break;
            }
            sqlconnectorclass.sqlOperatorsEdit("crudDevices", sqlConnector,sqlParameters);
        }
    }
}
