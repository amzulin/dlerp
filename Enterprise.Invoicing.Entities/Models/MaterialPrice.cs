using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class MaterialPrice
    {
        public int priceId { get; set; }
        public int supplierId { get; set; }
        public string materialNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public decimal price { get; set; }
        public System.DateTime startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public System.DateTime createDate { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Material Material { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
