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
    public class PurchaseService
    {
        private IPurchaseRepository _purchaseRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        #region 物料相关
        public IQueryable<Material> QueryMaterial(int valid)
        {
            return _purchaseRepository.QueryMaterial(valid);
        }
        public Material QueryOneMaterial(string no,int valid)
        {
            var list = _purchaseRepository.QueryMaterial(valid);
            return list.FirstOrDefault(p => p.materialNo == no);

        }
        public IQueryable<Supplier> QuerySupplier(int type, int valid)
        {
            return _purchaseRepository.QuerySupplier(type,  valid);
        }
        #endregion

        #region 采购需求

        public List<PurchaseRequireMode> RequireList(string no)
        {
            var list= _purchaseRepository.RequireList();
            if (no != null && no != "") return list.Where(p => p.requireNo.Contains(no)).ToList();
            return list.ToList();
        }

        public List<V_PurchaseRequireMode> RequireList(string where, string orderby)
        {
            return _purchaseRepository.RequireList(where, orderby);
        }

        public PrintHeadModel GetRequirePrintHeadModel(string no)
        {
            var one = _purchaseRepository.RequireList().Where(p => p.requireNo == no).Select(x => new PrintHeadModel { checkStaff= x.checkStaff, date=x.createDate, depName=x.depName, makeStaff=x.staffName, No=x.requireNo }).FirstOrDefault();
            return one;
        }
        public string GetRequireNo()
        {
            return _purchaseRepository.GetRequireNo();
        }

        public string AddRequire(string no, int staff, int dep, int status, string remark)
        {
            return _purchaseRepository.AddRequire(no, staff, dep, status, remark);
        }

        public ReturnValue SaveRequirDetail(string no, List<PurchaseRequireDetailModel> list,string remark)
        {
            return _purchaseRepository.SaveRequirDetail(no, list, remark);
        }








        public ViewModel.ReturnValue UpdateRequire(string no, int status, string remark)
        {
            return _purchaseRepository.UpdateRequire(no,  status, remark);
        }

        public bool DeleteRequire(string no)
        {
            return _purchaseRepository.DeleteRequire(no);
        }

        public List<PurchaseRequireDetailModel> RequireDetailList(string no)
        {
            return _purchaseRepository.RequireDetailList(no).ToList();
        }
        public List<string> GetRequireDetailStrList(string no)
        {
            var list = _purchaseRepository.RequireDetailList(no).ToList();
            var l2 = list.Select(x => x.materialName + "," + x.materialModel + "," + x.materialUnit + "," + x.orderAmount.ToString()).ToList();
            return l2;
        }
        public PurchaseRequireDetailModel RequireDetailOne(int sn)
        {
            return _purchaseRepository.RequireDetailOne(sn);
        }
        public ViewModel.ReturnValue AddRequireDetail(string no, string material, decimal amount)
        {
            return _purchaseRepository.AddRequireDetail(no,material,  amount);
        }

        public ViewModel.ReturnValue UpdateRequireDetail(string no, int sn, string material, decimal amount)
        {
            return _purchaseRepository.UpdateRequireDetail(no, sn, material, amount);
        }

        public ViewModel.ReturnValue DeleteRequireDetail(string no, int sn)
        {
            return _purchaseRepository.DeleteRequireDetail(no, sn);
        }
        #endregion

        #region 采购单
        public List<PurchaseModel> PurchaseList(string no)
        {
            var list = _purchaseRepository.PurchaseList();
            if (no != null && no != "") return list.Where(p => p.purchaseNo.Contains(no)).ToList();
            return list.ToList();
        }
        public List<V_PurchaseModel> PurchaseList(string where, string orderby)
        {
            return _purchaseRepository.PurchaseList(where, orderby);
        }
        public PrintHeadModel GetPurchasePrintHeadModel(string no)
        {
            var one = _purchaseRepository.PurchaseList().Where(p => p.purchaseNo == no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = x.depName, makeStaff = x.staffName, No = x.purchaseNo }).FirstOrDefault();
            return one;
        }
        public string GetPurchaseNo()
        {
            return _purchaseRepository.GetPurchaseNo();
        }
        public string AddPurchase(string no, int staff, int dep, int supplier, int type, int status, string remark)
        {
            return _purchaseRepository.AddPurchase(no,  staff,  dep,  supplier,  type,  status,  remark);
        }
        public ReturnValue SavePurchaseDetail(string no, List<PurchaseDetailModel> list, int supplier, string remark)
        {
            return _purchaseRepository.SavePurchaseDetail(no, list, supplier, remark);
        }
        public bool DeletePurchase(string no)
        {
            return _purchaseRepository.DeletePurchase(no);
        }
        public List<PurchaseDetailModel> PurchaseDetailList(string no)
        {
            return _purchaseRepository.PurchaseDetailList(no).ToList(); 
        }
        public PurchaseDetailModel PurchaseOneDetailList(int detailsn)
        {
            return _purchaseRepository.PurchaseOneDetailList(detailsn);
        }
        public bool ChangeRequireStatus(string no, int status, string checkstaff)
        {
            return _purchaseRepository.ChangeRequireStatus(no, status, checkstaff);
        }
        #endregion

        #region 申请单采购
        public List<KeyValue> GetCurchaseHadOrder(string no)
        {
            return _purchaseRepository.GetCurchaseHadOrder(no);
        }
        public List<PurchaseDetailModel> PurchaseDetailList2(string no)
        {
            return _purchaseRepository.PurchaseDetailList2(no).ToList();
        }
        public List<KeyValue> GetNeedBuyOrder(string order)
        {
            var l = _purchaseRepository.GetNeedBuyOrder();
            if (order != null && order != "")
            {
                return l.Where(item => item.text.Contains(order.ToUpper()) || item.column1.Contains(order.ToUpper()) || item.column2.Contains(order.ToUpper()) || item.column3.Contains(order.ToUpper())).ToList();
            }
            else
            {
                return l;
            }
        }

        public List<PurchaseRequireDetailModel> GetPurchaseRequireByPurchaseNo(string require, string purchase)
        {
            return _purchaseRepository.GetPurchaseRequireByPurchaseNo(require, purchase);
        }
        public ReturnValue SavePurchaseDetail2(string no, List<PurchaseDetailModel> list, int supplier, string remark)
        {
            return _purchaseRepository.SavePurchaseDetail2(no, list, supplier, remark);
        }
        public bool ChangePurchaseStatus(string no, int status, string checkstaff)
        {
            return _purchaseRepository.ChangePurchaseStatus(no, status, checkstaff);
        }
        #endregion

        #region 采购单入库

        public List<KeyValue> GetStorkInHadOrder(string no)
        {
            return _purchaseRepository.GetStorkInHadOrder(no);
        }
        public List<KeyValue> GetNeedStockInOrder(int suppler,string order)
        {
            List<KeyValue> list = new List<KeyValue>();
            var l = _purchaseRepository.GetNeedStockInOrder(suppler);
            if (order != null && order != "")
            {
                foreach (var item in l)
                {
                    var h = list.FirstOrDefault(p => p.text == item.text);
                    if (h != null) continue;
                    if (item.text.Contains(order.ToUpper()) || item.column1.Contains(order.ToUpper()) || item.column2.Contains(order.ToUpper()) || item.column3.Contains(order.ToUpper()))
                    {
                        list.Add(new KeyValue { text = item.text, value = "" });
                    }
                }
                return list;
            }
            else
            {
                return l;
            }
        }
        public List<StockDetailModel> GetStockHadDetailByPurchase(string no, string order)
        {
            return _purchaseRepository.GetStockHadDetailByPurchase(no, order);
        } 
        public List<StockDetailModel> GetStockHadDetailByPurchase(string no)
        {
            return _purchaseRepository.GetStockHadDetailByPurchase(no);
        }
        public List<StockDetailModel> GetPurchaseStockDetail(string no)
        {
            return _purchaseRepository.GetPurchaseStockDetail(no);
        }
        #endregion
    }
}
