using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockOutDetail
    {
        public StockOutDetail()
        {
            this.StockReturnDetails = new List<StockReturnDetail>();
        }

        public int detailSn { get; set; }
        public string stockoutNo { get; set; }
        public string materialNo { get; set; }
        public int depotId { get; set; }
        public Nullable<int> orderSn { get; set; }
        public Nullable<int> bomId { get; set; }
        public double outAmount { get; set; }
        public double returnAmount { get; set; }
        public double outPrice { get; set; }
        public string changeNo { get; set; }
        public string remark { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Material Material { get; set; }
        public virtual StockOut StockOut { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails { get; set; }
    }
}
