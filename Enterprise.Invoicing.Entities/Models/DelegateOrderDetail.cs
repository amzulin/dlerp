using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateOrderDetail
    {
        public DelegateOrderDetail()
        {
            this.DelegateSendDetails = new List<DelegateSendDetail>();
        }

        public int detailSn { get; set; }
        public string delegateNo { get; set; }
        public string materialNo { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
        public decimal sendAmount { get; set; }
        public decimal backAmount { get; set; }
        public string remark { get; set; }
        public virtual ICollection<DelegateSendDetail> DelegateSendDetails { get; set; }
    }
}
