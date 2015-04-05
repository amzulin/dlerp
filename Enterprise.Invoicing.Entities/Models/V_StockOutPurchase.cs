using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockOutPurchase
    {
        public string bomOrderNo { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string supplierName { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double sellAmount { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public string stockoutNo { get; set; }
        public System.DateTime createDate { get; set; }
        public int status { get; set; }
        public int isover { get; set; }
        public int depotId { get; set; }
        public double outAmount { get; set; }
        public double returnAmount { get; set; }
        public double outPrice { get; set; }
        public int orderSn { get; set; }
        public int outDetailSn { get; set; }
        public string remark { get; set; }
        public double totalCost { get; set; }
        public double outCost { get; set; }
        public double returnCost { get; set; }
    }
}
