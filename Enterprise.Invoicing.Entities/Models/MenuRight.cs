using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class MenuRight
    {
        public int rightSn { get; set; }
        public int roleSn { get; set; }
        public string menuNo { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual Role Role { get; set; }
    }
}
