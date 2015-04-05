using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class PurchaseRequireDetail
    {
        public int detailSn { get; set; }
        public string requireNo { get; set; }
        public string materialNo { get; set; }
        public double orderAmount { get; set; }
        public double buyAmount { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public virtual Material Material { get; set; }
        public virtual PurchaseRequire PurchaseRequire { get; set; }
    }
}
