using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ReportDepot
    {
        public int depotId { get; set; }
        public string depotName { get; set; }
        public bool valid { get; set; }
        public string remark { get; set; }
        public int detailSn { get; set; }
        public double depotAmount { get; set; }
        public double depotCost { get; set; }
        public double depotSafe { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string orderNo { get; set; }
        public bool mvalid { get; set; }
        public string mremark { get; set; }
        public string materialNo { get; set; }
        public string materialTu { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public Nullable<decimal> oldprice { get; set; }
        public decimal price { get; set; }
    }
}
