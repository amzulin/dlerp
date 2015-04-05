using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    public class StockModel
    {
        public string stockNo { get; set; }
        public string picker { get; set; }
        public string pickerDep { get; set; }
        public int staffId { get; set; }
        public string staffName { get; set; }
        public string checkStaff { get; set; }
        public int datatype { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public double amount { get; set; }
        public double cost { get; set; }
        public double remain { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> outDate { get; set; }
        public bool valid { get; set; }
        public int isover { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }

        public Nullable<int> bomdetailsn { get; set; }

        public string bomOrderNo { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string express { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string expresscode { get; set; }
    }

    public class StockDetailModel
    {
        public int datatype { get; set; }
        public int detailSn { get; set; }
        public int depotId { get; set; }
        public string stockNo { get; set; }
        public string materialNo { get; set; }
        public double amount { get; set; }
        public double returnamount { get; set; }
        public double cost { get; set; }
        public double price { get; set; }

        public string depotName { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }
        public string type { get; set; }
        public string remark { get; set; }

        #region 采购单独有
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }
        public double orderAmout { get; set; }
        public double remainAmout { get; set; }
        #endregion

        #region 以旧换新
        public string changeNo { get; set; }
        /// <summary>
        /// 以旧 入库：0
        /// 换新 出库：1
        /// </summary>
        public int changeType { get; set; }
        public string picker { get; set; }
        public string pickerDep { get; set; }
        #endregion

        #region bom领料

        public Nullable<int> bomId { get; set; }
        #endregion

        #region 销售出库
        public double orderamount { get; set; }
        public Nullable<int> orderdetailsn { get; set; }
        #endregion
    }

    public class DepotDetailModel
    {
        public int depotId { get; set; }
        public string depotName { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }
        public string type { get; set; }  
        public bool valid { get; set; }
        public string remark { get; set; }

        public int detailSn { get; set; }
        public double amout { get; set; }
        public double cost { get; set; }
        public double safe { get; set; }
        public string deportStaff { get; set; }
    }

    public class ReturnModel
    {
        public string returnNo { get; set; }
        public int staffId { get; set; }
        public string staffName { get; set; }
        public string checkStaff { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public double amount { get; set; }
        public double cost { get; set; }
        public int status { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public string remark { get; set; }
        public string deportStaff { get; set; }
        /// <summary>
        /// 退意类别：0 一般销售退意，1 直接销售退单，2 领料退单
        /// </summary>
        public int returnType { get; set; }
        /// <summary>
        /// 退意类别：0 一般销售退意，1 直接销售退单，2 领料退单
        /// </summary>
        public string returnName { get; set; }
    }

    public class ReturnDetailModel
    {
        public int returnSn { get; set; }
        public string returnNo { get; set; }

        public string materialNo { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialUnit { get; set; }
        public string materialTu { get; set; }
        
        public int depotId { get; set; }
        public string depotName { get; set; }

        public double returnAmount { get; set; }
        public double orderAmoutn { get; set; }
        public double orderPrice { get; set; }

        public double inAmount { get; set; }
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }

        public string stockinNo { get; set; }
        public int stockinDetailSn { get; set; }

        public string type { get; set; }
        public string remark { get; set; }

    }

    public class StockReturnDetailModel
    {
        public int detailSn { get; set; }
        public string returnNo { get; set; }

        public string materialNo { get; set; }
        public string materialCategory { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string materialTu { get; set; }
        public string materialUnit { get; set; }

        public int fromdepotId { get; set; }
        public string fromdepotName { get; set; }

        public int todepotId { get; set; }
        public string todepotName { get; set; }

        public double returnAmount { get; set; }
        public double outAmoutn { get; set; }
        public double hadreturnAmount { get; set; }


        public string stockoutNo { get; set; }
        public int stockoutDetailSn { get; set; }

        public string type { get; set; }
        public string remark { get; set; }

    }

}
