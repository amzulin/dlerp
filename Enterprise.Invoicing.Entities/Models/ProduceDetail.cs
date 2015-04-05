using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProduceDetail
    {
        public int detailSn { get; set; }
        public string produceNo { get; set; }
        public string materialNo { get; set; }
        public double amount { get; set; }
        public double outAmount { get; set; }
        public string remark { get; set; }
        public virtual Material Material { get; set; }
        public virtual Production Production { get; set; }
    }
}
