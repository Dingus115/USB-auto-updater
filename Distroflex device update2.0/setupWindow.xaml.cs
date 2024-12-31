using Distroflex_device_update2._0.Properties;
using Microsoft.Data.SqlClient;
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
    /// Interaction logic for setupWindow.xaml
    /// </summary>
    public partial class setupWindow : Window
    {
        private static string sqlConnectionString = "";
        private SqlConnection sqlConnector = new SqlConnection("");
        private SqlCommand commandOperator = new SqlCommand();
        public setupWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnector.Open();
                if(sqlConnector.State == System.Data.ConnectionState.Open)
                {
                    bool auth = verifyKey();
                }
            }
            catch 
            {
                MessageBox.Show("Could not connect to authentication server.\nPlease contact your IT admin.", "Error: internet connectivity error.", MessageBoxButton.OK);
                this.Close(); 
            }
        }

        private bool verifyKey()
        {
            bool authResult = false;

            

            return authResult;
        }
    }
}
