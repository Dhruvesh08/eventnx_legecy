using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ZoomUserReportModel
    {
        public int UserId { get; set; }
        public int? EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferredBy { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string ProfileImage { get; set; }
        public string EventName { get; set; }
        public string Company { get; set; }
        public string CRM_CompanyName { get; set; }
        public string CRM_JobTitle { get; set; }
        public string JobDesignation { get; set; }
        public string MobileNumber { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRegistered { get; set; }
        public int CustomerId { get; set; }
        public int? ReferralId { get; set; }
        public string Source { get; set; }
        public string SeniorityLevel { get; set; }
        public string PrimaryJob { get; set; }
        public string NatureofBusiness { get; set; }
        public string TopicOfInterest { get; set; }
        public string RegisteredForGlobal { get; set; }
        public string Checbox1 { get; set; }
        public string Checbox2 { get; set; }
        public string Checbox3 { get; set; }
        public string Checbox4 { get; set; }
    }

    public class ZoomUserExportModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string EventName { get; set; }
        public string Company { get; set; }
        public string JobDesignation { get; set; }
        public string MobileNumber { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public string Source { get; set; }
        public string SeniorityLevel { get; set; }
        public string PrimaryJob { get; set; }
        public string NatureofBusiness { get; set; }
        public string Checbox1 { get; set; }
        public string Checbox2 { get; set; }
        public string Checbox3 { get; set; }
        public string Checbox4 { get; set; }
    }
}