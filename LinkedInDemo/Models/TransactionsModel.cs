using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LinkedInDemo.Models
{
    public class TransactionsModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EventId { get; set; }
        public bool UserCredit { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public int Amount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string status { get; set; }
        public string TransactionType { get; set; }
        public string Remarks { get; set; }
        public string PaymentId1 { get; set; }
        public string PaymentId2 { get; set; }
        public string PaymentId3 { get; set; }
        public string PaymentId4 { get; set; }
        public string PaymentId5 { get; set; }
        [Required(ErrorMessage =" Please select one of the option")]
        public string PaymentMethod { get; set; }
        public Nullable<bool> Isdeleted { get; set; }
        public Nullable<System.DateTime> CreditedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public SelectList list { get; set; }
        public int TransactionId { get; set; }
        

    }
    public class RemarkModel
    {
        public int Tid { get; set; }
        public string Tremark { get; set; }
    }

    public class Remark
    {
        public Remark()
        {
            remark = new List<RemarkModel>();
        }
        public List<RemarkModel> remark { get; set; }
    }
}