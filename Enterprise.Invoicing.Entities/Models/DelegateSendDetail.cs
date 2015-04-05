using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateSendDetail
    {
        public int detailSn { get; set; }
        public string sendNo { get; set; }
        public string delegateNo { get; set; }
        public decimal productAmount { get; set; }
        public decimal price { get; set; }
        public decimal backProduct { get; set; }
        public string materialNo { get; set; }
        public int bomId { get; set; }
        public int delegateDetailSn { get; set; }
        public decimal theoryAmount { get; set; }
        public decimal realAmount { get; set; }
        public string remark { get; set; }
        public virtual DelegateOrderDetail DelegateOrderDetail { get; set; }
        public virtual DelegateSend DelegateSend { get; set; }
    }
}
