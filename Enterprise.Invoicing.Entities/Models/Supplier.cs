using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            this.BomOrders = new List<BomOrder>();
            this.DelegateSends = new List<DelegateSend>();
            this.MaterialPrices = new List<MaterialPrice>();
            this.Purchases = new List<Purchase>();
            this.PurchaseReturns = new List<PurchaseReturn>();
            this.StockIns = new List<StockIn>();
            this.StockOuts = new List<StockOut>();
        }

        public int supplierId { get; set; }
        public int type { get; set; }
        public string supplierName { get; set; }
        public string supplierNo { get; set; }
        public string person { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public bool valid { get; set; }
        public int fax { get; set; }
        public string remark { get; set; }
        public virtual ICollection<BomOrder> BomOrders { get; set; }
        public virtual ICollection<DelegateSend> DelegateSends { get; set; }
        public virtual ICollection<MaterialPrice> MaterialPrices { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; }
        public virtual ICollection<StockIn> StockIns { get; set; }
        public virtual ICollection<StockOut> StockOuts { get; set; }
    }
}
