using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductPullDetail
    {
        public int pullSn { get; set; }
        public string pullNo { get; set; }
        public int semiId { get; set; }
        public string materialNo { get; set; }
        public decimal theoryAmount { get; set; }
        public decimal realAmount { get; set; }
        public string remark { get; set; }
    }
}
