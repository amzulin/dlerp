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
    public class PurchaseRepository : BasicRepository<Purchase>,IPurchaseRepository
    {
        public PurchaseRepository()            : base(new InvoicingContext())        { }
        public PurchaseRepository(InvoicingContext context) : base(context) { }
        
        #region 物料相关
        public IQueryable<Material> QueryMaterial(int type)
        {
            if (type == 0) return from m in context.Materials where m.valid == false orderby m.materialNo ascending select m;
            else if (type == 1) return from m in context.Materials where m.valid == true orderby m.materialNo ascending select m;
            else return from m in context.Materials orderby m.materialNo ascending select m;
        }

        public IQueryable<Supplier> QuerySupplier(int type,int valid)
        {
            if (valid == 0) return from m in context.Suppliers where m.valid == false && m.type == type orderby m.supplierId ascending select m;
            else if (valid == 1) return from m in context.Suppliers where m.valid == true && m.type == type orderby m.supplierId ascending select m;
            else return from m in context.Suppliers where  m.type == type orderby m.supplierId ascending select m;
        }
        #endregion

        #region 采购需求

        public IQueryable<PurchaseRequireMode> RequireList()
        {
            var list = from r in context.PurchaseRequires
                       join p in context.Departments on r.depId equals p.depId
                       join s in context.Employees on r.staffId equals s.staffId
                       orderby r.createDate descending
                       select new PurchaseRequireMode
                       {
                           createDate = r.createDate,
                           depId = r.depId,
                           depName = p.depName,
                           remark = r.remark,
                           requireNo = r.requireNo,
                           staffId = r.staffId,
                           staffName = s.staffName,
                           status = r.status, isover=r.isover, valid=r.valid, checkStaff=r.checkStaff
                       };
            return list;
        }

        public List<V_PurchaseRequireMode> RequireList(string where,string orderby)
        {
            var list = context.V_PurchaseRequireMode.SqlQuery("select * from V_PurchaseRequireMode where requireNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }


        public string GetRequireNo()
        {
            var last = (from r in context.PurchaseRequires orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.requireNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddRequire(string no, int staff, int dep, int status, string remark)
        {
            try
            {
                PurchaseRequire model = new PurchaseRequire();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.requireNo = GetRequireNo();
                model.staffId = staff;
                model.status = status;
                model.isover = 0;
                model.valid = true; model.canfs = true;
                context.PurchaseRequires.Add(model);
                context.SaveChanges();
                return model.requireNo;
            }
            catch
            {
                return "";
            }
        }

        public ReturnValue SaveRequirDetail(string no, List<PurchaseRequireDetailModel> list,string remark)
        {
            using (var c=new InvoicingContext())
            {
                try
                {
                    var model = c.PurchaseRequires.FirstOrDefault(p => p.requireNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在申请单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在申请单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.PurchaseRequireDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.orderAmount = item.orderAmount;
                                d.remark = item.remark;
                                d.createDate = item.needdate;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            PurchaseRequireDetail d = new PurchaseRequireDetail();
                            d.materialNo = item.materialNo;
                            d.orderAmount = item.orderAmount;
                            d.buyAmount = 0;
                            d.requireNo = no;
                            d.remark = item.remark;
                            d.createDate = item.needdate;
                            c.PurchaseRequireDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark!=remark)
                    {
                        model.remark = remark;
                        c.Entry(model).State = EntityState.Modified;
                    }
                    #endregion
                    c.SaveChanges(); return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败："+ex.Message };
                }

            }
        }

        public ViewModel.ReturnValue UpdateRequire(string no, int status, string remark)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRequire(string no)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var r = c.PurchaseRequires.FirstOrDefault(p => p.requireNo == no);
                    if (r != null)
                    {
                        var list = c.PurchaseRequireDetails.Where(p => p.requireNo == no);
                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                c.PurchaseRequireDetails.Remove(item);
                            }
                        }
                        #region 如果是自动生成，则原订单可再次申请
                        var order = c.BomOrders.FirstOrDefault(p => p.bomOrderNo == r.bomOrderNo);
                        if (order != null)
                        {
                            var orderdetail = c.BomOrderDetails.Where(p => p.bomOrderNo == order.bomOrderNo).ToList();
                            if (orderdetail != null)
                            {
                                foreach (var item in orderdetail)
                                {
                                    item.hadRequire = false;
                                    c.Entry(item).State = EntityState.Modified;
                                }
                            }
                        }
                        #endregion
                        c.PurchaseRequires.Remove(r);
                        c.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool ChangeRequireStatus(string no, int status,string checkstaff)
        {
            using (var c=new InvoicingContext())
            {
                try
                {
                    var model = c.PurchaseRequires.FirstOrDefault(p => p.requireNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0) model.status = 1;
                        else if (model.status == 1) model.status = 0;
                        else if (model.status == 4) model.status = 0;

                        model.checkStaff = checkstaff;
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public IQueryable<PurchaseRequireDetailModel> RequireDetailList(string no)
        {
            var list = from r in context.PurchaseRequires
                       join d in context.PurchaseRequireDetails on r.requireNo equals d.requireNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       where d.requireNo==no
                       orderby d.detailSn ascending
                       select new PurchaseRequireDetailModel
                       {
                           createDate = r.createDate,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           orderAmount = d.orderAmount,
                            buyAmount=d.buyAmount,
                           remark = d.remark, materialTu=m.tunumber,
                           requireNo = d.requireNo, needdate=d.createDate, mysenddate=DateTime.MaxValue, xslength=m.xslength//, priceList=new List<ReturnValue>()
                       };
            return list;
        }
        public PurchaseRequireDetailModel RequireDetailOne(int sn)
        {
            var list = (from r in context.PurchaseRequires
                       join d in context.PurchaseRequireDetails on r.requireNo equals d.requireNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                        where d.detailSn == sn
                       orderby d.detailSn ascending
                       select new PurchaseRequireDetailModel
                       {
                           createDate = r.createDate,
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           orderAmount = d.orderAmount,
                           buyAmount = d.buyAmount,
                           remark = d.remark, materialTu=m.tunumber,
                           requireNo = d.requireNo,
                           xslength = m.xslength
                       }).FirstOrDefault();
            return list;
        }
        public ViewModel.ReturnValue AddRequireDetail(string no, string material, decimal amount)
        {
            throw new NotImplementedException();
        }

        public ViewModel.ReturnValue UpdateRequireDetail(string no, int sn, string material, decimal amount)
        {
            throw new NotImplementedException();
        }

        public ViewModel.ReturnValue DeleteRequireDetail(string no, int sn)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 采购单
        public IQueryable<PurchaseModel> PurchaseList()
        {
            var list = from p in context.Purchases
                       join d in context.Departments on p.depId equals d.depId
                       join e in context.Employees on p.staffId equals e.staffId
                       join s in context.Suppliers on p.supplierId equals s.supplierId
                       orderby p.createDate descending
                       select new PurchaseModel
                       {
                           createDate = p.createDate,
                           depId = p.depId,
                           depName = d.depName,
                           purchaseNo = p.purchaseNo,
                           remark = p.remark,
                           staffId = p.staffId,
                           staffName = e.staffName,
                           status = p.status,
                           supplierId = p.supplierId,
                           suppliername = s.supplierName,
                           totalAmount = p.totalAmount,
                           totalCost = p.totalCost,
                           type = p.type, isover=p.isover, valid=p.valid,  checkStaff=p.checkStaff
                       };
            return list;
        }
        public List<V_PurchaseModel> PurchaseList(string where, string orderby)
        {
            var list = context.V_PurchaseModel.SqlQuery("select * from V_PurchaseModel where purchaseNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public string GetPurchaseNo()
        {
            var last = (from r in context.Purchases orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "PO" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.purchaseNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "PO" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "PO" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddPurchase(string no, int staff, int dep, int supplier, int type, int status, string remark)
        {
            try
            {
                Purchase model = new Purchase();
                model.createDate = DateTime.Now;
                model.depId = dep;
                model.remark = remark;
                model.purchaseNo = GetPurchaseNo();
                model.staffId = staff;
                model.status = status;
                model.supplierId = supplier;
                model.type = type;
                model.totalAmount = 0;
                model.totalCost = 0;
                context.Purchases.Add(model);
                context.SaveChanges();
                return model.purchaseNo;
            }
            catch
            {
                return "";
            }
        }
        public ReturnValue SavePurchaseDetail(string no, List<PurchaseDetailModel> list, int supplier, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Purchases.FirstOrDefault(p => p.purchaseNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在采购单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在采购单" };
                    double amount = model.totalAmount;
                    double cost = model.totalCost;
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                amount -= d.poAmount;
                                cost -= d.poAmount * d.poPrice;
                                c.PurchaseDetails.Remove(d);
                            }
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                amount += item.poAmount - d.poAmount;
                                cost -= d.poAmount * d.poPrice;
                                cost += item.poAmount * item.poPrice;
                                d.poPrice = item.poPrice;
                                d.poAmount = item.poAmount;
                                d.poRemain = item.poAmount; 
                                d.sendDate = item.sendDate;
                                d.remark = item.remark;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            PurchaseDetail d = new PurchaseDetail();
                            d.materialNo = item.materialNo;
                            d.poAmount = item.poAmount;
                            d.poPrice = item.poPrice;
                            d.poRemain = item.poAmount;
                            d.purchaseNo = no;
                            amount += item.poAmount;
                            cost += item.poAmount * item.poPrice;
                            d.requireDetailSn = item.requireDetailSn;
                            d.sendDate = item.sendDate;
                            d.remark = item.remark;
                            c.PurchaseDetails.Add(d);
                        }
                    }
                    #region 申请单
                    model.supplierId = supplier;
                    model.remark = remark;
                    model.totalCost = cost;
                    model.totalAmount=amount;
                    c.Entry(model).State = EntityState.Modified;

                    #endregion
                    c.SaveChanges(); return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                }

            }
 
        }
        public ReturnValue SavePurchaseDetail2(string no, List<PurchaseDetailModel> list, int supplier, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Purchases.FirstOrDefault(p => p.purchaseNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在采购单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在采购单" };
                    double amount = model.totalAmount;
                    double cost = model.totalCost;
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        //var require = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.requireDetailSn);
                        //if (require == null) continue;


                        if (item.type == "delete")
                        {
                            var d = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                amount -= d.poAmount;
                                cost -=  d.poAmount * d.poPrice;
                                //require.buyAmount -= d.poAmount;
                                c.PurchaseDetails.Remove(d);
                            }
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.PurchaseDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                amount += item.poAmount - d.poAmount;
                                cost -= d.poAmount * d.poPrice;
                                cost += item.poAmount * item.poPrice;
                                d.poPrice = item.poPrice;
                                var hadpo = d.poAmount; //原有采购数量
                                d.poRemain = item.poRemain + (item.poAmount - d.poAmount);
                                d.poAmount = item.poAmount;
                                d.sendDate = item.sendDate;
                                d.remark = item.remark;
                                //require.buyAmount = (require.buyAmount - hadpo + item.poAmount);

                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            PurchaseDetail d = new PurchaseDetail();
                            d.materialNo = item.materialNo;
                            d.poAmount = item.poAmount;
                            d.poPrice = item.poPrice;
                            d.poRemain = item.poAmount;
                            d.purchaseNo = no;
                            amount += item.poAmount;
                            cost += item.poAmount * item.poPrice;
                            d.requireDetailSn = item.requireDetailSn;
                            d.requireNo = item.requireNo;
                            d.sendDate = item.sendDate;
                            d.remark = item.remark;
                            //require.buyAmount += item.poAmount; 

                            c.PurchaseDetails.Add(d);
                        }
                        //c.Entry(require).State = EntityState.Modified;

                    }
                    #region 申请单
                    model.supplierId = supplier;
                    model.remark = remark;
                    model.totalCost = cost;
                    model.totalAmount=amount;
                    c.Entry(model).State = EntityState.Modified;

                    #endregion
                    c.SaveChanges(); return new ReturnValue { status = true, message = "" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                }

            }
 
        }
        public bool DeletePurchase(string no)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var r = c.Purchases.FirstOrDefault(p => p.purchaseNo == no);
                    if (r != null)
                    {
                        var list = c.PurchaseDetails.Where(p => p.purchaseNo == no);
                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                //var require = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.requireDetailSn);
                                //if (require!=null)
                                //{
                                //    require.buyAmount -= item.poAmount;
                                //    c.Entry(require).State = EntityState.Modified;
                                //}
                                c.PurchaseDetails.Remove(item);
                            }
                        }
                        c.Purchases.Remove(r);
                        c.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public IQueryable<PurchaseDetailModel> PurchaseDetailList(string no)
        {
            var list1 = from p in context.Purchases
                        join de in context.PurchaseDetails on p.purchaseNo equals de.purchaseNo
                        join m in context.Materials on de.materialNo equals m.materialNo                       
                        where p.purchaseNo == no
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
                            materialUnit = m.unit,
                            purchaseNo = de.purchaseNo,
                            remark = de.remark,
                            returnAmount = de.returnAmount, requireNo=de.requireNo, requireDetailSn=de.requireDetailSn,   sendDate=de.sendDate.Value,
                            type = ""
                        };
            return list1;
        }
        public PurchaseDetailModel PurchaseOneDetailList(int detailsn)
        {
            var list1 = (from p in context.Purchases
                        join de in context.PurchaseDetails on p.purchaseNo equals de.purchaseNo
                        join m in context.Materials on de.materialNo equals m.materialNo
                        where de.detailSn == detailsn
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
                            materialUnit = m.unit,
                            purchaseNo = de.purchaseNo,
                            remark = de.remark,
                            returnAmount = de.returnAmount,
                            requireNo = de.requireNo,
                            requireDetailSn = de.requireDetailSn,
                            type = ""
                        }).FirstOrDefault();
            return list1;
        }
        public bool ChangePurchaseStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Purchases.FirstOrDefault(p => p.purchaseNo == no);
                    if (status == -1)
                    {
                        if (model.status == 1 && model.type == 1)
                        {
                            #region 审核通过 作废，更新申请单采购数量
                            var detail = c.PurchaseDetails.Where(p => p.purchaseNo == no);
                            foreach (var item in detail)
                            {
                                var require = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.requireDetailSn);
                                if (require == null) continue;
                                require.buyAmount -= item.poAmount;
                                c.Entry(require).State = EntityState.Modified;
                            }

                            #endregion
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
                            #region 审核通过，更新申请单采购数量
                            if (model.type == 1)
                            {
                                var detail = c.PurchaseDetails.Where(p => p.purchaseNo == no);
                                foreach (var item in detail)
                                {
                                    var require = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.requireDetailSn);
                                    if (require == null) continue;
                                    require.buyAmount += item.poAmount;
                                    c.Entry(require).State = EntityState.Modified;
                                }
                            }
                            #endregion
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.checkStaff = checkstaff;
                            #region 返审，更新申请单采购数量
                            if (model.type == 1)
                            {
                                var detail = c.PurchaseDetails.Where(p => p.purchaseNo == no);
                                foreach (var item in detail)
                                {
                                    var require = c.PurchaseRequireDetails.FirstOrDefault(p => p.detailSn == item.requireDetailSn);
                                    if (require == null) continue;
                                    require.buyAmount -= item.poAmount;
                                    c.Entry(require).State = EntityState.Modified;
                                }
                            }
                            #endregion
                        }
                        else if (model.status == 4)
                        {
                            model.checkStaff = checkstaff; model.status = 0;
                        }
                    }
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();

                    #region 更新申请单是否完工
                    SqlParameter[] sqlparams = new SqlParameter[1];
                    sqlparams[0] = new SqlParameter("@purchaseno", no);
                    var result = (from p in c.Purchases.SqlQuery(" exec usp_over_purchaseRequire @purchaseno", sqlparams) select p).ToList();
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
        #endregion

        #region 申请单采购
        public List<KeyValue> GetCurchaseHadOrder(string no)
        {
            var list = from p in context.Purchases
                       join pd in context.PurchaseDetails on p.purchaseNo equals pd.purchaseNo
                       where p.purchaseNo == no
                       orderby pd.detailSn ascending
                       select new KeyValue { text = pd.requireNo, value = p.purchaseNo };
            return list.Distinct().ToList();
        }
        public IQueryable<PurchaseDetailModel> PurchaseDetailList2(string no)
        {
            var list1 = from p in context.Purchases
                        join de in context.PurchaseDetails on p.purchaseNo equals de.purchaseNo
                        join m in context.Materials on de.materialNo equals m.materialNo      
                        join rde in context.PurchaseRequireDetails on de.requireDetailSn equals rde.detailSn
                        where p.purchaseNo == no 
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
                            materialUnit = m.unit,
                            purchaseNo = de.purchaseNo,
                            remark = de.remark,  requireAmount=rde.orderAmount,
                            returnAmount = de.returnAmount, requireNo=de.requireNo, requireDetailSn=de.requireDetailSn, 
                            type = "", sendDate=de.sendDate.Value
                        };
            return list1;
        }
        public List<KeyValue> GetNeedBuyOrder()
        {
            var list = from p in context.PurchaseRequires
                       join pd in context.PurchaseRequireDetails on p.requireNo equals pd.requireNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.status == 1 && p.isover == 0 && pd.buyAmount < pd.orderAmount
                       orderby p.createDate ascending
                       select new KeyValue { text = p.requireNo, value = "", column1 = m.materialNo, column2 = m.materialName, column3 = m.materialModel };
            return list.Distinct().ToList();
        }

        public List<PurchaseRequireDetailModel> GetPurchaseRequireByPurchaseNo(string require, string purchase)
        {
            var list = from pd in context.PurchaseDetails   
                       join d in context.PurchaseRequireDetails on pd.requireNo equals d.requireNo
                       join m in context.Materials on d.materialNo equals m.materialNo
                       where d.requireNo == require && pd.purchaseNo==purchase && pd.requireDetailSn==d.detailSn
                       orderby d.detailSn ascending
                       select new PurchaseRequireDetailModel
                       {
                           detailSn = d.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = d.materialNo,
                           materialUnit = m.unit,
                           orderAmount = d.orderAmount,
                           buyAmount = d.buyAmount,
                           mebuyAmount=pd.poAmount,
                           mebuyPrice=pd.poPrice, materialTu=m.tunumber,
                           remark = d.remark,
                           requireNo = d.requireNo, mysenddate=d.createDate
                       };
            return list.ToList();
        }
        #endregion

        #region 采购单入库

        public List<KeyValue> GetStorkInHadOrder(string no)
        {
            var list = from p in context.StockIns
                       join pd in context.StockInDetails on p.stockInNo equals pd.stockinNo
                       where p.stockInNo == no
                       orderby pd.detailSn ascending
                       select new KeyValue { text = pd.purchaseNo, value = p.stockInNo };
            return list.Distinct().ToList();
        }
        public List<KeyValue> GetNeedStockInOrder(int suppler)
        {
            var list = from p in context.Purchases
                       join pd in context.PurchaseDetails on p.purchaseNo equals pd.purchaseNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.status == 1 && p.isover == 0 && pd.poRemain>0 && p.supplierId==suppler
                       orderby p.createDate ascending
                       select new KeyValue { text = p.purchaseNo, value = "", column1 = m.materialNo, column2 = m.materialName, column3 = m.materialModel };
            return list.Distinct().ToList();
        }
        public List<StockDetailModel> GetStockHadDetailByPurchase(string no, string order)
        {
            var list=from sin in context.StockIns 
                     join sind in context.StockInDetails on sin.stockInNo equals sind.stockinNo
                     join od in context.PurchaseDetails on sind.purchaseDetailSn equals od.detailSn
                       join m in context.Materials on sind.materialNo equals m.materialNo
                       join de in context.Depots on sind.depotId equals de.depotId
                     where od.purchaseNo==order && sin.stockInNo==no
                     orderby sind.detailSn descending
                     select new StockDetailModel
                     {
                         amount = sind.inAmount,
                         detailSn = sind.detailSn,
                         materialModel = m.materialModel,
                         materialName = m.materialName,
                         materialNo = sind.materialNo,
                         materialUnit = m.unit,
                         purchaseDetailSn = sind.purchaseDetailSn,
                         purchaseNo = sind.purchaseNo,
                         stockNo = sin.stockInNo,
                         materialCategory = m.category,
                         depotName = de.depotName,
                         depotId = sind.depotId,
                         datatype = sin.inType, orderAmout=od.poAmount,remainAmout=od.poRemain,
                         type = ""
                     };
            return list.ToList();
        }
        public List<StockDetailModel> GetPurchaseStockDetail(string no)
        {
            var list = from p in context.Purchases
                       join pd in context.PurchaseDetails on p.purchaseNo equals pd.purchaseNo
                       join m in context.Materials on pd.materialNo equals m.materialNo
                       where p.purchaseNo==no
                       orderby pd.detailSn descending
                       select new StockDetailModel
                       {
                           amount = 0,
                           detailSn = 0,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = m.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = pd.detailSn,
                           purchaseNo = pd.purchaseNo,
                           stockNo = "",
                           materialCategory = m.category,
                           depotName = "",
                           depotId = 0,
                           datatype = 0,
                           orderAmout = pd.poAmount,
                           remainAmout = pd.poRemain,
                           type = ""
                       };
            return list.ToList();
        }
        public List<StockDetailModel> GetStockHadDetailByPurchase(string no)
        {
            var list = from sin in context.StockIns
                       join sind in context.StockInDetails on sin.stockInNo equals sind.stockinNo
                       join od in context.PurchaseDetails on sind.purchaseDetailSn equals od.detailSn
                       join m in context.Materials on sind.materialNo equals m.materialNo
                       join de in context.Depots on sind.depotId equals de.depotId
                       where sin.stockInNo == no
                       orderby sind.detailSn descending
                       select new StockDetailModel
                       {
                           amount = sind.inAmount,
                           detailSn = sind.detailSn,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = sind.materialNo,
                           materialUnit = m.unit,
                           purchaseDetailSn = sind.purchaseDetailSn,
                           purchaseNo = sind.purchaseNo,
                           stockNo = sin.stockInNo,
                           materialCategory = m.category,
                           depotName = de.depotName,
                           depotId = sind.depotId,
                           datatype = sin.inType, 
                           orderAmout = od.poAmount, 
                           remainAmout = od.poRemain,
                           type = ""
                       };
            return list.ToList();
        }
        #endregion

        #region 私有方法

        #endregion
    }
}
