using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BillCostDetail
    {
        public string billNo { get; set; }
        public string title { get; set; }
        public Nullable<int> staffMake { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> staffCheck { get; set; }
        public Nullable<System.DateTime> checkDate { get; set; }
        public Nullable<int> staffCfo { get; set; }
        public Nullable<System.DateTime> cfoDate { get; set; }
        public Nullable<int> staffBoss { get; set; }
        public Nullable<System.DateTime> bossDate { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public int billType { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<bool> checkRes { get; set; }
        public Nullable<bool> cfoRes { get; set; }
        public Nullable<bool> bossRes { get; set; }
        public string makeName { get; set; }
        public string checkName { get; set; }
        public string cfoName { get; set; }
        public string bossName { get; set; }
        public string depName { get; set; }
        public string checkMsg { get; set; }
        public string cfoMsg { get; set; }
        public string bossMsg { get; set; }
        public string billTitle { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public System.DateTime billDate { get; set; }
        public string detailRemark { get; set; }
        public int detailSn { get; set; }
    }
}
