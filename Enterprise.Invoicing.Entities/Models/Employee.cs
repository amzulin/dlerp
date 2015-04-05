using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Employee
    {
        public Employee()
        {
            this.BillCosts = new List<BillCost>();
            this.BillCosts1 = new List<BillCost>();
            this.BillCosts2 = new List<BillCost>();
            this.BillCosts3 = new List<BillCost>();
            this.BomOrders = new List<BomOrder>();
            this.BomOuts = new List<BomOut>();
            this.DelegateSends = new List<DelegateSend>();
            this.MaterialPrices = new List<MaterialPrice>();
            this.MsgReces = new List<MsgRece>();
            this.MsgSends = new List<MsgSend>();
            this.Productions = new List<Production>();
            this.Purchases = new List<Purchase>();
            this.PurchaseRequires = new List<PurchaseRequire>();
            this.PurchaseReturns = new List<PurchaseReturn>();
            this.StockExchanges = new List<StockExchange>();
            this.StockIns = new List<StockIn>();
            this.StockOuts = new List<StockOut>();
            this.StockReturns = new List<StockReturn>();
        }

        public int staffId { get; set; }
        public string staffName { get; set; }
        public Nullable<int> roleSn { get; set; }
        public int depId { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string duty { get; set; }
        public bool isUser { get; set; }
        public string userId { get; set; }
        public string userPwd { get; set; }
        public int rigthType { get; set; }
        public bool status { get; set; }
        public string remark { get; set; }
        public virtual ICollection<BillCost> BillCosts { get; set; }
        public virtual ICollection<BillCost> BillCosts1 { get; set; }
        public virtual ICollection<BillCost> BillCosts2 { get; set; }
        public virtual ICollection<BillCost> BillCosts3 { get; set; }
        public virtual ICollection<BomOrder> BomOrders { get; set; }
        public virtual ICollection<BomOut> BomOuts { get; set; }
        public virtual ICollection<DelegateSend> DelegateSends { get; set; }
        public virtual Department Department { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<MaterialPrice> MaterialPrices { get; set; }
        public virtual ICollection<MsgRece> MsgReces { get; set; }
        public virtual ICollection<MsgSend> MsgSends { get; set; }
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
