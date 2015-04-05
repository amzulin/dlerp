using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateBackDetail
    {
        public int detailSn { get; set; }
        public string backNo { get; set; }
        public string fromDelegateNo { get; set; }
        public bool isProduct { get; set; }
        public string materialNo { get; set; }
        public decimal backAmount { get; set; }
        public string remark { get; set; }
    }
}
