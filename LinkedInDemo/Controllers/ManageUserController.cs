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
    public class ManageUserController : Controller
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
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");

            }

            return View();
        }
        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "CustomerId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "")
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
                BindData = GetData(SearchRecords, SearchTitle, Alpha).OfType<CustomerModel>().ToList();
                TotalDataCount = BindData.OfType<CustomerModel>().ToList().Count();

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
            ViewBag.manageusercount = TotalDataCount;
            var ColumnName = typeof(CustomerModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            //Response.Write(ColumnName);
            //Response.End();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<CustomerModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<CustomerModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
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
            List<CustomerModel> Customerlist = new List<CustomerModel>();
            var customerdata = ManageCustomerService.CustomerDetails(0, Alpha);
            foreach (var data in customerdata)
            {
                CustomerModel model = new CustomerModel();
                model.CustomerId = data.CustomerId;
                model.FirstName = data.FirstName;
                model.LastName = data.LastName;
                model.Email = data.Email;
                model.Contactno = data.Contactno;
                model.Username = data.Username;
                model.Password = data.Password;
                model.CompanyName = data.CompanyName;
                model.Date_of_Registration = data.Date_of_Registration;
                model.NoOfEvents = data.No_of_events.Value;
                model.AvailableCredits = data.Availablecredits.Value;
                Customerlist.Add(model);
             }

            return Customerlist;

        }

        public ActionResult Create()
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            CustomerModel model = new CustomerModel();
            model.Date_of_Registration = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CustomerModel model)
        {

            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {

                var matchuser = db.Customers.Where(a => a.Username.Equals(model.Username)).FirstOrDefault();
                if (matchuser == null)
                {
                    Customer customer = new Customer();
                    customer.CustomerId = model.CustomerId;
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.Email = model.Email;
                    customer.Contactno = model.Contactno;
                    customer.Username = model.Username;
                    customer.Password = model.Password;
                    customer.CompanyName = model.CompanyName;
                    customer.Date_of_Registration = DateTime.Now;
                    customer.IsActive = true;
                    customer.createddate = DateTime.Now; 
                    customer.updateddate = DateTime.Now;
                   
                    ViewBag.CustomerId = customer.CustomerId;
                    ViewBag.StatusMessage = "SuccessAdd";


                    var role = RoleService.GetRoleByName("User");
                    CustomerRole customerrole = new CustomerRole();
                    if (role != null)
                    {
                        customerrole.RoleId = role.RoleId;
                        customerrole.CustomerId = customer.CustomerId;
                    }
                    customer.CustomerRoles.Add(customerrole);
                    ManageCustomerService.InsertCustomer(customer);


                    //Transaction
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
                }
                else
                {
                    ViewBag.Message = "  This user is already exists";
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult Edit(int CustomerId = 0)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Customer customer = ManageCustomerService.GetCustomerById(CustomerId);
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
            model.Password = customer.Password;
            model.CompanyName = customer.CompanyName;
            model.Date_of_Registration = customer.Date_of_Registration;
          
            model.IsActive = customer.IsActive;
            model.AccountStatus = customer.IsActive;


            ViewBag.CustomerId = model.CustomerId;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CustomerModel model)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Customer customer = ManageCustomerService.GetCustomerById(model.CustomerId);
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Contactno = model.Contactno;
            customer.Username = model.Username;
            customer.Password = model.Password;
            customer.CompanyName = model.CompanyName;
            customer.Date_of_Registration = model.Date_of_Registration;

            customer.CustomerId = model.CustomerId;
            customer.IsActive = model.AccountStatus;
            //customer.createddate = DateTime.Now;
            //customer.updateddate = DateTime.Now;
            ManageCustomerService.UpdateCustomer(customer);

            ViewBag.StatusMessage = "SuccessUpdate";
            return View(model);

        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int CustomerId)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ManageCustomerService.DeleteCustomer(CustomerId);
            return RedirectToAction("Grid");
        }

        public ActionResult ChangePassword( int CustomerId , string Password)
        {
            var rsult="";
            try
            {
                Customer customer = ManageCustomerService.GetCustomerById(CustomerId);

                customer.Password = Password;
                ManageCustomerService.UpdateCustomer(customer);
                 rsult = "Password Saved Successfully";
            }
            catch
            {
                rsult = "Try again";
            }
            
           
            return Json(rsult);
        }

        public ActionResult ExporttoExcel()
        {
            var customerdata = ManageCustomerService.CustomerDetails(0, "");
            List<ExportManageUserModel> ExportManageUserModel = new List<Models.ExportManageUserModel>();
            foreach (var item in customerdata)
            {
                var model = new ExportManageUserModel();
                model.CustomerNo = item.CustomerId;
                model.CustomerName = item.FirstName + item.LastName;
                model.Email = item.Email;
                model.DateofRegistration = item.Date_of_Registration;
                model.AvailableCredits = item.Availablecredits;
                model.NoOfEvents = item.No_of_events;
                ExportManageUserModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ExportManageUserModel;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ManageUser.xls");
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