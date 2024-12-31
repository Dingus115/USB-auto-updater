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
    public partial class osEditor : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        private sqlServerConnectionMethods sqlconnectorclass = new sqlServerConnectionMethods();

        private List<string> deviceTypes = new List<string>();

        public osEditor()
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
                loadDevices();
            }
            catch
            {
                MessageBox.Show("There was an issue connecting to the database.\nPlease contact your IT admin", "Error");
            }
        }

        private void loadDevices()
        {
            DataSet dataSet = sqlconnectorclass.sqlOperatorsSelect("selectDevices", sqlConnector);

            deviceComboBox.Items.Clear();
            deviceTypes.Clear();
            deviceTypes.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                deviceComboBox.Items.Add((string)row[0]);
                deviceTypes.Add((string)row[0]);
            }
        }

        private void refreshDeviceList(object sender, RoutedEventArgs e)
        {
            loadDevices();
        }

        private void addNewOs(object sender, RoutedEventArgs e)
        {
            string addOs = textbox_addOs.Text;
            if (deviceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Error please select a valid Device first", "Add new OS", MessageBoxButton.OK);
            }
            else
            {
                if (addOs == null)
                {
                    MessageBox.Show("Error: Please enter valid text to add to OS!", "Add new OS", MessageBoxButton.OK);
                }
                else
                {
                    bool doesItExist = checkIfExists(addOs);
                    if (doesItExist/*aka true*/)
                    {
                        MessageBox.Show("Error: OS already exists", "Add new OS", MessageBoxButton.OK);
                    }
                    else
                    {
                        //commander = new MySqlCommand("INSERT INTO deviceid.engines(device, engine) VALUES (" + deviceComboBox.Text + "," + addEngine + ")");
                        var result = MessageBox.Show("You are about to add OS:\"" + addOs + "\" to Device:\"" + deviceComboBox.Text + "\" Are you sure?", "Add new OS", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            editOsToDevice("add", addOs); //add engine to link table between deviceToEngine
                            deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                        }
                        else
                        {
                            MessageBox.Show("Operation cannceled", "Add new OS", MessageBoxButton.OK);
                        }
                    }
                }
            }

        }

        private void removeOs(object sender, RoutedEventArgs e)
        {
            string removeos = textbox_removeOs.Text;
            if (removeos == null)
            {
                MessageBox.Show("Error: Please enter valid text to add to OS!", "Add new OS", MessageBoxButton.OK);
            }
            else
            {
                bool doesItExist = checkIfExists(removeos);
                if (!doesItExist/*aka true*/)
                {
                    MessageBox.Show("Error: OS DOES NOT exist", "Remove OS", MessageBoxButton.OK);
                }
                else
                {
                    var result = MessageBox.Show("You are about to remove OS:\"" + removeos + "\" from Device:\"" + deviceComboBox.Text + "\" Are you sure?", "Remove OS", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        editOsToDevice("remove", removeos); //remove from link table first
                        deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                    }
                    else
                    {
                        MessageBox.Show("Operation cannceled", "Remove OS", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void editExistingOs(object sender, RoutedEventArgs e)
        {
            string oldOsName = textbox_initialOsName.Text;
            string newOsName = textbox_newOsName.Text;

            if (oldOsName == null || newOsName == null)
            {
                MessageBox.Show("Error: Please enter valid text into EITHER Current Engine Name textbox or new Engine Name textbox!", "edit Engine Name", MessageBoxButton.OK);
            }
            else
            {
                var result = MessageBox.Show("You are about to edit Engine:\"" + oldOsName + "\" to read as " + newOsName + "\" Are you sure?", "edit Engine", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    editOsToDevice("change", oldOsName, newOsName); //just change name since link table is shows descriptions of engines based on device
                    deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                }
            }
        }

        private bool checkIfExists(string check)
        {
            bool doesItExist = false;
            for (int i = 0; i < existingOs.Items.Count; i++)
            {
                if (existingOs.Items[i].ToString().Contains(check))
                {
                    {
                        doesItExist = true; break;
                    }
                }
            }
            return doesItExist;
        }

        private void refreshdataGrid(object sender, RoutedEventArgs e)
        {
            deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
        }

        private void deviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index;
            if (deviceComboBox.SelectedIndex < 0)
            {
                index = 0;
            }
            else
            {
                index = deviceComboBox.SelectedIndex;
            }

            string actualSelectedDevice = deviceTypes[index].ToString();
            if (index >= 0)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@deviceName", SqlDbType.VarChar) { Value = actualSelectedDevice },
                };
                DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("selectOsfromDevice", sqlConnector, sqlParameters);
                
                existingOs.Items.Clear();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    existingOs.Items.Add(row[0].ToString());
                }
                existingOs.Items.Refresh();
                
            }
            else
            {
                deviceComboBox.SelectedIndex = 0;
            }
        }


        private void editOsToDevice(string param, string Os)
        {
            string device = deviceComboBox.Text;

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = param },
                new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                new SqlParameter("@Os", SqlDbType.VarChar) { Value = Os },
            };
            sqlconnectorclass.sqlOperatorsEdit("crudOsToDevice", sqlConnector, sqlParameters);
        }

        private void editOsToDevice(string param, string Os, string newOs)
        {
            string device = deviceComboBox.Text;

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = param },
                new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                new SqlParameter("@Os", SqlDbType.VarChar) { Value = Os },
                new SqlParameter("@newOsName", SqlDbType.VarChar) { Value = newOs }
            };
            sqlconnectorclass.sqlOperatorsEdit("crudOsToDevice", sqlConnector, sqlParameters);
        }
    }
}
