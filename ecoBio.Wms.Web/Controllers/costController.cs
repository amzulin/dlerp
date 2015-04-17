using Enterprise.Invoicing.Entities;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Calabonga.Mvc.PagedListExt;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Service;
using Enterprise.Invoicing.Repositories;
namespace Enterprise.Invoicing.Web.Controllers
{
    public class costController : BaseController
    {
        private BomService bomService;
        private ManageService manageService;
        public costController(IManageRepository _manageRepository, IBomRepository _bomrepository)
        {
            manageService = new ManageService(_manageRepository);
            bomService = new BomService(_bomrepository);
        }
        #region 应付款

        public ActionResult paylist(int? page, int? pagesize, DateTime? start, DateTime? end, string supplier)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View(data);
        }
        [AjaxAction(ForAction = "paylist", ForController = "cost")]
        public ActionResult paylistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by settleNo desc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<Settlement>(" select * from Settlement  where settleType=1 " + where + orderby).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "paylist", ForController = "cost")]
        public ActionResult paydetailview(int supplier, string where)//, string orderby
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            //if (orderby == null || orderby == "") orderby = " order by supplierName asc ";
            //else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<NeedPayDetailModel>(" select stockinno,purchaseno,createdate as inDate,sum(incost) as cost,sum(pocost) as allcost,sum(inamount) as amount from V_StockInPurchase  where supplierid=" + supplier + where + " group by stockinno,purchaseno,createdate order by createdate desc ").ToList();

            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "paylist", ForController = "cost")]
        public ActionResult excelpaylist(string where, string orderby)
        {
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by supplierName asc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<Enterprise.Invoicing.Entities.Models.V_StockInPurchase>(" select * from V_StockInPurchase where supplierId>0 " + where + orderby).ToList();

            string[] head = new string[19] { "序号", "供应商", "采购单号", "采购数量", "采购单价", "采购金额", "入库单号", "入库日期", "入库数量", "入库金额", "退单数量", "退单金额", "类别", "物料编码", "物料名称", "物料规格", "物料图号", "单位", "应付金额" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.supplierName + "|";
                row += p.purchaseNo + "|";
                row += p.poAmount + "|";
                row += p.poPrice + "|";
                row += p.poCost + "|";
                row += p.stockInNo + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.inAmount + "|";
                row += p.inCost + "|";
                row += p.returnAmount + "|";
                row += p.returnCost + "|";
                row += p.category + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.tunumber + "|";
                row += p.unit + "|";
                row += (p.inCost - p.returnCost);
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }

        #endregion
        #region 应收款

        public ActionResult receivable(int? page, int? pagesize, DateTime? start, DateTime? end, string supplier)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View(data);
        }
        [AjaxAction(ForAction = "receivable", ForController = "cost")]
        public ActionResult receivablepart(SearchModel model, int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by settleNo desc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<Settlement>(" select * from Settlement  where settleType=0 " + where + orderby).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "receivable", ForController = "cost")]
        public ActionResult receivableview(int supplier, string where)//, string orderby
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            //if (orderby == null || orderby == "") orderby = " order by supplierName asc ";
            //else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<NeedPayDetailModel>(" select stockoutNo as stockinno, bomOrderNo as purchaseno,createdate as inDate,sum(outcost) as cost,sum(totalcost) as allcost,sum(outamount) as amount from V_StockOutPurchase  where stockoutno is not null and supplierid=" + supplier + where + " group by stockoutNo,bomOrderNo,createdate order by createdate desc ").ToList();

            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "receivable", ForController = "cost")]
        public ActionResult excelreceivable(string where, string orderby)
        {
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by settleNo asc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<Settlement>(" select * from Settlement  where settleNo is not null " + where + orderby).ToList();

            string[] head = new string[15] { "序号", "客户", "结算单", "开始日期", "结束日期", "生成日期", "销售金额", "退单金额", "应收金额", "实收金额", "坏账金额", "审核人", "审核时间", "状态", "是否完工" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.supplierName + "|";
                row += p.settleNo + "|";
                row += p.settleStart.ToString("yyyy-MM-dd") + "|";
                row += p.settleEnd.ToString("yyyy-MM-dd") + "|";
                row += p.createDate.ToString("yyyy-MM-dd") + "|";
                row += p.firstCost + "|";
                row += p.returnCost + "|";
                row += p.tradeCost + "|";
                row += p.realCost + "|";
                row += p.badCost + "|";
                row += p.checkStaff + "|";
                row += p.checkDate.ToString("yyyy-MM-dd") + "|";
                row += (p.status == 0 ? "未审核" : "已审核") + "|";
                row += (p.isover == 0 ? "未完工" : "已完工");
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 报销

        public ActionResult billlist()
        {
            return View();
        }
        [AjaxAction(ForAction = "billlist", ForController = "cost")]
        public ActionResult billlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by createDate desc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<V_BillCost>(" select " +
            "billNo,title,staffMake,depId ,staffCheck,checkDate,staffCfo,cfoDate,staffBoss,bossDate,status,valid,isover,billType,remark,createDate,checkRes,cfoRes,bossRes,makeName,checkName,cfoName,bossName,depName,checkMsg,cfoMsg,bossMsg"
                + " from V_BillCostDetail  where billType=0 "
                + where + " group by "
                + " billNo,title,staffMake,depId ,staffCheck,checkDate,staffCfo,cfoDate,staffBoss,bossDate,status,valid,isover,billType,remark,createDate,checkRes,cfoRes,bossRes,makeName,checkName,cfoName,bossName,depName,checkMsg,cfoMsg,bossMsg "
                + orderby).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "billlist", ForController = "cost")]
        public ActionResult billdetailview(string key, string where)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            var list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail  where billNo='" + key + "' " + where + "  order by cost desc ").ToList();

            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "billlist", ForController = "cost")]
        public ActionResult billdelete()
        {
            string no = WebRequest.GetString("no", true);
            var d1 = ServiceDB.Instance.ExecuteSqlCommand("delete BillCostDetail  where billNo='" + no + "'");
            var d2 = ServiceDB.Instance.ExecuteSqlCommand("delete BillCost  where billNo='" + no + "'");

            return Content((d1 + d2) > 1 ? "ok" : "error");
        }


        public ActionResult billone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_BillCost();
            List<V_BillCostDetail> list = new List<V_BillCostDetail>();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = manageService.GetBillNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_BillCost>(" select * from V_BillCost where billNo='" + no + "'");
                list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail where billNo='" + no + "'").ToList();
            }
            data.no = no;
            data.list = list;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "billone", ForController = "cost")]
        public ActionResult savebilldetail()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);
            string km = WebRequest.GetString("km", true);
            string bz = WebRequest.GetString("bz", true);
            string date = WebRequest.GetString("date", true);
            string cost = WebRequest.GetString("cost", true);
            int sn = WebRequest.GetInt("sn", 0);

            ReturnValue back = new ReturnValue { status = false };

            string title = WebRequest.GetString("title", true);
            string remark = WebRequest.GetString("remark", true);

            string type = WebRequest.GetString("type", true);

            if (type == "add")
            {
                var newno = manageService.GetBillNo();
                var insert = ServiceDB.Instance.ExecuteSqlCommand("insert into BillCost (billno,title,staffmake,depid,status,valid,isover,billtype,remark,createdate) values('" + newno + "','" + title + "'," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + ",0,1,0,0,'" + remark + "',getdate())");
                if (insert != 1)
                {
                    back.message = "报销单创建失败，请刷新当前页重试";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                no = newno;
            }
            #region 添加明细
            var had = ServiceDB.Instance.QueryOneModel<BillCostDetail>("select * from BillCostDetail where billNo='" + no + "' and billTitle='" + km + "' and price=" + cost);
            if (sn == 0)
            {
                #region 添加
                if (had != null)
                {
                    back.message = "报销单明细已存在";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insert2 = ServiceDB.Instance.ExecuteSqlCommand("insert into BillCostDetail values('" + no + "','" + km + "',1," + cost + ",'" + date + "','" + bz + "')");
                    if (insert2 != 1)
                    {
                        back.message = "报销单明细创建失败，请刷新当前页重试";
                        return Json(back, JsonRequestBehavior.AllowGet);
                    }
                    var me = ServiceDB.Instance.QueryOneModel<BillCostDetail>("select * from BillCostDetail where billNo='" + no + "' and billTitle='" + km + "' and price=" + cost);
                    if (me == null)
                    {
                        back.message = "报销单明细创建失败";
                    }
                    else
                    {
                        back.status = true;
                        back.value2 = no;
                        back.value = me.detailSn.ToString();
                    }
                }
                #endregion
            }
            else
            {
                #region 修改
                if (had != null && sn != had.detailSn)
                {
                    back.message = "报销单明细已存在";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insert3 = ServiceDB.Instance.ExecuteSqlCommand("update BillCostDetail set billTitle='" + km + "',price=" + cost + ",billDate='" + date + "',remark='" + bz + "' where detailSn=" + sn);
                    if (insert3 != 1)
                    {
                        back.message = "报销单明细修改失败，请刷新当前页重试";
                        return Json(back, JsonRequestBehavior.AllowGet);
                    }
                    back.status = true;
                    back.value2 = no;
                    back.value = sn.ToString();
                }
                #endregion
            }
            #endregion

            return Json(back, JsonRequestBehavior.AllowGet);

        }
        [AjaxAction(ForAction = "billone", ForController = "cost")]
        public ActionResult savebill()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);

            ReturnValue back = new ReturnValue { status = true };

            string title = WebRequest.GetString("title", true);
            string remark = WebRequest.GetString("remark", true);
            var insert3 = ServiceDB.Instance.ExecuteSqlCommand("update BillCost set title='" + title + "',remark='" + remark + "' where billNo='" + no + "'");

            return Json(back, JsonRequestBehavior.AllowGet);

        }
        [AjaxAction(ForAction = "billone", ForController = "cost")]
        public ActionResult deletebilldetail()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);
            int sn = WebRequest.GetInt("sn", 0);

            var delete = ServiceDB.Instance.ExecuteSqlCommand("delete BillCostDetail where detailSn=" + sn);
            return Content(delete == 1 ? "ok" : "error");

        }
        #endregion

        #region 普通采购

        public ActionResult purchaselist()
        {
            return View();
        }
        [AjaxAction(ForAction = "purchaselist", ForController = "cost")]
        public ActionResult purchaselistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by createDate desc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<V_BillCost>(" select " +
            "billNo,title,staffMake,depId ,staffCheck,checkDate,staffCfo,cfoDate,staffBoss,bossDate,status,valid,isover,billType,remark,createDate,checkRes,cfoRes,bossRes,makeName,checkName,cfoName,bossName,depName,checkMsg,cfoMsg,bossMsg"
                + " from V_BillCostDetail  where billType=1 "
                + where + " group by "
                + " billNo,title,staffMake,depId ,staffCheck,checkDate,staffCfo,cfoDate,staffBoss,bossDate,status,valid,isover,billType,remark,createDate,checkRes,cfoRes,bossRes,makeName,checkName,cfoName,bossName,depName,checkMsg,cfoMsg,bossMsg "
                + orderby).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "purchaselist", ForController = "cost")]
        public ActionResult purchasedetailview(string key, string where)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            var list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail  where billNo='" + key + "' " + where + "  order by cost desc ").ToList();

            data.list = list;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "purchaselist", ForController = "cost")]
        public ActionResult purchasedelete()
        {
            string no = WebRequest.GetString("no", true);
            var d1 = ServiceDB.Instance.ExecuteSqlCommand("delete BillCostDetail  where billNo='" + no + "'");
            var d2 = ServiceDB.Instance.ExecuteSqlCommand("delete BillCost  where billNo='" + no + "'");

            return Content((d1 + d2) > 1 ? "ok" : "error");
        }


        public ActionResult purchaseone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_BillCost();
            List<V_BillCostDetail> list = new List<V_BillCostDetail>();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = manageService.GetTCNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_BillCost>(" select * from V_BillCost where billNo='" + no + "'");
                list = ServiceDB.Instance.QueryModelList<V_BillCostDetail>(" select * from V_BillCostDetail where billNo='" + no + "'").ToList();
            }
            data.no = no;
            data.list = list;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "purchaseone", ForController = "cost")]
        public ActionResult savepurchasedetail()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);
            string km = WebRequest.GetString("km", true);
            string bz = WebRequest.GetString("bz", true);
            string date = WebRequest.GetString("date", true);
            string cost = WebRequest.GetString("cost", true);
            string amount = WebRequest.GetString("amount", true);
            int sn = WebRequest.GetInt("sn", 0);

            ReturnValue back = new ReturnValue { status = false };

            string title = WebRequest.GetString("title", true);
            string remark = WebRequest.GetString("remark", true);

            string type = WebRequest.GetString("type", true);

            if (type == "add")
            {
                var newno = manageService.GetTCNo();
                var insert = ServiceDB.Instance.ExecuteSqlCommand("insert into BillCost (billno,title,staffmake,depid,status,valid,isover,billtype,remark,createdate) values('" + newno + "',''," + Masterpage.CurrUser.staffid + "," + Masterpage.CurrUser.depId + ",0,1,0,1,'" + remark + "',getdate())");
                if (insert != 1)
                {
                    back.message = "采购单创建失败，请刷新当前页重试";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                no = newno;
            }
            #region 添加明细
            var had = ServiceDB.Instance.QueryOneModel<BillCostDetail>("select * from BillCostDetail where billNo='" + no + "' and billTitle='" + km + "' and remark=" + remark);
            if (sn == 0)
            {
                #region 添加
                if (had != null)
                {
                    back.message = "采购单明细已存在";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insert2 = ServiceDB.Instance.ExecuteSqlCommand("insert into BillCostDetail values('" + no + "','" + km + "'," + amount + "," + cost + ",'" + date + "','" + bz + "')");
                    if (insert2 != 1)
                    {
                        back.message = "采购单明细创建失败，请刷新当前页重试";
                        return Json(back, JsonRequestBehavior.AllowGet);
                    }
                    var me = ServiceDB.Instance.QueryOneModel<BillCostDetail>("select * from BillCostDetail where billNo='" + no + "' and billTitle='" + km + "' and price=" + cost);
                    if (me == null)
                    {
                        back.message = "采购单明细创建失败";
                    }
                    else
                    {
                        back.status = true;
                        back.value2 = no;
                        back.value = me.detailSn.ToString();
                    }
                }
                #endregion
            }
            else
            {
                #region 修改
                if (had != null && sn != had.detailSn)
                {
                    back.message = "采购单明细已存在";
                    return Json(back, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insert3 = ServiceDB.Instance.ExecuteSqlCommand("update BillCostDetail set billTitle='" + km + "',amount=" + amount + ",price=" + cost + ",billDate='" + date + "',remark='" + bz + "' where detailSn=" + sn);
                    if (insert3 != 1)
                    {
                        back.message = "采购单明细修改失败，请刷新当前页重试";
                        return Json(back, JsonRequestBehavior.AllowGet);
                    }
                    back.status = true;
                    back.value2 = no;
                    back.value = sn.ToString();
                }
                #endregion
            }
            #endregion

            return Json(back, JsonRequestBehavior.AllowGet);

        }
        [AjaxAction(ForAction = "purchaseone", ForController = "cost")]
        public ActionResult savepurchase()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);

            ReturnValue back = new ReturnValue { status = true };

            string remark = WebRequest.GetString("remark", true);
            var insert3 = ServiceDB.Instance.ExecuteSqlCommand("update BillCost set remark='" + remark + "' where billNo='" + no + "'");

            return Json(back, JsonRequestBehavior.AllowGet);

        }
        [AjaxAction(ForAction = "purchaseone", ForController = "cost")]
        public ActionResult deletepurchasedetail()
        {
            //no: no, sn: sn, remark: bz, date: date, km: km, cost: cost
            string no = WebRequest.GetString("no", true);
            int sn = WebRequest.GetInt("sn", 0);

            var delete = ServiceDB.Instance.ExecuteSqlCommand("delete BillCostDetail where detailSn=" + sn);
            return Content(delete == 1 ? "ok" : "error");

        }
        #endregion

        #region 成本

        public ActionResult bomcost(int? page)
        {
            return View();
        }
        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult bomcostpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " order by materialname asc,version asc ";
            else orderby = " order by " + orderby;
            var list = ServiceDB.Instance.QueryModelList<V_BomMaterialView>(" select * from V_BomMaterialView  where parent_Id is null and isChild=0 and status=1 " + where + orderby).ToList();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult excelbomcost(int id)
        {
            List<V_BomCostModel> list = new List<V_BomCostModel>();
            var child = ServiceDB.Instance.QueryModelList<V_BomCostModel>(" select * from V_BomMaterialView  where parent_Id=" + id).ToList();

            // var list = bomService.GetProductBomCostModel(child,Convert.ToDouble(1));
            string[] head = new string[10] { "序号", "类别", "物料编码", "物料名称", "物料规格", "单位", "数量", "单价", "金额", "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.materialCate + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.unit + "|";
                row += p.amount + "|";
                row += p.rootCost + "|";
                row += (p.rootCost * p.amount).ToString() + "|";
                row += p.bomremark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }

        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult bomcostview(string type, int id)
        {

            if (type == "edit" && !Masterpage.CheckRight("bomcost_update")) return RedirectToAction("login", "account");
            if (type == "view" && !Masterpage.CheckRight("bomcost_view")) return RedirectToAction("login", "account");
            dynamic data = new System.Dynamic.ExpandoObject();
            string bomjson = "";
            var one = ServiceDB.Instance.QueryOneModel<V_BomCostModel>(" select * from V_BomCostModel  where bomid=" + id);
            var child = ServiceDB.Instance.QueryModelList<V_BomCostModel>(" select * from V_BomCostModel  where parent_Id=" + id).ToList();


            var list = bomService.GetChildBomCost(child, Convert.ToDouble(1), 1);
            //var virtuals=ServiceDB.Instance.QueryModelList<BomVirtual>("select * from BomVirtual where bomid=" + id).ToList();
            //if (virtuals != null && virtuals.Count > 0)
            //{
            //    List<object> nvs = new List<object>();
            //    foreach (var item in virtuals)
            //    {
            //        var nvi = new { km = 1, id = 0, sn = item.virtualId, no = "其他科目", name = item.virtualName, model = "", amount = item.vAmount, price = item.vPrice.ToString("N"), cost = (item.vPrice * item.vAmount).ToString("N"), unit = "", unit2 = "", amount2 = "", remark = item.remark, index = 0 };
            //        list.Add(nvi);


            //    }
            //    //var nv = new { km = 0, id = 0, sn = 0, no = "其他科目", name = "", model = "", amount = "", price = "", cost = "", unit = "", unit2 = "", amount2 = "", remark = "", index = 0, children = nvs };
            //    //list.Add(nv);

            //}
            bomjson = JsonHelper.ToJson(list);
            data.one = one;
            data.bomjson = bomjson;
            data.type = type;
            return View(data);
        }

        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult savebomcost()
        {
            ReturnValue back = new ReturnValue { status = false };
            if (!Masterpage.CheckRight("bomcost_update"))
            {
                back.message = "非法权限！";
            }
            else
            {
                var id = WebRequest.GetInt("costsn", 0);
                var bomId = WebRequest.GetInt("bom", 0);
                var mybom = WebRequest.GetInt("mybom", 0);
                var price = WebRequest.GetFloat("price", 0);
                var amount = WebRequest.GetFloat("amount", 0);
                var oldprice = WebRequest.GetFloat("oldprice", 0);
                var remark = WebRequest.GetString("remark", true);
                int row = 0;
                if (id == 0)
                    row = ServiceDB.Instance.ExecuteSqlCommand("insert into BomCost values(" + mybom + "," + price + ",getdate(),'" + remark + "')");
                else
                    row = ServiceDB.Instance.ExecuteSqlCommand("update BomCost set price=" + price + ",remark='remark' where costId=" + id);
                if (row != 1) back.message = "价格更新失败！";
                else
                {
                    var egx = amount * price - amount * oldprice;
                    var row2 = ServiceDB.Instance.ExecuteSqlCommand("update BomMain set rootCost=rootCost+" + egx + " where bomId=" + bomId);
                    if (row != 1) back.message = "总成本更新失败！";
                    else back.status = true;
                }
            }
            return Json(back, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult savevirtual()
        {
            //id: id, type: type,amount:amount, price: price, remark: remark,km:km
            int id = WebRequest.GetInt("id", 0);
            int forbom = WebRequest.GetInt("forbom", 0);
            float amount = WebRequest.GetFloat("amount", 0.0f);
            float price = WebRequest.GetFloat("price", 0.0f);
            string km = WebRequest.GetString("km");
            string type = WebRequest.GetString("type");
            int bom = 0;
            int loss = 0;
            if (type == "add")
            {
                bom = id;
                id = 0;
            }
            string remark = WebRequest.GetString("remark");
            var r = bomService.SaveVirtual(type, id, bom, forbom, km, amount, price, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult deletevirtual()
        {
            int id = WebRequest.GetInt("id", 0);
            int forbom = WebRequest.GetInt("forbom", 0);
            var r = bomService.DeleteVirtual(id, forbom);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult updatebomnode()
        {
            //id: id,mybom: mybom, forbom: bomid, type: type,name:name,amount: amount, price: price, remark: rmk
            int id = WebRequest.GetInt("id", 0);
            int mybomid = WebRequest.GetInt("mybom", 0);
            int forbom = WebRequest.GetInt("forbom", 0);

            var type = WebRequest.GetString("type", true);
            var name = WebRequest.GetString("name", true);
            var remark = WebRequest.GetString("remark", true);
            var amount = WebRequest.GetFloat("amount", 0f);
            var price = WebRequest.GetFloat("price", 0f);
            var bom = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomid=" + forbom);
            var row = 0; double newcost = 0f;
            ReturnValue r = new ReturnValue { status = false };
            #region 更新
            if (type == "addk")
            {
                row = ServiceDB.Instance.ExecuteSqlCommand("insert into BomVirtual values(" + forbom + ",'" + name + "'," + amount + "," + price + ",'" + remark + "')");
                if (row == 1)
                {
                    newcost = bom.rootCost + amount * price;
                    r.status = true; r.message = "新项目添加成功";
                }
            }
            else if (type == "addm")
            {
                var mybom = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomid=" + mybomid);
                row = ServiceDB.Instance.ExecuteSqlCommand("insert into BomCost values(" + mybomid + "," + price + ",getdate(),'" + remark + "')");
                if (row == 1)
                {
                    newcost = bom.rootCost + amount * price;//
                    r.status = true; r.message = "新BOM单价添加成功";
                }
            }
            else if (type == "updk")
            {
                var old = ServiceDB.Instance.QueryOneModel<BomVirtual>("select * from BomVirtual where virtualId=" + id);
                row = ServiceDB.Instance.ExecuteSqlCommand("update BomVirtual set virtualName='" + name + "',vAmount=" + amount + ",vPrice=" + price + ",remark='" + remark + "' where virtualId=" + id);
                if (row == 1)
                {
                    newcost = bom.rootCost + (amount * price - old.vAmount * old.vPrice);
                    r.status = true; r.message = "项目修改成功";
                }
            }
            else if (type == "updm")
            {
                var mybom = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomid=" + mybomid);
                var old = ServiceDB.Instance.QueryOneModel<BomCost>("select * from BomCost where costId=" + id);
                row = ServiceDB.Instance.ExecuteSqlCommand("update BomCost set price=" + price + ",remark='" + remark + "' where costId=" + id);
                if (row == 1)
                {
                    newcost = bom.rootCost + (amount * price - old.price * amount);
                    r.status = true; r.message = "BOM单价修改成功";
                }
            }
            else if (type == "delm")
            {
                var mybom = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomid=" + mybomid);
                var old = ServiceDB.Instance.QueryOneModel<BomCost>("select * from BomCost where costId=" + id);
                row = ServiceDB.Instance.ExecuteSqlCommand("delete BomCost where costId=" + id);
                if (row == 1)
                {
                    newcost = bom.rootCost - (old.price * amount);
                    r.status = true; r.message = "BOM单价删除成功";
                }
            }
            else if (type == "delk")
            {
                var old = ServiceDB.Instance.QueryOneModel<BomVirtual>("select * from BomVirtual where virtualId=" + id);
                row = ServiceDB.Instance.ExecuteSqlCommand("delete BomVirtual where virtualId=" + id);
                if (row == 1)
                {
                    newcost = bom.rootCost - (old.vPrice * old.vAmount);
                    r.status = true; r.message = "BOM成本项目删除成功";
                }
            }
            else { r.status = false; r.message = type + "操作类别错误"; }
            if (r.status)
            {
                if (newcost < 0) newcost = 0;
                ServiceDB.Instance.ExecuteSqlCommand("update BomMain set rootCost=" + newcost + " where bomid=" + forbom);
                r.value = Math.Round(newcost, 4).ToString();

            }
            #endregion

            return Json(r, JsonRequestBehavior.AllowGet);
        }


        [AjaxAction(ForAction = "bomcost", ForController = "cost")]
        public ActionResult updatebomcost()
        {
            string forids = WebRequest.GetString("forids");
            string[] ids = forids.Split(',');
            int count = 0;
            foreach (string item in ids)
            {
                if (item == "") continue;
                System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[1];
                System.Data.SqlClient.SqlParameter sp = new System.Data.SqlClient.SqlParameter("@bomid", item);
                param[0] = sp;// new System.Data.SqlClient.SqlParameter { Direction = System.Data.ParameterDirection.Input, Value = item, };
                var bom = ServiceDB.Instance.ExecuteSqlCommand("exec usp_update_bomcost @bomid ", param);
                count++;
            }
            ReturnValue r = new ReturnValue();
            r.status = count > 0;
            r.message = r.status ? "" : "BOM成本更新失败";
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  应收应付明细
        [AjaxAction(ForAction = "receivable,paylist", ForController = "cost")]
        public ActionResult settledetail(string settleno, int type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var one = ServiceDB.Instance.QueryOneModel<Settlement>(" select * from Settlement  where settleNo='" + settleno + "' and settleType=" + type);
            var list = ServiceDB.Instance.QueryModelList<SettlementDetail>(" select * from SettlementDetail  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
            data.list = list;
            data.settleno = settleno;
            data.one = one;
            data.type = type;
            return View(data);
        }
        [AjaxAction(ForAction = "receivable,paylist", ForController = "cost")]
        public ActionResult settlecapital(string settleno, int type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = ServiceDB.Instance.QueryModelList<SettlementCapital>(" select * from SettlementCapital  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
            data.list = list;
            data.settleno = settleno;
            data.type = type;
            return View(data);
        }
        [AjaxAction(ForAction = "receivable,paylist", ForController = "cost")]
        public ActionResult capitallist(string settleno, int type)
        {
            var list = ServiceDB.Instance.QueryModelList<SettlementCapital>(" select * from SettlementCapital  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
            var r = list.Select(x => new { badCost = x.badCost.ToString("f2"), createDate = x.createDate.ToString("yyyy-MM-dd"), createStaff = x.createStaff, otherCost = x.otherCost.ToString("f2"), remark = x.remark, settleNo = x.settleNo, supplierName = x.supplierName, tradeCost = x.tradeCost.ToString("f2"), tradeDate = x.tradeDate.ToString("yyyy-MM-dd") }).ToList();
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "receivable,paylist", ForController = "cost")]
        public ActionResult gosettle()
        {
            ReturnValue back = new ReturnValue { status = false };
            string no = WebRequest.GetString("no", true);
            string forids = WebRequest.GetString("forids", true);
            string costs = WebRequest.GetString("costs", true);
            string date = WebRequest.GetString("date", true);
            string remark = WebRequest.GetString("remark", true);
            float cost = WebRequest.GetFloat("cost", 0);
            var one = ServiceDB.Instance.QueryOneModel<Settlement>(" select * from Settlement  where settleNo='" + no + "'");
            if (one == null)
            {
                back.message = "不存在结算单";
                return Json(back, JsonRequestBehavior.AllowGet);
            }
            string[] snlist = forids.Split(',');
            string[] costlist = costs.Split(',');
            if (snlist.Length != costlist.Length || snlist.Length < 1)
            {
                back.message = "传入参数数组有误";
                return Json(back, JsonRequestBehavior.AllowGet);
            }
            string sql = "";
            #region sql集合
            sql += "insert into SettlementCapital values('" + one.settleNo + "'," + one.supplierId + ",'" + one.supplierName + "','" + date + "','" + Masterpage.CurrUser.name + "'," + cost + ",0,0,getdate(),'" + remark + "');";

            sql += "update Settlement set realCost+=" + cost + " where settleNo='" + no + "';";
            for (int i = 0; i < snlist.Length; i++)
            {
                sql += "update SettlementDetail set isSettle=1,realCost=" + costlist[i] + " where detailSn=" + snlist[i] + ";";
            }
            #endregion
            try
            {
                var insert3 = ServiceDB.Instance.ExecuteSqlCommand(sql);
                if (insert3 > 0)
                {
                    var list = ServiceDB.Instance.QueryModelList<SettlementDetail>(" select * from SettlementDetail  where settleNo='" + no + "' and isSettle=0").ToList();
                    if (list == null || list.Count < 1)
                    {
                        ServiceDB.Instance.ExecuteSqlCommand("update Settlement set isover=1, status=2  where settleNo='" + no + "';");
                    }
                }
                back.status = true;
                return Json(back, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                back.status = false;
                back.value = ex.Message;
                return Json(back, JsonRequestBehavior.AllowGet);
            }
        }

        [AjaxAction(ForAction = "receivable,paylist", ForController = "cost")]
        public ActionResult settleelx(string settleno, int type)
        {
            //var list = ServiceDB.Instance.QueryModelList<SettlementCapital>(" select * from SettlementCapital  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
            //var r = list.Select(x => new { badCost = x.badCost.ToString("f2"), createDate = x.createDate.ToString("yyyy-MM-dd"), createStaff = x.createStaff, otherCost = x.otherCost.ToString("f2"), remark = x.remark, settleNo = x.settleNo, supplierName = x.supplierName, tradeCost = x.tradeCost.ToString("f2"), tradeDate = x.tradeDate.ToString("yyyy-MM-dd") }).ToList();
            var one = ServiceDB.Instance.QueryOneModel<Settlement>(" select * from Settlement  where settleNo='" + settleno + "'");
            List<string> data = new List<string>();
            string msg = "";
            #region 导出
            if (type == 0)
            {
                #region 导出明细
                #region 导出收款
                var list = ServiceDB.Instance.QueryModelList<SettlementDetail>(" select * from SettlementDetail  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
                string[] head = new string[12] { "序号", "编码", "物料名称", "物料规格", "出库单号", "交易时间", "销售金额", "退单金额", "单价", "应收金额", "实收金额", "状态" };
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    var row = (i + 1).ToString() + "|";
                    row += p.materialNo + "|";
                    row += p.materialName + "|";
                    row += p.materialModel + "|";
                    row += p.stockNo + "|";
                    row += p.tradeDate.ToString("yyyy-MM-dd") + "|";
                    row += p.tradeAmount.ToString("n") + "|";
                    row += p.returnAmount.ToString("n") + "|";
                    row += p.tradePrice.ToString("n") + "|";
                    row += ((p.tradeAmount - p.returnAmount) * p.tradePrice).ToString("n") + "|";
                    row += p.realCost.ToString("n") + "|";
                    row += p.isSettle == 0 ? "未结" : "已结";
                    data.Add(row);
                }
                #endregion
                msg = FileHelper.ExportEasy(head, data);

                #endregion
            }
            else if (type == 1)
            {
                #region 导出收款
                var list = ServiceDB.Instance.QueryModelList<SettlementCapital>(" select * from SettlementCapital  where settleNo='" + settleno + "'  order by tradeDate desc").ToList();
                string[] head = new string[8] { "序号", (one.settleType == 0 ? "客户" : "供应商"), "创建人员", "结算单号", "金额", (one.settleType == 0 ? "收款" : "付款") + "日期", "创建时间", "备注" };
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    var row = (i + 1).ToString() + "|";
                    row += p.supplierName + "|";
                    row += p.createStaff + "|";
                    row += p.settleNo + "|";
                    row += p.tradeCost + "|";
                    row += p.tradeDate.ToString("yyyy-MM-dd") + "|";
                    row += p.createDate.ToString("yyyy-MM-dd") + "|";
                    row += p.remark;
                    data.Add(row);
                }
                #endregion
                msg = FileHelper.ExportEasy(head, data);

            }
            else msg = "类型错误";
            return Content(msg);
            #endregion
        }
        #endregion
    }
}
