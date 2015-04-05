using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IBomRepository
    {
        #region Bom
        IQueryable<V_BomMaterial> GetBomMaterial();
        V_BomMaterial GetBomMaterial(int bomid);
        /// <summary>
        /// 得到一个V_BomMaterial的根级V_BomMaterial（BOM产品）
        /// </summary>
        /// <param name="bom"></param>
        /// <returns></returns>
        V_BomMaerialTwo GetBomRoot(V_BomMaerialTwo bom);
        object GetOneBom(int bomid, double amount);
        List<V_BomMaterial> GetChildBomMaterial(int BomId, double amount);
        ReturnValue SaveBom(string type, int id, int? parent_Id, string material, string cate, double amount, string remark, string version);
        ReturnValue SaveVirtual(string type, int id, int bom, int forbom, string km, double amount, double price, string remark);
        ReturnValue DeleteBom(int id); ReturnValue DeleteVirtual(int id, int forbom);
        List<KeyValue> GetBomNodeCate();

         IQueryable<V_BomMaerialTwo> GetBomMaterialTwo(string key);
         V_BomMaerialTwo GetBomMaterialTwo(int bomid);
        #endregion

        #region Bom订单
        List<V_BomOrderModel> BomOrderList();
        List<V_BomOrderModel> BomOrderList(string where, string orderby);
        string GetBomOrderNo();
        string AddBomOrder(BomOrder model);
        ReturnValue SaveBomOrderDetail(string no, List<V_BomOrderDetailModel> list, string deportStaff, string remark);

        bool DeleteBomOrder(string no);
        bool DeleteBomOrderDetail(string no, int sn);
        IQueryable<V_BomOrderDetailModel> BomOrderDetailList(string no);
        List<V_BomOrderDetailModel> BomOrderDetailList(string where, string orderby);
        V_BomOrderDetailModel BomOrderDetailOne(int sn);
        ReturnValue ChangeBomOrderStatus(string no, int status, string checkstaff);
        ReturnValue BomOrderDetailCreate(string no, int sn, string staff);
        IQueryable<V_BomOrderDetailListModel> GetBomOrderBomDetailList(int sn);
        IQueryable<V_BomOrderVirtualDetail> GetBomOrderVirtualDetailList(int sn);
        ReturnValue SaveBomOrderDetail(string no, int sn, List<int> ids, List<double> counts, List<string> dates, List<string> remarks, string st);
        #endregion

        #region BOM领料
        List<V_BomOrderDropList> GetBomOrderDropList(int supplieId);
        List<V_BomMaterial> GetBomoutDetail(string outno);
        string AddStockOut(int staff, int dep, int supplier, int bomordersn, double amount, int outtype, string remark, string deportStaff);
        ReturnValue SaveStockOutDetail(string no, List<V_BomMaterial> list, double amount, string remark, string deportStaff);
        #endregion

        List<V_BomCostModel> GetProductBomCostModel(List<V_BomCostModel> list, double amount);
        List<object> GetChildBomCost(List<V_BomCostModel> list, double amount, int index);

        #region  单据自动生成

        ReturnValue CreatePurcharseRequire(string no, int sn, int? staff, int? dep);
        ReturnValue CreateDelegate(string no, int sn, int? staff, int? dep);
        ReturnValue CreateProduce(string no, int sn, int? staff, int? dep);
        ReturnValue DeletePurcharseRequire(string no, int sn, int? staff, int? dep);
        ReturnValue DeleteDelegate(string no, int sn, int? staff, int? dep);
        ReturnValue DeleteProduce(string no, int sn, int? staff, int? dep);
        #endregion

        #region 委外
        string GetDelegateNo();
        ReturnValue ChangeDelegateStatus(string no, int status, string checkstaff);
        #endregion
        #region 领料
        string GetProductNo();
        ReturnValue ChangeProductionStatus(string no, int status, string checkstaff);
        #endregion

        #region 子BOM
        ReturnValue SaveChildBom(string type, int id, int? parent_Id, int childbom, string remark);
        /// <summary>
        /// bom复制添加
        /// </summary>
        /// <param name="parent_id">新BOM节点的父级</param>
        /// <param name="childbom">新BOM要复制的对象</param>
        /// <returns></returns>
        bool SaveChildBomNode(int parent_id, int childbom);
        List<object> GetBomTreeGrid(List<V_BomMaterial> list, int index);
        #endregion


        #region 委外发货
        string GetDelegateSendNo();
        ReturnValue ChangeDelegateSendStatus(string no, int status, string checkstaff);
        #endregion
        #region 委外收货
        string GetDelegateBackNo();
        ReturnValue ChangeDelegateBackStatus(string no, int status, string checkstaff);
        #endregion
        #region BOM领料
        string GetProductPullNo();
        ReturnValue ChangeProductPullStatus(string no, int status, string checkstaff);
        #endregion
        #region BOM领料交货
        string GetProductGiveNo();
        ReturnValue ChangeProductGiveStatus(string no, int status, string checkstaff);
        #endregion
    }
}
