using ecoBio.Wms.Common;
using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Entities;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Service;
using Enterprise.Invoicing.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class mainController : BaseController
    {
        private AccountService accountService;
        private PurchaseService purchaseService;
        private StockInService stockinService;
        private StockOutService stockoutService;
        private ManageService manageService;
        private BomService bomService;
        public mainController(IAccountRepository _accountRepository, IManageRepository _manageRepository, IPurchaseRepository _purchaseRepository, IStockInRepository _stockinrepository, IStockOutRepository _stockoutrepository, IBomRepository _bomrepository)
        {
            accountService = new AccountService(_accountRepository);
            purchaseService = new PurchaseService(_purchaseRepository);
            stockinService = new StockInService(_stockinrepository);
            stockoutService = new StockOutService(_stockoutrepository);
            manageService = new ManageService(_manageRepository);
            bomService = new BomService(_bomrepository);
        }
        [LoginAllow]
        public ActionResult Index()
        {
            var sid = SessionHelper.SessionId;
            var name = Masterpage.CurrUser.name;
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = accountService.GetMenuByRole(Masterpage.CurrUser.role_sn);//
            data.list = list;

            var require = purchaseService.RequireList("").Where(p => p.status == 0).ToList().Count;
            var order = purchaseService.PurchaseList("").Where(p => p.status == 0).ToList().Count;
            var sin = stockinService.StockInList("").Where(p => p.status == 0).ToList().Count;
            var sout = stockoutService.StockOutList("").Where(p => p.status == 0).ToList().Count;
            var oret = stockinService.ReturnModelList("").Where(p => p.status == 0).ToList().Count;
            var sret = stockoutService.ReturnModelList("").Where(p => p.status == 0).ToList().Count;
            var o2n = stockoutService.ChangeOtNList("").Where(p => p.status == 0).ToList().Count;
            var bomorder = bomService.BomOrderList("").Where(p => p.status == 0).ToList().Count;

            data.require = require;
            data.order = order;
            data.sin = sin;
            data.sout = sout;
            data.oret = oret;
            data.sret = sret;
            data.o2n = o2n;
            data.bomorder = bomorder;

            return View(data);
        }

        [LoginAllow]
        public ActionResult mainmenu()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = accountService.GetMenuByRole(Masterpage.CurrUser.role_sn);//
            data.list = list;
            return PartialView(data);
        }

        [LoginAllow]
        public ActionResult myinfo()
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var info = Masterpage.CurrUser;
            data.info = info;
            return View(data);
        }
        [LoginAllow]
        public ActionResult uppwd(string oldpwd, string newpwd)
        {
            ReturnValue r = new ReturnValue();
            if (newpwd.Length < 6 || newpwd.Length > 20)
            {
                r = new ReturnValue { message = "新密码长度需为6到12位", status = false };
            }
            else
            {
                r = accountService.UpdatePwd(Masterpage.CurrUser.staffid, MD5Helper.MD5_32(oldpwd), MD5Helper.MD5_32(newpwd));
            }
            return Json(r);
        }

        [LoginAllow]
        public ActionResult materialcate1(string select)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = manageService.GetFirstCate().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var had = list.FirstOrDefault(x => x.Value == select);
            if (had != null) had.Selected = true;
            data.list = list;
            return PartialView(data);
        }
        [LoginAllow]
        public ActionResult materialcate2(string cate, string select)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            cate = Server.UrlDecode(cate);
            var list = manageService.GetSecondCate(cate).Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var had = list.FirstOrDefault(x => x.Value == select);
            if (had != null) had.Selected = true;
            data.list = list;
            data.select = select;
            return PartialView(data);
        }

        #region 审核

        [LoginAllow]
        public ActionResult checkdata(string key, string no, int status)
        {
            ReturnValue back = new ReturnValue { status = true };
            if (key == "require")
            {
                #region 采购申请
                back.status = purchaseService.ChangeRequireStatus(no, status, Masterpage.CurrUser.name);
                #endregion
            }
            else if (key == "purchase")
            {
                back.status = purchaseService.ChangePurchaseStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "stockin")
            {
                back.status = stockinService.ChangeStockInStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "stockout")
            {
                back = stockoutService.ChangeStockOutStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "stockchange")
            {
                back = stockoutService.ChangeChangeOtNStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "orderreturn")
            {
                back = stockinService.ChangeReturnStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "stockreturn")
            {
                back = stockoutService.ChangeStockReturnStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "bomorder")
            {
                back = bomService.ChangeBomOrderStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "bill")
            {
                back.status = manageService.ChangeBillStatus(no, status, Masterpage.CurrUser.staffid);
            }
            else if (key == "delegate")
            {
                back = bomService.ChangeDelegateStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "produce")
            {
                back = bomService.ChangeProductionStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "delegatesend")
            {
                back = bomService.ChangeDelegateSendStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "delegateback")
            {
                back = bomService.ChangeDelegateBackStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "producepull")
            {
                back = bomService.ChangeProductPullStatus(no, status, Masterpage.CurrUser.name);
            }
            else if (key == "producegive")
            {
                back = bomService.ChangeProductGiveStatus(no, status, Masterpage.CurrUser.name);
            }
            return Json(back, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 打印
        [LoginAllow]
        public ActionResult printwin(string title, string key, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            PrintHeadModel pm = new PrintHeadModel();
            List<string> datas = new List<string>();
            bool showPrice = false;
            #region 分支
            switch (key)
            {
                case "require":
                    if (Masterpage.CheckRight("require_print"))
                    {      pm = purchaseService.RequireList(no).Where(p => p.requireNo == no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.requireNo, showdeport = false }).FirstOrDefault();
                    datas = purchaseService.RequireDetailList(no).Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.orderAmount.ToString("N") + "," + x.materialUnit + "," + (x.needdate < DateTime.Now.AddYears(5) ? x.needdate.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                   
                    }
                    break;
                case "delegate":
                    if (Masterpage.CheckRight("delegate_print"))
                    {
                        var one = ServiceDB.Instance.QueryOneModel<V_DelegateOrderModel>(" select * from V_DelegateOrderModel where delegateNo='" + no + "'");
                        pm = new PrintHeadModel { checkStaff = one.checkStaff, date = one.createDate, depName = "订单：" + one.bomOrderNo, makeStaff = one.staffName, No = one.delegateNo, showdeport = true, deportStaff ="产品："+ one.productNo + " " + one.productName + " " + one.productModel };
                        datas = ServiceDB.Instance.QueryModelList<V_DelegateOrderDetail>(" select * from V_DelegateOrderDetail where delegateNo='" + no + "'").ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.detailAmount.Value.ToString("N") + "," + x.unit + "," + x.detailRemark).ToList();
                    }
                    break;
                case "produce":
                    if (Masterpage.CheckRight("produce_print"))
                    {
                        var one = ServiceDB.Instance.QueryOneModel<V_ProductionModel>(" select * from V_ProductionModel where produceNo='" + no + "'");
                        pm = new PrintHeadModel { checkStaff = one.checkStaff, date = one.createDate.Value, depName = "订单：" + one.bomOrderNo, makeStaff = one.staffName, No = one.produceNo, showdeport = true, deportStaff ="产品："+ one.productNo + " " + one.productName + " " + one.productModel };
                        datas = ServiceDB.Instance.QueryModelList<V_ProductionDetailModel>(" select * from V_ProductionDetailModel where produceNo='" + no + "'").ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.amount.Value.ToString("N") + "," + x.outAmount.Value.ToString("N") + "," + x.unit + "," + x.remark).ToList();
                    }
                    break;
                case "producepull":
                    if (Masterpage.CheckRight("producepull_print"))
                    {
                        var one = ServiceDB.Instance.QueryOneModel<V_ProductPullModel>(" select * from V_ProductPullModel where pullNo='" + no + "'");
                        pm = new PrintHeadModel { checkStaff = one.checkStaff, date = one.createDate, depName = "工单：" + one.produceNo, makeStaff = one.staffName, No = one.pullNo, showdeport = true, deportStaff = "产品：" + one.materialNo + " " + one.materialName + " " + one.materialModel };
                        datas = ServiceDB.Instance.QueryModelList<V_ProductPullDetail>(" select * from V_ProductPullDetail where pullNo='" + no + "'").ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.theoryAmount.ToString("N") + "," + x.realAmount.ToString("N") + "," + x.unit + "," + x.remark).ToList();
                    }
                    break;
                case "producegive":
                    if (Masterpage.CheckRight("producegive_print"))
                    {
                        var one = ServiceDB.Instance.QueryOneModel<V_ProductGiveModel>(" select * from V_ProductGiveModel where giveNo='" + no + "'");
                        pm = new PrintHeadModel { checkStaff = one.checkStaff, date = one.createDate, depName = "工单：" + one.pullNo, makeStaff = one.staffName, No = one.giveNo, showdeport = true, deportStaff = "产品：" + one.materialNo + " " + one.materialName + " " + one.materialModel+" 交货数："+one.giveAmount };
                        datas = ServiceDB.Instance.QueryModelList<V_ProductGiveDetail>(" select * from V_ProductGiveDetail where giveNo='" + no + "'").ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.realAmount.ToString("N") + "," + x.giveAmount.ToString("N") + "," + x.unit + "," + x.remark).ToList();
                    }
                    break;
                case "order":
                    if (Masterpage.CheckRight("order_print"))
                    {
                        pm = purchaseService.PurchaseList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "供应商：" + x.suppliername, makeStaff = x.staffName, No = x.purchaseNo, showdeport = false }).FirstOrDefault();
                        if (Masterpage.CurrUser.showPrice) datas = purchaseService.PurchaseDetailList(no).Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.poAmount + "," + x.materialUnit + "," + x.poPrice + "," + (x.poAmount * x.poPrice) + "," + (x.sendDate < DateTime.Now.AddYears(5) ? x.sendDate.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                        else datas = purchaseService.PurchaseDetailList(no).Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.poAmount + "," + x.materialUnit + "," + (x.sendDate < DateTime.Now.AddYears(5) ? x.sendDate.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                    }
                    break;
                case "stockin":
                    if (Masterpage.CheckRight("stockin_print"))
                    {
                        pm = stockinService.StockInList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.stockNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        datas = stockinService.StockInDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.materialUnit + "," + x.depotName + "," + x.purchaseNo + "," + x.remark).ToList();
                    }
                    break;
                case "stockout":
                    if (Masterpage.CheckRight("stockout_print"))
                    {
                        pm = stockoutService.StockOutList(no).Where(p => p.datatype != 1).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.stockNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        datas = stockoutService.StockOutDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.materialUnit + "," + x.depotName + "," + x.remark).ToList();
                    }
                    break;
                case "change":
                    if (Masterpage.CheckRight("stockout_old2new_print"))
                    {
                        pm = stockoutService.ChangeOtNList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.stockNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        List<string> l1 = new List<string>(); List<string> l2 = new List<string>();
                        l1 = stockoutService.ChangeOtNInList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.materialUnit + ",入库," + x.depotName + "," + x.remark).ToList();
                        l2 = stockoutService.ChangeOtNOutList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.materialUnit + ",出库," + x.depotName + "," + x.remark).ToList();
                        datas.AddRange(l1);
                        datas.AddRange(l2);
                    }
                    break;
                case "orderreturn":
                    if (Masterpage.CheckRight("stockin_return_print"))
                    {
                        pm = stockinService.ReturnModelList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "供应商：" + x.supplierName, makeStaff = x.staffName, No = x.returnNo, showdeport = true, deportStaff = "供应商：" }).FirstOrDefault();
                        datas = stockinService.ReturnDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.returnAmount + "," + x.materialUnit + "," + x.purchaseNo + "," + x.depotName + "," + x.remark).ToList();
                    }
                    break;
                case "stockreturn":
                    if (Masterpage.CheckRight("stockout_return_print"))
                    {
                        pm = stockoutService.ReturnModelList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.returnNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        datas = stockoutService.StockReturnDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.outAmoutn + "," + x.returnAmount + "," + x.materialUnit + "," + x.stockoutNo + "," + x.todepotName + "," + x.remark).ToList();
                    }
                    break;
                case "bomorder":
                    if (Masterpage.CheckRight("bomorder_print"))
                    {
                        showPrice = Masterpage.CheckRight("ordermoney_print");
                        pm = bomService.BomOrderList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.bomOrderNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        if (showPrice) datas = bomService.BomOrderDetailList(no).ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.Amount + "," + x.Price.ToString("N").Replace(",", "") + "," + (x.Amount * x.Price).ToString("N").Replace(",", "") + "," + (x.sendDate.HasValue ? x.sendDate.Value.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();// + x.Price.ToString("N").Replace(",", "") + "," + (x.Amount * x.Price).ToString("N").Replace(",", "") + ","
                        else datas = bomService.BomOrderDetailList(no).ToList().Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.Amount + "," + (x.sendDate.HasValue ? x.sendDate.Value.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();// + x.Price.ToString("N").Replace(",", "") + "," + (x.Amount * x.Price).ToString("N").Replace(",", "") + ","
                    }
                    break;
                case "bomorderdetail":
                    if (Masterpage.CheckRight("bomorder_print"))
                    {
                        var sn = Convert.ToInt32(no);
                        var detail = bomService.BomOrderDetailOne(sn);
                        pm = bomService.BomOrderList(detail.bomOrderNo).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = "部门：" + x.depName, makeStaff = x.staffName, No = x.bomOrderNo, showdeport = true, deportStaff = "仓库：" + x.deportStaff }).FirstOrDefault();
                        datas = bomService.GetBomOrderBomDetailList(sn).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.materialCate + "," + x.bomAmount + "," + (x.sendDate.HasValue ? x.sendDate.Value.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                    }
                    break;
                case "bill":
                    if (Masterpage.CheckRight("bill_print"))
                    {

                        var model = ServiceDB.Instance.QueryOneModel<V_BillCost>(" select * from V_BillCost where billNo='" + no + "'");
                        var list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail where billNo='" + no + "'").ToList();
                        pm = new PrintHeadModel { checkStaff = model.checkName, makeStaff = model.makeName, bossStaff = model.bossName, cfoStaff = model.cfoName, date = model.createDate, depName = "部门：" + model.depName, No = model.billNo, showdeport = false };
                        datas = list.Select(x => x.billTitle + "," + x.cost + "," + x.billDate.ToString("yyyy-MM-dd") + "," + x.remark).ToList();
                    }
                    break;
                case "purchase2":
                    if (Masterpage.CheckRight("purchase2_print"))
                    {

                        var model = ServiceDB.Instance.QueryOneModel<V_BillCost>(" select * from V_BillCost where billNo='" + no + "'");
                        var list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail where billNo='" + no + "'").ToList();
                        pm = new PrintHeadModel { checkStaff = model.checkName, makeStaff = model.makeName, bossStaff = model.bossName, cfoStaff = model.cfoName, date = model.createDate, depName = "部门：" + model.depName, No = model.billNo, showdeport = false };
                        datas = list.Select(x => x.billTitle + "," + x.amount + "," + x.cost + "," + x.cost * x.amount + "," + x.billDate.ToString("yyyy-MM-dd") + "," + x.remark).ToList();
                    }
                    break;
                case "delegateback":
                    if (Masterpage.CheckRight("delegateback_print"))
                    {

                        var model = ServiceDB.Instance.QueryOneModel<V_DelegateBackModel>(" select * from V_DelegateBackModel where backNo='" + no + "'");
                        var list = ServiceDB.Instance.QueryModelList<V_DelegateBackDetail>(" select * from V_DelegateBackDetail where backNo='" + no + "'").ToList();
                        pm = new PrintHeadModel { checkStaff = model.checkStaff, date = model.backDate, depName = "供应商：" + model.supplierName, makeStaff = model.staffName, No = model.backNo, showdeport = true, deportStaff = "仓库：" + model.deportStaff, bossStaff=model.supplierName };
                        datas = list.Select(x => (x.isProduct ? "货品" : "原料") + "," + x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.backAmount + "," + x.fromDelegateNo + "," + x.remark).ToList();
                    }
                    break;
                case "stocksemi":
                    if (Masterpage.CheckRight("stockin_semi_print"))
                    {

                        showPrice = Masterpage.CheckRight("semishowmoney_print");
                        var model = ServiceDB.Instance.QueryOneModel<ProductSemi>(" select * from ProductSemi where semiNo='" + no + "'");
                        var list = ServiceDB.Instance.QueryModelList<ProductSemi>(" select * from ProductSemi where semiNo='" + no + "'").ToList();
                        var dep = ServiceDB.Instance.QueryOneModel<Department>("select d.* from Department as d inner join Employee as e on d.depId=e.depId where e.staffId=" + model.staffId);
                        pm = new PrintHeadModel { checkStaff = model.checkStaff, makeStaff = model.staffName, bossStaff = "", cfoStaff = "", date = model.createDate, depName = dep != null ? "部门：" + dep.depName : "", No = model.semiNo, showdeport = false };
                        datas = list.Select(x => x.proName + "," + x.proModel + "," + x.amount + "," + (showPrice?x.price.ToString():"") + "," + (showPrice?Math.Round((decimal)x.amount * x.price, 4).ToString():"") + "," + x.createDate.ToString("yyyy-MM-dd") + "," + x.remark).ToList();
                    }
                    break;
                case "delegatesend":
                    if (Masterpage.CheckRight("delegate_send_print"))
                    {

                        showPrice = Masterpage.CheckRight("delegatesendshowmoney_print");
                        var model = ServiceDB.Instance.QueryOneModel<V_DelegateSendModel>(" select * from V_DelegateSendModel where sendNo='" + no + "'");
                        var list = ServiceDB.Instance.QueryModelList<V_DelegateSendDetailModel>(" select * from V_DelegateSendDetailModel where sendNo='" + no + "'").ToList();
                        //pm = new PrintHeadModel { checkStaff = model.checkStaff, makeStaff = model.staffName, bossStaff = "", cfoStaff = "", date = model.createDate, depName = model.depName, No = model.sendNo, showdeport = true };
                        //datas = list.Select(x => x.materialNo + "," + x.materialName + "," + x.materialModel + "," + x.realAmount + "," + (showPrice ? x.price.ToString() : "") + "," + (showPrice ? Math.Round((decimal)x.productAmount * x.price, 4).ToString() : "") + "," + x.remark).ToList();
                        var delegatenos=ServiceDB.Instance.QueryModelList<string>("select distinct delegateNo from DelegateSendDetail where sendNo='" + no + "' ").ToList();

                        data.showPrice = showPrice;
                        data.delegatenos = delegatenos;
                        data.title = title;
                        data.pm = model;
                        data.list = list;
                        return View("~/Views/main/delegatesendprint.cshtml", data);
                    }
                    break;
                default:
                    break;
            }
            #endregion

            data.showPrice = showPrice;
            data.title = title;
            data.key = key;
            data.pm = pm;
            data.datas = datas;

            return View(data);
        }

        [LoginAllow]
        public ActionResult printa4(string title, string key, string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            PrintHeadModel pm = new PrintHeadModel();
            List<string> datas = new List<string>();
            #region 分支
            switch (key)
            {
                case "order":
                    if (Masterpage.CheckRight("order_print"))
                    {
                        pm = purchaseService.PurchaseList(no).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = x.suppliername, makeStaff = x.staffName, No = x.purchaseNo }).FirstOrDefault();
                        if (Enterprise.Invoicing.Web.Masterpage.CheckRight("purchasemoney_print")) datas = purchaseService.PurchaseDetailList(no).Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.poAmount + "," + x.materialUnit + "," + x.poPrice.ToString().Replace(",", "") + "," + (x.poAmount * x.poPrice).ToString().Replace(",", "") + "," + (x.sendDate < DateTime.Now.AddYears(5) ? x.sendDate.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                        else datas = purchaseService.PurchaseDetailList(no).Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.poAmount + "," + x.materialUnit + "," + (x.sendDate < DateTime.Now.AddYears(5) ? x.sendDate.ToString("yyyy-MM-dd") : "") + "," + x.remark).ToList();
                    }
                    break;
                default:
                    break;
            }
            #endregion
            data.key = key;
            data.showPrice = Enterprise.Invoicing.Web.Masterpage.CheckRight("purchasemoney_print");
            data.title = title;
            data.pm = pm;
            data.datas = datas;

            return View(data);
        }
        [LoginAllow]
        public ActionResult printsale(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            PrintHeadModel pm = new PrintHeadModel();
            Supplier sup = new Supplier();
            List<string> datas = new List<string>();
            #region 分支
            if (Masterpage.CheckRight("stockout_print"))
            {
                pm = stockoutService.StockOutList(no).Where(p => p.datatype == 1 || p.datatype == 4).Select(x => new PrintHeadModel { checkStaff = x.checkStaff, date = x.createDate, depName = x.supplierName, makeStaff = x.staffName, No = x.stockNo, showdeport = true, deportStaff = x.deportStaff, supplierid = x.supplierId }).FirstOrDefault();
                sup = manageService.GetSupplierList("").FirstOrDefault(p => p.supplierId == pm.supplierid);
                if (Masterpage.CheckRight("sellmoney_print")) datas = stockoutService.StockOutDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.price.ToString("N").Replace(",", "") + "," + (x.amount * x.price).ToString("N").Replace(",", "") + "," + x.remark).ToList();
                else datas = stockoutService.StockOutDetailList(no).ToList().Select(x => x.materialNo + "<br>" + x.materialName + "," + x.materialModel + "," + x.amount + "," + x.remark).ToList();
            }
            #endregion
            data.pm = pm;
            data.sup = sup;
            data.showPrice = Masterpage.CheckRight("sellmoney_print");
            data.datas = datas;

            return View(data);
        }
        #endregion

        #region 物料查询
        [LoginAllow]
        public ActionResult querymaterial(string key, string mc,string bigcate)
        {
            var all = purchaseService.QueryMaterial(1);

            if (key != null && key != "")
            {
                key = key.ToLower();
                var r = all.Where(p => p.materialNo.Contains(key) || p.materialName.Contains(key) || p.materialModel.Contains(key) || p.tunumber.Contains(key) || p.fastcode.Contains(key) || p.pinyin.Contains(key))
                    .Select(x => new { code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, mc = x.category,cate=x.bigcate }).ToList();
                if (mc != null && mc != "") r = r.Where(p => p.mc == mc).ToList();
                if (bigcate != null && bigcate != "") r = r.Where(p => p.cate == bigcate).ToList();
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            // var r = all.Select(x => new { value = x.text.Replace(Masterpage.CurrUser.client_code + "-", ""), key = x.text.Replace(Masterpage.CurrUser.client_code + "-", "") });
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult querychildbom(string key)
        {
            var all = bomService.GetBomMaterial();

            if (key != null && key != "")
            {
                key = key.ToLower();
                var r = all.Where(p => (p.bomName.ToLower().Contains(key) || p.remark.ToLower().Contains(key) || p.version.Contains(key)) && p.isChild == true && p.parent_Id.HasValue == false && p.status == 1)
                    .Select(x => new { id = x.bomId, name = x.bomName, version = x.version, remark = x.remark }).ToList();
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            // var r = all.Select(x => new { value = x.text.Replace(Masterpage.CurrUser.client_code + "-", ""), key = x.text.Replace(Masterpage.CurrUser.client_code + "-", "") });
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult querybommaterial(string key, int sn)
        {
           var all =    bomService.GetBomOrderBomDetailList(sn);
           if (key != null && key != "")
           {
               key = key.ToLower();
               var r = all.Where(p => (p.materialNo.Contains(key) || p.materialName.Contains(key) || p.materialModel.Contains(key) || p.tunumber.Contains(key) || p.fastcode.Contains(key) || p.pinyin.Contains(key)) && (p.materialCate != "" && p.materialCate != null))
                   .Select(x => new { bomid = x.bomId, code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, mc = x.materialCate }).ToList();
               return Json(r, JsonRequestBehavior.AllowGet);
           }
            // var r = all.Select(x => new { value = x.text.Replace(Masterpage.CurrUser.client_code + "-", ""), key = x.text.Replace(Masterpage.CurrUser.client_code + "-", "") });
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult querydepotmaterial(string key, int depotid)
        {
            var all = stockoutService.GetDepotMaterial(depotid, 1);

            if (key != null && key != "")
            {
                var r = all.Where(p => p.materialNo.Contains(key) || p.materialName.Contains(key) || p.tunumber.Contains(key) || p.materialModel.Contains(key))
                    .Select(x => new { code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, mc = x.category }).ToList();
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult querybom(string key, int supplierid)
        {
            key = key.ToLower();
            //var all = bomService.GetBomMaterial(key).Where(p => p.parent_Id == null);

            //var r = all.Select(x => new { bomid = x.bomId, code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, mc = x.category }).ToList();

           var list= ServiceDB.Instance.QueryModelList<V_BomProduct>("select * from V_BomProduct where supplierid=" + supplierid + " and ("
                + " materialNo like '%" + key + "%' or materialName like '%" + key + "%' or materialModel like '%" + key + "%' or tunumber like '%" + key + "%' or pinyin like '%" + key + "%'"
                +")");
           var r = list.Select(x => new { bomid = x.bomId, code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, price = x.price });

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult querybomprice(string key, int supplierid)
        {
            key = key.ToLower();
            var list = ServiceDB.Instance.QueryModelList<V_BomProduct>("select * from V_BomProduct where status=1 and supplierid=" + supplierid + " and ("
                 + " materialNo like '%" + key + "%' or materialName like '%" + key + "%' or materialModel like '%" + key + "%' or tunumber like '%" + key + "%' or pinyin like '%" + key + "%' or remark like '%" + key + "%'"
                 + ")");
            var r = list.Select(x => new { bomid = x.bomId, code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, price = x.price,remark=x.remark,version=x.version });

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [LoginAllow]
        public ActionResult querymaterialprice(string key, int supplierid)
        {
            key = key.ToLower();

            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where status=1 and supplierid=" + supplierid + " and ("
                 + " materialNo like '%" + key + "%' or materialName like '%" + key + "%' or materialModel like '%" + key + "%' or tunumber like '%" + key + "%' or pinyin like '%" + key + "%' or remark like '%" + key + "%'"
                 + ")");
            var r = list.Select(x => new {  code = x.materialNo, name = x.materialName, model = x.materialModel, tu = x.tunumber, price = x.price,remark=x.remark });

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// type==1,客户，0供应商
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [LoginAllow]
        public ActionResult querycustomer(string key,int type) {
            var list = manageService.GetSupplierList(key);
            var r = list.Where(x => x.type == type && x.valid == true).Select(x => new { id = x.supplierId, name = x.supplierName,no=x.supplierNo==null?"":x.supplierNo }).ToList();
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region session维持

        [LoginAllow]
        [HttpPost]
        public ActionResult autopost()
        {
            ReturnValue r = new ReturnValue();
            r.status = true;
            r.message = SessionHelper.SessionId;
            Session["autopostsession"] = r.message;
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 文件上传
        [LoginAllow]
        public ActionResult fileuploadactin(string filetype)
        {
            ReturnValue r = new ReturnValue() { status = true };
            var res = ConfigurationManager.AppSettings["FilePath"].ToString();//c:\Res\
            var directory = "";
            var httpfile = Request.Files["myfile"];
            if (httpfile != null)
            {
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                var filename = "";
                if (fn.Contains("\\")) filename = fn.Substring(fn.LastIndexOf("\\")).Replace("\\", "");
                else filename = fn;

                if (filetype == "material")
                {
                    #region 物料图片
                    directory = res  + "\\material\\";
                    #region 上传
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    if (exn.ToLower() != ".png" && exn.ToLower() != ".jpg" && exn.ToLower() != ".bmp" && exn.ToLower() != ".jpeg" && exn.ToLower() != ".gif")
                    {
                        r.status = false;
                        r.message= "请上传图片格式文件（jpg,jpeg,png,bmp,gif）";
                    }
                    else
                    {
                        var newname = Guid.NewGuid().ToString() + exn;
                        try
                        {
                            httpfile.SaveAs(directory + newname);
                        }
                        catch
                        {
                            r.status = false;
                            r.message = "上传失败";
                        }
                        r.status = true;
                        r.value = newname;
                        r.value2 = filename;
                    }
                    #endregion
                    #endregion
                }
                else if (filetype == "message")
                {
                    #region 站内消息附件
                    directory = res + "\\message\\";
                    #region 上传
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    if (exn.ToLower() == ".exe" || exn.ToLower() == ".dll" || exn.ToLower() == ".js" || exn.ToLower() == ".ms" || exn.ToLower() == ".php")
                    {
                        r.status = false;
                        r.message = "请上传正确的文档文件";
                    }
                    else
                    {
                        var newname = Guid.NewGuid().ToString() + exn;
                        try
                        {
                            httpfile.SaveAs(directory + newname);
                        }
                        catch
                        {
                            r.status = false;
                            r.message = "上传失败";
                        }
                        r.status = true;
                        r.value = newname;
                        r.value2 = filename;
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    var str2 = JsonHelper.ToJson(new { status = false, message = "文件上传类型错误" });
                    return Content(str2);
                }
            }
            else
            {
                r.status = false;
                r.message = "未上传文件";
            }
            var str = JsonHelper.ToJson(r);
            return Content(str);
        }
        #endregion
    }
}
