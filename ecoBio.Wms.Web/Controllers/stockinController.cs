using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Enterprise.Invoicing.ViewModel;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Entities;
namespace Enterprise.Invoicing.Web.Controllers
{
    public class stockinController : BaseController
    {
        private StockInService stockinService; 
        private PurchaseService purchaseService;

        public stockinController(IStockInRepository _stockinrepository, IPurchaseRepository _purchaseRepository)
        {
            stockinService = new StockInService(_stockinrepository); 
            purchaseService = new PurchaseService(_purchaseRepository);
        }

        #region 入库单列表
        public ActionResult inlist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
           
            return View(data);
        }
        [AjaxAction(ForAction = "inlist", ForController = "stockin")]
        public ActionResult inlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockinService.StockInList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return View(data);
        }

        [AjaxAction(ForAction = "inlist", ForController = "stockin")]
        public ActionResult inlistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var one = stockinService.StockInList(no).FirstOrDefault(); 
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            var list = stockinService.StockInDetailList(no).ToList();
            data.no = no;
            data.one = one;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "inlist", ForController = "stockin")]
        public ActionResult indelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockinService.DeleteStockIn(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        #region 采购入库
        public ActionResult inorder()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new StockModel();
                var list = new List<StockDetailModel>();
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetStockInNo(); 
                #endregion
            }
            else if (type == "edit")
            {
                model = stockinService.StockInList(no).FirstOrDefault();
                list = purchaseService.GetStockHadDetailByPurchase(no);
            }
            var ddl = purchaseService.QuerySupplier(0, 1).ToList();
            data.ddl = ddl; data.no = no;
            data.one = model;
            data.type = type;
            SessionHelper.SetSession("WE1" + no + Masterpage.CurrUser.staffid, list);
            return View(data);
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult needinorder(int? page, int? pagesize, string no, int supplier, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = purchaseService.GetStorkInHadOrder(no);
            var all = purchaseService.GetNeedStockInOrder(supplier,query);
            #region 合并
            if (all != null && had != null)
            {
                foreach (var item in all)
                {
                    var h = had.FirstOrDefault(p => p.text == item.text);
                    if (h == null) had.Add(item);
                }
            }
            #endregion

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = had.ToPagedList(_page, _pagesize);
            data.no = no;
            data.query = query;
            data.supplier = supplier;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult needintab(string order, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = purchaseService.GetStockHadDetailByPurchase(no,order);
            var orderdetail = purchaseService.GetPurchaseStockDetail(order);
            if (had != null && had.Count > 0)
            {
                foreach (var item in orderdetail)
                {
                    var h = had.FirstOrDefault(p => p.purchaseDetailSn == item.purchaseDetailSn);
                    if (h != null)
                    {
                        item.amount = h.amount;
                        item.depotId = h.depotId;
                        item.depotName = h.depotName;
                        item.detailSn = h.detailSn;
                        item.stockNo = h.stockNo; item.remark = h.remark;                 
                    }
                }
            } 
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            data.no = no;
            data.order = order;
            data.list = orderdetail;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult queryorder(string key, int? supplier)
        {
            List<KeyValue> all = new List<KeyValue>();
            if (supplier.HasValue) all = purchaseService.GetNeedStockInOrder(supplier.Value, key);
            var r = all.Select(x => new { value = x.text, key = x.text });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult needintemp(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockDetailModel>();
            list = (List<StockDetailModel>)SessionHelper.GetSession("WE1" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult saveinordertemp(string order, string no, string details, string counts, string depotids, string depotnames, string models, string remarks)
        {
            var list = new List<StockDetailModel>();
            list = (List<StockDetailModel>)SessionHelper.GetSession("WE1" + no + Masterpage.CurrUser.staffid);
            if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
            if (depotids.EndsWith(",")) depotids = depotids.Substring(0, depotids.Length - 1);
            if (depotnames.EndsWith(",")) depotnames = depotnames.Substring(0, depotnames.Length - 1);
            if (models.EndsWith(",")) models = models.Substring(0, models.Length - 1);
            //if (remarks.EndsWith(",")) remarks = remarks.Substring(0, remarks.Length - 1);
            string[] ds = details.Split(',');
            string[] cs = counts.Split(',');
            string[] ps = depotids.Split(',');
            string[] ms = models.Split(',');
            string[] dn = depotnames.Split(',');
            string[] rs = remarks.Split(',');

            for (int i = 0; i < ds.Length; i++)
            {
                double fc = Convert.ToDouble(cs[i]);
                int pc = Convert.ToInt32(ps[i]);
                int rdsn = Convert.ToInt32(ds[i]);
                var had = list.FirstOrDefault(p => p.purchaseDetailSn == rdsn && p.materialNo == ms[i]);
                if (had != null)
                {
                    if (fc == 0)
                    {
                        if (had.detailSn == 0) list.Remove(had);
                        else had.type = "delete";
                    }
                    else
                    {
                        had.amount = fc;
                        had.depotId = pc; had.depotName = dn[i]; had.remark = rs[i];
                        if (had.type != "add") had.type = "edit";
                    }
                }
                else
                {
                    if (fc == 0) continue;
                    //var m = purchaseService.QueryOneMaterial(ms[i]);
                    var r = purchaseService.PurchaseOneDetailList(rdsn);
                    list.Add(new StockDetailModel
                    {
                        depotId = pc,
                        detailSn = 0,
                        amount = fc,
                        materialModel = r.materialModel,
                        purchaseDetailSn = rdsn,
                        materialName = r.materialName, 
                        materialNo = r.materialNo,
                        materialUnit = r.materialUnit,depotName = dn[i],
                        stockNo = no,  orderAmout=r.poAmount, purchaseNo=r.purchaseNo, remainAmout=r.poRemain,
                        type = "add", remark=rs[i]

                    });
                }
            }
            SessionHelper.SetSession("WE1" + no + Masterpage.CurrUser.staffid, list);
            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult deleteneedintemp(string no, int sn, string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockDetailModel>();
            list = (List<StockDetailModel>)SessionHelper.GetSession("WE1" + no + Masterpage.CurrUser.staffid);

            var had = list.FirstOrDefault(p => p.purchaseDetailSn == sn && p.materialModel == m);
            if (had != null)
            {
                if (had.detailSn == 0) list.Remove(had);
                else had.type = "delete";
                SessionHelper.SetSession("WE1" + no + Masterpage.CurrUser.staffid, list);
            }
            return Content(list.Count.ToString());
        }

        [AjaxAction(ForAction = "inorder", ForController = "stockin")]
        public ActionResult savesneedin(string no)
        {
            string type = WebRequest.GetString("type", true);
            string supplier = WebRequest.GetString("supplier", true);
            int i_supplier = Convert.ToInt32(supplier);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            var list = new List<StockDetailModel>();
            list = (List<StockDetailModel>)SessionHelper.GetSession("WE1" + no + Masterpage.CurrUser.staffid);
            if (list.Count == 0)
            {
                r = new ReturnValue { status = false, message = "未提交任何订单明细" };
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockinService.AddStockIn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, i_supplier, 1, remark, deport);
                    if (backno == "")
                    {
                        r.message = "入库单生成失败";
                    }
                    else
                    {
                        r = stockinService.SaveStockInDetail(backno, list, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockinService.SaveStockInDetail(no, list, 0, remark, deport);

                }
            }
            SessionHelper.Del("WE1" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 产品入库

        public ActionResult inproduct()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new StockModel();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetStockInNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockinService.StockInList(no).FirstOrDefault();
            }
            var mc = stockinService.GetMaterialCategory().Select(x=>new SelectListItem{ Text=x.text,Value=x.value}).ToList();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.mc = mc;
            data.depots = depots;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "inproduct", ForController = "stockin")]
        public ActionResult saveinproducttemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            string m = WebRequest.GetString("m", true);
            string mc = WebRequest.GetString("mc", true);
            int int_depot = WebRequest.GetFormInt("depot", 0);
            string depotname = WebRequest.GetString("depotname", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string remark = WebRequest.GetString("remark", true);
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE2" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<StockDetailModel>();
                temp.Add(new StockDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    materialUnit = "",
                    materialName = name,
                    materialNo = m,
                    materialCategory = mc,
                    amount = int_count,
                    stockNo = no,
                    depotId = int_depot,
                    depotName = depotname, remark=remark,
                    type = "add"
                });
            }
            else
            {
                bool add = false;
                StockDetailModel ht = new StockDetailModel();
                if (detail > 0)
                {
                    ht = temp.FirstOrDefault(p => p.detailSn == detail);
                    ht.type = "edit";
                }
                else
                {
                    ht = temp.FirstOrDefault(p => p.materialNo == m);
                    if (ht == null)
                    {
                        ht = new StockDetailModel();
                        add = true; 
                        ht.type = "add";
                    }
                }
                ht.remark = remark;
                ht.amount = int_count;
                ht.materialCategory = mc;
                ht.materialModel = model;
                ht.materialName = name;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WE2" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "inproduct", ForController = "stockin")]
        public ActionResult deleteinproduct()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE2" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WE2" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "inproduct", ForController = "stockin")]
        public ActionResult saveinproduct()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE2" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在入库明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockinService.AddStockIn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, 2, remark, deport);
                    if (backno == "")
                    {
                        r.message = "入库单生成失败";
                    }
                    else
                    {
                        r = stockinService.SaveStockInDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockinService.SaveStockInDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WE2" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "inproduct", ForController = "stockin")]
        public ActionResult inproductdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockinService.StockInDetailList(no).ToList();
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE2" + no + Masterpage.CurrUser.staffid);
            List<StockDetailModel> newlist = new List<StockDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.detailSn == item.detailSn);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new StockDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            stockNo = p.stockNo,
                            amount = p.amount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            materialCategory = p.materialCategory,
                            remark = p.remark,
                            materialUnit = p.materialUnit,
                            depotName = p.depotName, depotId=p.depotId, purchaseDetailSn=p.purchaseDetailSn, purchaseNo=p.purchaseNo,
                            type = ""
                        }).ToList());
                }
            }
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("WE2" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }

   
        #endregion

        #region 盘点入库

        public ActionResult incheck()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new StockModel();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetStockInNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockinService.StockInList(no).FirstOrDefault();
            }
            var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.mc = mc;
            data.depots = depots;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "incheck", ForController = "stockin")]
        public ActionResult saveinchecktemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            int rad = WebRequest.GetFormInt("rad", 0);
            string m = WebRequest.GetString("m", true);
            string mc = WebRequest.GetString("mc", true);
            int int_depot = WebRequest.GetFormInt("depot", 0);
            string depotname = WebRequest.GetString("depotname", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            double int_count = Convert.ToDouble(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string remark = WebRequest.GetString("remark", true);
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE3" + no + Masterpage.CurrUser.staffid);
            if (rad == 0) int_count = 0 - int_count;
            if (temp == null || temp.Count == 0)
            {
                temp = new List<StockDetailModel>();
                temp.Add(new StockDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    materialUnit = "",
                    materialName = name,
                    materialNo = m,
                    materialCategory = mc,
                    amount = int_count,
                    stockNo = no,
                    depotId = int_depot,
                    depotName = depotname, remark=remark,
                    type = "add"
                });
            }
            else
            {
                bool add = false;
                StockDetailModel ht = new StockDetailModel();
                if (detail > 0)
                {
                    ht = temp.FirstOrDefault(p => p.detailSn == detail);
                    ht.type = "edit";
                }
                else
                {
                    ht = temp.FirstOrDefault(p => p.materialNo == m);
                    if (ht == null)
                    {
                        ht = new StockDetailModel();
                        add = true;
                        ht.type = "add";
                    }
                }
                ht.amount = int_count;
                ht.materialCategory = mc;
                ht.materialModel = model;
                ht.materialName = name;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname;
                ht.remark = remark;
                if (add) temp.Add(ht);
            }
            SessionHelper.SetSession("WE3" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "incheck", ForController = "stockin")]
        public ActionResult deleteincheck()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE3" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WE3" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "incheck", ForController = "stockin")]
        public ActionResult saveincheck()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE3" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在入库明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockinService.AddStockIn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, 3, remark, deport);
                    if (backno == "")
                    {
                        r.message = "入库单生成失败";
                    }
                    else
                    {
                        r = stockinService.SaveStockInDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockinService.SaveStockInDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WE3" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "incheck", ForController = "stockin")]
        public ActionResult incheckdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockinService.StockInDetailList(no).ToList();
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE3" + no + Masterpage.CurrUser.staffid);
            List<StockDetailModel> newlist = new List<StockDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.detailSn == item.detailSn);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new StockDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            stockNo = p.stockNo,
                            amount = p.amount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            materialCategory = p.materialCategory,
                            remark = p.remark,
                            materialUnit = p.materialUnit,
                            depotName = p.depotName,
                            depotId = p.depotId,
                            purchaseDetailSn = p.purchaseDetailSn,
                            purchaseNo = p.purchaseNo,
                            type = ""
                        }).ToList());
                }
            }
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("WE3" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }


        #endregion

        #region 其他入库

        public ActionResult inother()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new StockModel();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetStockInNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockinService.StockInList(no).FirstOrDefault();
            }
            var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.mc = mc;
            data.depots = depots;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "inother", ForController = "stockin")]
        public ActionResult saveinothertemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            string m = WebRequest.GetString("m", true);
            string mc = WebRequest.GetString("mc", true);
            int int_depot = WebRequest.GetFormInt("depot", 0);
            string depotname = WebRequest.GetString("depotname", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string remark = WebRequest.GetString("remark", true);
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE8" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<StockDetailModel>();
                temp.Add(new StockDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    materialUnit = "",
                    materialName = name,
                    materialNo = m,
                    materialCategory = mc,
                    amount = int_count,
                    stockNo = no,
                    depotId = int_depot,
                    depotName = depotname,remark=remark,
                    type = "add"
                });
            }
            else
            {
                bool add = false;
                StockDetailModel ht = new StockDetailModel();
                if (detail > 0)
                {
                    ht = temp.FirstOrDefault(p => p.detailSn == detail);
                    ht.type = "edit";
                }
                else
                {
                    ht = temp.FirstOrDefault(p => p.materialNo == m);
                    if (ht == null)
                    {
                        ht = new StockDetailModel();
                        add = true;
                        ht.type = "add";
                    }
                }
                ht.amount = int_count;
                ht.materialCategory = mc;
                ht.remark = remark;
                ht.materialModel = model;
                ht.materialName = name;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WE8" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "inother", ForController = "stockin")]
        public ActionResult deleteinother()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE8" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WE8" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "inother", ForController = "stockin")]
        public ActionResult saveinother()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE8" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在入库明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockinService.AddStockIn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, 8, remark, deport);
                    if (backno == "")
                    {
                        r.message = "入库单生成失败";
                    }
                    else
                    {
                        r = stockinService.SaveStockInDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockinService.SaveStockInDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WE8" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "inother", ForController = "stockin")]
        public ActionResult inotherdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockinService.StockInDetailList(no).ToList();
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WE8" + no + Masterpage.CurrUser.staffid);
            List<StockDetailModel> newlist = new List<StockDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.detailSn == item.detailSn);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new StockDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            stockNo = p.stockNo,
                            amount = p.amount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            materialCategory = p.materialCategory,
                            remark = p.remark,
                            materialUnit = p.materialUnit,
                            depotName = p.depotName,
                            depotId = p.depotId,
                            purchaseDetailSn = p.purchaseDetailSn,
                            purchaseNo = p.purchaseNo,
                            type = ""
                        }).ToList());
                }
            }
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("WE8" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }


        #endregion

        #region 采购退单
        #region 退单列表
        public ActionResult returnlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
          
            return View(data);
        }
        [AjaxAction(ForAction = "returnlist", ForController = "stockin")]
        public ActionResult returnlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockinService.ReturnModelList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnlist", ForController = "stockin")]
        public ActionResult returnlistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var one = stockinService.ReturnModelList(no).FirstOrDefault();
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            var list = stockinService.ReturnDetailList(no).ToList();
            data.no = no;
            data.one = one;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnlist", ForController = "stockin")]
        public ActionResult returndelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockinService.DeleteReturn(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        public ActionResult returnone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new ReturnModel();
            var list = new List<ReturnDetailModel>();
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetReturnNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockinService.ReturnModelList(no).FirstOrDefault();
                list = stockinService.ReturnDetailList(no).ToList();
            }
            var ddl = purchaseService.QuerySupplier(0, 1).ToList();
            data.ddl = ddl; data.no = no;
            data.one = model;
            data.type = type;
            SessionHelper.SetSession("PR" + no + Masterpage.CurrUser.staffid, list);
            return View(data);
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult needreturnone(int? page, int? pagesize, string no, int supplier, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockinService.GetHadReturnOrders(no);
            var all = stockinService.GetCanReturnOrders(supplier, query);
            #region 合并
            if (all != null && had != null)
            {
                foreach (var item in all)
                {
                    var h = had.FirstOrDefault(p => p.text == item.text);
                    if (h == null) had.Add(item);
                }
            }
            #endregion

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = had.ToPagedList(_page, _pagesize);
            data.no = no;
            data.query = query;
            data.supplier = supplier;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult canreturntab(string order, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockinService.ReturnDetailList(no, order);
            var orderdetail = stockinService.ReturnDetailListByOrder(order).ToList();
            if (had != null && had.Count > 0)
            {
                foreach (var item in orderdetail)
                {
                    var h = had.FirstOrDefault(p => p.purchaseDetailSn == item.purchaseDetailSn);
                    if (h != null)
                    {
                        item.returnAmount = h.returnAmount;
                        item.depotId = h.depotId;
                        item.depotName = h.depotName;
                        item.returnSn = h.returnSn;
                        item.returnNo = h.returnNo;
                        item.remark = h.remark;
                    }
                }
            }
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            data.no = no;
            data.order = order;
            data.list = orderdetail;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult queryreturn(string key, int? supplier)
        {
            List<KeyValue> all = new List<KeyValue>();
            if (supplier.HasValue) all = purchaseService.GetNeedStockInOrder(supplier.Value, key);
            var r = all.Select(x => new { value = x.text, key = x.text });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult needreturntemp(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<ReturnDetailModel>();
            list = (List<ReturnDetailModel>)SessionHelper.GetSession("PR" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult savereturnonetemp(string no, string details, string counts, string models, string remarks)
        {
            var list = new List<ReturnDetailModel>();
            list = (List<ReturnDetailModel>)SessionHelper.GetSession("PR" + no + Masterpage.CurrUser.staffid);
            if (list == null) list = new List<ReturnDetailModel>();
            if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
           // if (depotids.EndsWith(",")) depotids = depotids.Substring(0, depotids.Length - 1);
           // if (depotnames.EndsWith(",")) depotnames = depotnames.Substring(0, depotnames.Length - 1);
            if (models.EndsWith(",")) models = models.Substring(0, models.Length - 1);
            //if (remarks.EndsWith(",")) remarks = remarks.Substring(0, remarks.Length - 1);
            string[] ds = details.Split(',');
            string[] cs = counts.Split(',');
         //   string[] ps = depotids.Split(',');
            string[] ms = models.Split(',');
            string[] rs = remarks.Split(',');
         //   string[] dn = depotnames.Split(',');

            for (int i = 0; i < ds.Length; i++)
            {
                double fc = Convert.ToDouble(cs[i]);
                //int pc = Convert.ToInt32(ps[i]);
                int rdsn = Convert.ToInt32(ds[i]);
                var had = list.FirstOrDefault(p => p.stockinDetailSn == rdsn && p.materialNo == ms[i]);
                if (had != null)
                {
                    if (fc == 0)
                    {
                        if (had.returnSn == 0) list.Remove(had);
                        else had.type = "delete";
                    }
                    else
                    {
                        had.returnAmount = fc;
                        had.remark = rs[i];
                        //had.depotId = pc; had.depotName = dn[i];
                        if (had.type != "add") had.type = "edit";
                    }
                }
                else
                {
                    if (fc == 0) continue;
                    //var m = purchaseService.QueryOneMaterial(ms[i]);
                    var r = stockinService.StockInDetailOne(rdsn);
                    list.Add(new ReturnDetailModel
                    {
                        returnSn=0,
                        returnAmount=fc,
                        materialModel = r.materialModel,
                        purchaseDetailSn = r.purchaseDetailSn,
                        materialName = r.materialName,
                        materialNo = r.materialNo,  materialTu=r.materialTu,
                        materialUnit = r.materialUnit,
                        depotName =  r.depotName,
                        depotId=r.depotId,
                        stockinNo = r.stockNo,
                        remark=rs[i],
                        stockinDetailSn = rdsn,
                        inAmount = r.amount,
                        returnNo = no,
                        materialCategory = r.materialCategory,
                        orderAmoutn = r.orderAmout,
                        orderPrice = r.price,
                        purchaseNo = r.purchaseNo,  
                        type = "add"

                    });
                }
            }
            SessionHelper.SetSession("PR" + no + Masterpage.CurrUser.staffid, list);
            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult deletereturntemp(string no, int sn, string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<ReturnDetailModel>();
            list = (List<ReturnDetailModel>)SessionHelper.GetSession("PR" + no + Masterpage.CurrUser.staffid);

            var had = list.FirstOrDefault(p => p.purchaseDetailSn == sn && p.materialModel == m);
            if (had != null)
            {
                if (had.returnSn == 0) list.Remove(had);
                else had.type = "delete";
                SessionHelper.SetSession("PR" + no + Masterpage.CurrUser.staffid, list);
            }
            return Content(list.Count.ToString());
        }

        [AjaxAction(ForAction = "returnone", ForController = "stockin")]
        public ActionResult savereturn(string no)
        {
            string type = WebRequest.GetString("type", true);
            string supplier = WebRequest.GetString("supplier", true);
            int i_supplier = Convert.ToInt32(supplier);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            var list = new List<ReturnDetailModel>();
            list = (List<ReturnDetailModel>)SessionHelper.GetSession("PR" + no + Masterpage.CurrUser.staffid);
            if (list.Count == 0)
            {
                r = new ReturnValue { status = false, message = "未提交任何退单明细" };
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockinService.AddReturn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, i_supplier, remark, deport);
                    if (backno == "")
                    {
                        r.message = "采购退单生成失败";
                    }
                    else
                    {
                        r = stockinService.SaveReturnDetail(backno, list, i_supplier, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockinService.SaveReturnDetail(no, list, i_supplier, remark, deport);

                }
            }
            SessionHelper.Del("PR" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region 半成品盘点
        #region 入库单列表
        public ActionResult semilist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            return View(data);
        }
        [AjaxAction(ForAction = "semilist", ForController = "stockin")]
        public ActionResult semilistpart(int? page, int? pagesize, string where)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string txtkey = where;
            data.txtkey = txtkey;
            if (where != null && where != "") where = " where semiNo like '%" + where + "%' or proName like '%" + where + "%' or proModel like '%" + where + "%' or staffName like '%" + where + "%' or remark like '%" + where + "%'  ";

            string sql = "SELECT   TOP (100) PERCENT SUM(amount) AS amount, CASE SUM(amount) WHEN 0 THEN 0 ELSE SUM(price * amount)/ SUM(amount) END AS price, SUM(amount * price) AS total, semiNo,  "
                        + " MAX(staffName) AS staffName, MAX(remark) AS remark, MAX(createDate) AS createDate, semiDate, checkStaff, checkDate, status "
                        + " FROM      dbo.ProductSemi "
                        +  where
                        + " GROUP BY semiNo, semiDate, checkStaff, checkDate, status "
                        + " ORDER BY semiNo DESC";
            var list = ServiceDB.Instance.QueryModelList<V_ProductSemi>(sql);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return View(data);
        }

        [AjaxAction(ForAction = "semilist", ForController = "stockin")]
        public ActionResult semilistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var one = stockinService.StockInList(no).FirstOrDefault();
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            var list = ServiceDB.Instance.QueryModelList<ProductSemi>("select * from ProductSemi where semiNo='" + no + "' order by createDate desc");
            data.no = no;
            data.one = one;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "semilist", ForController = "stockin")]
        public ActionResult semidelete()
        {
            string no = WebRequest.GetString("no", true);
            var result = ServiceDB.Instance.ExecuteSqlCommand("delete ProductSemi where semiNo='" + no + "'");
            return Content(result > 0 ? "ok" : "error");
        }
        [AjaxAction(ForAction = "semilist", ForController = "stockin")]
        public ActionResult changestatus()
        {
            string no = WebRequest.GetString("no", true);
            int status = WebRequest.GetInt("status", 0);
            var result = ServiceDB.Instance.ExecuteSqlCommand("update ProductSemi set status=" + status + ",checkStaff='"+Masterpage.CurrUser.name+"',checkDate=getdate() where semiNo='" + no + "'");
            return Content(result > 0 ? "ok" : "error");
        }
        #endregion


                    [AjaxAction(ForAction = "semilist", ForController = "stockin")]
        public ActionResult semiexport(string where)
        {
            if (where != null && where != "") where = " where semiNo like '%" + where + "%' or proName like '%" + where + "%' or proModel like '%" + where + "%' or staffName like '%" + where + "%' or remark like '%" + where + "%'  ";
            if (where == null) where = "";
            var list = ServiceDB.Instance.QueryModelList<ProductSemi>("select * from ProductSemi " + where + " order by semiNo desc,proName asc ");
            string[] head = new string[10] { "序号", "盘点单号", "物料名称", "物料规格", "数量","单价","金额", "盘点日期","人员","备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.semiNo + "|";
                row += p.proName + "|";
                row += p.proModel + "|";
                row += p.amount + "|";
                if (Enterprise.Invoicing.Web.Masterpage.CheckRight("semishowmoney_report"))
                {
                    row += p.price + "|";
                    row += Math.Round((decimal)p.amount * p.price, 4).ToString() + "|";
                }
                else
                {
                    row += "|"; row += "|";
                }
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.staffName + "|";

                row += p.remark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }

        public ActionResult semione()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_ProductSemi();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockinService.GetStockSemiNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_ProductSemi>("select * from V_ProductSemi where semiNo='" + no + "'");
            }
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "semione", ForController = "stockin")]
        public ActionResult savesemionetemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            int rad = WebRequest.GetFormInt("rad", 0);
            string m = WebRequest.GetString("m", true);
            string n = WebRequest.GetString("n", true);
            string count = WebRequest.GetString("count", true);
            string price = WebRequest.GetString("price", true);
            string no = WebRequest.GetString("no", true);
            double int_count = Convert.ToDouble(count);
            decimal int_price = Convert.ToDecimal(price);
            string remark = WebRequest.GetString("remark", true);
            List<ProductSemi> temp = (List<ProductSemi>)SessionHelper.GetSession("WE4" + no + Masterpage.CurrUser.staffid);
            if (rad == 0) int_count = 0 - int_count;
            if (temp == null || temp.Count == 0)
            {
                temp = new List<ProductSemi>();
                temp.Add(new ProductSemi
                {

                    semiId = 0,
                    amount = int_count,
                    createDate = DateTime.Now,
                    price = int_price,
                    proModel = n,
                    proName = m,
                    remark = remark,
                    semiNo = no,
                    staffId = Masterpage.CurrUser.staffid,
                    staffName = Masterpage.CurrUser.name
                });
            }
            else
            {
                bool add = false;
                ProductSemi ht = new ProductSemi();
                if (detail > 0)
                {
                    ht = temp.FirstOrDefault(p => p.semiId == detail);
                }
                else
                {
                    ht = temp.FirstOrDefault(p => p.proName == m && p.proModel == n);
                    if (ht == null)
                    {
                        ht = new ProductSemi();
                        ht.semiId = 0;
                        add = true;
                    }
                }

                ht.amount = int_count;
                ht.createDate = DateTime.Now;
                ht.price = int_price;
                ht.proModel = n;
                ht.proName = m;
                ht.remark = remark;
                ht.semiNo = no;
                ht.staffId = Masterpage.CurrUser.staffid;
                ht.staffName = Masterpage.CurrUser.name;
                if (add) temp.Add(ht);
            }
            SessionHelper.SetSession("WE4" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "semione", ForController = "stockin")]
        public ActionResult deletesemione()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            string n = WebRequest.GetString("n", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<ProductSemi> temp = (List<ProductSemi>)SessionHelper.GetSession("WE4" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.proName == m && p.proModel == n);

                if (h != null)
                {
                    if (g != 0) ServiceDB.Instance.ExecuteSqlCommand("delete ProductSemi where semiId=" + g);
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WE4" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "semione", ForController = "stockin")]
        public ActionResult savesemione()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string date = WebRequest.GetString("date", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<ProductSemi> temp = (List<ProductSemi>)SessionHelper.GetSession("WE4" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在半成品明细";
            }
            else
            {
                string semino = no;
                int count = 0;
                if (type == "add")
                {
                    semino = stockinService.GetStockSemiNo();
                }
                else if (type == "edit")
                {
                    semino = no;
                }
                foreach (var item in temp)
                {
                    string sql = "";
                    if (item.semiId <= 0) { sql = "insert into ProductSemi values('" + semino + "','" + item.proName + "','" + item.proModel + "'," + item.amount + "," + item.price + ",'" + item.remark + "'," + Masterpage.CurrUser.staffid + ",'" + Masterpage.CurrUser.name + "',getdate(),'" + date + "','',null,0)"; }
                    else { sql = "update ProductSemi set proName='" + item.proName + "',proModel='" + item.proModel + "',amount=" + item.amount + ",price=" + item.price + ",remark='" + item.remark + "',staffId=" + Masterpage.CurrUser.staffid + ",staffName='" + Masterpage.CurrUser.name + "',semiDate='" + date + "' where semiId=" + item.semiId; }
                    ServiceDB.Instance.ExecuteSqlCommand(sql);
                    count++;
                }
                r.status = count > 0;
            }
            SessionHelper.Del("WE4" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "semione", ForController = "stockin")]
        public ActionResult semionedetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = ServiceDB.Instance.QueryModelList<ProductSemi>("select * from ProductSemi where semiNo='"+no+"'");
            List<ProductSemi> temp = (List<ProductSemi>)SessionHelper.GetSession("WE4" + no + Masterpage.CurrUser.staffid);
            List<ProductSemi> newlist = new List<ProductSemi>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.semiId == item.semiId);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new ProductSemi
                        {
                            semiId = p.semiId,
                            amount = p.amount,
                            createDate = p.createDate,
                            price = p.price,
                            proModel = p.proModel,
                            proName = p.proName,
                            remark = p.remark,
                            semiNo = p.semiNo,
                            staffId = p.staffId,
                            staffName = p.staffName
                        }).ToList());
                }
            }
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("WE4" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }



        #endregion

    }
}
