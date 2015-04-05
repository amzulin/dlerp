using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockChangeModel
    {
        public string changeNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public string picker { get; set; }
        public string pickdep { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public string deportStaff { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
    }
}
