using ecoBio.Wms.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ecoBio.Wms.Common;
using ecoBio.Wms.Data.Entities.Models;
using ecoBio.Wms.Service.Monitor;
using ecoBio.Wms.Repositories;
using Calabonga.Mvc.PagedListExt;
using System.Configuration;
using System.Text;

namespace ecoBio.Wms.Web.Controllers
{
    public class materialController : BaseController
    {
        private ManagementService managementService;
        private MaterialService materialService;
        private CostanalysisService costanalysisService;
        public materialController(IMaterialRepository _materialRepository, ICostanalysisRepository _costanalysisRepository,IManagementRepository _managementRepository)
        {
            materialService = new MaterialService(_materialRepository);
            costanalysisService = new CostanalysisService(_costanalysisRepository);
            managementService = new ManagementService(_managementRepository);
        }
        #region 库存

        /// <summary>
        /// 耗材库存
        /// </summary>
        /// <returns></returns>
        public ActionResult stock(int? page, string material)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            // if (material != null) material = WebRequest.GetString(material, true);
            data.material = material;
            var list = materialService.GetMaterialStockF_T(Masterpage.CurrUser.client_code, material);
            #region 计算总金额和报警数量
            double amount_sum = 0;
            int alarmcount = 0;
            foreach (var item in list)
            {
                if (item.inventory_amount < item.inventory_safety_amount) alarmcount++;
                amount_sum += item.inventory_sum;
            }
            #endregion
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&material=" + material;
            data.amount_sum = amount_sum;
            data.alarmcount = alarmcount;

            var ddl = materialService.GetMaterialSpecificationDDL(Masterpage.CurrUser.client_code, material);
            data.ddl = ddl;
            LogHelper.Info(Masterpage.CurrUser.alias, "301011:客户," + Masterpage.CurrUser.client_code + ",库存列表带金额，第" + _page + "页");
            return View(data);
        }

        /// <summary>
        /// 耗材库存 不带金额
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult inventory(int? page)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var material = WebRequest.GetString("material", true);
            data.material = material;
            var list = materialService.GetMaterialStockF_T(Masterpage.CurrUser.client_code, material);
            #region 计算总金额和报警数量
            int alarmcount = list.Where(p => p.inventory_amount < p.inventory_safety_amount).ToList().Count;

