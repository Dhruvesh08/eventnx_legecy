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
    
    public partial class UserPackage
    {
        public int UserPackageId { get; set; }
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
        public System.DateTime Purchasedate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime updateddate { get; set; }
    
        public virtual Package Package { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
