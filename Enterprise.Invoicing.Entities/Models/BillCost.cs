using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BillCost
    {
        public BillCost()
        {
            this.BillCostDetails = new List<BillCostDetail>();
        }

        public string billNo { get; set; }
        public string title { get; set; }
        public Nullable<int> staffMake { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> staffCheck { get; set; }
        public Nullable<bool> checkRes { get; set; }
        public string checkMsg { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public Nullable<int> staffCfo { get; set; }
        public string cfoMsg { get; set; }
        public Nullable<bool> cfoRes { get; set; }
        public Nullable<System.DateTime> cfoDate { get; set; }
        public Nullable<int> staffBoss { get; set; }
        public Nullable<bool> bossRes { get; set; }
        public string bossMsg { get; set; }
        public Nullable<System.DateTime> bossDate { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public int billType { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Employee Employee2 { get; set; }
        public virtual Employee Employee3 { get; set; }
        public virtual ICollection<BillCostDetail> BillCostDetails { get; set; }
    }
}
