using MySql.Data.MySqlClient;
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

namespace Application_Client
{
    /// <summary>
    /// Interaction logic for AddAppointmentWindow.xaml
    /// </summary>
    public partial class AddAppointmentWindow : Window
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        private AppointmentList appointmentList = new();
        private int customerId;

        public AddAppointmentWindow(Customer customer)
        {
            InitializeComponent();
            NameTextBox.Text = customer.Name;
            customerId = customer.CustomerId;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId, userId;
            String appType;
            String startString, endString;
            DateTime start, end;

            MySqlCommand getAppointmentId = new("SELECT appointmentId FROM appointment ORDER BY appointmentId Desc", connection);
            MySqlCommand getUserId = new("SELECT userId FROM appointment ORDER BY userId Desc", connection);

            string addApointment = "INSERT INTO appointment (title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (0, 0, 0, 0, @type, 0, @start, @end, 03/01/2023, 0, 0, 0)";

            if (AppointmentComboBox.SelectedItem != null && StartDatePicker.SelectedDate != null &&
                StartTimeTextBox.Text.Length >= 1 && EndDatePicker.SelectedDate != null &&
                EndTimeTextBox.Text.Length >= 1)
            {
                appType = AppointmentComboBox.SelectedItem.ToString();
                startString = StartDatePicker.Text + " " + StartTimeTextBox.Text;
                endString = EndDatePicker.Text + " " + EndTimeTextBox.Text;

                if (DateTime.TryParse(startString, out DateTime startDateTime) &&
                    DateTime.TryParse(endString, out DateTime endDateTime))
                {
                    start = startDateTime;
                    end = endDateTime;
                }
                else
                {
                    MessageBox.Show("Enter a valid time to continue");
                }
            }
            else
            {
                MessageBox.Show("All fields are required.");
            } 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to cancel? Any unsaved progress will be lost!","",MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Close();
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                return;
            }

        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime startTime = StartDatePicker.SelectedDate.Value.ToUniversalTime();
            StartTimeTextBox.Text = startTime.ToShortTimeString();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime endTime = EndDatePicker.SelectedDate.Value.ToUniversalTime();
            EndTimeTextBox.Text = endTime.ToShortTimeString();
        }
    }
}
