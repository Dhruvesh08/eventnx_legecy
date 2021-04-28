using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinessLayer;
using LinkedInDemo.Models;

namespace LinkedInDemo.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ContactUsModel model)
        {
            string subject = " New Inquiry ";
            string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
            string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
            string body = "<tr>";
            body = body + "<td style='text-align:left; padding: 0px 30px;'>";
            body = body + "<p>  <b>Name :</b> " + model.Name + " </p>";
            body = body + "<p>  <b>Company :</b> " + model.Company + " </p>";
            body = body + "<p> <b>Designation :</b> " + model.Designation + " </p>";
            body = body + "<p> <b>Phone :</b>" + model.Phone + " </p>";
            body = body + "<p> <b>Email :</b>" + model.Email + " </p>";
            body = body + "<p> <b>Message : </b>" + model.Message + " </p>";
            body = body + "<p>Thanks and Cheers,<br />";
            body = body + "The Entire EventNX team </p>";
            string mailbody = header + body + footer;
            SendMailService.SendMail("contact@eventnx.com,raufs@tacttree.com,nipulp@tacttree.com", subject, mailbody);
            return View(model);
        }
    }
}