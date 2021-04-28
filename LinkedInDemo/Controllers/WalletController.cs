using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using LinkedInDemo.Models;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;
using static LinkedInDemo.Models.TransactionsModel;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class WalletController : Controller
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

        public ActionResult Index(int CustomerId=0)
        {
            
            ViewBag.CustomerId = CustomerId;
            DashboardModel model = new DashboardModel();
            var eventId = RegisteredUserService.GetEventIdByCustomerId(Convert.ToInt32(Session["CustomerId"]));
            var customerId = EventService.geteventdetailbyeventId(eventId);
            model.AvailableCredits = Convert.ToInt32(CustomerService.GetCustomerAvailableCredit(Convert.ToInt32(Session["CustomerId"])));
            //model.AvailableCredits = Convert.ToInt32(CustomerService.GetCustomerAvailableCredit(Convert.ToInt32(Session["CustomerId"])));
            model.TotalDeposit = CustomerService.GetTotalDeposit(Convert.ToInt32(Session["CustomerId"]));
            model.CreditsUsed = Convert.ToInt32(RegisteredUserService.GetRegisteredUserByCustomerId(Convert.ToInt32(Session["CustomerId"])));
            return View(model);
        }

        public ActionResult Grid(int IsBindData = 1, int currentPageIndex = 1, string orderby = "CustomerId", int IsAsc = 0, int PageSize = 10, int SearchRecords = 1, string alpha = "", string transactiontype="", string status="",string Success="")
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
                        if (a1.Split(':')[0].ToString() == "alpha")
                        {
                            alpha = Convert.ToString(a1.Split(':')[1].ToString());
                        }
                        if (a1.Split(':')[0].ToString() == "transactiontype")
                        {
                            transactiontype = Convert.ToString(a1.Split(':')[1].ToString());
                        }

                        if (a1.Split(':')[0].ToString() == "status")
                        {
                            status = Convert.ToString(a1.Split(':')[1].ToString());
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
                ,"Alpha:" + alpha
                ,"SearchRecords:" + SearchRecords
               ,"status" + status
               ,"transactiontype" + transactiontype

            };

            int startIndex = ((currentPageIndex - 1) * PageSize) + 1;
            int endIndex = startIndex + PageSize - 1;

            if (IsBindData == 1)
            {
                BindData = GetData(SearchRecords, transactiontype, status).OfType<TransactionsModel>().ToList();
                TotalDataCount = BindData.OfType<TransactionsModel>().ToList().Count();
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
            ViewBag.Alpha = alpha;
          
            ViewBag.StatusMessage = StatusMessage;
            ViewBag.name = name;
            ViewBag.pas = pas;
            ViewBag.transactiontype = transactiontype;
            ViewBag.status = status;
            TempData["status"] = status;
            ViewBag.transactionscount =TotalDataCount;
            var ColumnName = typeof(TransactionsModel).GetProperties().Where(p => p.Name == orderby).FirstOrDefault();
         
            IEnumerable Data = null;

            if (IsAsc == 1)
            {
                ViewBag.AscVal = 0;
                Data = BindData.OfType<TransactionsModel>().ToList().OrderBy(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }
            else
            {
                ViewBag.AscVal = 1;

                Data = BindData.OfType<TransactionsModel>().ToList().OrderByDescending(n => ColumnName.GetValue(n, null)).Skip(startIndex - 1).Take(PageSize);
            }

            StatusMessage = "";
            Message = "";


            List<SelectListItem> list = getTransactiontype();
            ViewBag.list = list;
            if(Success == "true")
            {
                ViewBag.Success = "true";
                ViewBag.Message = "Credit approved successfully.";
            }
            else
            {
                if (Success == "false")
                {
                    ViewBag.Success = "false";
                    ViewBag.Message = "Credit Rejected!!";
                }
            }
           
            return View(Data);
        }

        private int getLastPageIndex(int PageSize) 
        {
            int lastPageIndex = Convert.ToInt32(TotalDataCount) / PageSize;
            if (TotalDataCount % PageSize > 0)
                lastPageIndex += 1;

            return lastPageIndex;
        }
        private IEnumerable GetData(int SearchRecords,  string transactiontype, string status)
        {

            int Id = 0;
            int CustomerId = Convert.ToInt32(Session["CustomerId"]);
            if(User.IsInRole("Admin"))
            {
                CustomerId = 0;
            }
            status = status.Trim().ToLower();
            transactiontype = transactiontype.Trim().ToLower();
            List<TransactionsModel> transactionlist = new List<TransactionsModel>();
            var transactions = TransactionService.UserTransactionDetails(Id, SearchRecords, CustomerId, transactiontype, status );

            foreach (var trans in transactions)
            {
                TransactionsModel  transactionmodel= new TransactionsModel();
                transactionmodel.Id = trans.Id;
                transactionmodel.CustomerId = trans.CustomerId;
                transactionmodel.Amount = Convert.ToInt32(trans.Amount.Value);
                transactionmodel.TransactionDate = trans.TransactionDate;
                transactionmodel.TransactionType = trans.TransactionType;
                transactionmodel.status = trans.status;
                transactionmodel.Remarks = trans.Remarks;
                transactionmodel.PaymentMethod = trans.PaymentMethod;
                transactionlist.Add(transactionmodel);
            }

            return transactionlist;
        }

        private static List<SelectListItem> getTransactiontype()
        {
            EventRegistrationEntities entities = new EventRegistrationEntities();
            var transtype = (from p in entities.Transactions.AsEnumerable()
                             select p.TransactionType).Distinct().ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            list.Insert(0, new SelectListItem { Text = "--Select TransactionType--", Value = "" });
            foreach (var item in transtype)
            {
                SelectListItem selectitem = new SelectListItem();
                selectitem.Text = item;
                selectitem.Value = item;
                list.Add(selectitem);
            }

            return list;
        }

        public ActionResult ExporttoExcel()
        {
            var exportuserdetails = (from data2 in db.GetTransactionsDetails(0,Convert.ToInt32(Session["CustomerId"]),"", Convert.ToString(TempData["status"]))
                                     select new TransactionsModel
                                     {
                                        CustomerId = data2.CustomerId,
                                         Amount = Convert.ToInt32(data2.Amount.Value),
                                         TransactionDate = data2.TransactionDate,
                                         TransactionType = data2.TransactionType,
                                         status = data2.status,
                                         TransactionId=data2.Id
                                         //PaymentMethod = data2.PaymentMethod
                                     }).ToList();
            List<ExportDetailsModel> ExportUserDetailsModel = new List<Models.ExportDetailsModel>();
            foreach (var item in exportuserdetails)
            {
                var model = new ExportDetailsModel();
                //model.CustomerId = item.CustomerId;
                model.Amount = item.Amount;
                model.TransactionDate = item.TransactionDate.ToString("dd-MMM-yyyy");
                model.status = item.status;
                //model.PaymentMethod = item.PaymentMethod;
                model.TransactionType = item.TransactionType;
                //model.CustomerId = item.Id;
                model.TransactionNumber = item.TransactionId;
                ExportUserDetailsModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ExportUserDetailsModel;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=TransactionsDetails.xls");
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

        public ActionResult ExporttoExcelAdmin()
        {
            var exportuserdetails = (from data2 in db.GetTransactionsDetails(0, 0, "", Convert.ToString(TempData["status"]))
                                     select new TransactionsModel
                                     {
                                         CustomerId = data2.CustomerId,
                                         Amount = Convert.ToInt32(data2.Amount.Value),
                                         TransactionDate = data2.TransactionDate,
                                         TransactionType = data2.TransactionType,
                                         status = data2.status,
                                         TransactionId = data2.Id,
                                         Remarks=data2.Remarks
                                         //PaymentMethod = data2.PaymentMethod
                                     }).ToList();
            List<ExportAccountDetailsModel> ExportUserDetailsModel = new List<Models.ExportAccountDetailsModel>();
            foreach (var item in exportuserdetails)
            {
                var model = new ExportAccountDetailsModel();
                model.CustomerNo = item.CustomerId;
                model.TransactionNo = item.TransactionId;
                model.Credits = item.Amount;
                model.TransactionDate = item.TransactionDate;
                model.status = item.status;
                model.Remark = item.Remarks;
                ExportUserDetailsModel.Add(model);
            }

            GridView gv = new GridView();
            gv.DataSource = ExportUserDetailsModel;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdminTransactionsDetails.xls");
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
        [HttpGet]
        public ActionResult ChangeTransactionStatus(bool IsApproved,int Id,string Remark)
        {
            var trans = TransactionService.GetTransactionById(Id);
            var evntid = EventService.geteventbycustomerId(trans.CustomerId);
            var name = CustomerService.GetAllCustomer().Where(a => a.CustomerId == trans.CustomerId).FirstOrDefault();
            if (IsApproved == true)
            {
                trans.Status = "Approved";
                trans.Remarks = Remark;
                RegisteredUserService.MarkRegisteredUserAsPaidForEvent(IsApproved, evntid, Convert.ToInt32(trans.Amount));
                sendMailtocustomer(name.Email, "Your request for credits is received", name.FirstName+ " " +name.LastName, evntid,Convert.ToInt32(trans.Amount));
                ViewBag.Success = "true";
                ViewBag.Message= "Credit approved successfully.";

            }
            else
            {
                trans.Status = "Rejected";
                trans.Remarks = Remark;
                ViewBag.Success = "false";
                ViewBag.Message = "Credit Rejected!!";
                //RegisteredUserService.MarkRegisteredUserAsPaidForEvent(IsApproved, evntid);
                // sendMailtocustomer(name.Email, "Your request has been rejected", name.FirstName + " " + name.LastName, evntid);

            }
            TransactionService.UpdateTransaction(trans);
            return RedirectToAction("grid",new { Success = ViewBag.Success });
            //return Json(new { response= trans,success = true, JsonRequestBehavior.AllowGet });
        }
        //public ActionResult UpdateComments(string Id, string Remark)
        //{
        //    var numbers = Id.Split(',').Select(Int32.Parse).ToList();
        //    var tremarks = Remark.Split(',').ToList();
        //    var i = 0;
        //    foreach (var item in numbers)
        //    {
                    
        //            var chckid = TransactionService.GetTransactionById(item);
        //            chckid.Remarks = tremarks[i].ToString();
        //            TransactionService.UpdateTransaction(chckid);
        //            i++;
                
        //    }
        //    return RedirectToAction("grid", new { Success = true });
        //}\
        [HttpPost]
        public ActionResult UpdateComments(List<TransactionsModel> RemarkModel)
        {
            foreach (var item in RemarkModel)
            {
                var chckid = TransactionService.GetTransactionById(item.Id);
                chckid.Remarks = item.Remarks;
                TransactionService.UpdateTransaction(chckid);
            }
            return RedirectToAction("grid", new { Success = true });
        }
        public void sendMailtocustomer(String Email, string subject, string firstname, int EventId , int amount)
        {
            try
            {
                EventMaster eventmaster = EventService.GetEventById(Convert.ToInt32(EventId));
                //Email = db.Adminsettings.Select(a => a.Smtpusername).FirstOrDefault();
                string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
                string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
                string body = "<tr>";
              
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'> Dear User,</p>";
                    //body = body + "<p style='text-align:left; padding: 0px 30px;'>Your credit request has been accepted for event " + eventmaster.EventName + ".</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>Your request for " + amount + " credits have been received. We will get in touch with you shortly to address this request. Please look for our email on " + Email + " </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>If you have any queries, please email us on contact@eventnx.com. </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 0px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; line-height: 22px;'><b>Thanks and Cheers,</b><br />The Entire EventNX team </p>";
                    
                string mailbody = header + body + footer;
                SendMailService.SendMail(Email, subject, mailbody);

            }
            catch (Exception ex)
            {

            }

        }
        public Transaction UpdateTransaction(OrderModel ordermodel, string TransType)
        {
            Transaction transaction = new Transaction();
            transaction.CustomerId = ordermodel.CustomerId;
            transaction.Amount = Convert.ToDecimal(ordermodel.OrderTotalWithoutTax);
            transaction.TransactionDate = DateTime.Now;
            if (TransType == "credit")
            {
                transaction.Status = "Pending";
            }
            else
            {
                transaction.Status = "Approved";
            }
            transaction.TransactionType = TransType;
            transaction.PaymentMethod = ordermodel.PaymentMethod;
            transaction.Isdeleted = false;
            transaction.CreatedDate = DateTime.Now;
            transaction.UpdatedDate = DateTime.Now;
            TransactionService.UpdateTransaction(transaction);

            return transaction;
        }
        
    }
}