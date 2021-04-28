using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PackageId { get; set; }
        public int EventId { get; set; }
        public bool UserCredit { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal OrderTotalWithoutTax { get; set; }
        public string OrderStatus { get; set; }
        public System.DateTime PurchasedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime OrderCompletedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int Id { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
        public int TransactionId { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string Email { get; set; }
        public string Contactno { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string Country { get; set; }
        public string companyaddress1 { get; set; }
        public string companyaddress2 { get; set; }
        public string companyEmail { get; set; }
        public string companyContactno { get; set; }
        public string companyCity { get; set; }
        public string companyState { get; set; }
        public int companyPincode { get; set; }
        public string companyCountry { get; set; }
        public string companyemail { get; set; }
        public string companywebsite { get; set; }
    }
}