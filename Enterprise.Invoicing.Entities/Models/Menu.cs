using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Menu
    {
        public Menu()
        {
            this.MenuRights = new List<MenuRight>();
        }

        public string menuNo { get; set; }
        public string parentNo { get; set; }
        public string menuName { get; set; }
        public string menuUrl { get; set; }
        public int menuType { get; set; }
        public string remark { get; set; }
        public virtual ICollection<MenuRight> MenuRights { get; set; }
    }
}
