using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DataLayer;

namespace LinkedInDemo
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<CustomerRole> Roles { get; set; }

        #endregion

        public CustomMembershipUser(Customer customer):base("CustomMembership", customer.Username, customer.CustomerId, customer.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            CustomerId = customer.CustomerId;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Roles = customer.CustomerRoles;
        }
    }
}