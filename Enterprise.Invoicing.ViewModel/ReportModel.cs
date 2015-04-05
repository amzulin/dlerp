using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    public class ReportDepot
    {
        public int depotId { get; set; }
        public string depotName { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }
        public string materialNo { get; set; }
        public double amout { get; set; }
        public string remark { get; set; }
    }
    public class ReportPurchaseRequire
    {
        public string requireNo { get; set; }
        public int staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> status { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime needdate { get; set; }
        public string remark { get; set; }

        public string staffName { get; set; }
        public string depName { get; set; } 
        public bool valid { get; set; }
        public int isover { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string materialNo { get; set; }
        /// <summary>
        /// 申请数量
        /// </summary>
        public double orderAmount { get; set; }
        /// <summary>
        /// 本申请已采购总数
        /// </summary>
        public double buyAmount { get; set; }
        
        public string materialTu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialUnit { get; set; }
    }
    public class ReportPurchase
    {
        public string purchaseNo { get; set; }
        /// <summary>
        /// 0,普通采购单
        /// 1，申请单采购
        /// </summary>
        public string type { get; set; }
        public int supplierId { get; set; }
        public string suppliername { get; set; }
        public int staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> status { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime sendDate { get; set; }
        public string remark { get; set; }
        public string depotStaff { get; set; }

        public string staffName { get; set; }
        public string depName { get; set; } 
        public bool valid { get; set; }
        public int isover { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string materialNo { get; set; }
        public double poAmount { get; set; }
        public double poPrice { get; set; }
        public double poRemain { get; set; }
        public double returnAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string materialName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialUnit { get; set; }

        public string requireNo { get; set; }

        public string returnNo { get; set; } 
        public string stockinNo { get; set; }    
        public int depotId { get; set; }
        public string depotName { get; set; }
    }
    public class ReportStock
    {
        public string stockNo { get; set; }
        public int staffId { get; set; }
        public string staffName { get; set; }
        public int datatype { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }



        public int depotId { get; set; }
        public string materialNo { get; set; }
        public double amount { get; set; }
        public double returnamount { get; set; }

        public string depotName { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }
        public string type { get; set; }

        /// <summary>
        /// 以旧换新
        /// </summary>
        public string changeNo { get; set; }
        /// <summary>
        /// 以旧 入库：0
        /// 换新 出库：1
        /// </summary>
        public int changeType { get; set; }

        #region 退单
        public string returnNo { get; set; }

        public int fdepotId { get; set; }
        public string fdepotName { get; set; }

        public int tdepotId { get; set; }
        public string tdepotName { get; set; }
        #endregion
    }


}
