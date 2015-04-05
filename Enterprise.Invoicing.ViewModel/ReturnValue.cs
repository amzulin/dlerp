using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    public class ReturnValue
    {
        /// <summary>
        /// 运行结果：ok,error
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 返回值2
        /// </summary>
        public string value2 { get; set; }
        public string value3 { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }
    }
    public class KeyValue
    {
        public string text { get; set; }
        public string value { get; set; }
        public Nullable<double> double_value { get; set; }
        public string column1 { get; set; }
        public string column2 { get; set; }
        public string column3 { get; set; }
        public string column4 { get; set; }
        public string column5 { get; set; }
        public string remark { get; set; }
        public DateTime date { get; set; }
        public Guid guid { get; set; }
        public Guid role { get; set; }
        public bool valid { get; set; }
        public long sn { get; set; }
    }

    public class OneModel { public double need { get; set; } }
}
