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
    
    public partial class Transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public string Remarks { get; set; }
        public string PaymentId1 { get; set; }
        public string PaymentId2 { get; set; }
        public string PaymentId3 { get; set; }
        public string PaymentId4 { get; set; }
        public string PaymentId5 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> Isdeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string PaymentMethod { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}