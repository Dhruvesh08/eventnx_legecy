using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Models
{
    public class CustomerModel
    {
        public List<SelectListItem> countrylist { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact No is required")]
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "Please enter valid Contact number")]
        public string Contactno { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        public DateTime Date_of_Registration { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in City")]
        public string City { get; set; }
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in State")]
        public string State { get; set; }
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Pincode")]
        public int Pincode { get; set; }

        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Country")]
        public string Country { get; set; }

        public bool IsActive { get; set; }
        public bool AccountStatus { get; set; }
        public System.DateTime createddate { get; set; }
        public DateTime updateddate { get; set; }

        public decimal AvailableCredits { get; set; }
        public int NoOfEvents { get; set; }

       
       
    }
}