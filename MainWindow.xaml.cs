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
        private AppointmentList appointment = new();
        private static readonly LoginPage login = new();

        public MainWindow()
        {
            InitializeComponent();
            CustomerRecordDataGrid.ItemsSource = CustomerList.Customers;
            AppointmentDataGrid.ItemsSource = AppointmentList.Appointments;

            try
            {
                while (LoginPage.isvalid == false)
                {
                    Hide();
                    login.ShowDialog();

                    if (LoginPage.isvalid == true)
                    {
                        customer.initCustomer();
                        appointment.initAppointment();
                        Show();
                    }
                }
            }
            catch (InvalidOperationException)
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
            if (CustomerRecordDataGrid.SelectedItem != null)
            {
                Customer thisCustomer = (Customer)CustomerRecordDataGrid.SelectedItem;
                ModifyCustomerWindow modifyCustomer = new(thisCustomer);
                modifyCustomer.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a customer to modify.");
            }
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
                        connection.Dispose();
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
            if (CustomerRecordDataGrid.SelectedItem != null)
            {
                Customer thisCustomer = (Customer)CustomerRecordDataGrid.SelectedItem;
                AddAppointmentWindow addAppointment = new(thisCustomer);
                addAppointment.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a customer to schedule an appointment.");
            }
        }

        private void ModifyAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

            ModifyAppointmentWindow modifyAppointment = new();
            modifyAppointment.ShowDialog();

        }

        private void DeleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            String deleteAppointment = "DELETE FROM appointment WHERE appointmentId=@appointmentId";

            if (AppointmentDataGrid.SelectedItem != null)
            {
                MessageBoxResult messageBox = MessageBox.Show("Are you sure you want to detele this customer? This process cannot be undone.", "", MessageBoxButton.YesNo);

                if (messageBox == MessageBoxResult.Yes)
                {
                    try
                    {
                        Appointment oldAppointment = (Appointment)AppointmentDataGrid.SelectedItem;
                        int appointmentId = oldAppointment.AppointmentId;

                        connection.Open();

                        using (MySqlCommand appointmentCommand = new(deleteAppointment, connection))
                        {
                            appointmentCommand.Parameters.Add("@appointmentId", MySqlDbType.Int32).Value = appointmentId;
                            appointmentCommand.ExecuteNonQuery();
                        }

                        connection.Close();

                        appointment.removeAppointment(oldAppointment);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex);
                        connection.Dispose();
                    }
                }
                else if (messageBox == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Select an appointment to delete.");
            }
        }

        private void WeekViewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MonthViewButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
