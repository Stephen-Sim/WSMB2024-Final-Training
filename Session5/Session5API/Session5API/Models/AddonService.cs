//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Session5API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddonService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddonService()
        {
            this.AddonServiceDetails = new HashSet<AddonServiceDetail>();
        }
    
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long UserID { get; set; }
        public Nullable<long> CouponID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddonServiceDetail> AddonServiceDetails { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual User User { get; set; }
    }
}
