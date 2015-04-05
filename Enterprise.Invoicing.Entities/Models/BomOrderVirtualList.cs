using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomOrderVirtualList
    {
        public int virtualSn { get; set; }
        public int detailSn { get; set; }
        public int virtualId { get; set; }
        public double sAmount { get; set; }
        public double sPrice { get; set; }
        public string remark { get; set; }
        public virtual BomOrderDetail BomOrderDetail { get; set; }
        public virtual BomVirtual BomVirtual { get; set; }
    }
}
