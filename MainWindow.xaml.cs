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
        readonly LoginPage login = new();
        CustomerList customer = new();

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                while (LoginPage.isvalid == false)
                {
                    Hide();
                    login.ShowDialog();
                    customer.initCustomer();
                }
            }
            catch (InvalidOperationException)
            {
                Close();
            }
            if (LoginPage.isvalid == true)
            {
                Show();
                getCustomerData();
                getScheduleData();
            }
        }
        public void getCustomerData()
        {

            MySqlCommand customerData = new("SELECT customer.customerId, customer.customerName, address.phone, CONCAT(address.address, ', ', city.city, ', ', country.country, ' ', address.postalCode) AS address FROM customer JOIN address ON customer.addressId = address.addressId JOIN city ON address.cityId = city.cityId JOIN country ON city.countryId = country.countryId", connection);

            try
            {
                connection.Open();

                DataTable customerTable = new();
                customerTable.Load(customerData.ExecuteReader());

                connection.Close();

                CustomerRecordDataGrid.DataContext = customerTable;

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

        }

        private void ModifyCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            ModifyCustomerWindow modifyCustomer = new();
            modifyCustomer.ShowDialog();

        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            int customerPrimaryKey, addressPrimaryKey, countryPrimaryKey;

            //MySqlCommand getCustomerPrimaryKey = new("SELECT customerId FROM Customer WHERE customerId=@customerId", connection);
            //MySqlCommand getAddressPrimaryKey = new("SELECT addressId FROM address ORDER BY addressId Desc", connection);
            //MySqlCommand getCountryPrimaryKey = new("SELECT countryId FROM country ORDER BY countryId Desc", connection);
            

            String deleteCustomerName = "DELETE FROM customer WHERE customerId=@customerId";
            //String deleteCustomerAddress = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@address, 0, @cityId, @zipCode, @phone, 0, 0, 0, 0)";
            //String deleteCustomerCity = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@city, @countryId, 07/17/2023, 0, 0, 0)";
            //String deleteCustomerCountry = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@country, 07/17/2023, 0, 0, 0)";


            if (CustomerRecordDataGrid.SelectedItem != null)
            {
                MessageBoxResult messageBox = MessageBox.Show("Are you sure you want to detele this customer? This process cannot be undone.","",MessageBoxButton.YesNo);

                if (messageBox == MessageBoxResult.Yes)
                {
                    try
                    {

                        connection.Open();

                        using (MySqlCommand customerCommand = new(deleteCustomerName, connection))
                        {
                            //customerCommand.Parameters.Add("@customerId", MySqlDbType.Int32).Value =
                            //customerCommand.ExecuteNonQuery();
                        }

                        connection.Close();

                        Close();
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
