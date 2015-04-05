using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public string Cate { get; set; }
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Writer { get; set; }
        public string Info { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
