using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class TransactionService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static Transaction GetTransactionById(int Id)
        {
            return db.Transactions.Where(x => x.Id == Id).FirstOrDefault();
        }

        public static IEnumerable<Transaction> GetAllTransactions()
        {
            return db.Transactions.AsEnumerable();
        }

        public static void InsertTransaction(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public static void UpdateTransaction(Transaction transaction)
        {
            db.Entry(transaction).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeleteTransaction(int Id)
        {
            Transaction transaction = db.Transactions.Find(Id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
        }
      
        public static IEnumerable<GetTransactionsDetails_Result> UserTransactionDetails(int Id,int SearchRecords, int CustomerId ,string transactiontype, string status)
        {

            transactiontype = transactiontype.Trim().ToLower();
            status = status.Trim().ToLower();
           
            var RtnData = (from data2 in db.GetTransactionsDetails(Id, CustomerId, transactiontype, status)
                           select data2);

            return RtnData;
        }
        public static Transaction GetTransactionBycustomerId(int customerId)
        {
            var data = db.Transactions.Where(a => a.CustomerId == customerId).FirstOrDefault();
            return data;
        }
    }
}
