using Distroflex_device_update_2._0;
using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using MySqlConnector;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Distroflex_device_update2._0.views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class deviceUpdater : UserControl
    {
        private static string sqlConnectionString = Settings.Default.sqlServerConnection;
        private SqlConnection sqlConnector = new SqlConnection(sqlConnectionString);
        //private SqlCommand commandOperator = new SqlCommand();
        private sqlServerConnectionMethods sqlconnectorclass;
        public singleFileUpdates singleFileUpdateObject = new singleFileUpdates();
        public deviceUpdater()
        {
            InitializeComponent();
            
            //disable visual of singleFileUpdate row
            if (Settings.Default.userIsAdmin == 0)
            {
                singleUpdateRow.Height = new GridLength(0.00);
            }

            //load component after user information loaded
            Loaded += componentLoaded;
        }
        

        private List<string> deviceTypes = new List<string>();
        BackgroundWorker? worker;

        //Constants for usb device detection
        private static Guid InterfaceClassGuid = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); //defines devices connected by usb interface
        private const int DIGCF_PRESENT = 0x02; //value that shows is usb device is present and connected
        private const int DIGCF_DEVICEINTERFACE = 0x10; //specifies device handle you are requesting aka what type of device
        

        private void componentLoaded(object sender, RoutedEventArgs e)//on window loading
        {
            serverCheck(); //check server
            StartDeviceDetection(); //start thread to check for devices
            newAndExistingMotorolaDeviceDetection(); //start filter to check for connecting and disconnecting devices
            loadComboBoxes();//load combo boxes based off of user
            disableAdjustments();//quick way to disable all adjustments of datagrid
        }

        //disable user adjustment of datagrid
        private void disableAdjustments()
        {
            foreach (DataGridColumn column in dataShowingCurrentDevices.Columns)
            {
                column.CanUserResize = false;
                column.CanUserSort = false;
                column.CanUserReorder = false;
                column.IsReadOnly = true;
            }
        }

        //check for local sql server connection
        private void serverCheck()//method to check connection to server
        {
            sqlConnector = new SqlConnection(sqlConnectionString); //check server connection (should be open as server is on local network and not online)
            if (sqlConnector.State == ConnectionState.Closed)
            {
                try
                {
                    sqlConnector.Open(); //open server
                    serverStatus.Content = "connected"; //show connection
                    sqlConnector.Close(); //close server to not keep networking going
                }
                catch
                {
                    string message = "Please check your network connection and click refresh"; //display message to user in seperate box is server cannot be reached
                    string title = "error";
                    MessageBox.Show(message, title);
                    serverStatus.Content = "not connected";
                }
            }
        }

        //load combo boxes based on user credentials
        private void loadComboBoxes()
        {
            try
            {
                sqlConnector.Open();
                sqlConnector.Close();

                userCreds user = new userCreds();
                user.userName = Settings.Default.User;

                List<SqlParameter> deviceParams = new List<SqlParameter>() { new SqlParameter("@userName", SqlDbType.VarChar) { Value = user.userName } };

                sqlconnectorclass = new sqlServerConnectionMethods();
                DataSet ds = sqlconnectorclass.sqlOperatorsRetrieve("selectDevicefromUsers", sqlConnector, deviceParams);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    deviceTypes.Add((string)row[0]);
                    device_combobox.Items.Add((string)row[0]);
                }
            }
            catch
            {
                MessageBox.Show("Error: Could not open database to fill device comboboxes", "Error: Unable to load devices", MessageBoxButton.OK);
            }
        }

        private void currentFileCount(motorollaDevice device, int currentFile, int totalFiles)
        {
            device.fileCount = currentFile + " of " + totalFiles + " completed";
        }

        private void removeFinishedDevice(motorollaDevice device)
        {
            Dispatcher.BeginInvoke(new Action(() => { 
                dataShowingCurrentDevices.Items.Remove(device);
                deviceConnections.Content = dataShowingCurrentDevices.Items.Count.ToString();
            }));
        }


        public void StartDeviceDetection()//start device detection through USB
        {
            ManagementEventWatcher watcher = new ManagementEventWatcher(); //Create watcher object
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_USBControllerDevice'"); //Querey for specific device standard
            watcher.EventArrived += USBEvents; //subscribe to events it should querey for
            watcher.Query = query; //Set Query
            watcher.Start();//start watch
        }

        private void USBEvents(object sender, EventArrivedEventArgs e)//detects when device is removed from computer
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string eventType = e.NewEvent.ClassPath.ClassName;

            //scanning for device arrival
            if (eventType.Equals("__InstanceCreationEvent"))
            {
                newAndExistingMotorolaDeviceDetection();
            }
            //scanning for device removal
            else if (eventType.Equals("__InstanceDeletionEvent"))
            {
                // USB device disconnected
                removingMotorolaDeviceDetection();
            }
        }

        private void newAndExistingMotorolaDeviceDetection()//find all usb devices connected to computer
        {
            string[] vidOrPid = {"vid_####", "pid_####" };
            List<string> devicePaths = GetUSBDevicePaths();//find and collect all usb devices connected to computer

            foreach (string singleDevicePath in devicePaths)//iterate through each device
            {
                if (singleDevicePath.Contains(vidOrPid[0]) && singleDevicePath.Contains(vidOrPid[1]))//find specific motorolla devices
                {
                    if (dataShowingCurrentDevices.Items.Count == 0)
                    {
                        //add to list of currently connected devices
                        Dispatcher.BeginInvoke(new Action(() => { dataShowingCurrentDevices.Items.Add(createDevice(singleDevicePath));
                                                                  deviceConnections.Content = dataShowingCurrentDevices.Items.Count.ToString();
                        }));
                    }
                    else
                    {
                        bool existingDevice = false;
                        foreach (motorollaDevice device in dataShowingCurrentDevices.Items)
                        {
                            if (device.devicePath == singleDevicePath)
                            {
                                existingDevice = true;
                            }
                        }

                        if (existingDevice != true)
                        {
                            Dispatcher.BeginInvoke(new Action(() => { dataShowingCurrentDevices.Items.Add(createDevice(singleDevicePath)); }));
                        }
                    }
                    Dispatcher.BeginInvoke(new Action(() => {
                        deviceConnections.Content = dataShowingCurrentDevices.Items.Count.ToString();
                    }));
                }
            }
        }

        private static motorollaDevice createDevice(string path)
        {
            motorollaDevice newDevice = new motorollaDevice();
            newDevice.devicePath = path;
            newDevice.devicePortNumber = path.Substring(0, path.IndexOf("#{"));
            newDevice.devicePortNumber = Convert.ToString(newDevice.devicePortNumber[newDevice.devicePortNumber.Length - 1]);
            newDevice.connectionStatus = "connected";
            newDevice.updateStatus = true;
            return newDevice;
        }

        private void removingMotorolaDeviceDetection()
        {
            //list of all existing objects
            List<string> devicePaths = GetUSBDevicePaths();//find and collect all usb devices connected to computer
            bool deviceExists = false;
            int deviceCount = 0;
            while (deviceExists == false && deviceCount < dataShowingCurrentDevices.Items.Count)
            {
                foreach (motorollaDevice device in dataShowingCurrentDevices.Items)
                {
                    if (!devicePaths.Contains(device.devicePath) && device.updateStatus == true)
                    {
                        Dispatcher.BeginInvoke(new Action(() => { dataShowingCurrentDevices.Items.Remove(device);
                                                                  deviceConnections.Content = dataShowingCurrentDevices.Items.Count.ToString();
                        }));
                        break;
                    }
                }
                deviceCount++;
                if (deviceCount == dataShowingCurrentDevices.Items.Count)
                {
                    deviceExists = true;
                }
            }
        }

        public static List<string> GetUSBDevicePaths()//Get list of all usb devices connected to computer
        {
            List<string> devicePaths = new List<string>(); //create list of device paths
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref InterfaceClassGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            /*Pointer deviceInfo   = Setup devices(ref InterfaceClassGuid == specifies that is connected to usb, 
             *                                     IntPtr.Zero == indicates no parent window controlling device aka no third aprty bullshit, 
             *                                     IntPtr.Zero == always set zero, 
             *                                     DIGCF_PRESENT | DIGCF_DEVICEINTERFACE == left to right if device is present THEN grab device handle aka path information)*/

            if (deviceInfoSet != IntPtr.Zero && deviceInfoSet.ToInt64() != -1)
            //If deviceInfoSet DOES NOT EQUAL 0 AND deviceInfoSet DOES NOT EQUAL -1 then run
            {
                SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();//create new deviceInterfaceData

                deviceInterfaceData.Size = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
                //set deviceInterfaceData size equal to the managed type in memory of a device

                for (uint i = 0; ; i++)
                {
                    if (!SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref InterfaceClassGuid, i, ref deviceInterfaceData))
                    {
                        break; // No more devices
                    }

                    uint requiredSize = 0;
                    SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, out requiredSize, IntPtr.Zero);

                    IntPtr detailDataBuffer = Marshal.AllocHGlobal((int)requiredSize);
                    // The cbSize field must be set correctly before calling SetupDiGetDeviceInterfaceDetail
                    // According to MSDN, for 64-bit applications, cbSize is 8; for 32-bit applications, it's 5.
                    Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 8) ? 8 : 5);

                    if (SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, detailDataBuffer, requiredSize, out requiredSize, IntPtr.Zero))
                    {
                        string? devicePath = Marshal.PtrToStringAuto((IntPtr)((long)detailDataBuffer + 4)); // Skip over the size field to get the device path
                        devicePaths.Add(devicePath);
                    }
                    Marshal.FreeHGlobal(detailDataBuffer);
                }

                SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
            return devicePaths;
        }

        /*DLL's for usb device finding and detection*/
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, uint MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, uint DeviceInterfaceDetailDataSize, out uint RequiredSize, IntPtr DeviceInfoData);

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVICE_INTERFACE_DATA
        {
            public int Size;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        /*DLL's for usb device data sending*/


        const uint GENERIC_READ = 0x80000000;
        const uint OPEN_EXISTING = 3;
        const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        const int initialDelay = 100; // Initial delay in milliseconds
        const int minDelay = 10; // Minimum delay in milliseconds
        const int delayDecrement = 10; // Delay decrement in milliseconds


        // Definitions and constants
        private const int LMEM_FIXED = 0x0000;
        public const uint SPDRP_DEVICEDESC = 0x00000000;
        public const uint SPDRP_CLASSGUID = 0x00000008;
        public const uint DIGCF_ALLCLASSES = 0x04;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;


        public const uint SPDRP_HARDWAREID = 0x00000001; // Hardware ID
        public const uint SPDRP_COMPATIBLEIDS = 0x00000002; // Compatible IDs
        public const uint SPDRP_SERVICE = 0x00000004; // Service
        public const uint SPDRP_CLASS = 0x00000007; // Device class name

        public const uint SPDRP_DRIVER = 0x00000009; // Driver key name
        public const uint SPDRP_CONFIGFLAGS = 0x0000000A; // Config flags
        public const uint SPDRP_MFG = 0x0000000B; // Manufacturer
        public const uint SPDRP_FRIENDLYNAME = 0x0000000C; // Friendly name
        public const uint SPDRP_LOCATION_INFORMATION = 0x0000000D; // Location information
        public const uint SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E; // PDO name
        public const uint SPDRP_CAPABILITIES = 0x0000000F; // Capabilities
        public const uint SPDRP_UI_NUMBER = 0x00000010; // UI number
        public const uint SPDRP_UPPERFILTERS = 0x00000011; // Upper filters
        public const uint SPDRP_LOWERFILTERS = 0x00000012; // Lower filters
        public const uint SPDRP_BUSTYPEGUID = 0x00000013; // Bus type GUID
        public const uint SPDRP_LEGACYBUSTYPE = 0x00000014; // Legacy bus type
        public const uint SPDRP_BUSNUMBER = 0x00000015; // Bus number
        public const uint SPDRP_ENUMERATOR_NAME = 0x00000016; // Enumerator name
        public const uint SPDRP_SECURITY = 0x00000017; // Security
        public const uint SPDRP_SECURITY_SDS = 0x00000018; // Security SDS
        public const uint SPDRP_DEVTYPE = 0x00000019; // Device type
        public const uint SPDRP_EXCLUSIVE = 0x0000001A; // Exclusive
        public const uint SPDRP_CHARACTERISTICS = 0x0000001B; // Characteristics
        public const uint SPDRP_ADDRESS = 0x0000001C; // Address
        public const uint SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D; // UI number desc format
        public const uint SPDRP_DEVICE_POWER_DATA = 0x0000001E; // Device power data
        public const uint SPDRP_REMOVAL_POLICY = 0x0000001F; // Removal policy
        public const uint SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020; // Removal policy HW default
        public const uint SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021; // Removal policy override
        public const uint SPDRP_INSTALL_STATE = 0x00000022; // Install state
        public const uint SPDRP_LOCATION_PATHS = 0x00000023; // Location paths
        public const uint SPDRP_BASE_CONTAINERID = 0x00000024; // Base Container ID



        public static IntPtr DeviceHandle { get; set; }

        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        

        public async void OpenDeviceHandle(motorollaDevice device)
        {
            // Constants for CreateFile
            const uint GENERIC_READ_WRITE = 0xC0000000;
            const uint OPEN_EXISTING = 3;
            const uint FILE_ATTRIBUTE_NORMAL = 0x80;

            // Open a handle to the device
            device.deviceHandle = DeviceManager.CreateFile(
                device.devicePath,
                GENERIC_READ_WRITE,
                0, // No sharing
                IntPtr.Zero, // No security attributes
                OPEN_EXISTING,
                FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero // No template file
            );

            

            //retrieve sql Files
            if (device.singleFileCheck!= true)
            {
                device.filesForSelectedDevice = deviceFiles(device);
            }

            //retrieve single file from box
            else
            {
                device.fileName = singleFileUpdateObject.fileName;
            }

            bool success = device.deviceHandle != IntPtr.Zero && device.deviceHandle != new IntPtr(-1);
            IntPtr fileHandle = 0;

            //windows retrieved a handle
            if (success && !device.cancellation.IsCancellationRequested)
            {
                //show no files found for device
                if(device.filesForSelectedDevice.Count == 0)
                {
                    MessageBox.Show($"Error: no files found for selected configuration of {device.selectedDeviceConfiguration}\n Please contact a Data Base Administrator.", "Error: No files found");
                    device.cancellation.Cancel();
                }
                //start process of sending found files to device
                else
                {
                    int fileCount = device.filesForSelectedDevice.Count;
                    int x = 0;
                    while (x < fileCount)
                    {
                        Thread.Sleep(500);
                        currentFileCount(device, x, fileCount);

                        //retrieve device handle
                        device.deviceHandle = DeviceManager.CreateFile(
                                            device.devicePath,
                                            GENERIC_READ_WRITE,
                                            0, // No sharing
                                            IntPtr.Zero, // No security attributes
                                            OPEN_EXISTING,
                                            FILE_ATTRIBUTE_NORMAL,
                                            IntPtr.Zero // No template file
                                        );

                        //check is cancelation was requested from errors
                        if (device.cancellation.IsCancellationRequested)
                        {
                            DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                            device.fileCount = "cancelling";
                            Thread.Sleep(1000);
                            removeFinishedDevice(device);
                            break;
                        }

                        //Make sure handle value is not null or -1
                        success = device.deviceHandle != IntPtr.Zero && device.deviceHandle != new IntPtr(-1); 

                        //if retrieved device handle then start process to send info to device
                        if (success)
                        {
                            //check and make sure files exist before attempting to send to device
                            bool fileCheck = false;
                            int fileCheckCounter = 0;
                            while(fileCheck == false && fileCheckCounter < 0)
                            {
                                if (device.fileCheck(device.filesForSelectedDevice[x]))
                                {
                                    fileCheck = true;
                                    break;
                                }
                                else
                                {
                                    device.fileCheck(device.filesForSelectedDevice[x]);
                                    fileCheckCounter++;
                                }
                            }

                            //present error if file cannot be found from ip or addressing and cancel device
                            if(fileCheckCounter > 0)
                            {
                                MessageBox.Show($"Error: File {device.filesForSelectedDevice[x]} could not be found!\nPlease contact a database Admin");
                                Thread.Sleep(1000);
                                device.cancellation.Cancel();
                                break;
                            }

                            //start clean up if device could not find file
                            if (device.cancellation.IsCancellationRequested)
                            {
                                DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                                device.fileCount = "cancelling";
                                Thread.Sleep(1000);
                                removeFinishedDevice(device);
                                break;
                            }

                            //if file is found then grab file handle from file
                            fileHandle = DeviceManager.CreateFile(
                                        device.filesForSelectedDevice[x],
                                        GENERIC_READ,
                                        0x00000001,
                                        IntPtr.Zero,
                                        OPEN_EXISTING,
                                        FILE_ATTRIBUTE_NORMAL,
                                        IntPtr.Zero);

                            int totalFileSize = 0;

                            //get file size for counting bytes sent
                            DeviceManager.GetFileSizeEx(fileHandle, out totalFileSize);

                            device.fileName = device.filesForSelectedDevice[x];
                            //send file to device
                            await Task.Run(() => ProcessSelectedFileAsync(fileHandle, totalFileSize, device));

                            //stop communication to file
                            DeviceManager.CloseHandle(fileHandle);

                            //show file was fully processed
                            x++;

                            //destroy device handle information in ram
                            if (device.cancellation.IsCancellationRequested)
                            {
                                DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                                device.fileCount = "cancelling";
                                Thread.Sleep(500);
                                removeFinishedDevice(device);
                                break;
                            }

                            //start loop to see when device disconnects then reconnects
                            if (x < fileCount)
                            {
                                //start looking for disconnect
                                while (device.deviceHandle != IntPtr.Zero && device.deviceHandle != new IntPtr(-1))
                                {
                                    device.connectionQueue = "disconnected";
                                    DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                                    Thread.Sleep(100);

                                    //attempt reconnectect to device
                                    device.deviceHandle = DeviceManager.CreateFile(
                                            device.devicePath,
                                            GENERIC_READ_WRITE,
                                            0, // No sharing
                                            IntPtr.Zero, // No security attributes
                                            OPEN_EXISTING,
                                            FILE_ATTRIBUTE_NORMAL,
                                            IntPtr.Zero // No template file
                                        );

                                    //once reconnection established then break
                                    if (device.deviceHandle == IntPtr.Zero || device.deviceHandle == new IntPtr(-1))
                                    {
                                        device.connectionQueue = "connected";
                                        break;
                                    }
                                }
                            }
                        }

                        //finish up device
                        if (x == fileCount)
                        {
                            DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                            device.fileCount = "finishing up";
                            Thread.Sleep(1500);
                            removeFinishedDevice(device);
                            break;
                        }
                    }
                }
                if (device.cancellation.IsCancellationRequested)
                {
                    DeviceManager.SetupDiDestroyDeviceInfoList(device.deviceHandle);
                    device.fileCount = "cancelling";
                    Thread.Sleep(1000);
                    removeFinishedDevice(device);
                }
            }
            
            //Could not retrieve device handle
            else
            {
                MessageBox.Show("Error: Failed to open device handle.", "Error", button: MessageBoxButton.OK);
                device.fileCount = "Error: Canceling";
                removeFinishedDevice(device);
            }

        }
        private void ProcessSelectedFileAsync(IntPtr fileHandle, int totalFileSize, motorollaDevice device)
        {

            if (fileHandle != new IntPtr(-1))
            {
                byte[] buffer = new byte[0x10000];
                uint bytesRead = 0;
                uint bytesWritten = 0;
                int bytesProcessed = 0;
                byte signal = buffer[0];
                decimal oldPercentage = 0;
                decimal newPercentage = 0;
                byte recieveByte = 0x01;
                byte sendBytes = 0x00;

                while (DeviceManager.ReadFile(device.deviceHandle, buffer, 1, out bytesRead, IntPtr.Zero))
                {
                    if (bytesRead > 0)
                    {
                        signal = buffer[0];
                        if (signal == recieveByte || signal == sendBytes)
                        {
                            if (device.cancellation.IsCancellationRequested)
                            {
                                device.updateStatus = true;
                                break;
                            }


                            //read byte buffer to file
                            if (!DeviceManager.ReadFile(fileHandle, buffer, (uint)buffer.Length, out bytesRead, IntPtr.Zero))
                            {
                                MessageBox.Show("Could not find file! Please contact your DB manager!\n");
                                break;
                            }


                            //write byte buffer to device
                            if (!DeviceManager.WriteFile(device.deviceHandle, buffer, bytesRead, out bytesWritten, IntPtr.Zero))
                            {
                                break;
                            }

                            if (bytesWritten < bytesRead)
                            {
                                MessageBox.Show("Incomplete write to the device.\n");
                                break;
                            }

                            if (bytesProcessed == 0)
                            {
                                device.fileProgress = 0;
                                device.fileProgressText = device.fileProgress.ToString() + "%";
                            }

                            bytesProcessed += (int)bytesWritten;

                            newPercentage = calculatePercentage(bytesProcessed, totalFileSize);
                            if (newPercentage > oldPercentage)
                            {
                                oldPercentage = newPercentage;
                                device.fileProgress = oldPercentage;
                                device.fileProgressText = device.fileProgress.ToString("#.##") + "%";
                            }

                            if (bytesProcessed == DeviceManager.GetFileSize(fileHandle, IntPtr.Zero))
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (device.cancellation.IsCancellationRequested)
                            {
                                device.updateStatus = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Failed to open the file.\nPlease check if file exists!");
            }
        }

        private List<string> deviceFiles(motorollaDevice device)
        {
            List<string> list = new List<string>();
            SqlConnection threadedSqlConnector = new SqlConnection(sqlConnectionString);

            List<SqlParameter> deviceParams = new List<SqlParameter>() 
            {
                new SqlParameter("@device", SqlDbType.VarChar) { Value = device.deviceTypeName },
                new SqlParameter("@engine", SqlDbType.VarChar) { Value = device.engineTypeName },
                new SqlParameter("@os", SqlDbType.VarChar) { Value = device.osTypeName }
            };

            sqlconnectorclass = new sqlServerConnectionMethods();
            DataSet ds = sqlconnectorclass.sqlOperatorsRetrieve("selectFilesFromDevice", threadedSqlConnector, deviceParams);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add((string)row[0]);
            }

            return list;
        }

        private static decimal calculatePercentage(int bytesProcessed, int totalBytes)
        {
            decimal percentage = ((decimal)bytesProcessed / (decimal)totalBytes) * 100;
            Debug.WriteLine((decimal)bytesProcessed / (decimal)totalBytes);
            return percentage;
        }

        private void updateDevice(object sender, RoutedEventArgs e)
        {
            motorollaDevice device = (motorollaDevice)dataShowingCurrentDevices.CurrentItem;
            device.cancellation = new CancellationTokenSource();
            device.updateStatus = false;
            device.deviceType = device_combobox.Text;
            device.selectedDeviceConfiguration = $"{device_combobox.Text} {engine_combobox.Text} {os_combobox.Text}";
            device.deviceTypeName = device_combobox.Text;
            device.engineTypeName = engine_combobox.Text;
            device.osTypeName = os_combobox.Text;

            if (singleFileUpdatesCheckbox.IsChecked == true)
            {
                device.filesForSelectedDevice.Add(individualeFileToSend.Text);
                device.singleFileCheck = true;
                
                engine_combobox.SelectedIndex = -1;
                os_combobox.SelectedIndex = -1;
            }
            else
            {
                if (device_combobox.Text != "" || engine_combobox.Text != "" || os_combobox.Text != "")
                {

                }
                else
                {
                    MessageBox.Show("Please select DEVICE, ENGINE, and OS before updating device");
                }
            }

            worker = new BackgroundWorker();
            worker.DoWork += sendFilesToDevice;
            worker.RunWorkerAsync(argument: device);
        }

        private void killWorker()
        {
            worker.Dispose();
        }
        private void cancelDevice(object sender, RoutedEventArgs e)
        {
            motorollaDevice device = (motorollaDevice)dataShowingCurrentDevices.CurrentItem;
            device.cancellation.Cancel();
        }

        private void sendFilesToDevice(object sender, DoWorkEventArgs e)
        {
            try
            {
                OpenDeviceHandle(e.Argument as motorollaDevice);
            }
            catch
            {
                MessageBox.Show("Please select DEVICE, ENGINE, and OS before updating", "Error", button: MessageBoxButton.OK);
                motorollaDevice? device = e.Argument as motorollaDevice;
                device.updateStatus = true;
            }
        }


        private void refreshDatabaseConnection(object sender, RoutedEventArgs e)
        {
            serverCheck();
            loadComboBoxes();
        }

        private void device_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int text = device_combobox.SelectedIndex;
            if(text == -1)
            {

            }
            else
            {
                string device = deviceTypes[text];
                sqlconnectorclass = new sqlServerConnectionMethods();

                //set engine combobox values
                List<SqlParameter> engineParams = new List<SqlParameter>(){ new SqlParameter("@deviceName", SqlDbType.VarChar) { Value = device } };
                DataSet ds = sqlconnectorclass.sqlOperatorsRetrieve("selectEnginesfromDevice", sqlConnector, engineParams);
                engine_combobox.ItemsSource = ds.Tables[0].DefaultView;
                engine_combobox.DisplayMemberPath = "engineName";

                //set os combobox values
                List<SqlParameter> osParams = new List<SqlParameter>(){ new SqlParameter("@deviceName", SqlDbType.VarChar) { Value = device } };
                DataSet ds2 = sqlconnectorclass.sqlOperatorsRetrieve("selectOsFromDevice", sqlConnector, osParams);
                os_combobox.ItemsSource = ds2.Tables[0].DefaultView;
                os_combobox.DisplayMemberPath = "osName";
                sqlConnector.Close();
            }
        }

        private void updateAllButton(object sender, RoutedEventArgs e)
        {
            if (device_combobox.Text != "" || engine_combobox.Text != "" || os_combobox.Text != "")
            {
                foreach (motorollaDevice device in dataShowingCurrentDevices.Items)
                {
                    if (device.updateStatus != false)
                    {
                        device.cancellation = new CancellationTokenSource();
                        device.updateStatus = false;
                        device.deviceType = device_combobox.Text;

                        worker = new BackgroundWorker();
                        worker.DoWork += sendFilesToDevice;
                        worker.RunWorkerAsync(argument: device);
                    }
                }
            }
            else 
            {
                MessageBox.Show("Please select DEVICE, ENGINE, and OS before updating device");   
            }
        }

        private void cancelAllButton(object sender, RoutedEventArgs e)
        {
            foreach (motorollaDevice device in dataShowingCurrentDevices.Items)
            {
                device.cancellation.Cancel();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadComboBoxes();
        }

        private void allowSingleFileUpdate(object sender, RoutedEventArgs e)
        {
            singleFileUpdateObject.fileName = individualeFileToSend.Text;
            if(singleFileUpdatesCheckbox.IsChecked == true)
            {
                singleFileUpdatesCheckbox.IsChecked = false;
                singleFileUpdatesCheckbox.Background = Brushes.Red;
                singleFileUpdateObject.fileName = string.Empty;
            }
            else
            {
                singleFileUpdatesCheckbox.IsChecked = true;
                singleFileUpdatesCheckbox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4caf50"));
                singleFileUpdatesCheckbox.Foreground = Brushes.White;
                singleFileUpdateObject.fileName = individualeFileToSend.Text;
            }
            
        }

        private void selectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                individualeFileToSend.Text = openFileDialog.FileName;
            }
        }

        public static class DeviceManager
        {
            // Define P/Invoke signatures
            [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

            [DllImport("setupapi.dll", SetLastError = true)]
            private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, uint MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

            [DllImport("setupapi.dll", SetLastError = true)]
            public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr LocalAlloc(int uFlags, uint uBytes);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr LocalFree(IntPtr hMem);

            [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);


            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ReadFile(
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToRead,
                out uint lpNumberOfBytesRead,
                IntPtr lpOverlapped);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteFile(
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToWrite,
                out uint lpNumberOfBytesWritten,
                IntPtr lpOverlapped);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetFileSizeEx(IntPtr hFile, out int lpFileSize);

            [DllImport("kernel32.dll")]
            public static extern void Sleep(uint dwMilliseconds);


            [DllImport("kernel32.dll", SetLastError = true)]
            static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

            [DllImport("setupapi.dll", SetLastError = true)]
            public static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, uint memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

            [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool SetupDiGetDeviceRegistryProperty(
                IntPtr deviceInfoSet,
                ref SP_DEVINFO_DATA deviceInfoData,
                uint property,
                out uint propertyRegDataType,
                IntPtr propertyBuffer,
                uint propertyBufferSize,
                out uint requiredSize);

            [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

            public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("36fc9e60-c465-11cf-8056-444553540000");
        }
    }
}