using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductGive
    {
        public string giveNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> depId { get; set; }
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
    }
}
