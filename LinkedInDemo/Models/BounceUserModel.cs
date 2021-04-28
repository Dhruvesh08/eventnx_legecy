using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class BounceUserModel
    {
        public int UserId { get; set; }
        public int? EventId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ReferredBy { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        public string EventName { get; set; }
        public System.DateTime EventStartDate { get; set; }
        public System.DateTime EventEndDate { get; set; }
        public string DomainName { get; set; }
        public string accesstoken { get; set; }
        public string profileid { get; set; }
        public string Company { get; set; }
        public string JobDesignation { get; set; }
        public string MobileNumber { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime DateOfRegistration { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRegistered { get; set; }
        public int CustomerId { get; set; }
        public int? ReferralId { get; set; }
        public string Source { get; set; }
    }
}