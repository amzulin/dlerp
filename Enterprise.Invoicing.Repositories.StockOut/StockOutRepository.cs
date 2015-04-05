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
    public class StockOutRepository : BasicRepository<StockOut>, IStockOutRepository
    {
        public StockOutRepository(): base(new InvoicingContext()){ }
        public StockOutRepository(InvoicingContext context) : base(context) { }


        #region 出库

        public IQueryable<StockModel> StockOutList()
        {
            var list = from r in context.StockOuts
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       join s in context.Suppliers on r.supplierId equals s.supplierId into g
                       from x in g.DefaultIfEmpty()
                       orderby r.createDate descending
                       select new StockModel
                       {
                           stockNo = r.stockoutNo,
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           isover = r.isover,
                           valid = r.valid,
                           supplierName = x == null ? "" : x.supplierName,
                           supplierId = x == null ? 0 : x.supplierId,
                           amount = r.outAmount,
                           cost = r.outCost,
                           datatype = r.outType, bomdetailsn=r.bomDetailSn, bomOrderNo=r.bomOrderNo,
                           picker = e.staffName, checkStaff=r.checkStaff, deportStaff=r.deportStaff, expresscode=r.expresscode,express=r.express, outDate=r.outDate
                       };
            return list;
        }

        public List<V_StockOutModel> StockOutList(string where, string orderby)
        {
            var list = context.V_StockOutModel.SqlQuery("select * from V_StockOutModel where stockOutNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public string GetStockOutNo()
        {
            var last = (from r in context.StockOuts orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "WO" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.stockoutNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "WO" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "WO" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }

        public string AddStockOut(int staff, int dep, int supplier, int outtype, string remark, string deportStaff)
        {
            try
            {
                StockOut model = new StockOut();
                model.createDate = DateTime.Now;
                model.outType = outtype;
                model.depId = dep;
                model.remark = remark;
                model.stockoutNo = GetStockOutNo();
                model.staffId = staff;
                model.status = 0;
                model.deportStaff = deportStaff;
                if (supplier != 0) model.supplierId = supplier;
                context.StockOuts.Add(model);
                context.SaveChanges();
                return model.stockoutNo;
            }
            catch
            {
                return "";
            }
        }

        public ReturnValue SaveStockOutDetail(string no, List<StockDetailModel> list, int supplier, string remark,string deportStaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockOuts.FirstOrDefault(p => p.stockoutNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在出库单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在出库单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.StockOutDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.remark = item.remark;
                                d.outAmount = item.amount;
                                d.depotId = item.depotId;
                                d.outPrice = item.price;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockOutDetail d = new StockOutDetail();
                            d.materialNo = item.materialNo;
                            d.outAmount = item.amount;
                            d.remark = item.remark;
                            d.stockoutNo = no; d.outPrice = item.price;
                            d.depotId = item.depotId;
                            c.StockOutDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark||model.deportStaff!=deportStaff)
                    {
                        model.remark = remark;
                        model.deportStaff = deportStaff;
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

        public bool DeleteStockOut(string no)
        {
            using (var c=new InvoicingContext())
            {
                var model = c.StockOuts.FirstOrDefault(p => p.stockoutNo == no);
                if (model!=null)
                {
                    var details = c.StockOutDetails.Where(p => p.stockoutNo == no);
                    foreach (var item in details)
                    {
                        c.StockOutDetails.Remove(item);
                    }
                    c.StockOuts.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public IQueryable<StockDetailModel> StockOutDetailList(string no)
        {
            var list = from i in context.StockOuts
                       join d in context.StockOutDetails on i.stockoutNo equals d.stockoutNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       where i.stockoutNo == no
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.outAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit, price=d.outPrice, returnamount=d.returnAmount,
                           stockNo = i.stockoutNo, materialCategory=m.category, depotName=de.depotName, depotId=d.depotId, datatype=i.outType,
                           type = "", remark=d.remark, bomId=d.bomId
                       };
            return list;
        }

        public StockDetailModel StockOutDetailOne(int sn)
        {
            var list = (from i in context.StockOuts
                        join d in context.StockOutDetails on i.stockoutNo equals d.stockoutNo
                        join m in context.Materials on d.materialNo equals m.materialNo
                        join de in context.Depots on d.depotId equals de.depotId
                        where d.detailSn == sn
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.outAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           price = d.outPrice,
                           stockNo = i.stockoutNo,
                           materialCategory = m.category,
                           returnamount = d.returnAmount,
                           depotName = de.depotName,
                           depotId = d.depotId,
                           datatype = i.outType,
                           type = "",remark=d.remark, bomId=d.bomId
                       }).FirstOrDefault();
            return list;
        }

        public ReturnValue ChangeStockOutStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockOuts.FirstOrDefault(p => p.stockoutNo == no);
                    var detail = c.StockOutDetails.Where(p => p.stockoutNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 审核通过 作废，加回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,作废失败", status = false };
                                }
                                #region 销售订单返回
                                if (model.bomOrderNo != null && model.bomOrderNo != "")
                                {
                                    var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == item.orderSn);
                                    if (bomdetail != null)
                                    {
                                        bomdetail.sellAmount -= item.outAmount;
                                        c.Entry(bomdetail).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                    }
                                }
                                #endregion
                                #endregion
                            }
                            #region bom订单出库数量回流
                            if (model.bomDetailSn.HasValue && model.bomDetailSn.Value > 0)
                            {
                                var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == model.bomDetailSn.Value);
                                if (bomdetail != null)
                                {
                                    bomdetail.outAmount -= model.outAmount;
                                    c.Entry(bomdetail).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                }
                            }
                            #endregion

                        }
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            foreach (var item in detail)
                            {
                                #region 审核通过 ，出库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null && depot.depotAmount >= item.outAmount)
                                {
                                    depot.depotAmount -= item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,审核失败", status = false };
                                }
                                #endregion
                                #region 销售订单增加卖出
                                if (model.bomOrderNo != null && model.bomOrderNo != "")
                                {
                                    var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == item.orderSn);
                                    if (bomdetail != null)
                                    {
                                        if (bomdetail.Amount - bomdetail.sellAmount < item.outAmount)
                                        {
                                            return new ReturnValue { message = model.stockoutNo + "客户订单明细的剩余数量小于领料数量,审核失败", status = false };
                                        }
                                        else
                                        {
                                            bomdetail.sellAmount += item.outAmount;
                                            c.Entry(bomdetail).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                    }
                                }
                                #endregion
                                #region bom订单数量出库数量增加
                                if (model.bomDetailSn.HasValue && model.bomDetailSn.Value > 0)
                                {
                                    var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == model.bomDetailSn.Value);
                                    if (bomdetail != null)
                                    {
                                        if (bomdetail.Amount - bomdetail.outAmount < model.outAmount)
                                        {
                                            return new ReturnValue { message = model.stockoutNo + "客户订单明细的剩余数量小于领料数量,审核失败", status = false };
                                        }
                                        else
                                        {
                                            bomdetail.outAmount += model.outAmount;
                                            c.Entry(bomdetail).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                    }
                                }
                                #endregion

                            }
                            model.status = 1;
                            model.isover = 1;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 返审，加回库存
                                if (item.returnAmount > 0)
                                {
                                    return new ReturnValue { message = item.materialNo + "已产生退单,该出库单不能反审", status = false };
                                }
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,反审失败", status = false };
                                }
                                #endregion
                                #region 销售订单增加卖出反回
                                if (model.bomOrderNo != null && model.bomOrderNo != "")
                                {
                                    var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == item.orderSn);
                                    if (bomdetail != null)
                                    {
                                        bomdetail.sellAmount -= item.outAmount;
                                        c.Entry(bomdetail).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                    }
                                }
                                #endregion
                                #region bom订单数量回流
                                if (model.bomDetailSn.HasValue && model.bomDetailSn.Value > 0)
                                {
                                    var bomdetail = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == model.bomDetailSn.Value);
                                    if (bomdetail != null)
                                    {
                                        bomdetail.outAmount -= model.outAmount;
                                        c.Entry(bomdetail).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        return new ReturnValue { message = model.stockoutNo + "不存在客户订单明细,作废失败", status = false };
                                    }
                                }
                                #endregion

                            }
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 4)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;
                        }
                    }
                    #region 销售订单是否完工
                    if (model.bomOrderNo != null && model.bomOrderNo != "")
                    {
                        var order = c.BomOrders.FirstOrDefault(p => p.bomOrderNo == model.bomOrderNo);
                        var odetails = c.BomOrderDetails.Where(p => p.bomOrderNo == model.bomOrderNo);
                        double xge = 0.0;
                        foreach (var item in odetails)
                        {
                            xge += (item.Amount - item.sellAmount);
                        }
                        order.isover = xge == 0.0 ? 1 : 0;
                        c.Entry(order).State = EntityState.Modified;
                    }
                    #endregion
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
                }
            }
        }

        #endregion

        #region 仓库
        public IQueryable<Depot> GetDepots(int? depotid)
        {
            if (depotid.HasValue) return context.Depots.Where(p => p.depotId == depotid.Value);
            else return context.Depots.OrderBy(x=>x.depotId);
        }
        public IQueryable<DepotDetailModel> GetDepotDetail(int depotid)
        {
            var list = from d in context.Depots
                       join dd in context.DepotDetails on d.depotId equals dd.depotId
                       join m in context.Materials on dd.materialNo equals m.materialNo
                       where d.depotId == depotid
                       orderby m.materialNo ascending
                       select new DepotDetailModel
                       {
                           amout = dd.depotAmount,
                           cost = dd.depotCost,
                           depotId = d.depotId,
                           depotName = d.depotName,
                           detailSn = dd.detailSn,
                           materialCategory = m.category,
                           materialModel = m.materialModel, materialTu=m.tunumber,
                           materialName = m.materialName,
                           materialUnit = m.unit,
                           remark = d.remark,
                           safe = dd.depotSafe,
                           valid = d.valid
                       };
            return list;
        }
        public IQueryable<Material> GetDepotMaterial(int depotid, int valid)
        {
            if (valid == 1)
            {
                var list = from d in context.Depots
                           join dd in context.DepotDetails on d.depotId equals dd.depotId
                           join m in context.Materials on dd.materialNo equals m.materialNo
                           where d.depotId == depotid && m.valid == true
                           orderby m.materialNo ascending
                           select m;
                return list;
            } 
            else if (valid == 0)
            {
                var list = from d in context.Depots
                           join dd in context.DepotDetails on d.depotId equals dd.depotId
                           join m in context.Materials on dd.materialNo equals m.materialNo
                           where d.depotId == depotid && m.valid==false
                           orderby m.materialNo ascending
                           select m;
                return list;
            }
            else
            {
                var list = from d in context.Depots
                           join dd in context.DepotDetails on d.depotId equals dd.depotId
                           join m in context.Materials on dd.materialNo equals m.materialNo
                           where d.depotId == depotid
                           orderby m.materialNo ascending
                           select m;
                return list;
            }
        }
        #endregion

        #region 以旧换新
        public IQueryable<StockModel> ChangeOtNList()
        {
            var list = from r in context.StockExchanges
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       orderby r.createDate descending
                       select new StockModel
                       {
                           stockNo = r.changeNo,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           isover = r.isover, createDate=r.createDate, picker=r.picker, pickerDep=r.pickdep,
                           valid = r.valid, checkStaff=r.checkStaff, deportStaff=r.deportStaff
                       };
            return list;
        }
        public List<V_StockChangeModel> ChangeOtNList(string where, string orderby)
        {
            var list = context.V_StockChangeModel.SqlQuery("select * from V_StockChangeModel where changeNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public IQueryable<StockDetailModel> ChangeOtNInList(string changeno)
        {
            var list = from i in context.StockExchanges
                       join d in context.StockInDetails on i.changeNo equals d.changeNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       where i.changeNo == changeno
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.inAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName, materialTu=m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           changeNo = i.changeNo,
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = d.depotId, changeType=0,
                           type = "",remark=d.remark
                       };
            return list;
        }
        public IQueryable<StockDetailModel> ChangeOtNOutList(string changeno)
        {
            var list = from i in context.StockExchanges
                       join d in context.StockOutDetails on i.changeNo equals d.changeNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       join de in context.Depots on d.depotId equals de.depotId
                       where i.changeNo == changeno
                       orderby d.detailSn descending
                       select new StockDetailModel
                       {
                           amount = d.outAmount,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           changeNo = i.changeNo,
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = d.depotId,
                           changeType = 1,
                           type = "",remark=d.remark
                       };
            return list;
        }
        public string GetChangeOtNNo()
        {
            var last = (from r in context.StockExchanges orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "ON" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.changeNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "ON" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "ON" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddChangeOtN(int staff, int dep, string remark,string deportStaff)
        {
            try
            {
                StockExchange model = new StockExchange();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.changeNo = GetChangeOtNNo();
                model.staffId = staff;
                model.status = 0;
                model.deportStaff = deportStaff;
                context.StockExchanges.Add(model);
                context.SaveChanges();
                return model.changeNo;
            }
            catch
            {
                return "";
            }
        }
        public ReturnValue SaveChangeOtNDetail(string no, List<StockDetailModel> inlist, List<StockDetailModel> outlist, string remark,string deportStaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockExchanges.FirstOrDefault(p => p.changeNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在以旧换新" };

                    #region 旧物料入库
                    if (inlist == null || inlist.Count < 1) return new ReturnValue { status = false, message = "以旧换新不存在旧物料入库单明细" };
                    foreach (var item in inlist)
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
                                d.depotId = item.depotId; d.remark = item.remark;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockInDetail d = new StockInDetail();
                            d.changeNo = no; 
                            d.depotId = item.depotId;
                            d.materialNo = item.materialNo;
                            d.inAmount = item.amount; d.remark = item.remark;
                            c.StockInDetails.Add(d);
                        }
                    }
                    #endregion

                    #region 旧物料出库

                    if (outlist == null || outlist.Count < 1) return new ReturnValue { status = false, message = "以旧换新不存在旧物料出库单明细" };
                    foreach (var item in outlist)
                    {
                        if (item.type == "") continue;
                        if (item.type == "delete")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.StockOutDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.outAmount = item.amount; d.remark = item.remark;
                                d.depotId = item.depotId;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockOutDetail d = new StockOutDetail();
                            d.materialNo = item.materialNo;
                            d.outAmount = item.amount;
                            d.changeNo = no;
                            d.depotId = item.depotId; d.remark = item.remark;
                            c.StockOutDetails.Add(d);
                        }
                    }
                    #endregion

                    #region 以旧换新
                    if (model.remark != remark||model.deportStaff!=deportStaff)
                    {
                        model.remark = remark;
                        model.deportStaff = deportStaff;
                        c.Entry(model).State = EntityState.Modified;
                    }
                    #endregion
                    c.SaveChanges(); 
                    return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                }

            }
        }
        public bool DeleteChangeOtN(string changeno)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.StockExchanges.FirstOrDefault(p => p.changeNo == changeno);
                if (model.status == 1) return false;
                if (model != null)
                {
                    var outdetails = c.StockOutDetails.Where(p => p.changeNo == changeno);
                    foreach (var item in outdetails)
                    {
                        c.StockOutDetails.Remove(item);
                    }
                    var indetails = c.StockInDetails.Where(p => p.changeNo == changeno);
                    foreach (var item in indetails)
                    {
                        c.StockInDetails.Remove(item);
                    }
                    c.StockExchanges.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public ReturnValue ChangeChangeOtNStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockExchanges.FirstOrDefault(p => p.changeNo == no);
                    var indetail = c.StockInDetails.Where(p => p.changeNo == no);
                    var outdetail = c.StockOutDetails.Where(p => p.changeNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1)
                        {
                            foreach (var item in outdetail)
                            {
                                #region 审核通过再作废，出库单加回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,出库单作废失败", status = false };
                                }
                                #endregion
                            } 
                            foreach (var item in indetail)
                            {
                                #region 审核通过再作废，入库单减回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= item.inAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,入库单作废失败", status = false };
                                }
                                #endregion
                            }
                        }
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            foreach (var item in outdetail)
                            {
                                #region 审核通过，出库单出库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,出库单出库失败", status = false };
                                }
                                #endregion
                            }
                            foreach (var item in indetail)
                            {
                                #region 审核通过，入库单回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.inAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    DepotDetail newdepot = new DepotDetail();
                                    newdepot.depotAmount = item.inAmount;
                                    newdepot.depotId = item.depotId;
                                    newdepot.depotSafe = item.inAmount;
                                    newdepot.materialNo = item.materialNo;
                                    c.DepotDetails.Add(newdepot);
                                }
                                #endregion
                            }
                            model.status = 1;
                            model.isover = 1;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 1)
                        {
                            foreach (var item in outdetail)
                            {
                                #region 返审，出库单加回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount += item.outAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,出库单返审失败", status = false };
                                }
                                #endregion
                            }
                            foreach (var item in indetail)
                            {
                                #region 返审，入库单减回库存
                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == item.depotId && p.materialNo == item.materialNo);
                                if (depot != null)
                                {
                                    depot.depotAmount -= item.inAmount;
                                    c.Entry(depot).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在库存记录,入库单返审失败", status = false };
                                }
                                #endregion
                            }
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 4)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;
                        }
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
                }
            }
        }
        #endregion

        #region 出库单退单
        /// <summary>
        /// 能退单的领料单
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public List<KeyValue> GetReturnHadOut(string no)
        {
            var list = from p in context.StockReturns
                       join pd in context.StockReturnDetails on p.returnNo equals pd.returnNo
                       where p.returnNo == no
                       orderby pd.detailSn ascending
                       select new KeyValue { text = pd.stockoutNo, value = p.returnNo };
            return list.Distinct().ToList();
        }

        
        /// <summary>
        /// 能退单的出库单
        /// returntype 出库类别：2 领料，4 直接销售
        /// </summary>
        /// <param name="returntype"></param>
        /// <returns></returns>
        public List<KeyValue> GetCanReturnType(int returntype)
        {
            var list = from p in context.StockOuts
                       join pd in context.StockOutDetails on p.stockoutNo equals pd.stockoutNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.status == 1 && p.outType == returntype
                       orderby p.createDate descending
                       select new KeyValue { text = p.stockoutNo, value = "", column1 = m.materialNo, column2 = m.materialName, column3 = m.materialModel, column4 = m.tunumber };
            return list.Distinct().ToList();
        }
        public List<KeyValue> GetCanReturn()
        {
            var list = from p in context.StockOuts
                       join pd in context.StockOutDetails on p.stockoutNo equals pd.stockoutNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.status == 1 && p.outType==2
                       orderby p.createDate descending
                       select new KeyValue { text = p.stockoutNo, value = "", column1 = m.materialNo, column2 = m.materialName, column3 = m.materialModel,column4=m.tunumber };
            return list.Distinct().ToList();
        }
        public IQueryable<ReturnModel> ReturnModelList()
        {
            var list = from r in context.StockReturns
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       where r.supplierId.HasValue==false
                       orderby r.createDate descending
                       select new ReturnModel
                       {
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           returnNo = r.returnNo,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           valid = r.valid, checkStaff=r.checkStaff, deportStaff=r.deportStaff
                       };
            return list;
        }
        public List<V_ReturnOutModel> ReturnModelList(string where, string orderby)
        {
            var list = context.V_ReturnOutModel.SqlQuery("select * from V_ReturnOutModel where supplierId is null and returnType=2 and returnNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public IQueryable<StockReturnDetailModel> StockReturnDetailList(string no)
        {
            var list1 = from p in context.StockReturns
                        join pd in context.StockReturnDetails on p.returnNo equals pd.returnNo
                        join m in context.Materials on pd.materialNo equals m.materialNo
                        join rde in context.StockOutDetails on pd.stockoutDetailSn equals rde.detailSn
                        join fromdep in context.Depots on pd.fromDepotId equals fromdep.depotId
                        join todep in context.Depots on pd.toDepotId equals todep.depotId
                        where p.returnNo == no
                        orderby p.createDate descending
                        select new StockReturnDetailModel
                        {
                            detailSn = pd.detailSn,
                            materialModel = m.materialModel,
                            materialName = m.materialName,
                            materialNo = pd.materialNo,
                            materialTu = m.tunumber,
                            materialUnit = m.unit,
                            fromdepotId = fromdep.depotId,
                            fromdepotName = fromdep.depotName,
                            materialCategory = m.category,
                            outAmoutn = rde.outAmount,
                            returnAmount = pd.returnAmount,
                            returnNo = p.returnNo,
                            stockoutDetailSn = rde.detailSn,
                            stockoutNo = rde.stockoutNo,
                            hadreturnAmount = rde.returnAmount,
                            todepotId = todep.depotId,
                            todepotName = todep.depotName,
                            type = "",remark=pd.remark
                        };
            return list1;
        }
        public IQueryable<StockReturnDetailModel> StockReturnDetailListByOut(string outno)
        {
            var list1 = from p in context.StockOuts
                        join pd in context.StockOutDetails on p.stockoutNo equals pd.stockoutNo
                        join m in context.Materials on pd.materialNo equals m.materialNo
                        join d in context.Depots on pd.depotId equals d.depotId
                        where p.stockoutNo == outno
                        orderby p.createDate descending
                        select new StockReturnDetailModel
                        {
                            detailSn = 0,
                            materialModel = m.materialModel,
                            materialName = m.materialName,
                            materialTu = m.tunumber,
                            materialNo = pd.materialNo,
                            materialUnit = m.unit,
                            fromdepotId = d.depotId,
                            fromdepotName = d.depotName,
                            materialCategory = m.category,
                            outAmoutn = pd.outAmount,
                            returnAmount = 0,
                            returnNo = "",
                            stockoutDetailSn = pd.detailSn,
                            stockoutNo = p.stockoutNo,
                             todepotId=0, todepotName="", hadreturnAmount=pd.returnAmount,
                            type = "",
                            remark = pd.remark
                        };
            return list1;
        }
        public string GetStockReturnNo()
        {
            var last = (from r in context.StockReturns orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "SR" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.returnNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "SR" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "SR" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddStockReturn(int staff, int dep, string remark, string deportStaff, int returntype)
        {
            try
            {
                StockReturn model = new StockReturn();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.returnNo = GetStockReturnNo();
                model.staffId = staff;
                model.deportStaff = deportStaff;
                model.status = 0; model.returnType = returntype;
                context.StockReturns.Add(model);
                context.SaveChanges();
                return model.returnNo;
            }
            catch
            {
                return "";
            }
        }
        public ReturnValue SaveStockReturnDetail(string no, List<StockReturnDetailModel> list, string remark, string deportStaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockReturns.FirstOrDefault(p => p.returnNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在出库单退单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在出库单退单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.StockReturnDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.StockReturnDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.StockReturnDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.returnAmount = item.returnAmount;
                                d.toDepotId = item.todepotId; d.remark = item.remark;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockReturnDetail d = new StockReturnDetail();
                            d.materialNo = item.materialNo;
                            d.returnAmount = item.returnAmount;
                            d.returnNo = item.returnNo;
                            d.stockoutDetailSn = item.stockoutDetailSn;
                            d.stockoutNo = item.stockoutNo;
                            d.toDepotId = item.todepotId; d.remark = item.remark;
                            d.fromDepotId = item.fromdepotId;
                            c.StockReturnDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark||model.deportStaff!=deportStaff)
                    {
                        model.remark = remark; model.deportStaff = deportStaff;
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
        public bool DeleteStockReturn(string no)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.StockReturns.FirstOrDefault(p => p.returnNo == no);
                if (model != null)
                {
                    var details = c.StockReturnDetails.Where(p => p.returnNo == no);
                    foreach (var item in details)
                    {
                        c.StockReturnDetails.Remove(item);
                    }
                    c.StockReturns.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public ReturnValue ChangeStockReturnStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockReturns.FirstOrDefault(p => p.returnNo == no);
                    var detail = c.StockReturnDetails.Where(p => p.returnNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 审核通过 作废，加回库存
                                //var depotf = c.DepotDetails.FirstOrDefault(p => p.depotId == item.fromDepotId && p.materialNo == item.materialNo);
                                var depott = c.DepotDetails.FirstOrDefault(p => p.depotId == item.toDepotId && p.materialNo == item.materialNo);
                                //if (depotf != null)
                                //{
                                //    depotf.depotAmount += item.returnAmount;
                                //    c.Entry(depotf).State = EntityState.Modified;
                                //}
                                //else
                                //{
                                //    return new ReturnValue { message = item.materialNo + "不存在出库库存记录,作废失败", status = false };
                                //} 
                                if (depott != null)
                                {
                                    depott.depotAmount -= item.returnAmount;
                                    c.Entry(depott).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在入库库存记录,作废失败", status = false };
                                }
                                var from = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.stockoutDetailSn);
                                if (from!=null)
                                {
                                    from.returnAmount -= item.returnAmount;
                                    c.Entry(from).State = EntityState.Modified;
                                }
                                else return new ReturnValue { message = item.materialNo + "不存在出库记录,作废失败", status = false };

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
                            foreach (var item in detail)
                            {
                                #region 审核通过 ，出库存
                                //var depotf = c.DepotDetails.FirstOrDefault(p => p.depotId == item.fromDepotId && p.materialNo == item.materialNo);
                                var depott = c.DepotDetails.FirstOrDefault(p => p.depotId == item.toDepotId && p.materialNo == item.materialNo);
                                //if (depotf != null)
                                //{
                                //    depotf.depotAmount -= item.returnAmount;
                                //    c.Entry(depotf).State = EntityState.Modified;
                                //}
                                //else
                                //{
                                //    return new ReturnValue { message = item.materialNo + "不存在出库库存记录, 审核失败", status = false };
                                //}
                                if (depott != null)
                                {
                                    depott.depotAmount += item.returnAmount;
                                    c.Entry(depott).State = EntityState.Modified;
                                }
                                else
                                {
                                    DepotDetail newdepot = new DepotDetail();
                                    newdepot.depotAmount = item.returnAmount;
                                    newdepot.depotId = item.toDepotId;
                                    newdepot.depotSafe = item.returnAmount;
                                    newdepot.materialNo = item.materialNo;
                                    c.DepotDetails.Add(newdepot);
                                }
                                var from = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.stockoutDetailSn);
                                if (from != null)
                                {
                                    from.returnAmount += item.returnAmount;
                                    c.Entry(from).State = EntityState.Modified;
                                }
                                else return new ReturnValue { message = item.materialNo + "不存在出库记录, 审核失败", status = false };
                                
                                #endregion
                            }
                            model.checkStaff = checkstaff;
                            model.status = 1;
                        }
                        else if (model.status == 1)
                        {
                            foreach (var item in detail)
                            {
                                #region 返审，加回库存
                                //var depotf = c.DepotDetails.FirstOrDefault(p => p.depotId == item.fromDepotId && p.materialNo == item.materialNo);
                                var depott = c.DepotDetails.FirstOrDefault(p => p.depotId == item.toDepotId && p.materialNo == item.materialNo);
                                //if (depotf != null)
                                //{
                                //    depotf.depotAmount += item.returnAmount;
                                //    c.Entry(depotf).State = EntityState.Modified;
                                //}
                                //else
                                //{
                                //    return new ReturnValue { message = item.materialNo + "不存在出库库存记录,返审失败", status = false };
                                //}
                                if (depott != null)
                                {
                                    depott.depotAmount -= item.returnAmount;
                                    c.Entry(depott).State = EntityState.Modified;
                                }
                                else
                                {
                                    return new ReturnValue { message = item.materialNo + "不存在入库库存记录,返审失败", status = false };
                                } 
                                
                                var from = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.stockoutDetailSn);
                                if (from != null)
                                {
                                    from.returnAmount -= item.returnAmount;
                                    c.Entry(from).State = EntityState.Modified;
                                }
                                else return new ReturnValue { message = item.materialNo + "不存在出库记录,返审失败", status = false };
                                #endregion
                            }
                            model.checkStaff = checkstaff;
                            model.status = 0;
                        }
                        else if (model.status == 4)
                        {
                            model.checkStaff = checkstaff;
                            model.status = 0;
                        }
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
                }
            }
        }
        #endregion

        #region 销售退单
      
        public List<KeyValue> GetCanReturn(int supplier)
        {
            var list = from p in context.StockOuts
                       join pd in context.StockOutDetails on p.stockoutNo equals pd.stockoutNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.status == 1 && (p.outType == 1||p.outType == 4) && p.supplierId==supplier
                       orderby p.createDate descending
                       select new KeyValue { text = p.stockoutNo, value = "", column1 = m.materialNo, column2 = m.materialName, column3 = m.materialModel ,column4=m.tunumber};
            return list.Distinct().ToList();
        }
        public IQueryable<ReturnModel> ReturnSellModelList()
        {
            var list = from r in context.StockReturns
                       join p in context.Departments on r.depId equals p.depId
                       join e in context.Employees on r.staffId equals e.staffId
                       join s in context.Suppliers on r.supplierId equals s.supplierId
                       orderby r.createDate descending
                       select new ReturnModel
                       {
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           returnNo = r.returnNo,
                           staffId = r.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           valid = r.valid,
                           checkStaff = r.checkStaff, returnType=r.returnType,
                           deportStaff = r.deportStaff, supplierId=s.supplierId, supplierName=s.supplierName
                       };
            return list;
        }
        public List<V_ReturnOutModel> ReturnSellModelList(string where, string orderby)
        {
            var list = context.V_ReturnOutModel.SqlQuery("select * from V_ReturnOutModel where   returnType in(0,1) and returnNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public string AddStockReturn(int staff, int dep, int supplier, string remark, string deportStaff, int returntype)
        {
            try
            {
                StockReturn model = new StockReturn();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.returnNo = GetStockReturnNo();
                model.staffId = staff;
                model.deportStaff = deportStaff;
                model.status = 0;
                model.supplierId = supplier; model.returnType = returntype;
                context.StockReturns.Add(model);
                context.SaveChanges();
                return model.returnNo;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 报表
        public IQueryable<ReportDepot> ReportDepot()
        {
            var list = from d in context.Depots
                       join dd in context.DepotDetails on d.depotId equals dd.depotId
                       join m in context.Materials on dd.materialNo equals m.materialNo
                       orderby m.orderNo ascending
                       select new ReportDepot
                       {
                           amout = dd.depotAmount,
                           depotName = d.depotName,
                           materialCategory = m.category,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialUnit = m.unit,
                           remark = dd.remark, depotId=d.depotId, materialNo=m.materialNo
                       };
            return list;
        }
        public IQueryable<ReportPurchaseRequire> ReportPurchaseRequire()
        {
            var list = from p in context.PurchaseRequires
                       join pd in context.PurchaseRequireDetails on p.requireNo equals pd.requireNo
                       join e in context.Employees on p.staffId equals e.staffId
                       join d in context.Departments on p.depId equals d.depId
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       orderby p.createDate descending
                       select new ReportPurchaseRequire
                       {
                           buyAmount = pd.buyAmount,
                           createDate = p.createDate,
                           depId = d.depId,
                           depName = d.depName,
                           isover = p.isover,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           orderAmount = pd.orderAmount,
                           remark = pd.remark, needdate=pd.createDate,
                           requireNo = p.requireNo,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           valid = p.valid
                       };
            return list;
        }
        public IQueryable<ReportPurchase> ReportPurchase()
        {
            var list = from p in context.Purchases
                       join pd in context.PurchaseDetails on p.purchaseNo equals pd.purchaseNo
                       join e in context.Employees on p.staffId equals e.staffId
                       join d in context.Departments on p.depId equals d.depId
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       join s in context.Suppliers on p.supplierId equals s.supplierId
                       join r in context.PurchaseRequireDetails on pd.requireDetailSn equals r.detailSn into g
                       from x in g.DefaultIfEmpty()
                       orderby p.createDate descending
                       select new ReportPurchase
                       {
                           createDate = p.createDate, sendDate=pd.sendDate.Value,
                           depId = d.depId,
                           depName = d.depName,
                           isover = p.isover,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           poAmount = pd.poAmount,
                           poPrice = pd.poPrice,
                           poRemain = pd.poRemain,
                           purchaseNo = p.purchaseNo,
                           remark = pd.remark,
                           returnAmount = pd.returnAmount,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           supplierId = s.supplierId,
                           suppliername = s.supplierName,
                           type = p.type == 0 ? "普通采购" : "申请采购",
                           valid = p.valid,
                           requireNo =  x == null ? "" : x.requireNo
                       };

            return list;

        }
        public IQueryable<ReportStock> ReportStockIn()
        {
            var list = from p in context.StockIns
                       join pd in context.StockInDetails on p.stockInNo equals pd.stockinNo
                       join e in context.Employees on p.staffId equals e.staffId
                       join d in context.Departments on p.depId equals d.depId
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       join de in context.Depots on pd.depotId equals de.depotId
                       join s in context.Suppliers on p.supplierId equals s.supplierId into g
                       from x in g.DefaultIfEmpty()
                       orderby p.createDate descending
                       select new ReportStock
                       {
                           createDate = p.createDate,
                           depId = d.depId,
                           depName = d.depName,
                           isover = p.isover,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           remark = pd.remark, deportStaff=p.deportStaff,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           valid = p.valid,
                           amount = pd.inAmount,
                           datatype = p.inType,
                           depotId = de.depotId,
                           depotName = de.depotName,
                           materialCategory = m.category,
                           returnamount = pd.returnAmount,
                           stockNo = p.stockInNo,
                           supplierId = x == null ? 0 : x.supplierId,
                           supplierName = x == null ? "" : x.supplierName
                       };
            return list;
        }
        public IQueryable<ReportStock> ReportStockOut()
        {
            var list = from p in context.StockOuts
                       join pd in context.StockOutDetails on p.stockoutNo equals pd.stockoutNo
                       join e in context.Employees on p.staffId equals e.staffId
                       join d in context.Departments on p.depId equals d.depId
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       join de in context.Depots on pd.depotId equals de.depotId
                       join s in context.Suppliers on p.supplierId equals s.supplierId into g
                       from x in g.DefaultIfEmpty()
                       orderby p.createDate descending
                       select new ReportStock
                       {
                           createDate = p.createDate,
                           depId = d.depId,
                           depName = d.depName,
                           isover = p.isover,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           remark = pd.remark,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           valid = p.valid,
                           amount = pd.outAmount,
                           datatype = p.outType,
                           depotId = de.depotId,
                           depotName = de.depotName,
                           materialCategory = m.category,
                           returnamount = pd.returnAmount, deportStaff=p.deportStaff,
                           stockNo = p.stockoutNo,
                           supplierId = x == null ? 0 : x.supplierId,
                           supplierName = x == null ? "" : x.supplierName
                       };
            return list;
        }
        public IQueryable<ReportStock> ReportStockOtN()
        {
            var list1 = from i in context.StockExchanges
                        join d in context.StockInDetails on i.changeNo equals d.changeNo
                        join m in context.Materials on d.materialNo equals m.materialNo
                        join de in context.Depots on d.depotId equals de.depotId
                        join e in context.Employees on i.staffId equals e.staffId
                        join p in context.Departments on i.depId equals p.depId
                        orderby d.detailSn descending
                        select new ReportStock
                        {
                            createDate = i.createDate,
                            depId = i.depId,
                            depName = p.depName,
                            isover = i.isover,
                            materialModel = m.materialModel,
                            materialName = m.materialName,
                            materialTu = m.tunumber,
                            materialNo = m.materialNo,
                            materialUnit = m.unit,
                            remark = d.remark,
                            staffId = e.staffId,
                            staffName = e.staffName,
                            status = i.status,
                            valid = p.valid,
                            amount = d.inAmount,
                            changeType = 0,
                            depotId = de.depotId,
                            depotName = de.depotName,
                            materialCategory = m.category,
                            changeNo = i.changeNo, deportStaff=i.deportStaff
                        };


            var list2 = from i in context.StockExchanges
                       join d in context.StockOutDetails on i.changeNo equals d.changeNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                        join de in context.Depots on d.depotId equals de.depotId
                        join e in context.Employees on i.staffId equals e.staffId
                        join p in context.Departments on i.depId equals p.depId
                       orderby d.detailSn descending
                       select new ReportStock
                       {
                           createDate = i.createDate,
                           depId = i.depId,
                           depName = p.depName,
                           isover = i.isover,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           remark = d.remark,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = i.status,
                           valid = p.valid,
                           amount = d.outAmount,
                           changeType = 1,
                           depotId = de.depotId,
                           depotName = de.depotName,
                           materialCategory = m.category,
                           changeNo = i.changeNo, deportStaff=d.remark
                       };
            var list= list1.Union(list2);
           return list.OrderByDescending(p => p.createDate).OrderBy(p => p.changeNo).ThenBy(p => p.changeType);


        }
        public IQueryable<ReportPurchase> ReportOrderRetrun()
        {
            var list = from r in context.PurchaseReturns
                       join rd in context.PurchaseReturnDetails on r.returnNo equals rd.returnNo
                       join m in context.Materials on rd.materialNo equals m.materialNo
                       join de in context.Depots on rd.depotId equals de.depotId
                       join e in context.Employees on r.staffId equals e.staffId
                       join p in context.Departments on r.depId equals p.depId
                       join od in context.PurchaseDetails on rd.purchaseDetailSn equals od.detailSn
                       join o in context.Purchases on rd.purchaseNo equals o.purchaseNo
                       join s in context.Suppliers on o.supplierId equals s.supplierId
                       orderby r.createDate descending
                       select new ReportPurchase
                       {
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           poAmount = od.poAmount,
                           returnAmount = rd.returnAmount,
                           poPrice = rd.buyPrice,
                           purchaseNo = rd.purchaseNo,
                           remark = rd.remark,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = r.status,
                           supplierId = s.supplierId,
                           suppliername = s.supplierName,
                           valid = p.valid,
                           stockinNo = rd.stockinNo,
                           depotId = de.depotId,
                           depotName = de.depotName,
                            returnNo = r.returnNo, depotStaff=r.deportStaff
                       };
            return list;
        }
        public IQueryable<ReportStock> ReportStockReturn()
        {
            var list = from p in context.StockReturns
                       join pd in context.StockReturnDetails on p.returnNo equals pd.returnNo
                       join o in context.StockOutDetails on pd.stockoutDetailSn equals o.detailSn
                       join e in context.Employees on p.staffId equals e.staffId
                       join d in context.Departments on p.depId equals d.depId
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       join df in context.Depots on pd.fromDepotId equals df.depotId
                       join dt in context.Depots on pd.toDepotId equals dt.depotId              
                       orderby p.createDate descending
                       select new ReportStock
                       {
                           createDate = p.createDate,
                           depId = d.depId,
                           depName = d.depName,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialTu = m.tunumber,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           remark = pd.remark,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           valid = p.valid,
                           amount = o.outAmount,
                           materialCategory = m.category,
                           returnamount = pd.returnAmount,
                           stockNo = pd.stockoutNo, returnNo=p.returnNo, deportStaff=p.deportStaff,
                            fdepotId=df.depotId, fdepotName=df.depotName, tdepotId=dt.depotId, tdepotName=dt.depotName
                       };
            return list;
        }

        public List<V_ReportDepot> ReportDepot(string where, string orderby)
        {
            var list = context.V_ReportDepot.SqlQuery("select * from V_ReportDepot where depotId<>0 " + where + " order by " + orderby);
            return list.ToList();
        }
        public List<V_ReceiptDetail> ReportReceiptDetail(string where, string orderby)
        {
            var list = context.V_ReceiptDetail.SqlQuery("select * from V_ReceiptDetail where reprottype<>'' " + where + " order by " + orderby);
            return list.ToList();
        }

        #endregion


        public List<MaterialCostModel> ReportCostDetail(string key)
        {
            var list = context.Database.SqlQuery<MaterialCostModel>("exec usp_QueryMaterialCost '"+key+"'");
           return list.ToList();
        }
    }
}
