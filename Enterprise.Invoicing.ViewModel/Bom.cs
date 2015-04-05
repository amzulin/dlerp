using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    public partial class V_BomMaterial
    {
        public int bomId { get; set; }
        public int xslength { get; set; }
        public int status { get; set; }
        public int loss { get; set; }
        public string materialNo { get; set; }
        public Nullable<int> parent_Id { get; set; }
        public Nullable<double> ratio { get; set; } 
        public string materialCate { get; set; }
        public string otherProject { get; set; }
        public double amount { get; set; }
        public string remark { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string bigcate { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public string tunumber { get; set; }
        public string version { get; set; }
        public string orderNo { get; set; }
        public bool valid { get; set; }


        public bool isChild { get; set; }
        public string bomName { get; set; }
        public Nullable<DateTime> startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }


        #region 出库
        public int outdetailsn { get; set; }
        public string outno { get; set; }
        public double outamount { get; set; }
        public int deoptid { get; set; }
        public string type { get; set; }
        #endregion
    }


    public class V_BomOrderDropList
    {

        public int supplierId { get; set; }
        public string supplierName { get; set; }

        public string bomOrderNO { get; set; }

        public int detailSn { get; set; }

        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }

        public int bomId { get; set; }
        public string materialCate { get; set; }

        public double orderAmount { get; set; }
        public double outAmount { get; set; }


    }
}
