using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Log
    {
        public int logSn { get; set; }
        public string staffName { get; set; }
        public string logType { get; set; }
        public string logContent { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
