using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthCare.Model;

namespace HealthCare.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {

            HealthcareDBEntities db = new HealthcareDBEntities();

            AdminTable admin = db.AdminTables.SingleOrDefault(x => x.Admin_Id == 1 && x.User_Table.Role_Id == 1);

            Session["AdminId"] = admin.Admin_Id;
            Session["Hospitalname"] = admin.Hospital_Name.ToString();
            Session["HospitalAddress"] = admin.Hospital_Address.ToString();
            Session["HospitalContact"] = admin.Hospital_Contact_No.ToString();
            Session["HospitalEmail"] = admin.User_Table.Email.ToString();

            return View();
        }

      
        public ActionResult Department()
        {
            return View();
        }

        public ActionResult ListofDoctors()
        {
            return View();
        }



    }
}