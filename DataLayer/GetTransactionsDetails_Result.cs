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
    
    public partial class GetTransactionsDetails_Result
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public int CustomerId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string status { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
    }
}