using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BussinessLayer;
using DataLayer;

namespace LinkedInDemo.Class
{
    public class AdminSiteConfiguration
    {
        public static string Address1,Address2,Country,City,State,Pincode;
        public static string GetURL()
        {
            Adminsetting adminsetting = AdminService.GetAdminSetting();
            return adminsetting.CompanyWebsite;
        }

        public static string SiteName
        {
            get
            {
                Adminsetting adminsetting = AdminService.GetAdminSetting();
                return adminsetting.Companyname;
            }
        }

        public static string CompanyEmail
        {
            get
            {
                Adminsetting adminsetting = AdminService.GetAdminSetting();
                return adminsetting.CompanyEmail;
            }
        }

        //public static string GetCompanyAddress()
        //{
        //    Adminsetting adminsetting = AdminService.GetAdminSetting();
        //    Address1 = adminsetting.Address1;
        //    Address2 = adminsetting.Adddress2;
        //    Country = adminsetting.Country;
        //    City = adminsetting.City;
        //    State = adminsetting.State;
        //    Pincode = adminsetting.Pincode;
        //    return adminsetting.CompanyWebsite;
        //}
    }
}