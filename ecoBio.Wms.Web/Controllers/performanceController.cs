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
    /// <summary>
    /// 系统绩效
    /// </summary>
    public class performanceController : BaseController
    {
         private DataCenterService centerService;
         public performanceController(IDataCenterRepository _centerRepository)
        {
            centerService = new DataCenterService(_centerRepository);
        }

        /// <summary>
        /// 废水回用率
        /// </summary>
        /// <returns></returns>
         public ActionResult wastewater()
         {
             //dynamic data = new System.Dynamic.ExpandoObject();
             //var code = centerService.GetCustomerCollectionByCollectionCode(Masterpage.CurrUser.client_code, "I514").ToList();
             //string flexpara = "";
             //if (code != null && code.Count > 0)
             //{
             //    flexpara = code[0].CustomerCollectionCode;

             //    List<string> selcodes = new List<string>();
             //    string bgcq = flexpara.Replace("I514", "I513");
             //    selcodes.Add(flexpara);
             //    selcodes.Add(bgcq);
             //    var statistics = centerService.GetParamsQueryStatistic(selcodes);
             //    #region 图表
             //    var point = centerService.GetOneCustomerCollection(selcodes[0]);
             //    var chart = centerService.GetParamsQueryFlexChart(point.StandardChartCode);
             //    chart.charttype = chart.standardtype;
             //    chart.customercode = Masterpage.CurrUser.client_code;
             //    chart.customername = Masterpage.CurrUser.client_name;
             //    chart.lowerlimit = point.CustomerCollectionLowLimit.HasValue ? point.CustomerCollectionLowLimit.Value : 0;
             //    chart.uperlimit = point.CustomerCollectionUpLimit.HasValue ? point.CustomerCollectionUpLimit.Value : 0;
             //    chart.processparms = flexpara + "," + bgcq;
             //    chart.queryparms = "I514";
             //    chart.title = "废水回用率";
             //    #endregion

             //    if (statistics != null && statistics.Count > 0) data.grid = statistics;
             //    else data.grid = new ViewModel.Grid_Statistic();
             //    data.chart = JsonHelper.ToJson(chart);
             //    data.hasdata = 1;
             //}
             //else
             //{
             //    #region 不存在采集点
             //    data.hasdata = 0;
             //    data.grid = new ViewModel.Grid_Statistic();
             //    var chart = new FlexChart();
             //    chart.customercode = Masterpage.CurrUser.client_code;
             //    chart.customername = Masterpage.CurrUser.client_name;
             //    chart.title = "废水回用率";
             //    data.chart = JsonHelper.ToJson(chart);
             //    #endregion
             //}
             //return View(data); 
             LogHelper.Info(Masterpage.CurrUser.alias, "205011:客户," + Masterpage.CurrUser.client_code + ",查看废水回用率30日趋势图表");
             return View();
         }
        /// <summary>
        /// 泥饼产率
        /// </summary>
        /// <returns></returns>
         public ActionResult cakeproduce()
         {
             LogHelper.Info(Masterpage.CurrUser.alias, "205021:客户," + Masterpage.CurrUser.client_code + ",查看泥饼产率30日趋势图表");
             //dynamic data = new System.Dynamic.ExpandoObject();

             //var code = centerService.GetCustomerCollectionByCollectionCode(Masterpage.CurrUser.client_code, "I332").ToList();
             //string flexpara = "";
             //if (code != null && code.Count > 0)
             //{
             //    flexpara = code[0].CustomerCollectionCode;

             //    List<string> selcodes = new List<string>();
             //    string bgcq = flexpara.Replace("I332", "I331");
             //    selcodes.Add(flexpara);
             //    selcodes.Add(bgcq);

             //    var statistics = centerService.GetParamsQueryStatistic(selcodes);
             //    #region 图表
             //    var point = centerService.GetOneCustomerCollection(selcodes[0]);
             //    var chart = centerService.GetParamsQueryFlexChart(point.StandardChartCode);
             //    chart.charttype = chart.standardtype;
             //    chart.customercode = Masterpage.CurrUser.client_code;
             //    chart.customername = Masterpage.CurrUser.client_name;
             //    chart.lowerlimit = point.CustomerCollectionLowLimit.HasValue ? point.CustomerCollectionLowLimit.Value : 0;
             //    chart.uperlimit = point.CustomerCollectionUpLimit.HasValue ? point.CustomerCollectionUpLimit.Value : 0;
             //    chart.processparms = flexpara + "," + bgcq;
             //    chart.queryparms = "I332";
             //    chart.title = "泥饼产率";
             //    #endregion

             //    if (statistics != null && statistics.Count > 0) data.grid = statistics;
             //    else data.grid = new ViewModel.Grid_Statistic();
             //    data.chart = JsonHelper.ToJson(chart);
             //    data.hasdata = 1;
             //}
             //else
             //{
             //    #region 不存在采集点
             //    data.hasdata = 0;
             //    data.grid = new ViewModel.Grid_Statistic();
             //    var chart = new FlexChart();
             //    chart.customercode = Masterpage.CurrUser.client_code;
             //    chart.customername = Masterpage.CurrUser.client_name;
             //    chart.title = "泥饼产率";
             //    data.chart = JsonHelper.ToJson(chart);
             //    #endregion
             //}
             //return View(data);
             return View();
         }


        /// <summary>
        /// 污染物消减
        /// </summary>
        /// <returns></returns>
        public ActionResult pollutants()
         {
             LogHelper.Info(Masterpage.CurrUser.alias, "205031:客户," + Masterpage.CurrUser.client_code + ",查看污染物消减30日趋势图表");
            //dynamic data = new System.Dynamic.ExpandoObject();
            //var list = centerService.GetParamsQueryList(Masterpage.CurrUser.client_code, "", 18).OrderBy(p => p.StandardProcessUnitCode).ToList();
            //List<SelectListItem> models = new List<SelectListItem>();
            //if (list != null && list.Count > 0)
            //{
            //    foreach (var item in list)
            //    {
            //        string pointname = item.collent_point_name;
            //        if (item.collent_point_name.EndsWith("(RD)"))
            //        {
            //            string front = item.collent_point_name.Replace("(RD)", "");
            //            string rvname = item.collent_point_name.Replace("(RD)", "(RV)");
            //            var rv = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_name == rvname);
            //            if (rv != null)
            //            {
            //                models.Add(new SelectListItem { Text = front + "(" + item.StandardProcessUnitCode.Replace("0000","系统") + ")", Value = item.CustomerCollectionCode + "," + rv.CustomerCollectionCode });
            //            }
            //        }
            //    }
            //    data.hasdata = 1;
            //}
            //else
            //{
            //    data.hasdata = 0;
            //}
            //data.ddl = models;
            //return View(data);

            return View();
        }
      
        /// <summary>
        /// 系统稳定性
        /// </summary>
        /// <returns></returns>
        public ActionResult stability()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "205041:客户," + Masterpage.CurrUser.client_code + ",查看系统稳定性30日趋势图表");
            return View();
        }
     

        /// <summary>
        /// 厌氧沼气产率
        /// </summary>
        /// <returns></returns>
        public ActionResult biogasproduce()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "205051:客户," + Masterpage.CurrUser.client_code + ",查看厌氧沼气产率30日趋势图表");
            //dynamic data = new System.Dynamic.ExpandoObject();
            //var code1 = centerService.GetCustomerCollectionByCollectionCode(Masterpage.CurrUser.client_code, "I333").ToList();
            //var code2 = centerService.GetCustomerCollectionByCollectionCode(Masterpage.CurrUser.client_code, "I334").ToList();
            //List<SelectListItem> models = new List<SelectListItem>();
            //if (code1 != null && code1.Count > 0)
            //{
            //    foreach (var item in code1)
            //    {
            //        var rv = code2.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode);
            //        if (rv != null)
            //        {
            //            models.Add(new SelectListItem { Text = item.StandardProcessUnitCode.Replace("0000", "系统"), Value = item.CustomerCollectionCode + "," + rv.CustomerCollectionCode });
            //        }

            //    }
            //    List<string> selcodes = new List<string>();
            //    string bgpr = models[0].Value;
            //    string bgcq = bgpr.Replace("I334", "I333");
            //    selcodes.Add(bgcq);
            //    selcodes.Add(bgpr);

            //    data.ddl = models;
            //    data.hasdata = 1;
            //}
            //else
            //{
            //    #region 不存在采集点
            //    data.hasdata = 0;
            //    #endregion
            //}
            //    data.ddl = models;
            //return View(data);

            return View();

        }
      


        /// <summary>
        /// 过滤关键字
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxAction(ForAction = "pollutants,stability,biogasproduce", ForController = "performance")]
        public ActionResult queryparam()
        {
            string type = WebRequest.GetQueryString("type", true);
            string key = WebRequest.GetQueryString("key", true);
            List<object> r = new List<object>();

            for (int i = 0; i < 12; i++)
            {
                string text = type + "_" + key + i.ToString();
                r.Add(new { value = i, text = text });
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 更新图表和统计数据
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        [HidNowLocal]
        [LoginAllow]
        [HttpGet]
        public ActionResult upchartandgrid(string select, string title)
        {
            #region 统计数据
            List<string> selcodes = new List<string>();
            string[] t = select.Split(',');
            foreach (var item in t)
            {
                if (item != "") selcodes.Add(item);
            }
            #region 图表
            var point = centerService.GetOneCustomerCollection(selcodes[0]);
            var chart = centerService.GetParamsQueryFlexChart(point.StandardChartCode);
            chart.charttype = chart.standardtype;
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.customername = Masterpage.CurrUser.client_name;
            //chart.lowerlimit = point.CustomerCollectionLowLimit.HasValue ? point.CustomerCollectionLowLimit.Value : 0;
            //chart.uperlimit = point.CustomerCollectionUpLimit.HasValue ? point.CustomerCollectionUpLimit.Value : 0;
            chart.processparms = select;
            chart.title = "沼气产率";
            chart.queryparms = "";
            #endregion


            var statistics = centerService.GetParamsQueryStatistic(selcodes);
            #endregion
            var result = new { grid = statistics, chart = chart};

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
