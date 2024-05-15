using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HealthCare.Model
{
    public class UserViewModel
    {
        public int User_Id { get; set; }

        public string AuthenticName { get; set; }


        [Required(ErrorMessage = "Email is required*")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required*")]
        public string Password { get; set; }
        public int Role_Id { get; set; }


    }
}