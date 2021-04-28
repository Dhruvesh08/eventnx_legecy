using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class CustomerService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static Customer GetCustomerById(int CustomerId)
        {
            var customer = db.Customers.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            return customer;
        }
        public static List<Customer> GetAllCustomer()
        {
            var customer = db.Customers.ToList();
            return customer;
        }
        public static bool IsEmailVerified(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            using (EventRegistrationEntities dbContext = new EventRegistrationEntities())
            {
                var user = (from us in dbContext.Customers
                            where string.Compare(username, us.Email, StringComparison.OrdinalIgnoreCase) == 0
                            && us.IsActive == true
                            select us).FirstOrDefault();

                return (user != null) ? true : false;
            }
        }

        public static Customer GetCustomerByUserName(string UserName)
        {
            var customer = db.Customers.Where(x => x.Username == UserName).FirstOrDefault();
            return customer;
        }
        public static Customer GetCustomerByEmail(string EmailId)
        {
            var customer = db.Customers.Where(x => x.Email == EmailId).FirstOrDefault();
            return customer;
        }
        public static EventUser CheckCustomerExists(int CustomerId ,int EventId)
        {
            var eventuser = db.EventUsers.Where(x => x.CustomerId == CustomerId && x.EventId == EventId).FirstOrDefault();
            return eventuser;
        }

        public static EventUser CheckEventUser(int CustomerId)
        {
            var eventuser = db.EventUsers.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            return eventuser;
        }

        public static void InsertCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public static void InsertCustomerRole(CustomerRole customerrole)
        {
            db.CustomerRoles.Add(customerrole);
            db.SaveChanges();
        }

        public static void InsertEventUser(EventUser eventuser)
        {
            db.EventUsers.Add(eventuser);
            db.SaveChanges();
        }

        public static void UpdateCustomer(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static decimal GetCustomerAvailableCredit(int CustomerId)
        {
            var availablecredit = GetCustomerDeposit(CustomerId) - RegisteredUserService.GetRegisteredUserByCustomerId(CustomerId);
            //var availablecredit = GetCustomerDeposit(CustomerId) - GetCustomerPurchase(CustomerId);
            return availablecredit;
        }

        public static decimal GetCustomerDeposit(int CustomerId)
        {
            decimal credit = 0;
            var query = db.Transactions.Where(x => x.TransactionType == "credit" && x.Status == "completed" &&
            x.CustomerId == CustomerId).ToList();
            if (query.Count > 0)
            {
                credit = query.Sum(x => x.Amount.Value);
            }
            return credit;
        }

        public static decimal GetCustomerPurchase(int CustomerId)
        {
            decimal debit = 0;

            var query = db.Transactions.Where(x => x.TransactionType == "debit" && x.Status == "completed" &&
            x.CustomerId == CustomerId).ToList();
            if (query.Count > 0)
            {
                debit = query.Sum(x => x.Amount.Value);
            }
            return debit;
        }

        public static int GetEventsCount(int CustomerId)
        {
            int eventcount = db.EventMasters.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).Count();

            int eventusercount = db.EventUsers.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).Count();

            int total = eventcount + eventusercount;

            return total;

            //int eventcount = db.EventMasters.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).Count();
            //return eventcount;
        }

        public static decimal GetTotalDeposit(decimal CustomerId)
        {
            decimal creditdeposit = 0;

            var deposit = db.Transactions.Where(x => x.TransactionType == "credit" && x.Status == "completed" && x.CustomerId == CustomerId).ToList();
            if (deposit.Count > 0)
            {
                creditdeposit = deposit.Sum(x => x.Amount).Value;
            }
            return creditdeposit;
        }

        public static decimal GetCreditsUsed(decimal CustomerId)
        {
            decimal creditused = 0;

            var debit = db.Transactions.Where(x => x.TransactionType == "debit" && x.Status == "completed" && x.CustomerId == CustomerId).ToList();
            if (debit.Count > 0)
            {
                creditused = debit.Sum(x => x.Amount.Value);
            }
            return creditused;
        }

        public static int TotalRegistration(int EventId)
        {
            if (EventId == 0)
                return db.RegisteredUsers.Count();
            int totalreg = db.RegisteredUsers.Where(x => x.EventId == EventId).Count();
            return totalreg;
        }

        public static int PaidRegistration(int EventId)
        {
            int totalreg = db.RegisteredUsers.Where(x => x.EventId == EventId && x.Ispaid == true).Count();
            return totalreg;
        }

        public static IEnumerable<GetEventDetails_Result> GetCustomerEvents(string SearchTitle, string Alpha, int CustomerId, string ManageEvent)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int id = 0;
            var RtnData = (from data2 in db.GetEventDetails(id, CustomerId, Alpha, SearchTitle, ManageEvent)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<EventUserDetails_Result> getuserdetails( int Event)
        {
          
            var RtnData = (from data2 in db.EventUserDetails(Event)
                           select data2);

            return RtnData;
        }
        public static IEnumerable<CustomerEmails_Result> CustomerEmails()
        {
       
            var RtnData = (from data2 in db.CustomerEmails()
                           select data2);

            return RtnData;
        }

        public static Customer GetCustomerByActivationCode(string ActivationCode)
        {
            Guid code = Guid.Parse(ActivationCode);
            return db.Customers.Where(x => x.ActivationCode == code).FirstOrDefault();
        }
        public static int GetTotalCustomers()
        {

            var custcount = db.Customers.Select(x => x.CustomerId).ToList();
            var chkrole = db.CustomerRoles.Where(x => custcount.Contains(x.CustomerId) && x.RoleId.Equals(1)).ToList();
            var eventuser = db.EventUsers.Select(x => x.CustomerId).ToList();
            var totalcount = chkrole.Count - eventuser.Count();
            return totalcount;
            //return chkrole.Count();
        }
        public static decimal GetTotalPurchase()
        {
            decimal credit = 0;

            var query = db.Transactions.Where(x => x.TransactionType == "credit" && x.Status == "Approved").ToList();
            if (query.Count > 0)
            {
                credit = query.Sum(x => x.Amount.Value);
            }
            return credit;
        }
        public static decimal GetActiveCustomers()
        {

            var cust = db.Customers.Where(x => x.IsActive == true).Select(x => x.CustomerId).ToList();
            var chkrole = db.CustomerRoles.Where(x => cust.Contains(x.CustomerId) && x.RoleId.Equals(1)).Select(x => x.CustomerId);
            //var chkrole = db.CustomerRoles.Where(x => x.RoleId.Equals(1)).Select(x => x.CustomerId).ToList();
            var activecust = db.Transactions.Where(x => chkrole.Contains(x.CustomerId) && x.TransactionType.Equals("credit") && (x.Status.Equals("Approved"))).GroupBy(x => x.CustomerId).ToList();

            return activecust.Count();
        }
        public static string GetCustomerByName(string FirstName, string LastName)
        {
            var fname = db.Customers.Where(x => x.FirstName == FirstName).FirstOrDefault();
            var lname = db.Customers.Where(x => x.LastName == LastName).FirstOrDefault();
            var concate = fname.FirstName + " " + lname.LastName;
            return concate;
        }
        
    }

}