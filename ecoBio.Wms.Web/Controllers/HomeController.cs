using ecoBio.Wms.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ecoBio.Wms.Repositories;
using ecoBio.Wms.ViewModel;
using Trirand.Web.Mvc;
using ecoBio.Wms.Service.Monitor;
using ecoBio.Wms.Common;
using ecoBio.Wms.FlexData;

namespace ecoBio.Wms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private ManagementService managementService;
        private DataCenterService centerService;
        private AccountService accountService;
        private MaterialService materialService;
        public HomeController(IDataCenterRepository _centerRepository, IMaterialRepository _materialRepository, IAccountRepository _rightRepository, IManagementRepository _managementRepository)
        {
            centerService = new DataCenterService(_centerRepository);
            accountService = new AccountService(_rightRepository);
            materialService = new MaterialService(_materialRepository);
            managementService = new ManagementService(_managementRepository);
        }

        [LoginAllow]
        public ActionResult UserCenter()
        {
            return RedirectToAction("", "");
        }
        [LoginAllow]
        public ActionResult Index()
        {
            bool plat = true;
            if (Masterpage.CurrUser.IsEmployee)
            {
                plat = accountService.CheckHasModuleFunction(Masterpage.CurrUser.role_guid, Masterpage.CurrUser.guid, "home", "platform");
            }
            if (plat) return RedirectToAction("platform");
            else
            {
                var list =new List<ModuleFunction>();
                if (!Masterpage.CurrUser.IsEmployee)
                {
                    list = accountService.GetModuleFunctionByRole(Masterpage.CurrUser.role_guid);
                }
                else
                {
                    list = accountService.GetEmployeeModuleFunctionByRole(Masterpage.CurrUser.role_guid, Masterpage.CurrUser.guid);
                }

                var f = list.FirstOrDefault(p => p.ModuleFunctionRoute != null && p.ModuleFunctionRoute != "");
                if (f == null)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "000000:客户," + Masterpage.CurrUser.client_code + ",用户无可访问页面的权限");
                    SessionHelper.SetSession("AutoNeedLogin", "无可访问页面的权限");
                    return Redirect("../account/login");
                }
                else
                {
                    return Redirect("../" + f.ModuleFunctionRoute);
                }
               // return RedirectToRoute();

            }
        }

        [LoginAllow]
        public ActionResult webhead()
        {
            var json = Masterpage.GetClientsJson();
            var showcode = Masterpage.CurrUser.client_code;
            var showname = Masterpage.CurrUser.client_name;
            dynamic data = new System.Dynamic.ExpandoObject();
            data.json = json;
            data.showcode = showcode;
            data.showname = showname;
            return PartialView(data);
        }

        /// <summary>
        /// 工作台
        /// </summary>
        /// <returns></returns>
        public ActionResult platform()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:客户," + Masterpage.CurrUser.client_code +"打开工作台");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.code = Masterpage.CurrUser.client_code;
            var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
            #region 工艺三个小图表
            var str1 = chartconfig.chart1_1 + (chartconfig.chart1_2 != "" ? "," + chartconfig.chart1_2 : "");
            var str2 = chartconfig.chart2_1 + (chartconfig.chart2_2 != "" ? "," + chartconfig.chart2_2 : "");
            var str3 = chartconfig.chart3_1 + (chartconfig.chart3_2 != "" ? "," + chartconfig.chart3_2 : "");
            data.str1 = str1;
            data.str2 = str2;
            data.str3 = str3;
            data.chart1_1 = chartconfig.chart1_1;
            data.chart1_2 = chartconfig.chart1_2;
            data.chart2_1 = chartconfig.chart2_1;
            data.chart2_2 = chartconfig.chart2_2;
            data.chart3_1 = chartconfig.chart3_1;
            data.chart3_2 = chartconfig.chart3_2;

            data.unitname1 = chartconfig.unitname1;
            data.unitname2 = chartconfig.unitname2;
            data.unitname3 = chartconfig.unitname3;
            FlexChart chart1 = new FlexChart();
            FlexChart chart2 = new FlexChart();
            FlexChart chart3 = new FlexChart();
            if (str1 != null && str1 != "")
            {
                chart1 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str1);
                chart1.url = Utils.GetFlexAddress();
            }
            if (str2 != null && str2 != ""){ 
                chart2 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str2); 
                chart2.url = Utils.GetFlexAddress();
            }
            if (str3 != null && str3 != "")
            {
                chart3 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str3);
                chart3.url = Utils.GetFlexAddress();
            }
            data.chart1 = JsonHelper.ToJson(chart1);
            data.chart2 = JsonHelper.ToJson(chart2);
            data.chart3 = JsonHelper.ToJson(chart3);
            #endregion
            #region 工艺三个图表的下拉单元
            List<long> long1 = new List<long> { 7, 8, 9 };
            List<long> long2 = new List<long> { 10, 11, 12, 13 };
            List<long> long3 = new List<long> { 14, 15, 16 };
            var units1 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long1);
            var units2 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long2);
            var units3 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long3);
            data.units1 = JsonHelper.ToJson(units1.Select(p => new { text = p, id = p }).ToList());
            data.units2 = JsonHelper.ToJson(units2.Select(p => new { text = p, id = p }).ToList());
            data.units3 = JsonHelper.ToJson(units3.Select(p => new { text = p, id = p }).ToList());
            #endregion
            #region 每四个图表的耗材
            var hc = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct().ToList();
            data.material = hc;
            #endregion
            #region 进水监管链接的参数
            int isic =-1;
            if (chartconfig!= null)
            {
                if (chartconfig.unitname1.Contains("厌氧")) isic = 1;
                else if (chartconfig.unitname1.Contains("好氧")) isic = 2;
                else isic = 0;
            }
            data.isic = isic;
            #endregion
            #region 第四个小图表
            var config3 = centerService.GetPlatFormFourthChart(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config3);
            FlexChart chart4 = new FlexChart();
            string chart4_1 = "";
            string chart4_2 = "";
            string chart4name = "";
            if (config3.number != null && config3.number != "")
            {
                chart4 = centerService.GetFlexChartByChartNumber(config3.number);
                chart4.customercode = Masterpage.CurrUser.client_code;
                chart4.customername = Masterpage.CurrUser.client_name;
                chart4.processparms = config3.material + "|" + DateTime.Now.Year.ToString();
                chart4.queryparms = config3.material;
                chart4_1 = config3.material;
                chart4_2 = config3.number;
                chart4.url = Utils.GetFlexAddress();
                if (config3.material != null && config3.material != "")
                {
                    var m = materialService.GetOneMaterialSpecification(config3.material);
                    chart4name = m.MaterialSpecificationName;
                }
            }
            data.chart4_1 = chart4_1;
            data.chart4_2 = chart4_2;
            data.chart4name = chart4name;
            data.chart4 = JsonHelper.ToJson(chart4);
            #endregion

            #region 工艺报警
            //var alarms = WebAccountHelper.GetCollectionAlarm(Masterpage.CurrUser.client_code);
            var alarms = managementService.GetCustomerCollectionAlarmRes(Masterpage.CurrUser.client_code, "").Where(p => p.Status.HasValue&&!p.Status.Value).ToList();
            data.alarms = alarms;
            #endregion

            #region 操作建议
            var option = centerService.GetSystemOptimization(Masterpage.CurrUser.client_code);
            //var s2 = centerService.GetSystemDiagnostic(Masterpage.CurrUser.client_code);
            //var s3 = centerService.GetLastOptimizationSuggestion(Masterpage.CurrUser.client_code, 1);
            data.option = option;
            //data.s2 = s2;
            #endregion
            #region 流程图采集点
            //var vs = centerService.GetProcessCollectionValue(Masterpage.CurrUser.client_code).Select(p => new { code = p.CustomerCollectionCode.Replace("-", "$"), value = p.CustomerCollectionAvgValue });
            var date = DateTime.Now.AddDays(-1);// DateTime.Parse("2013-07-04");//
            var nalarms = alarms.Where(x => DateTime.Compare(x.CustomerAlaramHappenTime.Value.Date, date.Date) == 0).ToList();
            var vs = centerService.GetProcessPlatCollection(Masterpage.CurrUser.client_code, date).Select(p => new ProcessPlatCollection { code = p.code.Replace("-", "$"), value = p.value, low = p.low, up = p.up, alarm = 0 }).ToList();
            foreach (var item in nalarms)
            {
                var ha = vs.FirstOrDefault(p => p.code == item.CustomerCollectionCode.Replace("-", "$"));
                if (ha!=null)
                {
                    ha.alarm = 1;
                }
            }
            data.list = JsonHelper.ToJson(vs);
            #endregion
            #region 流程图开关量
            //var switchs = centerService.GetProcessSwitchValue(Masterpage.CurrUser.client_code).Select(p => new { code = p.CustomerCollectionCode.Replace("-", "$"), value = p.CustomerCollectionAvgValue.HasValue ? (int)p.CustomerCollectionAvgValue.Value : 0 });
            data.switchs = "";// JsonHelper.ToJson(switchs);
            #endregion
            data.date = date;
            return View(data);
        }
        [LoginAllow]
        public ActionResult default1()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:客户," + Masterpage.CurrUser.client_code + "打开工作台");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.code = Masterpage.CurrUser.client_code;
            var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
            #region 工艺三个小图表
            var str1 = chartconfig.chart1_1 + (chartconfig.chart1_2 != "" ? "," + chartconfig.chart1_2 : "");
            var str2 = chartconfig.chart2_1 + (chartconfig.chart2_2 != "" ? "," + chartconfig.chart2_2 : "");
            var str3 = chartconfig.chart3_1 + (chartconfig.chart3_2 != "" ? "," + chartconfig.chart3_2 : "");
            data.str1 = str1;
            data.str2 = str2;
            data.str3 = str3;
            data.chart1_1 = chartconfig.chart1_1;
            data.chart1_2 = chartconfig.chart1_2;
            data.chart2_1 = chartconfig.chart2_1;
            data.chart2_2 = chartconfig.chart2_2;
            data.chart3_1 = chartconfig.chart3_1;
            data.chart3_2 = chartconfig.chart3_2;

            data.unitname1 = chartconfig.unitname1;
            data.unitname2 = chartconfig.unitname2;
            data.unitname3 = chartconfig.unitname3;
            FlexChart chart1 = new FlexChart();
            FlexChart chart2 = new FlexChart();
            FlexChart chart3 = new FlexChart();
            if (str1 != null && str1 != "")
            {
                chart1 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str1);
                chart1.url = Utils.GetFlexAddress();
            }
            if (str2 != null && str2 != "")
            {
                chart2 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str2);
                chart2.url = Utils.GetFlexAddress();
            }
            if (str3 != null && str3 != "")
            {
                chart3 = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "", str3);
                chart3.url = Utils.GetFlexAddress();
            }
            data.chart1 = JsonHelper.ToJson(chart1);
            data.chart2 = JsonHelper.ToJson(chart2);
            data.chart3 = JsonHelper.ToJson(chart3);
            #endregion
            #region 工艺三个图表的下拉单元
            List<long> long1 = new List<long> { 7, 8, 9 };
            List<long> long2 = new List<long> { 10, 11, 12, 13 };
            List<long> long3 = new List<long> { 14, 15, 16 };
            var units1 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long1);
            var units2 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long2);
            var units3 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long3);
            data.units1 = JsonHelper.ToJson(units1.Select(p => new { text = p, id = p }).ToList());
            data.units2 = JsonHelper.ToJson(units2.Select(p => new { text = p, id = p }).ToList());
            data.units3 = JsonHelper.ToJson(units3.Select(p => new { text = p, id = p }).ToList());
            #endregion
            #region 每四个图表的耗材
            var hc = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct().ToList();
            data.material = hc;
            #endregion
            #region 进水监管链接的参数
            int selc = 0;
            int isic = 0;
            if (chartconfig.chart1_1 != "")
            {
                if (chartconfig.chart1_1.Contains("_IC-"))
                {
                    isic = 1;
                    selc++;
                    if (chartconfig.chart1_2 != "")
                    {
                        if (chartconfig.chart1_2.Contains("_IC-"))
                        {
                            selc++;
                        }
                    }
                }
                else
                {
                    isic = 0;
                    selc++;
                    if (chartconfig.chart1_2 != "")
                    {
                        if (!chartconfig.chart1_2.Contains("_IC-"))
                        {
                            selc++;
                        }
                    }
                }
            }
            data.selc = selc;
            data.isic = isic;
            #endregion
            #region 第四个小图表
            var config3 = centerService.GetPlatFormFourthChart(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config3);
            FlexChart chart4 = new FlexChart();
            string chart4_1 = "";
            string chart4_2 = "";
            string chart4name = "";
            if (config3.number != null && config3.number != "")
            {
                chart4 = centerService.GetFlexChartByChartNumber(config3.number);
                chart4.customercode = Masterpage.CurrUser.client_code;
                chart4.customername = Masterpage.CurrUser.client_name;
                chart4.processparms = config3.material + "|" + DateTime.Now.Year.ToString();
                chart4.queryparms = config3.material;
                chart4_1 = config3.material;
                chart4_2 = config3.number;
                chart4.url = Utils.GetFlexAddress();
                if (config3.material != null && config3.material != "")
                {
                    var m = materialService.GetOneMaterialSpecification(config3.material);
                    chart4name = m.MaterialSpecificationName;
                }
            }
            data.chart4_1 = chart4_1;
            data.chart4_2 = chart4_2;
            data.chart4name = chart4name;
            data.chart4 = JsonHelper.ToJson(chart4);
            #endregion

            #region 工艺报警
            //var alarms = WebAccountHelper.GetCollectionAlarm(Masterpage.CurrUser.client_code);
            var alarms = managementService.GetCustomerCollectionAlarmRes(Masterpage.CurrUser.client_code, "").Where(p => p.Status.HasValue && !p.Status.Value).ToList();
            data.alarms = alarms;
            #endregion

            #region 操作建议
            var option = centerService.GetSystemOptimization(Masterpage.CurrUser.client_code);
            //var s2 = centerService.GetSystemDiagnostic(Masterpage.CurrUser.client_code);
            //var s3 = centerService.GetLastOptimizationSuggestion(Masterpage.CurrUser.client_code, 1);
            data.option = option;
            //data.s2 = s2;
            #endregion
            #region 流程图采集点
            //var vs = centerService.GetProcessCollectionValue(Masterpage.CurrUser.client_code).Select(p => new { code = p.CustomerCollectionCode.Replace("-", "$"), value = p.CustomerCollectionAvgValue });
            var date = DateTime.Now.AddDays(-1);// DateTime.Parse("2013-07-04");//
            var nalarms = alarms.Where(x => DateTime.Compare(x.CustomerAlaramHappenTime.Value.Date, date.Date) == 0).ToList();
            var vs = centerService.GetProcessPlatCollection(Masterpage.CurrUser.client_code, date).Select(p => new ProcessPlatCollection { code = p.code.Replace("-", "$"), value = p.value, low = p.low, up = p.up, alarm = 0 }).ToList();
            foreach (var item in nalarms)
            {
                var ha = vs.FirstOrDefault(p => p.code == item.CustomerCollectionCode.Replace("-", "$"));
                if (ha != null)
                {
                    ha.alarm = 1;
                }
            }
            data.list = JsonHelper.ToJson(vs);
            #endregion
            #region 流程图开关量
            //var switchs = centerService.GetProcessSwitchValue(Masterpage.CurrUser.client_code).Select(p => new { code = p.CustomerCollectionCode.Replace("-", "$"), value = p.CustomerCollectionAvgValue.HasValue ? (int)p.CustomerCollectionAvgValue.Value : 0 });
            data.switchs = "";// JsonHelper.ToJson(switchs);
            #endregion
            data.date = date;
            return View(data);
        }
        [LoginAllow]
        public ActionResult menu()
        {
            return PartialView();
        }
        #region 前台ajax调用
        [LoginAllow]
        public ActionResult changeclient(string code)
        {
            var fcode = Request.Form["code"].ToString();
            var fname = Request.Form["name"].ToString();
            Masterpage.SetShowClient(fcode, fname);
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:切换客户，由" + Masterpage.CurrUser.client_code + "切换到" + fcode);
            return Content("ok");
        }

        /// <summary>
        /// 工作台小图表获取单元的采集
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [LoginAllow]
        public ActionResult getunitcollections(string unit, string items)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:获取工作台三个图表单元对应的采集点列表，单元为" + unit + "，配置项目编号为" + items);
            string[] array = items.Split(',');
            List<long> ids = new List<long>();
            foreach (var item in array)
            {
                if (item != "") ids.Add(long.Parse(item));
            }
            var points = centerService.GetThreeChartCustomerCollectionByUnit(Masterpage.CurrUser.client_code, unit, ids);
            if (points != null)
            {
                var list = points.Select(p => new { text = p.text, id = p.value, left = p.double_value });
                return Json(JsonHelper.ToJson(list), JsonRequestBehavior.AllowGet);
            }
            return Content("null");
        }

        [LoginAllow]
        public ActionResult savenewconfig(int number, string unitname, string select1, string select2)
        {
            #region 保存配置
            var r = accountService.SavePlatChartPartConfig(number, Masterpage.CurrUser.client_code, Masterpage.CurrUser.IsEmployee, Masterpage.CurrUser.guid, Masterpage.CurrUser.config1, unitname, select1, select2);
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:保存工作台三个图表的用户配置config1，单元为" + unitname + "，两个配置为" + select1 + "," + select2 + ",结果：" + r.status);
            if (r.status == "ok")
            {
                Masterpage.CurrUser.config1 = r.value;
                SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                var nc = Masterpage.CurrUser.config1;
            }
            #endregion
            #region 图表
            FlexChart chart = new FlexChart();
            if (select1 != "" || select2 != "")
            {
                if (select1 == null) select1 = "";
                if (select2 == null) select2 = "";
                var select = select1 + (select2 != "" ? "," + select2 : "");
                chart = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "历史查询", select);
                chart.processparms = select;
                chart.title = "工作台图表";
                chart.queryparms = "";
                chart.url = Utils.GetFlexAddress();
            }
            #endregion
            var back = new { save = r, chart = chart };
            return Json(JsonHelper.ToJson(back), JsonRequestBehavior.AllowGet);
        }

        [LoginAllow]
        public ActionResult savematerialconfig(string number, string material, string materialn)
        {

            #region 保存配置
            var r = accountService.SavePlatChartFourConfig(Masterpage.CurrUser.client_code, Masterpage.CurrUser.IsEmployee, Masterpage.CurrUser.guid, Masterpage.CurrUser.config3, number, material);
            LogHelper.Info(Masterpage.CurrUser.alias, "101011:保存工作台第四个耗材图表的用户配置config3，图表编号为" + number + "，配置耗材为" + material + ",结果：" + r.status);
            if (r.status == "ok")
            {
                Masterpage.CurrUser.config3 = r.value;
                SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                var nc = Masterpage.CurrUser.config3;
            }
            #endregion
            #region 第四个小图表
            FlexChart chart4 = new FlexChart();
            chart4 = centerService.GetFlexChartByChartNumber(number);
            chart4.customercode = Masterpage.CurrUser.client_code;
            chart4.customername = Masterpage.CurrUser.client_name;
            chart4.processparms = material + "|" + DateTime.Now.Year.ToString();
            chart4.url = Utils.GetFlexAddress();
            #endregion
            var back = new { save = r, chart = chart4 };
            return Json(JsonHelper.ToJson(back), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
