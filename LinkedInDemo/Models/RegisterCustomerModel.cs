using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LinkedInDemo.Models
{
    public class RegisterCustomerModel
    {
        
        public string Username { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Numbers not allowed in First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        //[RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "Invalid Character in Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Numbers not allowed in Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.Please Correct.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public Guid ActivationCode { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        //[MembershipPassword(
        //    MinRequiredNonAlphanumericCharacters = 1,

        //    MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
        //    ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",

        //    MinRequiredPasswordLength = 6
        //    )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Error : Password Mismatch. Please Correct.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Contact No.")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Contact No should be of 10 digits")]
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "Please enter valid Contact number")]
        public string Contactno { get; set; }

       
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public bool isTrue
        { get { return true; } }

        [Required]
        [Display(Name = "I agree to the terms and conditions")]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool AgreeTerms { get; set; }
        public string Guid { get; set; }
    }
}