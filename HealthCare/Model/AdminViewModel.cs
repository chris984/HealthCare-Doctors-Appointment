using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Model
{
    public class AdminViewModel
    {
        public int Admin_Id { get; set; }
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Hospital_Name { get; set; }
        public string Hospital_Address { get; set; }
        public string Hospital_Contact_No { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}