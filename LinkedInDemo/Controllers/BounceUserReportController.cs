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
        public class BounceUserReportController : Controller
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
        BounceUserModel model = new BounceUserModel();

            public BounceUserReportController()
            {
                _mongodbHelper = new MongodbHelper();
            }
            public ActionResult Index(int EventId = 0, int UserId = 0, int usercount = 0)
            {
                ViewBag.EventId = EventId;
                model.ReferralId = UserId;
                if (EventId > 0)
                {
                    Session["EventId"] = EventId;
                    model.EventId = EventId;
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
                {
                    BindData = GetData(SearchRecords, SearchTitle, Alpha, Domain, Event, ReferralId).OfType<BounceUserModel>().ToList();
                    TotalDataCount = BindData.OfType<BounceUserModel>().ToList().Count();
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
                Session["eventid"] = Event;
                TempData["eventid"] = Event;
                ViewBag.ReferralId = ReferralId;
                ViewBag.TotalRegistration = TotalDataCount;
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            ViewBag.TotalCount = RegisteredUserService.BounceUsers(Event,CustomerId , SearchTitle).Count();
            var ColumnName = typeof(BounceUserModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
                IEnumerable Data = null;

                if (IsAsc == 1)
                {
                    ViewBag.AscVal = 0;
                    Data = BindData.OfType<BounceUserModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
                }
                else
                {
                    ViewBag.AscVal = 1;

                    Data = BindData.OfType<BounceUserModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
                }

                StatusMessage = "";
                Message = "";
            //List<SelectListItem> domainlist = getdomain();
            //ViewBag.domainlist = domainlist;
            List<SelectListItem> eventlist = getevent(CustomerId);
            ViewBag.eventlist = eventlist;
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

                List<BounceUserModel> bounceuserslist = new List<BounceUserModel>();
                var bounceuser = RegisteredUserService.BounceUsers(Event, CustomerId,SearchTitle);
                foreach (var user in bounceuser)
                {
                BounceUserModel model = new BounceUserModel();
                    model.UserId = user.UserId;
                    model.EventId = user.EventId;
                    model.EventName = user.EventName;
                    model.ReferralId = user.ReferalId;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.Email = user.Email;
                    model.DateOfRegistration = user.DateOfRegistration;
                    model.ReferredBy = user.RefferedBy;
                    model.Source = user.Source;
                    bounceuserslist.Add(model);
                }
                return bounceuserslist;
            }
        private static List<SelectListItem> getevent(int CustomerId)
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            List<SelectListItem> eventlist = (from p in entities.EventMasters.AsEnumerable().Where(a => a.CustomerId == CustomerId).GroupBy(a => a.EventName)
                                              select new SelectListItem
                                              {
                                                  Text = p.Select(a => a.EventName).FirstOrDefault(),
                                                  Value = p.Select(a => a.EventId).FirstOrDefault().ToString()
                                              }).ToList();

            eventlist.Insert(0, new SelectListItem { Text = "--Select Event--", Value = "0" });
            return eventlist;
        }
        public ActionResult ExporttoExcel()
            {

                var exportbounceuserdetails = RegisteredUserService.BounceUsers(Convert.ToInt32(Session["EventId"]), Convert.ToInt32(Session["CustomerId"]), Convert.ToString(TempData["SearchTitle"]));
                List<ExportBounceUserModel> ExportBounceUserModel = new List<Models.ExportBounceUserModel>();
                foreach (var item in exportbounceuserdetails)
                {
                    var model = new ExportBounceUserModel();
                    model.Id = item.UserId;
                    model.Name = item.FirstName + item.LastName;
                    model.ReferredBy = item.RefferedBy;
                    model.Email = item.Email;
                    model.EventName = item.EventName;
                    model.DateOfRegistration = item.DateOfRegistration;
                    ExportBounceUserModel.Add(model);
                }

                GridView gv = new GridView();
                gv.DataSource = ExportBounceUserModel;
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

