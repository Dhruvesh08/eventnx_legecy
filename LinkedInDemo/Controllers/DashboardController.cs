using DataLayer;
using LinkedInDemo.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;
using System.Web.Security;
using System.Collections;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class DashboardController : Controller
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
        [CustomAuthorize(Roles = "User,Admin")]

        public ActionResult Index(int EventId = 0)
        {

            DashboardModel model = new DashboardModel();
            var customer = CustomerService.GetCustomerById(Convert.ToInt32(Session["CustomerId"]));
            model.TotalEvent = CustomerService.GetEventsCount(customer.CustomerId);
            var eventId = RegisteredUserService.GetEventIdByCustomerId(Convert.ToInt32(Session["CustomerId"]));
            var customerId = EventService.geteventdetailbyeventId(eventId);
            if(eventId == 0)
            {
                model.AvailableCredits = Convert.ToInt32(CustomerService.GetCustomerAvailableCredit(Convert.ToInt32(Session["CustomerId"])));
            }
            else
            {
                model.AvailableCredits = Convert.ToInt32(CustomerService.GetCustomerAvailableCredit(customerId));
            }
           
            model.RegisteredUser = RegisteredUserService.GetRegisteredUserByCustomerId(customer.CustomerId);
            model.LinkedinUser = RegisteredUserService.GetLinkedinUserByCustomerId(customer.CustomerId);
            model.FacebookUser = RegisteredUserService.GetFacebookUserByCustomerId(customer.CustomerId);
            model.Name = CustomerService.GetCustomerByName(customer.FirstName, customer.LastName);
            if (model.Name != null)
            {
                ViewBag.Name = model.Name;
            }
            var eventuser = EventService.GetEventusersByCustomerId(Convert.ToInt32(Session["CustomerId"]));
            if (eventuser != null)
            {
                ViewBag.EventUserData = "false";
            }
            else
            {
                ViewBag.EventUserData = "true";
            }
            // SendMailService.SendMail("dharamistry@meghtechnologies.com", "Test", "Test");
            return View(model);
        }
        [CustomAuthorize(Roles = "User")]
        public ActionResult Test()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Admindashboard()
        {
            DashboardModel model = new DashboardModel();
            model.TotalEvent = EventService.GetAllEvents().Count();
            model.RegisteredUser = RegisteredUserService.GetAllRegisteredUser().Count();
            model.TotalCustomers = CustomerService.GetTotalCustomers();
            model.TotalPurchase = CustomerService.GetTotalPurchase();
            model.ActiveCustomers = CustomerService.GetActiveCustomers();

            return View(model);
        }
        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", int CustomerId = 0)
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
                        if (a1.Split(':')[0].ToString() == "CustomerId")
                        {
                            CustomerId = Convert.ToInt32(a1.Split(':')[1].ToString());
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
                ,"CustomerId:" + CustomerId

            };

            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                BindData = GetData(SearchRecords, SearchTitle, Alpha, CustomerId).OfType<EventMasterModel>().ToList();
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

            var ColumnName = typeof(EventMasterModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            Data = BindData.OfType<EventMasterModel>().ToList().Skip(startIndex - 1).Take(PageSize);
            //if (IsAsc == 1)
            //{
            //    ViewBag.AscVal = 0;
            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}
            //else
            //{
            //    ViewBag.AscVal = 1;

            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}

            StatusMessage = "";
            Message = "";

            return View(Data);
        }

        private int getLastPageIndex(int PageSize)
        {
            int lastPageIndex = Convert.ToInt32(TotalDataCount) / PageSize;
            if (TotalDataCount % PageSize > 0)
                lastPageIndex += 1;

            return lastPageIndex;
        }

        private IEnumerable GetData(int SearchRecords, string SearchTitle, string Alpha, int CustomerId)
            {

            CustomerId = Convert.ToInt32(Session["CustomerId"]);

            List<EventMasterModel> eventlist = new List<EventMasterModel>();
            var eventdetails = CustomerService.GetCustomerEvents(SearchTitle, Alpha, CustomerId,"");
            foreach (var eventrec in eventdetails)
            {
                EventMasterModel eventMasterModel = new EventMasterModel();
                eventMasterModel.EventId = eventrec.EventId;
                eventMasterModel.EventName = eventrec.EventName;
                eventMasterModel.EventURL = eventrec.DomainName;
                eventMasterModel.Description = eventrec.Description;
                eventMasterModel.EventStartDate = eventrec.EventStartDate.ToShortDateString();
                eventMasterModel.EventEndDate = eventrec.EventEndDate.ToShortDateString();
                EventMaster eventmaster = EventService.GetEventById(eventrec.EventId);
                string path = "../Images/EventLogo/" + eventmaster.LogoUrl;
                eventMasterModel.Image = path;
                eventMasterModel.ImageName = eventrec.LogoUrl;
                eventMasterModel.ContactPersonName = eventrec.ContactPersonName;
                eventMasterModel.ContactPersonPhone = eventrec.ContactPersonPhone;
                eventMasterModel.ContactPersonEmail = eventrec.ContactPersonEmail;
                eventMasterModel.RegisteredUsers = Convert.ToInt32(eventrec.TotalCount);
                eventMasterModel.Linkedinusers = Convert.ToInt32(eventrec.LinkedinUsers);
                eventMasterModel.Facebookusers = Convert.ToInt32(eventrec.FacebookUsers);
                eventMasterModel.Referralusers = RegisteredUserService.GetReferralIByEventId(eventrec.EventId);
                eventlist.Add(eventMasterModel);
            }
            return eventlist;
        }

        public ActionResult GridRegisteredUser(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", int CustomerId = 0)
        {
            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                //BindData = GetDataRegisteredUser(SearchRecords, SearchTitle, Alpha, CustomerId).OfType<EventMasterModel>().ToList();
                BindData = GetDataRegisteredUser().OfType<RegisteredUserModel>().ToList();
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
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.TotalEvent = TotalDataCount;

            var ColumnName = typeof(RegisteredUserModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            Data = BindData.OfType<RegisteredUserModel>().ToList().OrderByDescending(x => x.DateOfRegistration).Skip(startIndex - 1).Take(PageSize);
            //if (IsAsc == 1)
            //{
            //    ViewBag.AscVal = 0;
            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}
            //else
            //{
            //    ViewBag.AscVal = 1;

            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}

            StatusMessage = "";
            Message = "";

            return View(Data);
        }
        private IEnumerable GetDataRegisteredUser()
        {
            List<RegisteredUserModel> userlist = new List<RegisteredUserModel>();
            int Customer_Id = Convert.ToInt32(Session["CustomerId"]);

            var regusers = RegisteredUserService.SearchRegisteredUserByCustomer("", "", "", 0, 0, Customer_Id);
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
                userlist.Add(registeredUserModel);
            }
            return userlist;
        }
        public ActionResult GridTopReferrer(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", int CustomerId = 0)
        {
            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                //BindData = GetDataRegisteredUser(SearchRecords, SearchTitle, Alpha, CustomerId).OfType<EventMasterModel>().ToList();
                BindData = GetTopReferrer().OfType<RegisteredUserModel>().ToList();
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
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.TotalEvent = TotalDataCount;

            var ColumnName = typeof(RegisteredUserModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            Data = BindData.OfType<RegisteredUserModel>().ToList().OrderByDescending(x => x.ReferralCount).Skip(startIndex - 1).Take(PageSize);
            //if (IsAsc == 1)
            //{
            //    ViewBag.AscVal = 0;
            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}
            //else
            //{
            //    ViewBag.AscVal = 1;

            //    Data = BindData.OfType<EventMasterModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            //}

            StatusMessage = "";
            Message = "";

            return View(Data);
        }
        private IEnumerable GetTopReferrer()
        {
            List<RegisteredUserModel> userlist = new List<RegisteredUserModel>();
            int Customer_Id = Convert.ToInt32(Session["CustomerId"]);
            var regusers = RegisteredUserService.SearchRegisteredUserByCustomer("", "", "", 0, 0, Customer_Id);
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
                userlist.Add(registeredUserModel);
            }
            return userlist;
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int eventid)
        {
            EventService.DeleteEvent(eventid);
            RegisteredUserService.DeleteEventRegisterdUser(eventid);
            StatusMessage = "SuccessDelete";
            return Json(new { success = true });
        }
    }
}