using BussinessLayer;
using DataLayer;
using LinkedInDemo.CustomAuthentication;
using LinkedInDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class OrderController : Controller
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
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "CustomerId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string Alpha = "", string SearchTitle = "", int CustomerId = 0, int OrderId =0)
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
                        if (a1.Split(':')[0].ToString() == "OrderId")
                        {
                            OrderId = Convert.ToInt32(a1.Split(':')[1].ToString());
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
                ,"OrderId:" + OrderId

            };

            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                BindData = GetData(SearchRecords, SearchTitle, Alpha, CustomerId, OrderId).OfType<OrderModel>().ToList();
                TotalDataCount = BindData.OfType<OrderModel>().ToList().Count();
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
            ViewBag.TotalOrder = TotalDataCount;
            ViewBag.OrderId = OrderId;

            var ColumnName = typeof(OrderModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<OrderModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<OrderModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
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

        private IEnumerable GetData(int SearchRecords, string SearchTitle, string Alpha, int CustomerId , int OrderId)
        {

            CustomerId = Convert.ToInt32(Session["CustomerId"]);
             
            List<OrderModel> orderlist = new List<OrderModel>();
            var orderdetails = OrderService.getOrderDetails( CustomerId, OrderId);
            foreach (var orderrec in orderdetails)
            {
                OrderModel orderModel = new OrderModel();
                orderModel.OrderId = orderrec.OrderId;
                orderModel.FirstName = orderrec.FirstName;
                orderModel.LastName = orderrec.LastName;
                orderModel.OrderTotal = orderrec.OrderTotal;
                orderModel.OrderStatus = orderrec.OrderStatus;
                orderModel.OrderCompletedDate = orderrec.OrderCompletedDate;
                orderModel.Remarks = orderrec.Remarks;
                //orderModel.TransactionId = orderrec.TransactionId.Value;
                orderlist.Add(orderModel);
            }
            return orderlist;
        }

      
       public ActionResult PrintInvoice( int OrderId = 0)
        {
            OrderModel ordermodel = new OrderModel();
            Order order = OrderService.GetOrderById(OrderId);
            ordermodel.OrderId = order.OrderId;
            ordermodel.OrderCompletedDate = order.OrderCompletedDate;
            ordermodel.OrderStatus = order.OrderStatus;
            ordermodel.CustomerId = order.CustomerId;
            ordermodel.OrderTotalWithoutTax = order.OrderTotalWithoutTax;
            ordermodel.TaxAmount = order.TaxAmount;
            ordermodel.OrderTotal = order.OrderTotal;
            var customer = CustomerService.GetCustomerById(ordermodel.CustomerId);
            ordermodel.address1 = customer.Address1;
            ordermodel.address2 = customer.Address2;
            ordermodel.Pincode = Convert.ToInt32(customer.Pincode);
            ordermodel.City = customer.City;
            ordermodel.State = customer.State;
            ordermodel.Country = customer.Country;
            var adminsetting = AdminService.GetAdminSetting();
            ordermodel.companyaddress1 = adminsetting.Address1;
            ordermodel.companyaddress2 = adminsetting.Adddress2;
            ordermodel.companyPincode = Convert.ToInt32(adminsetting.Pincode);
            ordermodel.companyCity = adminsetting.City;
            ordermodel.companyState = adminsetting.State;
            ordermodel.companyCountry = adminsetting.Country;
            ordermodel.companyemail = adminsetting.CompanyEmail;
            ordermodel.companywebsite = adminsetting.CompanyWebsite;
            ordermodel.Email = customer.Email;
            ordermodel.Contactno = customer.Contactno;
          
            return View(ordermodel);
        }
    }
    
}