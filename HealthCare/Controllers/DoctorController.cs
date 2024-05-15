using HealthCare.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthCare.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Appointment()
        {

            return View();
        }

        public ActionResult View_Profile()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Doctor_Table> ListDoc = db.Doctor_Table.ToList();
            List<Specialty> ListofSpecial = db.Specialties.ToList();

            List<DoctorViewModel> ListDocVm = ListDoc.Where(x => x.User_Id == TempId.UserId && x.User_Table.Role_Id == 2).Select(x => new DoctorViewModel
            {
                Doctor_Name = x.Doctor_Name,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password,
                Degree = x.Degree,
                Specialities = x.Specialities,
                Contact_No = x.Contact_No,
                Date_of_Birth = x.Date_of_Birth,
                Address = x.Address,
                Img = x.Img,
                User_Id = x.User_Table.User_Id,
                Doctor_Id = x.Doctor_Id


            }).ToList();



            ListofTables listoftable = new ListofTables()
            {

                ListofDoctors = ListDocVm,
                ListofSpecialties = ListofSpecial

            };


            return View(listoftable);
       
        }



        public ActionResult GetDoctorRecord()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<DoctorScheduleViewModel> List = db.Doctor_Schedule_Table.Where(x => x.Doctor_Id == TempId.DoctorId).Select(x => new DoctorScheduleViewModel { Schedule_Date = x.Schedule_Date, Day=x.Day, Time_start = x.Time_start, Time_end = x.Time_end}).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetDoctorAppointmentAllRecord()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<AppointmentViewModel> List = db.AppointmentTables.Where(x => x.Doctor_Id == TempId.DoctorId).Select(x => new AppointmentViewModel
            {

                Patient_Name = x.PatientTable.Patient_Name,
                Schedule_Date = x.Doctor_Schedule_Table.Schedule_Date,
                Appointment_Time = x.Doctor_Schedule_Table.Time_start +"AM - " + x.Doctor_Schedule_Table.Time_end +"PM",
                Appointment_Day = x.Doctor_Schedule_Table.Day,
                Appointment_Status = x.Appointment_Status.Status,
                Reason_For_Appointment = x.Reason_For_Appointment,
                Appointment_Id = x.Appointment_Id

            }).ToList();

            return Json(List,JsonRequestBehavior.AllowGet);

        }


        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();



            return RedirectToAction("Index", "Admin");


        
        
        }

        [HttpPost]
        public ActionResult UpdateDoctor(DoctorViewModel docvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (docvm.User_Id > 0)
            {
                Doctor_Table doc = db.Doctor_Table.SingleOrDefault(x => x.User_Id == docvm.User_Id && x.User_Table.Role_Id == 2);
                doc.Doctor_Name = docvm.Doctor_Name;
                doc.User_Table.Email = docvm.Email;
                doc.User_Table.Password = docvm.Password;
                doc.Degree = docvm.Degree;
                doc.Specialities = docvm.Specialities;
                doc.Contact_No = docvm.Contact_No;
                doc.Date_of_Birth = docvm.Date_of_Birth;
                doc.Address = docvm.Address;
                doc.User_Id = docvm.User_Id;
                db.SaveChanges();
                result = "true";
                Session["SessionDoctorName"] = doc.Doctor_Name;

            }



            return Json(result,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddDoctorSchedule(DoctorScheduleViewModel docsched)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";
           


            if (docsched != null)
            {

                Doctor_Schedule_Table docschedmodel = new Doctor_Schedule_Table();


                docschedmodel.Doctor_Schedule_Id = docsched.Doctor_Schedule_Id;
                    docschedmodel.Doctor_Id = TempId.DoctorId;
                docschedmodel.Schedule_Date = docsched.Schedule_Date;

                string dateString = docsched.Schedule_Date;
                DateTime dateValue;
                dateValue = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                docschedmodel.Day = dateValue.ToString("dddd");

                docschedmodel.Time_start = docsched.Time_start;
                docschedmodel.Time_end = docsched.Time_end;
                db.Doctor_Schedule_Table.Add(docschedmodel);
                db.SaveChanges();
                result = "true";

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }



    }
}