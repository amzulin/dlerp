using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Function
    {
        public Function()
        {
            this.FunctionRights = new List<FunctionRight>();
        }

        public string functionNo { get; set; }
        public string parentNo { get; set; }
        public string functionName { get; set; }
        public string remark { get; set; }
        public virtual ICollection<FunctionRight> FunctionRights { get; set; }
    }
}
