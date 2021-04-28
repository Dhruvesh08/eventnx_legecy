using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using LinkedInDemo.Models;
using System.Data.Entity;
using Order = DataLayer.Order;
using Adminsetting = DataLayer.Adminsetting;
using BussinessLayer;
using LinkedInDemo.CustomAuthentication;

namespace LinkedInDemo.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class TransactionsController : Controller
    {
        private EventRegistrationEntities db = new EventRegistrationEntities();
        
        public ActionResult AddCredit(bool usercredit=false,int eventid=0,decimal amount=0,int CustomerId =0)
        {
            TransactionsModel model = new TransactionsModel();
            model.Amount = Convert.ToInt32(amount);
            model.EventId = eventid;
            model.UserCredit = usercredit;
            model.CustomerId = CustomerId;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCredit(TransactionsModel model)
        {
            //OrderModel ordermodel = new OrderModel();
            //int check = db.Adminsettings.Select(a => a.Taxpercent).FirstOrDefault();
            //ordermodel.OrderTotalWithoutTax = Convert.ToDecimal(model.Amount);
            //decimal tax = (ordermodel.OrderTotalWithoutTax) * (check) / 100;
            //decimal ordertotal = (tax) + (ordermodel.OrderTotalWithoutTax);
            //ordermodel.TaxAmount = tax;
            //ordermodel.OrderTotal = ordertotal;
            //ordermodel.CustomerId = Convert.ToInt32(Session["CustomerId"]);

            //ordermodel.PaymentMethod = model.PaymentMethod;
            //ordermodel.EventId = model.EventId;
            //ordermodel.UserCredit = model.UserCredit;
            int CustomerId;
            if(model.CustomerId == 0)
            {
                CustomerId = Convert.ToInt32(Session["CustomerId"]);
            }
            else
            {
                CustomerId = model.CustomerId;
            }
            var transaction = AddTransaction(CustomerId, model.Amount, "credit");
            ViewBag.Message = "Your request for additional credits is received. We will get in touch with you shortly. Thank you for using EventNX.";
            //var order = AddOrder(ordermodel);
            //ordermodel.Id = transaction.Id;
            //ordermodel.OrderStatus = transaction.Status;
            //ordermodel.TransactionDate = transaction.TransactionDate;
            
            var user = CustomerService.GetCustomerById(Convert.ToInt32(Session["CustomerId"]));
            sendMailtoadmin(user.FirstName, user.LastName,user.Email,user.Contactno,user.CompanyName,model.Amount);
            //if (ordermodel.UserCredit)
            //{
            //    transaction = AddTransaction(ordermodel, "debit");
            //    RegisteredUserService.MarkRegisteredUserAsPaidForEvent(ordermodel.EventId);
            //}
            return View(model);
        }
        public void sendMailtoadmin(string firstname, string LastName, string CustEmail,string Contactno, string CompanyName, decimal ordertotal)
        {
            try
            {
                var subject= "New Credit Request";
                var Email = db.Adminsettings.Select(a => a.Smtpusername).FirstOrDefault();
                string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
                string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
                string body = "<tr>";
               
                    body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 20px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000; text-align:left;'>" + " " + firstname +" "+ LastName +" "+ "has requested"+" "+ ordertotal +" "+ " to credit.</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 12px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'> User Details are :</p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 12px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>" + CustEmail + " </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 12px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;'>" + Contactno +" </p>";
                    body = body + "<p style='margin: 0px; padding: 0px 0px 12px 0px; font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #000;line-height: 22px;'>" + CompanyName + "</p>";
                string mailbody = header + body + footer;
                SendMailService.SendMail("contact@eventnx.com,raufs@thetacttree.com,nipulp@tacttree.com", subject, mailbody);

            }
            catch (Exception ex)
            {

            }

        }
        public void directcreditbyadmin(int CustomerId,string firstname, decimal ordertotal)
        {
            try
            {
                var subject = " Credits added to your account. ";
                var cust =  CustomerService.GetCustomerById(CustomerId) ;
                var Email = cust.Email;
                var url = string.Format("/Dashboard/");
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
                string header = EmailBody.MailHeader(subject, LinkedInDemo.Class.AdminSiteConfiguration.GetURL());
                string footer = EmailBody.MailFooter(LinkedInDemo.Class.AdminSiteConfiguration.CompanyEmail);
                string body = "<tr>";
                body = body + "<td style='text-align:left; padding: 0px 30px;'>";
                body = body + "<p>Dear User,";
                body = body + " "+ ordertotal + " Credits is added to your account.<br/>Visit your dashboard to check and use these credits for your events.</p>";
                body = body + "<a href='" + link + "'><button> Go To Dashboard </button></a><br/>";
                body = body + "If you have any queries, please email us on contact@eventnx.com.<br/>";
                body = body + "Thank you for using EventNX.";
                body = body + "</td></tr>";
                string mailbody = header + body + footer;
                SendMailService.SendMail(Email, subject, mailbody);
            }
            catch (Exception ex)
            {

            }

        }
        public Transaction AddTransaction(OrderModel ordermodel,string TransType)
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
            TransactionService.InsertTransaction(transaction);

            return transaction;
        }

        public Transaction AddTransaction(int customerId,decimal amount, string TransType)
        {
            Transaction transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.Amount = amount;
            
            transaction.TransactionDate = DateTime.Now;
            if (User.IsInRole("Admin"))
            {
                transaction.Status = "Approved";
                transaction.TransactionType = "credit";
                var cust = CustomerService.GetCustomerById(customerId);
                var fname = cust.FirstName;
                directcreditbyadmin(customerId, fname, amount);

            }
            else
            {
                if (TransType == "credit")
                {
                    transaction.Status = "Pending";
                }
                else
                {
                    transaction.Status = "Approved";
                }
                transaction.TransactionType = TransType;
            }
            
            transaction.PaymentMethod = "offline";
            transaction.Isdeleted = false;
            transaction.CreatedDate = DateTime.Now;
            transaction.UpdatedDate = DateTime.Now;
            TransactionService.InsertTransaction(transaction);

            return transaction;
        }
        public Order AddOrder(OrderModel ordermodel)
        {
            Order order = new Order();
            order.CustomerId = ordermodel.CustomerId;
            order.PackageId = PackageService.GetAllPackages().FirstOrDefault().PackageId;
            order.TaxAmount = ordermodel.TaxAmount;
            order.OrderTotal = ordermodel.OrderTotal;
            order.OrderTotalWithoutTax = ordermodel.OrderTotalWithoutTax;
            order.OrderStatus = "completed";
            order.PurchasedDate = DateTime.Now;
            order.IsActive = ordermodel.IsActive;
            order.IsDeleted = order.IsDeleted;
            order.OrderCompletedDate = DateTime.Now;
            order.CreatedDate = DateTime.Now;
            order.UpdatedDate = DateTime.Now;
            OrderService.InsertOrder(order);
            return order;
        }
    }
}