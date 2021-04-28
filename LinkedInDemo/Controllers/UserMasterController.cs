using BussinessLayer;
using DataLayer;
using LinkedInDemo.CustomAuthentication;
using LinkedInDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class UserMasterController : Controller
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
        UserMasterModel model = new UserMasterModel();
        public UserMasterController()
        {
            _mongodbHelper = new MongodbHelper();
        }
        public ActionResult Index(int EventId = 0, int UserId = 0 , int usercount=0)
        {
            ViewBag.EventId = EventId;
            model.ReferralId = UserId;
            if (EventId > 0)
            {
                Session["EventId"] = EventId;
                model.EventId = EventId;
                ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(EventId).Count();
                model.RegisteredUser = RegisteredUserService.GetRegisteredUserByEventId(EventId).Count();
                //model.Creditrequired = RegisteredUserService.Creditrequired(EventId);
                model.TotalReferences = RegisteredUserService.GetReferralId();
                model.EventName = RegisteredUserService.GetEventNamebyEventId(EventId);
                model.EventStartDate = RegisteredUserService.GetStartdatebyEventId(EventId);
                model.EventEndDate = RegisteredUserService.GetEnddatebyEventId(EventId);
                model.Image = RegisteredUserService.GetEventlogobyEventId(EventId);
                model.usercount = 0;
            }
            return View(model);
        }

        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "UserId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", string Domain = "", int Event = 0, int ReferralId = 0)
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
            {if (Event > 0)
                {
                    BindData = GetData(SearchRecords, SearchTitle, Alpha, Domain, Event, ReferralId).OfType<UserMasterModel>().ToList();

                }
                else
                {
                    Event = Convert.ToInt32(Request.QueryString["EventId"]);
                    BindData = GetData(SearchRecords, SearchTitle, Alpha, Domain, Event, ReferralId).OfType<UserMasterModel>().ToList();

                }
                TotalDataCount = BindData.OfType<UserMasterModel>().ToList().Count();
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
            
            TempData["Event"] = Event;
            ViewBag.ReferralId = ReferralId;
            ViewBag.TotalRegistration = TotalDataCount;
            ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(Event).Count();
            var ColumnName = typeof(UserMasterModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<UserMasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<UserMasterModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(Event).Count();
            StatusMessage = "";
            Message = "";
            //List<SelectListItem> domainlist = getdomain();
            //ViewBag.domainlist = domainlist;
            var eventid = Request.QueryString["EventId"];
            List<SelectListItem> eventlist = getevent(CustomerId,eventid);
            
            ViewBag.eventlist = eventlist;
            if (Event > 0)
            {
                ViewBag.Event = Event;
            }
            if(eventid!=null)
            {
                ViewBag.Event = eventid;

            }
            //var ev = new List<SelectListItem>();
            //foreach (var item in eventlist)
            //{
            //    item.Text = eventlist.Select(a => a.Text).FirstOrDefault();
            //    item.Value = eventlist.Select(a => a.Value).FirstOrDefault();
            //    model.eventList.Add(item);
            //}

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
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);

            List<UserMasterModel> userlist = new List<UserMasterModel>();
            var regusers = RegisteredUserService.Eventwiseuser( SearchTitle, Alpha, Domain, Event,CustomerId ,ReferralId);
            foreach (var user in regusers)
            {
                UserMasterModel model = new UserMasterModel();
                model.UserId = user.UserId;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.ReferredBy = user.ReferredBy;
                model.Email = user.Email;
                model.Country = user.Country;
                model.DomainName = user.DomainName;
                model.EventId = user.EventId;
                model.EventName = user.EventName;
                model.EventStartDate = user.EventStartDate;
                model.EventEndDate = user.EventEndDate;
                model.DateOfRegistration = user.DateOfRegistration;
                model.ReferralCount = Convert.ToInt32(user.ReferralCount);
                model.ConnectionCount = Convert.ToInt32(user.ConnectionCount);
                model.ProfileImage = user.ProfileImage;
                model.visitorcount = user.VisitorCount;
                model.CRM_CompanyName = user.CRM_CompanyName;
                model.CRM_JobTitle = user.CRM_JobTitle;
                model.CRM_RegistrationId = user.CRM_RegistrationId;
                model.Source = user.Source;
                userlist.Add(model);
            }
            ViewBag.TotalCount = RegisteredUserService.GetRegisteredUserByEventId(Event).Count();
            return userlist;
        }
        private static List<SelectListItem> getevent(int CustomerId,string eventid)
        {
            
            EventRegistrationEntities entities = new EventRegistrationEntities();
            var eventids = entities.EventMasters.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var eventusereventids = entities.EventUsers.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            eventids = eventids.Concat(eventusereventids).ToList();
            List<SelectListItem> eventlist = (from p in entities.EventMasters.AsEnumerable().Where(a => eventids.Contains(a.EventId)).GroupBy(a => a.EventName)
                                              select new SelectListItem
                                              {
                                                  Text = p.Select(a => a.EventName).FirstOrDefault(),
                                                  Value = p.Select(a => a.EventId).FirstOrDefault().ToString()
                                              }).ToList();
            //List<SelectListItem> eventlist = (from p in entities.EventMasters.AsEnumerable().Where(a=>a.CustomerId== CustomerId || a.EventId==Convert.ToInt32(eventid)).GroupBy(a=>a.EventName)
            //                                  select new  SelectListItem
            //                                  {
            //                                      Text = p.Select(a=>a.EventName).FirstOrDefault(),
            //                                      Value = p.Select(a=>a.EventId).FirstOrDefault().ToString(),
                                                  
            //                                  }).ToList();
            foreach (var item in eventlist)
            {
                if (item.Value== eventid)
                {
                    item.Selected = true;
                }
            }
            eventlist.Insert(0, new SelectListItem { Text = "--Select Event--", Value = "0" });
            return eventlist;
        }
        public ActionResult ExporttoExcel()
        {
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            var exportuserdetails = RegisteredUserService.Eventwiseuser(Convert.ToString(TempData["SearchTitle"]), "", "", Convert.ToInt32(TempData["Event"]), CustomerId, 0);
            List<ExportUserDetailsModel> ExportUserDetailsModel = new List<Models.ExportUserDetailsModel>();
            foreach (var item in exportuserdetails)
            {
                var model = new ExportUserDetailsModel();
                //model.EventId = item.EventId;
                model.Id = item.UserId;
                model.CRM_Id = item.CRM_RegistrationId;
                model.Name = item.FirstName +" "+ item.LastName;
                model.ReferredBy = item.ReferredBy;
                model.Email = item.Email;
                model.ReferralCount = item.ReferralCount;
                model.VisitorCount = item.VisitorCount;
                model.DateOfRegistration = item.DateOfRegistration;
                model.EventName = item.EventName;
                model.CompanyName = item.CRM_CompanyName;
                model.JobTitle = item.CRM_JobTitle;
                ExportUserDetailsModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ExportUserDetailsModel;
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
    }
}