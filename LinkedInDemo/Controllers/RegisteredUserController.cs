using DataLayer;
using LinkedInDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLayer;
using System.Web.Security;
using LinkedInDemo.CustomAuthentication;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using System.Web;
using LinkedInDemo.Class;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "Admin,User")]
    public class RegisteredUserController : Controller
    {
        protected static string name = "";
        protected static string pas = "";
        protected static string StatusMessage = "";
        protected static string Message = "";
        private const int FirstPageIndex = 1;
        protected static int TotalDataCount;
        MongodbHelper _mongodbHelper;
        protected static Array Arr;
        protected static bool IsArray;
        protected static IEnumerable BindData;
        RegisteredUserModel model = new RegisteredUserModel();
        public RegisteredUserController()
        {
            _mongodbHelper = new MongodbHelper();
        }
        public ActionResult Index(int EventId = 0, int UserId = 0, int usercount = 1)
        {
            ViewBag.EventId = EventId;
            model.ReferralId = UserId;
            if (EventId > 0)
            {
                Session["EventId"] = EventId;
                model.EventId = EventId;
                model.RegisteredUser = RegisteredUserService.GetRegisteredUserByEventId(EventId).Count();
                ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(EventId).Count();
                model.Linkedinusers = RegisteredUserService.GetLinkedinUserByEventId(EventId).Count();
                model.Facebookusers = RegisteredUserService.GetFacebookUserByEventId(EventId).Count();
                model.Creditrequired = RegisteredUserService.Creditrequired(EventId);
                model.TotalReferences = RegisteredUserService.GetReferralIByEventId(EventId);
                model.EventName = RegisteredUserService.GetEventNamebyEventId(EventId);
                model.EventStartDate = RegisteredUserService.GetStartdatebyEventId(EventId);
                model.EventEndDate = RegisteredUserService.GetEnddatebyEventId(EventId);
                model.Image = RegisteredUserService.GetEventlogobyEventId(EventId);
                model.ButtonURL = EventService.GetEventById(EventId).ButtonURL;
            }
            if (UserId == 0)
            {
                model.usercount = 1;
            }
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            ViewBag.CustomerId = CustomerId;
            var eventdata = EventService.geteventdetailbyeventId(EventId);
            ViewBag.Customers = eventdata;
            var eventuser = EventService.GetEventusersByCustomerId(Convert.ToInt32(Session["CustomerId"]));
            if (eventuser != null)
            {
                ViewBag.EventUserData = "false";
            }
            else
            {
                ViewBag.EventUserData = "true";
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Index(int EventId, HttpPostedFileBase file)
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
            var test = Request.Files.Count;
            if (file != null)
            {
                try
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    pic = eventmaster.EventId + "_" + pic;
                    string savepath = System.IO.Path.Combine(Server.MapPath("~/Content/images"), pic);
                    string path = AdminSiteConfiguration.GetURL() + "/Content/images/" + pic;
                    file.SaveAs(savepath);
                    eventmaster.ButtonURL = path;
                    EventService.UpdateEvent(eventmaster);
                    return RedirectToAction("Index", new { EventId = EventId });
                }
                catch
                {

                }
            }
            return RedirectToAction("Index", new { EventId = EventId });
        }
        public FileResult DownloadcodePDF(int EventId)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var eventdata = EventService.GetEventById(EventId);
                string eventcode = geteventcode(eventdata.EventKey.ToString());
                //eventcode = HttpUtility.HtmlEncode(eventcode);
                StringReader sr = new StringReader(eventcode);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "EventCode.pdf");
            }
        }
        public string geteventcode(string key)
        {
            var codeblock = "<h2>Setting Up Login Button:</h2>";
            codeblock = codeblock + "<p>Place below code in header section only if you do not have jquery installed on your webpage</p><br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p>Place below script in head section of your webpage </p> <br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p>Place below code on webpage where you want to show LinkedIn login button </p> <br>";
            codeblock = codeblock + "<code>&lt;div class='linkedinlogin' id='linkedinlogin_" + key + "'&gt;&lt;/div&gt;&lt;div id='divallowpost'&gt;";
            codeblock = codeblock + "&lt;input type='checkbox' id='AllowPost' checked='checked' /&gt;Let my social network know that I am attending the event. This gives us permission to post to your profile about your attendance. It also allows other registered visitors to see that you’re registered to attend.";
            codeblock = codeblock + "&lt;/div&gt;</code> <br><br>";
            codeblock = codeblock + "<h2>Setting Up Event Registration Form:</h2><br>Place below code in header section only if you do not have jquery installed on your webpage <br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p>Place below script in header section of your webpage</p> <br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p>Use below hidden fields value to autofill form with LinkedIn data</p><br>";
            codeblock = codeblock + "<p>hidEventProUserId , hidProfileImage , hidFirstName , hidLastName , hidEmail</p> <br><br>";
            codeblock = codeblock + "Place below div tag in Registration Form</br>";
            codeblock = codeblock + "<code>&lt;div id='eventregistration' class='eventregistration' &gt; &lt;/div &gt;</code><br><br>";
            codeblock = codeblock + "<h2>Setting Up Confirmation Page:</h2><br>Place below code in header section only if you do not have jquery installed on your webpage  <br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p>Place below script in header section of your webpage </p><br>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code> <br><br>";
            codeblock = codeblock + "<p>After successful registration, render below script with CRM parameters. </p> <br>";
            codeblock = codeblock + "<code>&lt;script src= 'https://www.eventnx.com/eventscript/submitdatatoeventpro.js'";
            codeblock = codeblock + " data-eventprouserid='Eventpro_system_userid'";
            codeblock = codeblock + " data-firstname='Your_system_firstname_param'";
            codeblock = codeblock + " data-lastname='Your_system_lastname_param'";
            codeblock = codeblock + " data-emailid='Your_system_emailid_param'";
            codeblock = codeblock + " data-companyname='Your_system_companyname_param'";
            codeblock = codeblock + " data-jobtitle='Your_system_jobtitle_param'";
            codeblock = codeblock + " data-crmregid='Your_system_crmregid_param'&gt;</code><br><br>";
            codeblock = codeblock + "<p>Below script will display a sharable link on the confirmation page which registered user can copy and share on social media.</p><br>";
            codeblock = codeblock + "<code>&lt;div id='divsuccessmessage' style='display: none;'&gt; &lt;/div &gt;</code><br>";
            codeblock = codeblock + "<p>Below script lists all the socially registered users in a grid manner.</p><br>";
            codeblock = codeblock + "<code>&lt;div id='divwhoisgoing' style='display: none;'&gt; &lt;/div &gt; </code><br><br>";
            codeblock = codeblock + "<h2>Embed Code  <br>";
            codeblock = codeblock + "<code>&lt;a id=&quot;btnLinkedIn&quot; target=&quot;blank&quot; ";
            codeblock = codeblock + "href=&quot;https://www.linkedin.com/oauth/v2/authorization?response_type=code&amp;amp;client_id=81wyrvvax51otg&amp;amp;scope=r_emailaddress%20w_share%20r_basicprofile%20r_liteprofile%20rw_company_admin%20w_member_social&amp;amp;redirect_uri=https%3A%2F%2Fwww.eventnx.com%2F%3Feid%3D" + key + "%26r%3D0&quot;&gt";
            codeblock = codeblock + "&lt;img src=&quot;https://www.eventnx.com/Content/images/button4.png&quot;&gt;";
            codeblock = codeblock + "&lt;/a&gt;</code>";
            return codeblock;
        }
        public ActionResult UserGrid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "UserId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", string Domain = "", int Event = 0, int ReferralId = 0)
        {
            try
            {

                if (IsArray == true)
                {
                    foreach (string a1 in Arr)
                    {
                        if (a1.Split(':')[0].ToString() == "IsBindData")
                        {
                            IsBindData = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "currentPageIndex")
                        {
                            currentPageIndex = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "orderby")
                        {
                            orderby = Convert.ToString(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "IsAsc")
                        {
                            IsAsc = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "PageSize")
                        {
                            PageSize = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "SearchRecords")
                        {
                            SearchRecords = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "Alpha")
                        {
                            Alpha = Convert.ToString(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "SearchTitle")
                        {
                            SearchTitle = Convert.ToString(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "Domain")
                        {
                            Domain = Convert.ToString(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "Event")
                        {
                            Event = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "ReferralId")
                        {
                            ReferralId = Convert.ToInt32(a1.Split(':')[1].ToString());
                        }

                    }
                }
            }
            catch { }

            IsArray = false;
            Arr = new string[]
            {  "IsBindData:" + IsBindData
                ,"currentPageIndex:" + currentPageIndex
                ,"orderby:" + orderby
                ,"IsAsc:" + IsAsc
                ,"PageSize:" + PageSize
                ,"Alpha:" + Alpha
                ,"SearchRecords:" + SearchRecords
                ,"SearchTitle:" + SearchTitle
                ,"Domain:" + Domain
                ,"Event:" + Event
                ,"ReferralId:" + ReferralId
            };

            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                //Event = Convert.ToInt32(Request.QueryString["EventId"]);
                BindData = GetData(SearchRecords, SearchTitle, Alpha, Domain, Event, ReferralId).OfType<RegisteredUserModel>().ToList();
                TotalDataCount = BindData.OfType<RegisteredUserModel>().ToList().Count();
            }

            if (TotalDataCount == 0)
            {
                StatusMessage = "NoItem";
            }

            ViewBag.IsBindData = IsBindData;
            ViewBag.CurrentPageIndex = currentPageIndex;
            ViewBag.LastPageIndex = this.getLastPageIndex(PageSize);
            ViewBag.OrderByVal = orderby;
            ViewBag.IsAscVal = IsAsc;
            ViewBag.PageSize = PageSize;
            ViewBag.SearchRecords = SearchRecords;
            ViewBag.Alpha = Alpha;
            ViewBag.SearchTitle = SearchTitle;
            TempData["SearchTitle"] = SearchTitle;
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.Domain = Domain;
            ViewBag.Event = Event;
            TempData["Event"] = Event;
            ViewBag.ReferralId = ReferralId;
            TempData["ReferralId"] = ReferralId;
            Session["ReferralId"] = ReferralId;

            ViewBag.TotalRegistration = TotalDataCount;
            var ColumnName = typeof(RegisteredUserModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {

                ViewBag.AscVal = 0;
                Data = BindData.OfType<RegisteredUserModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<RegisteredUserModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }

            StatusMessage = "";
            Message = "";
            List<SelectListItem> domainlist = getdomain();
            ViewBag.domainlist = domainlist;
            List<SelectListItem> eventlist = getevent();
            ViewBag.eventlist = eventlist;
           
            ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(Event).Count();
            return View(Data);
        }

        private int getLastPageIndex(int PageSize)
        {
            int lastPageIndex = Convert.ToInt32(TotalDataCount) / PageSize;
            if (TotalDataCount % PageSize > 0)
                lastPageIndex += 1;

            return lastPageIndex;
        }

        private IEnumerable GetData(int SearchRecords, string SearchTitle, string Alpha, string Domain, int Event, int ReferralId)
        {
            Domain = Domain.Trim().ToLower();
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            //int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            int CustomerId = 0;
            ViewBag.CustomerId = CustomerId;
            List<RegisteredUserModel> userlist = new List<RegisteredUserModel>();
            var regusers = RegisteredUserService.SearchRegisteredUser(SearchTitle, Alpha, Domain, Event, ReferralId);
            foreach (var user in regusers)
            {
                RegisteredUserModel registeredUserModel = new RegisteredUserModel();
                registeredUserModel.UserId = user.UserId;
                registeredUserModel.FirstName = user.FirstName;
                registeredUserModel.LastName = user.LastName;
                registeredUserModel.Email = user.Email;
                registeredUserModel.Country = user.Country;
                registeredUserModel.DomainName = user.DomainName;
                registeredUserModel.EventId = user.EventId;
                registeredUserModel.EventName = user.EventName;
                registeredUserModel.EventStartDate = user.EventStartDate;
                registeredUserModel.EventEndDate = user.EventEndDate;
                registeredUserModel.DateOfRegistration = user.DateOfRegistration;
                registeredUserModel.ReferralCount = Convert.ToInt32(user.ReferralCount);
                registeredUserModel.ConnectionCount = Convert.ToInt32(user.ConnectionCount);
                registeredUserModel.ProfileImage = user.ProfileImage;
                registeredUserModel.CRM_CompanyName = user.CRM_CompanyName;
                registeredUserModel.CRM_JobTitle = user.CRM_JobTitle;
                registeredUserModel.Source = user.Source;
                userlist.Add(registeredUserModel);
            }
            
            return userlist;
        }

        [HttpPost]
        public ActionResult GetRegisteredUser(int UserId)
        {
            var registereduser = RegisteredUserService.GetRegisteredUserById(UserId);
            if (registereduser == null)
                return Json(new { success = false });

            RegisteredUserModel model = new RegisteredUserModel();
            model.UserId = registereduser.UserId;
            model.EventId = (int)registereduser.EventId;
            model.FirstName = registereduser.FirstName;
            model.LastName = registereduser.LastName;
            model.Email = registereduser.Email;
            model.Country = registereduser.Country;
            model.ProfileLink = registereduser.ProfileLink;
            model.ProfileImage = registereduser.ProfileImage;
            return Json(new { success = true, user = model });
        }

        public ActionResult UserEdit(int UserId = 0)
        {
            RegisteredUser registeredUser = RegisteredUserService.GetRegisteredUserById(UserId);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            RegisteredUserModel model = new RegisteredUserModel();
            model.EventId = (int)registeredUser.EventId;
            model.FirstName = registeredUser.FirstName;
            model.LastName = registeredUser.LastName;
            model.Email = registeredUser.Email;
            model.Country = registeredUser.Country;
            model.DateOfRegistration = registeredUser.DateOfRegistration;
            model.IsDeleted = registeredUser.IsDeleted;
            model.IsRegistered = (bool)registeredUser.IsRegistered;
            model.ProfileImage = registeredUser.ProfileImage;
            model.ProfileLink = registeredUser.ProfileLink;
            var bsonresult = _mongodbHelper.GetUserDetailByEmail(registeredUser.Email, registeredUser.EventMaster.EventKey.ToString());
            if (bsonresult != null)
            {
                ViewBag.UserDocumentData = _mongodbHelper.ToJson(bsonresult);
            }
            ViewBag.FormCode = registeredUser.EventMaster.FormBuilderCode;
            return View(model);
        }

        [HttpPost]
        public ActionResult UserEdit(RegisteredUserModel model)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            RegisteredUser registeredUser = RegisteredUserService.GetRegisteredUserById(model.UserId);
            registeredUser.EventId = model.EventId;
            registeredUser.FirstName = model.FirstName;
            registeredUser.LastName = model.LastName;
            registeredUser.Email = model.Email;
            registeredUser.Country = model.Country;
            registeredUser.DateOfRegistration = model.DateOfRegistration;
            registeredUser.IsDeleted = model.IsDeleted;
            registeredUser.IsRegistered = model.IsRegistered;
            registeredUser.ProfileImage = model.ProfileImage;
            registeredUser.ProfileLink = model.ProfileLink;
            RegisteredUserService.UpdateRegisteredUser(registeredUser);

            ViewBag.StatusMessage = "SuccessUpdate";
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int UserId)
        {
            RegisteredUserService.DeleteRegisteredUser(UserId);
            return RedirectToAction("UserGrid");
        }

        private static List<SelectListItem> getdomain()
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            List<SelectListItem> domainlist = (from p in entities.EventMasters.AsEnumerable()
                                               select new SelectListItem
                                               {
                                                   Text = p.DomainName,
                                                   Value = p.DomainName.ToString()
                                               }).ToList();

            domainlist.Insert(0, new SelectListItem { Text = "--Select Domain--", Value = "" });
            return domainlist;
        }

        private static List<SelectListItem> getevent()
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            List<SelectListItem> eventlist = (from p in entities.EventMasters.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Text = p.EventName,
                                                  Value = p.EventId.ToString()
                                              }).ToList();

            eventlist.Insert(0, new SelectListItem { Text = "--Select Event--", Value = "0" });
            return eventlist;
        }

        public ActionResult ExporttoExcel()
        {
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);


            var exportregistereduserdetails = RegisteredUserService.Eventwiseuser(Convert.ToString(TempData["SearchTitle"]), "", "", Convert.ToInt32(TempData["Event"]), CustomerId, Convert.ToInt32(TempData["ReferralId"]));
            //var exportuserdetails = RegisteredUserService.SearchRegisteredUser(Convert.ToString(TempData["SearchTitle"]), "", "", Convert.ToInt32(TempData["Event"]), Convert.ToInt32(TempData["ReferralId"]));

            List<ExportRegisteredUserModel> ExportRegisteredUserModel = new List<Models.ExportRegisteredUserModel>();
            foreach (var item in exportregistereduserdetails)
            {
                var model = new ExportRegisteredUserModel();
                model.Id = item.UserId;
                model.Name = item.FirstName + item.LastName;
                model.Email = item.Email;
                model.DateOfRegistration = item.DateOfRegistration;
                model.ReferralCount = item.ReferralCount;
                ExportRegisteredUserModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ExportRegisteredUserModel;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Registereduser.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }

        public ActionResult Transactions(int EventId)
        {
            //var checkcredit = CustomerService.GetCustomerAvailableCredit(Convert.ToInt32(Session["CustomerId"]));
            var extracredit = RegisteredUserService.Creditrequired(EventId);
            //if (checkcredit >= extracredit)
            //{
            //    Transaction transaction = new Transaction();
            //    TransactionsModel transactionmodel = new TransactionsModel();
            //    transaction.Id = transactionmodel.Id;
            //    transaction.CustomerId = Convert.ToInt32(Session["CustomerId"]);
            //    transaction.Amount = extracredit;
            //    transaction.TransactionDate = DateTime.Now;
            //    transaction.Status = "completed";
            //    transaction.TransactionType = "debit";
            //    transaction.Isdeleted = false;
            //    transaction.CreatedDate = DateTime.Now;
            //    transaction.UpdatedDate = DateTime.Now;
            //    TransactionService.InsertTransaction(transaction);
            //    RegisteredUserService.MarkRegisteredUserAsPaidForEvent(false,EventId,0);

            //    return RedirectToAction("Index", new { EventId = EventId });
            //}
            //else
            //{
                Session["usecredit"] = true;
                return RedirectToAction("AddCredit", "Transactions", new { usercredit = true, eventid = EventId, amount = extracredit });
           // }

        }

        public ActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Changepassword(Changepasswordmodel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = CustomerService.GetCustomerById(Convert.ToInt32(Session["CustomerId"]));

                if (model.CurrentPassword == customer.Password)
                {
                    if (model.ConfirmPassword == model.NewPassword)
                    {
                        customer.Password = model.ConfirmPassword;
                        CustomerService.UpdateCustomer(customer);

                        ViewBag.success = "Updated Password Succesfully";
                    }
                    else
                    {
                        ViewBag.error = "New Password and Confirm Password should be same";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Incorrect Current Password";
                    return View();
                }
            }

            return View(model);
        }

        public ActionResult Editprofile(int CustomerId = 0)
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            Customer customer = CustomerService.GetCustomerById(Convert.ToInt32(Session["CustomerId"]));
            if (customer == null)
            {
                return HttpNotFound();
            }
            CustomerModel model = new CustomerModel();
            model.CustomerId = customer.CustomerId;
            model.FirstName = customer.FirstName;
            model.LastName = customer.LastName;
            model.Email = customer.Email;
            model.Contactno = customer.Contactno;
            model.Username = customer.Username;
            model.CompanyName = customer.CompanyName;
            model.Date_of_Registration = customer.Date_of_Registration;
            model.address1 = customer.Address1;
            model.address2 = customer.Address2;
            model.City = customer.City;
            model.Pincode = Convert.ToInt32(customer.Pincode);
            model.State = customer.State;
            List<SelectListItem> countrylist = getcountry();
            model.countrylist = countrylist;
            model.Country = customer.Country;
            return View(model);
        }

        [HttpPost]
        public ActionResult Editprofile(CustomerModel model)
        {
            Customer customer = CustomerService.GetCustomerById(model.CustomerId);
            customer.CustomerId = model.CustomerId;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Contactno = model.Contactno;
            customer.Username = model.Username;
            customer.CompanyName = model.CompanyName;
            customer.updateddate = DateTime.Now;
            customer.Address1 = model.address1;
            customer.Address2 = model.address2;
            customer.City = model.City;
            customer.Pincode = model.Pincode;
            customer.State = model.State;
            customer.Country = model.Country;
            CustomerService.UpdateCustomer(customer);
            ViewBag.message = "Profile updated successfully";
            List<SelectListItem> countrylist = getcountry();
            model.countrylist = countrylist;

            return View(model);


        }
        private static List<SelectListItem> getcountry()
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            List<SelectListItem> countrylist = (from p in entities.countries.AsEnumerable()
                                               select new SelectListItem
                                               {
                                                   Text = p.CountryName,
                                                   Value = p.CountryName.ToString()
                                               }).ToList();

            countrylist.Insert(0, new SelectListItem { Text = "--Select Country--", Value = "" });
            return countrylist;
        }

        [HttpPost]
        public ActionResult Download(string filename, string contenttype, string data)
        {
            string FilePath = Path.Combine(Server.MapPath("~/TempFiles/"), filename);
            if (!System.IO.File.Exists(FilePath))
            {
                byte[] bytes = System.Convert.FromBase64String(data);
                if (bytes != null)
                {
                    System.IO.File.WriteAllBytes(string.Format("{0}", FilePath), bytes);
                }
            }
            return Json(new { filepath = FilePath });
        }


    }
}
