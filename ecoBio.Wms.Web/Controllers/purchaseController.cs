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
namespace Enterprise.Invoicing.Web.Controllers
{
    public class purchaseController : BaseController
    {

        private PurchaseService purchaseService;


        public purchaseController(IPurchaseRepository _purchaseRepository)
        {
            purchaseService = new PurchaseService(_purchaseRepository);
        }

        #region 采购申请

        public ActionResult requirelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "requirelist", ForController = "purchase")]
        public ActionResult requirelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            //var no = WebRequest.GetString("no", true);
            //var status = WebRequest.GetInt("status", 0);
            ////var orderby = WebRequest.GetString("orderby", true);
            //data.no = no;
            //data.status = status;

            //if (orderby == "") orderby = "createdate desc";
            //string where = "";
            //if (no != "") where += " and requireNo like '%" + no + "%'";
            //where += " and isover=" + status;
            if (where == null) where = "";
            if (orderby == null||orderby=="") orderby = " createdate desc ";
            var list = purchaseService.RequireList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
           // data.otherParam = "&no=" + no + "&status=" + status + "&orderby=" + orderby;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "requirelist", ForController = "purchase")]
        public ActionResult requiredetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
           // var one = purchaseService.RequireList(no).FirstOrDefault();
            var list = purchaseService.RequireDetailList(no);
           // data.one = one;
            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        public ActionResult requireone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new PurchaseRequireMode();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = purchaseService.GetRequireNo();

                // string backno = purchaseService.AddRequire("",Masterpage.CurrUser.staffid,Masterpage.CurrUser.depId,0,remark);                  

                #endregion
            }
            else if (type == "edit")
            {
                model = purchaseService.RequireList(no).FirstOrDefault();
            }
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }



        [HttpPost]
        [AjaxAction(ForAction = "requireone", ForController = "purchase")]
        public ActionResult saverequiredetailtemp()
        {
            //type: htype, no: no, detail: hdetail, m: hmaterial, name: hname, model: hmodel, count: ctxt
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string hth = WebRequest.GetString("hth", true);

            string remark = WebRequest.GetString("remark", true);
            string date = WebRequest.GetString("date", true);
            List<PurchaseRequireDetailModel> temp = (List<PurchaseRequireDetailModel>)SessionHelper.GetSession("NE" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<PurchaseRequireDetailModel>();
                temp.Add(new PurchaseRequireDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    materialUnit = "",
                    materialName = name,
                    materialNo = m,
                    orderAmount = int_count,  materialTu=hth,
                    requireNo = no, remark=remark, needdate=(date!=""?Convert.ToDateTime(date):DateTime.MaxValue),
                    type = "add"
                });
            }
            else
            {
                var ht = temp.FirstOrDefault(p => p.materialNo == m);
                if (ht != null)
                {
                    if (ht.detailSn != 0) ht.type = "edit";
                    ht.orderAmount = int_count;
                    ht.needdate = (date != "" ? Convert.ToDateTime(date) : DateTime.MaxValue);
                    ht.remark = remark;
                }
                else
                {
                    temp.Add(new PurchaseRequireDetailModel
                    {
                        detailSn = 0,
                        materialModel = model,
                        materialUnit = "",
                        materialName = name,
                        materialNo = m,
                        orderAmount = int_count,
                        requireNo = no,
                        remark = remark, materialTu=hth,
                        needdate = (date != "" ? Convert.ToDateTime(date) : DateTime.MaxValue),
                        type = "add"
                    });
                }
            }
            SessionHelper.SetSession("NE" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "requireone", ForController = "purchase")]
        public ActionResult deleterequiredetail()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<PurchaseRequireDetailModel> temp = (List<PurchaseRequireDetailModel>)SessionHelper.GetSession("NE" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("NE" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "requireone", ForController = "purchase")]
        public ActionResult saverequiredetail()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<PurchaseRequireDetailModel> temp = (List<PurchaseRequireDetailModel>)SessionHelper.GetSession("NE" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在申请明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = purchaseService.AddRequire("", Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, remark);
                    if (backno == "")
                    {
                        r.message = "申请单生成失败";
                    }
                    else
                    {
                        r = purchaseService.SaveRequirDetail(backno, temp, remark);
                    }
                }
                else if (type == "edit")
                {
                    r = purchaseService.SaveRequirDetail(no, temp, remark);

                }
            }
            SessionHelper.Del("NE" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "requireone", ForController = "purchase")]
        public ActionResult requiredetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = purchaseService.RequireDetailList(no);
            List<PurchaseRequireDetailModel> temp = (List<PurchaseRequireDetailModel>)SessionHelper.GetSession("NE" + no + Masterpage.CurrUser.staffid);
            List<PurchaseRequireDetailModel> newlist = new List<PurchaseRequireDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.materialNo == item.materialNo);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new PurchaseRequireDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            requireNo = p.requireNo,
                            orderAmount = p.orderAmount,
                            materialModel = p.materialModel,
                            materialName = p.materialName, materialTu=p.materialTu,
                            remark = p.remark,
                            createDate = p.createDate,
                            materialUnit = p.materialUnit, needdate=p.needdate,
                            type = ""
                        }).ToList());
                }
            }
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("NE" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView("requiredetail", data);
        }

        [AjaxAction(ForAction = "requirelist", ForController = "purchase")]
        public ActionResult requiredelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = purchaseService.DeleteRequire(no);
            return Content(result ? "ok" : "error");
        }


        #endregion

        #region 采购订单

        public ActionResult orderlist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
          
            return View(data);
        }

        [AjaxAction(ForAction = "orderlist", ForController = "purchase")]
        public ActionResult orderlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = purchaseService.PurchaseList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return View(data);
        }

        [AjaxAction(ForAction = "orderlist", ForController = "purchase")]
        public ActionResult orderdetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = purchaseService.PurchaseDetailList(no);
            var one = purchaseService.PurchaseList(no).FirstOrDefault();
            data.one = one;
            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        public ActionResult orderone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new PurchaseModel();

            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = purchaseService.GetPurchaseNo();

                // string backno = purchaseService.AddRequire("",Masterpage.CurrUser.staffid,Masterpage.CurrUser.depId,0,remark);                  

                #endregion
            }
            else if (type == "edit")
            {
                model = purchaseService.PurchaseList(no).FirstOrDefault();
            }
            var ddl = purchaseService.QuerySupplier(0,1).ToList();
            data.ddl = ddl; data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }



        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "purchase")]
        public ActionResult saveorderdetailtemp()
        {
            //type: htype, no: no, detail: hdetail, m: hmaterial, name: hname, model: hmodel, count: ctxt
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string price = WebRequest.GetString("price", true);
            string no = WebRequest.GetString("no", true);
            double int_count = Convert.ToDouble(count);
            double d_price = Convert.ToDouble(price);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string remark = WebRequest.GetString("remark", true);
            string date = WebRequest.GetString("date", true);
            List<PurchaseDetailModel> temp = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<PurchaseDetailModel>();
                temp.Add(new PurchaseDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    materialUnit = "",
                    materialName = name,
                    materialNo = m,
                    poAmount = int_count,
                    remark = remark,
                    sendDate = (date != "" ? Convert.ToDateTime(date) : DateTime.MaxValue),
                    poPrice = d_price,
                    purchaseNo = no,
                    type = "add"
                });
            }
            else
            {
                var ht = temp.FirstOrDefault(p => p.materialNo == m);
                if (ht != null)
                {
                    if (ht.detailSn != 0) ht.type = "edit";
                    ht.poAmount = int_count;
                    ht.poPrice = d_price;
                    ht.remark = remark;
                    ht.sendDate = (date != "" ? Convert.ToDateTime(date) : DateTime.MaxValue);
                }
                else
                {
                    temp.Add(new PurchaseDetailModel
                    {
                        detailSn = 0,
                        materialModel = model,
                        materialUnit = "",
                        materialName = name,
                        materialNo = m,
                        poAmount = int_count,
                        poPrice = d_price,
                        purchaseNo = no,
                        remark = remark,
                        sendDate = (date != "" ? Convert.ToDateTime(date) : DateTime.MaxValue),
                        type = "add"
                    });
                }
            }
            SessionHelper.SetSession("PO" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "purchase")]
        public ActionResult deleteorderdetail()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<PurchaseDetailModel> temp = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("PO" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "purchase")]
        public ActionResult saveorderdetail()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string supplier = WebRequest.GetString("supplier", true);
            int i_supplier = Convert.ToInt32(supplier);
            string remark = WebRequest.GetString("remark", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<PurchaseDetailModel> temp = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在申请明细";
            }
            else
            {
                if (type == "add")
                {
                    string backno = purchaseService.AddPurchase("", Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, i_supplier, 0, 0, remark);
                    if (backno == "")
                    {
                        r.message = "申请单生成失败";
                    }
                    else
                    {
                        r = purchaseService.SavePurchaseDetail(backno, temp, i_supplier, remark);
                    }
                }
                else if (type == "edit")
                {
                    r = purchaseService.SavePurchaseDetail(no, temp, i_supplier, remark);

                }
            }
            SessionHelper.Del("PO" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "orderone", ForController = "purchase")]
        public ActionResult orderdetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = purchaseService.PurchaseDetailList(no);
            List<PurchaseDetailModel> temp = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO" + no + Masterpage.CurrUser.staffid);
            List<PurchaseDetailModel> newlist = new List<PurchaseDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.materialNo == item.materialNo);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new PurchaseDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            requireNo = p.requireNo,
                            poAmount = p.poAmount,
                            poPrice = p.poPrice,
                            poRemain = p.poRemain,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            remark = p.remark,
                            materialUnit = p.materialUnit, sendDate=p.sendDate,
                            type = ""
                        }).ToList());
                }
            }
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("PO" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView("orderdetail", data);
        }

        [AjaxAction(ForAction = "orderlist", ForController = "purchase")]
        public ActionResult orderdelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = purchaseService.DeletePurchase(no);
            return Content(result ? "ok" : "error");
        }


        #endregion

        #region 申请单采购
        public ActionResult requireorder()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new PurchaseModel();
            var list = new List<PurchaseDetailModel>();
            var message = "";
            var order = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = purchaseService.GetPurchaseNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = purchaseService.PurchaseList(no).FirstOrDefault();
            }
            var ddl = purchaseService.QuerySupplier(0,1).ToList();
            data.ddl = ddl; data.no = no;
            data.one = model;
            #region 默认申请单
            var had = purchaseService.GetCurchaseHadOrder(no);
            if (had != null && had.Count > 0) order = had[0].text;
            else
            {
                var all = purchaseService.GetNeedBuyOrder("");
                if (all != null && all.Count > 0) order = all[0].text;
            }
            list = purchaseService.PurchaseDetailList2(no);
            SessionHelper.SetSession("PO2" + no + Masterpage.CurrUser.staffid, list);
            #endregion
            data.order = order;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult needbuylist(int? page, int? pagesize, string no, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = purchaseService.GetCurchaseHadOrder(no);
            var all = purchaseService.GetNeedBuyOrder(query);
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
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }


        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult needbuytemp(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<PurchaseDetailModel>();
            list = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO2" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }


        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult needbuytab(string order, int supplier, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = purchaseService.GetPurchaseRequireByPurchaseNo(order, no);
            var orderdetail = purchaseService.RequireDetailList(order);

            foreach (var item in orderdetail)
            {
                var h = had.FirstOrDefault(p => p.detailSn == item.detailSn);
                if (h != null)
                {
                    //item.ChemicalPoInputAmount -= h.StockInputAmount;
                    item.mebuyAmount = h.mebuyAmount;
                    item.mebuyPrice = h.mebuyPrice;
                    item.mysenddate = h.mysenddate;
                }

                if (supplier > 0)
                {
                    #region 价格选择
                    var ps = ServiceDB.Instance.QueryModelList<ReturnValue>("select convert(varchar,price) as value,convert(varchar,price)+'备注:'+remark as value2,'' as message,convert(bit,1) as status from MaterialPrice where supplierId="
                          + supplier + " and materialNo='"
                          + item.materialNo + "' and status=1 and startDate<=getdate() and (endDate is null or endDate>=getdate())").ToList();
                    item.priceList = ps;
                    #endregion
                }
                else item.priceList = new List<ReturnValue>();
            }
            data.no = no;
            data.order = order;
            data.list = orderdetail;
            return PartialView(data);
        }
       [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult queryorder(string key)
        {
            var all = purchaseService.GetNeedBuyOrder(key);
            var r = all.Select(x => new { value = x.text, key = x.text });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
       [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
       public ActionResult saverequirebuytemp(string order, string no, string details, string counts, string prices, string models, string dates, string remarks)
       {
           var list = new List<PurchaseDetailModel>();
           list = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO2" + no + Masterpage.CurrUser.staffid);
           //if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
           //if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
           //if (prices.EndsWith(",")) prices = prices.Substring(0, prices.Length - 1);
           //if (models.EndsWith(",")) models = models.Substring(0, models.Length - 1);
           //if (dates.EndsWith(",")) dates = dates.Substring(0, dates.Length - 1);
           //if (remarks.EndsWith(",")) remarks = remarks.Substring(0, remarks.Length - 1);
           string[] ds = details.Split(',');
           string[] cs = counts.Split(',');
           string[] ps = prices.Split(',');
           string[] ms = models.Split(',');
           string[] ts = dates.Split(',');
           string[] rs = remarks.Split(',');

           for (int i = 0; i < ds.Length; i++)
           {
               double fc = Convert.ToDouble(cs[i]);
               double pc = Convert.ToDouble(ps[i]);
               int rdsn = Convert.ToInt32(ds[i]);
               var had = list.FirstOrDefault(p => p.requireDetailSn == rdsn&& p.materialNo==ms[i]);
               if (had != null)
               {
                   if (fc == 0)
                   {
                       if (had.detailSn == 0) list.Remove(had);
                       else had.type = "delete";
                   }
                   else
                   {
                       had.poAmount = fc;
                       had.poPrice = pc;
                       had.remark = rs[i];
                       had.sendDate = ts[i] != "" ? Convert.ToDateTime(ts[i]) : DateTime.MaxValue;
                       if (had.type != "add") had.type = "edit";
                   }
               }
               else
               {
                   if (fc == 0) continue;
                   //var m = purchaseService.QueryOneMaterial(ms[i]);
                   var r = purchaseService.RequireDetailOne(rdsn);
                   list.Add(new PurchaseDetailModel
                   {
                       poPrice = pc,
                       detailSn = 0,
                       poAmount = fc,
                       materialModel = r.materialModel,
                       requireDetailSn = rdsn,
                       materialName = r.materialName,
                       materialNo = r.materialNo,
                       materialUnit = r.materialUnit,
                       poRemain = fc,
                       purchaseNo = no,
                       requireAmount = r.orderAmount,
                       requireNo=r.requireNo,
                       type = "add", remark=rs[i],
                       sendDate = ts[i] != "" ? Convert.ToDateTime(ts[i]) : DateTime.MaxValue

                   });
               }
           }
           SessionHelper.SetSession("PO2" + no + Masterpage.CurrUser.staffid, list);
           return Json(list.Count, JsonRequestBehavior.AllowGet);
       }

       [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
       public ActionResult deleteneedbuytemp(string no, int sn, string m)
       {
           dynamic data = new System.Dynamic.ExpandoObject();
           var list = new List<PurchaseDetailModel>();
           list = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO2" + no + Masterpage.CurrUser.staffid);

           var had = list.FirstOrDefault(p => p.requireDetailSn == sn && p.materialModel == m);
           if (had != null)
           {
               if (had.detailSn == 0) list.Remove(had);
               else had.type = "delete";
               SessionHelper.SetSession("PO2" + no + Masterpage.CurrUser.staffid, list);
           }
           return Content(list.Count.ToString());
       }
       [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
       public ActionResult savesneedbuy(string no)
       {
           string type = WebRequest.GetString("type", true);
           string supplier = WebRequest.GetString("supplier", true);
           int i_supplier = Convert.ToInt32(supplier);
           string remark = WebRequest.GetString("remark", true);
           ReturnValue r = new ReturnValue() { status = false };
           var list = new List<PurchaseDetailModel>();
           list = (List<PurchaseDetailModel>)SessionHelper.GetSession("PO2" + no + Masterpage.CurrUser.staffid);
           if (list.Count == 0)
           {
               r = new ReturnValue { status = false, message = "未提交任何订单明细" };
           }
           else
           {
               if (type == "add")
               {
                   string backno = purchaseService.AddPurchase("", Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, i_supplier, 1, 0, remark);
                   if (backno == "")
                   {
                       r.message = "采购单生成失败";
                   }
                   else
                   {
                       r = purchaseService.SavePurchaseDetail2(backno, list, i_supplier, remark);
                   }
               }
               else if (type == "edit")
               {
                   r = purchaseService.SavePurchaseDetail2(no, list, i_supplier, remark);

               }
           }
           SessionHelper.Del("PO2" + no + Masterpage.CurrUser.staffid);
           return Json(r, JsonRequestBehavior.AllowGet);

       }
        #endregion

    }
}
