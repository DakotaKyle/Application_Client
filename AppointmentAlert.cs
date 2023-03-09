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
            while (LoginPage.isvalid)
            {
                DateTime start;
                DateTime localTime = DateTime.Now;
                DateTime alertTime = localTime.AddMinutes(15);
                int compareTimes;

                foreach (Appointment app in Appointments)
                {
                    start = app.Start;

                    compareTimes = DateTime.Compare(start, alertTime);

                    if (compareTimes >= 0)
                    {
                        MessageBox.Show("You have an appointment coming up!");
                    }

                    Thread.Sleep(60000);
                }
            }
        }
    }
}
