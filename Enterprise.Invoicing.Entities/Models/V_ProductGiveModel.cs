using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ProductGiveModel
    {
        public string giveNo { get; set; }
        public string pullNo { get; set; }
        public decimal giveAmount { get; set; }
        public System.DateTime giveDate { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public string deportStaff { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string produceNo { get; set; }
        public decimal makeAmount { get; set; }
        public Nullable<double> totalorderAmount { get; set; }
        public Nullable<decimal> totalPullAmount { get; set; }
        public Nullable<double> totalBackAmount { get; set; }
        public decimal hadgiveAmount { get; set; }
        public string materialNo { get; set; }
        public string bomOrderNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> staffId { get; set; }
    }
}
