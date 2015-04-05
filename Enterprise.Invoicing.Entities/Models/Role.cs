using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Role
    {
        public Role()
        {
            this.Employees = new List<Employee>();
            this.FunctionRights = new List<FunctionRight>();
            this.MenuRights = new List<MenuRight>();
        }

        public int roleSn { get; set; }
        public string roleName { get; set; }
        public bool showPrice { get; set; }
        public string remark { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<FunctionRight> FunctionRights { get; set; }
        public virtual ICollection<MenuRight> MenuRights { get; set; }
    }
}
