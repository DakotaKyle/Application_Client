using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Application_Client
{
    class AppointmentAlert : AppointmentList
    {
        public void checkTime()
        {
            try
            {
                while (LoginPage.isvalid)
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
                        {
                            MessageBox.Show("You have an appointment!");
                        }
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
