using BussinessLayer;
using DataLayer;
using LinkedInDemo.Class;
using LinkedInDemo.CustomAuthentication;
using LinkedInDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class EventController : Controller
    {
        protected static string name = "";
        protected static string pas = "";
        protected static string StatusMessage = "";
        protected static string Message = "";
        private const int FirstPageIndex = 1;
        protected static int TotalDataCount;

        protected static Array Arr;
        protected static bool IsArray;
        protected static IEnumerable BindData;
        public ActionResult Index(string message = "")
        {
            StatusMessage = message;
            var eventuser = EventService.GetEventusersByCustomerId(Convert.ToInt32(Session["CustomerId"]));
            if (eventuser != null)
            {
                ViewBag.EventUserData = "false";
            }
            else
            {
                ViewBag.EventUserData = "true";
            }
            return View();
        }

        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", string ManageEvent = "upcoming")
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

            };

            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                BindData = GetData(SearchRecords, SearchTitle, Alpha, ManageEvent).OfType<EventMasterModel>().ToList();
                TotalDataCount = BindData.OfType<EventMasterModel>().ToList().Count();
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
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.TotalEvent = TotalDataCount;
            ViewBag.ManageEvent = ManageEvent;
            //ViewBag.Referraluserscount=RegisteredUserService.GetReferralIByEventId();
            var ColumnName = typeof(EventMasterModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<EventMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<EventMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }

            StatusMessage = "";
            Message = "";

            return View(Data);
        }
        public ActionResult usergrid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", string ManageEvent = "", int Event=0)
        {
            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                BindData = usergriddetails(Event).OfType<EventMasterModel>().ToList();
                TotalDataCount = BindData.OfType<EventMasterModel>().ToList().Count();
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
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.TotalEvent = TotalDataCount;
            ViewBag.ManageEvent = ManageEvent;
            ViewBag.Event = Event;
            var ColumnName = typeof(EventMasterModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            Data = BindData.OfType<EventMasterModel>().ToList().Skip(startIndex - 1).Take(PageSize);
            StatusMessage = "";
            Message = "";

            return View(Data);
        }
        private IEnumerable usergriddetails(int Event = 0)
        {
            List<EventMasterModel> userlist = new List<EventMasterModel>();
           
            var eventusers = CustomerService.getuserdetails(Event);
            foreach (var user in eventusers)
            {
                EventMasterModel eventmastermodel = new EventMasterModel();
                eventmastermodel.Id = user.Id;
                eventmastermodel.EventId = user.EventId;
                eventmastermodel.CustomerId = user.CustomerId;
                eventmastermodel.EventName = user.EventName;
                eventmastermodel.Email = user.Email;
                eventmastermodel.CreatedDate = user.CreatedDate;
                userlist.Add(eventmastermodel);
            }
            return userlist;
        }
        private int getLastPageIndex(int PageSize)
        {
            int lastPageIndex = Convert.ToInt32(TotalDataCount) / PageSize;
            if (TotalDataCount % PageSize > 0)
                lastPageIndex += 1;

            return lastPageIndex;
        }

        private IEnumerable GetData(int SearchRecords = 0, string SearchTitle = "", string Alpha = "", string ManageEvent = "")
        {

            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            ViewBag.CustomerId = CustomerId;
            //if (string.IsNullOrEmpty(ManageEvent))
            //{
            //    ManageEvent = "upcoming";
            //}
            var eventdata = CustomerService.GetCustomerEvents(SearchTitle, Alpha, CustomerId, ManageEvent);
            List<EventMasterModel> eventlist = new List<EventMasterModel>();
            foreach (var eventrec in eventdata)
            {
                EventMasterModel eventMasterModel = new EventMasterModel();
                eventMasterModel.EventId = eventrec.EventId;
                eventMasterModel.CustomerId = eventrec.CustomerId;
                eventMasterModel.EventName = eventrec.EventName;
                eventMasterModel.Description = eventrec.Description;
                eventMasterModel.EventURL = eventrec.DomainName;
                eventMasterModel.Description = eventrec.Description;
                eventMasterModel.EventStartDate = eventrec.EventStartDate.ToShortDateString();
                eventMasterModel.EventEndDate = eventrec.EventEndDate.ToShortDateString();
                string path = "../Images/EventLogo/" + eventrec.LogoUrl;
                eventMasterModel.Image = path;
                eventMasterModel.ImageName = eventrec.LogoUrl;
                eventMasterModel.ButtonURL = eventrec.ButtonURL;
                eventMasterModel.FbButtonURL = eventrec.FbButtonURL;
                eventMasterModel.GoogleButtonURL = eventrec.GoogleButtonURL;
                eventMasterModel.RegisteredUsers = Convert.ToInt32(eventrec.TotalCount);
                eventMasterModel.Linkedinusers = Convert.ToInt32(eventrec.LinkedinUsers);
                eventMasterModel.Facebookusers = Convert.ToInt32(eventrec.FacebookUsers);
                eventMasterModel.Referralusers=  RegisteredUserService.GetReferralIByEventId(eventrec.EventId);
                eventlist.Add(eventMasterModel);
            }
            return eventlist;
        }
        
        [HttpGet]
        public ActionResult GetCustomerEvent()
        {
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            List<EventMasterModel> eventlist = new List<EventMasterModel>();
            var eventdata = CustomerService.GetCustomerEvents("", "", CustomerId, "");
            foreach (var eventrec in eventdata)
            {
                EventMasterModel eventMasterModel = new EventMasterModel();
                eventMasterModel.EventId = eventrec.EventId;
                eventMasterModel.CustomerId = eventrec.CustomerId;
                eventMasterModel.EventName = eventrec.EventName;
                eventMasterModel.Description = eventrec.Description;
                eventMasterModel.EventURL = eventrec.DomainName;
                eventMasterModel.Description = eventrec.Description;
                eventMasterModel.EventStartDate = eventrec.EventStartDate.ToShortDateString();
                eventMasterModel.EventEndDate = eventrec.EventEndDate.ToShortDateString();
                string path = "../Images/EventLogo/" + eventrec.LogoUrl;
                eventMasterModel.Image = path;
                eventMasterModel.RegisteredUsers = Convert.ToInt32(eventrec.TotalCount);
                eventlist.Add(eventMasterModel);
            }
            var data = JsonConvert.SerializeObject(eventlist.OrderByDescending(x => x.RegisteredUsers));
            return Json(new { eventdata = data, success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            EventMasterModel model = new EventMasterModel();
            model.AvailableCredits = CustomerService.GetCustomerAvailableCredit(Convert.ToInt32(Session["CustomerId"]));
            var package = PackageService.GetAllPackages().FirstOrDefault();
            if (package == null)
            {
                model.Cost = 0;
            }
            else
            {
                model.Cost = package.Cost;
            }
            //if (model.AvailableCredits >= model.Cost)
            //{
            //    return View(model);
            //}
            //else
            //{
            //    ViewBag.Message = "You do not have enough balance to create new event !! Min credit balance required " + model.Cost + "$ ,please add credit to create event.";
            //}
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EventMasterModel model, HttpPostedFileBase file, HttpPostedFileBase filebtn)
        {
            if (ModelState.IsValid)
            {
                EventMaster eventMaster = new EventMaster();
                eventMaster.CustomerId = Convert.ToInt32(Session["CustomerId"]);
                eventMaster.EventName = model.EventName;
                eventMaster.DomainName = model.EventURL;
                eventMaster.Description = model.Commentary;
                eventMaster.ResponseURL = model.ResponseURL;
                eventMaster.Commentary = model.Commentary;
                eventMaster.ArticalUrl = model.ArticalUrl;
                eventMaster.ArticalTitle = model.ArticalTitle;
                eventMaster.EventStartDate = Convert.ToDateTime(model.EventStartDate);
                eventMaster.EventEndDate = Convert.ToDateTime(model.EventEndDate);
                eventMaster.ContactPersonName = model.ContactPersonName;
                eventMaster.ContactPersonPhone = model.ContactPersonPhone;
                eventMaster.ContactPersonEmail = model.ContactPersonEmail;
                eventMaster.IsDeleted = false;
                eventMaster.Channel = model.Channel;
                eventMaster.HeySummitWebinarId = model.HeySummitWebinarId;
                eventMaster.HeySummitToken = model.HeySummitToken;
                if(model.ButtonURL == null)
                {
                    model.ButtonURL = AdminSiteConfiguration.GetURL() + "/Content/images/connect1.png";
                }
                eventMaster.ButtonURL = model.ButtonURL;
                eventMaster.FbButtonURL = model.FbButtonURL;
                eventMaster.GoogleButtonURL = model.GoogleButtonURL;
                eventMaster.EventKey = Guid.NewGuid();
                eventMaster.ZoomToken = model.ZoomToken;
                eventMaster.WebinarId = model.WebinarId;
                eventMaster.ZoomUUID = model.ZoomUUID;
                eventMaster.BigMarkerWebinairId = model.BigMarkerWebinairId;
                eventMaster.BigMarkerToken = model.BigMarkerToken;
               
                if (file != null)
                {
                    try
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        pic = eventMaster.EventId + "_" + pic;
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images/EventLogo"), pic);
                        file.SaveAs(path);
                        eventMaster.LogoUrl = pic;
                    }
                    catch
                    {

                    }
                }
                if (filebtn != null)
                {
                    try
                    {
                        string pic = System.IO.Path.GetFileName(filebtn.FileName);
                        //pic = eventMaster.EventId + "_" + pic;
                        string savepath = System.IO.Path.Combine(Server.MapPath("~/Content/images"), pic);
                        string path = AdminSiteConfiguration.GetURL() + "/Content/images/" + pic;
                        filebtn.SaveAs(savepath);
                        eventMaster.ButtonURL = path;
                    }
                    catch
                    {

                    }
                }
                EventService.InsertEvent(eventMaster);
                ViewBag.EventId = eventMaster.EventId;
                ViewBag.StatusMessage = "SuccessAdd";
              

                //Transaction transaction = new Transaction();
                //transaction.CustomerId = Convert.ToInt32(Session["CustomerId"]);
                //transaction.Amount = PackageService.GetAllPackages().FirstOrDefault().Cost;
                //transaction.TransactionDate = DateTime.Now;
                //transaction.Status = "completed";
                //transaction.TransactionType = "debit";
                //transaction.CreatedDate = DateTime.Now;
                //TransactionService.InsertTransaction(transaction);
                StatusMessage = "SuccessAdd";
                int custid = Convert.ToInt32(Session["CustomerId"]);
                string eventcode = geteventcode(eventMaster.EventKey.ToString());
                var customer = CustomerService.GetCustomerById(custid);
                string customeremail = customer.Email;
                string firstname = customer.FirstName;
                string eventname = model.EventName;
                sendcode(customeremail, "Your event is successfully created with EventNX", eventcode, eventMaster.EventId.ToString(), firstname, eventname);
                return RedirectToAction("Index");

            }
            else
            {
                return View(model);
            }
        }

        public string geteventcode(string key)
        {
            var codeblock = "<h2 style='font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'>Setting Up Login Button:</h2>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Place below code in header section only if you do not have jquery installed on your webpage<br>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Place below script in head section of your webpage  </br>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code> <br>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Place below code on webpage where you want to show LinkedIn login button  </br>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd; line-height: 22px;'>";
            codeblock = codeblock + "<code>&lt;div class='linkedinlogin' id='linkedinlogin_" + key + "'&gt;&lt;/div&gt;&lt;div id='divallowpost'&gt;";
            codeblock = codeblock + "&lt;input type='checkbox' id='AllowPost' checked='checked' /&gt;Let my social network know that I am attending the event. This gives us permission to post to your profile about your attendance. It also allows other registered visitors to see that you�re registered to attend.";
            codeblock = codeblock + "&lt;/div&gt;</code></p>";
            codeblock = codeblock + "<h2 style='font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'>Setting Up Event Registration Form:</h2>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Place below code in header section only if you do not have jquery installed on your webpage</p >";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code></p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Place below script in header section of your webpage  </p>";
            codeblock = codeblock + " <p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code> </p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Use below hidden fields value to autofill form with LinkedIn data</p>";
            codeblock = codeblock + "<ul style='margin: 0px; padding: 0px 0px 20px 15px;'>";
            codeblock = codeblock + "<li style='font-size: 16px; color: #000; padding-bottom: 7px;'>hidEventProUserId</li>";
            codeblock = codeblock + "<li style='font-size: 16px; color: #000; padding-bottom: 7px;'>hidProfileImage</li>";
            codeblock = codeblock + "<li style='font-size: 16px; color: #000; padding-bottom: 7px;'>hidFirstName</li>";
            codeblock = codeblock + " <li style='font-size: 16px; color: #000; padding-bottom: 7px;'>hidLastName</li>";
            codeblock = codeblock + " <li style='font-size: 16px; color: #000; padding-bottom: 7px;'>hidLastName</li></ul>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + " Place below div tag in Registration Form</p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;div id='eventregistration' class='eventregistration' &gt;&lt;/div&gt;</code></p >";
            codeblock = codeblock + "<h2 style='font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'>Setting Up Confirmation Page:</h2>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Place below code in header section only if you do not have jquery installed on your webpage</p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/jquery-1.12.0.min.js'&gt;&lt;/script&gt;</code></p >";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Place below script in header section of your webpage </p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;script src='https://www.eventnx.com/eventscript/eventproloader.js'&gt;&lt;/script&gt;</code></p >";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "After successful registration, render below script with CRM parameters.</p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd; line-height: 22px;'>";
            codeblock = codeblock + "<code>&lt;script src= 'https://www.eventnx.com/eventscript/submitdatatoeventpro.js' data-eventprouserid='Eventpro_system_userid' data-firstname='Your_system_firstname_param' data-lastname='Your_system_lastname_param' data-emailid='Your_system_emailid_param' data-companyname='Your_system_companyname_param' data-jobtitle='Your_system_jobtitle_param' data-crmregid='Your_system_crmregid_param'&gt;</code></p >";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Below script will display a sharable link on the confirmation page which registered user can copy and share on social media.</p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;div id='divsuccessmessage' style='display: none;'&gt;&lt;/div&gt;</code> </p >";
            codeblock = codeblock + "<p style='margin: 0px; padding: 0px 0px 5px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>";
            codeblock = codeblock + "Below script lists all the socially registered users in a grid manner.</p>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd;'>";
            codeblock = codeblock + "<code>&lt;div id='divwhoisgoing' style='display: none;'&gt;&lt;/div&gt; </code></p >";
            codeblock = codeblock + "<h2 style='font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'>Embed Code</h2>";
            codeblock = codeblock + "<p style='margin: 0px; padding: 5px 10px; margin-bottom: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; background-color: #ddd; line-height: 22px;'>";
            codeblock = codeblock + "<code>&lt;a id=&quot;btnLinkedIn&quot; target=&quot;blank&quot; href=&quot;https://www.linkedin.com/oauth/v2/authorization?response_type=code&amp;amp;client_id=81wyrvvax51otg&amp;amp;scope=r_emailaddress%20w_share%20r_basicprofile%20r_liteprofile%20rw_company_admin%20w_member_social&amp;amp;redirect_uri=https%3A%2F%2Fwww.eventnx.com%2F%3Feid%3D" + key + "%26r%3D0&quot;&gt&lt;img src=&quot;https://www.eventnx.com/Content/images/button4.png&quot;&gt;&lt;/a&gt;";
            codeblock = codeblock + "</code></p>";
            return codeblock;
        }

        [HttpPost]
        public ActionResult Updatebutton(int EventId, string linkedinradio, string facebookradio, string googleradio, HttpPostedFileBase file,HttpPostedFileBase filefb, HttpPostedFileBase filegb)
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
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
                }
                catch
                {

                }
            }
            else
            {
                eventmaster.ButtonURL = linkedinradio;
            }
            if (filefb != null)
            {
                try
                {
                    string pic = System.IO.Path.GetFileName(filefb.FileName);
                    pic = eventmaster.EventId + "_" + pic;
                    string savepath = System.IO.Path.Combine(Server.MapPath("~/Content/images/fb/"), pic);
                    string path = AdminSiteConfiguration.GetURL() + "/Content/images/fb/" + pic;
                    file.SaveAs(savepath);
                    eventmaster.FbButtonURL = path;
                }
                catch
                {

                }
            }
            else
            {
                eventmaster.FbButtonURL = facebookradio;
            }
            if (filegb != null)
            {
                try
                {
                    string pic = System.IO.Path.GetFileName(filegb.FileName);
                    pic = eventmaster.EventId + "_" + pic;
                    string savepath = System.IO.Path.Combine(Server.MapPath("~/Content/images/gb/"), pic);
                    string path = AdminSiteConfiguration.GetURL() + "/Content/images/gb/" + pic;
                    file.SaveAs(savepath);
                    eventmaster.GoogleButtonURL = path;
                }
                catch
                {

                }
            }
            else
            {
                eventmaster.GoogleButtonURL = googleradio;
            }
            EventService.UpdateEvent(eventmaster);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UpdateEventbuttonAjax(int EventId, string linkedinradio, HttpPostedFileBase file)
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
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
                }
                catch
                {

                }
            }
            else
            {
                eventmaster.ButtonURL = linkedinradio;
            }
            EventService.UpdateEvent(eventmaster);
            return RedirectToAction("Index", "RegisteredUser", new { EventId = eventmaster.EventId });
        }

        public ActionResult Edit(int EventId = 0)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            EventMaster eventmaster = EventService.GetEventById(EventId);
            if (eventmaster == null)
            {
                return HttpNotFound();
            }
            EventMasterModel model = new EventMasterModel();
            model.CustomerId = Convert.ToInt32(Session["CustomerId"]);
            model.EventId = eventmaster.EventId;
            model.EventName = eventmaster.EventName;
            model.EventURL = eventmaster.DomainName;
            model.Description = eventmaster.Description;
            model.ResponseURL = eventmaster.ResponseURL;
            model.Commentary = eventmaster.Commentary;
            model.ArticalUrl = eventmaster.ArticalUrl;
            model.ArticalTitle = eventmaster.ArticalTitle;
            model.EventStartDate = eventmaster.EventStartDate.Date.ToShortDateString();
            model.EventEndDate = eventmaster.EventEndDate.Date.ToShortDateString();
            model.EventStartDateString = eventmaster.EventStartDate.ToShortDateString();
            model.EventEndDateString = eventmaster.EventEndDate.ToShortDateString();
            model.ContactPersonName = eventmaster.ContactPersonName;
            model.ContactPersonPhone = eventmaster.ContactPersonPhone;
            model.ContactPersonEmail = eventmaster.ContactPersonEmail;
            model.ImageName = eventmaster.LogoUrl;
            model.EventKey = eventmaster.EventKey;
            if (eventmaster.LogoUrl != null)
            {
                string path = "../Images/EventLogo/" + eventmaster.LogoUrl;
                model.Image = path;
            }

            if (eventmaster.ButtonURL != null)
            {
                model.ButtonURL = eventmaster.ButtonURL;
            }
            if (eventmaster.FbButtonURL != null)
            {
                model.FbButtonURL = eventmaster.FbButtonURL;
            }
            if (eventmaster.GoogleButtonURL != null)
            {
                model.GoogleButtonURL = eventmaster.GoogleButtonURL;
            }
            ViewBag.EventId = eventmaster.EventId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EventMasterModel model, HttpPostedFileBase file, HttpPostedFileBase filebtn)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            EventMaster eventmaster = EventService.GetEventById(model.EventId);
            eventmaster.EventName = model.EventName;
            eventmaster.DomainName = model.EventURL;
            eventmaster.Description = model.Commentary;
            eventmaster.ResponseURL = model.ResponseURL;
            eventmaster.Commentary = model.Commentary;
            eventmaster.ArticalUrl = model.ArticalUrl;
            //eventmaster.ArticalTitle = model.ArticalTitle;
            eventmaster.LogoUrl = model.ImageName;
            eventmaster.ButtonURL = model.ButtonURL;
            eventmaster.FbButtonURL = model.FbButtonURL;
            eventmaster.GoogleButtonURL = model.GoogleButtonURL;
            eventmaster.IsDeleted = false;
            eventmaster.EventStartDate = Convert.ToDateTime(model.EventStartDate);
            eventmaster.EventEndDate = Convert.ToDateTime(model.EventEndDate);
            eventmaster.ContactPersonName = model.ContactPersonName;
            eventmaster.ContactPersonPhone = model.ContactPersonPhone;
            eventmaster.ContactPersonEmail = model.ContactPersonEmail;
            eventmaster.EventKey = model.EventKey;
            if (file != null)
            {
                try
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    pic = eventmaster.EventId + "_" + pic;
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/EventLogo"), pic);
                    file.SaveAs(path);
                    eventmaster.LogoUrl = pic;
                }
                catch
                {

                }
            }
            if (filebtn != null)
            {
                try
                {
                    string pic = System.IO.Path.GetFileName(filebtn.FileName);
                    //pic = eventMaster.EventId + "_" + pic;
                    string savepath = System.IO.Path.Combine(Server.MapPath("~/Content/images"), pic);
                    string path = AdminSiteConfiguration.GetURL() + "/Content/images/" + pic;
                    filebtn.SaveAs(savepath);
                    eventmaster.ButtonURL = path;
                }
                catch
                {

                }
            }
            EventService.UpdateEvent(eventmaster);
            ViewBag.EventId = eventmaster.EventId;
            StatusMessage = "SuccessUpdate";
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int eventid)
        {
            EventService.DeleteEvent(eventid);
            StatusMessage = "SuccessDelete";
            return RedirectToAction("Grid");
        }

        [HttpPost, ActionName("DeleteEvetnAjax")]
        public ActionResult DeleteEvetnAjax(int eventid)
        {
            EventService.DeleteEvent(eventid);
            StatusMessage = "SuccessDelete";
            return Json(new { success = true });
        }

        [HttpGet] // Remove Logo
        public ActionResult Remove(int EventId)
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
            string path = System.IO.Path.Combine(Server.MapPath("~/Images/EventLogo"), eventmaster.LogoUrl);
            eventmaster.LogoUrl = null;
            eventmaster.ButtonURL = null;
            System.IO.File.Delete(path);
            EventService.UpdateEvent(eventmaster);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet] // Remove Linkedin button
        public ActionResult Removebtn(int EventId)
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
            eventmaster.ButtonURL = null;
            EventService.UpdateEvent(eventmaster);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateEventForm(int EventId)
        {
            ViewBag.EventId = EventId;
            return View();
        }

        [HttpPost]
        public ActionResult GetEventKey(int EventId)
        {
            var eventdata = EventService.GetEventById(EventId);
            return Json(new { EventKey = eventdata.EventKey, status = "success", FormCode = eventdata.FormBuilderCode });
        }

        [HttpPost]
        public ActionResult UpdateZoomToken(string token)
        {
            EventService.UpdateZoomTokenEvent(token, Convert.ToInt32(Session["CustomerId"]));
            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult UpdateBigMarkerToken(string token)
        {
            EventService.UpdateBigMarkerTokenEvent(token);
            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult SaveFormJson(int EventId, string FormCode)
        {
            if (FormCode == null || FormCode == "")
            {
                return Json(new { status = "fail", message = "Please create form" });
            }
            try
            {
                var eventdata = EventService.GetEventById(EventId);
                eventdata.FormBuilderCode = FormCode;
                EventService.UpdateEvent(eventdata);
                return Json(new { status = "success", message = "Please create form" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "fail", message = "Could not create form" });
            }
        }

        public ActionResult EmailToDeveloper()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EmailToDeveloper(String Email, string eventcode, string EventId, string eventname)
        {
            try
            {
                string subject = eventname + " - Event Code";
                sendcode(Email, subject, eventcode, EventId, "", "");
                return Json(new { status = true, message = "Email send successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.ToString() });
            }

        }

        public void sendcode(String Email, string subject, string eventcode, string EventId, string firstname, string eventname)
        {
            try
            {
                EventMaster eventmaster = EventService.GetEventById(Convert.ToInt32(EventId));

                string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
                string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
                string body = "<tr>";
                if (subject == "Your event is successfully created with EventNX")
                {
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>  Dear " + firstname + ", </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>Congratulations! Your event <b>" + eventname + "</b> is successfully created. Please find attached instructions to integrate the code in your event website in order to activate EventNX social registration. </p>";
                    body = body + "<br/> " + eventcode + "<br/>";
                    body = body + "<h2 style='font-family: Arial, Helvetica, sans-serif; font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'></h2>";
                    body = body + "<h2 style='font-family: Arial, Helvetica, sans-serif; font-size:22px; color:#333333; margin:0px; padding:0px 0px 16px 0px;'>Did you know?</h2>";
                    body = body + "<ul style='margin: 0px; padding: 0px 0px 20px 15px;'>";
                    body = body + "<li style='font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; padding-bottom: 7px; text-decoration: none;'>You can check reports of registered users in real time in EventNX dashboard </ li > ";
                    body = body + "<li style='font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; padding-bottom: 7px;'>Users will get personalized trackable links to be shared and promote your event.</li>";
                    body = body + "<li style='font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; padding-bottom: 7px;'>You can also check real time reports of referred users and who got maximum referrals.</ li > ";
                    body = body + "<li style='font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; padding-bottom: 7px;'>You can also see users who did not successfully complete the registration process.</ li ></ul> ";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>If you have any questions you can contact us at contact @eventnx.com.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 0px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'><b>Thanks and Cheers,</b><br />The Entire EventNX team </p>";
                    
                }
                else
                {
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>  Dear User, </p>";
                    body = body +  eventcode;
                    body = body + "</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Cheers, <br />" + LinkedInDemo.Class.AdminSiteConfiguration.SiteName + " Team</p>";
                    body = body + "</td></tr>";
                }

                string mailbody = header + body + footer;
                SendMailService.SendMail(Email, subject, mailbody);

            }
            catch (Exception ex)
            {

            }

        }


        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public ActionResult InviteUser(int EventId = 0 )
        {
            EventMaster eventmaster = EventService.GetEventById(EventId);
            EventMasterModel model = new EventMasterModel();
            model.EventId = EventId;
            return View(model);
        }
        
        [HttpPost]
        public ActionResult InviteUser(EventMasterModel model)
        {
          
            try
            {
                var customer = CustomerService.CustomerEmails().Where(x=> x.Email==model.Email).FirstOrDefault();
                
                if (customer !=null)
                {
                    ////EventUser eventUser = new EventUser();
                    ////eventUser.EventId = model.EventId;
                    ////eventUser.CustomerId = customer.CustomerId;
                    ////eventUser.CreatedDate = DateTime.Now;
                    ////eventUser.IsDeleted = false;
                    ////var userexists = CustomerService.CheckCustomerExists(eventUser.CustomerId,eventUser.EventId);
                    ////if(userexists == null)
                    ////{
                    //    CustomerService.InsertEventUser(eventUser);
                    //    string eventname = EventService.GetEventById(model.EventId).EventName;
                    //    sendemailtoinviteuser(model.Email, "You are invited to Manage an Event on EventNX", model.EventId.ToString(), eventname, "", true);
                    //    ViewBag.succesmessage= "Invitation sent successfully";
                    //    model.Email = "";
                    //    return View(model);
                    //}
                    //else
                    //{
                        ViewBag.errormessage="This user already Exists";
                        model.Email = "";
                        return View(model);
                    //}
                   
                }
                else
                {
                        Customer cust = new Customer();
                        cust.FirstName = "";
                        cust.LastName = "";
                        cust.Email = model.Email;
                        cust.Contactno = "";
                        cust.Username = model.Email;
                        cust.Password = RandomString(8, true);
                        cust.CompanyName = "";
                        cust.Date_of_Registration = DateTime.Now;
                        cust.IsActive = true;
                        cust.createddate = DateTime.Now;
                        cust.updateddate = DateTime.Now;
                        cust.ActivationCode = Guid.NewGuid();
                        CustomerService.InsertCustomer(cust);

                        CustomerRole custrole = new CustomerRole();
                        custrole.CustomerId = cust.CustomerId;
                        custrole.RoleId = 1;
                        CustomerService.InsertCustomerRole(custrole);
                        
                        EventUser eventuser = new EventUser();
                        eventuser.EventId = model.EventId;
                        TempData["EventId"] = eventuser.EventId;
                        eventuser.CustomerId = cust.CustomerId;
                        eventuser.CreatedDate = DateTime.Now;
                        eventuser.IsDeleted = false;
                        CustomerService.InsertEventUser(eventuser);
                        string eventname = EventService.GetEventById(model.EventId).EventName;
                        sendemailtoinviteuser(model.Email, "You are invited to Manage Event", model.EventId.ToString(), eventname, cust.Password, false);
                        ViewBag.succesmessage = "Invited Successfully";
                        return View(model);
                }

            }
            catch (Exception ex)
            {
                
            }


            return RedirectToAction("InviteUser", new { EventId = model.EventId});
        }

        [HttpPost, ActionName("Deleteuser")]
        public ActionResult Deleteuser(int id,int EventId)
        {
            EventService.DeleteUser(id);
            ViewBag.succesmessage = "User Deleted Successfully";
            return RedirectToAction("usergrid", new { Event = EventId });
        }
        public void sendemailtoinviteuser(String Email, string subject, string EventId,string eventname , string password , Boolean existinguser)
        {
            try
            {
                var currentcustomername = CustomerService.GetCustomerById(Convert.ToInt32(Session["CustomerId"]));
                EventMaster eventmaster = EventService.GetEventById(Convert.ToInt32(EventId));
                var url = string.Format("/Account/Login/");
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
                string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
                string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
                string body = "<tr>";
                if (existinguser==true)
                {
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>  Dear User, </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>You are invited to access  <b>" + eventname + "</b> event by <b>"+currentcustomername.FirstName +"  "+ currentcustomername.LastName +"</b>. </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Please click on the link below to login to EventNX and start accessing details about the event.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><a href='" + link + "'><button> Login To Eventnx </button></a></p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'>You can update the event details as well as access real time Information about attendee registrations and various reports.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Enjoy your experience with EventNX.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Thanks and Cheers,<br />";
                    body = body + "The Entire EventNX team </p>";
                }
                else
                {
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><b>Dear</b> User, </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>You are invited to access  <b>" + eventname + "</b> event by <b>" + currentcustomername.FirstName + "  " + currentcustomername.LastName + "</b>. </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'> Below are the Credentials to Login </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><b> Username: </b> " + Email + "</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><b>Password :</b> " + password + " </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'> Please click on the link below to login to EventNX and start accessing details about the event.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'><a href='" + link + "'><button style='background: #6F1855; color:#fff; border:none; padding:9px 20px; cursor: pointer;'> Login To Eventnx </button></a>";
                    body = body + "</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>You can update the event details as well as access real time Information about attendee registrations and various reports.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Enjoy your experience with EventNX.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>Thanks and Cheers,<br />";
                    body = body + "The Entire EventNX team </p>";
                }
                string mailbody = header + body + footer;
                SendMailService.SendMail(Email, subject, mailbody);

            }
            catch (Exception ex)
            {

            }

        }

        //[HttpPost]

        //called next
        //public ActionResult Download(string filename, string contenttype, string data)
        //{
        //    string path = Server.MapPath(string.Format("~/temp/{0}.json", id));
        //    string json = System.IO.File.ReadAllText(path);
        //    System.IO.File.Delete(path);
        //    Apple[] apples = new JavaScriptSerializer().Deserialize<Apple[]>(json);

        //    // work with apples to build your file in memory
        //    byte[] file = createPdf(apples);

        //    Response.AddHeader("Content-Disposition", "attachment; filename=juicy.pdf");
        //    return File(file, "application/pdf");
        //}


    }
}