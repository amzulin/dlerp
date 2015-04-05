using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductModel { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Prokage { get; set; }
        public string LWH { get; set; }
        public string WightOnly { get; set; }
        public string WightAll { get; set; }
        public string MoreInfo { get; set; }
        public int OrderInt { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CategoryName { get; set; }
        public int CateOrder { get; set; }
        public string ShortInfo { get; set; }
        public bool IndexShow { get; set; }
        public string ForWight { get; set; }
        public string ForAge { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> GroupId { get; set; }
        public int ImgCount { get; set; }
        public string ProductEName { get; set; }
        public string CategoryEName { get; set; }
        public string EForAge { get; set; }
        public string EForWight { get; set; }
    }
}
