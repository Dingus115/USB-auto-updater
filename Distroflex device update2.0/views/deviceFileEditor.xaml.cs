using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Distroflex_device_update2._0.views
{
    /// <summary>
    /// Interaction logic for deviceFileEditor.xaml
    /// </summary>
    public partial class deviceFileEditor : UserControl
    {

        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        private sqlServerConnectionMethods sqlconnectorclass = new sqlServerConnectionMethods();

        private List<filePathStructure> oldFiles = new List<filePathStructure>();

        private List<string> deviceTypes = new List<string>();
        private List<string> osTypes = new List<string>();
        private List<string> engineTypes = new List<string>();

        public deviceFileEditor()
        {
            InitializeComponent();
            serverCheck();
            foreach(DataGridColumn column in existingFiles.Columns)
            {
                column.CanUserSort = false;
                column.CanUserResize = false;
            }
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
                loadOS(actualSelectedDevice);
                loadEngine(actualSelectedDevice);
            }
            else
            {
                deviceComboBox.SelectedIndex = 0;
            }
        }

        private void loadOS(string device)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@deviceName", SqlDbType.VarChar) { Value = device },
            };
            DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("selectOsfromDevice", sqlConnector, sqlParameters);

            osCombobox.Items.Clear();
            osTypes.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                osCombobox.Items.Add(row[0].ToString());
                osTypes.Add((string)row[0]);
            }
            osCombobox.Items.Refresh();
        }

        private void loadEngine(string device)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@deviceName", SqlDbType.VarChar) { Value = device },
            };
            DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("selectEnginesfromDevice", sqlConnector, sqlParameters);

            engineCombobox.Items.Clear();
            engineTypes.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                engineCombobox.Items.Add(row[0].ToString());
                engineTypes.Add((string)row[0]);
            }
            engineCombobox.Items.Refresh();
            
        }

        private void scripts_Checked(object sender, RoutedEventArgs e)
        {
            advancedOptions.Visibility = Visibility.Visible;
        }

        private void scripts_Unchecked(object sender, RoutedEventArgs e)
        {
            int deviceScriptCheck = 0;
            foreach (filePathStructure x in existingFiles.Items)
            {
                if (x.restartAfter == 1)
                {
                    deviceScriptCheck++;
                    break;
                }
            }
            if (deviceScriptCheck == 0)
            {
                advancedOptions.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Error! There are files that need a restart after!\nPlease note that a script is required for that functionality!\n\nIf you don't want a script then delete all files\nwith a \"restart after\" functionality.", "ERROR", MessageBoxButton.OK);
                scripts.IsChecked = true;
            }
            
        }

        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            filePathStructure newItem = new filePathStructure();
            newItem.engineName = engineCombobox.Text;
            newItem.filePath = fileSelected_textbox.Text;
            newItem.fileName = System.IO.Path.GetFileName(fileSelected_textbox.Text);
            if (partitionNumber_Textbox.Text == "") { newItem.partition = "T"; }
            else { newItem.partition = partitionNumber_Textbox.Text; }
            newItem.restartAfter = Convert.ToInt16(restart.IsChecked);
            newItem.configFile = Convert.ToInt16(config.IsChecked);
            existingFiles.Items.Add(newItem);
            existingFiles.Items.Refresh();  
        }

        private void osCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectPossibleFiles("select");   
        }
        private void selectPossibleFiles(string statement)
        {
            int deviceIndex;
            if (deviceComboBox.SelectedIndex < 0)
            {
                deviceIndex = 0;
            }
            else
            {
                deviceIndex = deviceComboBox.SelectedIndex;
            }
            string actualSelectedDevice = deviceTypes[deviceIndex].ToString();

            int osIndex;
            if (osCombobox.SelectedIndex < 0)
            {
                osIndex = 0;
            }
            else
            {
                osIndex = osCombobox.SelectedIndex;
            }
            string actualSelectedOs = osTypes[osIndex].ToString();

            int engineIndex;
            string acutalSelectedEngine;
            if (engineCombobox.SelectedIndex < 0)
            {
                engineIndex = 0;
                acutalSelectedEngine = engineTypes[engineIndex].ToString();
                engineCombobox.SelectedIndex = engineIndex;
            }
            else
            {
                engineIndex = engineCombobox.SelectedIndex;
                acutalSelectedEngine = engineTypes[engineIndex].ToString();
            }

            List<SqlParameter> newFileSelection = new List<SqlParameter>()
            {
                new SqlParameter("@statementType", SqlDbType.VarChar) { Value = statement},
                new SqlParameter("@device", SqlDbType.VarChar) { Value = actualSelectedDevice},
                new SqlParameter("@os", SqlDbType.VarChar) { Value = actualSelectedOs},
                new SqlParameter("@engine", SqlDbType.VarChar) { Value = acutalSelectedEngine }
            };
            DataSet dataSet = sqlconnectorclass.sqlOperatorsRetrieve("crudFilesfromOs", sqlConnector, newFileSelection);

            int x = existingFiles.Items.Count;
            existingFiles.Items.Clear();
            x = existingFiles.Items.Count;
            oldFiles.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                filePathStructure file = new filePathStructure();
                file.filePath = (string)row[0];
                file.fileName = System.IO.Path.GetFileName((string)row[0]);
                file.partition = row[1].ToString();
                file.restartAfter = (int)row[2];

                if(file.restartAfter == 1)
                {
                    scripts.IsChecked = true;
                }
                file.configFile = (int)row[3];
                file.orderOfFiles = (int)row[4];

                oldFiles.Add(file);
                existingFiles.Items.Add(file);
            }
            x = existingFiles.Items.Count;
            existingFiles.Items.Refresh();
        }
        private void engineCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (engineCombobox.SelectedIndex == -1)
            {
            }
            else
            {
                selectPossibleFiles("select");
            }
            
        }

        private void fileSelectbutton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();

            if (result == true) {
                fileSelected_textbox.Text = openFileDialog.FileName;
            }
        }

        private void moveUp_Click(object sender, RoutedEventArgs e)
        {
            if (existingFiles.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a row to move first", "Error");
            }
            else if(existingFiles.SelectedIndex == 0)
            {
                MessageBox.Show("Item has reached top of grid", "Error");
            }
            else
            {
                filePathStructure temp = new filePathStructure();
                temp = (filePathStructure)existingFiles.SelectedItem;
                int index = existingFiles.SelectedIndex;

                existingFiles.Items.Insert(existingFiles.SelectedIndex - 1, temp);
                existingFiles.Items.RemoveAt(existingFiles.SelectedIndex);
                existingFiles.SelectedItem = temp;
            }
        }

        private void modeDown_Click(object sender, RoutedEventArgs e)
        {
            if (existingFiles.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a row to move first", "Error");
            }
            else if (existingFiles.SelectedIndex == existingFiles.Items.Count -1)
            {
                MessageBox.Show("Item has reached bottom of grid", "Error");
            }
            else
            {
                filePathStructure temp = new filePathStructure();
                temp = (filePathStructure)existingFiles.SelectedItem;
                int index = existingFiles.SelectedIndex;

                existingFiles.Items.Insert(existingFiles.SelectedIndex + 2, temp);
                existingFiles.Items.RemoveAt(existingFiles.SelectedIndex);
                existingFiles.SelectedItem = temp;
            }
        }

        private void deleteRecord_Click(object sender, RoutedEventArgs e)
        {
            if (existingFiles.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a row to Delete", "Error");
            }
            else
            {
                filePathStructure temp = new filePathStructure();
                temp = (filePathStructure)existingFiles.SelectedItem;

                string parsedFile = temp.filePath.Substring(temp.filePath.LastIndexOf('\\') + 1);
                var result = MessageBox.Show($"You are about to remove file \"" + parsedFile + "\" from this table.\nAre you sure?" , "Delete Record", MessageBoxButton.YesNo);
                
                if(result == MessageBoxResult.Yes)
                {
                    existingFiles.Items.RemoveAt(existingFiles.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("Operation Cancelled", "Delete Record");
                }
                
                
            }
        }

        private void deleteAll_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"You are about to remove ALL files from this table.\nAre you sure?", "Delete all Records", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                while(existingFiles.Items.Count >= 1)
                {
                    existingFiles.Items.RemoveAt(0);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled", "Delete all Records");
            }
            scripts.IsChecked = false;
        }

        private void folderSelectbutton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            bool? result = openFolderDialog.ShowDialog();

            if (result == true)
            {
                folderSelected_textbox.Text = openFolderDialog.FolderName;
            }
        }

        private void commitAllChanges_Click(object sender, RoutedEventArgs e)
        {
            scriptDeleter();
            if(scripts.IsChecked == true)
            {
                if(folderSelected_textbox.Text != "null")
                {
                    createScripts();
                    crudFiles();
                    selectPossibleFiles("select");
                    MessageBox.Show("Changes have been saved", "Commit changes", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Error! Please select a folder for\nscripts to be written to!", "No folder selected error", MessageBoxButton.OK);
                }
            }
            else
            {
                crudFiles();
                selectPossibleFiles("select");
                MessageBox.Show("Changes have been saved", "Commit changes", MessageBoxButton.OK);
            }
        }
        private void scriptDeleter()
        {
            List<string> filesToDelete = new List<string>();
            
            //collect files to delete
            foreach (filePathStructure x in oldFiles)
            {
                if (x.filePath.Contains(".mcf"))
                {
                    filesToDelete.Add(x.filePath);
                }
            }

            //dispose of file from datagrid and dispose from file path
            foreach (string file in filesToDelete)
            {
                foreach(filePathStructure x in existingFiles.Items)
                {
                    if(x.filePath == file)
                    {
                        existingFiles.Items.Remove(x);
                        break;
                    }
                }
                if (File.Exists($@"{file}"))
                {
                    File.Delete(file);
                }
            }
        }
        private void createScripts()
        {
            if(scripts.IsChecked == true && folderSelected_textbox.Text != "null")
            {
                List<List<string>> files = new List<List<string>>();
                List<string> individualFilelist = new List<string>();

                foreach (filePathStructure f in existingFiles.Items) //go down the total list of files
                {
                    individualFilelist.Add(f.partition);//add each file to indivual list
                    if (f.restartAfter == 1)//see if it needs a restart after
                    {
                        files.Add(individualFilelist);//add current list to total List of Lists
                        individualFilelist = new List<string>();//clear individual list for new small list of files till another restart after.
                    }
                }
                scriptWriter(files);
            }
            else
            {
                if(folderSelected_textbox.Text != "null")
                {
                    MessageBox.Show("Please select a folder/destination for scripts to be written to!", "Script writing", MessageBoxButton.OK);
                }
            }
            
        }

        private void scriptWriter(List<List<string>> files)
        {
            List<filePathStructure> scriptFiles = new List<filePathStructure>();
            for(int x  = 0; x < files.Count; x++)
            {
                string mcfFile = $"{folderSelected_textbox.Text}\\{deviceComboBox.Text}-{osCombobox.Text}-{engineCombobox.Text}-{x}.mcf";

                File.WriteAllText($"{mcfFile}", "verify none\n");
                for (int y = 0; y < files[x].Count; y++)
                {
                    if(files[x][y] != "T")
                    {
                        File.AppendAllText($"{mcfFile}", $"d u: p={files[x][y]}\n");
                    }
                    else
                    {
                        File.AppendAllText($"{mcfFile}", $"d u: p= |T\n");
                    }
                    
                }
                File.AppendAllText($"{mcfFile}", "reset c");

                filePathStructure temp = new filePathStructure();
                temp.filePath = mcfFile;
                scriptFiles.Add(temp);
            }
            fileOrderRewrite(scriptFiles);
        }

        private void fileOrderRewrite(List<filePathStructure> scriptFiles)
        {
            List<filePathStructure> finalList = new List<filePathStructure>();
            List<filePathStructure> existingFilesList = new List<filePathStructure>();

            foreach (filePathStructure file in existingFiles.Items)
            {
                existingFilesList.Add(file);
            }

            int counter = 0;
            if(scriptFiles.Count > 0)
            {
                finalList.Add(scriptFiles[counter]);
            }
            
            for (int x = 0; x < existingFilesList.Count; x++) {
                filePathStructure file = existingFilesList[x];
                finalList.Add(file);
                if (file.restartAfter == 1)
                {
                    counter++;
                    if(counter < scriptFiles.Count())
                    {
                        finalList.Add(scriptFiles[counter]);
                    }
                }
                
            }

            existingFiles.Items.Clear();
            foreach (filePathStructure file in finalList)
            {
                existingFiles.Items.Add(file);
            }
            existingFiles.Items.Refresh();

        }

        private void crudFiles()
        {
            string device = deviceComboBox.Text;
            string os = osCombobox.Text;
            string engine = engineCombobox.Text;

            //delete first
            List<List<SqlParameter>> deleteFiles= new List<List<SqlParameter>>();
            foreach (filePathStructure x in oldFiles)//collect List of files into mass list
            {
                //delete
                List<SqlParameter> deleteRecord = new List<SqlParameter>()
                {
                    new SqlParameter("@statementType", SqlDbType.VarChar) { Value = "delete" },
                    new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                    new SqlParameter("@engine", SqlDbType.VarChar) { Value = engine },
                    new SqlParameter("@os", SqlDbType.VarChar) { Value = os },
                    new SqlParameter("@filepath", SqlDbType.VarChar) { Value = x.filePath }
                };
                deleteFiles.Add(deleteRecord);
            }
            for (int x = 0; x < deleteFiles.Count; x++)//go through mass list and send records to be deleted
            {
                sqlconnectorclass.sqlOperatorsEdit("crudFilesfromOs", sqlConnector, deleteFiles[x]);
            }

            int count = 0;
            List<List<SqlParameter>> addFiles = new List<List<SqlParameter>>();
            foreach (filePathStructure x in existingFiles.Items)
            {
                //add
                List<SqlParameter> addRecord = new List<SqlParameter>()
                {
                    new SqlParameter("@statementType", SqlDbType.VarChar) { Value = "add" },
                    new SqlParameter("@device", SqlDbType.VarChar) { Value = device },
                    new SqlParameter("@engine", SqlDbType.VarChar) { Value = engine },
                    new SqlParameter("@os", SqlDbType.VarChar) { Value = os },
                    new SqlParameter("@orderOfFile", SqlDbType.VarChar) { Value = count },
                    new SqlParameter("@filepath", SqlDbType.VarChar) { Value = x.filePath },
                    new SqlParameter("@partition", SqlDbType.VarChar) { Value = x.partition },
                    new SqlParameter("@restartAfter", SqlDbType.Int) { Value = x.restartAfter },
                    new SqlParameter("@isConfig", SqlDbType.Int) { Value = x.configFile }
                };
                count++;
                addFiles.Add(addRecord);
            }
            for(int x = 0; x < addFiles.Count; x++)//go through mass list and send records to be added
            {
                sqlconnectorclass.sqlOperatorsEdit("crudFilesfromOs", sqlConnector, addFiles[x]);
            }
        }

        private void editScript_Click(object sender, RoutedEventArgs e)
        {
            filePathStructure filePath = new filePathStructure();
            filePath = (filePathStructure)existingFiles.SelectedItem;
            string fileToEdit = filePath.filePath;

            if(!fileToEdit.Contains(".mcf") || fileToEdit == "" || fileToEdit == null)
            {
                MessageBox.Show($"Error: Current file ({fileToEdit}) is not a valid format", "Error: Invalid format", MessageBoxButton.OK);
            }
            else
            {
                Process.Start("notepad.exe", fileToEdit);
            }
        }

        private void existingFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                filePathStructure filePath = (filePathStructure)existingFiles.SelectedItem;
                currentFileName.Text = filePath.fileName;
            }
            catch
            {
                currentFileName.Text = "";
            }
        }
    }

    class filePathStructure
    {
        public string filePath { get; set; } = string.Empty;
        public string fileName {  get; set; } = string.Empty;
        public string partition { get; set; } = "T";

        public int restartAfter { get; set; }
        public int configFile {  get; set; }

        public string engineName { get; set; } = string.Empty ;
        public int orderOfFiles {  get; set; }
    }
}
