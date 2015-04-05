using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IPurchaseRepository
    {
        #region 物料相关
        IQueryable<Material> QueryMaterial(int valid);
        IQueryable<Supplier> QuerySupplier(int type, int valid);
        #endregion
        #region 采购需求
        IQueryable<PurchaseRequireMode> RequireList();
        List<V_PurchaseRequireMode> RequireList(string where, string orderby);
        string GetRequireNo();
        string AddRequire(string no, int staff, int dep, int status, string remark);
        ReturnValue SaveRequirDetail(string no, List<PurchaseRequireDetailModel> list, string remark);

        ReturnValue UpdateRequire(string no, int status, string remark);
        bool DeleteRequire(string no);

        IQueryable<PurchaseRequireDetailModel> RequireDetailList(string no);
        PurchaseRequireDetailModel RequireDetailOne(int sn);
        ReturnValue AddRequireDetail(string no, string material, decimal amount);
        ReturnValue UpdateRequireDetail(string no, int sn, string material,decimal amount);
        ReturnValue DeleteRequireDetail(string no,int sn);
        bool ChangeRequireStatus(string no, int status, string checkstaff);
        #endregion

        #region 采购单
        IQueryable<PurchaseModel> PurchaseList();
        List<V_PurchaseModel> PurchaseList(string where, string orderby);
        string GetPurchaseNo();
        string AddPurchase(string no, int staff, int dep,int supplier,int type, int status, string remark);
        ReturnValue SavePurchaseDetail(string no, List<PurchaseDetailModel> list, int supplier, string remark);
        ReturnValue SavePurchaseDetail2(string no, List<PurchaseDetailModel> list, int supplier, string remark);
        bool DeletePurchase(string no);
        IQueryable<PurchaseDetailModel> PurchaseDetailList(string no);
        bool ChangePurchaseStatus(string no, int status, string checkstaff);
        #endregion

        #region 申请单采购
        List<KeyValue> GetCurchaseHadOrder(string no);
        IQueryable<PurchaseDetailModel> PurchaseDetailList2(string no);
        PurchaseDetailModel PurchaseOneDetailList(int detailsn);
        List<KeyValue> GetNeedBuyOrder();

        List<PurchaseRequireDetailModel> GetPurchaseRequireByPurchaseNo(string require, string purchase);
      
        #endregion

        #region 采购单入库
        /// <summary>
        /// 查找入库单已有的采购单
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        List<KeyValue> GetStorkInHadOrder(string no);
        /// <summary>
        /// 得到供应商可以入库的采购单
        /// </summary>
        /// <param name="suppler"></param>
        /// <returns></returns>
        List<KeyValue> GetNeedStockInOrder(int suppler);
        /// <summary>
        /// 根据入库单和采购单得到入库明细模型
        /// </summary>
        /// <param name="no"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        List<StockDetailModel> GetStockHadDetailByPurchase(string no, string order);
        List<StockDetailModel> GetStockHadDetailByPurchase(string no);
        /// <summary>
        /// 将采购单转化为入库单明细模型
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        List<StockDetailModel> GetPurchaseStockDetail(string no);
        #endregion
    }
}
