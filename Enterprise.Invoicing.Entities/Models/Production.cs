using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Production
    {
        public Production()
        {
            this.ProduceDetails = new List<ProduceDetail>();
        }

        public string produceNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public string materialNo { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> orderDetailSn { get; set; }
        public double amount { get; set; }
        public decimal pullAmount { get; set; }
        public double backAmount { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string remark { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Material Material { get; set; }
        public virtual ICollection<ProduceDetail> ProduceDetails { get; set; }
    }
}
