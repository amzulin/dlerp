using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_DelegateBackDetail
    {
        public int detailSn { get; set; }
        public string backNo { get; set; }
        public bool isProduct { get; set; }
        public string materialNo { get; set; }
        public decimal backAmount { get; set; }
        public string remark { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public string fastcode { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public Nullable<decimal> price { get; set; }
        public System.DateTime backDate { get; set; }
        public string fromDelegateNo { get; set; }
    }
}
