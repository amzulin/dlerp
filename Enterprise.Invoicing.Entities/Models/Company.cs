using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Company
    {
        public int Id { get; set; }
        public string ShowName { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string Remark { get; set; }
    }
}
