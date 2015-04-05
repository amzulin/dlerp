using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockOut
    {
        public StockOut()
        {
            this.StockOutDetails = new List<StockOutDetail>();
            this.StockReturnDetails = new List<StockReturnDetail>();
        }

        public string stockoutNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string bomOrderNo { get; set; }
        public Nullable<int> bomDetailSn { get; set; }
        public int outType { get; set; }
        public string picker { get; set; }
        public double outAmount { get; set; }
        public double outCost { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }
        public string express { get; set; }
        public string expresscode { get; set; }
        public Nullable<System.DateTime> outDate { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails { get; set; }
    }
}
