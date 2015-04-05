using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_PurchaseDetailModel
    {
        public string purchaseNo { get; set; }
        public int supplierId { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public int isover { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public string remark { get; set; }
        public string supplierName { get; set; }
        public string materialNo { get; set; }
        public double poAmount { get; set; }
        public double poPrice { get; set; }
        public double poRemain { get; set; }
        public double returnAmount { get; set; }
        public string returnNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string unit { get; set; }
        public string tunumber { get; set; }
        public string pinyin { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public string detailRemark { get; set; }
    }
}
