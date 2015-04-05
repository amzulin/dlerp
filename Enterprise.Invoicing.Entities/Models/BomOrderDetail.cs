using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomOrderDetail
    {
        public BomOrderDetail()
        {
            this.BomOrderDetailLists = new List<BomOrderDetailList>();
            this.BomOrderVirtualLists = new List<BomOrderVirtualList>();
            this.BomOuts = new List<BomOut>();
        }

        public int detailSn { get; set; }
        public string bomOrderNo { get; set; }
        public int bomId { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double outAmount { get; set; }
        public double sellAmount { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public bool hadRequire { get; set; }
        public bool haddelegate { get; set; }
        public bool hadproduce { get; set; }
        public string requireNo { get; set; }
        public string delegateNo { get; set; }
        public string produceNo { get; set; }
        public string createStaff { get; set; }
        public Nullable<System.DateTime> bomDate { get; set; }
        public string bomSerial { get; set; }
        public string remark { get; set; }
        public virtual BomMain BomMain { get; set; }
        public virtual BomOrder BomOrder { get; set; }
        public virtual ICollection<BomOrderDetailList> BomOrderDetailLists { get; set; }
        public virtual ICollection<BomOrderVirtualList> BomOrderVirtualLists { get; set; }
        public virtual ICollection<BomOut> BomOuts { get; set; }
    }
}
