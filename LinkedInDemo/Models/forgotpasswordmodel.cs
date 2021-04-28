using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ForgotPasswordmodel
    {
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}