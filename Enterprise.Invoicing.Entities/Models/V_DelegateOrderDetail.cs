using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_DelegateOrderDetail
    {
        public string delegateNo { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string productNo { get; set; }
        public Nullable<int> bomId { get; set; }
        public Nullable<int> staffId { get; set; }
        public decimal productAmount { get; set; }
        public decimal productPrice { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime backDate { get; set; }
        public string remark { get; set; }
        public string supplierName { get; set; }
        public string person { get; set; }
        public string productName { get; set; }
        public string productModel { get; set; }
        public string productUnit { get; set; }
        public string productPinyin { get; set; }
        public string ProductTu { get; set; }
        public string staffName { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> detailSn { get; set; }
        public string materialNo { get; set; }
        public Nullable<decimal> detailAmount { get; set; }
        public Nullable<decimal> detailPrice { get; set; }
        public Nullable<decimal> sendAmount { get; set; }
        public Nullable<decimal> backAmount { get; set; }
        public string detailRemark { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public Nullable<int> xslength { get; set; }
        public decimal productBackAmount { get; set; }
        public bool isclose { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> orderDetailSn { get; set; }
        public bool canfs { get; set; }
        public decimal productGiveAmount { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
    }
}
