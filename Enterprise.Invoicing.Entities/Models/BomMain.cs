using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomMain
    {
        public BomMain()
        {
            this.BomCosts = new List<BomCost>();
            this.BomOrderDetailLists = new List<BomOrderDetailList>();
            this.BomOutDetails = new List<BomOutDetail>();
            this.BomVirtuals = new List<BomVirtual>();
            this.BomOrderDetails = new List<BomOrderDetail>();
        }

        public int bomId { get; set; }
        public string materialNo { get; set; }
        public Nullable<int> parent_Id { get; set; }
        public string materialCate { get; set; }
        public string otherProject { get; set; }
        public double amount { get; set; }
        public int loss { get; set; }
        public double rootCost { get; set; }
        public int status { get; set; }
        public string version { get; set; }
        public string remark { get; set; }
        public bool isChild { get; set; }
        public string bomName { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<System.Guid> bomguid { get; set; }
        public int rootId { get; set; }
        public virtual ICollection<BomCost> BomCosts { get; set; }
        public virtual Material Material { get; set; }
        public virtual ICollection<BomOrderDetailList> BomOrderDetailLists { get; set; }
        public virtual ICollection<BomOutDetail> BomOutDetails { get; set; }
        public virtual ICollection<BomVirtual> BomVirtuals { get; set; }
        public virtual ICollection<BomOrderDetail> BomOrderDetails { get; set; }
    }
}
