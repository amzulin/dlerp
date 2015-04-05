using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            this.Products = new List<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryEName { get; set; }
        public int OrderInt { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
