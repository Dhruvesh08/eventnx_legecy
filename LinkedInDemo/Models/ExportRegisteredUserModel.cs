using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportRegisteredUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string ReferredBy { get; set; }
        public string Email { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public int? ReferralCount { get; set; }
    }
}