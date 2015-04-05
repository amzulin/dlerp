using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ReturnOutModel
    {
        public string returnNo { get; set; }
        public int depId { get; set; }
        public int staffId { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public string deportStaff { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public Nullable<int> supplierId { get; set; }
        public string supplierName { get; set; }
        public int returnType { get; set; }
        public string returnName { get; set; }
    }
}
