using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportDetailsModel
    {
        public int TransactionNumber { get; set; }
        public decimal? Amount { get; set; }
        public string TransactionDate { get; set; }
        public string status { get; set; }
        public string TransactionType { get; set; }
        // public string PaymentMethod { get; set; }
    }
}