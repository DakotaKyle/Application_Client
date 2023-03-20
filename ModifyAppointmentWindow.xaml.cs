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
using System.Windows.Shapes;

namespace Application_Client
{
    /// <summary>
    /// Interaction logic for ModifyAppointmentWindow.xaml
    /// </summary>
    public partial class ModifyAppointmentWindow : Window
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        AppointmentList appointments = new();
        Appointment oldAppointment;
        int appointmentId, customerId, userId;
        String customerName, appType, startString, endString;
        String userTimes = "SELECT start, end FROM appointment WHERE appointmentId=@appointmentId";
        DateTime start, end, startTimeZone, endTimeZone;
        TimeZoneInfo timeZone;

        public ModifyAppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            appointmentId = appointment.AppointmentId;
            customerId = appointment.CustomerId;
            userId = appointment.UserId;
            NameTextBox.Text = appointment.CustomerName;
            customerName = appointment.CustomerName;
            AppointmentComboBox.Text = appointment.AppointmentType;
            StartDatePicker.SelectedDate = appointment.Start;
            EndDatePicker.SelectedDate = appointment.End;

            DataTable times = new();
            String startTimeString, endTimeString;

            connection.Open();

            using (MySqlCommand selectTimes = new(userTimes, connection))
            {
                selectTimes.Parameters.Add("@appointmentId", MySqlDbType.Int32).Value = appointment.AppointmentId;
                times.Load(selectTimes.ExecuteReader());

                startTimeString = times.Rows[0]["start"].ToString();
                endTimeString = times.Rows[0]["end"].ToString();

                if (DateTime.TryParse(startTimeString, out DateTime startTimeZone) && DateTime.TryParse(endTimeString, out DateTime endTimeZone))
                {
                    StartTimeTextBox.Text = startTimeZone.ToShortTimeString();
                    EndTimeTextBox.Text = endTimeZone.ToShortTimeString();
                }
                else
                    MessageBox.Show("An unexpected error has occurred. Invalid start or end time was generated.");
            }

            connection.Close();

            oldAppointment = appointment;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            String modifyAppointment = "UPDATE appointment SET type=@type, start=@start, end=@end WHERE appointmentId=@appointmentId";

            if (AppointmentComboBox.SelectedItem != null && StartDatePicker.SelectedDate != null &&
                StartTimeTextBox.Text.Length >= 1 && EndDatePicker.SelectedDate != null &&
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

                        appointments.validateTimes(start, end);

                        if (appointments.isTimeValid)
                        {
                            using (MySqlConnection con = new(connectionString))
                            {
                                try
                                {
                                    connection.Open();

                                    using (MySqlCommand updateAppointmentCommand = new(modifyAppointment, connection))
                                    {
                                        updateAppointmentCommand.Parameters.Add("@type", MySqlDbType.VarChar).Value = appType;
                                        updateAppointmentCommand.Parameters.Add("@start", MySqlDbType.DateTime).Value = start;
                                        updateAppointmentCommand.Parameters.Add("@end", MySqlDbType.DateTime).Value = end;
                                        updateAppointmentCommand.Parameters.Add("@appointmentId", MySqlDbType.Int32).Value = appointmentId;
                                        updateAppointmentCommand.ExecuteNonQuery();
                                    }

                                    connection.Close();

                                    appointments.removeAppointment(oldAppointment);
                                    Appointment newAppointment = new(appointmentId, customerId, userId, customerName, appType, startTimeZone, endTimeZone);
                                    appointments.addAppointment(newAppointment);
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
                            MessageBox.Show("Appointment times cannot overlap.");
                    }
                    else
                        MessageBox.Show("Appointment times must be within business hours.");
                }
                else
                    MessageBox.Show("Enter a valid date or time to continue.");
            }
            else
                MessageBox.Show("All fields are required to continue.");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to cancel? Any unsaved progress will be lost!", "", MessageBoxButton.YesNo);

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
