﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
        public static BindingList<Appointment> UserWeeklyView = new();
        public static BindingList<Appointment> UserMonthlyView = new();

        public static ArrayList customerIDs = new() { };
        public bool isTimeValid = true;

        public void initAppointment()
        {
            int appId, customerId, userId;
            int i = 0;
            String customerName, appType;
            DateTime start, end;
            MySqlCommand appointmentData = new("SELECT appointment.appointmentId, appointment.customerId, appointment.userId, customer.customerName, appointment.type, appointment.start, appointment.end FROM customer JOIN appointment ON customer.customerId = appointment.customerId", connection);

            try
            {
                connection.Open();

                DataTable appointmentTable = new();
                appointmentTable.Load(appointmentData.ExecuteReader());

                connection.Close();

                foreach (DataRow row in appointmentTable.Rows)
                {
                    appId = (int)appointmentTable.Rows[i]["appointmentId"];
                    customerId = (int)appointmentTable.Rows[i]["customerId"];
                    userId = (int)appointmentTable.Rows[i]["userId"];
                    customerName = appointmentTable.Rows[i]["customerName"].ToString();
                    appType = appointmentTable.Rows[i]["type"].ToString();
                    start = (DateTime)appointmentTable.Rows[i]["start"];
                    end = (DateTime)appointmentTable.Rows[i]["end"];

                    Appointment newAppointment = new(appId, customerId, userId, customerName, appType, start.ToLocalTime(), end.ToLocalTime());
                    addAppointment(newAppointment);

                    i++;
                }
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

        public void validateTimes(DateTime start, DateTime end)//startA and endA
        {
            int compareStartBWithEndA, compareStartAWithEndB;
            DateTime startSlots, endSlots;//startB and endB

            foreach (Appointment time in Appointments)
            {
                startSlots = time.Start;
                endSlots = time.End;

                compareStartBWithEndA = DateTime.Compare(startSlots, end); //compares start B with end A.
                compareStartAWithEndB = DateTime.Compare(start, endSlots); //compares start A with end B.

                if ((compareStartAWithEndB < 0) && (compareStartBWithEndA < 0))
                {
                    isTimeValid = false;
                    return;
                }
            }
        }

        public void validateTimes(DateTime start, DateTime end, int appointmentId)
        {
            int compareStartBWithEndA, compareStartAWithEndB;
            DateTime startSlots, endSlots;//startB and endB

            foreach (Appointment time in Appointments)
            {
                startSlots = time.Start;
                endSlots = time.End;

                compareStartBWithEndA = DateTime.Compare(startSlots, end); //compares start B with end A.
                compareStartAWithEndB = DateTime.Compare(start, endSlots); //compares start A with end B.

                if ((compareStartAWithEndB < 0) && (compareStartBWithEndA < 0) && (time.AppointmentId != appointmentId))
                {
                    isTimeValid = false;
                    return;
                }
            }
        }

        public void typesByMonth(String type)
        {
            DateTime localTime = DateTime.Now;
            String appType = type;
            int month = localTime.Month;
            int count = 0;

            foreach (Appointment app in Appointments)
            {
                if (app.Start.Month == month)
                {
                    if (appType == app.AppointmentType)
                    {
                        count++;
                    }
                }
            }
            MessageBox.Show("You have " + count + " " + appType + " appointments this month.");
        }

        public void yourWeeklySchedule()
        {
            int compareWeekStart, compareWeekEnd, compareMonthStart, compareMonthEnd;
            string name, type;
            DateTime start, end;

            int daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

            DateTime sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime saturday = sunday.AddDays(6);

            DateTime startOfMonth = DateTime.Today.AddDays(-(int)DateTime.Today.Day + 1);
            DateTime endOfMonth = startOfMonth.AddDays(daysInMonth - 1);

            foreach (Appointment app in Appointments)
            {
                if (app.UserId == LoginPage.UserID)
                {
                    name = app.CustomerName;
                    type = app.AppointmentType;
                    start = app.Start.ToLocalTime();
                    end = app.End.ToLocalTime();

                    compareWeekStart = DateTime.Compare(start, sunday);
                    compareWeekEnd = DateTime.Compare(end, saturday);

                    if ((compareWeekStart >= 0) && (compareWeekEnd <= 0))
                        {
                            Appointment newView = new(LoginPage.UserID, name, type, start, end);
                            addWeeklyView(newView);
                        }

                        compareMonthStart = DateTime.Compare(start, startOfMonth);
                        compareMonthEnd = DateTime.Compare(end, endOfMonth);

                        if ((compareMonthStart >= 0) && (compareMonthEnd <= 0))
                        {
                            Appointment newView = new(LoginPage.UserID, name, type, start, end);
                            addMonthlyView(newView);
                        }
                }
            }
        }

        public void totalAppointments()
        {
            int count = Appointments.Count;
            
            MessageBox.Show("There are " + count + " appointments.");
        }

        public void addAppointment(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        public void removeAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
        }

        public void addWeeklyView(Appointment appointment)
        {
            UserWeeklyView.Add(appointment);
        }
        public void addMonthlyView(Appointment appointment)
        {
            UserMonthlyView.Add(appointment);
        }
    }
}
