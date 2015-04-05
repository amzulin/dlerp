using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ProductionDetailModel
    {
        public string produceNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public string productNo { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> orderDetailSn { get; set; }
        public double productAmount { get; set; }
        public double productBackAmount { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string productRemark { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string productName { get; set; }
        public string productModel { get; set; }
        public string productUnit { get; set; }
        public string productFast { get; set; }
        public string productTu { get; set; }
        public Nullable<int> detailSn { get; set; }
        public string materialNo { get; set; }
        public Nullable<double> amount { get; set; }
        public Nullable<double> outAmount { get; set; }
        public string remark { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string fastcode { get; set; }
        public string tunumber { get; set; }
        public string edittype { get; set; }
        public Nullable<int> xslength { get; set; }
    }
}
