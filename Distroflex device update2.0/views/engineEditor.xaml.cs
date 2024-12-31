using MySqlConnector;
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
using System.Configuration;
using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;

namespace Distroflex_device_update2._0.views
{
    /// <summary>
    /// Interaction logic for serverEditor.xaml
    /// </summary>
    public partial class engineEditor : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);

        private sqlServerConnectionMethods sqlconnectorclass = new sqlServerConnectionMethods();

        private List<string> deviceTypes = new List<string>();
        public engineEditor()
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

            deviceTypes.Clear();
            deviceComboBox.Items.Clear();
            
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

        private void addNewEngine(object sender, RoutedEventArgs e)
        {
            string addEngine = textbox_addEngine.Text;
            if(deviceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Error please select a valid Device first", "Add new Engine", MessageBoxButton.OK);
            }
            else
            {
                if (addEngine == null)
                {
                    MessageBox.Show("Error: Please enter valid text to add to Engine!", "Add new Engine", MessageBoxButton.OK);
                }
                else
                {
                    bool doesItExist = checkIfExists(addEngine);
                    if (doesItExist/*aka true*/)
                    {
                        MessageBox.Show("Error: Engine already exists", "Add new Engine", MessageBoxButton.OK);
                    }
                    else
                    {
                        //commander = new MySqlCommand("INSERT INTO deviceid.engines(device, engine) VALUES (" + deviceComboBox.Text + "," + addEngine + ")");
                        var result = MessageBox.Show("You are about to add Engine:\"" + addEngine + "\" to Device:\"" + deviceComboBox.Text + "\" Are you sure?", "Add new engine", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            editEngineToDevice("add", addEngine); //add engine to link table between deviceToEngine
                            deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                        }
                        else
                        {
                            MessageBox.Show("Operation cannceled", "Add new Engine", MessageBoxButton.OK);
                        }
                    }
                }
            }
            
        }

        private void removeEngine(object sender, RoutedEventArgs e)
        {
            string removeEngine = textbox_removeEngine.Text;
            if (removeEngine == null)
            {
                MessageBox.Show("Error: Please enter valid text to add to Engine!", "Add new Engine", MessageBoxButton.OK);
            }
            else
            {
                bool doesItExist = checkIfExists(removeEngine);
                if (!doesItExist/*aka true*/)
                {
                    MessageBox.Show("Error: Engine DOES NOT exist", "Remove Engine", MessageBoxButton.OK);
                }
                else
                {
                    var result = MessageBox.Show("You are about to remove Engine:\"" + removeEngine + "\" from Device:\"" + deviceComboBox.Text + "\" Are you sure?", "Remove engine", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        editEngineToDevice("remove", removeEngine); //remove from link table first
                        deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                    }
                    else
                    {
                        MessageBox.Show("Operation cannceled", "Remove Engine", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void editExistingEngine(object sender, RoutedEventArgs e)
        {
            string oldEngineName = textbox_initialEngineName.Text;
            string newEngineName = textbox_newEngineName.Text;

            if(oldEngineName == null || newEngineName == null)
            {
                MessageBox.Show("Error: Please enter valid text into BOTH Current Engine Name textbox and/or new Engine Name textbox!", "edit Engine Name", MessageBoxButton.OK);
            }
            else
            {
                bool exists = checkIfExists(oldEngineName);
                if (exists)
                {
                    var result = MessageBox.Show("You are about to edit Engine:\"" + oldEngineName + "\" to read as " + newEngineName + "\" Are you sure?", "edit Engine", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        editEngineToDevice("change", oldEngineName, newEngineName); //just change name since link table is shows descriptions of engines based on device
                        deviceComboBox_SelectionChanged(sender, e as SelectionChangedEventArgs);
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {oldEngineName} does not exist for device {deviceComboBox.Text}!\nPlease check your spelling!", "Error: Engine not found", MessageBoxButton.OK);
                }
            }
        }

        private bool checkIfExists(string check)
        {
            bool doesItExist = false;
            for (int i = 0; i < existingEngine.Items.Count; i++)
            {
                if (existingEngine.Items[i].ToString().Contains(check))
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
                DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("selectEnginesfromDevice", sqlConnector, sqlParameters);

                existingEngine.Items.Clear();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    existingEngine.Items.Add(row[0].ToString());
                }
                existingEngine.Items.Refresh();
            }
            else
            {
                deviceComboBox.SelectedIndex = 0;
            }
        }
        
        private void editEngineToDevice(string param, string engine)
        {
            string device = deviceComboBox.Text;

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value =param },
                new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                new SqlParameter("@engine", SqlDbType.VarChar) { Value =engine }
            };
            sqlconnectorclass.sqlOperatorsEdit("crudEngineToDevice", sqlConnector, sqlParameters);
        }

        private void editEngineToDevice(string param, string engine, string newEngineName)
        {
            string device = deviceComboBox.Text;

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = param },
                new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                new SqlParameter("@engine", SqlDbType.VarChar) { Value = engine },
                new SqlParameter("@newEngineName", SqlDbType.VarChar) { Value = newEngineName }
            };
            sqlconnectorclass.sqlOperatorsEdit("crudEngineToDevice", sqlConnector, sqlParameters);
        }
    }
}
