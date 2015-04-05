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
    public class StockInService
    {
        private IStockInRepository _stockinRepository;
        public StockInService(IStockInRepository stockinRepository)
        {
            _stockinRepository = stockinRepository;
        }

        #region 仓库相关
        public List<KeyValue> GetMaterialCategory()
        {
            return _stockinRepository.GetMaterialCategory();
        }
        public List<KeyValue> GetMaterialUnit()
        {
            return _stockinRepository.GetMaterialUnit();
        }
        public List<KeyValue> GetMaterialNoFix()
        {
            return _stockinRepository.GetMaterialNoFix();
        }
        public List<Depot> QueryDepot(int valid)
        {
            return _stockinRepository.QueryDepot(valid).ToList();
        }

        public Depot GetOneDepot(int depotid)
        {
            return _stockinRepository.GetOneDepot(depotid);
        }
        public Material GetOneMaterial(string material)
        {
            return _stockinRepository.GetOneMaterial(material);
        }
        #endregion

        #region 产品入库
        public List<StockModel> StockInList(string no)
        {
            var list = _stockinRepository.StockInList();
            if (no != "")
            {
                return list.Where(p => p.stockNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_StockInModel> StockInList(string where, string orderby)
        {
            return _stockinRepository.StockInList( where,  orderby);
        }
        public string GetStockInNo()
        {
            return _stockinRepository.GetStockInNo();
        }
        public string GetStockSemiNo()
        {
            return _stockinRepository.GetStockSemiNo();
        }
        public string AddStockIn(int staff, int dep, int supplier, int intype, string remark,string deport)
        {
            return _stockinRepository.AddStockIn(staff, dep, supplier, intype, remark, deport);
        }
        public ReturnValue SaveStockInDetail(string no, List<StockDetailModel> list, int supplier, string remark,string deport)
        {
            return _stockinRepository.SaveStockInDetail(no, list, supplier, remark, deport);
        }

        public bool DeleteStockIn(string no)
        {
            return _stockinRepository.DeleteStockIn(no);
        }

        public IQueryable<StockDetailModel> StockInDetailList(string no)
        {
            return _stockinRepository.StockInDetailList(no);
        }
        public StockDetailModel StockInDetailOne(int sn)
        {
            return _stockinRepository.StockInDetailOne(sn);
        }
        public bool ChangeStockInStatus(string no, int status, string checkstaff)
        {
            return _stockinRepository.ChangeStockInStatus(no, status, checkstaff);
        }


        #endregion

        #region 退单
        public List<ReturnModel> ReturnModelList(string no)
        {
            var list = _stockinRepository.ReturnModelList();
            if (no != "")
            {
                return list.Where(p => p.returnNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_ReturnInModel> ReturnModelList(string where, string orderby)
        {
            return _stockinRepository.ReturnModelList(where, orderby);
        }
        public string GetReturnNo()
        {
            return _stockinRepository.GetReturnNo();
        }
        public string AddReturn(int staff, int dep, int supplier, string remark, string deport)
        {
            return _stockinRepository.AddReturn(staff, dep, supplier, remark, deport);
        }
        public ReturnValue SaveReturnDetail(string no, List<ReturnDetailModel> list, int supplier, string remark,string deport)
        {
            return _stockinRepository.SaveReturnDetail(no, list, supplier, remark, deport);
        }
        public bool DeleteReturn(string no)
        {
            return _stockinRepository.DeleteReturn(no);
        }
        public IQueryable<ReturnDetailModel> ReturnDetailList(string no)
        {
            return _stockinRepository.ReturnDetailList(no);
        }
        public List<ReturnDetailModel> ReturnDetailList(string no,string order)
        {
            var list= _stockinRepository.ReturnDetailList(no);
            return list.Where(p => p.purchaseNo == order).ToList();
        }
        public IQueryable<ReturnDetailModel> ReturnDetailListByOrder(string order)
        {
            return _stockinRepository.ReturnDetailListByOrder(order);
        }
        public ReturnDetailModel ReturnDetailOne(int sn)
        {
            return _stockinRepository.ReturnDetailOne(sn);
        }
        public ReturnValue ChangeReturnStatus(string no, int status, string checkstaff)
        {
            return _stockinRepository.ChangeReturnStatus(no, status, checkstaff);
        }

        public List<KeyValue> GetCanReturnOrders(int supplier, string key)
        {
            var list = _stockinRepository.CanReturnOrderList(supplier);
            if (key != "")
            {
                var l = list.Where(p => p.materialModel.Contains(key) || p.materialName.Contains(key) || p.materialNo.Contains(key) || p.purchaseNo.Contains(key)).
                    Select(p => new KeyValue { text=p.purchaseNo,value="" }).ToList();
                return l.Distinct().ToList();
            }
            else
            {
                var l = list.Select(p => new KeyValue { text = p.purchaseNo, value = "" }).ToList();
                return l.Distinct().ToList();
            }
        }
        //var had = purchaseService.GetStorkInHadOrder(no);
        //var all = purchaseService.GetNeedStockInOrder(supplier, query);
        public List<KeyValue> GetHadReturnOrders(string no)
        {
            var list= _stockinRepository.ReturnDetailList(no);
            var l = list.Select(p => new KeyValue { text = p.purchaseNo, value = p.returnNo }).ToList();
            return l.Distinct().ToList();
        }


        #endregion
    }
}
