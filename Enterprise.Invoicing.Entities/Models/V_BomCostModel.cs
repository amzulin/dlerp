using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomCostModel
    {
        public int bomId { get; set; }
        public Nullable<int> parent_Id { get; set; }
        public string materialCate { get; set; }
        public double amount { get; set; }
        public int loss { get; set; }
        public double rootCost { get; set; }
        public string bomremark { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public Nullable<double> ratio { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public Nullable<int> xslength { get; set; }
        public int status { get; set; }
        public string version { get; set; }
        public bool isChild { get; set; }
        public string bomName { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.Guid> bomguid { get; set; }
        public int rootId { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
    }
}
