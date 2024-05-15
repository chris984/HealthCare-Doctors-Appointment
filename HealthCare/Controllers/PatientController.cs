using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthCare.Model;

namespace HealthCare.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Patient_Gender> ListGender = db.Patient_Gender.ToList();
            List<Patient_Status> ListStatus = db.Patient_Status.ToList();
            List<PatientTable> ListPatient = db.PatientTables.ToList();


            List<PatientViewModel> ListPatientVm = ListPatient.Where(x => x.User_Id == TempId.UserId && x.User_Table.Role_Id == 3).Select(x => new PatientViewModel
            {
                Patient_First_Name = x.Patient_Name,
                Patient_Date_of_Birth = x.Patient_Date_of_Birth,
                Patient_Gender_Type = x.Patient_Gender.Patient_gender_type,
                Patient_Address = x.Patient_Address,
                Patient_Phone_No = x.Patient_Phone_No,
                Patient_Marital_Status = x.Patient_Status.Patient_Status1,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password,
                User_Id = x.User_Table.User_Id,
                Patient_Id = x.Patient_Id




            }).ToList();


            ListofTables ListofTable = new ListofTables()
            {
                ListofGender = ListGender,
                ListofPatient = ListPatientVm,
                ListofPatientMaritalStatus = ListStatus


            };


            return View(ListofTable);
        }

        public ActionResult EditPatient(int User_Id)
        {


            HealthcareDBEntities db = new HealthcareDBEntities();

            List<PatientTable> ListPatient = db.PatientTables.ToList();
            List<Patient_Gender> ListofGender = db.Patient_Gender.ToList();
            List<Patient_Status> ListofStatus = db.Patient_Status.ToList();


            List<PatientViewModel> ListPatientVm = ListPatient.Where(x => x.User_Id == User_Id && x.User_Table.Role_Id == 3).Select(x => new PatientViewModel
            {

                Patient_First_Name = x.Patient_Name,
                Patient_Date_of_Birth = x.Patient_Date_of_Birth,
                Patient_Address = x.Patient_Address,
                Patient_Phone_No = x.Patient_Phone_No,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password,
                Patient_Gender_Type = x.Patient_Gender.Patient_gender_type,
                Patient_Gender_Type_Id = x.Patient_Gender.Patient_Gender_Type_Id,
                Patient_Marital_Status = x.Patient_Status.Patient_Status1,
                Patient_Marital_Status_Id = x.Patient_Status.Patient_Marital_Status_Id,
                Patient_Id = x.Patient_Id,
                User_Id = x.User_Table.User_Id



            }).ToList();


            ListofTables listoftable = new ListofTables()
            {

                ListofPatient = ListPatientVm,
                ListofGender = ListofGender,
                ListofPatientMaritalStatus = ListofStatus

            };





            return PartialView("_ModalPatientUserProfile", listoftable);



        }

        [HttpPost]
        public ActionResult SavePatient(PatientViewModel patienvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (patienvm.User_Id > 0)
            {
                PatientTable patientModel = db.PatientTables.SingleOrDefault(x => x.User_Id == patienvm.User_Id && x.User_Table.Role_Id == 3);
                patientModel.Patient_Name = patienvm.Patient_First_Name;
                patientModel.Patient_Date_of_Birth = patienvm.Patient_Date_of_Birth;
                patientModel.Patient_Gender_Type_Id = patienvm.Patient_Gender_Type_Id;
                patientModel.Patient_Marital_Status_Id = patienvm.Patient_Marital_Status_Id;
                patientModel.Patient_Address = patienvm.Patient_Address;
                patientModel.Patient_Phone_No = patienvm.Patient_Phone_No;
                patientModel.User_Table.Email = patienvm.Email;
                patientModel.User_Table.Password = patienvm.Password;
                patientModel.User_Id = patienvm.User_Id;
                db.SaveChanges();
                result = "true";

            }




            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_Appointment()
        {

            return View();

        }


        public ActionResult GetDoctorScheduleRecord()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<DoctorViewModel> List = db.Doctor_Schedule_Table.Select(x => new DoctorViewModel
            {

                Doctor_Schedule_Id = x.Doctor_Schedule_Id,
                Doctor_Id = x.Doctor_Table.Doctor_Id,
                Doctor_Name = x.Doctor_Table.Doctor_Name,
                Degree = x.Doctor_Table.Degree,
                Specialities = x.Doctor_Table.Specialities,
                Schedule_Date = x.Schedule_Date,
                Day = x.Day,
                Time_start = x.Time_start,
                Time_end = x.Time_end

            }).ToList();




            return Json(List, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDoctorScheduleId(int Doctor_Schedule_Id)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Doctor_Schedule_Table> Listofschedule = db.Doctor_Schedule_Table.ToList();
            List<PatientTable> ListPatient = db.PatientTables.ToList();




            List<DoctorViewModel> ListScheduleVm = Listofschedule.Where(x => x.Doctor_Schedule_Id == Doctor_Schedule_Id).Select(x => new DoctorViewModel
            {

                Schedule_Date = x.Schedule_Date,
                Day = x.Day,
                Time_start = x.Time_start,
                Time_end = x.Time_end,
                Doctor_Name = x.Doctor_Table.Doctor_Name,
                Doctor_Id = x.Doctor_Table.Doctor_Id,
                Doctor_Schedule_Id = x.Doctor_Schedule_Id




            }).ToList();





            List<PatientViewModel> ListPatientVm = ListPatient.Where(x => x.User_Id == TempId.UserId).Select(x => new PatientViewModel
            {
                Patient_First_Name = x.Patient_Name,

                Patient_Address = x.Patient_Address,
                Patient_Phone_No = x.Patient_Phone_No,
                User_Id = x.User_Table.User_Id,
                Patient_Id = x.Patient_Id

            }).ToList();


            ListofTables listofTables = new ListofTables()
            {

                ListofPatient = ListPatientVm,
                ListofDoctors = ListScheduleVm

            };


            return PartialView("_ModalPatientBookAppointment",listofTables);
        }


        [HttpPost]
        public ActionResult SavePatientAppointment(AppointmentViewModel AppVm)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";


            if (AppVm.Appointment_Id <= 0)
            {
                AppointmentTable appointmenttable = new AppointmentTable()
                {

                    Doctor_Id = AppVm.Doctor_Id,
                    Patient_Id = AppVm.Patient_Id,
                    Doctor_Schedule_Id = AppVm.Doctor_Schedule_Id,
                    Reason_For_Appointment = AppVm.Reason_For_Appointment,
                    Appointment_Time = AppVm.Time_Start + "AM - " + AppVm.Time_end + "PM",
                    Appointment_Status_Id = 2

                };
                db.AppointmentTables.Add(appointmenttable);
                db.SaveChanges();
                result = "true";
            }
           


            return Json(result,JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPatientAppointmentRecord()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<AppointmentViewModel> List = db.AppointmentTables.Where(x => x.Patient_Id == TempId.PatientId).Select(x => new AppointmentViewModel
            {
                Appointment_Id = x.Appointment_Id,
                Doctor_Name = x.Doctor_Table.Doctor_Name,
                Schedule_Date = x.Doctor_Schedule_Table.Schedule_Date,
                Appointment_Time = x.Doctor_Schedule_Table.Time_start + "AM - "+ x.Doctor_Schedule_Table.Time_end +"PM",
                Appointment_Day = x.Doctor_Schedule_Table.Day,
                Appointment_Status = x.Appointment_Status.Status 


            }).ToList();


            return Json(List, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CancelPatientAppointment(int Appointment_Id)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            bool result = false;

            AppointmentTable appointment = db.AppointmentTables.Where(x => x.Appointment_Id == Appointment_Id).SingleOrDefault();

            if (appointment != null)
            {
                db.AppointmentTables.Remove(appointment);
                db.SaveChanges();

                result = true;

            }
           



            return Json(result,JsonRequestBehavior.AllowGet);

        }



        public ActionResult My_Appointment()
        {
            return View();

        }


        public ActionResult Logout()
        {

            Session.Clear();
            Session.Abandon();



            return RedirectToAction("Login", "User");

        }
    }
}