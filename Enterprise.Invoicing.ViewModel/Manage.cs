using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    public class EmployeeModel
    {
        public int staffId { get; set; }
        public string staffName { get; set; }
        public int roleSn { get; set; }
        public int rigthType { get; set; }
        public bool showPrice { get; set; }
        public string roleName { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string duty { get; set; }
        public bool isUser { get; set; }
        public string userId { get; set; }
        public string userPwd { get; set; }
        public bool status { get; set; }
        public string remark { get; set; }
    }


    public class MaterialCostModel
    {
        public int costSn { get; set; }
        public string materialNo { get; set; }
        public string MaterialModel { get; set; }
        public string MaterialName { get; set; }
        public string th { get; set; }
        public string unit { get; set; }
        public string category { get; set; }
        public string shortInfo { get; set; }
        public string longInfo { get; set; }
    }
}
