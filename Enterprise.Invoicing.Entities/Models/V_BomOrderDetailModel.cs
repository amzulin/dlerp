using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_BomOrderDetailModel
    {
        public string staffName { get; set; }
        public string depName { get; set; }
        public int bomId { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double OrderOutAmount { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public string createStaff { get; set; }
        public Nullable<System.DateTime> bomDate { get; set; }
        public string bomSerial { get; set; }
        public string OrderDetailRemark { get; set; }
        public string materialNo { get; set; }
        public Nullable<int> parent_Id { get; set; }
        public string materialCate { get; set; }
        public string bomOrderNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string tunumber { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string type { get; set; }
        public int detailSn { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public System.DateTime createDate { get; set; }
        public bool hadRequire { get; set; }
        public string orderType { get; set; }
        public string supplierName { get; set; }
        public double sellAmount { get; set; }
        public bool haddelegate { get; set; }
        public bool hadproduce { get; set; }
        public string requireNo { get; set; }
        public string delegateNo { get; set; }
        public string produceNo { get; set; }
        public string version { get; set; }
    }
}
