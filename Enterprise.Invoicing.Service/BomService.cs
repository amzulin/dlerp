using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Service
{
    public class BomService
    {
        private IBomRepository _BomRepository;
        public BomService(IBomRepository BomRepository)
        {
            _BomRepository = BomRepository;
        }

        #region Bom
        public IQueryable<V_BomMaterial> GetBomMaterial(string key, string valid)
        {
            var list = _BomRepository.GetBomMaterial();
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(p => p.version.Contains(key) || p.materialNo.Contains(key) || p.materialName.Contains(key) || p.materialModel.Contains(key) || p.tunumber.Contains(key));
            }
            if (!string.IsNullOrEmpty(valid))
            {
                if (valid == "0") list = list.Where(p => p.status == 0);
                else list = list.Where(p => p.status == 1);
            }
            return list;
        }
        public IQueryable<V_BomMaterial> GetBomMaterial()
        {
            return _BomRepository.GetBomMaterial();
        }
        public object GetOneBom(int bomid, int amount)
        {
            return _BomRepository.GetOneBom(bomid, amount);
        }
        public List<V_BomMaterial> GetChildBomMaterial(int BomId, int amount)
        {
            return _BomRepository.GetChildBomMaterial(BomId, amount);
        }
        public ReturnValue SaveBom(string type, int id, int? parent_Id, string material, string cate, double amount, string remark, string version)
        {
            return _BomRepository.SaveBom(type, id, parent_Id, material, cate, amount, remark, version);
        }
        public ReturnValue SaveVirtual(string type, int id, int bom, int forbom, string km, double amount, double price, string remark)
        {
            return _BomRepository.SaveVirtual(type, id, bom, forbom, km, amount, price, remark);
        }
        public ReturnValue DeleteBom(int id)
        {
            return _BomRepository.DeleteBom(id);
        }
        public ReturnValue DeleteVirtual(int id, int forbom)
        {
            return _BomRepository.DeleteVirtual(id, forbom);
        }
        public List<KeyValue> GetBomNodeCate()
        {
            return _BomRepository.GetBomNodeCate();
        }

        public V_BomMaerialTwo GetBomMaterialTwo(int bomid)
        {
            return _BomRepository.GetBomMaterialTwo(bomid);
        }
        public IQueryable<V_BomMaerialTwo> GetBomMaterialTwo(string key)
        {
            return _BomRepository.GetBomMaterialTwo(key);
        }
        #endregion

        #region Bom订单
        public List<V_BomOrderModel> BomOrderList(string no)
        {
            var list = _BomRepository.BomOrderList();
            if (no != "")
            {
                return list.Where(p => p.bomOrderNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_BomOrderModel> BomOrderList()
        {
            return _BomRepository.BomOrderList();
        }
        public List<V_BomOrderModel> BomOrderList(string where, string orderby)
        {
            return _BomRepository.BomOrderList(where, orderby);
        }
        public string GetBomOrderNo()
        {
            return _BomRepository.GetBomOrderNo();
        }
        public string AddBomOrder(BomOrder model)
        {
            return _BomRepository.AddBomOrder(model);
        }
        public ReturnValue SaveBomOrderDetail(string no, List<V_BomOrderDetailModel> list,
            string deportStaff, string remark)
        {
            return _BomRepository.SaveBomOrderDetail(no, list,
                        deportStaff, remark);
        }

        public bool DeleteBomOrder(string no)
        {
            return _BomRepository.DeleteBomOrder(no);
        }
        public bool DeleteBomOrderDetail(string no, int sn)
        {
            return _BomRepository.DeleteBomOrderDetail(no, sn);
        }
        public IQueryable<V_BomOrderDetailModel> BomOrderDetailList(string no)
        {
            return _BomRepository.BomOrderDetailList(no);
        }
        public List<V_BomOrderDetailModel> BomOrderDetailList(string where, string orderby)
        {
            return _BomRepository.BomOrderDetailList(where, orderby);
        }
        public V_BomOrderDetailModel BomOrderDetailOne(int sn)
        {
            return _BomRepository.BomOrderDetailOne(sn);
        }
        public ReturnValue ChangeBomOrderStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeBomOrderStatus(no, status, checkstaff);
        }
        public ReturnValue BomOrderDetailCreate(string no, int sn, string staff)
        {
            return _BomRepository.BomOrderDetailCreate(no, sn, staff);
        }
        public IQueryable<V_BomOrderDetailListModel> GetBomOrderBomDetailList(int sn)
        {
            return _BomRepository.GetBomOrderBomDetailList(sn);
        }
        public IQueryable<V_BomOrderVirtualDetail> GetBomOrderVirtualDetailList(int sn)
        {
            return _BomRepository.GetBomOrderVirtualDetailList(sn);
        }
        public ReturnValue SaveBomOrderDetail(string no, int sn, List<int> ids, List<double> counts, List<string> dates, List<string> remarks, string st)
        {
            return _BomRepository.SaveBomOrderDetail(no, sn, ids, counts, dates, remarks, st);
        }
        #endregion

        #region BOM领料
        public List<V_BomOrderDropList> GetBomOrderDropList(int supplieId)
        {
            return _BomRepository.GetBomOrderDropList(supplieId);

        }
        public List<V_BomMaterial> GetBomoutDetail(string outno)
        {
            return _BomRepository.GetBomoutDetail(outno);
        }
        public string AddStockOut(int staff, int dep, int supplier, int bomordersn, double amount, int outtype, string remark, string deportStaff)
        {
            return _BomRepository.AddStockOut(staff, dep, supplier, bomordersn, amount, outtype, remark, deportStaff);
        }
        public ReturnValue SaveStockOutDetail(string no, List<V_BomMaterial> list, double amount, string remark, string deportStaff)
        {
            return _BomRepository.SaveStockOutDetail(no, list, amount, remark, deportStaff);
        }
        #endregion


        public List<V_BomCostModel> GetProductBomCostModel(List<V_BomCostModel> list, double amount)
        {
            return _BomRepository.GetProductBomCostModel(list, amount);
        }
        public List<object> GetChildBomCost(List<V_BomCostModel> list, double amount, int index)
        {
            return _BomRepository.GetChildBomCost(list, amount, index);
        }

        #region  单据自动生成

        public ReturnValue CreatePurcharseRequire(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.CreatePurcharseRequire(no, sn, staff, dep);
        }
        public ReturnValue CreateDelegate(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.CreateDelegate(no, sn, staff, dep);
        }
        public ReturnValue CreateProduce(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.CreateProduce(no, sn, staff, dep);
        }

        public ReturnValue DeletePurcharseRequire(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.DeletePurcharseRequire(no, sn, staff, dep);
        }
        public ReturnValue DeleteDelegate(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.DeleteDelegate(no, sn, staff, dep);
        }
        public ReturnValue DeleteProduce(string no, int sn, int? staff, int? dep)
        {
            return _BomRepository.DeleteProduce(no, sn, staff, dep);
        }
        #endregion

        #region 委外
        public string GetDelegateNo()
        { return _BomRepository.GetDelegateNo(); }

        public ReturnValue ChangeDelegateStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeDelegateStatus(no, status, checkstaff);
        }
        #endregion
        #region 领料
        public string GetProductNo()
        { return _BomRepository.GetProductNo(); }

        public ReturnValue ChangeProductionStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeProductionStatus(no, status, checkstaff);
        }
        #endregion

        #region  子BOM
        public ReturnValue SaveChildBom(string type, int id, int? parent_Id, int childbom, string remark)
        {
            return _BomRepository.SaveChildBom(type, id, parent_Id, childbom, remark);
        }
        /// <summary>
        /// bom复制添加
        /// </summary>
        /// <param name="parent_id">新BOM节点的父级</param>
        /// <param name="childbom">新BOM要复制的对象</param>
        /// <returns></returns>
        public bool SaveChildBomNode(int parent_id, int childbom)
        {
            return _BomRepository.SaveChildBomNode(parent_id, childbom);
        }
        public List<object> GetBomTreeGrid(List<V_BomMaterial> list, int index)
        {
            return _BomRepository.GetBomTreeGrid(list, index);
        }
        #endregion


        #region 委外发货
        public string GetDelegateSendNo()
        {
            return _BomRepository.GetDelegateSendNo();
        }
        public ReturnValue ChangeDelegateSendStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeDelegateSendStatus(no, status, checkstaff);
        }
        #endregion
        #region 委外收货
        public string GetDelegateBackNo()
        {
            return _BomRepository.GetDelegateBackNo();
        }
        public ReturnValue ChangeDelegateBackStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeDelegateBackStatus(no, status, checkstaff);
        }
        #endregion
        #region BOM领料
        public string GetProductPullNo()
        {
            return _BomRepository.GetProductPullNo();
        }
        public ReturnValue ChangeProductPullStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeProductPullStatus(no, status, checkstaff);
        }
        #endregion
        #region BOM领料交货
        public string GetProductGiveNo()
        {
            return _BomRepository.GetProductGiveNo();
        }
        public ReturnValue ChangeProductGiveStatus(string no, int status, string checkstaff)
        {
            return _BomRepository.ChangeProductGiveStatus(no, status, checkstaff);
        }
        #endregion
    }
}
