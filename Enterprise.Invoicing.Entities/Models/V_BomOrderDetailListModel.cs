using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomOrderDetailListModel
    {
        public int detailListSn { get; set; }
        public int detailSn { get; set; }
        public int bomId { get; set; }
        public double bomAmount { get; set; }
        public Nullable<System.DateTime> needDate { get; set; }
        public string remark { get; set; }
        public string bomOrderNo { get; set; }
        public double Price { get; set; }
        public double outAmount { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public string createStaff { get; set; }
        public double Amount { get; set; }
        public string materialCate { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string bigcate { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public string materialNo { get; set; }
        public int forbomId { get; set; }
        public int xslength { get; set; }
    }
}
