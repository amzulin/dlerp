using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    [Serializable]
    public class LoginUser
    {
        public int staffid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 令牌号
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 当前客户号
        /// </summary>
        public string userpwd { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string role_name { get; set; }
        public int role_sn { get; set; }

        /// <summary>
        /// 权限类别：0 普通权限，1 部门权限，2 全局权限
        /// </summary>
        public int rigthType { get; set; }

        public string dep_name { get; set; }
        public int depId { get; set; }
     
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 登录时的sessionid
        /// </summary>
        public string sessionid { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime login_time { get; set; }

        public bool IsAdmin { get; set; }
        public bool showPrice { get; set; }

    }

    public class PrintHeadModel
    {
        public string No { get; set; }
        public string depName { get; set; }
        public DateTime date { get; set; }

        public string makeStaff { get; set; }
        public string checkStaff { get; set; }
        public string cfoStaff { get; set; }
        public string bossStaff { get; set; }

        public bool showdeport { get; set; }
        public string deportStaff { get; set; }

        public int supplierid { get; set; }
    }
}
