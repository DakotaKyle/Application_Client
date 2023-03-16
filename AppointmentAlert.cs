using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Application_Client
{
    class AppointmentAlert : AppointmentList
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);

        public void checkTime()
        {
            try
            {
                while (LoginPage.isvalid && MainWindow.mainThread.IsAlive)
                {
                    DateTime start, end, alertTime;
                    DateTime localTime = DateTime.Now;
                    int compareLocalTimes, compareStartTimes;

                    foreach (Appointment app in Appointments)
                    {
                        start = app.Start;
                        end = app.End;
                        alertTime = start.AddMinutes(-15);

                        compareLocalTimes = DateTime.Compare(localTime, alertTime);
                        compareStartTimes = DateTime.Compare(localTime, start);

                        if (compareLocalTimes >= 0 && compareStartTimes <= 0)
                            MessageBox.Show("You have an appointment!");
                        
                    }
                        Thread.Sleep(60000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }
}
