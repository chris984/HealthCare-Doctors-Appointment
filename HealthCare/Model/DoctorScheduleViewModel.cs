using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Model
{
    public class DoctorScheduleViewModel
    {

        public int Doctor_Schedule_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string Schedule_Date { get; set; }
        public string Day { get; set; }
        public string Time_start { get; set; }
        public string Time_end { get; set; }


    




    }
}