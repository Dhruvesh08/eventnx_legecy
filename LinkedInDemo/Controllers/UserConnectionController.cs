using BussinessLayer;
using LinkedInDemo.CustomAuthentication;
using LinkedInDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Controllerss
{
    [CustomAuthorize(Roles = "User")]
    public class UserConnectionController : Controller
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
            UserConnectionModel model = new UserConnectionModel();
            public ActionResult Index(int EventId = 0, int UserId = 0)
            {
                ViewBag.EventId = EventId;
                model.ReferralId = UserId;
                if (EventId > 0)
                {
                    Session["EventId"] = EventId;
                    model.EventId = EventId;
                   
                }
                return View();
            }

            public ActionResult UserGrid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "UserId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", string Domain = "", int Event = 0, int ReferralId = 0,int UserId =0, int EventId= 0)
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
                            if (a1.Split(':')[0].ToString() == "UserId")
                            {
                                UserId = Convert.ToInt32(a1.Split(':')[1].ToString());
                            }
                            if (a1.Split(':')[0].ToString() == "EventId")
                            {
                                EventId = Convert.ToInt32(a1.Split(':')[1].ToString());
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
                ,"UserId:" + UserId
                ,"EventId" +EventId
                };

                int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
                int endIndex = startIndex + PageSize - 1;

                if (IsBindData == 1)
                {
                    BindData = GetData(UserId, EventId).OfType<UserConnectionModel>().ToList();
                    TotalDataCount = BindData.OfType<UserConnectionModel>().ToList().Count();
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
                ViewBag.Domain = Domain;
                ViewBag.Event = Event;
                ViewBag.ReferralId = ReferralId;
                ViewBag.TotalRegistration = TotalDataCount;
                var ColumnName = typeof(UserConnectionModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
                IEnumerable Data = null;

                if (IsAsc == 1)
                {
                    ViewBag.AscVal = 0;
                    Data = BindData.OfType<UserConnectionModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
                }
                else
                {
                    ViewBag.AscVal = 1;

                    Data = BindData.OfType<UserConnectionModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
                }

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

            private IEnumerable GetData(int UserId, int EventId)
            {
               
                List<UserConnectionModel> userlist = new List<UserConnectionModel>();
                var regusers = UserConnectionService.UserConnectionDetails(UserId, EventId);
                foreach (var user in regusers)
                {
                    UserConnectionModel userconnectionmodel = new UserConnectionModel();
                    userconnectionmodel.UserId = user.UserId;
                    userconnectionmodel.FirstName = user.FirstName;
                    userconnectionmodel.LastName = user.LastName;
                    userconnectionmodel.Email = user.Email;
                    userconnectionmodel.Country = user.Country;
                    userconnectionmodel.DomainName = user.DomainName;
                    userconnectionmodel.EventId = user.EventId;
                    userconnectionmodel.EventName = user.EventName;
                    userconnectionmodel.DateOfRegistration = user.DateOfRegistration;
                    userconnectionmodel.ReferralCount = Convert.ToInt32(user.ReferralCount);
                    userconnectionmodel.ConnectionCount = Convert.ToInt32(user.ConnectionCount);
                    userlist.Add(userconnectionmodel);
                }
                return userlist;
            }
        }
}