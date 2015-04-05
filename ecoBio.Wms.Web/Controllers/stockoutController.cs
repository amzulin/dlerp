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
using Enterprise.Invoicing.Entities;
using Enterprise.Invoicing.Entities.Models;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class stockoutController : BaseController
    {
       
        private StockInService stockinService; 
        private StockOutService stockoutService; 
        private PurchaseService purchaseService;

        public stockoutController(IStockInRepository _stockinrepository, IStockOutRepository _stockoutrepository, IPurchaseRepository _purchaseRepository)
        {
            stockinService = new StockInService(_stockinrepository);
            stockoutService = new StockOutService(_stockoutrepository); 
            purchaseService = new PurchaseService(_purchaseRepository);
        }

        #region 出库单列表
        public ActionResult outlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            return View(data);
        }
         [AjaxAction(ForAction = "outlist", ForController = "stockout")]
        public ActionResult outlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockoutService.StockOutList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "outlist", ForController = "stockout")]
        public ActionResult outlistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);

            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            var list = stockoutService.StockOutDetailList(no).ToList();
            data.no = no;
            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "outlist", ForController = "stockout")]
        public ActionResult outdelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockoutService.DeleteStockOut(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        #region 销售出库

        public ActionResult outsell()
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
                no = stockoutService.GetStockOutNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.StockOutList(no).FirstOrDefault();
            }
            var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.mc = mc;
            var ddl = purchaseService.QuerySupplier(1, 1).ToList();
            data.ddl = ddl; data.no = no;
            data.depots = depots;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult saveoutselltemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            string m = WebRequest.GetString("m", true);
            string mc = WebRequest.GetString("mc", true);
            int int_depot = WebRequest.GetFormInt("depot", 0);
            string depotname = WebRequest.GetString("depotname", true);
            string count = WebRequest.GetString("count", true);
           float price = WebRequest.GetFormFloat("price", 0);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);
            double dprice = Convert.ToDouble(price);
            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string remark = WebRequest.GetString("remark", true);
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO1" + no + Masterpage.CurrUser.staffid);
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
                    depotName = depotname,
                    remark = remark,
                    price = dprice,
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
                ht.price = dprice;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WO1" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult deleteoutsell()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO1" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WO1" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult saveoutsell()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            int supplier = WebRequest.GetInt("supplier", 0);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO1" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在出库明细";
            }
            else if (supplier==0)
            {
                r.message = "未选择客户";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockoutService.AddStockOut(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, supplier, 1, remark,deport);
                    if (backno == "")
                    {
                        r.message = "出库单生成失败";
                    }
                    else
                    {
                        r = stockoutService.SaveStockOutDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockoutService.SaveStockOutDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WO1" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult outselldetail(string no,string order)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list0 = ServiceDB.Instance.QueryModelList<V_StockOutPurchase>("select * from V_StockOutPurchase where bomOrderNo='" + order + "' and stockoutNo='" + no + "'").ToList();
            var list1 = ServiceDB.Instance.QueryModelList<V_StockOutPurchase>("select * from V_StockOutPurchase where bomOrderNo='" + order + "' and stockoutNo is null").ToList();

            List<V_StockOutPurchase> list = new List<V_StockOutPurchase>();
            list.AddRange(list0);
            foreach (var item in list1)
            {
                var h = list0.FirstOrDefault(p => p.materialNo == item.materialNo);
                if (h == null) list.Add(item);
            }
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();       
            data.depots = depots;
            data.list = list;
            data.no = no;
            data.order = order;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult getcustomerorder(int supplier)
        {
            var list = ServiceDB.Instance.QueryModelList<BomOrder>("select * from BomOrder where status=1 and isover=0 and supplierId=" + supplier);
            return Json(list.Select(x => new { id = x.bomOrderNo, value = x.bomOrderNo }).ToList(), JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        [AjaxAction(ForAction = "outsell", ForController = "stockout")]
        public ActionResult saveoutsell_2(string outsnlist, string ordersnlist, string remarklist, string materiallist, string depotlist, string amountlist)
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            int supplier = WebRequest.GetInt("supplier", 0);
            string deport = WebRequest.GetString("deport", true);
            string order = WebRequest.GetString("order", true);
            ReturnValue r = new ReturnValue() { status = false };

                        string[] outsnl = outsnlist.Split(',');
                        string[] ordersnl = ordersnlist.Split(',');
                        string[] remarkl = remarklist.Split(',');
                        string[] materiall = materiallist.Split(',');
                        string[] depotl = depotlist.Split(',');
                        string[] amountl = amountlist.Split(',');
                        if (type == "add")
                        {
                            string backno = stockoutService.AddStockOut(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, supplier, 1, remark, deport);
                            if (backno == "")
                            {
                                r.message = "出库单生成失败";
                            }
                            else
                            {
                                try
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("update StockOut set  bomOrderNo='" + order + "' where stockoutNo='" + backno + "'");
                                    for (int i = 0; i < ordersnl.Length; i++)
                                    {
                                        var od = ServiceDB.Instance.QueryOneModel<BomOrderDetail>("select * from BomOrderDetail where detailSn=" + ordersnl[i]);
                                        ServiceDB.Instance.ExecuteSqlCommand("insert into StockOutDetail values('" + backno + "','" + materiall[i] + "'," + depotl[i] + "," + ordersnl[i] + ",null," + amountl[i] + ",0," + od.Price + ",null,'" + remarkl[i] + "')");
                                    }
                                    r.status = true;
                                }
                                catch (Exception x)
                                { r.message = "添加明细失败:" + x.Message; }
                            }
                        }
                        else if (type == "edit")
                        {
                            try
                            {
                                for (int i = 0; i < ordersnl.Length; i++)
                                {

                                    if (outsnl[i] == "0")
                                        ServiceDB.Instance.ExecuteSqlCommand("insert into StockOutDetail values('" + no + "','" + materiall[i] + "'," + depotl[i] + "," + ordersnl[i] + ",null," + amountl[i] + ",0,0,null,'" + remarkl[i] + "')");
                                    else if (outsnlist[i] > 0 && amountlist[i] == 0) ServiceDB.Instance.ExecuteSqlCommand("delete StockOutDetail where detailSn=" + outsnlist[i]);
                                    else ServiceDB.Instance.ExecuteSqlCommand("update StockOutDetail set depotId=" + depotl[i] + ",outAmount=" + amountl[i] + ",remark='" + remarkl[i] + "' where detailSn=" + outsnl[i]);

                                }
                                r.status = true;
                            }
                            catch (Exception x)
                            { r.message = "修改明细失败:" + x.Message; }

                        }
                        else r.message = "操作类别有误";

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 领料出库

        public ActionResult outpull()
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
                no = stockoutService.GetStockOutNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.StockOutList(no).FirstOrDefault();
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
        [AjaxAction(ForAction = "outpull", ForController = "stockout")]
        public ActionResult saveoutpulltemp()
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
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO2" + no + Masterpage.CurrUser.staffid);
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
                ht.materialModel = model;
                ht.materialName = name;
                ht.remark = remark;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WO2" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "outpull", ForController = "stockout")]
        public ActionResult deleteoutpull()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO2" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WO2" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "outpull", ForController = "stockout")]
        public ActionResult saveoutpull()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO2" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在出库明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockoutService.AddStockOut(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, 2, remark, deport);
                    if (backno == "")
                    {
                        r.message = "出库单生成失败";
                    }
                    else
                    {
                        r = stockoutService.SaveStockOutDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockoutService.SaveStockOutDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WO2" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "outpull", ForController = "stockout")]
        public ActionResult outpulldetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockoutService.StockOutDetailList(no).ToList();
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO2" + no + Masterpage.CurrUser.staffid);
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
            SessionHelper.SetSession("WO2" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }


        #endregion

        #region 其他出库

        public ActionResult outother()
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
                no = stockoutService.GetStockOutNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.StockOutList(no).FirstOrDefault();
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
        [AjaxAction(ForAction = "outother", ForController = "stockout")]
        public ActionResult saveoutothertemp()
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
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO8" + no + Masterpage.CurrUser.staffid);
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
                    depotId = int_depot,remark=remark,
                    depotName = depotname,
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
                ht.remark = remark;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WO8" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "outother", ForController = "stockout")]
        public ActionResult deleteoutother()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO8" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WO8" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "outother", ForController = "stockout")]
        public ActionResult saveoutother()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO8" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在出库明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockoutService.AddStockOut(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, 8, remark, deport);
                    if (backno == "")
                    {
                        r.message = "出库单生成失败";
                    }
                    else
                    {
                        r = stockoutService.SaveStockOutDetail(backno, temp, 0, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockoutService.SaveStockOutDetail(no, temp, 0, remark, deport);

                }
            }
            SessionHelper.Del("WO8" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "outother", ForController = "stockout")]
        public ActionResult outotherdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockoutService.StockOutDetailList(no).ToList();
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("WO8" + no + Masterpage.CurrUser.staffid);
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
            SessionHelper.SetSession("WO8" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }


        #endregion

        #region 以旧换新

        #region 列表
        public ActionResult changelist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
          
            return View(data);
        }
        [AjaxAction(ForAction = "changelist", ForController = "stockout")]
        public ActionResult changelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockoutService.ChangeOtNList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "changelist", ForController = "stockout")]
        public ActionResult changelistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            var inlist = stockoutService.ChangeOtNInList(no).ToList();
            var outlist = stockoutService.ChangeOtNOutList(no).ToList();
            data.no = no;
            data.inlist = inlist;
            data.outlist = outlist;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "changelist", ForController = "stockout")]
        public ActionResult changedelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockoutService.DeleteChangeOtN(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        #region 新增以旧换新

        public ActionResult outold2new()
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
                no = stockoutService.GetChangeOtNNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.ChangeOtNList(no).FirstOrDefault();
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
        [AjaxAction(ForAction = "outold2new", ForController = "stockout")]
        public ActionResult saveoutold2newtemp()
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
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("ON1" + no + Masterpage.CurrUser.staffid);
            //var material = stockinService.GetOneMaterial(m);
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
                    changeNo = no,
                    depotId = int_depot,
                    depotName = depotname,remark=remark,
                    changeType = rad,
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
                    ht = temp.FirstOrDefault(p => p.materialNo == m && p.depotId == int_depot && p.changeType == rad);
                    if (ht == null)
                    {
                        ht = new StockDetailModel();
                        add = true;
                        ht.type = "add";
                    }
                }
                ht.changeType = rad;
                ht.amount = int_count;
                ht.materialCategory = mc;
                ht.materialModel = model;
                ht.materialName = name;
                ht.remark = remark;
                ht.materialNo = m;
                ht.depotId = int_depot;
                ht.depotName = depotname; ht.changeNo = no;
                if (add) temp.Add(ht);
            }
            SessionHelper.SetSession("ON1" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "outold2new", ForController = "stockout")]
        public ActionResult deleteoutold2new()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            //changetype:type,depot
            var changetype = WebRequest.GetInt("changetype", 0);
            var depot = WebRequest.GetInt("depot", 0);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("ON1" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m && p.changeType == changetype && p.depotId == depot);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("ON1" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "outold2new", ForController = "stockout")]
        public ActionResult saveoutold2new()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("ON1" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在以旧换新明细";
            }
            else
            {
                var inlist = temp.Where(p => p.changeType == 0).ToList();

                var outlist = temp.Where(p => p.changeType == 1).ToList();
                if (inlist.Count < 1 || outlist.Count < 1)
                {
                    r.message = "旧物料入库、新物料出库均不能为空";
                }
                else
                {
                    if (type == "add")
                    {
                        string backno = stockoutService.AddChangeOtN(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, remark, deport);
                        if (backno == "")
                        {
                            r.message = "以旧换新单生成失败";
                        }
                        else
                        {
                            r = stockoutService.SaveChangeOtNDetail(backno, inlist, outlist, remark, deport);
                        }
                    }
                    else if (type == "edit")
                    {
                        r = stockoutService.SaveChangeOtNDetail(no, inlist, outlist, remark, deport);

                    }
                    else
                    {
                        r.message = "操作类别错误";
                    }
                }

            }
            SessionHelper.Del("ON1" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "outold2new", ForController = "stockout")]
        public ActionResult outold2newdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject(); 
            var inlist = stockoutService.ChangeOtNInList(no).ToList();
            var outlist = stockoutService.ChangeOtNOutList(no).ToList();
            List<StockDetailModel> list = new List<StockDetailModel>();
            list.AddRange(inlist);
            list.AddRange(outlist);
            List<StockDetailModel> temp = (List<StockDetailModel>)SessionHelper.GetSession("ON1" + no + Masterpage.CurrUser.staffid);
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
                            changeNo = p.stockNo,
                            amount = p.amount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            materialCategory = p.materialCategory,
                            remark = p.remark,
                            materialUnit = p.materialUnit,
                            depotName = p.depotName,
                            depotId = p.depotId,  changeType=p.changeType,                       
                            type = ""
                        }).ToList());
                }
            }
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("ON1" + no + Masterpage.CurrUser.staffid, newlist.OrderBy(p=>p.changeType).ToList());
            return PartialView(data);
        }


        #endregion
        #endregion

        #region 出库单退单

        #region 退单列表
        public ActionResult returnlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            return View(data);
        }
        [AjaxAction(ForAction = "returnlist", ForController = "stockout")]
        public ActionResult returnlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockoutService.ReturnModelList(where, orderby);

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
            //var one = stockoutService.ReturnModelList(no).FirstOrDefault();
            //var mc = stockinService.GetMaterialCategory();
            //data.mc = mc;
            var list = stockoutService.StockReturnDetailList(no).ToList();
            data.no = no;
            //data.one = one;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnlist", ForController = "stockin")]
        public ActionResult returndelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockoutService.DeleteStockReturn(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        public ActionResult returnone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new ReturnModel();
            var list = new List<StockReturnDetailModel>();
            var message = "";
            var outno = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockoutService.GetStockReturnNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.ReturnModelList(no).FirstOrDefault();
            }
            data.one = model;
            #region 默认申请单
            var had = stockoutService.GetReturnHadOut(no);
            if (had != null && had.Count > 0) outno = had[0].text;
            else
            {
                var all = stockoutService.GetCanReturn("",2);
                if (all != null && all.Count > 0) outno = all[0].text;
            }
            list = stockoutService.StockReturnDetailList(no).ToList();
            SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            #endregion     
         
            data.no = no;
            data.outno = outno;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult canreturnlist(int? page, int? pagesize, string no,int returntype, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockoutService.GetReturnHadOut(no);
            var all = stockoutService.GetCanReturn(query, returntype);
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
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult canreturntemp(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult canreturntab(string outno, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockoutService.StockReturnDetailList( no, outno);
            var orderdetail = stockoutService.StockReturnDetailListByOut(outno).ToList();
            if (had != null && had.Count > 0)
            {
                foreach (var item in orderdetail)
                {
                    var h = had.FirstOrDefault(p => p.stockoutDetailSn == item.stockoutDetailSn&&p.materialNo==item.materialNo);
                    if (h != null)
                    {
                        item.detailSn = h.detailSn;
                        item.returnNo = h.returnNo;
                        item.returnAmount = h.returnAmount;
                        item.todepotName = h.todepotName;
                        item.todepotId = h.todepotId; item.remark = h.remark;
                    }
                }
            }
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            data.no = no;
            data.outno = outno;
            data.list = orderdetail; 
            return PartialView(data);
        }
        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult queryorder(string key,int outtype)
        {
            var all = stockoutService.GetCanReturn(key, outtype);
            var r = all.Select(x => new { value = x.text, key = x.text });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult savecanreturntemp(string outno, string no, string details, string counts, string depotids, string models,string remarks)
        {
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
            if (depotids.EndsWith(",")) depotids = depotids.Substring(0, depotids.Length - 1);
            if (models.EndsWith(",")) models = models.Substring(0, models.Length - 1);
            //if (remarks.EndsWith(",")) remarks = remarks.Substring(0, remarks.Length - 1);
            string[] ds = details.Split(',');
            string[] cs = counts.Split(',');
            string[] ps = depotids.Split(',');
            string[] ms = models.Split(',');
            string[] rs = remarks.Split(',');

            for (int i = 0; i < ds.Length; i++)
            {
                double fc = Convert.ToDouble(cs[i]);
                int depid = Convert.ToInt32(ps[i]);
                int rdsn = Convert.ToInt32(ds[i]);
                    var to = stockoutService.GetDepotDetail(depid, "").FirstOrDefault();
                var had = list.FirstOrDefault(p => p.stockoutDetailSn == rdsn && p.materialNo == ms[i]);
                if (had != null)
                {
                    if (fc == 0)
                    {
                        if (had.detailSn == 0) list.Remove(had);
                        else had.type = "delete";
                    }
                    else
                    {
                        had.returnAmount = fc;
                        had.todepotId = depid;
                        had.todepotName = to.depotName;
                        had.remark = rs[i];
                        if (had.type != "add") had.type = "edit";
                    }
                }
                else
                {
                    if (fc == 0) continue;
                    var r = stockoutService.StockOutDetailOne(rdsn);
                    list.Add(new StockReturnDetailModel
                    {
                        detailSn = 0,
                        materialModel = r.materialModel,
                        materialName = r.materialName,
                        materialNo = r.materialNo,
                        materialUnit = r.materialUnit,
                        stockoutDetailSn = rdsn,
                        fromdepotId = r.depotId,
                        fromdepotName = r.depotName,
                        materialCategory = r.materialCategory,
                        outAmoutn = r.amount,
                        stockoutNo = r.stockNo,
                        todepotId = to.depotId,
                        todepotName = to.depotName,
                        returnAmount = fc,
                        returnNo = no,
                        remark=rs[i],
                        type = "add"

                    });
                }
            }
            SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult deletecanreturntemp(string no, int sn, string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);

            var had = list.FirstOrDefault(p => p.stockoutDetailSn == sn && p.materialModel == m);
            if (had != null)
            {
                if (had.detailSn == 0) list.Remove(had);
                else had.type = "delete";
                SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            }
            return Content(list.Count.ToString());
        }
        [AjaxAction(ForAction = "returnone,returnred", ForController = "stockout")]
        public ActionResult savecanreturn(string no)
        {
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            var returntype = WebRequest.GetInt("returntype", 0);
            ReturnValue r = new ReturnValue() { status = false };
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            if (list.Count == 0)
            {
                r = new ReturnValue { status = false, message = "未提交任何退单明细" };
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockoutService.AddStockReturn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, remark, deport, returntype);
                    if (backno == "")
                    {
                        r.message = "领料单生成失败";
                    }
                    else
                    {
                        r = stockoutService.SaveStockReturnDetail(backno, list, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockoutService.SaveStockReturnDetail(no, list, remark, deport);

                }
            }
            SessionHelper.Del("SR" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 直接销售退单

        public ActionResult returnred()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new ReturnModel();
            var list = new List<StockReturnDetailModel>();
            var message = "";
            var outno = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockoutService.GetStockReturnNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.ReturnModelList(no).FirstOrDefault();
            }
            data.one = model;
            #region 默认申请单
            var had = stockoutService.GetReturnHadOut(no);
            if (had != null && had.Count > 0) outno = had[0].text;
            else
            {
                var all = stockoutService.GetCanReturn("",4);
                if (all != null && all.Count > 0) outno = all[0].text;
            }
            list = stockoutService.StockReturnDetailList(no).ToList();
            SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            #endregion

            data.no = no;
            data.outno = outno;
            data.message = message;
            data.type = type;
            return View(data);
        }
        #endregion

        #region 销售出库单退单

        #region 退单列表
        public ActionResult rselllist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View(data);
        }
        [AjaxAction(ForAction = "rselllist", ForController = "stockout")]
        public ActionResult rselllistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = stockoutService.ReturnSellModelList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "rselllist", ForController = "stockin")]
        public ActionResult rselllistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = stockoutService.StockReturnDetailList(no).ToList();
            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "rselllist", ForController = "stockin")]
        public ActionResult rselldelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = stockoutService.DeleteStockReturn(no);
            return Content(result ? "ok" : "error");
        }
        #endregion
        public ActionResult returnrsell()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var supplier = WebRequest.GetInt("supplier", 0);
            var model = new ReturnModel();
            var list = new List<StockReturnDetailModel>();
            var message = "";
            var outno = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockoutService.GetStockReturnNo();
                var ones = ServiceDB.Instance.QueryOneModel<Supplier>("select * from Supplier where supplierid=" + supplier);
                if (ones != null)
                {
                    model.supplierName = ones.supplierName;
                    model.supplierId = supplier;
                }
                else model.supplierName = "";
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.ReturnSellModelList(no).FirstOrDefault();
            }
            data.one = model;
            #region 默认申请单
            var had = stockoutService.GetReturnHadOut(no);
            if (had != null && had.Count > 0) outno = had[0].text;
            else
            {
                var all = stockoutService.GetCanReturn(supplier,"");
                if (all != null && all.Count > 0) outno = all[0].text;
            }
            list = stockoutService.StockReturnDetailList(no).ToList();
            SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            #endregion
              var ddl = purchaseService.QuerySupplier(1, 1).ToList();
              data.ddl = ddl;
            data.no = no;
            data.outno = outno;
            data.message = message;
            data.supplier = supplier;
            data.type = type;
            return View(data);
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult canreturnrlist(int? page, int? pagesize, int supplier, string no, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockoutService.GetReturnHadOut(no);
            var all = stockoutService.GetCanReturn(supplier,query);
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
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult canreturnrtemp(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult canreturnrtab(string outno, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = stockoutService.StockReturnDetailList(no, outno);
            var orderdetail = stockoutService.StockReturnDetailListByOut(outno).ToList();
            if (had != null && had.Count > 0)
            {
                foreach (var item in orderdetail)
                {
                    var h = had.FirstOrDefault(p => p.stockoutDetailSn == item.stockoutDetailSn && p.materialNo == item.materialNo);
                    if (h != null)
                    {
                        item.detailSn = h.detailSn;
                        item.returnNo = h.returnNo;
                        item.returnAmount = h.returnAmount;
                        item.todepotName = h.todepotName;
                        item.todepotId = h.todepotId; item.remark = h.remark;
                    }
                }
            }
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            data.no = no;
            data.outno = outno;
            data.list = orderdetail;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult queryrorder(int supplier, string key)
        {
            var all = stockoutService.GetCanReturn(supplier,key);
            var r = all.Select(x => new { value = x.text, key = x.text });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult savecanreturnrtemp(string outno, string no, string details, string counts, string depotids, string models, string remarks)
        {
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
            if (depotids.EndsWith(",")) depotids = depotids.Substring(0, depotids.Length - 1);
            if (models.EndsWith(",")) models = models.Substring(0, models.Length - 1);
            //if (remarks.EndsWith(",")) remarks = remarks.Substring(0, remarks.Length - 1);
            string[] ds = details.Split(',');
            string[] cs = counts.Split(',');
            string[] ps = depotids.Split(',');
            string[] ms = models.Split(',');
            string[] rs = remarks.Split(',');

            for (int i = 0; i < ds.Length; i++)
            {
                double fc = Convert.ToDouble(cs[i]);
                int depid = Convert.ToInt32(ps[i]);
                int rdsn = Convert.ToInt32(ds[i]);
                var to = stockoutService.GetDepots(depid).FirstOrDefault();
                var had = list.FirstOrDefault(p => p.stockoutDetailSn == rdsn && p.materialNo == ms[i]);
                if (had != null)
                {
                    if (fc == 0)
                    {
                        if (had.detailSn == 0) list.Remove(had);
                        else had.type = "delete";
                    }
                    else
                    {
                        had.returnAmount = fc;
                        had.todepotId = depid;
                        had.todepotName = to.depotName;
                        had.remark = rs[i];
                        if (had.type != "add") had.type = "edit";
                    }
                }
                else
                {
                    if (fc == 0) continue;
                    var r = stockoutService.StockOutDetailOne(rdsn);
                    list.Add(new StockReturnDetailModel
                    {
                        detailSn = 0,
                        materialModel = r.materialModel,
                        materialName = r.materialName,
                        materialNo = r.materialNo,
                        materialUnit = r.materialUnit,
                        stockoutDetailSn = rdsn,
                        fromdepotId = r.depotId,
                        fromdepotName = r.depotName,
                        materialCategory = r.materialCategory,
                        outAmoutn = r.amount,
                        stockoutNo = r.stockNo,
                        todepotId = to.depotId,
                        todepotName = to.depotName,
                        returnAmount = fc,
                        returnNo = no,
                        remark = rs[i],
                        type = "add"

                    });
                }
            }
            SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult deletecanreturnrtemp(string no, int sn, string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);

            var had = list.FirstOrDefault(p => p.stockoutDetailSn == sn && p.materialModel == m);
            if (had != null)
            {
                if (had.detailSn == 0) list.Remove(had);
                else had.type = "delete";
                SessionHelper.SetSession("SR" + no + Masterpage.CurrUser.staffid, list);
            }
            return Content(list.Count.ToString());
        }
        [AjaxAction(ForAction = "returnrsell", ForController = "stockout")]
        public ActionResult savecanrreturn(string no)
        {
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            var supplier = WebRequest.GetInt("supplier", 0);
            var returntype = WebRequest.GetInt("returntype", 0);
            ReturnValue r = new ReturnValue() { status = false };
            var list = new List<StockReturnDetailModel>();
            list = (List<StockReturnDetailModel>)SessionHelper.GetSession("SR" + no + Masterpage.CurrUser.staffid);
            if (list.Count == 0)
            {
                r = new ReturnValue { status = false, message = "未提交任何退单明细" };
            }
            else
            {
                if (type == "add")
                {
                    string backno = stockoutService.AddStockReturn(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, supplier, remark, deport, returntype);
                    if (backno == "")
                    {
                        r.message = "销售退单生成失败";
                    }
                    else
                    {
                        r = stockoutService.SaveStockReturnDetail(backno, list, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = stockoutService.SaveStockReturnDetail(no, list, remark, deport);

                }
            }
            SessionHelper.Del("SR" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 销售出库

        public ActionResult outtsell()
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
                no = stockoutService.GetStockOutNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.StockOutList(no).FirstOrDefault();
            }
            var mc = ServiceDB.Instance.QueryOneModel<Dictionary>("select * from Dictionary where dictionaryKey='ExpressCompany'");
            var ds = mc.dictionaryValue.Split('|').Select(x => new SelectListItem { Text = x, Value = x }).ToList();
            data.mc = ds;
            var ddl = purchaseService.QuerySupplier(1, 1).ToList();
            data.ddl = ddl; data.no = no;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "outtsell", ForController = "stockout")]
        public ActionResult outtselldetail(string no, string order)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockoutService.StockOutDetailList(no).ToList();
            data.list = list;
            data.no = no;
            return PartialView(data);
        }



        [HttpPost]
        [AjaxAction(ForAction = "outtsell", ForController = "stockout")]
        public ActionResult saveouttsell_2( string pricelist, string remarklist, string materiallist, string amountlist)
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            int supplier = WebRequest.GetInt("supplier", 0);
            string express = WebRequest.GetString("express", true);
            string expresscode = WebRequest.GetString("expresscode", true);
            string date = WebRequest.GetString("date", true);
            ReturnValue r = new ReturnValue() { status = false };

            string[] remarkl = remarklist.Split(',');
            string[] materiall = materiallist.Split(',');
            string[] pricel = pricelist.Split(',');
            string[] amountl = amountlist.Split(',');
            if (amountl.Length < 1 || materiall.Length != pricel.Length)
            {
                r = new ReturnValue { status = false, message = "未提交任何明细" };
            }
            else
            {
                var head = 0;
                if (type == "add")
                {
                    string backno = stockoutService.GetStockOutNo();
                    if (backno != "")
                    {
                        head = ServiceDB.Instance.ExecuteSqlCommand(
                            "INSERT INTO [dbo].[StockOut] values('" + backno + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + "," + supplier
                            + ",'',null,4,'',0,0,0,getdate(),1,0,0,'','" + remark + "','" + deport+"','"+express+"','"+expresscode+"',"+(date==""?"null":"'"+date+"'")+")");
                        no = backno;
                    }
                }
                else if (type == "edit")
                {
                    head = ServiceDB.Instance.ExecuteSqlCommand("update [dbo].[StockOut] set deportStaff='" + deport + "',remark='" + remark + "',express='" + express + "',expresscode='" + expresscode + "',outDate= " + (date == "" ? "null" : "'" + date + "'") + " where stockoutNo='" + no + "'");

                }
                if (head > 0)
                {
                    #region 修改明细
                    if (type == "edit") ServiceDB.Instance.ExecuteSqlCommand("delete StockOutDetail where stockoutNo='" + no + "'");
                    head = 0;
                    List<string> delgno = new List<string>();
                    for (int i = 0; i < materiall.Length; i++)
                    {
                        if (materiall[i] == "" || pricel[i] == "" || pricel[i] == "0" || amountl[i] == ""|| amountl[i] == "0") continue;
                        var haddetail = ServiceDB.Instance.QueryOneModel<StockOutDetail>("select * from StockOutDetail where stockoutNo='" + no + "' and materialNo='" + materiall[i] + "'");
                        var addone = 0;
                        if (haddetail != null)
                        {
                            addone = ServiceDB.Instance.ExecuteSqlCommand("update INTO [dbo].[StockOutDetail] set outAmount+=" + amountl[i] + ", outPrice=" + pricel[i] + " where detailSn=" + haddetail.orderSn);
                        }
                        else
                        {
                            addone = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[StockOutDetail] VALUES('" + no + "','" + materiall[i] + "',2,null,null," + amountl[i] + ",0," + pricel[i] + ",'','" + remarkl[i] + "')");
                        }
                        head += addone;
                    }
                    if (head < 1)
                    {
                        ServiceDB.Instance.ExecuteSqlCommand("delete StockOut where stockoutNo='" + no + "'");
                    }
                    #endregion
                    r.status = true;
                }
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
