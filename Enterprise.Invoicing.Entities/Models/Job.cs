using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int OrderInt { get; set; }
        public string Category { get; set; }
    }
}
