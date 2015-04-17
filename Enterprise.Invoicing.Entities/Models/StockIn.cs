using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockIn
    {
        public StockIn()
        {
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
            this.StockInDetails = new List<StockInDetail>();
        }

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
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }
        public int isSettle { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
    }
}
