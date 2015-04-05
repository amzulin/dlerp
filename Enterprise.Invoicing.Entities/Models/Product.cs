using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductEName { get; set; }
        public string ProductModel { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Prokage { get; set; }
        public string LWH { get; set; }
        public string WightOnly { get; set; }
        public string WightAll { get; set; }
        public string ShortInfo { get; set; }
        public string MoreInfo { get; set; }
        public bool IndexShow { get; set; }
        public int OrderInt { get; set; }
        public int ImgCount { get; set; }
        public System.DateTime CreateDate { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
    }
}
