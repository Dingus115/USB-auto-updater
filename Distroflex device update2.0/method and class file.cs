using Distroflex_device_update_2._0;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Distroflex_device_update2._0.views.deviceUpdater;

namespace Distroflex_device_update2._0
{
    public class motorollaDevice : INotifyPropertyChanged
    {
        public CancellationTokenSource cancellation = new CancellationTokenSource();
        public string devicePath { get; set; } = string.Empty;
        public string devicePortNumber { get; set; } = string.Empty;

        private string connectionQueueUpdate { get; set; } = string.Empty;
        private string connectionStatusUpdate { get; set; } = string.Empty;
        private string deviceTypeUpdate { get; set; } = string.Empty;

        private bool updateStatusGuiUpdate { get; set; }
        private string fileCountUpdate { get; set; } = string.Empty;
        private string fileNameUpdate { get; set; } = string.Empty;
        private decimal fileProgressUpdate { get; set; } = 0;
        private string fileProgressTextUpdate { get; set; } = string.Empty;

        public IntPtr deviceHandle { get; set; } = IntPtr.Zero;
        public List<string> filesForSelectedDevice = new List<string>();
        public string selectedDeviceConfiguration = string.Empty;

        public string deviceTypeName { get; set; } = string.Empty;
        public string engineTypeName { get; set; } = string.Empty;
        public string osTypeName { get; set; } = string.Empty;


        public string connectionQueue
        {
            get => connectionQueueUpdate; //continuous get and set of progress for object 

            set
            {
                connectionQueueUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public string connectionStatus
        {
            get => connectionStatusUpdate; //continuous get and set of progress for object 

            set
            {
                connectionStatusUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public string deviceType
        {
            get => deviceTypeUpdate; //continuous get and set of progress for object 

            set
            {
                deviceTypeUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public bool updateStatus
        {
            get => updateStatusGuiUpdate; //continuous get and set of progress for object 

            set
            {
                updateStatusGuiUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public string fileCount
        {
            get => fileCountUpdate; //continuous get and set of progress for object 

            set
            {
                fileCountUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public string fileName
        {
            get => fileNameUpdate; //continuous get and set of progress for object 

            set
            {
                fileNameUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }

        public string fileProgressText
        {
            get => fileProgressTextUpdate; //continuous get and set of progress for object 

            set
            {
                fileProgressTextUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public decimal fileProgress
        {
            get => fileProgressUpdate; //continuous get and set of progress for object 

            set
            {
                fileProgressUpdate = value;
                OnPropertyChanged();//fire off notification of object that value has changed
            }
        }
        public bool singleFileCheck = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null/*name of property changed ie: progress*/)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));//notifies gui to update specific any areas of specific object that pertains to said value
        }

        public bool fileCheck(string path)
        {
            bool doesFileExist = false;
            if (File.Exists(path))
            {
                fileName = path;
                doesFileExist = true;
            }
            else
            {
                path.Replace("DISTRSERVER", "192.168.1.157");
            }
            return doesFileExist;
        }
    }

    public class singleFileUpdates
    {
        public string fileName { get; set; } = string.Empty;
    }
    public class userCredentials
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public bool? adminAccess { get; set; }
    }
    public class sqlServerConnectionMethods
    {
        private SqlCommand commandOperator = new SqlCommand();

        //without sql params method
        public DataSet sqlOperatorsSelect(string storedProcedureName, SqlConnection sqlConnection)
        {
            DataTable table = new DataTable();
            DataSet dataSet = new DataSet();

            commandOperator = new SqlCommand(storedProcedureName, sqlConnection);
            commandOperator.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            table.Load(commandOperator.ExecuteReader());
            sqlConnection.Close();
            dataSet.Tables.Add(table);

            return dataSet;
        }


        //with sql params
        public DataSet sqlOperatorsRetrieve(string storedProcedureName, SqlConnection sqlConnection, List<SqlParameter> parameters)
        {
            DataTable table = new DataTable();
            DataSet dataSet = new DataSet();

            commandOperator = new SqlCommand(storedProcedureName, sqlConnection);
            commandOperator.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter x in parameters)
            {
                commandOperator.Parameters.Add(x);
            }
            sqlConnection.Open();
            table.Load(commandOperator.ExecuteReader());
            sqlConnection.Close();
            dataSet.Tables.Add(table);

            return dataSet;
        }

        public void sqlOperatorsEdit(string storedProcedureName, SqlConnection sqlConnection, List<SqlParameter> parameters)
        {
            commandOperator = new SqlCommand(storedProcedureName, sqlConnection);
            commandOperator.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter x in parameters)
            {
                commandOperator.Parameters.Add(x);
            }
            sqlConnection.Open();
            commandOperator.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
