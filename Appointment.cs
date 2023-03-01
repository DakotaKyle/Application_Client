using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Client
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public String CustomerName { get; set; }
        public String AppointmentType { get; set; }
        public String Start { get; set; }
        public String End { get; set; }

        public Appointment(int appointmentId, int customerId, int userId, String customerName, String appointmentType, String startTime, String endTime)
        {
            AppointmentId = appointmentId;
            CustomerId = customerId;
            UserId = userId;
            CustomerName = customerName;
            AppointmentType = appointmentType;
            Start = startTime;
            End = endTime;
        }
    }
}
