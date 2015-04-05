using ecoBio.Wms.Common;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.ViewModel;
using ecoBio.Wms.Data.Entities.Models;

namespace ecoBio.Wms.Web.Controllers
{
    public class costanalysisController : BaseController
    {
        private CostanalysisService costanalysisService;
        private MaterialService materialService;
        public costanalysisController(IMaterialRepository _materialRepository, ICostanalysisRepository _costanalysisRepository)
        {
            materialService = new MaterialService(_materialRepository);
            costanalysisService = new CostanalysisService(_costanalysisRepository);
        }

        #region 采集管理

        public ActionResult purchase(int? page, int? pagesize, string q)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = costanalysisService.GetChemicalOrderlist(Masterpage.CurrUser.client_code, q);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            LogHelper.Info(Masterpage.CurrUser.alias, "401011:客户" + Masterpage.CurrUser.client_code + ",打开化学品价格管理页面第" + _page + "页");
            data.q = q;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return View(data);
        }
        #endregion

        #region 价格管理



        [HidNowLocal]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult pricechemical(int? page, int? pagesize, string q)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = costanalysisService.GetChemicalOrderlist(Masterpage.CurrUser.client_code, q);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            LogHelper.Info(Masterpage.CurrUser.alias, "401011:客户" + Masterpage.CurrUser.client_code + ",打开化学品价格管理页面第" + _page + "页");
            data.q = q;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView("pricechemical", data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult chemicalview(string order)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            List<ChemicalOrderModel> list = new List<ChemicalOrderModel>();
            ChemicalPriceManagement one;
            if (order == null || order == "")
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "401012:客户" + Masterpage.CurrUser.client_code + ",查看订单" + order + "的订单明细失败：参数错误");
            }
            else
            {

                one = costanalysisService.GetOneChemicalOrder(Masterpage.CurrUser.client_code, order);
                if (one == null)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "401012:客户" + Masterpage.CurrUser.client_code + ",查看订单" + order + "的订单明细失败：订单不存在");
                }
                else
                {
                    list = costanalysisService.GetChemicalOrderDetaillist(Masterpage.CurrUser.client_code, order).ToList();
                    LogHelper.Info(Masterpage.CurrUser.alias, "401012:客户" + Masterpage.CurrUser.client_code + ",查看订单" + order + "的订单明细");
                }
            }
            data.list = list;
            data.order = order.Replace(Masterpage.CurrUser.client_code + "-", "");

            return PartialView("chemicalview",data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult orderdetail(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var order = WebRequest.GetString("order", true);
            var date = WebRequest.GetString("date", true);
            var person = WebRequest.GetString("person", true);
            var type = WebRequest.GetString("type", true);
            //var list = new List<ChemicalOrderModel>();
            var one = new ChemicalPriceManagement();

            var material = costanalysisService.GetMaterialSpecification(Masterpage.CurrUser.client_code).ToList();

            var newin_no = "";
            var message = "";
            if (type == "add")
            {
                newin_no = costanalysisService.CreateOrderNumber(Masterpage.CurrUser.client_code);
                newin_no = newin_no.Substring(0, newin_no.Length - 4);
                one = null;
                LogHelper.Info(Masterpage.CurrUser.alias, "401013:客户" + Masterpage.CurrUser.client_code + ",新增订单，订单号为：" + newin_no + ",等待提交");
            }
            else if (type == "edit")
            {
                one = costanalysisService.GetOneChemicalOrder(Masterpage.CurrUser.client_code, order);
                List<ChemicalOrderModel> list = costanalysisService.GetChemicalOrderDetaillist(Masterpage.CurrUser.client_code, order).ToList();
                SessionHelper.SetSession(order, list);
                LogHelper.Info(Masterpage.CurrUser.alias, "401013:客户" + Masterpage.CurrUser.client_code + ",编辑订单内存中信息，订单号为：" + order + ",订单明细为：" + JsonHelper.ToJson(list));
            }
            else if (type == "createinno")
            {
                #region 创建入库单

                var in_sn = WebRequest.GetString("in_sn", true);
                if (in_sn.Length > 20)
                {
                    message = "订单号长度错误";
                    type = "add";
                }
                else
                {
                    string backno = costanalysisService.AddChemicalOrder(Masterpage.CurrUser.client_code, date, person);
                    if (backno == "")
                    {
                        message = "订单创建失败";
                        LogHelper.Info(Masterpage.CurrUser.alias, "401013:客户" + Masterpage.CurrUser.client_code + ",创建订单失败");
                        type = "add";
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "401013:客户" + Masterpage.CurrUser.client_code + ",创建一个新的订单，订单号为：" + backno);
                        return RedirectToAction("orderdetail", new { type = "edit", order = backno });
                    }
                }

                #endregion
            }
            else
            {
                return RedirectToAction("orderdetail");
            }
            data.supplier = costanalysisService.GetSupplierDDL("耗材");
            data.message = message;
            data.type = type;

            data.material = material;
            data.one = one;
            data.newin_no = newin_no;
            //int _page = page.HasValue ? page.Value : 1;
            //int _pagesize = 14;
            //var vs = list.ToPagedList(_page, _pagesize);
            //data.list = vs;
            //data.pageSize = _pagesize;
            //data.pageIndex = _page;
            //data.totalCount = vs.TotalCount;
            //data.otherParam = "&type=edit&order=" + order;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult detaillist(string order)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401014:客户" + Masterpage.CurrUser.client_code + ",查看订单明细，订单号为：" + order);

            dynamic data = new System.Dynamic.ExpandoObject();
            var list = new List<ChemicalOrderModel>();
            list = (List<ChemicalOrderModel>)SessionHelper.GetSession(order);
            data.list = list;
            // var list = costanalysisService.GetChemicalOrderDetaillist(Masterpage.CurrUser.client_code, order).ToList();
            // data.list = list;
            return PartialView("detaillist", data);
        }



        [HidNowLocal]
        [HttpGet]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult filterorder()
        {
            string v = WebRequest.GetQueryString("v", true);
            string key = WebRequest.GetQueryString("key", true);
            List<object> r = new List<object>();
            var list = costanalysisService.GetChemicalPriceManagement(Masterpage.CurrUser.client_code, v, key).Select(x => new { text = x.chemical_po_no.Replace(Masterpage.CurrUser.client_code + "-", ""), value = x.chemical_po_no.Replace(Masterpage.CurrUser.client_code + "-", "") });
            LogHelper.Info(Masterpage.CurrUser.alias, "401016:客户" + Masterpage.CurrUser.client_code + ",过滤查询订单，类别为:" + v + "，关键字为:" + key);
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        //", { id: id, m: m, count: count, order: order, price: price }
        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult savechemical()
        {
            ReturnValue ret = new ReturnValue();
            string order = WebRequest.GetString("order", true);
            var list = new List<ChemicalOrderModel>();
            list = (List<ChemicalOrderModel>)SessionHelper.GetSession(order);
            if (list == null || list.Count < 1)
            {
                ret = new ReturnValue { status = "error", message = "无订单明细" };
                LogHelper.Info(Masterpage.CurrUser.alias, "401017:客户," + Masterpage.CurrUser.client_code + ",保存订单" + order + "的修改失败，无订单明细");
            }
            else
            {
                ret = costanalysisService.SaveChemicalOrderDetail(Masterpage.CurrUser.client_code, order, list);
                LogHelper.Info(Masterpage.CurrUser.alias, "401017:客户," + Masterpage.CurrUser.client_code + ",保存订单" + order + "的修改成功，订单明细为" + JsonHelper.ToJson(list));
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult savechemicaltemp()
        {
            string id = WebRequest.GetString("id", true);
            string m = WebRequest.GetString("m", true);
            string s = WebRequest.GetString("s", true);
            string count = WebRequest.GetString("count", true);
            string order = WebRequest.GetString("order", true);
            string price = WebRequest.GetString("price", true);
            Guid supplier = Guid.Parse(s);
            ReturnValue ret = new ReturnValue();

            //if (id == null || id == "") ret = costanalysisService.AddChemicalOrderDetail(Masterpage.CurrUser.client_code, order, m, count, price, supplier);
            //else ret = costanalysisService.UpdateChemicalOrderDetail(Masterpage.CurrUser.client_code, order, id, m, count, price, supplier);

            try
            {
                var sup = costanalysisService.GetOneSupplier(supplier);
                var material = costanalysisService.GetOneMaterialSpecification(Masterpage.CurrUser.client_code, m);
                #region 保存临时
                var list = new List<ChemicalOrderModel>();
                list = (List<ChemicalOrderModel>)SessionHelper.GetSession(order);
                if (id != null && id != "")
                {
                    #region 修改
                    var had = list.FirstOrDefault(p => p.ChemicalPoItemsNo == id);
                    had.ChemicalPoAmount = Convert.ToDouble(count);
                    had.ChemicalPoPrice = Convert.ToDouble(price);
                    had.MaterialSpecificationCode = m;
                    had.SupplierGuid = supplier;
                    if (sup != null) had.SupplierName = sup.SupplierFullName;
                    had.material_specification_displayname = material.MaterialSpecificationDisplayName;
                    had.material_specification_model = material.MaterialSpecificationModel;
                    had.material_specification_name = material.MaterialSpecificationName;
                    #endregion
                }
                else
                {
                    #region 添加
                    ChemicalOrderModel had = new ChemicalOrderModel();
                    had.ChemicalPoNo = order;
                    had.ChemicalPoItemsNo = "";
                    had.ChemicalPoAmount = Convert.ToDouble(count);
                    had.ChemicalPoInputAmount = 0;
                    had.ChemicalPoPrice = Convert.ToDouble(price);
                    had.MaterialSpecificationCode = m;
                    had.ChemicalPoPriceUnit = material.MaterialSpecificationUnit;
                    had.SupplierGuid = supplier;
                    if (sup != null) had.SupplierName = sup.SupplierFullName;
                    had.material_specification_displayname = material.MaterialSpecificationDisplayName;
                    had.material_specification_model = material.MaterialSpecificationModel;
                    had.material_specification_name = material.MaterialSpecificationName;
                    list.Add(had);
                    #endregion
                }
                SessionHelper.SetSession(order, list);
                #endregion
                ret = new ReturnValue { status = "ok", message = "" };
            }
            catch
            {
                ret = new ReturnValue { status = "error", message = "提交失败" };
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult delchemicaltemp()
        {
            string order = WebRequest.GetString("order", true);
            string id = WebRequest.GetString("id", true);
            ReturnValue ret = new ReturnValue();
            try
            {
                #region 保存临时
                var list = new List<ChemicalOrderModel>();
                list = (List<ChemicalOrderModel>)SessionHelper.GetSession(order);
                if (id != null && id != "")
                {
                    #region 修改
                    var had = list.FirstOrDefault(p => p.ChemicalPoItemsNo == id);
                    had.ChemicalPoAmount = 0;
                    #endregion
                    SessionHelper.SetSession(order, list);
                }
                else
                {
                    ret = new ReturnValue { status = "error", message = "缺少参数" };
                }
                #endregion
                ret = new ReturnValue { status = "ok", message = "" };
            }
            catch
            {
                ret = new ReturnValue { status = "error", message = "提交失败" };
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult price()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401010:客户" + Masterpage.CurrUser.client_code + ",打开价格管理页面");

            // var cate1 = costanalysisService.GetEnergyCategory("energy_category");
            //var cate2 = costanalysisService.GetEnergyCategory("MATERIAL_CATEGORY");
            // var cate3 = costanalysisService.GetEnergyCategory("material_specification_categeory_other");
            //var elist=costanalysisService.GetPriceManagement(Masterpage.CurrUser.client_code, "E");
            dynamic data = new System.Dynamic.ExpandoObject();
            //data.cate1 = cate1; data.cate2 = cate2; data.cate3 = cate3;
            //var elist = costanalysisService.GetPriceManagementDictinrt(Masterpage.CurrUser.client_code, "E");
            //var slist = costanalysisService.GetPriceManagementDictinrt(Masterpage.CurrUser.client_code, "S");

            var elist = costanalysisService.GetEnergyCategory("PRICE_ENERGY_CATEGORY");
            var clist = costanalysisService.GetEnergyCategory("PRICE_BYPRODUCT_CATEGORY");
            var slist = costanalysisService.GetEnergyCategory("PRICE_WASTE_CATEGORY");
            data.elist = elist;
            data.clist = clist;
            data.slist = slist;
            data.customername = Masterpage.CurrUser.client_name;

            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "price", ForController = "costanalysis")]
        public ActionResult priceecs(int? page, int? pagesize, string v, string vname, string vunit)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401015:客户" + Masterpage.CurrUser.client_code + ",查看能资源" + vname + "的价格列表");

            dynamic data = new System.Dynamic.ExpandoObject();
            var list = costanalysisService.GetPriceManagementByName(Masterpage.CurrUser.client_code, vname);
            var units = costanalysisService.GetEnergyCategory("PRICE_ENERGY_UNIT").Select(x => new SelectListItem
            {
                Text = x.key,
                Value = x.value
            }).ToList();
            List<EnergyPriceModel> show = new List<EnergyPriceModel>();
            #region 电合并
            if (vname == "电")
            {
                List<DateTime> starts = new List<DateTime>();
                foreach (var item in list)
                {
                    if (starts.Contains(item.EnergyPriceBeginningDate.Value)) continue;
                    var t = list.Where(p => p.EnergyPriceBeginningDate == item.EnergyPriceBeginningDate);
                    EnergyPriceModel o = new EnergyPriceModel();
                    o.start = item.EnergyPriceBeginningDate;
                    o.end = item.EnergyPriceEndingDate;
                    o.price = "";                    
                    foreach (var ot in t)
                    {
                        //o.price += ot.EnergyResourceName.Replace(vname, "") + ":" + ot.EnergyPrice.ToString("N")  + ";   ";//+ ot.EnergyPriceUnit
                        if (ot.EnergyResourceName.Contains("峰"))  o.pricef = ot.EnergyPrice.ToString("N");
                        if (ot.EnergyResourceName.Contains("谷")) o.priceg = ot.EnergyPrice.ToString("N"); 
                        if (ot.EnergyResourceName.Contains("平")) o.pricep = ot.EnergyPrice.ToString("N");
                    }
                    show.Add(o);
                    starts.Add(item.EnergyPriceBeginningDate.Value);
                }
            }
            else
            {
                foreach (var ot in list)
                {
                    EnergyPriceModel o = new EnergyPriceModel();
                    o.start = ot.EnergyPriceBeginningDate;
                    o.end = ot.EnergyPriceEndingDate;
                    o.price = ot.EnergyPrice.ToString("N");// + ot.EnergyPriceUnit
                    show.Add(o);
                }
            }
            #endregion
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = show.ToPagedList(_page, _pagesize);
            data.vname = vname;
            data.vunit = vunit;
            data.v = v;
            data.units = units;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView("priceecs", data);
        }


        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "price", ForController = "costanalysis")]
        public ActionResult saveecs()
        {
            string date = WebRequest.GetString("date", true);
            string mu = WebRequest.GetString("mu", true);
            string v = WebRequest.GetString("v", true);
            string vname = WebRequest.GetString("vname", true);
            string price = WebRequest.GetString("price", true);

            ReturnValue ret = ret = costanalysisService.AddPriceManagement(Masterpage.CurrUser.client_code, vname, v, mu, date, price);
            LogHelper.Info(Masterpage.CurrUser.alias, "401018:客户," + Masterpage.CurrUser.client_code + ",保存能资源" + vname + "的价格，价格：" + price + "，启用日期：" + date);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult deletechemical()
        {
            string id = WebRequest.GetString("id", true);
            ReturnValue ret = new ReturnValue();
            if (id == null || id == "") ret = new ReturnValue { status = "error", message = "参数错误" };
            // else ret = costanalysisService.DeleteChemicalPriceManagement(Masterpage.CurrUser.client_code, id);
            else ret = costanalysisService.DelChemicalOrderDetail(Masterpage.CurrUser.client_code, id);
            LogHelper.Info(Masterpage.CurrUser.alias, "401019:客户," + Masterpage.CurrUser.client_code + ",删除订单" + id + "，结果" + ret.status);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [HttpPost]
        [AjaxAction(ForAction = "purchase", ForController = "costanalysis")]
        public ActionResult deleteorder()
        {
            string id = WebRequest.GetString("id", true);
            ReturnValue ret = new ReturnValue();
            if (id == null || id == "") ret = new ReturnValue { status = "error", message = "参数错误" };
            else ret = costanalysisService.DelChemicalOrder(Masterpage.CurrUser.client_code, id);

            LogHelper.Info(Masterpage.CurrUser.alias, "401019:客户," + Masterpage.CurrUser.client_code + ",删除订单" + id + "，结果" + ret.status);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 成本结构
        public ActionResult structure()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401021:客户," + Masterpage.CurrUser.client_code + ",查看成本结构图表");

            dynamic data = new System.Dynamic.ExpandoObject();
            var chart = costanalysisService.GetStructureChart("CC07");
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.customername = Masterpage.CurrUser.client_name;
            chart.url = Utils.GetFlexAddress();
            chart.title = "能资源成本结构";
            var grid = costanalysisService.GetStructureStatistic(Masterpage.CurrUser.client_code);
            data.chart = JsonHelper.ToJson(chart);
            data.grid = grid;
            return View(data);
        }

        #endregion

        #region 耗材趋势
        /// <summary>
        /// 耗材趋势
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult trend()
        {

            LogHelper.Info(Masterpage.CurrUser.alias, "401031:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表");
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
        [AjaxAction(ForAction = "trend,nutrients", ForController = "costanalysis")]
        public ActionResult CostTrend(int? page, int? pagesize, string key, string select, string t)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401031:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表，选择耗材为：" + select);
            dynamic data = new System.Dynamic.ExpandoObject();
            var material = costanalysisService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct();
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
            return PartialView("CostTrend", data);
        }

        /// <summary>
        /// 过滤关键字
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "trend,nutrients", ForController = "costanalysis")]
        [HttpGet]
        public ActionResult filterparam()
        {
            string t = WebRequest.GetQueryString("t", true);
            string key = WebRequest.GetQueryString("key", true);
            List<object> r = new List<object>();
            var material = costanalysisService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct();
            var list = material.Where(p => p.MaterialSpecificationName.Contains(key) || p.MaterialSpecificationName.Contains(key.ToLower()) || p.MaterialSpecificationName.Contains(key.ToUpper()));
            if (t != "") list = list.Where(p => p.MaterialSpecificationCategeory == t);
            var result = list.Select(x => new { text = x.MaterialSpecificationName, value = x.MaterialSpecificationName }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新图表和统计数据
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        [HidNowLocal]
        [AjaxAction(ForAction = "trend,nutrients", ForController = "costanalysis")]
        [HttpGet]
        public ActionResult upchartandgrid(string select,string chartnumber)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401031:客户," + Masterpage.CurrUser.client_code + ",查看耗材趋势图表，选择耗材为：" + select);
            #region 统计数据
            var statistics = costanalysisService.GetStockStatistic(Masterpage.CurrUser.client_code, select, DateTime.Now.Year.ToString());
            #endregion

            #region 图表
            var chart = costanalysisService.GetFlexChart(chartnumber);
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.processparms = select + "|" + DateTime.Now.Year.ToString();
            chart.title = "成本趋势"; 
            chart.url = Utils.GetFlexAddress();
            #endregion
            var result = new { grid = statistics, chart = chart };
            return Json(result, JsonRequestBehavior.AllowGet);
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
            var chart = costanalysisService.GetFlexChart(n);
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
            LogHelper.Info(Masterpage.CurrUser.alias, "401032:客户," + Masterpage.CurrUser.client_code + ",查看全屏图表，耗材为：" + m);
            return View("chartfullscreen", data);
            
        } 
        [LoginAllow]
        public ActionResult costfullscreen()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string title = WebRequest.GetString("t", true);

            string w = WebRequest.GetString("w", true);
            string h = WebRequest.GetString("h", true);
            string n = WebRequest.GetString("n", true);
            string acl = WebRequest.GetString("acl", true);
            string acr = WebRequest.GetString("acr", true);
            string line = WebRequest.GetString("line", true);
            #region 图表
            var chart = costanalysisService.GetFlexChart(n);
            chart.charttype = line;
            chart.title =  title;
            chart.height = int.Parse(h);
            chart.width = int.Parse(w);
            chart.leftprecision = int.Parse(acl);
            chart.rightprecision = int.Parse(acr);
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.url = Utils.GetFlexAddress();
            #endregion
            data.chart = JsonHelper.ToJson(chart);
            LogHelper.Info(Masterpage.CurrUser.alias, "401032:客户," + Masterpage.CurrUser.client_code + ",查看能资源成本结构全屏图表"  );
            return View("chartfullscreen", data);            
        }


        /// <summary>
        /// 生物营养剂趋势
        /// </summary>
        /// <param name="txtkey"></param>
        /// <returns></returns>
        public ActionResult nutrients()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401033:客户," + Masterpage.CurrUser.client_code + ",查看营养剂耗材趋势图表");
            return View();
        }
        #endregion

        #region 沼气效益
        public ActionResult biogas()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "401041:客户," + Masterpage.CurrUser.client_code + ",沼气效益");

            dynamic data = new System.Dynamic.ExpandoObject();
            var chart = costanalysisService.GetFlexChart("CC10");
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.customername = Masterpage.CurrUser.client_name;
            chart.lowerlimit = 0;
            chart.lowlimit = 0;
            chart.title = "沼气效益";
            chart.url = Utils.GetFlexAddress();
            chart.queryparms = "";
            var grid = costanalysisService.GetBiogasStatistic(Masterpage.CurrUser.client_code, "");
            data.chart = JsonHelper.ToJson(chart);
            data.grid = grid;
            data.code = Masterpage.CurrUser.client_code;
            data.name = Masterpage.CurrUser.client_name;
            return View(data);
        }
        public ActionResult biogasfullscreen()
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
            var chart = costanalysisService.GetFlexChart("CC10");
            chart.charttype = line;
            chart.title = (one != null ? one.MaterialSpecificationName : "") + title;
            chart.height = int.Parse(h);
            chart.width = int.Parse(w);
            chart.leftprecision = int.Parse(acl);
            chart.rightprecision = int.Parse(acr);
            chart.customercode = Masterpage.CurrUser.client_code;            
            chart.url = Utils.GetFlexAddress();
            #endregion
            data.chart = JsonHelper.ToJson(chart);
            LogHelper.Info(Masterpage.CurrUser.alias, "401032:客户," + Masterpage.CurrUser.client_code + ",查看全屏图表，耗材为：" + m);
            return View("chartfullscreen", data);
        }
        #endregion

    }
}
