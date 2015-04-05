using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomOutModel
    {
        public int staffId { get; set; }
        public int depId { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string supplierName { get; set; }
        public double myOutAmount { get; set; }
        public string deportStaff { get; set; }
        public int detailSn { get; set; }
        public string bomOutNo { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string bomOrderNo { get; set; }
        public double totalAmount { get; set; }
        public double Price { get; set; }
        public double totalOutAmount { get; set; }
        public int bomId { get; set; }
        public string materialNo { get; set; }
        public string materialCate { get; set; }
        public double bomAmount { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
    }
}
