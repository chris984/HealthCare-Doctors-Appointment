using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthCare.Model;

namespace HealthCare.Controllers
{
        

    public class UserController : Controller
    {
        public ActionResult Login()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();


            List<Patient_Status> patient_Status = db.Patient_Status.ToList();
            List<Patient_Gender> patient_Type = db.Patient_Gender.ToList();

            PatientViewModel list = new PatientViewModel
            {
                ListofStatus = patient_Status,
                ListofType = patient_Type
                

            };

            return View(list);
        }


        [HttpPost]
        public ActionResult RegisterUser(PatientViewModel ptvm)
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

            int LatestPatientId = user.User_Id;

            PatientTable patient = new PatientTable()
            {

                Patient_Name = ptvm.Patient_First_Name + " " + ptvm.Patient_Last_Name,
                Patient_Date_of_Birth = ptvm.Patient_Date_of_Birth,
                Patient_Gender_Type_Id = ptvm.Patient_Gender_Type_Id,
                Patient_Marital_Status_Id = ptvm.Patient_Marital_Status_Id,
                Patient_Phone_No = ptvm.Patient_Phone_No,
                Patient_Address = ptvm.Patient_Address,
                User_Id = LatestPatientId,
                Patient_Id = ptvm.Patient_Id
            };
            db.PatientTables.Add(patient);
            db.SaveChanges();


            return Json(data: new { message = "Successfully Register", success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidateLogin(PatientViewModel uservm)
        {
            HealthcareDBEntities db = new HealthcareDBEntities();
            User_Table user = db.User_Table.SingleOrDefault(x => x.Email == uservm.Email && x.Password == uservm.Password);
            var result = "Fail";

            if (user != null)
            {
                Session["UserId"] = user.User_Id;
                Session["Username"] = user.Email;


                TempId.UserId = user.User_Id;
                PatientTable patient = db.PatientTables.SingleOrDefault(x => x.User_Id == user.User_Id);

                if (patient != null)
                {
                    TempId.PatientId = patient.Patient_Id;
                    Session["SessionPatientName"] = patient.Patient_Name;
                }




                if (user.Role_Id == 3)
                {
                    result = "Patient";
                }



            }
   
            return Json(result,JsonRequestBehavior.AllowGet);
        }    
    }
}