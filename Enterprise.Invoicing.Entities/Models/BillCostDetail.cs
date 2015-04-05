using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BillCostDetail
    {
        public int detailSn { get; set; }
        public string billNo { get; set; }
        public string billTitle { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public System.DateTime billDate { get; set; }
        public string remark { get; set; }
        public virtual BillCost BillCost { get; set; }
    }
}
