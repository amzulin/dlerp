using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class SettlementCapital
    {
        public long capitalSn { get; set; }
        public string settleNo { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public System.DateTime tradeDate { get; set; }
        public string createStaff { get; set; }
        public decimal tradeCost { get; set; }
        public decimal badCost { get; set; }
        public decimal otherCost { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
    }
}
