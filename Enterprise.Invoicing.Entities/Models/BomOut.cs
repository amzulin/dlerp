using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomOut
    {
        public BomOut()
        {
            this.BomOutDetails = new List<BomOutDetail>();
        }

        public string bomOutNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public int detailSn { get; set; }
        public double outAmount { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public virtual BomOrderDetail BomOrderDetail { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<BomOutDetail> BomOutDetails { get; set; }
    }
}
