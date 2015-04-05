using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Attachment
    {
        public System.Guid attrGuid { get; set; }
        public string forKey { get; set; }
        public string fileGuid { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
