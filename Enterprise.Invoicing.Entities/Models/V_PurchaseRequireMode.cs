using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_PurchaseRequireMode
    {
        public string requireNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public int status { get; set; }
        public int isover { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public string depName { get; set; }
        public string staffName { get; set; }
        public string bomOrderNo { get; set; }
        public bool canfs { get; set; }
    }
}
