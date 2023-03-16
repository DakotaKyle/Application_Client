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
        private String customerName;

        public AddAppointmentWindow(Customer customer)
        {
            InitializeComponent();
            NameTextBox.Text = customer.Name;
            customerName = NameTextBox.Text;
            customerId = customer.CustomerId;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId, userId;
            String appType, startString, endString;
            DateTime start, end, startTimeZone, endTimeZone;
            TimeZoneInfo timeZone;

            MySqlCommand getAppointmentId = new("SELECT appointmentId FROM appointment ORDER BY appointmentId Desc", connection);
            MySqlCommand getUserId = new("SELECT userId FROM user ORDER BY userId Desc", connection);

            string addApointment = "INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@customerId, @userId, 0, 0, 0, 0, @type, 0, @start, @end, 03/01/2023, 0, 0, 0)";

            if (AppointmentComboBox.SelectedItem is not null && StartDatePicker.SelectedDate is not null &&
                 StartTimeTextBox.Text.Length >= 1 && EndDatePicker.SelectedDate is not null &&
                 EndTimeTextBox.Text.Length >= 1)
            {
                appType = AppointmentComboBox.Text;
                startString = StartDatePicker.Text + " " + StartTimeTextBox.Text;
                endString = EndDatePicker.Text + " " + EndTimeTextBox.Text;

                if (DateTime.TryParse(startString, out DateTime startDateTime) &&
                    DateTime.TryParse(endString, out DateTime endDateTime))
                {
                    start = startDateTime;
                    end = endDateTime;

                    if ((start.Hour >= 8 && start.Hour <= 17) && (end.Hour >= 8 && end.Hour <= 17))
                    {
                        timeZone = TimeZoneInfo.Local;

                        startTimeZone = TimeZoneInfo.ConvertTimeFromUtc(start, timeZone);
                        endTimeZone = TimeZoneInfo.ConvertTimeFromUtc(end, timeZone);

                        appointmentList.validateTimes(start, end);

                        if (appointmentList.isTimeValid)
                        {
                            using (MySqlConnection con = new(connectionString))
                            {
                                try
                                {
                                    connection.Open();

                                    using (MySqlCommand addCommand = new(addApointment, connection))
                                    {
                                        userId = (int)getUserId.ExecuteScalar();

                                        addCommand.Parameters.Add("@customerId", MySqlDbType.Int32).Value = customerId;
                                        addCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                                        addCommand.Parameters.Add("@type", MySqlDbType.VarChar).Value = appType;
                                        addCommand.Parameters.Add("@start", MySqlDbType.DateTime).Value = start;
                                        addCommand.Parameters.Add("@end", MySqlDbType.DateTime).Value = end;
                                        addCommand.ExecuteNonQuery();

                                        appointmentId = (int)getAppointmentId.ExecuteScalar();
                                    }

                                    connection.Close();

                                    Appointment newAppointment = new(appointmentId, customerId, userId, customerName, appType, startTimeZone, endTimeZone);
                                    appointmentList.addAppointment(newAppointment);
                                    Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error: " + ex);
                                    connection.Dispose();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Appointment times cannot overlap.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a time between 8am and 5pm.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter a valid date or time to continue.");
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
