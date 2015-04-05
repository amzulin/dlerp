using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Department
    {
        public Department()
        {
            this.BillCosts = new List<BillCost>();
            this.BomOrders = new List<BomOrder>();
            this.BomOuts = new List<BomOut>();
            this.DelegateSends = new List<DelegateSend>();
            this.Employees = new List<Employee>();
            this.Productions = new List<Production>();
            this.Purchases = new List<Purchase>();
            this.PurchaseRequires = new List<PurchaseRequire>();
            this.PurchaseReturns = new List<PurchaseReturn>();
            this.StockExchanges = new List<StockExchange>();
            this.StockIns = new List<StockIn>();
            this.StockOuts = new List<StockOut>();
            this.StockReturns = new List<StockReturn>();
        }

        public int depId { get; set; }
        public string depName { get; set; }
        public string phone { get; set; }
        public string leader { get; set; }
        public bool valid { get; set; }
        public string remark { get; set; }
        public virtual ICollection<BillCost> BillCosts { get; set; }
        public virtual ICollection<BomOrder> BomOrders { get; set; }
        public virtual ICollection<BomOut> BomOuts { get; set; }
        public virtual ICollection<DelegateSend> DelegateSends { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<PurchaseRequire> PurchaseRequires { get; set; }
        public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; }
        public virtual ICollection<StockExchange> StockExchanges { get; set; }
        public virtual ICollection<StockIn> StockIns { get; set; }
        public virtual ICollection<StockOut> StockOuts { get; set; }
        public virtual ICollection<StockReturn> StockReturns { get; set; }
    }
}
