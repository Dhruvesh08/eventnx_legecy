using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class UserConnectionModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public bool IsDeleted { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string DomainName { get; set; }
        public string ProfileId { get; set; }
        public Nullable<bool> Ispaid { get; set; }
        public int ReferralId { get; set; }
        public int ConnectionCount { get; set; }
        public Nullable<int> ReferralCount { get; set; }
    }
}