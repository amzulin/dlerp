using Enterprise.Invoicing.Entities;
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
    public class BomRepository : BasicRepository<BomMain>, IBomRepository
    {
        public BomRepository() : base(new InvoicingContext()) { }
        public BomRepository(InvoicingContext context) : base(context) { }


        #region Bom
        public IQueryable<V_BomMaterial> GetBomMaterial()
        {
            var list = from v in context.BomMains
                       join m in context.Materials on v.materialNo equals m.materialNo into temp
                       from tt in temp.DefaultIfEmpty()
                       orderby v.bomId ascending
                       select new V_BomMaterial
                       {
                           bomId = v.bomId,
                           amount = v.amount,
                           otherProject = v.otherProject,
                           loss = v.loss,
                           materialCate = v.materialCate,
                           bigcate = tt == null ? "" : tt.bigcate,
                           category = tt == null ? "" : tt.category,
                           xslength = tt == null ? 0 : tt.xslength,
                           materialModel = tt == null ? "" : tt.materialModel,
                           materialName = tt == null ? "" : tt.materialName,
                           materialNo = tt == null ? "" : tt.materialNo,
                           orderNo = tt == null ? "" : tt.orderNo,
                           unit2 = tt == null ? "" : tt.unit2,
                           ratio = tt == null ? 0 : tt.ratio,
                           tunumber = tt == null ? "" : tt.tunumber,
                           unit = tt == null ? "" : tt.unit,
                           valid = tt == null ? true : tt.valid,
                           parent_Id = v.parent_Id,
                           remark = v.remark,
                           version = v.version,
                           status = v.status,
                           bomName = v.bomName,
                           isChild = v.isChild, startDate=v.startDate, endDate=v.endDate
                       };
            return list;
        }

        public V_BomMaterial GetBomMaterial(int bomid)
        {
            var list = from v in context.BomMains
                       join m in context.Materials on v.materialNo equals m.materialNo into temp
                       from tt in temp.DefaultIfEmpty()
                       where v.bomId==bomid
                       orderby v.bomId ascending
                       select new V_BomMaterial
                       {
                           bomId = v.bomId,
                           amount = v.amount,
                           otherProject = v.otherProject,
                           loss = v.loss,
                           materialCate = v.materialCate,
                           bigcate = tt == null ? "" : tt.bigcate,
                           category = tt == null ? "" : tt.category,
                           xslength = tt == null ? 0 : tt.xslength,
                           materialModel = tt == null ? "" : tt.materialModel,
                           materialName = tt == null ? "" : tt.materialName,
                           materialNo = tt == null ? "" : tt.materialNo,
                           orderNo = tt == null ? "" : tt.orderNo,
                           unit2 = tt == null ? "" : tt.unit2,
                           ratio = tt == null ? 0 : tt.ratio,
                           tunumber = tt == null ? "" : tt.tunumber,
                           unit = tt == null ? "" : tt.unit,
                           valid = tt == null ? true : tt.valid,
                           parent_Id = v.parent_Id,
                           remark = v.remark,
                           version = v.version,
                           status = v.status,
                           bomName = v.bomName,
                           isChild = v.isChild,
                           startDate = v.startDate,
                           endDate = v.endDate
                       };
            return list.FirstOrDefault();
        }
        public V_BomMaerialTwo GetBomMaterialTwo(int bomid)
        {
            var list = from v in context.V_BomMaerialTwo
                       where v.bomId == bomid
                       orderby v.materialName ascending
                       select v;
            return list.FirstOrDefault();
        }
        public IQueryable<V_BomMaerialTwo> GetBomMaterialTwo(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {

                var list = from v in context.V_BomMaerialTwo
                           where v.materialModel.Contains(key) || v.materialName.Contains(key) || v.materialNo.Contains(key) || v.bomName.Contains(key) || v.tunumber.Contains(key)
                           orderby v.materialName ascending
                           select v;
                return list;
            }
            else
            {
                var list = from v in context.V_BomMaerialTwo
                           orderby v.materialName ascending
                           select v;
                return list;
            }
        }

        public object GetOneBom(int bomid, double amount)
        {
            var one = GetBomMaterial().FirstOrDefault(p => p.bomId == bomid);
            var obj = new object();
            if (one != null)
            {
                obj = GetChildBom(one, amount, true);

            }
            else obj = null;

            return obj;
        }
        public object GetChildBom(V_BomMaterial mode, double amount, bool root)
        {
            object obj = new object();
            #region 其他科目
            object virtusl = null;
            if (root)
            {
                //var virtusls = context.BomVirtuals.Where(p => p.bomId == mode.bomId).ToList();
                //if (virtusls != null)
                //{
                //    List<object> list = new List<object>();
                //    foreach (var item in virtusls)
                //    {
                //        var one = new { isroot = "0", canclick = "1", isvir = "1", text = item.virtualName + "，数量" + Math.Round(item.vAmount, 2) + "，单价" + Math.Round(item.vPrice, 2) + (item.remark != "" ? ",备注:" + item.remark : ""), id = item.virtualId, amount = (amount != 0 ? amount * item.vAmount : item.vAmount).ToString("N") };
                //        list.Add(one);
                //    }
                //    if (list.Count > 0)
                //    {
                //        virtusl = new { isroot = "0", canclick = "0", isvir = "0", text = "其他科目", id = mode.bomId, amount = 0, children = list };
                //    }
                //}
            }
            #endregion
            var child = GetBomMaterial().Where(p => p.parent_Id == mode.bomId).ToList();
            var sl = Math.Round(mode.amount, mode.xslength);// "";
            if (child.Count > 0)
            {
                List<object> list = new List<object>();
                foreach (var item in child)
                {
                    var one = GetChildBom(item, amount * mode.amount, false);
                    list.Add(one);
                }
                #region 根节点加入其他科目
                if (root && virtusl != null)
                {
                    //list.Add(virtusl);
                }
                #endregion
                obj = new { isroot = root ? "1" : "0", canclick = "1", isvir = "0", text = mode.isChild ? ("子BOM" + mode.bomName + " 版本：" + mode.version) : ((root ? "" : mode.materialCate + " 编码：") + mode.materialNo + " 名称:" + mode.materialName + " 规格:" + mode.materialModel + " 图号:" + mode.tunumber + " 单位:" + (mode.unit2 != null && mode.unit2 != "" ? mode.unit2 : mode.unit) + (root ? " 版本："+mode.version : " 数量：" + sl) + (mode.loss > 0 ? " 损耗率" + mode.loss + "%" : "")), id = mode.bomId, amount = amount != 0 ? amount * mode.amount : mode.amount, children = list };
            }
            else
                obj = new { isroot = root ? "1" : "0", canclick = "1", isvir = "0", text = mode.isChild ? ("子BOM" + mode.bomName + " 版本：" + mode.version) : ((root ? "" : mode.materialCate + " 编码：") + mode.materialNo + " 名称:" + mode.materialName + " 规格:" + mode.materialModel + " 图号:" + mode.tunumber + " 单位:" + (mode.unit2 != null && mode.unit2 != "" ? mode.unit2 : mode.unit) + (root ? " 版本：" + mode.version : " 数量：" + sl) + (mode.loss > 0 ? " 损耗率" + mode.loss + "%" : "")), id = mode.bomId, amount = amount != 0 ? amount * mode.amount : mode.amount };

            return obj;
        }
        public List<V_BomMaterial> GetChildBomMaterial(int BomId, double amount)
        {
            List<V_BomMaterial> result = new List<V_BomMaterial>();
            var list = GetBomMaterial().Where(p => p.parent_Id == BomId).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (amount != 0) item.amount *= amount;
                    var clist = GetChildBomMaterial(item.bomId, amount != 0 ? item.amount : 0);
                    result.Add(item);
                    if (clist.Count > 0) result.AddRange(clist);
                }
            }
            return result;
        }
        public ReturnValue SaveBom(string type, int id, int? parent_Id, string material, string cate, double amount, string remark, string version)
        {
            using (var c = new InvoicingContext())
            {
                try
                {

                    if (type == "add")
                    {
                        var parent = c.BomMains.FirstOrDefault(p => p.bomId == parent_Id);
                        #region 添加
                        var had = c.BomMains.FirstOrDefault(p => p.parent_Id == parent_Id && p.materialNo == material&&p.version==version);
                        if (had != null) return new ReturnValue { status = false, message = "Bom已存在，不能重复添加!" };
                        var m = new BomMain();
                        m.materialNo = material;
                        m.amount = amount;
                        m.materialCate = cate;
                        m.parent_Id = parent_Id;
                        m.remark = remark; m.loss = 0; m.version = version;
                        m.startDate = DateTime.Now; m.isChild = false;
                        if (parent != null)
                        {
                            if (parent.parent_Id.HasValue && parent.parent_Id > 0)
                            {
                                m.rootId = parent.rootId;
                            }
                            else m.rootId = parent.bomId;
                        }
                        c.BomMains.Add(m);
                        #endregion
                    }
                    else if (type == "edit")
                    {
                        var m = c.BomMains.FirstOrDefault(p => p.bomId == id);
                        if (m == null) return new ReturnValue { status = false, message = "Bom不存在，修改失败" };
                        #region 修改
                        var mn = c.BomMains.FirstOrDefault(p => p.bomId != id && p.parent_Id == m.parent_Id && p.materialNo == material);
                        if (mn != null) return new ReturnValue { status = false, message = "Bom已存在，不能重复添加" };
                        m.materialNo = material;
                        m.amount = amount; m.loss = 0; m.version = version;
                        m.materialCate = cate;
                        //   m.parent_Id = parent_Id;
                        m.remark = remark;
                        c.Entry(m).State = EntityState.Modified;
                        #endregion
                    }
                    else return new ReturnValue { status = false, message = "操作类别错误" };
                    c.SaveChanges();
                    #region 添加得到的id

                    if (type == "add" && !parent_Id.HasValue)
                    {
                        var had = c.BomMains.FirstOrDefault(p => p.parent_Id == null && p.materialNo == material && p.version == version);
                        id = had.bomId;
                    }
                    #endregion
                    return new ReturnValue { status = true, value = id.ToString() };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public ReturnValue DeleteBom(int id)
        {
            try
            {
                var model = context.BomMains.FirstOrDefault(p => p.bomId == id);
                var child = context.BomMains.Where(p => p.parent_Id == id).ToList();
                if (child.Count > 0)
                {
                    foreach (var item in child)
                    {
                        DeleteBom(item.bomId);
                    }
                }
                if (model != null) context.BomMains.Remove(model);
                context.SaveChanges(); return new ReturnValue { status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { status = false, message = "操作失败：" + ex.Message };

            }
        }
        public ReturnValue DeleteVirtual(int id, int forbom)
        {
            try
            {
                var model = context.BomVirtuals.FirstOrDefault(p => p.virtualId == id);

                if (model != null)
                {
                    var bommain = context.BomMains.FirstOrDefault(p => p.bomId == forbom);
                    bommain.rootCost -= model.vAmount * model.vPrice;
                    context.BomVirtuals.Remove(model);
                    context.SaveChanges(); return new ReturnValue { status = true };
                }
                else return new ReturnValue { status = false, message = "科目不存在" };
            }
            catch (Exception ex)
            {
                return new ReturnValue { status = false, message = "操作失败：" + ex.Message };

            }
        }
        public ReturnValue SaveVirtual(string type, int id, int bom, int forbom, string km, double amount, double price, string remark)
        {

            using (var c = new InvoicingContext())
            {
                try
                {
                    var bommain = c.BomMains.FirstOrDefault(p => p.bomId == forbom);
                    if (type == "add")
                    {
                        #region 添加
                        var had = c.BomVirtuals.FirstOrDefault(p => p.bomId == bom && p.virtualName == km);
                        if (had != null) return new ReturnValue { status = false, message = "Bom项目已存在，不能重复添加!" };
                        var m = new BomVirtual();
                        m.virtualName = km;
                        m.vAmount = amount;
                        m.vPrice = price;
                        m.bomId = forbom;
                        m.remark = remark;
                        bommain.rootCost += amount * price;
                        c.BomVirtuals.Add(m);
                        #endregion
                    }
                    else if (type == "edit")
                    {
                        var m = c.BomVirtuals.FirstOrDefault(p => p.virtualId == id);
                        if (m == null) return new ReturnValue { status = false, message = "Bom项目不存在，修改失败" };
                        #region 修改
                        //  var mn = c.BomVirtuals.FirstOrDefault(p => p.virtualId != id && p.virtualName == km);
                        // if (mn != null) return new ReturnValue { status = false, message = "Bom项目已存在，不能重复添加" };

                        bommain.rootCost += (amount * price - m.vAmount * m.vPrice);

                        m.virtualName = km;
                        m.vAmount = amount;
                        m.vPrice = price;
                        m.remark = remark;
                        c.Entry(m).State = EntityState.Modified;
                        #endregion
                    }
                    else return new ReturnValue { status = false, message = "操作类别错误" };

                    c.Entry(bommain).State = EntityState.Modified;
                    c.SaveChanges();

                    return new ReturnValue { status = true, value = "8" };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }

        public List<KeyValue> GetBomNodeCate()
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "BomNodeCategory");
            if (data != null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                    string[] t = item.Split(':');
                    r.Add(new KeyValue { text = t[0], value = t[0] });
                }
            }
            return r;
        }

        public V_BomMaerialTwo GetBomRoot(V_BomMaerialTwo bom)
        {
            if (bom.parent_Id.HasValue)
            {
                var parent = GetBomMaterialTwo(bom.parent_Id.Value);
                if (parent.parent_Id.HasValue)
                {
                    return GetBomRoot(parent);
                }
                return parent;
            }
            else return bom;
        }
        #endregion

        #region Bom订单
        public List<V_BomOrderModel> BomOrderList()
        {
            return context.V_BomOrderModel.ToList();
        }
        public List<V_BomOrderModel> BomOrderList(string where, string orderby)
        {
            var list = context.V_BomOrderModel.SqlQuery("select * from V_BomOrderModel where BomOrderNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public string GetBomOrderNo()
        {
            var last = (from r in context.BomOrders orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "BD" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.bomOrderNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "BD" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "BD" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public string AddBomOrder(BomOrder model)
        {
            try
            {
                model.canfs = true;
                context.BomOrders.Add(model);
                context.SaveChanges();
                return model.bomOrderNo;
            }
            catch
            {
                return "";
            }
        }
        public ReturnValue SaveBomOrderDetail(string no, List<V_BomOrderDetailModel> list, string deportStaff, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.BomOrders.FirstOrDefault(p => p.bomOrderNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在BOM订单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在BOM订单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null) c.BomOrderDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.BomOrderDetails.FirstOrDefault(p => p.detailSn == item.detailSn);
                            if (d != null)
                            {
                                d.bomId = item.bomId;
                                d.remark = item.OrderDetailRemark;
                                d.Amount = item.Amount;
                                d.Price = item.Price;
                                d.sendDate = item.sendDate;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            BomOrderDetail d = new BomOrderDetail();
                            d.bomId = item.bomId;
                            d.remark = item.OrderDetailRemark;
                            d.Amount = item.Amount;
                            d.Price = item.Price;
                            d.sendDate = item.sendDate;
                            d.bomOrderNo = item.bomOrderNo;
                            d.outAmount = 0;
                            d.hadRequire = false;

                            c.BomOrderDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark || model.deportStaff != deportStaff)
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

        public bool DeleteBomOrder(string no)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);
                if (model != null)
                {
                    var details = c.BomOrderDetails.Where(p => p.bomOrderNo == no);
                    foreach (var item in details)
                    {
                        c.BomOrderDetails.Remove(item);
                    }
                    c.BomOrders.Remove(model);
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool DeleteBomOrderDetail(string no, int sn)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (model != null)
                {
                    //if (model.hadBom)
                    //{
                    //    var boms = c.BomOrderDetailLists.Where(p => p.detailSn == sn);
                    //    foreach (var item in boms)
                    //    {
                    //        c.BomOrderDetailLists.Remove(item);
                    //    }
                    //    //var virtusl = c.BomOrderVirtualLists.Where(p => p.detailSn == sn);
                    //    //foreach (var item in virtusl)
                    //    //{
                    //    //    c.BomOrderVirtualLists.Remove(item);
                    //    //}
                    //    model.hadBom = false;
                    //    model.bomDate = null;
                    //    model.createStaff = "";
                    //    c.Entry(model).State = EntityState.Modified;
                    //}
                    //else
                    //{
                        c.BomOrderDetails.Remove(model);
                    //}
                    c.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public IQueryable<V_BomOrderDetailModel> BomOrderDetailList(string no)
        {
            return context.V_BomOrderDetailModel.Where(p => p.bomOrderNo == no).OrderBy(x => x.detailSn);
        }
        public List<V_BomOrderDetailModel> BomOrderDetailList(string where, string orderby)
        {
            var list = context.V_BomOrderDetailModel.SqlQuery("select * from V_BomOrderDetailModel where BomOrderNo<>'' " + where + " order by " + orderby);
            return list.ToList();
        }
        public V_BomOrderDetailModel BomOrderDetailOne(int sn)
        {
            return context.V_BomOrderDetailModel.FirstOrDefault(p => p.detailSn == sn);
        }
        public ReturnValue ChangeBomOrderStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            //model.isover = 1;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;
                            #region 返审 把已生成的bom明细删除
                            //var details = c.BomOrderDetails.Where(p => p.bomOrderNo == no);
                            //foreach (var item in details)
                            //{
                            //    if (item.hadBom)
                            //    {
                            //        var boms = c.BomOrderDetailLists.Where(p => p.detailSn == item.detailSn);
                            //        foreach (var ob in boms)
                            //        {
                            //            c.BomOrderDetailLists.Remove(ob);
                            //        }
                            //        item.hadBom = false;
                            //        item.createStaff = "";
                            //        item.outAmount = 0;
                            //        c.Entry(item).State = EntityState.Modified;
                            //    }
                            //}

                            #endregion
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

        public ReturnValue BomOrderDetailCreate(string no, int sn, string staff)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在BOM订单明细", status = false };
              //  if (detail.hadBom) return new ReturnValue { message = "BOM已生成", status = false };
                //var r = CreateBomDetail(sn, detail.bomId, detail.bomId, (double)detail.Amount, detail.sendDate);
                ////var r2 = CreateVirtDetail(sn, detail.bomId, detail.Amount);
                //if (r)//&& r2
                //{
                //    detail.hadBom = true;
                //    detail.createStaff = staff;
                //    detail.bomDate = DateTime.Now;

                //    context.Entry(detail).State = EntityState.Modified;
                //    context.SaveChanges();
                //    return new ReturnValue { message = "", status = r };
                //}
                //else
                  return new ReturnValue { message = "生成失败", status =false };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }

        public bool CreateBomDetail(int sn, int rootbom, int bomid, double amount, DateTime? needdate)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var bom = c.BomMains.FirstOrDefault(p => p.bomId == bomid);
                    var material = c.Materials.FirstOrDefault(p => p.materialNo == bom.materialNo);
                    var totalneed = amount * (1 + bom.loss / 100.0);
                    if (material.unit2 != null && material.unit2 != "" && material.ratio.HasValue)
                    {
                        totalneed = Math.Ceiling((amount * (1 + bom.loss / 100.0)) / material.ratio.Value);
                    }
                    if (bom.materialCate == "外购")
                    {
                        BomOrderDetailList one = new Entities.Models.BomOrderDetailList();
                        one.bomAmount = totalneed;
                        one.remainAmount = totalneed;
                        one.bomId = bomid;
                        one.detailSn = sn;
                        one.needDate = needdate;
                        c.BomOrderDetailLists.Add(one);
                        c.SaveChanges();
                    }
                    else if (bom.materialCate == "外协")
                    {
                        #region 生成委外单
                        #region 计算数量
                        //实际需求=该bom订单用量总数-库存-已有订单未领数量+采购已下单未交货数量
                        //System.Data.SqlClient.SqlParameter[3] param=new SqlParameter[];
                        //c.Database.SqlQuery<float>("");

                        #endregion
                        #endregion
                        var mychild = c.BomMains.Where(p => p.parent_Id == bom.bomId);
                    }
                    var child = c.BomMains.Where(p => p.parent_Id == bomid).ToList();
                    if (child.Count > 0)
                    {
                        foreach (var item in child)
                        {
                            CreateBomDetail(sn, rootbom, item.bomId, amount * item.amount, needdate);
                        }
                    }
                    return true;

                }
                catch
                {
                    return false;
                }
            }
        }

        
        public bool CreateVirtDetail(int sn, int bomid, double amount)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var bom = c.BomMains.FirstOrDefault(p => p.bomId == bomid);
                    var child = c.BomVirtuals.Where(p => p.bomId == bomid).ToList();
                    if (child.Count > 0)
                    {
                        foreach (var item in child)
                        {
                            BomOrderVirtualList v = new Entities.Models.BomOrderVirtualList();
                            v.virtualId = item.virtualId;
                            v.sPrice = item.vPrice;
                            v.sAmount = amount * (1 + bom.loss / 100.0) * item.vAmount;
                            v.remark = "";
                            v.detailSn = sn;
                            c.BomOrderVirtualLists.Add(v);
                        }
                        c.SaveChanges();
                    }
                    return true;

                }
                catch
                {
                    return false;
                }
            }
        }

        public IQueryable<V_BomOrderDetailListModel> GetBomOrderBomDetailList(int sn)
        {
            return context.V_BomOrderDetailListModel.Where(p => p.detailSn == sn);
        }
        public IQueryable<V_BomOrderVirtualDetail> GetBomOrderVirtualDetailList(int sn)
        {
            return context.V_BomOrderVirtualDetail.Where(p => p.detailSn == sn);
        }
        public ReturnValue SaveBomOrderDetail(string no, int sn, List<int> ids, List<double> counts, List<string> dates, List<string> remarks, string st)
        {
            try
            {
                if (st != "")
                {
                    #region 已生成需求单
                    var require = context.PurchaseRequires.FirstOrDefault(p => p.requireNo == st);
                    var order = context.BomOrderDetails.FirstOrDefault(p => p.detailSn == sn);
                    if (require == null || order == null)
                    {
                        return new ReturnValue { message = "未生成采购需求单" + st + "或客户订单不存在", status = false };
                    }
                    else
                    {
                        require.bomOrderNo = no;
                        context.Entry(require).State = EntityState.Modified;
                        order.hadRequire = true;
                        context.Entry(order).State = EntityState.Modified;
                    }
                    #endregion
                }
                for (int i = 0; i < ids.Count; i++)
                {
                    var id = ids[i];
                    var detail = context.BomOrderDetailLists.FirstOrDefault(p => p.detailListSn == id);
                    if (detail == null) return new ReturnValue { message = "不存在BOM订单明细", status = false };
                    detail.bomAmount = counts[i];
                    try
                    {
                        detail.needDate = Convert.ToDateTime(dates[i]);
                    }
                    catch { detail.needDate = null; }
                    detail.remark = remarks[i];

                    #region 生成需求单
                    if (st != "")
                    {

                        PurchaseRequireDetail d = new PurchaseRequireDetail();
                        var mno = context.BomMains.FirstOrDefault(p => p.bomId == detail.bomId);
                        d.materialNo = mno.materialNo;
                        d.orderAmount = detail.bomAmount;
                        d.buyAmount = 0;
                        d.requireNo = st;
                        d.remark = detail.remark;
                        d.createDate = detail.needDate.HasValue ? detail.needDate.Value : DateTime.MaxValue;
                        context.PurchaseRequireDetails.Add(d);
                    }
                    #endregion
                    context.Entry(detail).State = EntityState.Modified;
                }
                context.SaveChanges();
                return new ReturnValue { message = "", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }
        #endregion

        #region  单据自动生成

        public ReturnValue CreatePurcharseRequire(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                if (detail.hadRequire) return new ReturnValue { message = "采购申请已生成", status = false };
                var purchase = context.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);
                var detaillist = GetNeedBuyList(detail.bomId, (double)detail.Amount);
                if (detaillist == null || detaillist.Count < 1) return new ReturnValue { message = "不存在申请单明细，生成失败", status = false };
                #region 生成申请单
                var requireno = GetRequireNo();
                PurchaseRequire model = new PurchaseRequire();
                model = context.PurchaseRequires.FirstOrDefault(p => p.bomOrderNo == no);
                if (model != null)
                {
                    #region 已存在申请单
                    model.status = 0;
                    model.isover = 0;
                    model.valid = true;
                    model.isclose = false;
                    model.canfs = true;
                    context.Entry(model).State = EntityState.Modified;
                    #endregion
                }
                else
                {
                    #region 不存在申请单
                    model = new PurchaseRequire();
                    model.createDate = DateTime.Now;
                    if (dep.HasValue) model.depId = dep.Value;
                    model.remark = "";
                    model.requireNo = requireno;
                    if (staff.HasValue) model.staffId = staff.Value;
                    model.bomOrderNo = detail.bomOrderNo;
                    model.orderDetailSn = detail.detailSn;
                    model.status = 0;
                    model.isover = 0;
                    model.valid = true;
                    model.isclose = false;
                    model.canfs = true;
                    context.PurchaseRequires.Add(model);

                    #endregion
                } 
                #endregion

                #region 增加明细
                foreach (var item in detaillist)
                {

                    PurchaseRequireDetail d = new PurchaseRequireDetail();
                    d = context.PurchaseRequireDetails.FirstOrDefault(p => p.requireNo == model.requireNo && p.materialNo == item.value);
                    if (d == null)
                    {
                        d = new PurchaseRequireDetail();
                        d.materialNo = item.value;
                        d.orderAmount = item.double_value.Value;
                        d.buyAmount = 0;
                        d.requireNo = model.requireNo;
                        d.remark = item.remark;
                        d.createDate = DateTime.Now;
                        context.PurchaseRequireDetails.Add(d);
                    }
                    else
                    {
                        d.orderAmount += item.double_value.Value;
                        context.Entry(d).State = EntityState.Modified;
                    }
                }
                #endregion
                detail.hadRequire = true;
                detail.requireNo = requireno;
                if (purchase.canfs)
                {
                    purchase.canfs = false;
                    context.Entry(purchase).State = EntityState.Modified;
                }
                context.Entry(detail).State = EntityState.Modified;
                context.SaveChanges();

                return new ReturnValue { message = "生成成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }
        public ReturnValue CreateDelegate(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                if (detail.haddelegate) return new ReturnValue { message = "委外工单已生成", status = false };
                var purchase = context.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);
                var detaillist = GetNeedDelegateList(detail.bomId, (double)detail.Amount, staff, detail.bomOrderNo, detail.detailSn);
                if (detaillist > 0)
                    detail.haddelegate = true;
                else { return new ReturnValue { message = "未生成生成委外工单", status = false }; }
                context.Entry(detail).State = EntityState.Modified;
                if (purchase.canfs)
                {
                    purchase.canfs = false;
                    context.Entry(purchase).State = EntityState.Modified;
                }
                context.SaveChanges();

                return new ReturnValue { message = "生成成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }
        public ReturnValue CreateProduce(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                if (detail.hadproduce) return new ReturnValue { message = "领料工单已生成", status = false };
                var purchase = context.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);
                var detaillist = ProductNoList(true, detail.bomId, (double)detail.Amount, staff, detail.bomOrderNo, detail.detailSn);
                if (detaillist > 0)
                    detail.hadproduce = true;
                else { return new ReturnValue { message = "未生成生成领料工单", status = false }; }
                context.Entry(detail).State = EntityState.Modified;
                if (purchase.canfs)
                {
                    purchase.canfs = false;
                    context.Entry(purchase).State = EntityState.Modified;
                }
                context.SaveChanges();

                return new ReturnValue { message = "生成成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }

        public ReturnValue DeletePurcharseRequire(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                PurchaseRequire require = new PurchaseRequire();
                if (detail != null) require = context.PurchaseRequires.FirstOrDefault(p => p.bomOrderNo == detail.bomOrderNo);
                if (detail == null || !detail.hadRequire || require == null)
                {
                    detail.hadRequire = false;
                    context.Entry(detail).State = EntityState.Modified;
                    context.SaveChanges();
                    if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                    if (require == null) return new ReturnValue { message = "不存在采购申请单", status = false };
                    return new ReturnValue { message = "删除失败", status = false };
                }
                else
                {

                    #region 删除
                    if (require.canfs == false || require.status != 0) return new ReturnValue { message = "采购申请单已使用，不允许删除", status = false };
                    var listreq = context.PurchaseRequireDetails.Where(p => p.requireNo == require.requireNo).ToList();//所有申请明细
                    var detaillist = GetNeedBuyList(detail.bomId, (double)detail.Amount);//该订单明细的申请明细
                    double totalamount = 0;
                    foreach (var item in listreq)
                    {
                        var hadreq = detaillist.FirstOrDefault(p => p.value == item.materialNo);
                        if (hadreq != null)
                        {
                            item.orderAmount -= hadreq.double_value.Value;
                            if (item.orderAmount < 0) item.orderAmount = 0;
                            context.Entry(item).State = EntityState.Modified;
                        }
                        totalamount += item.orderAmount;
                    }
                    #region 明细数量之和为0 清除单据
                    if (totalamount == 0)
                    {
                        foreach (var item in listreq)
                        {
                            context.PurchaseRequireDetails.Remove(item);
                        }
                        context.PurchaseRequires.Remove(require);
                    }
                    #endregion
                    #endregion

                    detail.requireNo = "";
                }

                detail.hadRequire = false;
                context.Entry(detail).State = EntityState.Modified;
                context.SaveChanges();

                return new ReturnValue { message = "删除成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }
        public ReturnValue DeleteDelegate(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                if (!detail.haddelegate) return new ReturnValue { message = "未生成委外单", status = false };

                var delegates = context.DelegateOrders.Where(p => p.bomOrderNo == no && p.orderDetailSn == sn).ToList();
                if (delegates == null || delegates.Count < 1) return new ReturnValue { message = "不存在委外单", status = false };
                else
                {
                    #region 删除
                    foreach (var require in delegates)
                    {


                        if (require.status != 0) return new ReturnValue { message = "委外单请单已使用，不允许删除", status = false };
                        var listreq = context.DelegateOrderDetails.Where(p => p.delegateNo == require.delegateNo).ToList();
                        foreach (var item in listreq)
                        {
                            if (listreq != null) context.DelegateOrderDetails.Remove(item);
                        }
                        context.DelegateOrders.Remove(require);
                    }
                    #endregion

                    detail.haddelegate = false;
                    context.Entry(detail).State = EntityState.Modified;
                }

                //var purchase = context.BomOrders.FirstOrDefault(p => p.bomOrderNo == no);

                //if (purchase.canfs)
                //{
                //    purchase.canfs = false;
                //    context.Entry(purchase).State = EntityState.Modified;
                //}
                context.SaveChanges();

                return new ReturnValue { message = "删除成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }
        public ReturnValue DeleteProduce(string no, int sn, int? staff, int? dep)
        {
            try
            {
                var detail = context.BomOrderDetails.FirstOrDefault(p => p.bomOrderNo == no && p.detailSn == sn);
                if (detail == null) return new ReturnValue { message = "不存在客户订单明细", status = false };
                if (!detail.hadproduce) return new ReturnValue { message = "未生成领料单", status = false };

                var delegates = context.Productions.Where(p => p.bomOrderNo == no && p.orderDetailSn == sn).ToList();
                if (delegates == null || delegates.Count < 1)
                {
                    detail.hadproduce = false;
                    context.Entry(detail).State = EntityState.Modified;
                    context.SaveChanges();
                    return new ReturnValue { message = "不存在委外单", status = false };
                }
                else
                {
                    #region 删除
                    foreach (var require in delegates)
                    {

                        if (require.status != 0) return new ReturnValue { message = "领料单请单已使用，不允许删除", status = false };
                        var listreq = context.ProduceDetails.Where(p => p.produceNo == require.produceNo).ToList();
                        foreach (var item in listreq)
                        {
                            if (listreq != null) context.ProduceDetails.Remove(item);
                        }
                        context.Productions.Remove(require);
                    }
                    #endregion

                    detail.hadproduce = false;
                    context.Entry(detail).State = EntityState.Modified;
                }
                context.SaveChanges();

                return new ReturnValue { message = "删除成功", status = true };
            }
            catch (Exception ex)
            {
                return new ReturnValue { message = "操作异常(" + ex.Message + ")", status = false };
            }
        }


        /// <summary>
        /// 得到要生成采购申请的外购列表
        /// </summary>
        /// <param name="bomid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<KeyValue> GetNeedBuyList(int bomid, double amount)
        {
            List<KeyValue> result = new List<KeyValue>();
            using (var c = new InvoicingContext())
            {
                var totalneed = amount;
                var bom = c.BomMains.FirstOrDefault(p => p.bomId == bomid);
                if (bom.materialCate == "外购")
                {
                    try
                    {
                        var material = c.Materials.FirstOrDefault(p => p.materialNo == bom.materialNo);
                        if (material != null)
                        {
                            totalneed = amount * (1 + bom.loss / 100.0);
                            if (material.unit2 != null && material.unit2 != "" && material.ratio.HasValue)
                            {
                                totalneed = Math.Ceiling((amount * (1 + bom.loss / 100.0)) / material.ratio.Value);
                            }
                            //SqlParameter[] para = new SqlParameter[3];
                            //para[0] = new SqlParameter("@material", bom.materialNo);
                            //para[1] = new SqlParameter("@total", totalneed);
                            //para[2] = new SqlParameter("@need", 0);
                            //para[2].Direction = System.Data.ParameterDirection.Output;
                            //double lamount = 0;
                            //var list = ServiceDB.Instance.QueryModelList<OneModel>("exec usp_get_purchaseAmount @material,@total,@need output", para);
                            //lamount = Convert.ToDouble(para[2].Value);
                            result.Add(new KeyValue { value = bom.materialNo, double_value = totalneed, });//lamount
                        }
                    }
                    catch
                    {
                    }
                }
                var child = c.BomMains.Where(p => p.parent_Id == bomid).ToList();
                if (child.Count > 0)
                {
                    foreach (var item in child)
                    {
                        var list = GetNeedBuyList(item.bomId, totalneed * item.amount);
                        result.AddRange(list);
                    }
                }
                return result;
            }
        }


        public int GetNeedDelegateList(int bomid, double amount, int? staff, string orderno, int ordersn)
        {
            int result = 0;
            using (var c = new InvoicingContext())
            {
                var totalneed = amount;
                var bom = c.BomMains.FirstOrDefault(p => p.bomId == bomid);
                if (bom.materialCate == "外协")
                {
                    #region 外协数量
                    var material = c.Materials.FirstOrDefault(p => p.materialNo == bom.materialNo);
                    totalneed = amount * (1 + bom.loss / 100.0);
                    if (material.unit2 != null && material.unit2 != "" && material.ratio.HasValue)
                    {
                        totalneed = Math.Ceiling((amount * (1 + bom.loss / 100.0)) / material.ratio.Value);
                    }
                    //SqlParameter[] para = new SqlParameter[3];
                    //para[0] = new SqlParameter("@material", bom.materialNo);
                    //para[1] = new SqlParameter("@total", totalneed);
                    //para[2] = new SqlParameter("@need", 0);
                    //para[2].Direction = System.Data.ParameterDirection.Output;
                    double lamount = 0;
                    //var list = ServiceDB.Instance.QueryModelList<OneModel>("exec usp_get_purchaseAmount @material,@total,@need output", para);
                    //lamount = Convert.ToDouble(para[2].Value);
                    lamount = totalneed;
                    #endregion
                    #region 创建外协单
                    var childw = c.BomMains.Where(p => p.parent_Id == bomid && p.isChild == false).ToList();
                    #region 有子bom
                    var nextchild = c.BomMains.Where(p => p.parent_Id == bomid && p.isChild == true).ToList();
                    if (nextchild.Count > 0)
                    {
                        List<BomMain> childbom = new List<BomMain>();
                        foreach (var item in nextchild)
                        {
                            childbom = c.BomMains.Where(p => p.parent_Id == item.bomId && p.isChild == false).ToList();
                        }
                        if (childw == null) childw = new List<BomMain>();
                        childw.AddRange(childbom);
                        childbom = new List<BomMain>();
                    }
                    #endregion 

                    if (childw.Count > 0)
                    {
                        try
                        {
                            var requireno = GetDelegateNo();
                            #region 单头
                            DelegateOrder model = new DelegateOrder();
                            model.createDate = DateTime.Now;
                            model.remark = "";
                            model.delegateNo = requireno;
                            if (staff.HasValue) model.staffId = staff.Value;
                            model.status = 0;
                            model.isover = 0;
                            model.valid = true;
                            model.isclose = false;
                            model.bomOrderNo = orderno;
                            model.bomId = bom.bomId;
                            model.materialNo = bom.materialNo;
                            model.amount =Convert.ToDecimal( lamount);
                            model.backAmount = 0;
                            model.price = 0; model.orderDetailSn = ordersn;
                            model.backDate = DateTime.Now.AddMonths(1);
                            model.deportStaff = "";
                            c.DelegateOrders.Add(model);
                            #endregion
                            #region 明细
                            foreach (var item in childw)
                            {
                                DelegateOrderDetail del = new DelegateOrderDetail();
                                del.delegateNo = requireno;
                                del.materialNo = item.materialNo;
                                var materialw = c.Materials.FirstOrDefault(p => p.materialNo == item.materialNo);
                                var totalneedw = item.amount * lamount * (1 + bom.loss / 100.0);
                                if (materialw.unit2 != null && materialw.unit2 != "" && materialw.ratio.HasValue)
                                {
                                    totalneedw = Math.Ceiling((item.amount * lamount * (1 + bom.loss / 100.0)) / materialw.ratio.Value);
                                }
                                del.amount =Convert.ToDecimal( totalneedw);
                                del.price = 0;
                                del.sendAmount = 0;
                                del.backAmount = 0;

                                c.DelegateOrderDetails.Add(del);
                            }
                            #endregion
                            c.SaveChanges();
                            result = 1;
                        }
                        catch
                        {
                            result = 0;
                        }
                    }
                    #endregion
                }
                var child = c.BomMains.Where(p => p.parent_Id == bomid).ToList();
                if (child.Count > 0)
                {
                    foreach (var item in child)
                    {
                        result += GetNeedDelegateList(item.bomId, totalneed * item.amount, staff, orderno, ordersn);
                    }
                }
                return result;
            }
        }

        public int ProductNoList(bool root,int bomid, double amount, int? staff, string orderno, int ordersn)
        {
            int result = 0;
            using (var c = new InvoicingContext())
            {
                var totalneed = amount;
                var bom = c.BomMains.FirstOrDefault(p => p.bomId == bomid);
                if (root||bom.materialCate=="自制")
                {
                    
                
                    #region 自制数量
                    var material = c.Materials.FirstOrDefault(p => p.materialNo == bom.materialNo);
                    totalneed = amount * (1 + bom.loss / 100.0);
                    if (material.unit2 != null && material.unit2 != "" && material.ratio.HasValue)
                    {
                        totalneed = Math.Ceiling((amount * (1 + bom.loss / 100.0)) / material.ratio.Value);
                    }
                    //SqlParameter[] para = new SqlParameter[3];
                    //para[0] = new SqlParameter("@material", bom.materialNo);
                    //para[1] = new SqlParameter("@total", totalneed);
                    //para[2] = new SqlParameter("@need", 0);
                    //para[2].Direction = System.Data.ParameterDirection.Output;
                    double lamount = 0;
                    //var list = ServiceDB.Instance.QueryModelList<OneModel>("exec usp_get_purchaseAmount @material,@total,@need output", para);
                    //lamount = Convert.ToDouble(para[2].Value);
                    lamount = totalneed;
                    #endregion
                    #region 创建领料
                    
                    var childw = c.BomMains.Where(p => p.parent_Id == bomid&&p.isChild==false).ToList();
                   #region 有子bom
                    var nextchild = c.BomMains.Where(p => p.parent_Id == bomid && p.isChild == true).ToList();
                    if (nextchild.Count > 0)
                    {
                        List<BomMain> childbom = new List<BomMain>();
                        foreach (var item in nextchild)
                        {
                            childbom = c.BomMains.Where(p => p.parent_Id == item.bomId && p.isChild == false).ToList();
                        }
                        if (childw == null) childw = new List<BomMain>();
                        childw.AddRange(childbom);
                        childbom = new List<BomMain>();
                    }
                    #endregion 
                    if (childw.Count > 0)
                    {
                        try
                        {
                            var requireno = GetProductNo();
                            #region 单头
                            Production model = new Production()
                            {
                                amount = lamount,
                                backAmount = 0,
                                bomOrderNo = orderno,
                                canfs = true,
                                checkStaff = "",
                                createDate = DateTime.Now,
                                depId = null,
                                deportStaff = "",
                                isclose = false,
                                isover = 0,
                                materialNo = bom.materialNo,
                                orderDetailSn = ordersn,
                                produceNo = requireno,
                                remark = "",
                                staffId = staff,
                                status = 0,
                                valid = true
                            };
                            c.Productions.Add(model);
                            #endregion
                            #region 明细
                            foreach (var item in childw)
                            {
                                ProduceDetail del = new ProduceDetail();
                                del.produceNo = requireno;
                                del.materialNo = item.materialNo;
                                var materialw = c.Materials.FirstOrDefault(p => p.materialNo == item.materialNo);
                                var totalneedw = item.amount * lamount * (1 + bom.loss / 100.0);
                                if (materialw.unit2 != null && materialw.unit2 != "" && materialw.ratio.HasValue)
                                {
                                    totalneedw = Math.Ceiling((item.amount * lamount * (1 + bom.loss / 100.0)) / materialw.ratio.Value);
                                }
                                del.amount = totalneedw;
                                del.outAmount = 0;
                                del.remark = "";
                                c.ProduceDetails.Add(del);
                            }
                            #endregion
                            c.SaveChanges();
                            result = 1;
                        }
                        catch(Exception ex)
                        {
                            result = 0;
                        }
                    }
                    #endregion
                }
                var child = c.BomMains.Where(p => p.parent_Id == bomid).ToList();
                if (child.Count > 0)
                {
                    foreach (var item in child)
                    {
                        result += ProductNoList(false,item.bomId, totalneed * item.amount, staff, orderno, ordersn);
                    }
                }
                return result;
            }
        }


        #endregion

        #region BOM领料
        public List<V_BomOrderDropList> GetBomOrderDropList(int supplieId)
        {
            var list = from od in context.BomOrderDetails
                       join o in context.BomOrders on od.bomOrderNo equals o.bomOrderNo
                       join s in context.Suppliers on o.supplierId equals s.supplierId
                       join b in context.BomMains on od.bomId equals b.bomId
                       join m in context.Materials on b.materialNo equals m.materialNo
                       where o.supplierId == supplieId  && od.outAmount != od.Amount//&& od.hadBom == true
                       orderby od.detailSn ascending
                       select new V_BomOrderDropList
                       {
                           bomId = b.bomId,
                           bomOrderNO = od.bomOrderNo,
                           detailSn = od.detailSn,
                           materialCate = b.materialCate,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = m.materialNo,
                           materialTu = m.tunumber,
                           orderAmount = od.Amount,
                           outAmount = od.outAmount,
                           supplierId = s.supplierId,
                           supplierName = s.supplierName
                       };
            return list.ToList();
        }
        public List<V_BomMaterial> GetBomoutDetail(string outno)
        {
            var list = from od in context.StockOutDetails
                       join o in context.StockOuts on od.stockoutNo equals o.stockoutNo
                       join b in context.BomMains on od.bomId equals b.bomId
                       join m in context.Materials on b.materialNo equals m.materialNo
                       where o.stockoutNo == outno
                       orderby od.detailSn ascending
                       select new V_BomMaterial
                       {
                           bomId = b.bomId,
                           materialCate = b.materialCate,
                           deoptid = od.depotId,
                           materialModel = m.materialModel,
                           materialName = m.materialName,
                           materialNo = m.materialNo,
                           outamount = od.outAmount,
                           outno = od.stockoutNo,
                           outdetailsn = od.detailSn,
                           tunumber = m.tunumber,
                           remark = od.remark,
                           unit = m.unit, version=b.version
                       };
            return list.ToList();
        }
        public string AddStockOut(int staff, int dep, int supplier, int bomordersn, double amount, int outtype, string remark, string deportStaff)
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
                model.bomDetailSn = bomordersn;
                model.outAmount = amount;
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

        public ReturnValue SaveStockOutDetail(string no, List<V_BomMaterial> list, double amount, string remark, string deportStaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.StockOuts.FirstOrDefault(p => p.stockoutNo == no && p.status == 0);
                    if (model == null) return new ReturnValue { status = false, message = "不存在领料出库单" };
                    if (list == null || list.Count < 1) return new ReturnValue { status = false, message = "不存在领料出库单" };
                    foreach (var item in list)
                    {
                        if (item.type == "") continue;

                        if (item.type == "delete")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.outdetailsn);
                            if (d != null) c.StockOutDetails.Remove(d);
                        }
                        else if (item.type == "edit")
                        {
                            var d = c.StockOutDetails.FirstOrDefault(p => p.detailSn == item.outdetailsn);
                            if (d != null)
                            {
                                d.materialNo = item.materialNo;
                                d.remark = item.remark;
                                d.outAmount = item.outamount;
                                d.depotId = item.deoptid;
                                d.bomId = item.bomId;
                                c.Entry(d).State = EntityState.Modified;
                            }
                        }
                        else if (item.type == "add")
                        {
                            StockOutDetail d = new StockOutDetail();
                            d.materialNo = item.materialNo;
                            d.outAmount = item.outamount;
                            d.remark = item.remark;
                            d.stockoutNo = no;
                            d.bomId = item.bomId;
                            d.depotId = item.deoptid;
                            c.StockOutDetails.Add(d);
                        }
                    }
                    #region 申请单
                    if (model.remark != remark || model.deportStaff != deportStaff || model.outAmount != amount)
                    {
                        model.remark = remark;
                        model.deportStaff = deportStaff;
                        model.outAmount = amount;
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
        #endregion


        #region 成本核算
        public List<V_BomCostModel> GetProductBomCostModel(List<V_BomCostModel> list, double amount)
        {
            List<V_BomCostModel> result = new List<V_BomCostModel>();
            foreach (var item in list)
            {
                item.amount = amount * item.amount;
                result.Add(item);
                var children = context.V_BomCostModel.Where(p => p.parent_Id == item.bomId).ToList();
                var rows = GetProductBomCostModel(children, item.amount);
                result.AddRange(rows);
            }
            return result;
        }

        public List<object> GetChildBomCost(List<V_BomCostModel> list, double amount, int index)
        {
            List<object> result = new List<object>();
            foreach (var item in list)
            {
                item.amount = amount * item.amount;
                //result.Add(item);
                var childrenx = context.V_BomCostModel.Where(p => p.parent_Id == item.bomId);

                if (childrenx != null)
                {
                    var children = childrenx.ToList();
                    var rows = GetChildBomCost(children, item.amount, index);
                    result.Add(new { km = 0, id = item.bomId, no = item.materialNo == null ? "child" : item.materialNo, name = item.materialNo == null ? "子BOM" : item.materialName, model = item.materialModel, amount = Math.Round(item.amount, item.xslength.HasValue ? item.xslength.Value : 0), price = Math.Round(item.rootCost, 4).ToString(), cost = item.rootCost * item.amount, useunit = item.unit2 != null && item.unit2 != "" ? item.unit2 : item.unit, unit = item.unit, unit2 = item.unit2, amount2 = (item.unit2 != null && item.unit2 != "" && item.ratio.HasValue ? (Math.Round(item.amount / item.ratio.Value, 4).ToString()) : ""), remark = item.bomremark, index = index++, children = rows });
                }
                else
                    result.Add(new { km = 0, id = item.bomId, no = item.materialNo == null ? "child" : item.materialNo, name = item.materialNo == null ? "子BOM" : item.materialName, model = item.materialModel, amount = Math.Round(item.amount, item.xslength.HasValue ? item.xslength.Value : 0), price = Math.Round(item.rootCost, 4).ToString(), cost = item.rootCost * item.amount, useunit = item.unit2 != null && item.unit2 != "" ? item.unit2 : item.unit, unit = item.unit, unit2 = item.unit2, amount2 = (item.unit2 != null && item.unit2 != "" && item.ratio.HasValue ? (Math.Round(item.amount / item.ratio.Value, 4).ToString()) : ""), remark = item.bomremark, index = index++ });
            }
            return result;

        }


        #endregion

        #region 生成采购申请

        #endregion

        #region 委外
        public string GetDelegateNo()
        {
            var last = (from r in context.DelegateOrders orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.delegateNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeDelegateStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.DelegateOrders.FirstOrDefault(p => p.delegateNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 1)
                        {
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

        #region 领料工单
        public string GetProductNo()
        {
            var last = (from r in context.Productions orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Value.Month != DateTime.Now.Month)
                {
                    return "GM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.produceNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "GM" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "GM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeProductionStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Productions.FirstOrDefault(p => p.produceNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                        }
                        else if (model.status == 1)
                        {
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

        #region  子BOM
        public ReturnValue SaveChildBom(string type, int id, int? parent_Id, int childbom, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {

                    if (type == "add")
                    {
                        #region 添加
                        var child = c.BomMains.FirstOrDefault(p => p.bomId == childbom);
                        if (child == null) return new ReturnValue { status = false, message = "子Bom不存在，添加失败!" };
                        var had = c.BomMains.FirstOrDefault(p => p.parent_Id == parent_Id && p.bomName == child.bomName && p.version == child.version);
                        if (had != null) return new ReturnValue { status = false, message = "Bom已存在，不能重复添加!" };
                        var m = new BomMain();
                        m.materialNo = null;
                        m.isChild = true;
                        m.amount = 1;
                        m.materialCate = "子BOM";
                        m.parent_Id = parent_Id;
                        m.remark = remark;
                        m.loss = 0;
                        m.version = child.version;
                        m.bomName = child.bomName;
                        m.status = 1;
                        m.startDate = DateTime.Now;
                        Guid guid = Guid.NewGuid();
                        m.bomguid = guid;

                        var parent = c.BomMains.FirstOrDefault(p => p.bomId == parent_Id);
                        if (parent != null)
                        {
                            if (parent.parent_Id.HasValue && parent.parent_Id > 0)
                            {
                                m.rootId = parent.rootId;
                            }
                            else m.rootId = parent.bomId;
                        }
                        c.BomMains.Add(m);
                        #endregion
                        c.SaveChanges();
                        #region 复制子级
                        var childlist = c.BomMains.Where(p => p.parent_Id == childbom).ToList();
                        if (childlist != null && childlist.Count > 0)
                        {
                            #region 得到刚添加的id
                            var myp = c.BomMains.FirstOrDefault(p => p.bomguid == guid);
                            foreach (var item in childlist)
                            {
                                SaveChildBomNode(myp.bomId, item.bomId);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else if (type == "edit")
                    {
                        var m = c.BomMains.FirstOrDefault(p => p.bomId == id);
                        if (m == null) return new ReturnValue { status = false, message = "Bom不存在，修改失败" };
                        #region 修改
                        m.remark = remark;
                        c.Entry(m).State = EntityState.Modified; c.SaveChanges();
                        #endregion
                    }
                    else return new ReturnValue { status = false, message = "操作类别错误" };

                    return new ReturnValue { status = true, value = id.ToString() };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }

        }
        /// <summary>
        /// bom复制添加
        /// </summary>
        /// <param name="parent_id">新BOM节点的父级</param>
        /// <param name="childbom">新BOM要复制的对象</param>
        /// <returns></returns>
        public bool SaveChildBomNode(int parent_id,int childbom)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var from = c.BomMains.FirstOrDefault(p => p.bomId == childbom);
                    var m = new BomMain();
                    m.materialNo = from.materialNo;
                    m.amount = from.amount;
                    m.materialCate =from.materialCate;
                    m.parent_Id = parent_id;
                    m.remark = from.remark;
                    m.loss = from.loss;
                    m.version = from.version;
                    m.bomName = from.bomName;
                    m.status = 1;
                    m.isChild = from.isChild;
                    m.startDate = DateTime.Now;
                    Guid guid = Guid.NewGuid();
                    m.bomguid = guid;
                    var parent = c.BomMains.FirstOrDefault(p => p.bomId == parent_id);
                    if (parent != null)
                    {
                        if (parent.parent_Id.HasValue && parent.parent_Id > 0)
                        {
                            m.rootId = parent.rootId;
                        }
                        else m.rootId = parent.bomId;
                    }
                    c.BomMains.Add(m);
                    c.SaveChanges();
                    var child = c.BomMains.Where(p => p.parent_Id == childbom).ToList();
                    if (child!=null&&child.Count>0)
                    {
                        #region 得到刚添加的id
                        var myp = c.BomMains.FirstOrDefault(p => p.bomguid == guid);
                        foreach (var item in child)
                        {
                            SaveChildBomNode(myp.bomId, item.bomId);
                        }
                        #endregion
                    }
                    c.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public List<object> GetBomTreeGrid(List<V_BomMaterial> list, int index)
        {
            List<object> result = new List<object>();
            foreach (var item in list)
            {
                var children = GetBomMaterial().Where(p => p.parent_Id == item.bomId).ToList();
                if (children != null && children.Count > 0)
                {

                    var rows = GetBomTreeGrid(children, index);
                    result.Add(new { canclick = 1, isroot = 0, version = item.version, ischild = item.isChild, cate = item.materialCate, km = 0, id = item.bomId, no = item.materialNo, name = item.isChild ? item.bomName : item.materialName, model = item.materialModel, amount = item.isChild ? "" : Math.Round(item.amount, item.xslength).ToString(), useunit = item.unit2 != null && item.unit2 != "" ? item.unit2 : item.unit, unit = item.unit, unit2 = item.unit2, amount2 = item.isChild ? "" : (item.unit2 != null && item.unit2 != "" && item.ratio.HasValue ? (Math.Round(item.amount / item.ratio.Value, 4).ToString()) : ""), remark = item.remark, index = index++, children = rows });
                }
                else
                    result.Add(new { canclick = 1, isroot = 0, version = item.version, ischild = item.isChild, cate = item.materialCate, km = 0, id = item.bomId, no = item.materialNo, name = item.isChild ? item.bomName : item.materialName, model = item.materialModel, amount = item.isChild ? "" : Math.Round(item.amount, item.xslength).ToString(), useunit = item.unit2 != null && item.unit2 != "" ? item.unit2 : item.unit, unit = item.unit, unit2 = item.unit2, amount2 = item.isChild ? "" : (item.unit2 != null && item.unit2 != "" && item.ratio.HasValue ? (Math.Round(item.amount / item.ratio.Value, 4).ToString()) : ""), remark = item.remark, index = index++ });
            }
            return result;
        }

        #endregion

        #region 委外发货
        public string GetDelegateSendNo()
        {
            var last = (from r in context.DelegateSends orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "DS" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.sendNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "DS" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "DS" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeDelegateSendStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.DelegateSends.FirstOrDefault(p => p.sendNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            #region 审核通过
                            #region 委外工单增加已发数量
                            var dnos = ServiceDB.Instance.QueryModelList<string>("select distinct delegateNo  from DelegateSendDetail where sendNo='" + model.sendNo + "'");
                            foreach (var item in dnos)
                            {
                                var dlone = c.DelegateOrders.FirstOrDefault(p => p.delegateNo == item);
                                var dsone = c.DelegateSendDetails.FirstOrDefault(p => p.delegateNo == item && p.sendNo == model.sendNo);
                                if (dlone != null)//已发数量增加
                                {
                                    dlone.giveAmount += dsone.productAmount;
                                }
                                c.Entry(dlone).State = EntityState.Modified;
                            }
                            #endregion
                            #region 扣库存
                            var details = c.DelegateSendDetails.Where(p => p.sendNo == model.sendNo);
                            foreach (var item in details)
                            {

                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == item.materialNo);
                                if (depot == null || (decimal)depot.depotAmount < item.realAmount)
                                {
                                    return new ReturnValue { message = "审核失败，" + item.materialNo + " 库存不足", status = false };
                                }
                                depot.depotAmount -= (double)item.realAmount;
                                c.Entry(depot).State = EntityState.Modified;

                                var orderdetail = c.DelegateOrderDetails.FirstOrDefault(p => p.detailSn == item.delegateDetailSn);
                                if (orderdetail != null)
                                {
                                    orderdetail.sendAmount += item.realAmount;
                                    c.Entry(orderdetail).State = EntityState.Modified;
                                }
                            }
                            #endregion
                            #endregion
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;

                            #region 返审
                            #region 委外工单增加已发数量
                            var dnos = ServiceDB.Instance.QueryModelList<string>("select distinct delegateNo  from DelegateSendDetail where sendNo='" + model.sendNo + "'");
                            foreach (var item in dnos)
                            {
                                var dlone = c.DelegateOrders.FirstOrDefault(p => p.delegateNo == item);
                                var dsone = c.DelegateSendDetails.FirstOrDefault(p => p.delegateNo == item && p.sendNo == model.sendNo);
                                if (dlone != null)//已发数量增加
                                {
                                    dlone.giveAmount -= dsone.productAmount;
                                }
                                c.Entry(dlone).State = EntityState.Modified;
                            }
                            #endregion
                            #region 库存回溯
                            var details = c.DelegateSendDetails.Where(p => p.sendNo == model.sendNo);
                            foreach (var item in details)
                            {

                                var depot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == item.materialNo);                                
                                depot.depotAmount += (double)item.realAmount;
                                var orderdetail = c.DelegateOrderDetails.FirstOrDefault(p => p.detailSn == item.delegateDetailSn);
                                if (orderdetail != null) {
                                    orderdetail.sendAmount -= item.realAmount;
                                    c.Entry(orderdetail).State = EntityState.Modified;
                                }
                                c.Entry(depot).State = EntityState.Modified;
                            }
                            #endregion
                            #endregion
                        }
                        else if (model.status == 4)//撤消作废
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
        #region 委外收货
        public string GetDelegateBackNo()
        {
            var last = (from r in context.DelegateBacks orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "WB" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.sendNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "WB" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "WB" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeDelegateBackStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.DelegateBacks.FirstOrDefault(p => p.backNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            #region 审核通过 委外工单增加收货数量
                            var dnos = ServiceDB.Instance.QueryModelList<V_DelegateBackDetail>("select *  from V_DelegateBackDetail where backNo='" + model.backNo + "'");
                            foreach (var item in dnos)
                            {
                                if (item.isProduct)
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("update DelegateSendDetail set backProduct+=" + item.backAmount + " where sendNo='" + model.sendNo + "'");
                                }
                                #region 回仓库
                                var haddepot = ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='"+item.materialNo+"'");
                                if (haddepot == null)
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("insert into DepotDetail values(1,'" + item.materialNo + "'," + item.backAmount + ",0,0,'')");
                                }
                                else
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("update DepotDetail set depotAmount+=" + item.backAmount + " where depotId=1 and materialno='" + item.materialNo + "'");
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;

                            #region 返审 委外工单增加收货数量
                            var dnos = ServiceDB.Instance.QueryModelList<V_DelegateBackDetail>("select *  from V_DelegateBackDetail where backNo='" + model.backNo + "'");
                            foreach (var item in dnos)
                            {
                                if (item.isProduct)
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("update DelegateSendDetail set backProduct-=" + item.backAmount + " where sendNo='" + model.sendNo + "'");
                                }
                                #region 回仓库
                                var haddepot = ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='" + item.materialNo + "' and depotAmount>="+item.backAmount);
                                if (haddepot == null)
                                {
                                    return new ReturnValue { message = item.materialNo+item.materialName+item.materialModel +"库存不足或不存在，返审失败", status = false };
                                }
                                else
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("update DepotDetail set depotAmount-=" + item.backAmount + " where depotId=1 and materialno='" + item.materialNo + "'");
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else if (model.status == 4)//撤消作废
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
        #region BOM领料
        public string GetProductPullNo()
        {
            var last = (from r in context.ProductPulls orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "PM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.pullNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "PM" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "PM" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeProductPullStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.ProductPulls.FirstOrDefault(p => p.pullNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            #region 审核通过 委外工单增加收货数量
                            var dnos = ServiceDB.Instance.QueryOneModel<Production>("select *  from Production where produceNo='" + model.produceNo + "'");
                            if (dnos != null)
                            {
                                dnos.pullAmount += model.makeAmount;
                                dnos.canfs = false;
                                c.Entry(dnos).State = EntityState.Modified;
                                #region 原料出库
                                var details = c.ProductPullDetails.Where(p => p.pullNo == model.pullNo).ToList();
                                foreach (var item in details)
                                {
                                    var haddepot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == item.materialNo);// ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='" + item.materialNo + "'");
                                    if (haddepot == null||haddepot.depotAmount<(double)item.realAmount)
                                    {
                                        return new ReturnValue { message = "原材料物料"+item.materialNo+"库存不存在或库存不足，审核失败" + model.produceNo, status = false };
                                    }
                                    haddepot.depotAmount -= (double)item.realAmount;

                                    var prodetail = c.ProduceDetails.FirstOrDefault(p => p.detailSn == item.semiId);
                                    if (prodetail != null) {
                                        prodetail.outAmount += (double)item.realAmount;
                                        c.Entry(prodetail).State = EntityState.Modified;
                                    }

                                    c.Entry(haddepot).State = EntityState.Modified;
                                }
                                #endregion
                            }
                            else
                            {
                                return new ReturnValue { message = "未生成领料工单" + model.produceNo, status = false };
                            }                            
                            #endregion
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;

                            #region 审核通过 委外工单增加收货数量
                            var dnos = ServiceDB.Instance.QueryOneModel<Production>("select *  from Production where produceNo='" + model.produceNo + "'");
                            if (dnos != null)
                            {
                                dnos.pullAmount -= model.makeAmount;
                                if (dnos.pullAmount == 0)
                                    dnos.canfs = true;
                                c.Entry(dnos).State = EntityState.Modified;
                                #region 原料回库
                                var details = c.ProductPullDetails.Where(p => p.pullNo == model.pullNo).ToList();
                                foreach (var item in details)
                                {
                                    var haddepot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == item.materialNo);// ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='" + item.materialNo + "'");
                                    if (haddepot == null)
                                    {
                                        DepotDetail dd = new DepotDetail();
                                        dd.depotAmount = (double)item.realAmount;
                                        dd.depotCost = 0;
                                        dd.depotId = 1;
                                        dd.depotSafe = 0;
                                        dd.materialNo = item.materialNo;
                                        c.DepotDetails.Add(dd);
                                    }
                                    else
                                    {
                                        haddepot.depotAmount += (double)item.realAmount;
                                        c.Entry(haddepot).State = EntityState.Modified;
                                    }
                                    var prodetail = c.ProduceDetails.FirstOrDefault(p => p.detailSn == item.semiId);
                                    if (prodetail != null)
                                    {
                                        prodetail.outAmount -= (double)item.realAmount;
                                        c.Entry(prodetail).State = EntityState.Modified;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                return new ReturnValue { message = "未生成领料工单" + model.produceNo, status = false };
                            }
                            #endregion
                        }
                        else if (model.status == 4)//撤消作废
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


        #region BOM领料
        public string GetProductGiveNo()
        {
            var last = (from r in context.ProductGives orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "PG" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.giveNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "PG" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "PG" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        public ReturnValue ChangeProductGiveStatus(string no, int status, string checkstaff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.ProductGives.FirstOrDefault(p => p.giveNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.isover = 0;
                        model.checkStaff = checkstaff;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkStaff = checkstaff;
                            model.checkDate = DateTime.Now;
                            #region 审核通过 领料单、生产领料单增加交货数量
                            var pull = ServiceDB.Instance.QueryOneModel<ProductPull>("select *  from ProductPull where pullNo='" + model.pullNo + "'");
                            if (pull == null) { return new ReturnValue { message = "不存在生产领料单" + model.pullNo, status = false }; }
                            var dnos = ServiceDB.Instance.QueryOneModel<Production>("select *  from Production where produceNo='" + pull.produceNo + "'");
                            if (dnos != null)
                            {
                                pull.giveAmount += model.giveAmount;
                                pull.canfs = false;
                                dnos.backAmount += (double)model.giveAmount;
                                dnos.canfs = false;
                                c.Entry(dnos).State = EntityState.Modified;
                                c.Entry(pull).State = EntityState.Modified;                               
                                #region 产品入库
                                var product = c.DepotDetails.FirstOrDefault(p => p.depotId == 2 && p.materialNo == dnos.materialNo);
                                if (product == null)
                                {
                                    DepotDetail dd = new DepotDetail();
                                    dd.materialNo = dnos.materialNo;
                                    dd.depotAmount = (double)model.giveAmount;
                                    dd.depotCost = 0;
                                    dd.depotId = 2;
                                    dd.depotSafe = 0;
                                    c.DepotDetails.Add(dd);
                                }
                                else {
                                    product.depotAmount += (double)model.giveAmount;
                                    c.Entry(product).State = EntityState.Modified;
                                }
                                #endregion
                                #region 原材料入库
                                var details = c.ProductGiveDetails.Where(p => p.giveNo == model.giveNo).ToList();
                                foreach (var item in details)
                                {
                                    var m = c.ProductPullDetails.FirstOrDefault(p => p.pullSn == item.pullDetailSn);
                                    var haddepot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == m.materialNo);// ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='" + item.materialNo + "'");
                                    if (haddepot == null)
                                    {
                                        DepotDetail dd = new DepotDetail();
                                        dd.materialNo = dnos.materialNo;
                                        dd.depotAmount = (double)item.giveAmount;
                                        dd.depotCost = 0;
                                        dd.depotId = 1;
                                        dd.depotSafe = 0;
                                        c.DepotDetails.Add(dd);
                                    }
                                    else
                                    {
                                        haddepot.depotAmount += (double)item.giveAmount;
                                        c.Entry(haddepot).State = EntityState.Modified;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                return new ReturnValue { message = "未生成生产交货工单" + model.giveNo, status = false };
                            }
                            #endregion
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.isover = 0;
                            model.checkStaff = checkstaff;

                            #region 审核通过 领料单、生产领料单增加交货数量
                            var pull = ServiceDB.Instance.QueryOneModel<ProductPull>("select *  from ProductPull where pullNo='" + model.pullNo + "'");
                            if (pull == null) { return new ReturnValue { message = "不存在生产领料单" + model.pullNo, status = false }; }
                            var dnos = ServiceDB.Instance.QueryOneModel<Production>("select *  from Production where produceNo='" + pull.produceNo + "'");
                            if (dnos != null)
                            {
                                pull.giveAmount -= model.giveAmount;
                                if (pull.giveAmount == 0) pull.canfs = true;
                                dnos.backAmount -= (double)model.giveAmount;
                                dnos.canfs = false;
                                c.Entry(dnos).State = EntityState.Modified;
                                c.Entry(pull).State = EntityState.Modified;
                                #region 产品还库
                                var product = c.DepotDetails.FirstOrDefault(p => p.depotId == 2 && p.materialNo == dnos.materialNo);
                                if (product == null || product.depotAmount < (double)model.giveAmount)
                                {
                                    return new ReturnValue { message = dnos.materialNo+"货品在成品仓库库存不足", status = false };
                                }
                                else
                                {
                                    product.depotAmount -= (double)model.giveAmount;
                                    c.Entry(product).State = EntityState.Modified;
                                }
                                #endregion
                                #region 原材料出库
                                var details = c.ProductGiveDetails.Where(p => p.giveNo == model.giveNo).ToList();
                                foreach (var item in details)
                                {
                                    var m = c.ProductPullDetails.FirstOrDefault(p => p.pullSn == item.pullDetailSn);
                                    var haddepot = c.DepotDetails.FirstOrDefault(p => p.depotId == 1 && p.materialNo == m.materialNo);// ServiceDB.Instance.QueryOneModel<DepotDetail>("select * from DepotDetail where depotId=1 and materialno='" + item.materialNo + "'");
                                    if (haddepot == null || haddepot.depotAmount < (double)item.giveAmount)
                                    {
                                        return new ReturnValue { message = m.materialNo + "原料在原材料仓库库存不足", status = false };
                                    }
                                    else
                                    {
                                        haddepot.depotAmount -= (double)item.giveAmount;
                                        c.Entry(haddepot).State = EntityState.Modified;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                return new ReturnValue { message = "未生成生产交货工单" + model.giveNo, status = false };
                            }
                            #endregion
                        }
                        else if (model.status == 4)//撤消作废
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


    }
}
