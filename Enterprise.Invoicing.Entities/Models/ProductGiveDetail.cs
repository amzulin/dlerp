using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductGiveDetail
    {
        public int giveSn { get; set; }
        public string giveNo { get; set; }
        public int pullDetailSn { get; set; }
        public decimal giveAmount { get; set; }
        public string remark { get; set; }
    }
}
