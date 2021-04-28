using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportAccountDetailsModel
    {
        public int CustomerNo { get; set; }
        public int TransactionNo { get; set; }
        public int Credits { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string status { get; set; }
        public string Remark { get; set; }
    }
}