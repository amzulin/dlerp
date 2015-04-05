using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_MaterialCost
    {
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string bigcate { get; set; }
        public string category { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public string supplierName { get; set; }
        public Nullable<int> supplierId { get; set; }
        public double poPrice { get; set; }
        public System.DateTime createDate { get; set; }
        public double poAmount { get; set; }
        public string unit { get; set; }
        public int costSn { get; set; }
    }
}
