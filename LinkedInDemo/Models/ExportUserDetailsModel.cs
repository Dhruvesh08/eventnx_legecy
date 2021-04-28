using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ExportUserDetailsModel
    {
        public int Id { get; set; }
        public string CRM_Id { get; set; }
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in  Name")]
        public string Name { get; set; }
        public string ReferredBy { get; set; }
        public string Email { get; set; }
        public string EventName { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public int? ReferralCount { get; set; }
        public int? VisitorCount { get; set; }
        //public int? EventId { get; set; }
        
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in First Name")]
        //public string FirstName { get; set; }
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Last Name")]
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Country")]
        //public int? ReferralCount { get; set; }
        //public int? VisitorCount { get; set; }
        //public string Country { get; set; }
        
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Event Name")]
        //public string EventName { get; set; }
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Domain Name")]
        //public string DomainName { get; set; }
        //public string CRM_FirstName { get; set; }
        //public string CRM_LastName { get; set; }
        //public string CRM_EmaiId { get; set; }
        //public string CRM_RegistrationId { get; set; }
    

    }
}