using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockInModel
    {
        public string stockInNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public Nullable<int> supplierId { get; set; }
        public int inType { get; set; }
        public double inAmount { get; set; }
        public double inCost { get; set; }
        public double inRemain { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string supplierName { get; set; }
    }
}
