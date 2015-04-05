using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ProductGiveDetail
    {
        public int giveSn { get; set; }
        public string giveNo { get; set; }
        public int pullDetailSn { get; set; }
        public decimal giveAmount { get; set; }
        public string remark { get; set; }
        public string pullNo { get; set; }
        public string materialNo { get; set; }
        public decimal theoryAmount { get; set; }
        public decimal realAmount { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public Nullable<decimal> price { get; set; }
    }
}
