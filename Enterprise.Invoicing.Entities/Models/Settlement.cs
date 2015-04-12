using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Settlement
    {
        public string settleNo { get; set; }
        public int settleType { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public string supplierCode { get; set; }
        public System.DateTime settleStart { get; set; }
        public System.DateTime settleEnd { get; set; }
        public decimal firstCost { get; set; }
        public decimal returnCost { get; set; }
        public decimal tradeCost { get; set; }
        public decimal realCost { get; set; }
        public decimal badCost { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public bool iscolse { get; set; }
        public int isover { get; set; }
        public System.DateTime createDate { get; set; }
        public string checkStaff { get; set; }
        public System.DateTime checkDate { get; set; }
        public string remark { get; set; }
    }
}
