using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class BomCost
    {
        public int costId { get; set; }
        public int bomId { get; set; }
        public double price { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public virtual BomMain BomMain { get; set; }
    }
}
