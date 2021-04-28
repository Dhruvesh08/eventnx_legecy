using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LinkedInDemo.Models
{
    public class Changepasswordmodel
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required ")]
        public string ConfirmPassword { get; set; }
        public string ActivationCode { get; set; }

    }
}