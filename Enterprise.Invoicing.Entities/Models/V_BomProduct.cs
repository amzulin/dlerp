using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomProduct
    {
        public int bomId { get; set; }
        public int priceId { get; set; }
        public decimal price { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string pinyin { get; set; }
        public int supplierId { get; set; }
        public string supplierNo { get; set; }
        public string supplierName { get; set; }
        public string tunumber { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public string version { get; set; }
    }
}
