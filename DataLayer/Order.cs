//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
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
        public Nullable<int> TransactionId { get; set; }
    
        public virtual Package Package { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
