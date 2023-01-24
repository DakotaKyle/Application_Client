using MySql.Data.MySqlClient;
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

namespace Application_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            Hide();
            LoginPage login = new();
            login.ShowDialog();

            if (login.isvalid == true)
            {
                String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
                MySqlConnection connection = new(connectionString);
                MySqlCommand cmd = new("SELECT * FROM customer", connection);

                try
                {                
                    connection.Open();
                    DataTable dt = new();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();

					CustomerRecordDataGrid.DataContext = dt;

                    Show();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
				finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }

            }
            else
            {
                Close();
            }
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            AddCustomerWindow addCustomer = new();
            addCustomer.ShowDialog();

        }

        private void ModifyCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            ModifyCustomerWindow modifyCustomer = new();
            modifyCustomer.ShowDialog();

        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

            AddAppointmentWindow addAppointment = new();
            addAppointment.ShowDialog();

        }

        private void ModifyAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

            ModifyAppointmentWindow modifyAppointment = new();
            modifyAppointment.ShowDialog();

        }

        private void DeleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WeekViewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MonthViewButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
