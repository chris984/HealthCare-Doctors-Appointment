using HealthCare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthCare.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin



        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateAdminLogin(UserViewModel userViewModel)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            User_Table user = db.User_Table.SingleOrDefault(x => x.Email == userViewModel.Email && x.Password == userViewModel.Password);
            AdminTable admin = db.AdminTables.SingleOrDefault(x => x.User_Id == 1);



            var result = "Fail";



            if (user != null)
            {
                Session["UserId"] = user.User_Id;
                Session["AdminName"] = admin.Name;



                TempId.UserId = user.User_Id;
                Doctor_Table doctor = db.Doctor_Table.SingleOrDefault(x => x.User_Id == user.User_Id);


                if (doctor != null)
                {
                    TempId.DoctorId = doctor.Doctor_Id;
                    Session["SessionDoctorName"] = doctor.Doctor_Name;
                }



                if (user.Role_Id == 1)
                {
                    result = "Admin";

                }
                else if (user.Role_Id == 2)
                {
                    result = "Doctor";
                }

            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Dashboard()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            //Count Total of Patients and Doctors


            int CountDoctors = db.User_Table.Where(x => x.Role_Id == 2).Count();
            int CountPatient = db.User_Table.Where(x => x.Role_Id == 3).Count();
            int CountPending = db.AppointmentTables.Where(x => x.Appointment_Status_Id == 2).Count();
            int TotalAppointment = db.AppointmentTables.Count();


            ListofTables listofcount = new ListofTables()
            {
                CountDoctorsModel = CountDoctors,
                CountPatientModel = CountPatient,
                CountPendingRequest = CountPending,
                CountAppointmentTable = TotalAppointment
            };



            return View(listofcount);
        }

        public ActionResult Appointment_Details()
        {
            return View();

        }

        public ActionResult List_of_Patient()
        {
            return View();

        }
        public ActionResult Add_Patient()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Patient_Gender> listgender = db.Patient_Gender.ToList();
            List<Patient_Status> liststatus = db.Patient_Status.ToList();

            PatientViewModel listPt = new PatientViewModel()
            {

                ListofType = listgender,
                ListofStatus = liststatus

            };



            return View(listPt);

        }

        [HttpPost]
        public ActionResult Create_Patient(PatientViewModel ptvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            User_Table user = new User_Table()
            {

                Email = ptvm.Email,
                Password = ptvm.Password,
                User_Id = ptvm.User_Id,
                Role_Id = 3,

            };
            db.User_Table.Add(user);
            db.SaveChanges();

            int latestUserId = user.User_Id;


            PatientTable patient = new PatientTable()
            {

                Patient_Name = ptvm.Patient_First_Name + " " + ptvm.Patient_Last_Name,
                Patient_Date_of_Birth = ptvm.Patient_Date_of_Birth,
                Patient_Gender_Type_Id = ptvm.Patient_Gender_Type_Id,
                Patient_Phone_No = ptvm.Patient_Phone_No,
                Patient_Marital_Status_Id = ptvm.Patient_Marital_Status_Id,
                Patient_Address = ptvm.Patient_Address,
                User_Id = latestUserId,
                Patient_Id = ptvm.Patient_Id,


            };

            db.PatientTables.Add(patient);
            db.SaveChanges();


            return Json(data: new { message = "Saved Successfully", success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult List_of_Doctors()
        {
            return View();

        }


        public JsonResult GetDoctorRecord(DataTablesParam param)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<DoctorViewModel> List = db.Doctor_Table.Select(x => new DoctorViewModel
            {


                Doctor_Name = x.Doctor_Name,
                Doctor_Id = x.Doctor_Id,
                User_Id = x.User_Table.User_Id,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password,
                Degree = x.Degree,
                Specialities = x.Specialities,
                Contact_No = x.Contact_No,
                Date_of_Birth = x.Date_of_Birth,
                Address = x.Address



            }).ToList();

            return Json(new
            {

                aaData = List,
                sEcho = param.sEcho,
                iTotalDisplayRecords = List.Count(),
                iTotalRecords = List.Count()

            },

                JsonRequestBehavior.AllowGet);


        }


        public ActionResult Add_Doctors()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Specialty> list = db.Specialties.ToList();

            SpecialityViewModel Splist = new SpecialityViewModel();

            List<SpecialityViewModel> Spvmlist = list.Select(x => new SpecialityViewModel
            {


                Specialties = x.Specialties,
                Sp_Id = x.Sp_Id

            }).ToList();


            //ViewBag.GetList = new SelectList(list, "Sp_Id", "Specialties");

            //ViewBag.GetList = list;


            return View(Spvmlist);

        }

        [HttpPost]
        public ActionResult Create_Doctor(DoctorViewModel dvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            User_Table user = new User_Table()
            {
                Email = dvm.Email,
                Password = dvm.Password,
                User_Id = dvm.User_Id,
                Role_Id = 2,


            };
            db.User_Table.Add(user);
            db.SaveChanges();


            int latestUserId = user.User_Id;


            Doctor_Table doc = new Doctor_Table()
            {
                Doctor_Name = dvm.Doctor_Name,
                Degree = dvm.Degree,
                Specialities = dvm.Specialities,
                Contact_No = dvm.Contact_No,
                Date_of_Birth = dvm.Date_of_Birth,
                Address = dvm.Address,
                Img = null,
                User_Id = latestUserId,
                Doctor_Id = dvm.Doctor_Id
            };

            db.Doctor_Table.Add(doc);
            db.SaveChanges();

            return Json(data: new { message = "Added Successfully", success = true }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EditDoctor(int User_Id)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Doctor_Table> ListDoc = db.Doctor_Table.ToList();
            List<Specialty> ListofSpecial = db.Specialties.ToList();

            List<DoctorViewModel> ListDocVm = ListDoc.Where(x => x.User_Id == User_Id && x.User_Table.Role_Id == 2).Select(x => new DoctorViewModel
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





            return PartialView("_ModalDocPartial", listoftable);
        }

        [HttpPost]
        public ActionResult SaveDoctor(DoctorViewModel Dvm)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (Dvm.User_Id > 0)
            {
                Doctor_Table doc = db.Doctor_Table.SingleOrDefault(x => x.User_Id == Dvm.User_Id && x.User_Table.Role_Id == 2);
                doc.Doctor_Name = Dvm.Doctor_Name;
                doc.User_Table.Email = Dvm.Email;
                doc.User_Table.Password = Dvm.Password;
                doc.Degree = Dvm.Degree;
                doc.Specialities = Dvm.Specialities;
                doc.Contact_No = Dvm.Contact_No;
                doc.Date_of_Birth = Dvm.Date_of_Birth;
                doc.Address = Dvm.Address;
                doc.User_Id = Dvm.User_Id;
                db.SaveChanges();
                result = "true";

            }





            return Json(result, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult DeleteDoctor(int User_Id)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            bool result = false;

            Doctor_Table doc = db.Doctor_Table.Where(x => x.User_Id == User_Id && x.User_Table.Role_Id == 2).SingleOrDefault();
            User_Table user = db.User_Table.Where(x => x.User_Id == User_Id && x.Role_Id == 2).SingleOrDefault();
            int tempDoctorId = doc.Doctor_Id;
            //Doctor_Schedule_Table docschedule = db.Doctor_Schedule_Table.Where(x => x.Doctor_Id == doc.Doctor_Id).SingleOrDefault();
            //AppointmentTable appointmentTable = db.AppointmentTables.Where(x => x.Doctor_Id == tempDoctorId).SingleOrDefault();



            if (doc != null && user != null/* && docschedule != null && appointmentTable != null*/)
            {

                db.Doctor_Table.Remove(doc);
                db.User_Table.Remove(user);
                //db.Doctor_Schedule_Table.Remove(docschedule);
                //db.AppointmentTables.Remove(appointmentTable);
                db.SaveChanges();

                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add_Specialities()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            List<Specialty> splist = db.Specialties.ToList();

            List<SpecialityViewModel> spvmlist = splist.Select(x => new SpecialityViewModel
            {

                Sp_Id = x.Sp_Id,
                Specialties = x.Specialties,



            }).ToList();

            return View(spvmlist);

        }


        public ActionResult EditSpeciality(int Sp_Id)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();


            List<SpecialityViewModel> list = db.Specialties.Where(x => x.Sp_Id == Sp_Id).Select(x => new SpecialityViewModel
            { Sp_Id = x.Sp_Id, Specialties = x.Specialties }).ToList();


            return PartialView("_ModalSpPartial", list);

        }

        [HttpPost]
        public ActionResult SaveSp_Id(SpecialityViewModel spvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (spvm.Specialties != null)
            {


                Specialty sp = db.Specialties.SingleOrDefault(x => x.Sp_Id == spvm.Sp_Id);
                sp.Sp_Id = spvm.Sp_Id;
                sp.Specialties = spvm.Specialties;
                db.SaveChanges();
                result = "true";
            }
            else
            {

                result = "fail";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveSpeciality(SpecialityViewModel spvm)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (spvm.Specialties != null)
            {
                Specialty objsp = new Specialty()
                {
                    Sp_Id = spvm.Sp_Id,
                    Specialties = spvm.Specialties,
                };

                db.Specialties.Add(objsp);
                db.SaveChanges();

                result = "true";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteSpeciality(int Sp_Id)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            Specialty sp = db.Specialties.Where(x => x.Sp_Id == Sp_Id).SingleOrDefault();
            bool result = false;

            if (sp != null)
            {
                db.Specialties.Remove(sp);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }


        public ActionResult AdminProfile()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();


            //List<AdminViewModel> List = db.AdminTables.Where(x => x.Admin_Id == 1).Select(x => new AdminViewModel {

            //    Name = x.Name,
            //    Hospital_Address = x.Hospital_Address,
            //    Hospital_Contact_No = x.Hospital_Contact_No,
            //    Admin_Id = x.Admin_Id,
            //    User_Id = x.User_Id,
            //    Username = x.User_Table.Username,
            //    Password = x.User_Table.Password

            //}).ToList();

            AdminTable admin = db.AdminTables.SingleOrDefault(x => x.Admin_Id == 1);

            AdminViewModel Adminview = new AdminViewModel()
            {

                Name = admin.Name,
                Hospital_Name = admin.Hospital_Name,
                Hospital_Address = admin.Hospital_Address,
                Hospital_Contact_No = admin.Hospital_Contact_No,
                Password = admin.User_Table.Password,
                Email = admin.User_Table.Email,



            };

            return View(Adminview);

        }


        [HttpPost]
        public ActionResult SaveProfile(AdminViewModel model)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            AdminTable admin = db.AdminTables.SingleOrDefault(x => x.Admin_Id == 1);

            admin.Name = model.Name;
            admin.Hospital_Name = model.Hospital_Name;
            admin.Hospital_Address = model.Hospital_Address;
            admin.Hospital_Contact_No = model.Hospital_Contact_No;



            User_Table user = db.User_Table.SingleOrDefault(x => x.User_Id == 1);
            user.Email = model.Email;
            user.Password = model.Password;
            db.SaveChanges();
            Session["AdminName"] = admin.Name;
            result = "true";

            return Json(result, JsonRequestBehavior.AllowGet);




        }

        public ActionResult GetPatientScheduleAllRecord()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<AppointmentViewModel> List = db.AppointmentTables.Select(x => new AppointmentViewModel
            {
                Appointment_Id = x.Appointment_Id,
                Doctor_Name = x.Doctor_Table.Doctor_Name,
                Patient_Name = x.PatientTable.Patient_Name,
                Schedule_Date = x.Doctor_Schedule_Table.Schedule_Date,
                Appointment_Time = x.Doctor_Schedule_Table.Time_start + "AM - " + x.Doctor_Schedule_Table.Time_end + "PM",
                Appointment_Day = x.Doctor_Schedule_Table.Day,
                Appointment_Status = x.Appointment_Status.Status,
                Reason_For_Appointment = x.Reason_For_Appointment



            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatientRecord()
        {
            HealthcareDBEntities db = new HealthcareDBEntities();

            List<PatientViewModel> List = db.PatientTables.Select(x => new PatientViewModel
            {
                Patient_Id = x.Patient_Id,
                User_Id = x.User_Table.User_Id,
                Patient_First_Name = x.Patient_Name,
                Patient_Date_of_Birth = x.Patient_Date_of_Birth,
                Patient_Gender_Type = x.Patient_Gender.Patient_gender_type,
                Patient_Marital_Status = x.Patient_Status.Patient_Status1,
                Patient_Phone_No = x.Patient_Phone_No,
                Patient_Address = x.Patient_Address,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password



            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPatient(int User_Id)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            List<Patient_Status> ListofStatus = db.Patient_Status.ToList();
            List<Patient_Gender> ListofGender = db.Patient_Gender.ToList();

            List<PatientViewModel> Listpatientvm = db.PatientTables.Where(x => x.User_Id == User_Id && x.User_Table.Role_Id == 3).Select(x => new PatientViewModel
            {

                Patient_First_Name = x.Patient_Name,
                Patient_Date_of_Birth = x.Patient_Date_of_Birth,
                Patient_Address = x.Patient_Address,
                Patient_Phone_No = x.Patient_Phone_No,
                Patient_Gender_Type = x.Patient_Gender.Patient_gender_type,
                Patient_Marital_Status = x.Patient_Status.Patient_Status1,
                Patient_Marital_Status_Id = x.Patient_Status.Patient_Marital_Status_Id,
                Patient_Gender_Type_Id = x.Patient_Gender.Patient_Gender_Type_Id,
                Email = x.User_Table.Email,
                Password = x.User_Table.Password,
                User_Id = x.User_Table.User_Id,
                Patient_Id = x.Patient_Id



            }).ToList();


            ListofTables listofTables = new ListofTables()
            {
                ListofPatient = Listpatientvm,
                ListofGender = ListofGender,
                ListofPatientMaritalStatus = ListofStatus

            };





            return PartialView("_ModalPatientPartial", listofTables);


        }

        [HttpPost]
        public ActionResult SavePatient(PatientViewModel ptvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (ptvm.User_Id > 0)
            {

                PatientTable patient = db.PatientTables.SingleOrDefault(x => x.User_Id == ptvm.User_Id && x.User_Table.Role_Id == 3);
                patient.Patient_Name = ptvm.Patient_First_Name;
                patient.Patient_Date_of_Birth = ptvm.Patient_Date_of_Birth;
                patient.Patient_Gender_Type_Id = ptvm.Patient_Gender_Type_Id;
                patient.Patient_Address = ptvm.Patient_Address;
                patient.Patient_Phone_No = ptvm.Patient_Phone_No;
                patient.User_Table.Email = ptvm.Email;
                patient.User_Table.Password = ptvm.Password;
                patient.Patient_Marital_Status_Id = ptvm.Patient_Marital_Status_Id;
                patient.User_Id = ptvm.User_Id;
                db.SaveChanges();
                result = "true";


            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeletePatient(int User_Id)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            bool result = false;

            PatientTable patient = db.PatientTables.Where(x => x.User_Id == User_Id && x.User_Table.Role_Id == 3).SingleOrDefault();
            User_Table user = db.User_Table.Where(x => x.User_Id == User_Id && x.Role_Id == 3).SingleOrDefault();
            int tempPatientId = patient.Patient_Id;
            AppointmentTable appointmentTable = db.AppointmentTables.Where(x => x.Patient_Id == tempPatientId).SingleOrDefault();




            if (patient != null && user != null)
            {
                if (appointmentTable != null)
                {
                    db.PatientTables.Remove(patient);
                    db.User_Table.Remove(user);
                    db.AppointmentTables.Remove(appointmentTable);
                    db.SaveChanges();

                    result = true;
                }
                else
                {
                    db.PatientTables.Remove(patient);
                    db.User_Table.Remove(user);
                    db.SaveChanges();

                    result = true;
                }



            }




            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ViewAppointmentDetails(int Appointment_Id)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();


            List<AppointmentTable> Listofappointment = db.AppointmentTables.ToList();
            List<PatientTable> ListPatient = db.PatientTables.ToList();
            List<Appointment_Status> listofappointmentstatus = db.Appointment_Status.ToList();


            AppointmentTable appointmentTable = db.AppointmentTables.SingleOrDefault(x => x.Appointment_Id == Appointment_Id);



            List<AppointmentViewModel> appointmentViewModels = Listofappointment.Where(x => x.Appointment_Id == Appointment_Id).Select(x => new AppointmentViewModel
            {
                Schedule_Date = x.Doctor_Schedule_Table.Schedule_Date,
                Appointment_Day = x.Doctor_Schedule_Table.Day,
                Time_Start = x.Doctor_Schedule_Table.Time_start,
                Time_end = x.Doctor_Schedule_Table.Time_end,
                Doctor_Name = x.Doctor_Table.Doctor_Name,
                Appointment_Status = x.Appointment_Status.Status,
                Reason_For_Appointment = x.Reason_For_Appointment,
                Appointment_Status_Id = x.Appointment_Status.Appointment_Status_Id,
                Doctor_Id = x.Doctor_Table.Doctor_Id,
                Appointment_Id = x.Appointment_Id




            }).ToList();

            List<PatientViewModel> ListPatientVm = ListPatient.Where(x => x.Patient_Id == appointmentTable.Patient_Id).Select(x => new PatientViewModel
            {

                Patient_First_Name = x.Patient_Name,
                Patient_Phone_No = x.Patient_Phone_No,
                Patient_Address = x.Patient_Address,
                Patient_Id = x.Patient_Id

            }).ToList();


            ListofTables List = new ListofTables()
            {

                ListofPatient = ListPatientVm,
                ListofAppointment = appointmentViewModels,
                ListofAppointmentStatus = listofappointmentstatus

            };


            return PartialView("_ModalViewAppointmentAdmin", List);
        }


        [HttpPost]
        public ActionResult SaveViewAppointmentStatus(AppointmentViewModel Apvm)
        {

            HealthcareDBEntities db = new HealthcareDBEntities();
            string result = "fail";

            if (Apvm.Appointment_Id > 0)
            {

                AppointmentTable appointmentModel = db.AppointmentTables.SingleOrDefault(x => x.Appointment_Id == Apvm.Appointment_Id);
                appointmentModel.Appointment_Status_Id = Apvm.Appointment_Status_Id;
                db.SaveChanges();
                result = "true";

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();



            return RedirectToAction("Index", "Admin");

        }

        //public ActionResult DisplayPartial()
        //{
        //    HealthcareDBEntities db = new HealthcareDBEntities();

        //    AdminViewModel admin = (AdminViewModel)db.AdminTables.Where(x => x.Admin_Id == 1).Select(x => new AdminViewModel { Name = x.Name });


        //    ViewBag.Data = admin;


        //    return PartialView("_Display1");

        //}


    }
}