using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_DelegateOrderModel
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
