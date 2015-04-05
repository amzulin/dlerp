using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateSend
    {
        public DelegateSend()
        {
            this.DelegateSendDetails = new List<DelegateSendDetail>();
        }

        public string sendNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> supplierId { get; set; }
        public System.DateTime sendDate { get; set; }
        public System.DateTime needDate { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<DelegateSendDetail> DelegateSendDetails { get; set; }
    }
}
