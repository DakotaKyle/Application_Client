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
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        private CustomerList customer = new();
        private static readonly LoginPage login = new();

        public MainWindow()
        {
            InitializeComponent();
            CustomerRecordDataGrid.ItemsSource = CustomerList.Customers;

            try
            {
                while (LoginPage.isvalid == false)
                {
                    Hide();
                    login.ShowDialog();
                    customer.initCustomer();
                    getScheduleData();

                    if (LoginPage.isvalid == true)
                    {
                        Show();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Close();
            }
            
        }

        public void getScheduleData()
        {

            MySqlCommand appointmentData = new("SELECT customer.customerName, appointment.type, appointment.start, appointment.end FROM customer JOIN appointment ON customer.customerId = appointment.customerId", connection);

            try
            {
                connection.Open();

                DataTable appointmentTable = new();
                appointmentTable.Load(appointmentData.ExecuteReader());

                connection.Close();

                AppointmentDataGrid.DataContext = appointmentTable;

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

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddCustomerWindow addCustomer = new();
            addCustomer.ShowDialog();
            Show();
        }

        private void ModifyCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            ModifyCustomerWindow modifyCustomer = new();
            modifyCustomer.ShowDialog();

        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            String deleteCustomerName = "DELETE FROM customer WHERE customerId=@customerId";
            String deleteCustomerAddress = "DELETE FROM address WHERE addressId=@addressId";
            String deleteCustomerCity = "DELETE FROM city WHERE cityId=@cityId";
            String deleteCustomerCountry = "DELETE FROM country WHERE countryId=@countryId";


            if (CustomerRecordDataGrid.SelectedItem != null)
            {
                MessageBoxResult messageBox = MessageBox.Show("Are you sure you want to detele this customer? This process cannot be undone.", "", MessageBoxButton.YesNo);

                if (messageBox == MessageBoxResult.Yes)
                {
                    try
                    {
                        Customer oldCustomer = (Customer)CustomerRecordDataGrid.SelectedItem;
                        int customerId = oldCustomer.CustomerId;
                        int addressId = oldCustomer.AddressId;
                        int cityId = oldCustomer.CityId;
                        int countryId = oldCustomer.CountryId;

                        connection.Open();

                        using (MySqlCommand customerCommand = new(deleteCustomerName, connection))
                        {
                            customerCommand.Parameters.Add("@customerId", MySqlDbType.Int32).Value = customerId;
                            customerCommand.ExecuteNonQuery();
                        }

                        using (MySqlCommand addressCommand = new(deleteCustomerAddress, connection))
                        {
                            addressCommand.Parameters.Add("@addressId", MySqlDbType.Int32).Value = addressId;
                            addressCommand.ExecuteNonQuery();
                        }
                        using (MySqlCommand cityCommand = new(deleteCustomerCity, connection))
                        {
                            cityCommand.Parameters.Add("@cityId", MySqlDbType.Int32).Value = cityId;
                            cityCommand.ExecuteNonQuery();
                        }
                        using (MySqlCommand countryCommand = new(deleteCustomerCountry, connection))
                        {
                            countryCommand.Parameters.Add("@countryId", MySqlDbType.Int32).Value = countryId;
                            countryCommand.ExecuteNonQuery();
                        }

                        connection.Close();

                        customer.removeCustomer(oldCustomer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else if (messageBox == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Select a customer to delete.");
            }
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
