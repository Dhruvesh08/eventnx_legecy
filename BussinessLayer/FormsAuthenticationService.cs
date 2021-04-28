using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace BussinessLayer
{
    public static class FormsAuthenticationService
    {
        private static Customer _cachedCustomer;

        public static Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                return _cachedCustomer;
            }
            set
            {
                _cachedCustomer = value;
            }
        }

        public static Customer GetAuthenticatedCustomer(HttpContextBase _httpContext)
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext?.Request == null || !_httpContext.Request.IsAuthenticated || _httpContext.User == null)
                return null;

            Customer customer = null;
            FormsIdentity formsIdentity = null;

            if ((formsIdentity = _httpContext.User.Identity as FormsIdentity) != null)
            {
                customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            }

            if (customer != null && customer.IsActive)
            {
                _cachedCustomer = customer;
            }

            return _cachedCustomer;
        }

        public static Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;

            var customer = CustomerService.GetCustomerByUserName(usernameOrEmail);

            return customer;
        }
    }
}
