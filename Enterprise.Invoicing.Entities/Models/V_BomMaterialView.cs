using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomMaterialView
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
        public string materialremark { get; set; }
    }
}
