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
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Appointment(int appointmentId, int customerId, int userId, String customerName, String appointmentType, DateTime startTime, DateTime endTime)
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
