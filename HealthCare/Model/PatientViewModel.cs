using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCare.Model
{
    public class PatientViewModel
    {
        public int Patient_Id { get; set; }
        public int User_Id { get; set; }
        public string Patient_First_Name { get; set; }
        public string Patient_Last_Name { get; set; }
        public string Patient_Date_of_Birth { get; set; }
        public int Patient_Gender_Type_Id { get; set; }
        public string Patient_Gender_Type { get; set; }
        public string Patient_Address { get; set; }
        public string Patient_Phone_No { get; set; }
        public int Patient_Marital_Status_Id { get; set; }
        public string Patient_Marital_Status{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }



        /// <summary>
        /// Additional
        /// </summary>
        public  List <Patient_Status> ListofStatus { get; set; }
        public List<Patient_Gender> ListofType { get; set; }





    }
}