using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomMaerialTwo
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
        public Nullable<bool> valid { get; set; }
        public Nullable<int> xslength { get; set; }
        public string materialremark { get; set; }
        public string version { get; set; }
        public bool isChild { get; set; }
        public string bomName { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<System.Guid> bomguid { get; set; }
        public Nullable<int> parent_bomid { get; set; }
        public string parent_cate { get; set; }
        public string parent_verison { get; set; }
        public Nullable<bool> parent_ischild { get; set; }
        public string parent_bomname { get; set; }
        public string parent_materialName { get; set; }
        public string parent_materialModel { get; set; }
        public string parent_pinyin { get; set; }
        public string parent_tunumber { get; set; }
        public int rootId { get; set; }
        public string parent_materialNo { get; set; }
        public Nullable<System.DateTime> parent_start { get; set; }
        public Nullable<System.DateTime> parent_end { get; set; }
        public Nullable<int> parent_status { get; set; }
        public int status { get; set; }
    }
}