            #endregion
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&material=" + material;
            data.alarmcount = alarmcount;
            var ddl = materialService.GetMaterialSpecificationDDL(Masterpage.CurrUser.client_code, material);
            data.ddl = ddl;
            LogHelper.Info(Masterpage.CurrUser.alias, "301011:客户," + Masterpage.CurrUser.client_code + ",库存列表不带金额，第" + _page + "页");
            return View(data);
        }

        //stockdetail      
        [AjaxAction(ForAction = "input,inventory", ForController = "material")]
        public ActionResult stockdetail()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var code = WebRequest.GetString("code", true);
            var t = WebRequest.GetString("t", true);

            var list = materialService.GetStockMaterialDetail(Masterpage.CurrUser.client_code, code);
            data.list = list;
            data.t = t;
            LogHelper.Info(Masterpage.CurrUser.alias, "301012:客户," + Masterpage.CurrUser.client_code + ",耗材" + code + "的库存明细查看");
            return PartialView(data);
        }
        #endregion

        #region 入库
        /// <summary>
        /// 耗材入库
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult input(int? page)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            var message = WebRequest.GetString("message", true);
            data.in_no = in_no;
            var list = materialService.GetStockInListModel(Masterpage.CurrUser.client_code, in_no);
            data.stophour = materialService.StopEditHour();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.code = Masterpage.CurrUser.client_code;
            data.message = message;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&in_no=" + in_no;
            LogHelper.Info(Masterpage.CurrUser.alias, "302011:客户," + Masterpage.CurrUser.client_code + ",入库单列表，第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult inputdetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            data.in_no = in_no.Replace(Masterpage.CurrUser.client_code + "-", "");
            var input = materialService.GetOneStockInListModel(Masterpage.CurrUser.client_code, in_no);
            data.input = input;
            var list = materialService.GetStockInDetail(Masterpage.CurrUser.client_code, in_no);
            data.list = list;
            LogHelper.Info(Masterpage.CurrUser.alias, "302012:客户," + Masterpage.CurrUser.client_code + ",入库单" + in_no + "明细查看");
            return PartialView(data);
        }

        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult inputdetailview2()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            data.in_no = in_no.Replace(Masterpage.CurrUser.client_code + "-", "");
            var input = materialService.GetOneStockInListModel(Masterpage.CurrUser.client_code, in_no);
            data.input = input;
            var list = materialService.GetStockInDetail(Masterpage.CurrUser.client_code, in_no);
            data.logo = Masterpage.CurrUser.client_logo;
            data.code = Masterpage.CurrUser.client_code;
            data.list = list;
            LogHelper.Info(Masterpage.CurrUser.alias, "302012:客户," + Masterpage.CurrUser.client_code + ",入库单" + in_no + "明细模板查看");
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult inputdetail()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            var type = WebRequest.GetString("type", true);
            var list = new List<StockInDetailModel>();
            var input = new StockInListModel();
            //var stophour = materialService.StopEditHour();
            var material = materialService.GetMaterialSpecificationDDL(Masterpage.CurrUser.client_code);
            //var supplier = materialService.GetSupplierDDL();
            var newin_no = "";
            var message = "";
            var order = "";
            if (type == "add")
            {
                newin_no = materialService.CreateNewStockInNo(Masterpage.CurrUser.client_code);
                newin_no = newin_no.Substring(0, newin_no.Length - 4);
                newin_no = newin_no.Replace(Masterpage.CurrUser.client_code + "-", "");
                input = null;
                LogHelper.Info(Masterpage.CurrUser.alias, "302015:客户," + Masterpage.CurrUser.client_code + ",新增入库单" + newin_no);
            }
            else if (type == "edit")
            {
                #region 查找一个默认订单
                var had = materialService.GetStockInHadOrder(Masterpage.CurrUser.client_code, in_no);
                if (had != null && had.Count > 0) order = had[0].text;
                else
                {
                    var all = materialService.GetCanUserOrder(Masterpage.CurrUser.client_code, "");
                    if (all != null && all.Count > 0) order = all[0].text;
                }
                #endregion
                input = materialService.GetOneStockInListModel(Masterpage.CurrUser.client_code, in_no);
                list = materialService.GetStockInDetail(Masterpage.CurrUser.client_code, in_no);
                SessionHelper.SetSession(in_no, list);
                LogHelper.Info(Masterpage.CurrUser.alias, "302015:客户," + Masterpage.CurrUser.client_code + ",编辑入库单" + in_no);
            }
            else if (type == "createinno")
            {
                #region 创建入库单

                var in_sn = WebRequest.GetString("in_sn", true);
                var date = WebRequest.GetString("date", true);
                var person = WebRequest.GetString("person", true);
                if (in_sn.Length > 20)
                {
                    message = "入库单流水号长度错误";
                    LogHelper.Info(Masterpage.CurrUser.alias, "302015:客户," + Masterpage.CurrUser.client_code + ",创建一个入库单失败，入库单流水号长度错误");
                    type = "add";
                }
                else
                {
                    string backno = materialService.AddStockInList(Masterpage.CurrUser.client_code, in_sn, Masterpage.CurrUser.guid, person, date);
                    if (backno == "")
                    {
                        message = "入库单创建失败";
                        type = "add";
                        LogHelper.Info(Masterpage.CurrUser.alias, "302015:客户," + Masterpage.CurrUser.client_code + ",创建一个入库单失败");
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "302015:客户," + Masterpage.CurrUser.client_code + ",创建一个入库单" + in_no);
                        return RedirectToAction("inputdetail", new { type = "edit", in_no = backno });
                    }
                }

                #endregion
            }
            else
            {
                return RedirectToAction("input");
            }
            data.message = message;
            data.type = type;
            //data.stophour = stophour;
            data.material = material;
            data.input = input;
            data.newin_no = newin_no;
            data.order = order;
            //data.list = JsonHelper.ToJson(list);   
            return View(data);
        }

        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult indetailtemp(string in_no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockInDetailModel>();
            list = (List<StockInDetailModel>)SessionHelper.GetSession(in_no);
            data.list = list;
            return PartialView("indetailtemp", data);
        }
        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="in_no"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpPost]
        public ActionResult inputdelete()
        {
            var in_no = WebRequest.GetString("in_no", true);
            bool dr = materialService.DeleteStockIn(Masterpage.CurrUser.client_code, in_no);
            LogHelper.Info(Masterpage.CurrUser.alias, "302013:客户," + Masterpage.CurrUser.client_code + ",入库单" + in_no + "删除入库单");
            return Content(dr ? "ok" : "error");
        }

        /// <summary>
        /// 过滤订单号
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpGet]
        public ActionResult orderlist()
        {
            string key = WebRequest.GetQueryString("order", true);
            string m = WebRequest.GetQueryString("material", true);
            var list = materialService.GetPriceManagement(Masterpage.CurrUser.client_code, m, key);
            var r = list.Where(p => p.ChemicalPoInputIsFinish.Value == false).Select(x => new { value = x.ChemicalPoItemsNo, key = x.ChemicalPoItemsNo, count = x.ChemicalPoAmount.Value, item_no = x.ChemicalPoNo });
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加入库明细
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpPost]
        public ActionResult addinputdetail()
        {
            string order = WebRequest.GetString("order", true);
            string type = WebRequest.GetString("type", true);
            string guid = WebRequest.GetString("guid", true);
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string sup = WebRequest.GetString("sup", true);
            string in_no = WebRequest.GetString("in_no", true);
            int int_count = Convert.ToInt32(count);
            Guid g = Guid.NewGuid();
            ReturnValue nguid;
            if (type == "add" && guid == "")
            {
                nguid = materialService.AddStockInDetail(Masterpage.CurrUser.client_code, in_no, m, int_count, g, order);
            }
            else
            {
                Guid d = Guid.Parse(guid);
                nguid = materialService.UpdateStockInDetail(Masterpage.CurrUser.client_code, in_no, d, m, int_count, g, order);
            }
            return Json(nguid, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除入库明细
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpPost]
        public ActionResult deleteinputdetail()
        {
            string in_no = WebRequest.GetString("in_no", true);
            string detail = WebRequest.GetString("detail", true);
            Guid g = Guid.Parse(detail);
            bool result = materialService.DeleteStockInDetail(Masterpage.CurrUser.client_code, in_no, g);
            return Content(result ? "ok" : "error");
        }
        #region 2013年6月26日修改
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult inorderlist(int? page, int? pagesize, string in_no, string query)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = materialService.GetStockInHadOrder(Masterpage.CurrUser.client_code, in_no);
            var all = materialService.GetCanUserOrder(Masterpage.CurrUser.client_code, query);
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

            data.code = Masterpage.CurrUser.client_code;
            data.in_no = in_no;
            data.query = query;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView("inorderlist", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult indetailtab(string order, string in_no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var had = materialService.GetStockInDetailByOrder(Masterpage.CurrUser.client_code, order, in_no);
            var orderdetail = costanalysisService.GetChemicalOrderDetaillist(Masterpage.CurrUser.client_code, order).ToList();
            if (had != null && had.Count > 0)
            {
                foreach (var item in orderdetail)
                {
                    var h = had.FirstOrDefault(p => p.ChemicalPoItemsNo == item.ChemicalPoItemsNo);
                    if (h != null)
                    {
                        //item.ChemicalPoInputAmount -= h.StockInputAmount;
                        item.StockInputAmount = h.StockInputAmount;
                        item.StockRemainAmount = h.StockRemainAmount;
                    }
                }
            }
            data.in_no = in_no;
            data.order = order;
            data.list = orderdetail;
            return PartialView("indetailtab", data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult savestockindetail(string in_no)
        {
            //if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            //if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
            //string[] ds = details.Split(',');
            //string[] cs = counts.Split(',');
            //var r = materialService.SaveStockInDetail(Masterpage.CurrUser.client_code, in_no, order, ds, cs);
            //return Json(r, JsonRequestBehavior.AllowGet);

            ReturnValue r = new ReturnValue();
            var list = new List<StockInDetailModel>();
            list = (List<StockInDetailModel>)SessionHelper.GetSession(in_no);
            if (list.Count == 0)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "302014:客户," + Masterpage.CurrUser.client_code + ",入库单" + in_no + "保存入库单明细失败，未提交任何订单明细");
                r = new ReturnValue { status = "error", message = "未提交任何订单明细" };
            }
            else
            {
                r = materialService.SaveStockInDetail(Masterpage.CurrUser.client_code, in_no, list);
                LogHelper.Info(Masterpage.CurrUser.alias, "302014:客户," + Masterpage.CurrUser.client_code + ",入库单" + in_no + "保存入库单明细" + JsonHelper.ToJson(list));

            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }

        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult deletedetailtemp(string in_no, string detailno)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockInDetailModel>();
            list = (List<StockInDetailModel>)SessionHelper.GetSession(in_no);
            var had = list.FirstOrDefault(p => p.purchase_details_order_no == detailno);
            if (had != null)
            {
                had.stock_in_details_amount = 0;
                //list.Remove(had);
            }
            SessionHelper.SetSession(in_no, list);
            return Content(list.Count.ToString());
        }
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult savestockindetailtemp(string order, string in_no, string details, string counts)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<StockInDetailModel>();
            list = (List<StockInDetailModel>)SessionHelper.GetSession(in_no);
            if (details.EndsWith(",")) details = details.Substring(0, details.Length - 1);
            if (counts.EndsWith(",")) counts = details.Substring(0, counts.Length - 1);
            string[] ds = details.Split(',');
            string[] cs = counts.Split(',');

            for (int i = 0; i < ds.Length; i++)
            {
                double fc = Convert.ToDouble(cs[i]);
                var had = list.FirstOrDefault(p => p.purchase_details_order_no == ds[i]);
                if (had != null)
                {
                    var chage = fc - had.stock_in_details_amount;
                    had.stock_in_details_amount = fc;
                    had.stock_in_details_reserve_amount += chage;
                }
                else
                {
                    if (fc == 0) continue;
                    var m = costanalysisService.GetOneChemicalOrderDetail(Masterpage.CurrUser.client_code, order, ds[i]);
                    list.Add(new StockInDetailModel
                    {
                        material_model = m.material_specification_model,
                        material_name_html = m.material_specification_html,
                        MaterialSpecificationCode = m.MaterialSpecificationCode,
                        purchase_details_order_no = ds[i],
                        purchase_amount = m.ChemicalPoAmount,
                        order_inamount = m.ChemicalPoInputAmount,
                        order_amount = m.ChemicalPoAmount.Value,
                        customer_code = m.CustomerCode,
                        stock_in_details_amount = fc,
                        stock_in_details_reserve_amount = fc,
                        purchase_order_no = order

                    });
                }
            }
            SessionHelper.SetSession(in_no, list);

            data.list = list;
            return PartialView("indetailtemp", data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult queryorder(string key)
        {
            var all = materialService.GetCanUserOrder(Masterpage.CurrUser.client_code, key);
            var r = all.Select(x => new { value = x.text.Replace(Masterpage.CurrUser.client_code + "-", ""), key = x.text.Replace(Masterpage.CurrUser.client_code + "-", "") });
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "input", ForController = "material")]
        public ActionResult inputdown()
        {
            string no = WebRequest.GetString("no", true);
            #region 查找
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/stockin/" + no + ".pdf";
                FileHelper.DownLoadFile(path, no + ".pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "302018:客户," + Masterpage.CurrUser.client_code + ",耗材入库单" + no + ",耗材入库单下载");
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "302018:客户," + Masterpage.CurrUser.client_code + ",耗材入库单" + no + ",耗材入库单下载失败" + ex.Message);
                return Content("文件不存在");
            }
            #endregion
            return View();
        }
        #endregion

        [Anonymous]
        public ActionResult pdfin(string para)
        {
            //para='+$customer+'_'+$logo+'_'+$inno+'_'+$indate+'_'+$person+'_'+$insn+'_'+$detailstr
            //dgklsz_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg_szkl20136045_2013-08-01_蒙祖林_ls20130920_1,d2,m3,w4,n6,9,11,b#2,d22,m23,w24,n26,29,211,2b
            //string 客户号0, string logo图片1, string 入库单号2, string 入库日期3, string 入库人4, string 入库流水号5, string 入库单明细6
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                string[] t = para.Split('_');
                string host = Utils.GetHostHead();
                string code = t[0];
                data.logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + t[1];
                data.inno = t[2].Replace(code + "-", "");
                data.date = t[3];
                data.person = t[4];
                data.sn = t[5];
                #region 第15行开始明细
                StringBuilder trs = new StringBuilder();
                string[] ds = t[6].Split('@');
                for (int i = 0; i < ds.Length; i++)
                {
                    if (i > 20) break;
                    //序号 	订单号 	订单明细号 	物料编码 	物料名称 	入库数量 	入库存量 	备注	
                    var one = ds[i].Split(',');
                    trs.Append("<tr>");
                    for (int j = 0; j < one.Length; j++)
                    {
                        if (j > 7) break;
                        trs.Append("<td class=\"endwise_title\">" + one[j] + "</td>");
                    }
                    trs.Append("</tr>");
                }                
                #endregion
                data.rows = ds.Length;
                data.trs = trs.ToString();             
            }
            catch
            {
                data.inno = ""; data.date = ""; data.logo = ""; data.rows = 0; data.trs = ""; data.person = ""; data.sn = "";
            }

            return View(data);
        }

        #endregion

        #region 出库

        /// <summary>
        /// 耗材出库
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult output(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            var message = WebRequest.GetString("message", true);
            data.in_no = in_no;
            var list = materialService.GetStockOutListModel(Masterpage.CurrUser.client_code, in_no);
            data.stophour = materialService.StopEditHour();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.message = message;
            data.code = Masterpage.CurrUser.client_code;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&in_no=" + in_no;
            LogHelper.Info(Masterpage.CurrUser.alias, "303011:客户," + Masterpage.CurrUser.client_code + ",出库单列表,第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        public ActionResult outputdetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            data.in_no = in_no.Replace(Masterpage.CurrUser.client_code + "-", "");
            var input = materialService.GetOneStockOutListModel(Masterpage.CurrUser.client_code, in_no);
            data.input = input;
            var list = materialService.GetStockOutDetail(Masterpage.CurrUser.client_code, in_no);
            data.list = list;
            LogHelper.Info(Masterpage.CurrUser.alias, "303012:客户," + Masterpage.CurrUser.client_code + ",出库单" + in_no + "明细查看");
            return PartialView(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        public ActionResult outputdetail(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var in_no = WebRequest.GetString("in_no", true);
            var type = WebRequest.GetString("type", true);
            var list = new List<StockOutDetailModel>();
            var input = new StockInListModel();
            var stophour = materialService.StopEditHour();
            var material = materialService.GetCustomerInInventoriesDDL(Masterpage.CurrUser.client_code);
            var newin_no = "";
            var message = "";
            if (type == "add")
            {
                newin_no = materialService.CreateNewStockOutNo(Masterpage.CurrUser.client_code);
                newin_no = newin_no.Substring(0, newin_no.Length - 4);
                newin_no = newin_no.Replace(Masterpage.CurrUser.client_code + "-", "");
                input = null;
            }
            else if (type == "edit")
            {
                input = materialService.GetOneStockOutListModel(Masterpage.CurrUser.client_code, in_no);
                list = materialService.GetStockOutDetail(Masterpage.CurrUser.client_code, in_no);
                LogHelper.Info(Masterpage.CurrUser.alias, "303013:客户," + Masterpage.CurrUser.client_code + ",编辑出库单" + in_no + "信息");
            }
            else if (type == "createinno")
            {
                #region 创建入库单

                var in_sn = WebRequest.GetString("in_sn", true);
                var date = WebRequest.GetString("date", true);
                var person = WebRequest.GetString("person", true);
                if (in_sn.Length > 20)
                {
                    message = "出库单流水号长度错误";
                    LogHelper.Info(Masterpage.CurrUser.alias, "303013:客户," + Masterpage.CurrUser.client_code + ",出库单创建失败，出库单流水号长度错误");
                    type = "add";
                }
                else
                {
                    string backno = materialService.AddStockOutList(Masterpage.CurrUser.client_code, in_sn, Masterpage.CurrUser.guid, person, date);
                    if (backno == "")
                    {
                        message = "出库单创建失败";
                        type = "add";
                        LogHelper.Info(Masterpage.CurrUser.alias, "303013:客户," + Masterpage.CurrUser.client_code + ",出库单创建失败");
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "303013:客户," + Masterpage.CurrUser.client_code + ",创建出库单，单号：" + backno);
                        return RedirectToAction("outputdetail", new { type = "edit", in_no = backno });
                    }
                }

                #endregion
            }
            else
            {
                return RedirectToAction("input");
            }
            data.message = message;
            data.type = type;
            data.stophour = stophour;
            data.material = material;
            data.input = input;
            data.newin_no = newin_no;
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&type=edit&in_no=" + in_no;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        public ActionResult queryindetail()
        {
            var m = WebRequest.GetString("m", true);
            var guid = WebRequest.GetString("guid", true);
            var list = materialService.GetStockInDetailByM(Masterpage.CurrUser.client_code, m).Select(x => new StockInOutPage
            {

                stock_in_details_guid = x.stock_in_details_guid,
                stock_in_no = x.stock_in_no,
                MaterialSpecificationCode = x.MaterialSpecificationCode,
                purchase_amount = x.purchase_amount,
                indate = x.indate.ToString("yyyy-MM-dd"),
                supplier_name = x.supplier_name,
                stock_in_details_amount = x.stock_in_details_amount,
                stock_in_details_reserve_amount = x.stock_in_details_reserve_amount,
                had = 0,
                use = 0
            }).ToList();
            if (guid != null && guid.ToString() != "")
            {
                Guid g = Guid.Parse(guid);
                var had = materialService.GetStockInOutAssociate(g).ToList();
                foreach (var item in list)
                {
                    var o = had.FirstOrDefault(p => p.StockInDetailsGuid == item.stock_in_details_guid);
                    if (o != null)
                    {
                        item.had = 1;
                        item.use = o.StockInOutVolume;
                    }
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]

        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpPost]
        public ActionResult saveoutputdetailtemp()
        {


            // string select = WebRequest.GetString("select", true);
            //string type = WebRequest.GetString("type", true);
            //string guid = WebRequest.GetString("guid", true);
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string in_no = WebRequest.GetString("in_no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            //string arr = WebRequest.GetString("arr", true);
            string[] nm = name.Split(' ');
            name = nm[0];
            string model = nm[1];
            List<StockOutDetailModeltemp> temp = (List<StockOutDetailModeltemp>)SessionHelper.GetSession(in_no);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<StockOutDetailModeltemp>();
                temp.Add(new StockOutDetailModeltemp
                {
                    stock_out_guid = in_no,
                    material_name = name,
                    type = "add",
                    material_model = model,
                    MaterialSpecificationCode = m,
                    stock_out_details_amount = int_count,
                    stock_out_details_guid = Guid.Empty,
                    ass_str = "新增"
                });
            }
            else
            {
                var ht = temp.FirstOrDefault(p => p.MaterialSpecificationCode == m);
                if (ht != null)
                {
                    if (ht.stock_out_details_guid != Guid.Empty) ht.type = "edit";
                    ht.stock_out_details_amount = int_count;
                }
                else
                {
                    temp.Add(new StockOutDetailModeltemp
                    {
                        stock_out_guid = in_no,
                        type = "add",
                        material_name = name,
                        material_model = model,
                        MaterialSpecificationCode = m,
                        stock_out_details_amount = int_count,
                        stock_out_details_guid = Guid.Empty,
                        ass_str = "新增"
                    });
                }
            }
            SessionHelper.SetSession(in_no, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "input", ForController = "material")]
        [HttpPost]
        public ActionResult saveoutputdetail()
        {
            string outno = WebRequest.GetString("outno", true);
            List<StockOutDetailModeltemp> temp = (List<StockOutDetailModeltemp>)SessionHelper.GetSession(outno);
            ReturnValue r = new ReturnValue();
            if (temp != null && temp.Count > 0)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "303014:客户," + Masterpage.CurrUser.client_code + ",修改出库单，单号：" + outno);
                r = materialService.UpdateStockOutDetail(Masterpage.CurrUser.client_code, outno, temp);
            }
            else
            {
                r.message = "不存在出库明细";
                r.status = "error";
            }
            SessionHelper.Del(outno);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="in_no"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        [HttpPost]
        public ActionResult outputdelete()
        {
            var in_no = WebRequest.GetString("in_no", true);
            bool dr = materialService.DeleteStockOut(Masterpage.CurrUser.client_code, in_no);
            LogHelper.Info(Masterpage.CurrUser.alias, "303015:客户," + Masterpage.CurrUser.client_code + ",删除出库单，单号：" + in_no + ",结果：" + dr);

            return Content(dr ? "ok" : "error");
        }
        /// <summary>
        /// 删除出库明细
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        [HttpPost]
        public ActionResult deleteoutputdetail()
        {
            string in_no = WebRequest.GetString("in_no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            Guid g = Guid.Parse(detail); 
            bool result = false;
            List<StockOutDetailModeltemp> temp = (List<StockOutDetailModeltemp>)SessionHelper.GetSession(in_no);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.MaterialSpecificationCode == m);

                if (h != null)
                {
                    if (g != Guid.Empty) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession(in_no, temp);
                }
            }
            //bool result = materialService.DeleteStockOutDetail(Masterpage.CurrUser.client_code, in_no, g);
            //LogHelper.Info(Masterpage.CurrUser.alias, "303015:客户," + Masterpage.CurrUser.client_code + ",删除出库明细，单号：" + in_no + "，出库明细" + detail + ",结果：" + result);
            return Content(result ? "ok" : "error");
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "output", ForController = "material")]
        public ActionResult sockoutdetail(string outno)
        {
            //SessionHelper.Del(outno);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = materialService.GetStockOutDetail(Masterpage.CurrUser.client_code, outno);
            List<StockOutDetailModeltemp> temp = (List<StockOutDetailModeltemp>)SessionHelper.GetSession(outno);
            List<StockOutDetailModeltemp> newlist = new List<StockOutDetailModeltemp>();
            if (temp!=null&&temp.Count>0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {                
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.MaterialSpecificationCode == item.MaterialSpecificationCode);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new StockOutDetailModeltemp
                        {
                            stock_out_guid = outno,
                            ass_str = p.ass_str,
                            material_model = p.material_model,
                            material_name = p.material_name,
                            MaterialSpecificationCode = p.MaterialSpecificationCode,
                            stock_out_details_amount = p.stock_out_details_amount,
                            stock_out_details_guid = p.stock_out_details_guid,
                            type = ""
                        }).ToList());
                }
            }
            data.list = newlist;
            data.outno = outno;
            LogHelper.Info(Masterpage.CurrUser.alias, "303016:客户," + Masterpage.CurrUser.client_code + ",查看出库明细，单号：" + outno);
            SessionHelper.SetSession(outno, newlist);
            return PartialView("sockoutdetail", data);
        }
        [AjaxAction(ForAction = "output", ForController = "material")]
        public ActionResult outputdown()
        {
            string no = WebRequest.GetString("no", true);
            #region 查找
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/stockin/" + no + ".pdf";
                FileHelper.DownLoadFile(path, no + ".pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "302028:客户," + Masterpage.CurrUser.client_code + ",耗材出库单" + no + ",耗材出库单下载");
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "302028:客户," + Masterpage.CurrUser.client_code + ",耗材出库单" + no + ",耗材出库单下载失败" + ex.Message);
                return Content("文件不存在");
            }
            #endregion
            return View();
        }

        [Anonymous]
        public ActionResult pdfout(string para)
        {
            //para='+$customer+'_'+$logo+'_'+$inno+'_'+$indate+'_'+$person+'_'+$insn+'_'+$detailstr
            //dgklsz_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg_szkl20136045_2013-08-01_蒙祖林_ls20130920_1,d2,m3,w4,n6,9,11,b#2,d22,m23,w24,n26,29,211,2b
            //string 客户号0, string logo图片1, string 入库单号2, string 入库日期3, string 入库人4, string 入库流水号5, string 入库单明细6
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                string[] t = para.Split('_');
                string host = Utils.GetHostHead();
                string code = t[0];
                data.logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + t[1];
                data.inno = t[2].Replace(code + "-", "");
                data.date = t[3];
                data.person = t[4];
                data.sn = t[5];
                #region 第15行开始明细
                StringBuilder trs = new StringBuilder();
                string[] ds = t[6].Split('@');
                for (int i = 0; i < ds.Length; i++)
                {
                    if (i > 20) break;
                    //序号 	耗材规格 	耗材数量 	关联入库单 	备注	
                    var one = ds[i].Split(',');
                    trs.Append("<tr>");
                    for (int j = 0; j < one.Length; j++)
                    {
                        if (j > 7) break;
                        trs.Append("<td class=\"endwise_title\">" + one[j] + "</td>");
                    }
                    trs.Append("</tr>");
                }
                #endregion
                data.rows = ds.Length;
                data.trs = trs.ToString();
            }
            catch
            {
                data.inno = ""; data.date = ""; data.logo = ""; data.rows = 0; data.trs = ""; data.person = ""; data.sn = "";
            }

            return View(data);
        }

        #endregion


        #region 盘点
        ///// <summary>
        ///// 耗材盘点
        ///// </summary>
        ///// <param name="txtkey"></param>
        ///// <returns></returns>
        //public ActionResult check(int? page)
        //{
        //        dynamic data = new System.Dynamic.ExpandoObject();
        //        var material = WebRequest.GetString("material", true);
        //        data.material = material;
        //        var list = materialService.GetMaterialStockF_T(Masterpage.CurrUser.client_code, material);

        //        int _page = page.HasValue ? page.Value : 1;
        //        int _pagesize = 14;
        //        var vs = list.ToPagedList(_page, _pagesize);
        //        var c = materialService.CheckStockCan();
        //        var stop =  c ? 0 : 1;
        //        data.stop = stop;
        //        data.list = vs;
        //        data.pageSize = _pagesize;
        //        data.pageIndex = _page;
        //        data.totalCount = vs.TotalCount;
        //        data.otherParam = "&material=" + material;
        //        var ddl = materialService.GetMaterialSpecificationDDL(Masterpage.CurrUser.client_code, material);
        //        data.ddl = ddl;
        //        return View(data);            
        //}

        ///// <summary>
        ///// 盘点数量
        ///// </summary>
        ///// <returns></returns>
        //[HidNowLocal]
        //[AjaxAction(ForAction = "check", ForController = "material")]
        //[HttpPost]
        //public ActionResult checkcount()
        //{
        //    var guid = WebRequest.GetString("guid", true);
        //    var count = WebRequest.GetString("count", true);
        //    int c = 0;
        //    Guid g;
        //    try
        //    {
        //        g = Guid.Parse(guid);
        //        c = Convert.ToInt32(count);
        //    }
        //    catch
        //    {
        //        return Json(new ReturnValue { value = "", status = "error", message = "输入参数有误！" }, JsonRequestBehavior.AllowGet);
        //    }

        //    var dr = materialService.CheckStock(Masterpage.CurrUser.client_code, g, c);
        //    return Json(dr, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region 耗材趋势
        /// <summary>
        /// 耗材趋势
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult trend()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "304011:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表");
            return View();
        }

        /// <summary>
        /// 工艺监管中工艺参数的查询分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="key">查询关键字</param>
        /// <param name="select">以选参数,英文逗号分隔</param>
        /// <param name="c">请求的controller</param>
        /// <param name="a">请求的action</param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "trend,nutrients", ForController = "material")]
        public ActionResult StockQuery(int? page, int? pagesize, string key, string select, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var material = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct();
            List<MaterialSpecification> list = new List<MaterialSpecification>();
            if (t != "")
                list = material.Where(p => p.MaterialSpecificationCategeory == t).ToList();
            else
                list = material.ToList();
            if (key != null && key.ToString() != "") list = list.Where(p => p.MaterialSpecificationName.Contains(key) || p.MaterialSpecificationName.Contains(key.ToLower()) || p.MaterialSpecificationName.Contains(key.ToUpper())).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            #region 统计数据图表
            if ((select == null || select.ToString() == "") && vs.Count > 0)
            {
                data.select = (vs[0].MaterialSpecificationCode);
            }
            else
            {
                data.select = select;
            }
            #endregion
            data.key = key;
            data.t = t;
            LogHelper.Info(Masterpage.CurrUser.alias, "304011:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表,耗材：" + select);
            return PartialView("StockQuery", data);
        }

        /// <summary>
        /// 过滤关键字
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "trend,nutrients", ForController = "material")]
        [HttpGet]
        public ActionResult filterparam()
        {
            string t = WebRequest.GetQueryString("t", true);
            string key = WebRequest.GetQueryString("key", true);
            List<object> r = new List<object>();
            var material = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct();
            var list = material.Where(p => p.MaterialSpecificationName.Contains(key) || p.MaterialSpecificationName.Contains(key.ToLower()) || p.MaterialSpecificationName.Contains(key.ToUpper()));
            if (t != "") list = list.Where(p => p.MaterialSpecificationCategeory == t);
            var result = list.Select(x => new { text = x.MaterialSpecificationName, value = x.MaterialSpecificationName }).Distinct();
            LogHelper.Info(Masterpage.CurrUser.alias, "304012:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表,过滤耗材：" + key);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新图表和统计数据
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "trend,nutrients", ForController = "material")]
        [HttpGet]
        public ActionResult upchartandgrid(string select, string t, string chartnumber)
        {
            #region 统计数据
            var statistics = materialService.GetStockStatistic(Masterpage.CurrUser.client_code, select, DateTime.Now.Year.ToString());
            #endregion

            #region 图表
            var chart = materialService.GetFlexChart(chartnumber);
            chart.charttype = chart.standardtype;
            if (t != null && t != "")
                chart.title = t + "耗材趋势";
            else chart.title = "耗材趋势";
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.processparms = select + "|" + DateTime.Now.Year.ToString();
            chart.url = Utils.GetFlexAddress();
            #endregion
            var result = new { grid = statistics, chart = chart };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 生物营养剂趋势
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult nutrients()
        {

            return View();
        }

        [LoginAllow]
        public ActionResult fullscreen()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string title = WebRequest.GetString("t", true);

            string w = WebRequest.GetString("w", true);
            string h = WebRequest.GetString("h", true);
            string n = WebRequest.GetString("n", true);
            string m = WebRequest.GetString("m", true);
            string acl = WebRequest.GetString("acl", true);
            string acr = WebRequest.GetString("acr", true);
            string line = WebRequest.GetString("line", true);
            #region 图表
            var one = materialService.GetOneMaterialSpecification(m);
            var chart = materialService.GetFlexChart(n);
            chart.charttype = line;
            chart.title = (one != null ? one.MaterialSpecificationName : "") + title;
            chart.height = int.Parse(h);
            chart.width = int.Parse(w);
            chart.leftprecision = int.Parse(acl);
            chart.rightprecision = int.Parse(acr);
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.processparms = m + "|" + DateTime.Now.Year.ToString();
            chart.url = Utils.GetFlexAddress();
            #endregion
            data.chart = JsonHelper.ToJson(chart);
            LogHelper.Info(Masterpage.CurrUser.alias, "304012:客户," + Masterpage.CurrUser.client_code + ",查看全屏图表，耗材为：" + m);
            return View("chartfullscreen", data);
        }
        #endregion

        #region 报表

        /// <summary>
        /// 耗材报表
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult report(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);

            var path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/inventory_y/";
            //var path = "~/res/" + Masterpage.CurrUser.client_code + "/files/inventory_y/";
            var list = materialService.GetPdfFileList(path);
            if (key != null && key != "") list = list.Where(p => p.name.Contains(key)).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 24;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            // data.ddl = ddl;
            data.key = key;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            if (key != null && key.ToString() != "")
                data.otherParam = "&key=" + key;
            else data.otherParam = "";
            LogHelper.Info(Masterpage.CurrUser.alias, "305011:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表带金额");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "report", ForController = "material")]
        public ActionResult reportview(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var date = WebRequest.GetString("date", true);

            var list = materialService.GetInventoryReportModel(Masterpage.CurrUser.client_code, date);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 30;
            var vs = list.ToPagedList(_page, _pagesize);
            data.title = Masterpage.CurrUser.client_name + date.Substring(0, 4) + "年" + date.Substring(4, 2) + "月库存报表";
            data.list = vs;
            data.date = date;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&date=" + date;
            LogHelper.Info(Masterpage.CurrUser.alias, "305012:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表明细带金额");
            return View(data);
        }
        [AjaxAction(ForAction = "report", ForController = "material")]
        public ActionResult reportdetail()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var date = WebRequest.GetString("date", true);

            var list = materialService.GetInventoryReportModel(Masterpage.CurrUser.client_code, date).ToList();
            data.list = list;
            LogHelper.Info(Masterpage.CurrUser.alias, "305012:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表明细带金额");
            return PartialView(data);
        }
        /// <summary>
        /// 耗材报表，无金额入口
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult filetable(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);

            var path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/inventory_n/";
            var list = materialService.GetPdfFileList(path);
            if (key != null && key != "") list = list.Where(p => p.name.Contains(key)).ToList();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 24;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            //data.ddl = ddl;
            data.key = key;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            if (key != null && key.ToString() != "")
                data.otherParam = "&key=" + key;
            else data.otherParam = "";
            LogHelper.Info(Masterpage.CurrUser.alias, "305011:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表无金额");
            return View(data);
        }
        /// <summary>
        /// 耗材报表明细，无金额
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "filetable", ForController = "material")]
        public ActionResult tableview(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var date = WebRequest.GetString("date", true);

            var list = materialService.GetInventoryReportModel(Masterpage.CurrUser.client_code, date);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 30;
            var vs = list.ToPagedList(_page, _pagesize);
            data.title = Masterpage.CurrUser.client_name + date.Substring(0, 4) + "年" + date.Substring(4, 2) + "月库存报表";
            data.list = vs;
            data.date = date;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&date=" + date;
            LogHelper.Info(Masterpage.CurrUser.alias, "305012:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表明细无金额");
            return View(data);
        }
        [AjaxAction(ForAction = "filetable", ForController = "material")]
        public ActionResult tabledetail()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var date = WebRequest.GetString("date", true);

            var list = materialService.GetInventoryReportModel(Masterpage.CurrUser.client_code, date).ToList();
            data.list = list;

            LogHelper.Info(Masterpage.CurrUser.alias, "305012:客户," + Masterpage.CurrUser.client_code + ",查看耗材报表明细无金额");
            return PartialView(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "report,filetable", ForController = "material")]
        public ActionResult downloadreport()
        {
            var time = WebRequest.GetString("time", true);

            string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/inventory_y/" + time + ".pdf";
            bool exit = FileHelper.IsFileExist(path);
            if (exit)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "305013:客户," + Masterpage.CurrUser.client_code + ",下载耗材报表" + path);
                FileHelper.DownLoadFile(path, Masterpage.CurrUser.client_name + time + "库存报表.pdf");
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "305013:客户," + Masterpage.CurrUser.client_code + ",下载耗材报表失败,文件不存在，" + path);
                return Content("文件不存在");
            }
            return View();
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "report,filetable", ForController = "material")]
        public ActionResult downloadtab()
        {
            var time = WebRequest.GetString("time", true);
            var path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/inventory_n/" + time + ".pdf";
            bool exit = FileHelper.IsFileExist(path);
            if (exit)
            {
                FileHelper.DownLoadFile(path, Masterpage.CurrUser.client_name + time + "库存报表.pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "305013:客户," + Masterpage.CurrUser.client_code + ",下载耗材报表" + path);
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "305013:客户," + Masterpage.CurrUser.client_code + ",下载耗材报表失败,文件不存在，" + path);
                return Content("文件不存在");
            }
            return View();
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "report,filetable", ForController = "material")]
        public ActionResult createpdf()
        {
            //var list = materialService.GetInventoryReportModel(Masterpage.CurrUser.client_code, DateTime.Now.ToString("yyyyMM"));
            //string title = Masterpage.CurrUser.client_name + DateTime.Now.ToString("yyyyMM") + "耗材报表";
            //PDFbyiTextSharp.Genera("~\\Content\\my_pdf\\" + title + ".pdf", title, list.ToList());
            return Content("ok");
        }

        [Anonymous]
        public ActionResult forpdfy(string para)
        {
            //dgklsz_苏州可乐_2013-08-01_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg_1,2,3,4,5,6,7,8,9,0,11,12,13,14,15,16,17#21,22,23,24,25,26,27,28,29,20,211,212,213,214,215,216,217
            //string code, string 公司名称, string 年月,string logo图片, string 报表明细
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {

                string[] t = para.Split('_');
                string host = Utils.GetHostHead();
                Customer one = managementService.GetCustomerInfo(t[0]);
                data.customer = one.CustomerFullName;
                data.ym = Convert.ToDateTime(t[1]).ToString("yyyy年MM月");
                data.logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + one.CustomerLogoUrl;
                #region 第15行开始明细
                //StringBuilder trs = new StringBuilder();
                //string[] ds = t[4].Split('@');
                //List<double> hj = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //for (int i = 0; i < ds.Length; i++)
                //{
                //    if (i > 14) break;
                //    //0序号	1耗材规格	2单位	3期初库存	4期初库存额	5期初结余量	6期初结余额	7期初总金额	8入库数	9入库金额	10出库数	
                //    //11出库金额	12期末库存	13期末库存额	14期末结余量	15期初结余额	16期末总金额	17备注
                //    var one = ds[i].Split(',');
                //    trs.Append("<tr>");
                //    for (int j = 0; j < one.Length; j++)
                //    {
                //        if (j > 17) break;
                //        trs.Append("<td class=\"endwise_title\">" + one[j] + "</td>");
                //        if (j > 2 && j < 17)
                //        {
                //            try
                //            {
                //                hj[j] += Convert.ToDouble(one[j]);
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //    trs.Append("</tr>");
                //}
                //#region 合计
                //StringBuilder trhj = new StringBuilder();
                //trhj.Append("<tr><td bgcolor=\"#E3EEF4\" class=\"endwise_title\">小计</td>");
                //for (int k = 1; k < 17; k++)
                //{
                //    if (k > 2) trhj.Append("<td bgcolor=\"#E3EEF4\" class=\"endwise_title\">" + hj[k] + "</td>");
                //    else trhj.Append("<td bgcolor=\"#E3EEF4\" class=\"endwise_title\">&nbsp;</td>");
                //}
                //trhj.Append("</tr>");
                //#endregion
                #endregion
                //data.rows = ds.Length;
                //data.trs = trs.ToString();
                //data.trhj = trhj.ToString();
                var list = materialService.GetInventoryReportModel(t[0], Convert.ToDateTime(t[1]).ToString("yyyyMM")).ToList();
                data.list = list;
            }
            catch(Exception ex)
            {
                data.customer = ex.Message; data.ym = ""; data.logo = ""; data.rows = 0; data.trs = ""; data.trhj = "";
                data.list = new List<InventoryReportModel>();
            }

            return View(data);
        }
        
        [Anonymous]
        public ActionResult forpdfn(string para)
        {
            //dgklsz_苏州可乐_2013-08-01_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg_1,2,3,4,6,9,11,13,15,17#21,22,23,24,26,29,211,213,214,217
            //string code, string 公司名称, string 年月,string logo图片, string 报表明细
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                string[] t = para.Split('_'); 
                Customer one = managementService.GetCustomerInfo(t[0]);
                string host = Utils.GetHostHead();
                data.customer = one.CustomerFullName;
                data.ym = Convert.ToDateTime(t[1]).ToString("yyyy年MM月");
                data.logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + one.CustomerLogoUrl;
                #region 第15行开始明细
                //StringBuilder trs = new StringBuilder();
                //string[] ds = t[4].Split('@');
                //List<double> hj = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                //for (int i = 0; i < ds.Length; i++)
                //{
                //    if (i > 14) break;
                //    //0序号	1耗材规格	2单位	3期初库存		5期初结余量		7期初总金额	8入库数	9入库金额	10出库数	
                //    //11出库金额	12期末库存	13期末库存额	14期末结余量	15期初结余额	16期末总金额	17备注
                //    var one = ds[i].Split(',');
                //    trs.Append("<tr>");
                //    for (int j = 0; j < one.Length; j++)
                //    {
                //        if (j > 9) break;
                //        trs.Append("<td class=\"endwise_title\">" + one[j] + "</td>");
                //        if (j > 2 && j < 9)
                //        {
                //            try
                //            {
                //                hj[j] += Convert.ToDouble(one[j]);
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //    trs.Append("</tr>");
                //}
                //#region 合计
                //StringBuilder trhj = new StringBuilder();
                //trhj.Append("<tr><td bgcolor=\"#E3EEF4\" class=\"endwise_title\">小计</td>");
                //for (int k = 1; k < 9; k++)
                //{
                //    if (k > 2) trhj.Append("<td bgcolor=\"#E3EEF4\" class=\"endwise_title\">" + hj[k] + "</td>");
                //    else trhj.Append("<td bgcolor=\"#E3EEF4\" class=\"endwise_title\">&nbsp;</td>");
                //}
                //trhj.Append("</tr>");
                //#endregion
                #endregion
                //data.rows = ds.Length;
                //data.trs = trs.ToString();
                //data.trhj = trhj.ToString(); 
                var list = materialService.GetInventoryReportModel(t[0], Convert.ToDateTime(t[1]).ToString("yyyyMM")).ToList();
                data.list = list;
            }
            catch
            {
                data.customer = ""; data.ym = ""; data.logo = ""; data.rows = 0; data.trs = ""; data.trhj = "";
            }

            return View(data);
        }
        #endregion
    }
}
