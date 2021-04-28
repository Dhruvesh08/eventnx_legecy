using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LinkedInDemo.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email Id is required")]
        [Display(Name = "Email Id") ]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        //[MembershipPassword(
        //    MinRequiredNonAlphanumericCharacters = 1,

        //    MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
        //    ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",

        //    MinRequiredPasswordLength = 6
        //    )]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> RoleName { get; set; }
    }
}