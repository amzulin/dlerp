using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_DelegateSendDetailModel
    {
        public int detailSn { get; set; }
        public string sendNo { get; set; }
        public string delegateNo { get; set; }
        public decimal productAmount { get; set; }
        public decimal price { get; set; }
        public decimal backProduct { get; set; }
        public int bomId { get; set; }
        public int delegateDetailSn { get; set; }
        public decimal theoryAmount { get; set; }
        public decimal realAmount { get; set; }
        public string remark { get; set; }
        public string productNo { get; set; }
        public string productName { get; set; }
        public string productModel { get; set; }
        public string productUnit { get; set; }
        public string productTu { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public Nullable<decimal> materialprice { get; set; }
    }
}
