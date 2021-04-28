using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ContactUsModel
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "Please enter valid Phone Number")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address.Please Correct.")]
        public string Email { get; set; }
        public string Message { get; set; }

        public string FullName { get; set; }
        public string CompanyName { get; set; }
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "Please enter valid Phone Number")]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address.Please Correct.")]
        public string EmailAddress { get; set; }
        public string CreditsRequired { get; set; }
    }
}