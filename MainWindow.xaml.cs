using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
        private AppointmentAlert alert = new();
        private static readonly LoginPage login = new();
        public static Thread mainThread { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            mainThread = Thread.CurrentThread;
            CustomerRecordDataGrid.ItemsSource = CustomerList.Customers;
            AppointmentDataGrid.ItemsSource = AppointmentList.Appointments;

            try
            {
                while (!LoginPage.isvalid)
                {
                    Hide();
                    login.ShowDialog();

                    if (LoginPage.isvalid)
                    {
                        customer.initCustomer();
                        appointment.initAppointment();

                        (new Thread(() => { alert.checkTime(); })).Start(); //Lambda Expression to check user time an alert user of upcoming appointments.
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
            if (CustomerRecordDataGrid.SelectedItem is not null)
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
            String deleteCustomerCity = "DELETE FROM city WHERE countryId=@countryId";
            String deleteCustomerCountry = "DELETE FROM country WHERE countryId=@countryId";


            if (CustomerRecordDataGrid.SelectedItem is not null)
            {
                try
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
                                cityCommand.Parameters.Add("@countryId", MySqlDbType.Int32).Value = countryId;
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
                            if (ex is MySqlException)
                            {
                                MessageBox.Show("Delete associated customer appointments and check database for invaild entries to continue.");
                            }
                            else
                            {
                                MessageBox.Show("Error: " + ex);
                            }
                            connection.Dispose();
                        }
                    }
                    else if (messageBox == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select a customer to delete.");
            }
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerRecordDataGrid.SelectedItem is not null)
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
            if (AppointmentDataGrid.SelectedItem is not null)
            {
                try
                {
                    Appointment thisAppointment = (Appointment)AppointmentDataGrid.SelectedItem;
                    ModifyAppointmentWindow modifyAppointment = new(thisAppointment);
                    modifyAppointment.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
            else
            {
                MessageBox.Show("Select an appointment to modify.");
            }
        }

        private void DeleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String deleteAppointment = "DELETE FROM appointment WHERE appointmentId=@appointmentId";

                if (AppointmentDataGrid.SelectedItem is not null)
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void AppointmentTypesByMonth_Click(object sender, RoutedEventArgs e)
        {
            string type = AppointmentTypeComboBox.Text;
            appointment.typesByMonth(type);
        }

        private void ConcultantSchedule_Click(object sender, RoutedEventArgs e)
        {
            appointment.yourWeeklySchedule();
            UserViewWindow userview = new();
            userview.ShowDialog();
        }

        private void TotalAppointments_Click(object sender, RoutedEventArgs e)
        {
            appointment.totalAppointments();
        }
    }
}
