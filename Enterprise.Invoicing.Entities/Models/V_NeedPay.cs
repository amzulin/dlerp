using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_NeedPay
    {
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public Nullable<double> cost { get; set; }
        public Nullable<System.DateTime> dateStart { get; set; }
        public Nullable<System.DateTime> dateEnd { get; set; }
        public Nullable<int> amount { get; set; }
    }
}
