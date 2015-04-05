using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockReturn
    {
        public StockReturn()
        {
            this.StockReturnDetails = new List<StockReturnDetail>();
        }

        public string returnNo { get; set; }
        public int depId { get; set; }
        public int staffId { get; set; }
        public Nullable<int> supplierId { get; set; }
        public int returnType { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public string deportStaff { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails { get; set; }
    }
}
