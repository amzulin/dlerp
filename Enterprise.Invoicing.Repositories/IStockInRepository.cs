using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IStockInRepository
    {
        #region 仓库相关
        IQueryable<Depot> QueryDepot(int valid);
        Depot GetOneDepot(int depotid);
        Material GetOneMaterial(string material);
        List<KeyValue> GetMaterialCategory(); 
        List<KeyValue> GetMaterialUnit();
        List<KeyValue> GetMaterialNoFix();
        #endregion

        #region 产品入库
        IQueryable<StockModel> StockInList();
        List<V_StockInModel> StockInList(string where, string orderby);
        string GetStockInNo();
        string AddStockIn(int staff, int dep,int supplier,int intype, string remark,string deport);
        ReturnValue SaveStockInDetail(string no, List<StockDetailModel> list, int supplier, string remark, string deport);

        bool DeleteStockIn(string no);

        IQueryable<StockDetailModel> StockInDetailList(string no);
        StockDetailModel StockInDetailOne(int sn);
        bool ChangeStockInStatus(string no, int status, string checkstaff);


        #endregion        

        #region 退单
        IQueryable<ReturnModel> ReturnModelList();
        List<V_ReturnInModel> ReturnModelList(string where, string orderby);
        string GetReturnNo();
        string AddReturn(int staff, int dep, int supplier, string remark, string deport);
        ReturnValue SaveReturnDetail(string no, List<ReturnDetailModel> list, int supplier, string remark, string deport);
        bool DeleteReturn(string no);
        IQueryable<ReturnDetailModel> ReturnDetailList(string no);
        IQueryable<ReturnDetailModel> ReturnDetailListByOrder(string order);
        ReturnDetailModel ReturnDetailOne(int sn);
        ReturnValue ChangeReturnStatus(string no, int status, string checkstaff);

        IQueryable<PurchaseDetailModel> CanReturnOrderList(int supplierid);
        #endregion        

        string GetStockSemiNo();
    }
}
