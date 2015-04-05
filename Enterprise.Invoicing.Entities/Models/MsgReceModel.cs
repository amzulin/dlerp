using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class MsgReceModel
    {
        public System.Guid msgId { get; set; }
        public Nullable<System.Guid> parentId { get; set; }
        public string msgcate { get; set; }
        public string msgcontent { get; set; }
        public System.DateTime createDate { get; set; }
        public string receIds { get; set; }
        public string receNames { get; set; }
        public bool hadAttr { get; set; }
        public int sendstaffid { get; set; }
        public string sendstaffname { get; set; }
        public string senddepname { get; set; }
        public int receId { get; set; }
        public Nullable<int> recestaffid { get; set; }
        public string recestaffname { get; set; }
        public bool isRead { get; set; }
        public Nullable<System.DateTime> readDate { get; set; }
        public bool isDelete { get; set; }
        public Nullable<System.DateTime> deleteDate { get; set; }
        public string title { get; set; }
        public string fileGuid { get; set; }
        public string fileName { get; set; }
    }
}
