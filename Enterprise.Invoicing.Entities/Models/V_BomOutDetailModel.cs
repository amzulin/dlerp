using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomOutDetailModel
    {
        public string bomOutNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public int detailSn { get; set; }
        public double outAmount { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public int bomId { get; set; }
        public double amount { get; set; }
        public string detailRemark { get; set; }
        public string materialNo { get; set; }
        public string materialCate { get; set; }
        public double bomAmount { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string tunumber { get; set; }
    }
}
