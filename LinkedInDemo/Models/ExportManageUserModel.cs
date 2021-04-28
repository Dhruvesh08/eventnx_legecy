using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportManageUserModel
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public DateTime DateofRegistration { get; set; }
        public decimal? AvailableCredits { get; set; }
        public int? NoOfEvents { get; set; }

    }
}