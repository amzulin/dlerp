using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductSemi
    {
        public int semiId { get; set; }
        public string semiNo { get; set; }
        public string proName { get; set; }
        public string proModel { get; set; }
        public double amount { get; set; }
        public decimal price { get; set; }
        public string remark { get; set; }
        public int staffId { get; set; }
        public string staffName { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime semiDate { get; set; }
        public string checkStaff { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public int status { get; set; }
    }
}
