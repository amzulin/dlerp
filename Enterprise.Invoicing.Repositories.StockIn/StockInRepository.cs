using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public class StockInRepository : BasicRepository<StockIn>, IStockInRepository
    {
        public StockInRepository(): base(new InvoicingContext()){ }
        public StockInRepository(InvoicingContext context) : base(context) { }

        public IQueryable<Depot> QueryDepot(int valid)
        {
            if (valid == 0) return from m in context.Depots where m.valid == false orderby m.depotId ascending select m;
            else if (valid == 1) return from m in context.Depots where m.valid == true orderby m.depotId ascending select m;
            else return from m in context.Depots orderby m.depotId ascending select m;
        }

        public List<KeyValue> GetMaterialCategory()
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "MaterialCategory");
            if (data != null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                    string[] t = item.Split(':');
                    if (t.Length > 1 && t[1] != "")
                    {
                        string[] dt = t[1].Split(',');
                        foreach (var odt in dt)
                        {

                            r.Add(new KeyValue { text = odt, value = odt });
                        }
                    }
                }
            }
            return r;
        }
        public List<KeyValue> GetMaterialUnit()
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "MaterialUnit");
            if (data!=null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                    r.Add(new KeyValue { text = item, value = item });
                }
            }
            return r;
        }
        public List<KeyValue> GetMaterialNoFix()
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "MaterialNoFix");
            if (data!=null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                    r.Add(new KeyValue { text = item, value = item });
                }
            }
            return r;
        }


        public Depot GetOneDepot(int depotid)
        {
            return context.Depots.FirstOrDefault(p => p.depotId == depotid);
        }
        public Material GetOneMaterial(string material)
        {
            return context.Materials.FirstOrDefault(p => p.materialNo == material);
        }

        public IQueryable<StockModel> StockInList()
        {
            var list = from r in context.StockIns
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       join s in context.Suppliers on r.supplierId equals s.supplierId into g
                       from x in g.DefaultIfEmpty()
                       orderby r.createDate descending
                       select new StockModel
                       {
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           isover = r.isover,
                           valid = r.valid, deportStaff=r.deportStaff,
                            supplierName = x == null ? "" : x.supplierName, checkStaff=r.checkStaff,
                            supplierId = x == null ? 0 : x.supplierId, amount=r.inAmount, cost=r.inCost, datatype=r.inType,picker=e.staffName, remain=r.inRemain, stockNo=r.stockInNo
                       };
            return list;
        }
        public List<V_StockInModel> StockInList(string where, string orderby)
        {
            var list = context.V_StockInModel.SqlQuery("select * from V_StockInModel where stockInNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }

        public string GetStockInNo()
        {
            var last = (from r in context.StockIns orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "WE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.stockInNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "WE" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "WE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }

        public string AddStockIn(int staff, int dep, int supplier, int intype, string remark, string deport)
        {
            try
            {
                StockIn model = new StockIn();
                model.createDate = DateTime.Now;
                model.inType = intype;
                model.depId = dep;
                model.remark = remark;
                model.stockInNo = GetStockInNo();
                model.staffId = staff;
                model.status = 0;
                model.deportStaff = deport;
                if (supplier != 0) model.supplierId = supplier;
                context.StockIns.Add(model);
                context.SaveChanges();
                return model.stockInNo;
            }
            catch
            {
                return "";
            }
        }

        public ReturnValue SaveStockInDetail(string no, List<StockDetailModel> list, int supplier, string remark, string deport)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockIns.FirstOrDefault(p => p.stockInNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在入库单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在入库单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.StockInDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.inAmount = item.amount;
                                d.depotId = item.depotId;
                                d.remark = item.remark;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockInDetail d = new StockInDetail();
                            d.materialNo = item.materialNo;
                            d.inAmount = item.amount;                 
                            d.stockinNo = no;
                            if (item.purchaseDetailSn != 0)
                            {
                                d.purchaseDetailSn = item.purchaseDetailSn;
                                d.purchaseNo = item.purchaseNo;
                            }
                            d.remark = item.remark;
                            d.depotId = item.depotId;
                            c.StockInDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark||model.deportStaff!=deport)
                    {
                        model.remark = remark;
                        model.deportStaff = deport;
                        c.Entry(model).State = EntityState.Modified;
                    }
                    #endregion
                    c.SaveChanges(); return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                }

            }
        }

        public bool DeleteStockIn(string no)
        {
            using (var c=new InvoicingContext())
            {
                var model = c.StockIns.FirstOrDefault(p => p.stockInNo == no);
                if (model!=null)
                {
                    var details = c.StockInDetails.Where(p => p.stockinNo == no);
                    foreach (var item in details)
                    {
                        c.StockInDetails.Remove(item);
                    }
                    c.StockIns.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public IQueryable<StockDetailModel> StockInDetailList(string no)
        {
            var list = from i in context.StockIns
                       join d in context.StockInDetails on i.stockInNo equals d.stockinNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       where i.stockInNo == no
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.inAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = d.materialNo, materialTu=m.tunumber,
                           materialUnit = m.unit,
                           purchaseDetailSn = d.purchaseDetailSn,
                           purchaseNo = d.purchaseNo, remainAmout=d.returnAmount, 
                           stockNo = i.stockInNo, materialCategory=m.category, depotName=de.depotName, depotId=d.depotId, datatype=i.inType,
                           type = "",remark=d.remark
                       };
            return list;
        }

        public StockDetailModel StockInDetailOne(int sn)
        {
            var list = (from i in context.StockIns
                       join d in context.StockInDetails on i.stockInNo equals d.stockinNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join dep in context.Depots on d.depotId equals dep.depotId
                        where d.detailSn == sn
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.inAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = d.purchaseDetailSn,
                           remainAmout = d.returnAmount, 
                           purchaseNo = d.purchaseNo, depotId=d.depotId, depotName=dep.depotName,
                           stockNo = i.stockInNo,  
                           type = ""
                       }).FirstOrDefault();
            if (list.purchaseDetailSn != 0)
            {
                var order = context.PurchaseDetails.FirstOrDefault(p => p.detailSn == list.purchaseDetailSn);
                if (order!=null)
                {
                    list.orderAmout = order.poAmount;
                    
                }
            }
            return list;
        }

        public bool ChangeStockInStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockIns.FirstOrDefault(p => p.stockInNo == no);
                    var detail = c.StockInDetails.Where(p => p.stockinNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 审核通过 作废，吐出库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= (item.inAmount - item.returnAmount);
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                #endregion

                                #region 采购单入库，对应采购单未完工，加回未入库数量
                                if (item.purchaseDetailSn > 0)
                                {
                                    var purchasedetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                    if (purchasedetail!=null)
                                    {
                                        purchasedetail.poRemain += (item.inAmount-item.returnAmount);
                                    }
                                }
                                #endregion
                            }
                        }
                        model.checkStaff = checkstaff;
                        model.status = 4;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            foreach (var item in detail)
                            {
                                #region 审核通过 ，加入出库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += (item.inAmount - item.returnAmount);
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    DepotDetail newdepot = new DepotDetail();
                                    newdepot.depotAmount = (item.inAmount - item.returnAmount); 
                                    newdepot.depotId = item.depotId;
                                    newdepot.depotSafe = item.inAmount;
                                    newdepot.materialNo = item.materialNo;
                                    c.DepotDetails.Add(newdepot);
                                }
                                #endregion

                                #region 采购单入库，对应采购单加上入库数量
                                if (item.purchaseDetailSn > 0)
                                {
                                    var purchasedetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                    if (purchasedetail != null)
                                    {
                                        purchasedetail.poRemain -= (item.inAmount - item.returnAmount);
                                        if (purchasedetail.poRemain < 0) return false;
                                        if (purchasedetail.returnAmount>0)
                                        {
                                            purchasedetail.returnAmount -= (item.inAmount - item.returnAmount);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.checkStaff = checkstaff;
                            foreach (var item in detail)
                            {
                                #region 返审，吐出库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= (item.inAmount - item.returnAmount);
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                #endregion

                                #region 采购单入库，对应采购单未完工，加回未入库数量
                                if (item.purchaseDetailSn > 0)
                                {
                                    var purchasedetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                    if (purchasedetail != null)
                                    {
                                        purchasedetail.poRemain += (item.inAmount - item.returnAmount); 
                                    }
                                }
                                #endregion
                            }
                        }
                        else if (model.status == 4)
                        {
                            model.checkStaff = checkstaff; model.status = 0;
                        }
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    #region 更新采购单是否完工
                    SqlParameter[] sqlparams = new SqlParameter[1];
                    sqlparams[0] = new SqlParameter("@stockinno", no);
                    var result = (from p in c.Purchases.SqlQuery(" exec usp_over_purchase @stockinno", sqlparams) select p).ToList();
                    c.SaveChanges();
                    #endregion

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #region 退单
        public IQueryable<ReturnModel> ReturnModelList()
        {
            var list = from r in context.PurchaseReturns
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       join s in context.Suppliers on r.supplierId equals s.supplierId
                       orderby r.createDate descending
                       select new ReturnModel
                       {
                           amount = r.returnAmount,
                           cost = r.returnCost,
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           returnNo = r.returnNo,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           supplierId = r.supplierId,
                           supplierName = s.supplierName,
                           valid = r.valid, checkStaff=r.checkStaff, deportStaff=r.deportStaff
                       };
            return list;
        }

        public List<V_ReturnInModel> ReturnModelList(string where, string orderby)
        {
            var list = context.V_ReturnInModel.SqlQuery("select * from V_ReturnInModel where returnNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }

        public string GetReturnNo()
        {
            var last = (from r in context.PurchaseReturns orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "PR" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.returnNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "PR" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "PR" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddReturn(int staff, int dep, int supplier, string remark, string deport)
        {
            try
            {
                PurchaseReturn model = new PurchaseReturn();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.returnNo = GetReturnNo();
                model.staffId = staff;
                model.status = 0;
                model.supplierId = supplier; model.deportStaff = deport;
                context.PurchaseReturns.Add(model);
                context.SaveChanges();
                return model.returnNo;
            }
            catch
            {
                return "";
            }
        }
        public ReturnValue SaveReturnDetail(string no, List<ReturnDetailModel> list, int supplier, string remark, string deport)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.PurchaseReturns.FirstOrDefault(p => p.returnNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在采购退单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在采购退单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.PurchaseReturnDetails.FirstOrDefault(p => p.detailSn == item.returnSn);
                            if (d != null) c.PurchaseReturnDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.PurchaseReturnDetails.FirstOrDefault(p => p.detailSn == item.returnSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.returnAmount = item.returnAmount;
                                d.remark = item.remark;
                                d.depotId = item.depotId;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            PurchaseReturnDetail d = new PurchaseReturnDetail();
                            var order = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                            //var stockin = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.stockinDetailSn);
                            //stockin.returnAmount += item.returnAmount;
                            d.materialNo = item.materialNo;
                            d.returnAmount = item.returnAmount;
                            d.returnNo = no;
                            d.depotId = item.depotId;
                            d.remark = item.remark;
                            d.buyPrice = order.poPrice;
                            d.purchaseDetailSn = item.purchaseDetailSn;
                            d.purchaseNo = item.purchaseNo;
                            d.stockinDetailSn = item.stockinDetailSn;
                            d.stockinNo = item.stockinNo;
                            c.PurchaseReturnDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark||model.deportStaff!=deport)
                    {
                        model.remark = remark;
                        model.deportStaff = deport;
                        c.Entry(model).State = EntityState.Modified;
                    }
                    #endregion
                    c.SaveChanges(); return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                }

            }
        }
        public bool DeleteReturn(string no)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.PurchaseReturns.FirstOrDefault(p => p.returnNo == no);
                if (model != null)
                {
                    var details = c.PurchaseReturnDetails.Where(p => p.returnNo == no);
                    foreach (var item in details)
                    {
                        c.PurchaseReturnDetails.Remove(item);
                    }
                    c.PurchaseReturns.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public IQueryable<ReturnDetailModel> ReturnDetailList(string no)
        {
            var list = from i in context.PurchaseReturns
                       join d in context.PurchaseReturnDetails on i.returnNo equals d.returnNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       join od in context.PurchaseDetails on d.purchaseDetailSn equals od.detailSn
                       join io in context.StockInDetails on d.stockinDetailSn equals io.detailSn
                       where i.returnNo == no
                       orderby d.detailSn descending
                       select new ReturnDetailModel
                       {
                           returnAmount = d.returnAmount,
                           returnSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName, materialTu=m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = d.purchaseDetailSn,
                           purchaseNo = d.purchaseNo,
                           returnNo = i.returnNo,
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = d.depotId,
                           orderAmoutn = od.poAmount,
                           inAmount = io.inAmount,
                           orderPrice = od.poPrice,
                           stockinDetailSn = d.stockinDetailSn,
                           stockinNo = io.stockinNo,
                           type = "", remark=d.remark
                       };
            return list;
        }
        public IQueryable<ReturnDetailModel> ReturnDetailListByOrder(string order)
        {
            var list =                        from d in context.PurchaseDetails 
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join io in context.StockInDetails on d.detailSn equals io.purchaseDetailSn
                       join de in context.Depots on io.depotId equals de.depotId
                       where d.purchaseNo==order
                       orderby d.detailSn descending
                       select new ReturnDetailModel
                       {
                           returnAmount = d.returnAmount,
                           returnSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = d.detailSn,
                           purchaseNo = d.purchaseNo,
                           returnNo = "",
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = de.depotId,
                           orderAmoutn =d.poAmount,
                           inAmount = io.inAmount,
                           orderPrice = d.poPrice,
                           stockinDetailSn =io.detailSn,
                           stockinNo = io.stockinNo,
                           type = "", remark=""
                       };
            return list;
        }
        public ReturnDetailModel ReturnDetailOne(int sn)
        {
            var list = (from i in context.PurchaseReturns
                       join d in context.PurchaseReturnDetails on i.returnNo equals d.returnNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       join od in context.PurchaseDetails on d.purchaseDetailSn equals od.detailSn
                       join io in context.StockInDetails on d.stockinDetailSn equals io.detailSn
                       where d.detailSn==sn
                       orderby d.detailSn descending
                       select new ReturnDetailModel
                       {
                           returnAmount = d.returnAmount,
                           returnSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = d.purchaseDetailSn,
                           purchaseNo = d.purchaseNo,
                           returnNo = i.returnNo,
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = d.depotId,
                           orderAmoutn = od.poAmount,
                           orderPrice = od.poPrice,
                           stockinDetailSn = d.stockinDetailSn,
                           stockinNo = od.purchaseNo,
                           type = "", remark=d.remark
                       }).FirstOrDefault();
            return list;
        }
        public ReturnValue ChangeReturnStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.PurchaseReturns.FirstOrDefault(p => p.returnNo == no);
                    var detail = c.PurchaseReturnDetails.Where(p => p.returnNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 从审核通过到作废，采购明细数量回去，入库明细数量回去，库存回去
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.returnAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                var stock = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.stockinDetailSn);
                                if (stock!=null)
                                {
                                    stock.returnAmount -= item.returnAmount;
                                    c.Entry(stock).State = EntityState.Modified;
                                }
                                var orderdetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                if (orderdetail!=null)
                                {
                                    orderdetail.poRemain -= item.returnAmount;
                                    orderdetail.returnAmount -= item.returnAmount;
                                }
                                #endregion
                            }
                        }
                        model.status = 4;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            foreach (var item in detail)
                            {
                                #region 审核通过
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= item.returnAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                var stock = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.stockinDetailSn);
                                if (stock != null)
                                {
                                    stock.returnAmount += item.returnAmount;
                                    c.Entry(stock).State = EntityState.Modified;
                                }
                                var orderdetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                if (orderdetail != null)
                                {
                                    orderdetail.poRemain += item.returnAmount;
                                    orderdetail.returnAmount += item.returnAmount;
                                }
                                #endregion
                            }
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.checkStaff = checkstaff;
                            foreach (var item in detail)
                            {
                                #region 从审核通过到返审，采购明细数量回去，入库明细数量回去，库存回去
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.returnAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                var stock = c.StockInDetails.FirstOrDefault(p => p.detailSn == item.stockinDetailSn);
                                if (stock != null)
                                {
                                    stock.returnAmount -= item.returnAmount;
                                    c.Entry(stock).State = EntityState.Modified;
                                }
                                var orderdetail = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.purchaseDetailSn);
                                if (orderdetail != null)
                                {
                                    orderdetail.poRemain -= item.returnAmount;
                                    orderdetail.returnAmount -= item.returnAmount;
                                }
                                #endregion
                            }
                        }
                        else if (model.status == 4)
                        {
                            model.status = 0;
                            model.checkStaff = checkstaff;
                        }
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    #region 更新采购单是否完工
                    List<string> hadno = new List<string>();
                    foreach (var item in detail)
                    {
                        if (hadno.Contains(item.stockinNo)) continue;
                        hadno.Add(item.stockinNo);
                        SqlParameter[] sqlparams = new SqlParameter[1];
                        sqlparams[0] = new SqlParameter("@stockinno", item.stockinNo);
                        var result = (from p in c.Purchases.SqlQuery(" exec usp_over_purchase @stockinno", sqlparams) select p).ToList();
                        c.SaveChanges();
                    }
                    #endregion

                    return new ReturnValue { status = true };
                }
                catch(Exception ex)
                {
                    return new ReturnValue { message="操作失败:"+ex.Message, status=false };
                }
            }
        }
        public IQueryable<PurchaseDetailModel> CanReturnOrderList(int supplierid)
        {
            var list1 = from p in context.Purchases
                        join de in context.PurchaseDetails on p.purchaseNo equals de.purchaseNo
                        join ind in context.StockInDetails on de.detailSn equals ind.purchaseDetailSn
                        join sin in context.StockIns on ind.stockinNo equals sin.stockInNo
                        join m in context.Materials on de.materialNo equals m.materialNo
                        where p.supplierId == supplierid && sin.status == 1
                        orderby p.createDate descending
                        select new PurchaseDetailModel
                        {
                            detailSn = de.detailSn,
                            materialModel = m.materialModel,
                            poRemain = de.poRemain,
                            poAmount = de.poAmount,
                            poPrice = de.poPrice,
                            materialName = m.materialName,
                            materialNo = de.materialNo,
                            materialTu = m.tunumber,
                            materialUnit = m.unit,
                            purchaseNo = de.purchaseNo,
                            remark = de.remark,
                            returnAmount = de.returnAmount,
                            requireNo = de.requireNo,
                            requireDetailSn = de.requireDetailSn,
                            type = ""
                        };
            return list1;
        }
        #endregion   

        #region 半成品

        public string GetStockSemiNo()
        {
            var last = (from r in context.ProductSemis orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "SM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.semiNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "SM" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "SM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        #endregion

    }
}
