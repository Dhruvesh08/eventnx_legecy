using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using LinkedInDemo.Models;
using System.Data.Entity;
using System.Collections;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ManageEventsController : Controller
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
        private EventRegistrationEntities db = new EventRegistrationEntities();

        public ActionResult Index( ManageEventModel model)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            return View(model);
        }
        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "EventName", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "")
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
                BindData = GetData(SearchRecords, SearchTitle, Alpha).OfType<ManageEventModel>().ToList();
                TotalDataCount = BindData.OfType<ManageEventModel>().ToList().Count();

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
            ViewBag.manageusercount = TotalDataCount;
            var ColumnName = typeof(ManageEventModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            //Response.Write(ColumnName);
            //Response.End();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<ManageEventModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<ManageEventModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
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
        private IEnumerable GetData(int SearchRecords, string SearchTitle, string Alpha)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            List<ManageEventModel> Customerlist = new List<ManageEventModel>();
            var customerdata = ManageCustomerService.ManageEvents(0, SearchTitle);
            foreach (var data in customerdata)
            {
                ManageEventModel model = new ManageEventModel();
                model.CustomerName = data.Name;
                model.EventName = data.EventName;
                model.EventStartDate = data.EventStartDate;
                model.EventEndDate = data.EventEndDate;
                model.RegisteredUser = data.TotalCount;
                model.ReferredUser = data.ReferredUsers;
                Customerlist.Add(model);
             }

            return Customerlist;

        }

        public ActionResult ExporttoExcel()
        {
            var customerdata = ManageCustomerService.ManageEvents(0, Convert.ToString(TempData["SearchTitle"]));
            List<ManageEventModel> ManageEventModel = new List<Models.ManageEventModel>();
            foreach (var item in customerdata)
            {
                var model = new ManageEventModel();
                model.CustomerName = item.Name;
                model.EventName = item.EventName;
                model.EventStartDate = item.EventStartDate;
                model.EventEndDate = item.EventEndDate;
                model.RegisteredUser = item.TotalCount;
                model.ReferredUser = item.ReferredUsers;
                ManageEventModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ManageEventModel;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ManageEvent.xls");
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