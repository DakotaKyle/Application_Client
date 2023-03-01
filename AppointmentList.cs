using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Application_Client
{
    class AppointmentList
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);

        public static BindingList<Appointment> Appointments = new();

        public void initAppointment()
        {
            int appId, customerId, userId;
            int i = 0;
            String customerName, appType, start, end;
            MySqlCommand appointmentData = new("SELECT appointment.appointmentId, appointment.customerId, appointment.userId, customer.customerName, appointment.type, appointment.start, appointment.end FROM customer JOIN appointment ON customer.customerId = appointment.customerId", connection);

            try
            {
                connection.Open();

                DataTable appointmentTable = new();
                appointmentTable.Load(appointmentData.ExecuteReader());

                foreach (DataRow row in appointmentTable.Rows)
                {
                    appId = (int)appointmentTable.Rows[i]["appointmentId"];
                    customerId = (int)appointmentTable.Rows[i]["customerId"];
                    userId = (int)appointmentTable.Rows[i]["userId"];
                    customerName = appointmentTable.Rows[i]["customerName"].ToString();
                    appType = appointmentTable.Rows[i]["type"].ToString();
                    start = appointmentTable.Rows[i]["start"].ToString();
                    end = appointmentTable.Rows[i]["end"].ToString();

                    Appointment initAppointments = new(appId, customerId, userId, customerName, appType, start, end);
                    addAppointment(initAppointments);

                    i++;
                }

                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public void addAppointment(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        public void removeAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
        }
    }
}
