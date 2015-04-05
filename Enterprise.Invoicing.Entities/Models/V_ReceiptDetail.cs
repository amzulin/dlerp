using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ReceiptDetail
    {
        public string reprottype { get; set; }
        public Nullable<int> staffId { get; set; }
        public string staff { get; set; }
        public string deportStaff { get; set; }
        public string dep { get; set; }
        public Nullable<int> depid { get; set; }
        public string checkstaff { get; set; }
        public string showno { get; set; }
        public string suuplier { get; set; }
        public Nullable<int> supplierId { get; set; }
        public int type { get; set; }
        public string datatype { get; set; }
        public int status { get; set; }
        public string statustype { get; set; }
        public int isover { get; set; }
        public string overtype { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public string material { get; set; }
        public string materialNo { get; set; }
        public string bigcate { get; set; }
        public string category { get; set; }
        public string materialModel { get; set; }
        public string materialName { get; set; }
        public string materialTu { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string unit { get; set; }
        public string detail { get; set; }
        public double amount1 { get; set; }
        public double amount2 { get; set; }
        public double amount3 { get; set; }
        public Nullable<System.DateTime> detaildate { get; set; }
        public string detailremark { get; set; }
        public string linkno1 { get; set; }
        public double linkfor1 { get; set; }
        public string linkno2 { get; set; }
        public double linkfor2 { get; set; }
        public string depot { get; set; }
        public int depotid { get; set; }
        public string depotname { get; set; }
    }
}
