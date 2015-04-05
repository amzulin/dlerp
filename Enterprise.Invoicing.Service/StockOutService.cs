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
    public class StockOutService
    {
        private IStockOutRepository _stockoutRepository;
        public StockOutService(IStockOutRepository stockoutRepository)
        {
            _stockoutRepository = stockoutRepository;
        }

        #region 产品出库
        public List<StockModel> StockOutList(string no)
        {
            var list = _stockoutRepository.StockOutList();
            if (no != "")
            {
                return list.Where(p => p.stockNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_StockOutModel> StockOutList(string where, string orderby)
        {
            return _stockoutRepository.StockOutList(where, orderby);
        }
        public string GetStockOutNo()
        {
            return _stockoutRepository.GetStockOutNo();
        }
        public string AddStockOut(int staff, int dep, int supplier, int outtype, string remark, string deportStaff)
        {
            return _stockoutRepository.AddStockOut(staff, dep, supplier, outtype, remark, deportStaff);
        }
        public ReturnValue SaveStockOutDetail(string no, List<StockDetailModel> list, int supplier, string remark, string deportStaff)
        {
            return _stockoutRepository.SaveStockOutDetail(no, list, supplier, remark, deportStaff);
        }

        public bool DeleteStockOut(string no)
        {
            return _stockoutRepository.DeleteStockOut(no);
        }

        public IQueryable<StockDetailModel> StockOutDetailList(string no)
        {
            return _stockoutRepository.StockOutDetailList(no);
        }
        public StockDetailModel StockOutDetailOne(int sn)
        {
            return _stockoutRepository.StockOutDetailOne(sn);
        }
        public ReturnValue ChangeStockOutStatus(string no, int status,string checkstaff)
        {
            return _stockoutRepository.ChangeStockOutStatus(no, status, checkstaff);
        }


        #endregion

        #region 仓库
        public List<Depot> GetDepots(int? depotid)
        {
            return _stockoutRepository.GetDepots(depotid).ToList();
        }
        public List<DepotDetailModel> GetDepotDetail(int depotid,string key)
        {
            var list= _stockoutRepository.GetDepotDetail(depotid);
            if (key!=null&&key!="")
            {
               return list.Where(p => p.materialCategory.Contains(key) || p.materialModel.Contains(key) || p.materialName.Contains(key) || p.materialUnit.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public IQueryable<Material> GetDepotMaterial(int depotid, int valid)
        {
            return _stockoutRepository.GetDepotMaterial(depotid, valid);
        }
        #endregion

        #region 以旧换新
        public List<StockModel> ChangeOtNList(string no)
        {
            var list =_stockoutRepository.ChangeOtNList(); 
            if (no != "")
            {
                return list.Where(p => p.stockNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_StockChangeModel> ChangeOtNList(string where, string orderby)
        {
            return _stockoutRepository.ChangeOtNList(where, orderby);
        }
        public IQueryable<StockDetailModel> ChangeOtNInList(string changeno)
        {
            return _stockoutRepository.ChangeOtNInList( changeno);
        }
        public IQueryable<StockDetailModel> ChangeOtNOutList(string changeno)
        {
            return _stockoutRepository.ChangeOtNOutList( changeno);
        }
        public string GetChangeOtNNo()
        {
            return _stockoutRepository.GetChangeOtNNo();
        }
        public string AddChangeOtN(int staff, int dep, string remark, string deportStaff)
        {
            return _stockoutRepository.AddChangeOtN(staff, dep, remark, deportStaff);
        }
        public ReturnValue SaveChangeOtNDetail(string no, List<StockDetailModel> inlist, List<StockDetailModel> outlist, string remark, string deportStaff)
        {
            return _stockoutRepository.SaveChangeOtNDetail(no, inlist, outlist, remark, deportStaff);
        }
        public bool DeleteChangeOtN(string changeno)
        {
            return _stockoutRepository.DeleteChangeOtN( changeno);
        }
        public ReturnValue ChangeChangeOtNStatus(string changeno, int status, string checkstaff)
        {
            return _stockoutRepository.ChangeChangeOtNStatus(changeno, status, checkstaff);
        }
        #endregion

        #region 退单
        public List<KeyValue> GetReturnHadOut(string no)
        {
            return _stockoutRepository.GetReturnHadOut( no);
        }
        public List<KeyValue> GetCanReturn(string order,int returntype)
        {
            var l = _stockoutRepository.GetCanReturnType(returntype); 
            if (order != null && order != "")
            {
                return l.Where(item => item.text.Contains(order.ToUpper()) || item.column1.Contains(order.ToUpper()) || item.column2.Contains(order.ToUpper()) || item.column3.Contains(order.ToUpper())).ToList();
            }
            else
            {
                return l;
            }
        }
        public List<ReturnModel> ReturnModelList(string no)
        {
            var list= _stockoutRepository.ReturnModelList();
            if (no != "")
            {
                return list.Where(p => p.returnNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_ReturnOutModel> ReturnModelList(string where, string orderby)
        {
            return _stockoutRepository.ReturnModelList(where, orderby);
        }
        public IQueryable<StockReturnDetailModel> StockReturnDetailList(string no)
        {
            return _stockoutRepository.StockReturnDetailList( no);
        }
        public List<StockReturnDetailModel> StockReturnDetailList(string no,string outno)
        {
            var list= _stockoutRepository.StockReturnDetailList(no);
            var l = list.ToList();
            return list.Where(p => p.stockoutNo == outno).ToList();
        }
        public IQueryable<StockReturnDetailModel> StockReturnDetailListByOut(string outno)
        {
            return _stockoutRepository.StockReturnDetailListByOut( outno);
        }
        public string GetStockReturnNo()
        {
            return _stockoutRepository.GetStockReturnNo();
        }
        public string AddStockReturn(int staff, int dep, string remark, string deportStaff,int returntype)
        {
            return _stockoutRepository.AddStockReturn( staff,  dep,   remark, deportStaff, returntype);
        }    
        public ReturnValue SaveStockReturnDetail(string no, List<StockReturnDetailModel> list, string remark, string deportStaff)
        {
            return _stockoutRepository.SaveStockReturnDetail( no,  list,  remark, deportStaff);
        }
        public bool DeleteStockReturn(string no)
        {
            return _stockoutRepository.DeleteStockReturn( no);
        }
        public ReturnValue ChangeStockReturnStatus(string no, int status, string checkstaff)
        {
            return _stockoutRepository.ChangeStockReturnStatus( no,  status, checkstaff);
        }
        #endregion

        #region 销售退单 
        public List<KeyValue> GetCanReturn(int supplierid, string order)
        {
            var l = _stockoutRepository.GetCanReturn(supplierid);
            if (order != null && order != "")
            {
                return l.Where(item => item.text.Contains(order.ToUpper()) || item.column1.Contains(order.ToUpper()) || item.column2.Contains(order.ToUpper()) || item.column3.Contains(order.ToUpper())).ToList();
            }
            else
            {
                return l;
            }
        }
        public List<ReturnModel> ReturnSellModelList(string no)
        {
            var list = _stockoutRepository.ReturnSellModelList();
            if (no != "")
            {
                return list.Where(p => p.returnNo.Contains(no)).ToList();
            }
            return list.ToList();
        }
        public List<V_ReturnOutModel> ReturnSellModelList(string where, string orderby)
        {
            return _stockoutRepository.ReturnSellModelList(where, orderby);
        }
        public string AddStockReturn(int staff, int dep, int supplier, string remark, string deportStaff, int returntype)
        {
            return _stockoutRepository.AddStockReturn( staff,  dep, supplier,   remark, deportStaff, returntype);
        }
        #endregion


        #region 报表
        public List<ReportDepot> ReportDepot(int depid,string category,string m,int less,int big)
        {
            var list = _stockoutRepository.ReportDepot();
            if (depid != 0) list=list.Where(p => p.depotId == depid);
            if (category != "" && category != "0") list = list.Where(p => p.materialCategory == category);
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less!=-404) list = list.Where(p => p.amout>=less);
            if (big != -404) list = list.Where(p => p.amout < big);
            return list.ToList();
        }

        public List<ReportPurchaseRequire> ReportPurchaseRequire(int depid, int staffid, int status, int valid, int isover, string no, string m, string start, string end, int less, int big)
        {
            var list = _stockoutRepository.ReportPurchaseRequire();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (isover != -1) list = list.Where(p => p.isover == isover);
            if (no != "") list = list.Where(p => p.requireNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.orderAmount >= less);
            if (big != -404) list = list.Where(p => p.orderAmount < big);
            //var result = list.ToList();
            if (start!="")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }
        public List<ReportPurchase> ReportPurchase(int depid, int staffid,int supplier,  int status, int valid, int isover, int type,int ret, string no, string m, string start, string end, int less, int big, float pless, float pbig)
        {
            var list = _stockoutRepository.ReportPurchase();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (supplier != 0) list = list.Where(p => p.supplierId == supplier);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (isover != -1) list = list.Where(p => p.isover == isover);
            if (no != "") list = list.Where(p => p.requireNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.poAmount >= less);
            if (big != -404) list = list.Where(p => p.poAmount < big);
            if (pless != -404) { var fless = Convert.ToDouble(pless); list = list.Where(p => p.poPrice >= fless); }
            if (pbig != -404) { var fbig = Convert.ToDouble(pbig); list = list.Where(p => p.poPrice < fbig); }
            if (type == 0) list = list.Where(p => p.type == "普通采购");
            if (type == 1) list = list.Where(p => p.type == "申请采购");
            if (ret == 0) list = list.Where(p => p.returnAmount==0);
            if (ret == 1) list = list.Where(p => p.returnAmount > 0);
            if (start!="")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }

        public List<ReportStock> ReportStockIn(int depid, int staffid,int depotid,int status, int valid,int intype, int ret, string no, string m, string start, string end, int less, int big)
        {
            var list = _stockoutRepository.ReportStockIn();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (depotid != 0) list = list.Where(p => p.depotId == depotid);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (no != "") list = list.Where(p => p.stockNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.amount >= less);
            if (big != -404) list = list.Where(p => p.amount < big);
            if (intype != -1) list = list.Where(p => p.datatype == intype);
            if (ret == 0) list = list.Where(p => p.returnamount == 0);
            if (ret == 1) list = list.Where(p => p.returnamount > 0);
            if (start != "")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }
       
        public List<ReportStock> ReportStockOut(int depid, int staffid, int depotid, int status, int valid, int supplier, int outtype, int ret, string no, string m, string start, string end, int less, int big)
        {
            var list = _stockoutRepository.ReportStockOut();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (depotid != 0) list = list.Where(p => p.depotId == depotid);
            if (supplier != 0) list = list.Where(p => p.supplierId == supplier);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (no != "") list = list.Where(p => p.stockNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.amount >= less);
            if (big != -404) list = list.Where(p => p.amount < big);
            if (outtype != -1) list = list.Where(p => p.datatype == outtype);
            if (ret == 0) list = list.Where(p => p.returnamount == 0);
            if (ret == 1) list = list.Where(p => p.returnamount > 0);
            if (start != "")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }

        public List<ReportStock> ReportStockOtN(int depid, int staffid, int depotid, int status, int valid, int intype, string no, string m, string start, string end, int less, int big)
        {
            var list = _stockoutRepository.ReportStockOtN();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (depotid != 0) list = list.Where(p => p.depotId == depotid);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (no != "") list = list.Where(p => p.stockNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.amount >= less);
            if (big != -404) list = list.Where(p => p.amount < big);
            if (intype != -1) list = list.Where(p => p.changeType == intype);
            if (start != "")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }

        public List<ReportPurchase> ReportOrderRetrun(int depid, int staffid, int depotid, int status, int valid, int supplier, string no, string m, string start, string end, int less, int big)
        { 
            var list = _stockoutRepository.ReportOrderRetrun();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (depotid != 0) list = list.Where(p => p.depotId == depotid);
            if (supplier != 0) list = list.Where(p => p.supplierId == supplier);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (no != "") list = list.Where(p => p.returnNo.Contains(no) || p.purchaseNo.Contains(no) || p.stockinNo.Contains(no));
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.returnAmount >= less);
            if (big != -404) list = list.Where(p => p.returnAmount < big);
            if (start != "")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }

        public List<ReportStock> ReportStockReturn(int depid, int staffid, int fdepotid, int tdepotid, int status, int valid, string no, string m, string start, string end, int less, int big)
        {
            var list = _stockoutRepository.ReportStockReturn();
            if (depid != 0) list = list.Where(p => p.depId == depid);
            if (staffid != 0) list = list.Where(p => p.staffId == staffid);
            if (fdepotid != 0) list = list.Where(p => p.fdepotId == fdepotid);
            if (tdepotid != 0) list = list.Where(p => p.tdepotId == tdepotid);
            if (status != -1) list = list.Where(p => p.status == status);
            if (valid == 1) list = list.Where(p => p.valid == true);
            if (valid == 0) list = list.Where(p => p.valid == false);
            if (no != "") list = list.Where(p => p.returnNo.Contains(no) || p.stockNo.Contains(no) );
            if (m != "") list = list.Where(p => p.materialName.Contains(m) || p.materialModel.Contains(m) || p.materialUnit.Contains(m) || p.materialNo.Contains(m));
            if (less != -404) list = list.Where(p => p.returnamount >= less);
            if (big != -404) list = list.Where(p => p.returnamount < big);
            if (start != "")
            {
                var ds = Convert.ToDateTime(start);
                list = list.Where(x => DateTime.Compare(x.createDate, ds) >= 0);
            }
            if (end != "")
            {
                var de = Convert.ToDateTime(end);
                list = list.Where(x => DateTime.Compare(x.createDate, de) < 0);
            }
            return list.ToList();
        }

        public List<V_ReportDepot> ReportDepot(string where, string orderby)
        {
            return _stockoutRepository.ReportDepot(where, orderby);
        }
        public List<V_ReceiptDetail> ReportReceiptDetail(string where, string orderby)
        {
            return _stockoutRepository.ReportReceiptDetail(where, orderby);
        }

        public List<MaterialCostModel> ReportCostDetail(string key)
        {
            return _stockoutRepository.ReportCostDetail(key);
        }
        
        #endregion
    }
}
