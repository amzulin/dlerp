using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ProductSemi
    {
        public Nullable<double> amount { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<double> total { get; set; }
        public string semiNo { get; set; }
        public string staffName { get; set; }
        public string remark { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public System.DateTime semiDate { get; set; }
        public string checkStaff { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public int status { get; set; }
    }
}
