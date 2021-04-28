using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
     public static class ManageCustomerService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static Customer GetCustomerById(int Id)
        {
            return db.Customers.Where(x => x.CustomerId == Id).FirstOrDefault();
        }


        public static void InsertCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public static void UpdateCustomer(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void DeleteCustomer(int Id)
        {
            Customer customer = db.Customers.Find(Id);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
        public static IEnumerable<GetCustomerDetails_Result> CustomerDetails( int Id, string Alpha)
        {
           
            Alpha = Alpha.Trim().ToLower();
            var RtnData = (from data2 in db.GetCustomerDetails(Id, Alpha)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<ManageEvents_Result> ManageEvents(int Id ,string SearchTitle)
        {
            var RtnData = (from data2 in db.ManageEvents(Id, SearchTitle)
                           select data2);

            return RtnData;
        }

    }
}
