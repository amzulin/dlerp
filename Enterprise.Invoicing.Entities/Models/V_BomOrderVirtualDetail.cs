using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomOrderVirtualDetail
    {
        public int virtualSn { get; set; }
        public int detailSn { get; set; }
        public int virtualId { get; set; }
        public double sAmount { get; set; }
        public double sPrice { get; set; }
        public string remark { get; set; }
        public int bomId { get; set; }
        public string virtualName { get; set; }
        public double vAmount { get; set; }
        public double vPrice { get; set; }
        public string bomOrderNo { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public bool hadBom { get; set; }
        public bool hadRequire { get; set; }
        public string createStaff { get; set; }
    }
}
