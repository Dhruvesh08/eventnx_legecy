using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportReferraluserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReferredBy { get; set; }
        public string Email { get; set; }
        public string EventName { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }

    }
}