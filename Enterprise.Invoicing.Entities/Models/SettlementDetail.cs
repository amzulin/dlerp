using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class SettlementDetail
    {
        public long detailSn { get; set; }
        public string settleNo { get; set; }
        public string stockNo { get; set; }
        public System.DateTime tradeDate { get; set; }
        public int tradeType { get; set; }
        public string fromNo { get; set; }
        public int fromDetailSn { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialUnit { get; set; }
        public string materialTu { get; set; }
        public int stockdetailsn { get; set; }
        public decimal tradeAmount { get; set; }
        public decimal returnAmount { get; set; }
        public decimal tradePrice { get; set; }
        public int depotId { get; set; }
        public string depotName { get; set; }
        public string stockdetailRemark { get; set; }
        public int isSettle { get; set; }
        public int capitalSn { get; set; }
    }
}
