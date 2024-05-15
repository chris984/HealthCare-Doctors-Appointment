using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Model
{
    //This is for Display Multiple Tables in Views
    public class ListofTables
    {
        public List<DoctorViewModel> ListofDoctors { get; set; }
        public List<Specialty> ListofSpecialties { get; set; }


        public List<PatientViewModel>ListofPatient { get; set; }
        public List<Patient_Status> ListofPatientMaritalStatus { get; set; }
        public List<Patient_Gender> ListofGender { get; set; }

        public List<Appointment_Status> ListofAppointmentStatus { get; set; }
        public List<AppointmentViewModel> ListofAppointment { get; set; }


        //Counting different table for dashboard
        //
        public int CountDoctorsModel { get; set; }

        public int CountPatientModel { get; set; }
        public int CountPendingRequest { get; set; }
        public int CountAppointmentTable { get; set; }
    }
}