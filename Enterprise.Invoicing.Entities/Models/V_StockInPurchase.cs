using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockInPurchase
    {
        public string stockInNo { get; set; }
        public Nullable<int> supplierId { get; set; }
        public System.DateTime createDate { get; set; }
        public string materialNo { get; set; }
        public double inAmount { get; set; }
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }
        public double poAmount { get; set; }
        public double poPrice { get; set; }
        public string supplierName { get; set; }
        public string category { get; set; }
        public string materialModel { get; set; }
        public string materialName { get; set; }
        public string tunumber { get; set; }
        public string unit { get; set; }
        public string pinyin { get; set; }
        public string fastcode { get; set; }
        public string person { get; set; }
        public double returnAmount { get; set; }
        public double inCost { get; set; }
        public double poCost { get; set; }
        public double returnCost { get; set; }
    }
}
