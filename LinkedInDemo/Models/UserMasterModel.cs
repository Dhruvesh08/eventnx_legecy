using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedInDemo.Models
{
    public class UserMasterModel
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string FullName { get; set; }
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
        public string Source { get; set; }
        public string Image { get; set; }
        public string ImageName { get; set; }
        public string MobileNumber { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime DateOfRegistration { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRegistered { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileLink { get; set; }
        public SelectList domainlist { get; set; }
        public SelectList eventlist { get; set; }
        public int RegisteredUser { get; set; }
        public decimal Creditrequired { get; set; }
        public int TotalReferences { get; set; }
        public int CustomerId { get; set; }
        public int ReferralId { get; set; }
        public Nullable<int> ReferralCount { get; set; }
        public int ConnectionCount { get; set; }
        public string EventKey { get; set; }
        public int usercount { get; set; }
        public int? visitorcount { get; set; }
        public List<SelectListItem> eventList { get; set; }
        public string CRM_CompanyName { get; set; }
        public string CRM_JobTitle { get; set; }
        public string CRM_RegistrationId { get; set; }

    }
}