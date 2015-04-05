using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductGroup
    {
        public ProductGroup()
        {
            this.Products = new List<Product>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string ForAge { get; set; }
        public string EForAge { get; set; }
        public string ForWight { get; set; }
        public string EForWight { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
