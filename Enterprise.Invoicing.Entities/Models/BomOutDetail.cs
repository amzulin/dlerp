using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomOutDetail
    {
        public int detailSn { get; set; }
        public string bomOutNo { get; set; }
        public int bomId { get; set; }
        public double amount { get; set; }
        public string remark { get; set; }
        public virtual BomMain BomMain { get; set; }
        public virtual BomOut BomOut { get; set; }
    }
}
