using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Model
{
    public class AppointmentViewModel
    {

        public int Appointment_Id { get; set; }
        public int Doctor_Id { get; set; }
        public int Patient_Id { get; set; }
        public int Doctor_Schedule_Id { get; set; }
        public string Reason_For_Appointment { get; set; }
        public int Appointment_Status_Id { get; set; }
        public string Doctor_Comment { get; set; }

        //additional attributes
        public string Doctor_Name { get; set; }
        public string Patient_Name { get; set; }

        public string Schedule_Date { get; set; }

        public string Time_Start { get; set; }
        public string Time_end { get; set; }

        public string Appointment_Status { get; set; }
        public string Appointment_Day { get; set; }
        public string Appointment_Time { get; set; }



    }
}