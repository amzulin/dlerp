using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class MsgRece
    {
        public int receId { get; set; }
        public Nullable<System.Guid> msgId { get; set; }
        public Nullable<int> staffId { get; set; }
        public bool isRead { get; set; }
        public Nullable<System.DateTime> readDate { get; set; }
        public bool isDelete { get; set; }
        public Nullable<System.DateTime> deleteDate { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual MsgSend MsgSend { get; set; }
    }
}
