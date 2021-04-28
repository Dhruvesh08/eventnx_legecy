using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Collections;
using LinkedInDemo.Models;
using System.Data.Entity;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminsettingsController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Editadminsettings");
        }
     
        public ActionResult Editadminsettings(int Id = 0)
        {
            var adminsetting = AdminService.GetAdminSetting();
            if (adminsetting == null)
            {
                return HttpNotFound();
            }
            Adminsettingmodel model = new Adminsettingmodel();
            model.Id = adminsetting.Id;
            model.Taxpercent = adminsetting.Taxpercent;
            model.Companyname = adminsetting.Companyname;
            model.Address1 = adminsetting.Address1;
            model.Adddress2 = adminsetting.Adddress2;
            model.Pincode = adminsetting.Pincode;
            model.State = adminsetting.State;
            model.City = adminsetting.City;
            model.Country = adminsetting.Country;
            model.Hostname = adminsetting.Hostname;
            model.Smtpusername = adminsetting.Smtpusername;
            model.Smtpassword = adminsetting.Smtpassword;
            model.portno = adminsetting.portno;
            model.Utm_Source = adminsetting.Utm_Source;
            model.Utm_Medium = adminsetting.Utm_Medium;
            model.Utm_Campaign = adminsetting.Utm_Campaign;
            model.Utm_Content = adminsetting.Utm_Content;
            model.Utm_Term = adminsetting.Utm_Term;
            ViewBag.Id = model.Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Editadminsettings(Adminsettingmodel model)
        {
            var adminsetting = AdminService.GetAdminSetting();

            adminsetting.Taxpercent = model.Taxpercent;
            adminsetting.Companyname = model.Companyname;
            adminsetting.Address1 = model.Address1;
            adminsetting.Adddress2 = model.Adddress2;
            adminsetting.Pincode = model.Pincode;
            adminsetting.State = model.State;
            adminsetting.City = model.City;
            adminsetting.Country = model.Country;
            adminsetting.Hostname = model.Hostname;
            adminsetting.Smtpusername = model.Smtpusername;
            adminsetting.Smtpassword = model.Smtpassword;
            adminsetting.portno = model.portno;
            adminsetting.Utm_Source = model.Utm_Source;
            adminsetting.Utm_Medium = model.Utm_Medium;
            adminsetting.Utm_Campaign = model.Utm_Campaign;
            adminsetting.Utm_Content = model.Utm_Content;
            adminsetting.Utm_Term = model.Utm_Term;
            AdminService.UpdateAdminsetting(adminsetting);
            ViewBag.Message = "Record Updated Successful!!";
            return View(model);
        }


    }
}