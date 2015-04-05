using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Material
    {
        public Material()
        {
            this.BomMains = new List<BomMain>();
            this.DepotDetails = new List<DepotDetail>();
            this.MaterialPrices = new List<MaterialPrice>();
            this.ProduceDetails = new List<ProduceDetail>();
            this.Productions = new List<Production>();
            this.PurchaseDetails = new List<PurchaseDetail>();
            this.PurchaseRequireDetails = new List<PurchaseRequireDetail>();
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
            this.StockInDetails = new List<StockInDetail>();
            this.StockOutDetails = new List<StockOutDetail>();
            this.StockReturnDetails = new List<StockReturnDetail>();
        }

        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string bigcate { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public string orderNo { get; set; }
        public bool valid { get; set; }
        public int xslength { get; set; }
        public string remark { get; set; }
        public Nullable<decimal> price { get; set; }
        public string image { get; set; }
        public virtual ICollection<BomMain> BomMains { get; set; }
        public virtual ICollection<DepotDetail> DepotDetails { get; set; }
        public virtual ICollection<MaterialPrice> MaterialPrices { get; set; }
        public virtual ICollection<ProduceDetail> ProduceDetails { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<PurchaseRequireDetail> PurchaseRequireDetails { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails { get; set; }
    }
}
