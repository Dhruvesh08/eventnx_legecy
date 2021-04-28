using BussinessLayer;
using DataLayer;
using LinkedInDemo.Class;
using LinkedInDemo.CustomAuthentication;
using LinkedInDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LinkedInDemo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                bool isAdmin = false;
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    if (!CustomerService.IsEmailVerified(loginView.UserName))
                    {
                        ModelState.AddModelError("", "Please verify your Email");
                        return View(loginView);
                    }
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    List<string> role = new List<string>();
                    foreach (var r in user.Roles)
                    {
                        var rolename = RoleService.GetRoleById(r.RoleId);
                        role.Add(rolename.RoleName);
                        if (rolename.RoleName == "Admin")
                            isAdmin = true;
                    }
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            UserId = user.CustomerId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = role
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                        (
                        1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                        );
                        Session["CustomerId"] = userModel.UserId;
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("usercookie", enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        if (isAdmin)
                            return RedirectToAction("Admindashboard", "Dashboard");
                        else
                            return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password invalid");
                }
            }

            return View(loginView);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("usercookie", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegisterCustomerModel model = new RegisterCustomerModel();

            return View(model);
        }


        [HttpPost]
        public ActionResult Register(RegisterCustomerModel model)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(model.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    return View(model);
                }

                //Save User Data 
                var customer = new Customer()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Contactno = model.Contactno,
                    Username = model.Email,
                    Password = model.Password,
                    CompanyName = model.CompanyName,
                    Date_of_Registration = DateTime.Now,
                    IsActive = false,
                    createddate = DateTime.Now,
                    updateddate = DateTime.Now,
                    ActivationCode = Guid.NewGuid()
                };
                var role = RoleService.GetRoleByName("User");
                CustomerRole customerrole = new CustomerRole();
                if (role != null)
                {
                    customerrole.RoleId = role.RoleId;
                    customerrole.CustomerId = customer.CustomerId;
                }
                customer.CustomerRoles.Add(customerrole);

                CustomerService.InsertCustomer(customer);

                //Verification Email
                VerificationEmail(model.FirstName, model.LastName, model.Email, customer.ActivationCode.ToString());

                messageRegistration = "Congratulations! You have successfully created your account.Please check your email for instruction to activate your account.";
                statusRegistration = true;
                Transaction transaction = new Transaction();
                transaction.CustomerId = customer.CustomerId;
                transaction.Amount = 100;
                transaction.TransactionDate = DateTime.Now;
                transaction.Status = "completed";
                transaction.TransactionType = "credit";
                transaction.CreatedDate = DateTime.Now;
                transaction.UpdatedDate = DateTime.Now;
                transaction.PaymentMethod = "offline";
                TransactionService.InsertTransaction(transaction);
                ViewBag.Message = messageRegistration;
                ViewBag.Status = statusRegistration;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;

            var customer = CustomerService.GetCustomerByActivationCode(id);
            if (customer != null)
            {
                customer.IsActive = true;
                CustomerService.UpdateCustomer(customer);
                statusAccount = true;
            }
            else
            {
                ViewBag.Message = "Something Wrong !!";
            }
            ViewBag.Status = statusAccount;
            return View();
        }

        [NonAction]
        public void VerificationEmail(string firstname, string lastname, string email, string activationCode)
        {

            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var companywebsite = AdminService.GetAdminSetting();
            var link = companywebsite.CompanyWebsite + url;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
            string subject = "Please activate your EventNX account";
            string header = EmailBody.MailHeader("Account Activation Email", LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
            string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
            string body = "<tr>";
            body = body + "<td style='text-align:left; padding: 0px 30px;'>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>  Hello " + firstname + " " + lastname + ",</p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            body = body + "Welcome to EventNX!</p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>";
            body = body + "To activate your account, please click on the button below to verify your email address.</p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            body = body + "<a href='" + link + "'><button style='background: #6F1855; color:#fff; border:none; padding:9px 20px; cursor: pointer;'> Activation Account ! </button></a></p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Or paste this link intoyour browser:</ p > ";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>";
            body = body + "<a href='" + link + "' target='_blank' style='text-decoration: none; color: #d21180;'>" + link + "</p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>If you have any questions you can contact us at <a href='mailto:contact@eventnx.com' style='text-decoration: none; color: #d21180;'>contact@eventnx.com</a>.</ p > ";
            body = body + "<p style='margin: 0px; padding: 0px 0px 0px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'><b>Thanks and Cheers,</ b > <br />" + "The Entire EventNX team </p> ";
            body = body + "</td></tr>";
            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>To activate your account, please click on the button below to verify your email address.</p>" + "  <p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><a href='" + link + "'><button> Activation Account ! </button></a></p>";
            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Or paste this link into your browser:";
            //body = body + "</p><br/>";
            //body = body + " <p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>" + link + "</p>";
            //body = body + "<br/>";
            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>If you have any questions you can contact us at contact@eventnx.com </p>";
            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>Thanks and Cheers, <br />" + "The Entire EventNX team </p>";
            //body = body + "</td></tr>";
            string mailbody = header + body + footer;
            SendMailService.SendMail(email, subject, mailbody);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordmodel model)
        {
            var customer = CustomerService.GetCustomerByEmail(model.Email);
            if (customer == null)
            {
                ViewBag.error = "You are not registered yet";
                return View(model);
            }
            var url = string.Format("/Account/ResetPassword/{0}", customer.ActivationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
            string subject = "Forgot Password ";
            string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
            string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
            string body = "<tr>";
            body = body + "<td style='text-align:left; padding: 0px 30px;'>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>  Dear User, </p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'> Please click on the following link in order to reset your account password.</p>";
            body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><a href='" + link + "' style='text-decoration: none; color: #d21180;'> Forgot Password ! </a></p><br />";
            body = body + "<p style='margin: 0px; padding: 0px 0px 0px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'><b>Cheers,</b> <br />" + LinkedInDemo.Class.AdminSiteConfiguration.SiteName + " Team</p>";
            body = body + "</td></tr>";

            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><a href='" + link + "'> Forgot Password ! </a>";
            //body = body + "</p><br/>";
            //body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>Cheers, <br />" + LinkedInDemo.Class.AdminSiteConfiguration.SiteName + " Team</p>";

            string mailbody = header + body + footer;
            SendMailService.SendMail(model.Email, subject, mailbody);
            ViewBag.success = "Password reset link sent";
            return View(model);
        }

        public ActionResult ResetPassword(string id)
        {
            var customer = CustomerService.GetCustomerByActivationCode(id);
            if (customer == null)
            {
                ViewBag.Message = "Invalid Url";
            }
            Changepasswordmodel model = new Changepasswordmodel();
            model.ActivationCode = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPassword(Changepasswordmodel model)
        {

            if (model.NewPassword == null || model.ConfirmPassword == null || model.NewPassword == "" || model.ConfirmPassword == "")
            {
                return View();
            }
            if (model.ConfirmPassword == model.NewPassword)
            {
                var customer = CustomerService.GetCustomerByActivationCode(model.ActivationCode);
                customer.Password = model.ConfirmPassword;
                CustomerService.UpdateCustomer(customer);
                ViewBag.success = "Password Updated Succesfully";
            }
            else
            {
                ViewBag.error = "New Password and Confirm Password should be same";
                return View();
            }


            return View(model);
        }

        //public ActionResult ContactUs()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult ContactUs(ContactUsModel model)
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

        [HttpPost]
        public JsonResult GetQuote(ContactUsModel model)
        {
            string subject = " New Inquiry for Credits ";
            string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
            string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
            string body = "<tr>";
            body = body + "<td style='text-align:left; padding: 0px 30px;'>";
            body = body + "<p>  <b>FullName :</b> " + model.FullName + " </p>";
            body = body + "<p>  <b>CompanyName :</b> " + model.CompanyName + " </p>";
            body = body + "<p> <b>PhoneNumber :</b>" + model.PhoneNumber + " </p>";
            body = body + "<p> <b>EmailAddress :</b>" + model.EmailAddress + " </p>";
            body = body + "<p> <b>CreditsRequired : </b>" + model.CreditsRequired + " </p>";
            body = body + "<p>Thanks and Cheers,<br />";
            body = body + "The Entire EventNX team </p>";
            string mailbody = header + body + footer;
            SendMailService.SendMail("contact@eventnx.com,raufs@tacttree.com,nipulp@tacttree.com", subject, mailbody);
            //  return View(model);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}