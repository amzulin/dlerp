using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IStockOutRepository
    {

        #region 产品入库
        IQueryable<StockModel> StockOutList();
        List<V_StockOutModel> StockOutList(string where, string orderby);
        string GetStockOutNo();
        string AddStockOut(int staff, int dep, int supplier, int outtype, string remark, string deportStaff);
        ReturnValue SaveStockOutDetail(string no, List<StockDetailModel> list, int supplier, string remark, string deportStaff);

        bool DeleteStockOut(string no);

        IQueryable<StockDetailModel> StockOutDetailList(string no);
        StockDetailModel StockOutDetailOne(int sn);
        ReturnValue ChangeStockOutStatus(string no, int status, string checkstaff);
        #endregion

        #region 仓库
        IQueryable<Depot> GetDepots(int? depotid);
        IQueryable<DepotDetailModel> GetDepotDetail(int depotid);
        IQueryable<Material> GetDepotMaterial(int depotid, int valid);
        #endregion

        #region 以旧换新
        IQueryable<StockModel> ChangeOtNList();
        List<V_StockChangeModel> ChangeOtNList(string where, string orderby);
        IQueryable<StockDetailModel> ChangeOtNInList(string changeno);
        IQueryable<StockDetailModel> ChangeOtNOutList(string changeno);
        string GetChangeOtNNo();
        string AddChangeOtN(int staff, int dep, string remark, string deportStaff);
        ReturnValue SaveChangeOtNDetail(string no, List<StockDetailModel> inlist, List<StockDetailModel> outlist, string remark, string deportStaff);
        bool DeleteChangeOtN(string changeno);
        ReturnValue ChangeChangeOtNStatus(string changeno, int status, string checkstaff);
        #endregion

        #region 退单
        List<KeyValue> GetReturnHadOut(string no);
        List<KeyValue> GetCanReturn();
        List<KeyValue> GetCanReturnType(int returntype);
        IQueryable<ReturnModel> ReturnModelList();
        List<V_ReturnOutModel> ReturnModelList(string where, string orderby);
        IQueryable<StockReturnDetailModel> StockReturnDetailList(string no);
        IQueryable<StockReturnDetailModel> StockReturnDetailListByOut(string outno);
        string GetStockReturnNo();
        string AddStockReturn(int staff, int dep, string remark, string deportStaff, int returntype);
        ReturnValue SaveStockReturnDetail(string no, List<StockReturnDetailModel> list, string remark, string deportStaff);
        bool DeleteStockReturn(string no);
        ReturnValue ChangeStockReturnStatus(string no, int status, string checkstaff);
        #endregion

        #region 销售退单

        List<KeyValue> GetCanReturn(int supplier);
        IQueryable<ReturnModel> ReturnSellModelList();
        List<V_ReturnOutModel> ReturnSellModelList(string where, string orderby);
        string AddStockReturn(int staff, int dep, int supplier, string remark, string deportStaff, int returntype);
        #endregion

        #region 报表
        IQueryable<ReportDepot> ReportDepot();
        IQueryable<ReportPurchaseRequire> ReportPurchaseRequire();
        IQueryable<ReportPurchase> ReportPurchase();
        IQueryable<ReportStock> ReportStockIn();
        IQueryable<ReportStock> ReportStockOut();
        IQueryable<ReportStock> ReportStockOtN();
        IQueryable<ReportPurchase> ReportOrderRetrun();
        IQueryable<ReportStock> ReportStockReturn();
        List<V_ReportDepot> ReportDepot(string where, string orderby);
        List<V_ReceiptDetail> ReportReceiptDetail(string where, string orderby);
        List<MaterialCostModel> ReportCostDetail(string key);
        #endregion
    }
}
