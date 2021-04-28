using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using LinkedInDemo.Models;
using System.Data.Entity;
using System.Collections;
using Order = DataLayer.Order;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class PackagemasterController : Controller
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "PackageId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "")
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
                BindData = GetData(SearchRecords, SearchTitle, Alpha).OfType<PackagemasterModel>().ToList();
                TotalDataCount = BindData.OfType<PackagemasterModel>().ToList().Count();
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
            ViewBag.packagecount = TotalDataCount;
            var ColumnName = typeof(PackagemasterModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            //Response.Write(ColumnName);
            //Response.End();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<PackagemasterModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<PackagemasterModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
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
           
            List<PackagemasterModel> packagelist = new List<PackagemasterModel>();
            var packagedata = PackageService.PackageDetails(SearchTitle, Alpha);
            foreach (var data in packagedata)
            {
                PackagemasterModel packagemasterModel = new PackagemasterModel();
                packagemasterModel.PackageId = data.PackageId;
                packagemasterModel.Name = data.Name;
                packagemasterModel.Detail = data.Detail;
                packagemasterModel.Cost = data.Cost;
                packagemasterModel.Validity = data.Validity;
                packagemasterModel.No_of_Registration = data.No_of_Registration;
                packagemasterModel.Cost_per_Registration = data.Cost_per_Registration;
                packagemasterModel.No_of_Events = data.No_of_Events;
                packagelist.Add(packagemasterModel);
            }
            return packagelist;
        }
        
        public ActionResult CreatePackage()
        {
            PackagemasterModel model = new PackagemasterModel();
            return View();
        }

        [HttpPost]
        public ActionResult CreatePackage( PackagemasterModel model)
        {
            if (ModelState.IsValid)
            {
                Package package = new Package();
                package.PackageId = model.PackageId;
                package.Name = model.Name;
                package.Detail = model.Detail;
                package.Cost = model.Cost;
                package.Validity = model.Validity;
                package.No_of_Registration = model.No_of_Registration;
                package.Cost_per_Registration = model.Cost_per_Registration;
                package.No_of_Events = model.No_of_Events;
                package.IsActive = model.IsActive;
                package.Createddate = DateTime.Now;
                package.Updateddate = DateTime.Now;
                PackageService.InsertPackage(package);
                ViewBag.Message = "Package successfully added ";
                
            }
            return View(model);
        }

        public ActionResult EditPackage( int PackageId = 0 )
        {
            Package package = PackageService.GetPackageById(PackageId);
            PackagemasterModel model = new PackagemasterModel();
            model.PackageId = package.PackageId;
            model.Name = package.Name;
            model.Detail = package.Detail;
            model.Cost = package.Cost;
            model.Validity = package.Validity;
            model.No_of_Registration = package.No_of_Registration;
            model.Cost_per_Registration = package.Cost_per_Registration.Value;
            model.No_of_Events = package.No_of_Events;
            ViewBag.PackageId = package.PackageId;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPackage( PackagemasterModel model)
        {
            Package package = PackageService.GetPackageById(model.PackageId);
            package.PackageId = model.PackageId;
            package.Name = model.Name;
            package.Detail = model.Detail;
            package.Cost = Convert.ToInt32(model.Cost);
            package.Validity = model.Validity;
            package.No_of_Registration = model.No_of_Registration;
            package.Cost_per_Registration = Convert.ToDecimal(model.Cost_per_Registration);
            package.No_of_Events = model.No_of_Events;

            PackageService.UpdatePackage(package);
            ViewBag.StatusMessage = "SuccessUpdate";
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int PackageId)
        {
            PackageService.DeletePackage(PackageId);
            return RedirectToAction("Grid");
        }

    }
}