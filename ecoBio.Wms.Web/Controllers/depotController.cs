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
using System.Data;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Entities;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class depotController : BaseController
    {
        private StockInService stockinService;
        private StockOutService stockoutService;
        private PurchaseService purchaseService;
        private ManageService manageService;
        private BomService bomService;

        public depotController(IStockInRepository _stockinrepository, IStockOutRepository _stockoutrepository, IPurchaseRepository _purchaseRepository, IManageRepository _manageRepository, IBomRepository _bomRepository)
        {
            stockinService = new StockInService(_stockinrepository);
            stockoutService = new StockOutService(_stockoutrepository);
            purchaseService = new PurchaseService(_purchaseRepository);
            manageService = new ManageService(_manageRepository);
            bomService = new BomService(_bomRepository);
        }

        public ActionResult rawmaterial(int? page)
        {
            var depotid = 1;
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var message = WebRequest.GetString("message", true);
            data.key = key;
            var list = stockoutService.GetDepotDetail(depotid, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.depotid = depotid;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }

        public ActionResult midproduct(int? page)
        {
            var depotid = 3;
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var message = WebRequest.GetString("message", true);
            data.key = key;
            var list = stockoutService.GetDepotDetail(depotid, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.depotid = depotid;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }

        public ActionResult product(int? page)
        {
            var depotid = 2;
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var message = WebRequest.GetString("message", true);
            data.key = key;
            var list = stockoutService.GetDepotDetail(depotid, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.depotid = depotid;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        public ActionResult scrap(int? page)
        {
            var depotid = 4;
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var message = WebRequest.GetString("message", true);
            data.key = key;
            var list = stockoutService.GetDepotDetail(depotid, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.depotid = depotid;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        public ActionResult otherdepot(int? page)
        {
            var depotid = 5;
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var message = WebRequest.GetString("message", true);
            data.key = key;
            var list = stockoutService.GetDepotDetail(depotid, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            var mc = stockinService.GetMaterialCategory();
            data.mc = mc;
            data.depotid = depotid;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }

        #region 报表

        #region 库存报表
        public ActionResult reportdepot(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_mc = WebRequest.GetString("sel_mc", true);
            //data.sel_mc = sel_mc;

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var list = stockoutService.ReportDepot(sel_dep, sel_mc.ToString(), m, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;

            var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            data.mc = mc;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_mc != "") param += "&sel_mc=" + sel_mc;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportdepot", ForController = "depot")]
        public ActionResult reportdepotpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " category asc,materialName asc ";
            var list = stockoutService.ReportDepot(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportdepot", ForController = "depot")]
        public ActionResult exceldepot(string where, string orderby)
        {
            //var sel_mc = WebRequest.GetString("sel_mc", true);
            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var m = WebRequest.GetString("m", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var list = stockoutService.ReportDepot(sel_dep, sel_mc.ToString(), m, less, big);  
            //var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " category asc,materialName asc ";
            var list = stockoutService.ReportDepot(where, orderby);
            string[] head = new string[12] { "序号", "所在仓库", "物料类别", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "库存","单价","金额", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.depotName + "|";
                //var hc = mc.FirstOrDefault(x => x.Value == p.materialCategory);
                row += p.category + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.depotAmount.ToString() + "|";
                if (Enterprise.Invoicing.Web.Masterpage.CheckRight("depotmoney_report"))
                {
                    row += (p.price.ToString() ) + "|";
                    row += (Math.Round(((double)p.price * p.depotAmount), 4).ToString() ) + "|";
                }
                else
                {
                    row += "|"; row += "|";
                }
                row += p.remark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 申请单报表
        public ActionResult reportrequire(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var isover = WebRequest.GetInt("isover", -1);
            //data.isover = isover;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;


            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportPurchaseRequire(sel_dep, sel_staff, status, valid, isover, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;

            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (isover != -1) param += "&isover=" + isover;

            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportrequire", ForController = "depot")]
        public ActionResult reportrequirepart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype='采购申请' ";
            else where += " and reprottype='采购申请' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
 
        [AjaxAction(ForAction = "reportrequire", ForController = "depot")]
        public ActionResult excelrequire(string where, string orderby)
        {
            if (where == null && where == "") where = " and reprottype='采购申请' ";
            else where += " and reprottype='采购申请' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            string[] head = new string[15] { "序号", "部门", "员工", "申请单号", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "申请数量", "采购数量", "申请日期", "需求日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.showno + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += p.amount2.ToString() + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += (p.detaildate.HasValue ? ((p.detaildate.Value < DateTime.Now.AddYears(5) ? p.detaildate.Value.ToString("yyyy-MM-dd") : "")) : "") + "|";
                row += (p.isover == 1 ? "完工" : p.statustype) + "|";
                row += p.detailremark;
                data.Add(row);
            }

          var msg=  FileHelper.ExportEasy(head, data);

          return Content(msg);
        }
        #endregion

        #region 采购单报表
        public ActionResult reportorder(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //data.sel_sup = sel_sup;
            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var isover = WebRequest.GetInt("isover", -1);
            //data.isover = isover;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var ret = WebRequest.GetInt("ret", -1);
            //data.ret = ret;

            //var type = WebRequest.GetInt("type", -1);
            //data.type = type;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var pless = WebRequest.GetFloat("pless", -404);
            //data.pless = pless;
            //var pbig = WebRequest.GetFloat("pbig", -404);
            //data.pbig = pbig;


            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportPurchase(sel_dep, sel_staff,sel_sup, status, valid, isover, type,ret, no,  m, start, end, less, big, pless, pbig);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;
            var suppliers = purchaseService.QuerySupplier(0,1).ToList().
                Select(p => new SelectListItem { Text = p.supplierName, Value = p.supplierId.ToString() }).ToList();
            data.suppliers = suppliers;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big; 
            //if (pless != -404) param += "&pless=" + pless;
            //if (pbig != -404) param += "&pbig=" + pbig;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_sup != 0) param += "&sel_sup=" + sel_sup;

            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (isover != -1) param += "&isover=" + isover;
            //if (type != -1) param += "&type=" + type;
            //if (ret != -1) param += "&ret=" + ret;

            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportorder", ForController = "depot")]
        public ActionResult reportorderpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype='采购单' ";
            else where += " and reprottype='采购单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportorder", ForController = "depot")]
        public ActionResult excelorder(string where, string orderby)
        {
            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var isover = WebRequest.GetInt("isover", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var ret = WebRequest.GetInt("ret", -1);

            //var type = WebRequest.GetInt("type", -1);

            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);

            //var pless = WebRequest.GetFloat("pless", -404);
            //var pbig = WebRequest.GetFloat("pbig", -404);


            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportPurchase(sel_dep, sel_staff,sel_sup, status, valid, isover, type,ret, no, m, start, end, less, big, pless, pbig);
            if (where == null && where == "") where = " and reprottype='采购单' ";
            else where += " and reprottype='采购单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[20] { "序号", "部门", "员工", "采购单号", "供应商", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "采购数量", "采购单价", "退单数量", "采购日期", "交货日期", "状态", "备注", "类别", "申请单号", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.showno + "|";
                row += p.suuplier + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += (Enterprise.Invoicing.Web.Masterpage.CheckRight("purchasemoney_report")?p.amount3.ToString():"") + "|";
                row += p.linkfor2.ToString() + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += (p.detaildate.HasValue ? (p.detaildate.Value < DateTime.Now.AddYears(5) ? p.detaildate.Value.ToString("yyyy-MM-dd") : "") : "") + "|";
                row += (p.isover == 1 ? "完工" : p.statustype) + "|";
                row += p.remark + "|";
                row += p.type + "|";
                row += p.linkno1 + "|";
                row += p.detailremark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 入库单报表
        public ActionResult reportstockin(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //data.sel_depot = sel_depot;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var ret = WebRequest.GetInt("ret", -1);
            //data.ret = ret;

            //var intype = WebRequest.GetInt("intype", -1);
            //data.intype = intype;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportStockIn(sel_dep, sel_staff,sel_depot, status, valid, intype, ret, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs; 
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_depot != 0) param += "&sel_depot=" + sel_dep;

            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (intype != -1) param += "&intype=" + intype;
            //if (ret != -1) param += "&ret=" + ret;

            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportstockin", ForController = "depot")]
        public ActionResult reportstockinpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype like '%入库单' ";
            else where += " and reprottype like '%入库单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        
        [AjaxAction(ForAction = "reportstockin", ForController = "depot")]
        public ActionResult excelstockin(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var ret = WebRequest.GetInt("ret", -1);
            //var intype = WebRequest.GetInt("intype", -1);
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportStockIn(sel_dep, sel_staff, sel_depot, status, valid, intype, ret, no, m, start, end, less, big);
            if (where == null && where == "") where = " and reprottype like '%入库单' ";
            else where += " and reprottype like '%入库单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[17] { "序号", "部门", "员工", "入库类别", "入库单号", "仓库人员", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "入库数量", "退单数量", "仓库", "入库日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.datatype + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += p.linkfor1.ToString() + "|";
                row += p.depotname + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.statustype + "|";
                row += p.detailremark ;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        
        #region 出库单报表
        public ActionResult reportstockout(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //data.sel_depot = sel_depot; 
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //data.sel_sup = sel_sup;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var ret = WebRequest.GetInt("ret", -1);
            //data.ret = ret;

            //var intype = WebRequest.GetInt("intype", -1);
            //data.intype = intype;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportStockOut(sel_dep, sel_staff,sel_depot, status, valid,sel_sup, intype, ret, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs; 
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots; 
            var suppliers = purchaseService.QuerySupplier(1, 1).ToList().
                 Select(p => new SelectListItem { Text = p.supplierName, Value = p.supplierId.ToString() }).ToList();
            data.suppliers = suppliers;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_depot != 0) param += "&sel_depot=" + sel_dep;
            //if (sel_sup != 0) param += "&sel_sup=" + sel_sup;

            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (intype != -1) param += "&intype=" + intype;
            //if (ret != -1) param += "&ret=" + ret;

            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportstockin", ForController = "depot")]
        public ActionResult reportstockoutpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype like '%出库单' ";
            else where += " and reprottype like '%出库单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportstockout", ForController = "depot")]
        public ActionResult excelstockout(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var ret = WebRequest.GetInt("ret", -1);
            //var intype = WebRequest.GetInt("intype", -1);
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportStockOut(sel_dep, sel_staff, sel_depot, status, valid,sel_sup, intype, ret, no, m, start, end, less, big);
            if (where == null && where == "") where = " and reprottype like '%出库单' ";
            else where += " and reprottype like '%出库单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[20] { "序号", "部门", "员工", "出库类别", "出库单号", "仓库人员", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "出库数量", "单价","退单数量", "仓库", "出库日期", "状态", "备注", "客户","销售订单号" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.datatype + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.materialName + "|";
                row += p.materialNo + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += ((p.datatype.EndsWith("销售出库单")&& Enterprise.Invoicing.Web.Masterpage.CheckRight("sellmoney_report"))?p.amount2.ToString():"") + "|";
                row += p.amount3.ToString() + "|";
                row += p.depotname + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.statustype + "|";
                row += p.detailremark + "|";
                row += (p.suuplier != null ? p.suuplier.ToString() : "") + "|";
                row += p.linkno1 != null ? p.linkno1 : "";

                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 以旧换新报表
        public ActionResult reportold2new(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //data.sel_depot = sel_depot;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;

            //var intype = WebRequest.GetInt("intype", -1);
            //data.intype = intype;

            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportStockOtN(sel_dep, sel_staff, sel_depot, status, valid,  intype,  no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_depot != 0) param += "&sel_depot=" + sel_dep;

            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (intype != -1) param += "&intype=" + intype;

            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportold2new", ForController = "depot")]
        public ActionResult reportold2newpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype='以旧换新' ";
            else where += " and reprottype='以旧换新' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportold2new", ForController = "depot")]
        public ActionResult excelold2new(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var intype = WebRequest.GetInt("intype", -1);
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportStockOtN(sel_dep, sel_staff, sel_depot, status, valid, intype, no, m, start, end, less, big);
            if (where == null && where == "") where = " and reprottype='以旧换新' ";
            else where += " and reprottype='以旧换新' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[16] { "序号", "部门", "员工", "明细类别", "以旧换新单号", "仓库人员", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "数量", "仓库", "日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row +=  p.datatype + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += p.depotname + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.statustype + "|";
                row += p.detailremark ;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 采购退单报表
        public ActionResult reportreturnorder(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //data.sel_depot = sel_depot;
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //data.sel_sup = sel_sup;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportOrderRetrun(sel_dep, sel_staff, sel_depot, status, valid, sel_sup, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            var suppliers = purchaseService.QuerySupplier(0, 1).ToList().
                 Select(p => new SelectListItem { Text = p.supplierName, Value = p.supplierId.ToString() }).ToList();
            data.suppliers = suppliers;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_depot != 0) param += "&sel_depot=" + sel_dep;
            //if (sel_sup != 0) param += "&sel_sup=" + sel_sup;
            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportreturnorder", ForController = "depot")]
        public ActionResult reportreturnorderpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype='采购退单' ";
            else where += " and reprottype='采购退单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportreturnorder", ForController = "depot")]
        public ActionResult excelreturnorder(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_depot = WebRequest.GetInt("sel_depot", 0);
            //var sel_sup = WebRequest.GetInt("sel_sup", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);   
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportOrderRetrun(sel_dep, sel_staff, sel_depot, status, valid, sel_sup, no, m, start, end, less, big);
            if (where == null && where == "") where = " and reprottype='采购退单' ";
            else where += " and reprottype='采购退单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[19] { "序号", "部门", "员工", "退单号", "仓库人员", "采购单号", "供应商", "入库单号", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "采购数量", "退单数量", "仓库", "出库日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.linkno1 + "|";
                row += p.suuplier + "|";
                row += p.linkno2 + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.linkfor1.ToString() + "|";
                row += p.amount1.ToString() + "|";
                row += p.depotname + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.statustype + "|";
                row += p.detailremark ;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 领料退单报表
        public ActionResult reportreturnout(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_fdepot = WebRequest.GetInt("sel_fdepot", 0);
            //data.sel_fdepot = sel_fdepot;
            //var sel_tdepot = WebRequest.GetInt("sel_tdepot", 0);
            //data.sel_tdepot = sel_tdepot;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportStockReturn(sel_dep, sel_staff, sel_fdepot, sel_tdepot, status, valid, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;
            var fdepots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.fdepots = fdepots;
            var tdepots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.tdepots = tdepots;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_fdepot != 0) param += "&sel_fdepot=" + sel_fdepot;
            //if (sel_tdepot != 0) param += "&sel_tdepot=" + sel_tdepot;
            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "reportreturnout", ForController = "depot")]
        public ActionResult reportreturnoutpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " and reprottype='领料退单' ";
            else where += " and reprottype='领料退单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportreturnout", ForController = "depot")]
        public ActionResult excelreturnout(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_fdepot = WebRequest.GetInt("sel_fdepot", 0);
            //var sel_tdepot = WebRequest.GetInt("sel_tdepot", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportStockReturn(sel_dep, sel_staff, sel_fdepot, sel_tdepot, status, valid, no, m, start, end, less, big);
            if (where == null && where == "") where = " and reprottype='领料退单' ";
            else where += " and reprottype='领料退单' ";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[18] { "序号", "部门", "员工", "退单号", "仓库人员", "领料单号", "领料仓库", "退回仓库", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "领料数量", "退单数量", "出库日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.linkno1 + "|";
                row += p.depotname + "|";
                row += p.linkno2 + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount2.ToString() + "|";
                row += p.amount1.ToString() + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.statustype + "|";
                row += p.detailremark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion        

        #region 时序表
        public ActionResult timetab(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //data.sel_dep = sel_dep;
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //data.sel_staff = sel_staff;
            //var sel_fdepot = WebRequest.GetInt("sel_fdepot", 0);
            //data.sel_fdepot = sel_fdepot;
            //var sel_tdepot = WebRequest.GetInt("sel_tdepot", 0);
            //data.sel_tdepot = sel_tdepot;

            //var status = WebRequest.GetInt("status", -1);
            //data.status = status;
            //var valid = WebRequest.GetInt("valid", -1);
            //data.valid = valid;
            //var m = WebRequest.GetString("m", true);
            //data.m = m;
            //var no = WebRequest.GetString("no", true);
            //data.no = no;
            //var less = WebRequest.GetInt("less", -404);
            //data.less = less;
            //var big = WebRequest.GetInt("big", -404);
            //data.big = big;

            //var start = WebRequest.GetString("start", true);
            //data.start = start;
            //var end = WebRequest.GetString("end", true);
            //data.end = end;

            //var list = stockoutService.ReportStockReturn(sel_dep, sel_staff, sel_fdepot, sel_tdepot, status, valid, no, m, start, end, less, big);

            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 17;
            //if (_page < 1) _page = 1;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            var tdepots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.tdepots = tdepots;
            var suppliers = purchaseService.QuerySupplier(0, 1).ToList().
                 Select(p => new SelectListItem { Text = p.supplierName, Value = p.supplierId.ToString() }).ToList();
            data.suppliers = suppliers;
            var customer = purchaseService.QuerySupplier(1, 1).ToList().
                 Select(p => new SelectListItem { Text = p.supplierName, Value = p.supplierId.ToString() }).ToList();
            data.customer = customer;

            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //var param = "";
            //if (m != "") param += "&m=" + m;
            //if (no != "") param += "&no=" + no;
            //if (less != -404) param += "&less=" + less;
            //if (big != -404) param += "&big=" + big;
            //if (sel_staff != 0) param += "&sel_staff=" + sel_staff;
            //if (sel_dep != 0) param += "&sel_dep=" + sel_dep;
            //if (sel_fdepot != 0) param += "&sel_fdepot=" + sel_fdepot;
            //if (sel_tdepot != 0) param += "&sel_tdepot=" + sel_tdepot;
            //if (status != -1) param += "&status=" + status;
            //if (valid != -1) param += "&valid=" + valid;
            //if (start != "") param += "&start=" + start;
            //if (end != "") param += "&end=" + end;
            //data.otherParam = param;
            return View(data);
        }
        [AjaxAction(ForAction = "timetab", ForController = "depot")]
        public ActionResult timetabpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " reprottype desc,showno desc, createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "timetab", ForController = "depot")]
        public ActionResult exceltimetab(string where, string orderby)
        {

            //var sel_dep = WebRequest.GetInt("sel_dep", 0);
            //var sel_staff = WebRequest.GetInt("sel_staff", 0);
            //var sel_fdepot = WebRequest.GetInt("sel_fdepot", 0);
            //var sel_tdepot = WebRequest.GetInt("sel_tdepot", 0);
            //var status = WebRequest.GetInt("status", -1);
            //var valid = WebRequest.GetInt("valid", -1);
            //var m = WebRequest.GetString("m", true);
            //var no = WebRequest.GetString("no", true);
            //var less = WebRequest.GetInt("less", -404);
            //var big = WebRequest.GetInt("big", -404);
            //var start = WebRequest.GetString("start", true);
            //var end = WebRequest.GetString("end", true);

            //var list = stockoutService.ReportStockReturn(sel_dep, sel_staff, sel_fdepot, sel_tdepot, status, valid, no, m, start, end, less, big);
            if (where == null ) where = "";
            if (orderby == null || orderby == "") orderby = " showno desc,createdate desc ";
            var list = stockoutService.ReportReceiptDetail(where, orderby);
            string[] head = new string[16] { "序号", "类别", "部门", "员工", "单号", "仓库人员", "仓库", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "数量", "日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.reprottype + "|";
                row += p.dep + "|";
                row += p.staff + "|";
                row += p.showno + "|";
                row += p.deportStaff + "|";
                row += p.depotname + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.materialTu + "|";
                row += p.unit + "|";
                row += p.amount1.ToString() + "|";
                row += p.createDate.ToString("yyyy-MM-dd HH:mm:ss") + "|";
                row += p.statustype + "|";
                row += p.detailremark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion        

        #region BOM订单报表
        public ActionResult reportbomorder(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            var staffs = manageService.GetEmployeeList("").Where(p => p.status == true && p.isUser == true).
                Select(p => new SelectListItem { Text = p.staffName, Value = p.staffId.ToString() }).ToList();
            data.staffs = staffs;

            return View(data);
        }
        [AjaxAction(ForAction = "reportbomorder", ForController = "depot")]
        public ActionResult reportbomorderpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null && where == "") where = " ";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = bomService.BomOrderDetailList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "reportbomorder", ForController = "depot")]
        public ActionResult excelbomorder(string where, string orderby)
        {
            if (where == null && where == "") where = " ";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = bomService.BomOrderDetailList(where, orderby);

            string[] head = new string[18] { "序号", "部门", "员工", "订单号", "客户", "类别", "物料编码", "物料名称", "物料规格", "物料图号", "数量", "已发数", "未交货数", "单价", "总价", "交货日期", "状态", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.depName + "|";
                row += p.staffName + "|";
                row += p.bomOrderNo + "|";
                row += p.supplierName + "|";
                row += p.orderType + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.tunumber + "|";
                row += p.Amount.ToString() + "|";
                row += p.sellAmount.ToString() + "|";
                row += (p.Amount-p.sellAmount).ToString() + "|";
                row += (Enterprise.Invoicing.Web.Masterpage.CheckRight("ordermoney_report") ? p.Price.ToString("N") : "") + "|";
                row += (Enterprise.Invoicing.Web.Masterpage.CheckRight("ordermoney_report") ? (p.Amount * p.Price).ToString("N"):"") + "|";
                row += (p.sendDate.HasValue ? p.sendDate.Value.ToString("yyyy-MM-dd") : "") + "|";
                row += Utils.GetStatus(p.status) + "|";
                row += p.remark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 采购单报表
        public ActionResult reportcost(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            
            return View(data);
        }
        [AjaxAction(ForAction = "reportcost", ForController = "depot")]
        public ActionResult reportcostpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = stockoutService.ReportCostDetail(where);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportcost", ForController = "depot")]
        public ActionResult excelcost(string where, string orderby)
        {
            var list = stockoutService.ReportCostDetail(where);
            string[] head = new string[11] { "序号", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "类别", "供应商","采购日期","采购单价","采购数量" };
            List<string> data = new List<string>();
            var index = 1;
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                //var row = (i + 1).ToString() + "|";
                var row = p.materialNo + "|";
                row += p.MaterialName + "|";
                row += p.MaterialModel + "|";
                row += p.th + "|";
                row += p.unit + "|";
                row += p.category + "|";
                //row += p.shortInfo;
                var suplist = p.shortInfo.Split(';');
                foreach (var item in suplist)
                {
                    if (item == "" || !item.Contains('|')) continue;
                    //item:set @shortInfo+=(@suppliername+'|'+convert(varchar(20),@getdate,23)+'|'+convert(varchar(10),@price)+'|'+convert(varchar(10),@count)+';')
                    var nrow = index + "|" + row + item;
                    index++;
                    data.Add(nrow);
                }
            }
            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion


        #region 价格报表
        public ActionResult reportprice(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View(data);
        }
        [AjaxAction(ForAction = "reportprice", ForController = "depot")]
        public ActionResult reportpricepart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " suppliername asc,price desc ";
            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where supplierid>0" + where + " order by " + orderby).ToList();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "reportprice", ForController = "depot")]
        public ActionResult excelprice(string where, string orderby)
        {
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " suppliername asc,price desc ";
            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where supplierid>0" + where + " order by " + orderby).ToList();
            //var list = stockoutService.ReportDepot(where, orderby);
            string[] head = new string[10] { "序号", "供应商","物料编码", "物料名称", "物料规格",  "单位", "价格","启用日期","状态" ,"备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.supplierName + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.unit + "|";
                row += p.price + "|";
                row += p.startDate.ToString("yyyy-MM-dd") + "|";
                row += (p.status == 0 ? "未启用" : (p.status == 1 ? "启用" : "终止")) + "|";
                row += p.remark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion


        #region 未交货报表
        public ActionResult notback(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View(data);
        }
        [AjaxAction(ForAction = "notback", ForController = "depot")]
        public ActionResult notbackpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate asc ";
            var list = ServiceDB.Instance.QueryModelList<V_PurchaseDetailModel>("select * from V_PurchaseDetailModel where status=1 and isover=0 and poRemain>0  " + where + " order by " + orderby).ToList();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "notback", ForController = "depot")]
        public ActionResult excelnotback(string where, string orderby)
        {
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate asc ";
            var list = ServiceDB.Instance.QueryModelList<V_PurchaseDetailModel>("select * from V_PurchaseDetailModel where status=1 and isover=0 and poRemain>0 " + where + " order by " + orderby).ToList();
            //var list = stockoutService.ReportDepot(where, orderby);
            string[] head = new string[12] { "序号", "供应商", "订单号", "物料编码", "物料名称", "物料规格", "单位", "订单数量", "未次数量", "订单日期", "交货日期", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.supplierName + "|";
                row += p.purchaseNo + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.unit + "|";
                row += p.poAmount + "|";
                row += p.poRemain + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += (p.sendDate.HasValue && p.sendDate.Value.Year < 3000 ? p.sendDate.Value.ToString("yyyy-MM-dd") : "") + "|";
                row += p.remark + p.detailRemark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #endregion


        #region 库存报表
        public ActionResult stockreport(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            return View(data);
        }
        [AjaxAction(ForAction = "stockreport", ForController = "depot")]
        public ActionResult stockreportpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null || where == "") where = " ";
            where = " where depotId>0 " + where; 
            if (orderby == null || orderby == "") orderby = "  ";
            else orderby = "order by" + orderby;
            var list=ServiceDB.Instance.QueryModelList<V_StockReportStatistics>("select * from V_StockReportStatistics " + where + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "stockreport", ForController = "depot")]
        public ActionResult excelstock(string where, string orderby)
        {
            var w=WebRequest.GetString("where");
            if (where == null || where == "") where = " ";
            where = " where depotId>0 " + where; 
            if (orderby == null || orderby == "") orderby = "  ";
            else orderby = "order by" + orderby;
            var list = ServiceDB.Instance.QueryModelList<V_StockReportStatistics>("select * from V_StockReportStatistics " + where + orderby);
            string[] head = new string[12] { "序号", "报表期", "仓库", "物料种类", "期初数量", "期初金额", "入库数量", "入库金额", "出库数量", "出库金额", "期末数量", "期末金额" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.stockMonth + "|";
                row += p.depotName + "|";
                row += p.materials + "|";
                row += p.startAmount + "|";
                row += p.startCost + "|";
                row += p.inAmount + "|";
                row += p.inCost + "|";
                row += p.outAmount + "|";
                row += p.outCost.ToString() + "|";
                row += p.endAmount.ToString() + "|";
                row += p.endCost.ToString();
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        [AjaxAction(ForAction = "stockreport", ForController = "depot")]
        public ActionResult stockdc(string where, string orderby)
        {

            if (where == null || where == "") where = " ";
            where = " where depotId>0 " + where; 
            if (orderby == null || orderby == "") orderby = "  ";
            else orderby = "order by" + orderby;
            var list = ServiceDB.Instance.QueryModelList<V_StockReport>("select * from V_StockReport " + where + orderby);
            string[] head = new string[16] { "序号", "报表期", "仓库", "物料编码", "物料名称", "物料规格", "单位", "图号", "期初数量", "期初金额", "入库数量", "入库金额", "出库数量", "出库金额", "期末数量", "期末金额" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.stockMonth + "|";
                row += p.depotName + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.unit + "|";
                row += p.tunumber + "|";
                row += p.startAmount + "|";
                row += p.startCost + "|";
                row += p.inAmount + "|";
                row += p.inCost + "|";
                row += p.outAmount + "|";
                row += p.outCost.ToString() + "|";
                row += p.endAmount.ToString() + "|";
                row += p.endCost.ToString();
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion
    }
}
