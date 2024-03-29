//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracticeMarketplace.ADO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderItem = new HashSet<OrderItem>();
        }
    
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Status_Id { get; set; }
        public int PointOfDelivery_Id { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PointOfDelivery PointOfDelivery { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
