using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomVirtual
    {
        public BomVirtual()
        {
            this.BomOrderVirtualLists = new List<BomOrderVirtualList>();
        }

        public int virtualId { get; set; }
        public int bomId { get; set; }
        public string virtualName { get; set; }
        public double vAmount { get; set; }
        public double vPrice { get; set; }
        public string remark { get; set; }
        public virtual BomMain BomMain { get; set; }
        public virtual ICollection<BomOrderVirtualList> BomOrderVirtualLists { get; set; }
    }
}
