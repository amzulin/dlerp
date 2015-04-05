using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_ReturnInModel
    {
        public string returnNo { get; set; }
        public int depId { get; set; }
        public int staffId { get; set; }
        public int supplierId { get; set; }
        public double returnAmount { get; set; }
        public double returnCost { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public string deportStaff { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string supplierName { get; set; }
    }
}
