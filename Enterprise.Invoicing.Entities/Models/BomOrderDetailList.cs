using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomOrderDetailList
    {
        public int detailListSn { get; set; }
        public int detailSn { get; set; }
        public int bomId { get; set; }
        public double bomAmount { get; set; }
        public double remainAmount { get; set; }
        public Nullable<System.DateTime> needDate { get; set; }
        public string remark { get; set; }
        public virtual BomMain BomMain { get; set; }
        public virtual BomOrderDetail BomOrderDetail { get; set; }
    }
}
