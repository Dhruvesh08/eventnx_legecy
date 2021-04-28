using LinkedInDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Controllers
{
    public class MailTemplatesController : Controller
    {
        // GET: MailTemplate
        public ActionResult Register(RegisterCustomerModel model)
        {
            ViewBag.Subject = "Customer Registration";
            return View(model);
        }
    }
}