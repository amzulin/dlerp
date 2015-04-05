using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_MaterialPriceModel
    {
        public int priceId { get; set; }
        public int supplierId { get; set; }
        public string materialNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public decimal price { get; set; }
        public System.DateTime startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public string materialName { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public string staffName { get; set; }
        public string materialModel { get; set; }
        public string supplierName { get; set; }
        public int type { get; set; }
    }
}
