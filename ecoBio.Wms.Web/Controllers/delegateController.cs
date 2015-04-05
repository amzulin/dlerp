using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Service;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Enterprise.Invoicing.Entities;
using Enterprise.Invoicing.Entities.Models;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class delegateController : BaseController
    {
        private ManageService manageService;
        private BomService bomService;
        private PurchaseService purchaseService;
        private StockOutService stockoutService;
        private StockInService stockinService;
        public delegateController(IManageRepository _manageRepository, IBomRepository _bomrepository, IPurchaseRepository _bomRepository, IStockInRepository _stockinrepository, IStockOutRepository _stockoutrepository)
        {
            manageService = new ManageService(_manageRepository);
            bomService = new BomService(_bomrepository);
            stockoutService = new StockOutService(_stockoutrepository);
            stockinService = new StockInService(_stockinrepository);
            purchaseService = new PurchaseService(_bomRepository);
        }

        #region 委外单列表

        public ActionResult delegatelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "delegatelist", ForController = "delegate")]
        public ActionResult delegatelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            //select * from V_PurchaseRequireMode where delegateNo<>'' " + where + " order by " + orderby
            var list = ServiceDB.Instance.QueryModelList<V_DelegateSendModel>("select * from V_DelegateSendModel where sendNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "delegatelist", ForController = "delegate")]
        public ActionResult delegatedetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_DelegateSendDetailModel>("select * from V_DelegateSendDetailModel where sendNo='" + no + "'");

            //var list = bomService.RequireDetailList(no);
            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "delegatelist", ForController = "delegate")]
        public ActionResult delegatedelete()
        {
            string no = WebRequest.GetString("no", true);
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateSendDetail where sendno='" + no + "'");
            var r2 = ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateSend where sendno='" + no + "'");
            return Content(r2 == 1 ? "ok" : "error");
        }
        #endregion

        #region 创建委外单发货

        public ActionResult requireorder()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_DelegateSendModel();
            List<V_DelegateSendDetailModel> list = new List<V_DelegateSendDetailModel>();
            var message = "";
            var order = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetDelegateSendNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_DelegateSendModel>("select * from V_DelegateSendModel where sendNo='" + no + "'");
            }
            data.no = no;
            data.one = model;
            #region 默认申请单
            //var had = purchaseService.GetCurchaseHadOrder(no);
            //if (had != null && had.Count > 0) order = had[0].text;
            //else
            //{
            //    var all = purchaseService.GetNeedBuyOrder("");
            //    if (all != null && all.Count > 0) order = all[0].text;
            //}
            list = ServiceDB.Instance.QueryModelList<V_DelegateSendDetailModel>("select * from V_DelegateSendDetailModel where sendNo='" + no + "'").ToList();
            SessionHelper.SetSession("DS" + no + Masterpage.CurrUser.staffid, list);
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
            var had = ServiceDB.Instance.QueryModelList<string>("select distinct  delegateNo+'_' from V_DelegateSendDetailModel where sendNo='" + no + "'");
            string where = "";
            if (query!=null&&query!="")
            {
             where = " and ( materialNo like '%" + query + "%' or bomOrderNo like '%" + query + "%' or delegateNo like '%" + query + "%')";
            }
            var all = ServiceDB.Instance.QueryModelList<string>("select  distinct delegateNo from DelegateOrder where isover=0 and isclose=0 and status=1 and valid=1 and amount>giveAmount " + where);
            #region 合并
            if (all != null && had != null)
            {
                foreach (var item in all)
                {
                    if (!(had.Contains(item) || had.Contains(item+"_"))) had.Add(item);
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
            var list = new List<V_DelegateSendDetailModel>();
            list = (List<V_DelegateSendDetailModel>)SessionHelper.GetSession("DS" + no + Masterpage.CurrUser.staffid);
            data.list = list;
            return PartialView(data);
        }


        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult needbuytab(string order, int supplier, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var gate = ServiceDB.Instance.QueryOneModel<V_DelegateOrderModel>("select * from V_DelegateOrderModel where delegateNo='" + order + "'");
            var gatehad = ServiceDB.Instance.QueryOneModel<DelegateSendDetail>("select top 1 * from DelegateSendDetail where delegateNo='" + order + "' and sendNo='" + no + "'");
            List<SelectListItem> price = new List<SelectListItem>();
            if (supplier > 0)
            {
                #region 价格选择
                price = ServiceDB.Instance.QueryModelList<SelectListItem>("select convert(varchar,price) as Value,convert(varchar,price)+'备注:'+remark as Text,convert(bit,1) as Selected from MaterialPrice where supplierId="
                      + supplier + " and materialNo='"+ gate.productNo 
                      + "' and status=1 and startDate<=getdate() and (endDate is null or endDate>=getdate())").ToList();

                #endregion
            }
            var had = ServiceDB.Instance.QueryModelList<V_DelegateSendDetailModel>("select * from V_DelegateSendDetailModel where delegateNo='" + order + "' and sendNo='" + no + "'").ToList();
            var orderdetail = ServiceDB.Instance.QueryModelList<V_DelegateOrderDetail>("select * from V_DelegateOrderDetail where delegateNo='" + order + "'").ToList();// purchaseService.RequireDetailList(order);
            var boms = ServiceDB.Instance.QueryModelList<BomMain>("select * from bommain where parent_Id=" + (gate != null ? gate.bomId : 0));
            data.no = no;
            data.order = order;
            data.price = price;

            data.gate = gate;
            data.boms = boms;
            data.gatehad = gatehad;
            data.had = had;
            data.orderdetail = orderdetail;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult queryorder(string key)
        {
            string where = "";
            if (key != null && key != "")
            {
                where = " and ( materialNo like '%" + key + "%' or bomOrderNo like '%" + key + "%' or delegateNo like '%" + key + "%')";
            }
            var all = ServiceDB.Instance.QueryModelList<string>("select  distinct delegateNo from DelegateOrder where isover=0 and isclose=0 and status=1 and valid=1 and amount>giveAmount " + where).Select(x => new { value = x, key = x }); ;

            return Json(all, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult saverequirebuytemp(string order, string no,
          decimal sendamount, decimal price, string sendmaterial,
            string details, string orderdetails, string realcounts, string jscounts, string models, string bomids, string remarks)
        {
            //order: order, no: no,
            //sendamount: sendamount, price: price, sendmaterial: mno,
            //details: details.toString(), orderdetails: orderdetails.toString(),
            //realcounts: realcounts.toString(), jscounts: jscounts.toString(), models: models.toString(), remarks

            var list = new List<V_DelegateSendDetailModel>();
            list = (List<V_DelegateSendDetailModel>)SessionHelper.GetSession("DS" + no + Masterpage.CurrUser.staffid);
            string[] ds = details.Split(',');
            string[] ods = orderdetails.Split(',');
            string[] rc = realcounts.Split(',');
            string[] jc = jscounts.Split(',');
            string[] ms = models.Split(',');
            string[] bs = bomids.Split(',');
            string[] rs = remarks.Split(',');
            var had = list.Where(p => p.delegateNo == order).ToList();
            if (had != null && had.Count > 0)
            {
                foreach (var item in had) { list.Remove(item); }
            }
            for (int i = 0; i < ds.Length; i++)
            {
                decimal fc = Convert.ToDecimal(rc[i]);
                decimal pc = Convert.ToDecimal(jc[i]);

                int dsn = Convert.ToInt32(ds[i]);
                int odsn = Convert.ToInt32(ods[i]);
                int bomid = Convert.ToInt32(bs[i]);


                #region 添加

                var material = ServiceDB.Instance.QueryOneModel<Material>("select * from Material where materialNo='" + ms[i] + "'");
                var pm = ServiceDB.Instance.QueryOneModel<Material>("select * from Material where materialNo='" + sendmaterial + "'");
                list.Add(new V_DelegateSendDetailModel
                {
                    backProduct = 0,
                    bomId = bomid,
                    detailSn = dsn,
                    delegateDetailSn = odsn,
                    delegateNo = order,
                    materialModel = material.materialModel,
                    materialName = material.materialName,
                    materialNo = material.materialNo,
                    price = price,
                    productAmount = sendamount,
                    productModel = pm.materialModel,
                    productName = pm.materialName,
                    productNo = pm.materialNo,
                    realAmount = fc,
                    theoryAmount = pc,
                    sendNo = no,
                    remark = rs[i],
                    productUnit = pm.unit,
                    unit = material.unit

                });
                #endregion
            }
            SessionHelper.SetSession("DS" + no + Masterpage.CurrUser.staffid, list);
            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "requireorder", ForController = "purchase")]
        public ActionResult deleteneedbuytemp(string no, int sn, string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<PurchaseDetailModel>();
            list = (List<PurchaseDetailModel>)SessionHelper.GetSession("DS" + no + Masterpage.CurrUser.staffid);

            var had = list.FirstOrDefault(p => p.requireDetailSn == sn && p.materialModel == m);
            if (had != null)
            {
                if (had.detailSn == 0) list.Remove(had);
                else had.type = "delete";
                SessionHelper.SetSession("DS" + no + Masterpage.CurrUser.staffid, list);
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
            //deport, datesend: datesend, dateback
            string deport = WebRequest.GetString("deport", true);
            string datesend = WebRequest.GetString("datesend", true);
            string dateback = WebRequest.GetString("dateback", true);
            ReturnValue r = new ReturnValue() { status = false };
            var list = new List<V_DelegateSendDetailModel>();
            list = (List<V_DelegateSendDetailModel>)SessionHelper.GetSession("DS" + no + Masterpage.CurrUser.staffid);
            if (list.Count == 0)
            {
                r = new ReturnValue { status = false, message = "未提交任何发货明细" };
            }
            else
            {
                var head = 0;
                if (type == "add")
                {
                    string backno = bomService.GetDelegateSendNo();// purchaseService.AddPurchase("", Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, i_supplier, 1, 0, remark);
                    if (backno != "")
                    {
                        head = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[DelegateSend] values('" + backno + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + "," + i_supplier
                               + ",'" + datesend + "','" + dateback + "',getdate(),'" + remark + "',0,1,1,0,0,'','" + deport + "')");
                   no=backno;
                    }
                }
                else if (type == "edit")
                {
                    head = ServiceDB.Instance.ExecuteSqlCommand("update [dbo].[DelegateSend] set sendDate='" + datesend + "',needDate='" + dateback + "',remark='" + remark + "',deportStaff='" + deport + "' where sendNo='" + no + "'");

                }
                if (head>0)
                {
                    #region 修改明细
                    if (type == "edit") ServiceDB.Instance.ExecuteSqlCommand("delete DelegateSendDetail where sendNo='" + no + "'");
                    head = 0;
                    List<string> delgno = new List<string>();
                    foreach (var item in list)
                    {
                        var addone = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[DelegateSendDetail] VALUES('" + no + "','" + item.delegateNo + "'," + item.productAmount + "," + item.price + ",0,'" + item.materialNo + "'," + item.bomId + "," + item.delegateDetailSn + "," + item.theoryAmount + "," + item.realAmount + ",'" + item.remark + "')");
                        if (!delgno.Contains(item.delegateNo))
                        {
                            var hadd = ServiceDB.Instance.QueryOneModel<DelegateOrder>("select * from DelegateOrder where delegateNo='" + item.delegateNo + "' and canfs=1");
                            if (hadd != null) ServiceDB.Instance.ExecuteSqlCommand("update DelegateOrder set canfs=0 where delegateNo='" + item.delegateNo + "'");
                            delgno.Add(item.delegateNo);
                        }
                        
                        head += addone;
                    }                    
                    #endregion
                    if (head == list.Count) r.status = true;
                    else r.message = "保存失败";
                }
            }
            SessionHelper.Del("DS" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region 创建委外收货

        public ActionResult backone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_DelegateBackModel();
            List<V_DelegateSendDetailModel> list = new List<V_DelegateSendDetailModel>();
            var message = "";
            var order = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetDelegateBackNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_DelegateBackModel>("select * from V_DelegateBackModel where backNo='" + no + "'");
            }
            data.no = no;
            data.one = model;
            
            data.order = order;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "backone", ForController = "delegate")]
        public ActionResult backonedetail(string backno,string sendno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var backmodel = ServiceDB.Instance.QueryOneModel<V_DelegateBackModel>("select * from V_DelegateBackModel where backNo='" + backno + "'");
            var backdetail = ServiceDB.Instance.QueryModelList<V_DelegateBackDetail>("select * from V_DelegateBackDetail where backNo='" + backno + "'").ToList();
            var senddetail = ServiceDB.Instance.QueryModelList<V_DelegateSendDetailModel>("select * from V_DelegateSendDetailModel where sendNo='" + sendno + "'").ToList();

            data.backmodel = backmodel;
            data.backdetail = backdetail;
            data.senddetail = senddetail;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "backone", ForController = "delegate")]
        public ActionResult savebackone(string no)
        {
            string type = WebRequest.GetString("type", true);
            string date = WebRequest.GetString("date", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            string sendno = WebRequest.GetString("sendno", true);

            string material = WebRequest.GetString("material", true);
            string product = WebRequest.GetString("product", true);
            string amount = WebRequest.GetString("amount", true);
            string remarks = WebRequest.GetString("remarks", true);
            string delegates = WebRequest.GetString("delegates", true);
            string[] listmaterial = material.Split(',');
            string[] listproduct = product.Split(',');
            string[] listamount = amount.Split(',');
            string[] listremarks = remarks.Split(',');
            string[] listdelegates = delegates.Split(',');
            ReturnValue r = new ReturnValue() { status = false };
            if (listmaterial.Length<1||listmaterial.Length!=listamount.Length)
            {
                r = new ReturnValue { status = false, message = "未提交任何收货明细" };
            }
            else
            {
                var head = 0;
                if (type == "add")
                {
                    string backno = bomService.GetDelegateBackNo();
                    if (backno != "")
                    {
                        head = ServiceDB.Instance.ExecuteSqlCommand(
                            "INSERT INTO [dbo].[DelegateBack] values('" + backno + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + ",'" + sendno
                               + "','" + date + "',getdate(),'" + remark + "',0,1,1,0,0,'','" + deport + "')");
                        no = backno;
                    }
                }
                else if (type == "edit")
                {
                    head = ServiceDB.Instance.ExecuteSqlCommand("update [dbo].[DelegateBack] set backDate='" + date + "',remark='" + remark + "',deportStaff='" + deport + "' where backNo='" + no + "'");

                }
                if (head > 0)
                {
                    #region 修改明细
                    if (type == "edit") ServiceDB.Instance.ExecuteSqlCommand("delete DelegateBackDetail where backNo='" + no + "'");
                    head = 0;
                    List<string> delgno = new List<string>();
                    for (int i = 0; i < listmaterial.Length; i++)
                    {
                        if (listmaterial[i] == "" || listamount[i] == "" || listamount[i] == "0") continue;
                        var addone = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[DelegateBackDetail] VALUES('" + no + "','" + listdelegates[i] + "'," + listproduct[i] + ",'" + listmaterial[i] + "'," + listamount[i] + ",'" + listremarks[i] + "')");
                        head += addone;
                    }
                    if (head < 1)
                    {
                        ServiceDB.Instance.ExecuteSqlCommand("delete DelegateBack where backNo='" + no + "'");
                    }
                    else
                    {
                        var hadd = ServiceDB.Instance.QueryOneModel<DelegateSend>("select * from DelegateSend where sendNo='" + sendno + "' and canfs=1");
                        if (hadd != null) ServiceDB.Instance.ExecuteSqlCommand("update DelegateSend set canfs=0 where sendNo='" + sendno + "'");
                    }
                    #endregion
                    r.status = true;
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }


        [AjaxAction(ForAction = "backone", ForController = "delegate")]
        public ActionResult getsupplierdelegate(int supplierid)
        {
          var list=  ServiceDB.Instance.QueryModelList<DelegateSend>("select * from DelegateSend where supplierId=" + supplierid + " and status=1 and valid=1 and isclose=0 and isover=0");
         var details = list.Select(x => new { sendno = x.sendNo }).ToList();
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 委外收货单列表

        public ActionResult backlist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "backlist", ForController = "delegate")]
        public ActionResult backlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = ServiceDB.Instance.QueryModelList<V_DelegateBackModel>("select * from V_DelegateBackModel where backNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "backlist", ForController = "delegate")]
        public ActionResult backdetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_DelegateBackDetail>("select * from V_DelegateBackDetail where backNo='" + no + "'");

            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "backlist", ForController = "delegate")]
        public ActionResult backdelete()
        {
            string no = WebRequest.GetString("no", true);
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateBackDetail where backNo='" + no + "'");
            var r2 = ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateBack where backNo='" + no + "'");
            return Content(r2 == 1 ? "ok" : "error");
        }
        #endregion

        #region 创建领料

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult pullone(string productno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_ProductPullModel();
            var product = new V_ProductionModel();
            var message = "";
            var order = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetProductPullNo();
                product = ServiceDB.Instance.QueryOneModel<V_ProductionModel>("select top 1 * from V_ProductionModel where produceNo='" + productno + "'");
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_ProductPullModel>("select * from V_ProductPullModel where pullNo='" + no + "'");
                product = ServiceDB.Instance.QueryOneModel<V_ProductionModel>("select top 1 * from V_ProductionModel where produceNo='" + model.produceNo + "'");
            }
            data.no = no;
            data.one = model;
            data.order = order;

            data.product = product;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult pullonedetail(string backno, string productno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var backmodel = ServiceDB.Instance.QueryOneModel<V_ProductPullModel>("select * from V_ProductPullModel where pullNo='" + backno + "'");
            var psdetail = ServiceDB.Instance.QueryModelList<V_ProductionDetailModel>("select * from V_ProductionDetailModel where produceNo='" + productno + "'").ToList();
            var pulldetail = ServiceDB.Instance.QueryModelList<V_ProductPullDetail>("select * from V_ProductPullDetail where pullNo='" + backno + "'").ToList();
            var product = ServiceDB.Instance.QueryOneModel<Production>("select * from Production where produceNo='" + productno + "'");
            var orderdetail = ServiceDB.Instance.QueryOneModel<BomOrderDetail>("select * from BomOrderDetail where detailSn=" + product.orderDetailSn.Value);
            var boms = ServiceDB.Instance.QueryModelList<BomMain>("select * from bommain where parent_Id=" + orderdetail.bomId).ToList();
            data.boms = boms;

            data.backmodel = backmodel;
            data.psdetail = psdetail;
            data.pulldetail = pulldetail;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult savepullone(string no)
        {
            string type = WebRequest.GetString("type", true);
            string backdate = WebRequest.GetString("backdate", true);
            string pulldate = WebRequest.GetString("pulldate", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            string productno = WebRequest.GetString("productno", true);
            string pullamount = WebRequest.GetString("pullamount", true);

            string material = WebRequest.GetString("material", true);
            string product = WebRequest.GetString("product", true);
            string amount = WebRequest.GetString("amount", true);
            string jsamount = WebRequest.GetString("jsamount", true);
            string remarks = WebRequest.GetString("remarks", true);
            string[] listmaterial = material.Split(',');
            string[] listproduct = product.Split(',');
            string[] listamount = amount.Split(',');
            string[] listremarks = remarks.Split(',');
            string[] listjsamount = jsamount.Split(',');
            ReturnValue r = new ReturnValue() { status = false };
            if (listmaterial.Length < 1 || listmaterial.Length != listamount.Length)
            {
                r = new ReturnValue { status = false, message = "未提交任何明细" };
            }
            else
            {
                var head = 0;
                if (type == "add")
                {
                    string backno = bomService.GetProductPullNo();
                    if (backno != "")
                    {
                        head = ServiceDB.Instance.ExecuteSqlCommand(
                            "INSERT INTO [dbo].[ProductPull] values('" + backno + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + ",'" + productno
                               + "'," + pullamount + ",'" + pulldate + "','" + backdate + "',0,1,1,0,0,'',null,'" + deport + "',getdate(),'" + remark + "')");
                        no = backno;
                    }
                }
                else if (type == "edit")
                {
                    head = ServiceDB.Instance.ExecuteSqlCommand("update [dbo].[ProductPull] set makeAmount=" + pullamount + ",backDate='" + backdate + "',pullDate='" + pulldate + "',remark='" + remark + "',deportStaff='" + deport + "' where pullNo='" + no + "'");

                }
                if (head > 0)
                {
                    #region 修改明细
                    if (type == "edit") ServiceDB.Instance.ExecuteSqlCommand("delete ProductPullDetail where pullNo='" + no + "'");
                    head = 0;
                    List<string> delgno = new List<string>();
                    for (int i = 0; i < listmaterial.Length; i++)
                    {
                        if (listmaterial[i] == "" || listamount[i] == "" || listamount[i] == "0") continue;
                        var addone = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[ProductPullDetail] VALUES('" + no + "'," + listproduct[i] + ",'" + listmaterial[i] + "'," + listjsamount[i] + "," + listamount[i] + ",'" + listremarks[i] + "')");
                        head += addone;
                    }
                    if (head < 1)
                    {
                        ServiceDB.Instance.ExecuteSqlCommand("delete ProductPull where pullNo='" + no + "'");
                    }
                    else
                    {
                        var hadd = ServiceDB.Instance.QueryOneModel<Production>("select * from Production where produceNo='" + productno + "' and  canfs=1");
                        if (hadd != null) ServiceDB.Instance.ExecuteSqlCommand("update Production set canfs=0 where produceNo='" + productno + "'");
                    }
                    #endregion
                    r.status = true;
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region 领料工单列表

        public ActionResult pulllist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult pulllistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = ServiceDB.Instance.QueryModelList<V_ProductPullModel>("select * from V_ProductPullModel where pullNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult pulldetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_ProductPullDetail>("select * from V_ProductPullDetail where pullNo='" + no + "'");

            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult pulldelete()
        {
            string no = WebRequest.GetString("no", true);
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from ProductPullDetail where pullNo='" + no + "'");
            var r2 = ServiceDB.Instance.ExecuteSqlCommand("delete from ProductPull where pullNo='" + no + "'");
            return Content(r2 == 1 ? "ok" : "error");
        }
        #endregion



        #region 创建领料

        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult giveone(string pullno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_ProductGiveModel();
            var pull = new V_ProductPullModel();
            var message = "";
            var order = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetProductGiveNo();
                pull = ServiceDB.Instance.QueryOneModel<V_ProductPullModel>("select top 1 * from V_ProductPullModel where pullNo='" + pullno + "'");
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_ProductGiveModel>("select * from V_ProductGiveModel where giveNo='" + no + "'");
                pull = ServiceDB.Instance.QueryOneModel<V_ProductPullModel>("select top 1 * from V_ProductPullModel where pullNo='" + model.pullNo + "'");
            }
            data.no = no;
            data.one = model;
            data.order = order;

            data.pull = pull;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult giveonedetail(string giveno, string pullno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var backmodel = ServiceDB.Instance.QueryOneModel<V_ProductGiveModel>("select * from V_ProductGiveModel where giveNo='" + giveno + "'");
            var psdetail = ServiceDB.Instance.QueryModelList<V_ProductPullDetail>("select * from V_ProductPullDetail where pullNo='" + pullno + "'").ToList();
            var givedetail = ServiceDB.Instance.QueryModelList<V_ProductGiveDetail>("select * from V_ProductGiveDetail where giveNo='" + giveno + "'").ToList();


            data.backmodel = backmodel;
            data.psdetail = psdetail;
            data.givedetail = givedetail;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "pulllist", ForController = "delegate")]
        public ActionResult savegiveone(string no)
        {
            string type = WebRequest.GetString("type", true);
            string givedate = WebRequest.GetString("givedate", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            string pullno = WebRequest.GetString("pullno", true);
            string giveamount = WebRequest.GetString("giveamount", true);

            string amount = WebRequest.GetString("amount", true);
            string pulldetail = WebRequest.GetString("pulldetail", true);
            string remarks = WebRequest.GetString("remarks", true);
            string[] listpull = pulldetail.Split(',');
            string[] listamount = amount.Split(',');
            string[] listremarks = remarks.Split(',');
            ReturnValue r = new ReturnValue() { status = false };
            if (listpull.Length < 1 || listpull.Length != listamount.Length)
            {
                r = new ReturnValue { status = false, message = "未提交任何明细" };
            }
            else
            {
                var head = 0;
                if (type == "add")
                {
                    string backno = bomService.GetProductGiveNo();
                    if (backno != "")
                    {
                        head = ServiceDB.Instance.ExecuteSqlCommand(
                            "INSERT INTO [dbo].[ProductGive] values('" + backno + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + ",'" + pullno
                               + "'," + giveamount + ",'" + givedate + "',0,1,1,0,0,'',null,'" + deport + "',getdate(),'" + remark + "')");
                        no = backno;
                    }
                }
                else if (type == "edit")
                {
                    head = ServiceDB.Instance.ExecuteSqlCommand("update [dbo].[ProductGive] set giveAmount=" + giveamount + ",giveDate='" + givedate + "',remark='" + remark + "',deportStaff='" + deport + "' where giveNo='" + no + "'");

                }
                if (head > 0)
                {
                    #region 修改明细
                    if (type == "edit") ServiceDB.Instance.ExecuteSqlCommand("delete ProductGiveDetail where giveNo='" + no + "'");
                    head = 0;
                    List<string> delgno = new List<string>();
                    for (int i = 0; i < listpull.Length; i++)
                    {
                        if (listpull[i] == "" || listamount[i] == "" || listpull[i] == "0" || listamount[i] == "0") continue;
                        var addone = ServiceDB.Instance.ExecuteSqlCommand("INSERT INTO [dbo].[ProductGiveDetail] VALUES('" + no + "'," + listpull[i] + "," + listamount[i] + ",'" + listremarks[i] + "')");
                        head += addone;
                    }
                    //if (head < 1)
                    //{
                    //    ServiceDB.Instance.ExecuteSqlCommand("delete ProductGive where giveNo='" + no + "'");
                    //}
                    //else
                    //{
                    var hadd = ServiceDB.Instance.QueryOneModel<ProductPull>("select * from ProductPull where pullNo='" + pullno + "' and  canfs=1");
                    if (hadd != null) ServiceDB.Instance.ExecuteSqlCommand("update ProductPull set canfs=0 where pullNo='" + pullno + "'");
                    //}
                    #endregion
                    r.status = true;
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region 领料工单列表

        public ActionResult givelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "givelist", ForController = "delegate")]
        public ActionResult givelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = ServiceDB.Instance.QueryModelList<V_ProductGiveModel>("select * from V_ProductGiveModel where giveNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "givelist", ForController = "delegate")]
        public ActionResult givedetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_ProductGiveDetail>("select * from V_ProductGiveDetail where giveNo='" + no + "'");

            data.no = no;
            data.list = list;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "givelist", ForController = "delegate")]
        public ActionResult givedelete()
        {
            string no = WebRequest.GetString("no", true);
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from ProductGiveDetail where giveNo='" + no + "'");
            var r2 = ServiceDB.Instance.ExecuteSqlCommand("delete from ProductGive where giveNo='" + no + "'");
            return Content(r2 == 1 ? "ok" : "error");
        }
        #endregion
    }
}
