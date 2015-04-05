using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class MsgSendModel
    {
        public System.Guid msgId { get; set; }
        public Nullable<System.Guid> parentId { get; set; }
        public int staffId { get; set; }
        public string msgcate { get; set; }
        public string msgcontent { get; set; }
        public System.DateTime createDate { get; set; }
        public string receIds { get; set; }
        public string receNames { get; set; }
        public bool hadAttr { get; set; }
        public string staffName { get; set; }
        public string depName { get; set; }
        public string title { get; set; }
        public bool isDelete { get; set; }
        public Nullable<System.DateTime> deleteDate { get; set; }
        public string fileGuid { get; set; }
        public string fileName { get; set; }
    }
}
