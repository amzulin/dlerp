using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class FunctionRight
    {
        public int rightSn { get; set; }
        public int roleSn { get; set; }
        public string functionNo { get; set; }
        public virtual Function Function { get; set; }
        public virtual Role Role { get; set; }
    }
}
