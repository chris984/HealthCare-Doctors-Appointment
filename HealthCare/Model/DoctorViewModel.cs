using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Model
{
    public class DoctorViewModel
    {
        public int Doctor_Id { get; set; }
        public int User_Id { get; set; }
        public string Doctor_Name { get; set; }
        public string Degree { get; set; }
        public string Contact_No { get; set; }
        public string Date_of_Birth { get; set; }
        public string Address { get; set; }
        public string Specialities { get; set; }
        public string Img { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }



        /// this is ridiculous
        /// 


        public int Doctor_Schedule_Id { get; set; }
        public string Schedule_Date { get; set; }
        public string Day { get; set; }
        public string Time_start { get; set; }
        public string Time_end { get; set; }






    }
}