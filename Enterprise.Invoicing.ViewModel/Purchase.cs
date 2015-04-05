using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    #region 采购申请 

    /// <summary>
    /// 采购申请单
    /// </summary>
    public class PurchaseRequireMode
    {
        public string requireNo { get; set; }
        public int staffId { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> status { get; set; }
        public System.DateTime createDate { get; set; }
        public string remark { get; set; }

        public string staffName { get; set; }
        public string checkStaff { get; set; }
        public string depName { get; set; } public bool valid { get; set; }
        public int isover { get; set; }
    }
    /// <summary>
    /// 申请明细
    /// </summary>
    public class PurchaseRequireDetailModel {
        /// <summary>
        /// 申请明细序号
        /// </summary>
        public int detailSn { get; set; }
        /// <summary>
        /// 申请单no
        /// </summary>
        public string requireNo { get; set; }
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
        /// <summary>
        /// 对应单次采购明细的采购数量
        /// </summary>
        public double mebuyAmount { get; set; }
        /// <summary>
        /// 对应单次采购明细的采购单价
        /// </summary>
        public double mebuyPrice { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime createDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string materialName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public int xslength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialUnit { get; set; }
        /// <summary>
        /// 临时操作的类别 add edit delete
        /// </summary>
        public string type { get; set; }

        public DateTime needdate { get; set; }
        public DateTime mysenddate { get; set; }

        public List<ReturnValue> priceList { get; set; }

    }
    #endregion

    #region 采购单
    public class PurchaseModel
    {
        public string purchaseNo { get; set; }
        public int staffId { get; set; }
        /// <summary>
        /// 0,普通采购单
        /// 1，申请单采购
        /// </summary>
        public int type { get; set; }
        public Nullable<int> depId { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> supplierId { get; set; }
        public System.DateTime createDate { get; set; }
        public double totalAmount { get; set; }
        public double totalCost { get; set; }
        public string remark { get; set; }

        public string staffName { get; set; }
        public string checkStaff { get; set; }
        public string depName { get; set; }
        public string suppliername { get; set; } public bool valid { get; set; }
        public int isover { get; set; }
    }
    public class PurchaseDetailModel
    {

        public int detailSn { get; set; }
        public string purchaseNo { get; set; }
        public string materialNo { get; set; }
        public double poAmount { get; set; }
        public double poPrice { get; set; }
        public double poRemain { get; set; }
        public double returnAmount { get; set; }
        public string remark { get; set; }

        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }
        public string type { get; set; }

        public  int requireDetailSn { get; set; }
        public double requireAmount { get; set; }
        public string requireNo { get; set; }
        public System.DateTime requireDate { get; set; }
        public System.DateTime sendDate { get; set; } 

    }
    #endregion
}
