using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class PurchaseRequire
    {
        public PurchaseRequire()
        {
            this.PurchaseRequireDetails = new List<PurchaseRequireDetail>();
        }

        public string requireNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public int isover { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> orderDetailSn { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<PurchaseRequireDetail> PurchaseRequireDetails { get; set; }
    }
}
