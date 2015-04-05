using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DelegateBack
    {
        public string backNo { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public string sendNo { get; set; }
        public System.DateTime backDate { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }
        public int status { get; set; }
        public bool canfs { get; set; }
        public bool vaild { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string deportStaff { get; set; }
    }
}
