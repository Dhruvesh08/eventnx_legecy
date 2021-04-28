using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class Adminsettingmodel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tax Percent is required")]
        public int Taxpercent { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string Companyname { get; set; }
        [Required(ErrorMessage = "Address 1 is required")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Address 2 is required")]
        public string Adddress2 { get; set; }
        [Required(ErrorMessage = "Pincode is required")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Host Name is required")]
        public string Hostname { get; set; }
        [Required(ErrorMessage = "Smtp Username is required")]
        public string Smtpusername { get; set; }
        [Required(ErrorMessage = "Smtp Password is required")]
        public string Smtpassword { get; set; }
        [Required(ErrorMessage = " Port No is required")]
        public string portno { get; set; }
        
        public Nullable<bool> Ismaintenance { get; set; }
        public string Utm_Source { get; set; }
        public string Utm_Medium { get; set; }
        public string Utm_Campaign { get; set; }
        public string Utm_Content { get; set; }
        public string Utm_Term { get; set; }
    }
}