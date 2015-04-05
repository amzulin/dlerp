using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateOrder
    {
        public string delegateNo { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string materialNo { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> orderDetailSn { get; set; }
        public Nullable<int> bomId { get; set; }
        public Nullable<int> staffId { get; set; }
        public decimal amount { get; set; }
        public decimal giveAmount { get; set; }
        public decimal backAmount { get; set; }
        public decimal price { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime backDate { get; set; }
        public string remark { get; set; }
    }
}
