using ecoBio.Wms.Common;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using ecoBio.Wms.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.Data.Entities.Models;

namespace ecoBio.Wms.Web.Controllers
{
    public class usercenterController : BaseController
    {

        private MaterialService materialService;
        private CostanalysisService costanalysisService;
        private DataCenterService centerService;
        private AccountService accountService;
        public usercenterController(IAccountRepository _rightRepository, IDataCenterRepository _datacenterRepository,IMaterialRepository _materialRepository, ICostanalysisRepository _costanalysisRepository)
        {
            accountService = new AccountService(_rightRepository);
            centerService = new DataCenterService(_datacenterRepository);
            materialService = new MaterialService(_materialRepository);
            costanalysisService = new CostanalysisService(_costanalysisRepository);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult index()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var mslist = accountService.GetUserModuelFunction(Masterpage.CurrUser.role_guid);
            var ms = mslist.Where(p => p.ModuleFunctionType == "M" && p.ModuleFunctionId.StartsWith("U")).ToList();
            data.list1 = ms;
            data.list2 = mslist;
            data.chinesename = Masterpage.CurrUser.alias;
            return View(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult welcome()
        {
            return PartialView();
        }

        [UserCenter(Admin = true)]
        [LoginAllow]
        public ActionResult admin()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.roles = accountService.GetUserRoleDDL();
            return View(data);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult info()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
            data.user = Masterpage.CurrUser;
            #region 配置一
            data.chart1_1 = chartconfig.chart1_1;
            data.chart1_2 = chartconfig.chart1_2;
            data.chart2_1 = chartconfig.chart2_1;
            data.chart2_2 = chartconfig.chart2_2;
            data.chart3_1 = chartconfig.chart3_1;
            data.chart3_2 = chartconfig.chart3_2;
            #endregion

            #region  配置2
            var units = centerService.GetCustomerHaveStandardUnit(Masterpage.CurrUser.client_code);
            var weekly = centerService.GetServiceWeeklyConfig(Masterpage.CurrUser.client_code, units, Masterpage.CurrUser.config2);
            data.units = units;
            data.weekly = weekly;
            #endregion
            return View(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult info2()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
            data.user = Masterpage.CurrUser;
            #region 配置一
            data.chart1_1 = chartconfig.chart1_1;
            data.chart1_2 = chartconfig.chart1_2;
            data.chart2_1 = chartconfig.chart2_1;
            data.chart2_2 = chartconfig.chart2_2;
            data.chart3_1 = chartconfig.chart3_1;
            data.chart3_2 = chartconfig.chart3_2;
            string chart4_1 = "";
            string chart4_2 = "";
            #endregion

            #region  配置2
            var units = centerService.GetCustomerHaveStandardUnit(Masterpage.CurrUser.client_code);
            var weekly = centerService.GetServiceWeeklyConfig(Masterpage.CurrUser.client_code, units, Masterpage.CurrUser.config2);
            data.units = units;
            data.weekly = weekly;
            #endregion
            #region 配置3
            var chart4 = centerService.GetPlatFormFourthChart(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config3);
            chart4_1 = chart4.number;
            chart4_2 = chart4.material;
            data.chart4_1 = chart4_1;
            data.chart4_2 = chart4_2;
            #endregion

            return View(data);
        }

        #region 工作台配置
        //configplatchart", "usercenter", new { index = 1, config = 7 });
        [UserCenter]
        [LoginAllow]
        public ActionResult configplatchart(int? index, int? config, string select1, string select2)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int _config = config.HasValue ? config.Value : 7;
            int _index = index.HasValue ? index.Value : 1;
            var list = accountService.UCGetPlatChartPoints(Masterpage.CurrUser.client_code, _config);
            data.config = config;
            data.index = _index;
            data.select1 = select1;
            data.select2 = select2;
            data.list = list;
            return PartialView(data);
        }

        //configplatchart", "usercenter", new { index = 1, config = 7 });
        [UserCenter]
        [LoginAllow]
        public ActionResult configplatchart2(int? index, int? config, int? isguoup, string select1, string select2)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int _config = config.HasValue ? config.Value : 7;
            int _index = index.HasValue ? index.Value : 1;
            var list = accountService.UCGetPlatChartPoints(Masterpage.CurrUser.client_code, _config);


            #region  处理组合采集点
            List<CustomerCollectionModel> models = new List<CustomerCollectionModel>();
            var t = _config;
            if (list != null && list.Count > 0)
            {
                if (t >= 7 && t <= 9)
                {
                    #region 进水监管 -I和(L)组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        CustomerCollectionModel half = new CustomerCollectionModel();
                        string halfname = "";
                        if (item.collent_point_name.EndsWith("-I")) halfname = item.collent_point_name.Replace("-I", "(L)");
                        if (item.collent_point_name.EndsWith("(L)")) halfname = item.collent_point_name.Replace("(L)", "-I");
                        half = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_name == halfname);
                        if (half != null && half.CustomerCollectionCode != "")
                        {
                            item.isgroup = 1;
                            item.customer_point_code1 = item.CustomerCollectionCode;
                            item.customer_point_code2 = half.CustomerCollectionCode;
                            item.point_html1 = item.collent_point_html;
                            item.point_html2 = half.collent_point_html;
                            item.point_remark1 = item.collent_point_remark;
                            item.point_remark2 = half.collent_point_remark;
                            item.CustomerCollectionCode = item.CustomerCollectionCode + "," + half.CustomerCollectionCode;
                            models.Add(item);
                        }
                        else
                        {
                            models.Add(item);
                        }
                    }
                    #endregion
                }
                else if (t >= 14 && t <= 16)
                {
                    #region 水质管理 -I和-E组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        CustomerCollectionModel half = new CustomerCollectionModel();
                        string halfname = "";
                        if (item.collent_point_name.EndsWith("-I")) halfname = item.collent_point_name.Replace("-I", "-E");
                        if (item.collent_point_name.EndsWith("-E")) halfname = item.collent_point_name.Replace("-E", "-I");
                        half = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_name == halfname);
                        if (half != null && half.CustomerCollectionCode != "")
                        {
                            item.isgroup = 1;
                            item.customer_point_code1 = item.CustomerCollectionCode;
                            item.customer_point_code2 = half.CustomerCollectionCode;
                            item.point_html1 = item.collent_point_html;
                            item.point_html2 = half.collent_point_html;
                            item.point_remark1 = item.collent_point_remark;
                            item.point_remark2 = half.collent_point_remark;
                            item.CustomerCollectionCode = item.CustomerCollectionCode + "," + half.CustomerCollectionCode;
                            models.Add(item);
                        }
                        else
                        {
                            models.Add(item);
                        }
                    }
                    #endregion
                }
                else if (t == 17)
                {
                    #region 系统排放 D与DT组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        else
                        {
                            if (item.collent_point_code == "I601")//ＤＷＣＱ排放量
                            {
                                models.Add(item);
                            }
                            else
                            {
                                string w1 = item.collent_point_name.Substring(0, 1);    //DSS
                                string wr = item.collent_point_name.Substring(1);       //SS
                                string part2 = "DT" + wr;                               //DTSS
                                var hadpart2 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_name == part2);
                                if (w1 == "D" && hadpart2 != null)                      //D和DT同时存在
                                {
                                    item.isgroup = 1;
                                    item.customer_point_code1 = item.CustomerCollectionCode;
                                    item.customer_point_code2 = hadpart2.CustomerCollectionCode;
                                    item.point_html1 = item.collent_point_html;
                                    item.point_html2 = hadpart2.collent_point_html;
                                    item.point_remark1 = item.collent_point_remark;
                                    item.point_remark2 = hadpart2.collent_point_remark;
                                    item.CustomerCollectionCode = item.CustomerCollectionCode + "," + hadpart2.CustomerCollectionCode;
                                    models.Add(item);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (t == 22)
                {
                    #region 废水回量与率组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        else
                        {
                            CustomerCollectionModel i33 = new CustomerCollectionModel();
                            if (item.collent_point_code == "I513")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I514");
                            }
                            if (item.collent_point_code == "I514")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I513");
                            }
                            if (i33 != null && i33.CustomerCollectionCode != "")
                            {
                                item.isgroup = 1;
                                item.customer_point_code1 = item.CustomerCollectionCode;
                                item.customer_point_code2 = i33.CustomerCollectionCode;
                                item.point_html1 = item.collent_point_html;
                                item.point_html2 = i33.collent_point_html;
                                item.point_remark1 = item.collent_point_remark;
                                item.point_remark2 = i33.collent_point_remark;
                                item.CustomerCollectionCode = item.CustomerCollectionCode + "," + i33.CustomerCollectionCode;
                                models.Add(item);
                            }
                            else
                            {
                                models.Add(item);
                            }
                        }
                    }
                    #endregion
                }
                else if (t == 23)
                {
                    #region 泥饼产量与率组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        else
                        {
                            CustomerCollectionModel i33 = new CustomerCollectionModel();
                            if (item.collent_point_code == "I331")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I332");
                            }
                            if (item.collent_point_code == "I332")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I331");
                            }
                            if (i33 != null && i33.CustomerCollectionCode != "")
                            {
                                item.isgroup = 1;
                                item.customer_point_code1 = item.CustomerCollectionCode;
                                item.customer_point_code2 = i33.CustomerCollectionCode;
                                item.point_html1 = item.collent_point_html;
                                item.point_html2 = i33.collent_point_html;
                                item.point_remark1 = item.collent_point_remark;
                                item.point_remark2 = i33.collent_point_remark;
                                item.CustomerCollectionCode = item.CustomerCollectionCode + "," + i33.CustomerCollectionCode;
                                models.Add(item);
                            }
                            else
                            {
                                models.Add(item);
                            }
                        }
                    }
                    #endregion
                }
                else if (t == 24)
                {
                    #region 沼气产量与率组合
                    foreach (var item in list)
                    {
                        var had = models.FirstOrDefault(p => p.customer_point_code1 == item.CustomerCollectionCode || p.customer_point_code2 == item.CustomerCollectionCode);
                        if (had != null)
                        {
                            continue;
                        }
                        else
                        {
                            CustomerCollectionModel i33 = new CustomerCollectionModel();
                            if (item.collent_point_code == "I333")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I334");
                            }
                            if (item.collent_point_code == "I334")
                            {
                                i33 = list.FirstOrDefault(p => p.StandardProcessUnitCode == item.StandardProcessUnitCode && p.collent_point_code == "I333");
                            }
                            if (i33 != null && i33.CustomerCollectionCode != "")
                            {
                                item.isgroup = 1;
                                item.customer_point_code1 = item.CustomerCollectionCode;
                                item.customer_point_code2 = i33.CustomerCollectionCode;
                                item.point_html1 = item.collent_point_html;
                                item.point_html2 = i33.collent_point_html;
                                item.point_remark1 = item.collent_point_remark;
                                item.point_remark2 = i33.collent_point_remark;
                                item.CustomerCollectionCode = item.CustomerCollectionCode + "," + i33.CustomerCollectionCode;
                                models.Add(item);
                            }
                            else
                            {
                                models.Add(item);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 不组合
                    foreach (var item in list)
                    {
                        models.Add(item);
                    }
                    #endregion
                }

            }
            #endregion


            data.config = config;
            data.index = _index;
            data.select1 = select1;
            data.select2 = select2;
            #region 初始化是否组合
            var md = models.FirstOrDefault(p => p.CustomerCollectionCode == select1 + "," + select2);
            if (md != null) isguoup = 1;
            data.isguoup = isguoup;
            #endregion
            data.list = models;
            return PartialView(data);
        }


        [UserCenter]
        [LoginAllow]
        public ActionResult savechartconfig()
        {
            string v1_1 = WebRequest.GetString("v1_1", true);
            string v1_2 = WebRequest.GetString("v1_2", true);
            string v2_1 = WebRequest.GetString("v2_1", true);
            string v2_2 = WebRequest.GetString("v2_2", true);
            string v3_1 = WebRequest.GetString("v3_1", true);
            string v3_2 = WebRequest.GetString("v3_2", true);
            string v4_1 = WebRequest.GetString("v4_1", true);
            string v4_2 = WebRequest.GetString("v4_2", true);

            ReturnValue rv = new ReturnValue();
            if (v1_1 == "" || v1_2 == "" || v2_1 == "" || v2_2 == "" || v3_1 == "" || v3_2 == ""|| v4_1 == "")
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                rv = accountService.SavePlatChartConfig(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1.Trim(), Masterpage.CurrUser.IsEmployee, Masterpage.CurrUser.guid, v1_1, v1_2, v2_1, v2_2, v3_1, v3_2, Masterpage.CurrUser.config3.Trim(), v4_1, v4_2);
                if (rv.status == "ok")
                {
                    Masterpage.CurrUser.config1 = rv.value;
                    Masterpage.CurrUser.config3 = rv.value2;
                    SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                    var nc = Masterpage.CurrUser.config1;
                }
            }
            LogHelper.Info(Masterpage.CurrUser.alias, "902014:客户," + Masterpage.CurrUser.client_code + ",用户中心保存用户配置1工作台图表配置,选择为：" + v1_1 + v1_2 + v2_1 + v2_2 + v3_1 + v3_2 + v4_1 + v4_2);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 周报配置

        [UserCenter]
        [LoginAllow]
        public ActionResult configweekly(int? index, string config, string unit)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int _index = index.HasValue ? index.Value : 0;
            var list = centerService.UCGetUnitAllPoints(Masterpage.CurrUser.client_code, unit);
            if (config != null && config != "")
            {
                string array = "[";
                string[] f = config.Split('#');
                foreach (var item in f)
                {
                    if (item != "")
                    {
                        array += ("'" + item + "',");
                    }
                }
                if (array.EndsWith(",")) array = array.Substring(0, array.Length - 1);
                array += "]";
                data.config = array;
            }
            else
                data.config = "[]";
            data.index = _index;
            data.unit = unit;
            data.list = list;
            return PartialView(data);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult configweekly2(int? index, string config, string unit)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int _index = index.HasValue ? index.Value : 0;
            var list = centerService.UCGetUnitAllPoints(Masterpage.CurrUser.client_code, unit);
            var units = centerService.GetCustomerHaveStandardUnit(Masterpage.CurrUser.client_code);

            if (config != null && config != "")
            {
                string array = "[";
                string[] f = config.Split('#');
                foreach (var item in f)
                {
                    if (item != "")
                    {
                        array += ("'" + item + "',");
                    }
                }
                if (array.EndsWith(",")) array = array.Substring(0, array.Length - 1);
                array += "]";
                data.config = array;
            }
            else
                data.config = "[]";
            data.index = _index;
            data.units = units;

            data.unit = unit;
            data.list = list;
            return PartialView(data);
        }
        
        [UserCenter]
        [LoginAllow]
        public ActionResult configplatchart4(string select1, string select2)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var hc = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct().ToList();
            //var hc2 = hc.Where(p => p.MaterialSpecificationCategeory == "营养剂").ToList();

            data.hc = hc;
            //data.hc2 = hc2;
            data.number = select1;
            data.material = select2;
            return PartialView(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult saveweeklyconfig()
        {
            string select = WebRequest.GetString("select", true);
            select = select.Replace("#", ",");
            ReturnValue rv = new ReturnValue();

            rv = centerService.SaveServiceWeeklyConfig(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config2, Masterpage.CurrUser.IsEmployee, Masterpage.CurrUser.guid, select);
            LogHelper.Info(Masterpage.CurrUser.alias, "902013:客户," + Masterpage.CurrUser.client_code + ",用户中心保存用户配置2服务周报采集点选取,选择为：" + select);
            if (rv.status == "ok")
            {
                Masterpage.CurrUser.config2 = rv.value;
                SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                var nc = Masterpage.CurrUser.config2;
            }

            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 用户列表
        [UserCenter]
        [LoginAllow]
        public ActionResult ucuserlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.roles = accountService.GetUserRoleDDL();
            var list = accountService.GetCustomerUserList(Masterpage.CurrUser.client_code, Masterpage.CurrUser.IsEmployee);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "";
            LogHelper.Info(Masterpage.CurrUser.alias, "902011:客户," + Masterpage.CurrUser.client_code + ",用户中心用户列表");
            return PartialView(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult getoneuser(Guid guid)
        {
            var one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, guid, Masterpage.CurrUser.IsEmployee);
            if (one != null)
                LogHelper.Info(Masterpage.CurrUser.alias, "902012:客户," + Masterpage.CurrUser.client_code + ",用户中心读取单个用户信息：用户登录标识" + one.loginid);
            else LogHelper.Info(Masterpage.CurrUser.alias, "902012:客户," + Masterpage.CurrUser.client_code + ",用户中心读取单个用户信息失败：用户guid" + guid);
            return Json(one, JsonRequestBehavior.AllowGet);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult deloneuser(Guid guid)
        {
            var one = accountService.DeleteCustomerUser(Masterpage.CurrUser.client_code, guid);
            return Json(one, JsonRequestBehavior.AllowGet);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult saveuser()
        {
            //string type, Guid guid, string loginid, string tokencode, string username, string email, string mobile, Guid role, bool valid, string remark
            string type = WebRequest.GetString("type", true);
            string uguid = WebRequest.GetString("uguid", true);
            string loginid = WebRequest.GetString("loginid", true);
            string tokencode = WebRequest.GetString("tokencode", true);
            string username = WebRequest.GetString("username", true);
            string email = WebRequest.GetString("email", true).Replace("#", "@");
            string mobile = WebRequest.GetString("mobile", true);
            string role = WebRequest.GetString("role", true);
            string valid = WebRequest.GetString("valid", true);
            string remark = WebRequest.GetString("remark", true);

            ReturnValue r;
            bool _valid;
            Guid guid = Guid.Empty;
            Guid rguid = Guid.Empty;
            #region 验证
            try
            {
                if (uguid != "")
                {
                    guid = Guid.Parse(uguid);
                }
                if (role != "")
                {
                    rguid = Guid.Parse(role);
                }
                type = "edit";
                _valid = Convert.ToBoolean(valid);
                r = accountService.SaveCustomerUser(Masterpage.CurrUser.client_code, type, guid, loginid, tokencode, username, email, mobile, rguid, _valid, remark);
                LogHelper.Info(Masterpage.CurrUser.alias, "902012:客户," + Masterpage.CurrUser.client_code + ",用户中心保存用户信息：操作类别：" + type + "，用户guid:,登录标识：" + loginid + "，令牌号：" + tokencode + "，姓名：" + username + "，邮箱：" + email + "，电话：" + mobile + "，角色：" + role + ",操作结果：" + r.status);
            }
            catch
            {
                r = new ReturnValue { status = "error", message = "参数有误" };
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }



        [UserCenter]
        [LoginAllow]
        public ActionResult userinfo2(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r;
            UserMdoel one;
            List<KeyValue> units = new List<KeyValue>();
            List<KeyValue> weekly = new List<KeyValue>();
            string chart1_1 = "";
            string chart1_2 = "";
            string chart2_1 = "";
            string chart2_2 = "";
            string chart3_1 = "";
            string chart3_2 = "";
            string chart4_1 = "";
            string chart4_2 = "";
            try
            {
                Guid g = Guid.Parse(guid);
                one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g,false);
                if (!Masterpage.CurrUser.IsAdmin)
                {
                    r = new ReturnValue { status = "error", message = "非法操作" };
                }
                else if (one != null && one.loginid != "")
                {
                    var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, one.config1);

                    #region 配置一
                    chart1_1 = chartconfig.chart1_1;
                    chart1_2 = chartconfig.chart1_2;
                    chart2_1 = chartconfig.chart2_1;
                    chart2_2 = chartconfig.chart2_2;
                    chart3_1 = chartconfig.chart3_1;
                    chart3_2 = chartconfig.chart3_2;
                    #endregion
                    #region  配置2
                    units = centerService.GetCustomerHaveStandardUnit(Masterpage.CurrUser.client_code);
                    weekly = centerService.GetServiceWeeklyConfig(Masterpage.CurrUser.client_code, units, one.config2);

                    #endregion
                    #region 配置3
                    var chart4 = centerService.GetPlatFormFourthChart(Masterpage.CurrUser.client_code, one.config3);
                    chart4_1 = chart4.number;
                    chart4_2 = chart4.material;
                    #endregion
                    r = new ReturnValue { status = "ok", message = "" };
                }
                else
                {
                    r = new ReturnValue { status = "error", message = "参数有误" };
                }
            }
            catch
            {
                one = new UserMdoel();
                r = new ReturnValue { status = "error", message = "程序异常" };
            } 
            var roles= accountService.GetUserRoleDDL(Masterpage.CurrUser.client_code);
            var adminrole = roles.FirstOrDefault(p => p.Text.Contains("ADMIN"));
            if (adminrole != null) roles.Remove(adminrole);
            data.roles = roles;
            #region 配置一
            data.chart1_1 = chart1_1;
            data.chart1_2 = chart1_2;
            data.chart2_1 = chart2_1;
            data.chart2_2 = chart2_2;
            data.chart3_1 = chart3_1;
            data.chart3_2 = chart3_2;
            data.chart4_1 = chart4_1;
            data.chart4_2 = chart4_2;
            #endregion
            data.user = one;
            data.units = units;
            data.weekly = weekly;
            data.r = r;
            return PartialView("userinfo2", data);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult saveuserchartconfig(string guid)
        {
            string v1_1 = WebRequest.GetString("v1_1", true);
            string v1_2 = WebRequest.GetString("v1_2", true);
            string v2_1 = WebRequest.GetString("v2_1", true);
            string v2_2 = WebRequest.GetString("v2_2", true);
            string v3_1 = WebRequest.GetString("v3_1", true);
            string v3_2 = WebRequest.GetString("v3_2", true);
            string v4_1 = WebRequest.GetString("v4_1", true);
            string v4_2 = WebRequest.GetString("v4_2", true);

            ReturnValue rv = new ReturnValue();
            if (guid == "" || v1_1 == "" || v1_2 == "" || v2_1 == "" || v2_2 == "" || v3_1 == "" || v3_2 == ""|| v4_1 == "")
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                Guid g = Guid.Parse(guid);
                var one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g,Masterpage.CurrUser.IsEmployee);
                if (one.config1 == null) one.config1 = ""; 
                if (one.config3 == null) one.config3 = "";
                rv = accountService.SavePlatChartConfig(Masterpage.CurrUser.client_code, one.config1.Trim(), one.isemployee, g, v1_1, v1_2, v2_1, v2_2, v3_1, v3_2, one.config3.Trim(), v4_1, v4_2);
                if (rv.status == "ok" && Masterpage.CurrUser.guid == g)
                {
                    Masterpage.CurrUser.config1 = rv.value;
                    Masterpage.CurrUser.config3 = rv.value2;
                    SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                    var nc = Masterpage.CurrUser.config1;
                }
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }


        [UserCenter]
        [LoginAllow]
        public ActionResult saveuserweeklyconfig(string guid)
        {
            string select = WebRequest.GetString("select", true);
            select = select.Replace("#", ",");
            ReturnValue rv = new ReturnValue();
            Guid g = Guid.Parse(guid);
            bool b = false;
            if (g == Masterpage.CurrUser.guid) b = true;
            var one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g, b);
            if (one.config2 == null) one.config2 = "";
            rv = centerService.SaveServiceWeeklyConfig(Masterpage.CurrUser.client_code, one.config2, one.isemployee, g, select);
            if (rv.status == "ok" && g == Masterpage.CurrUser.guid)
            {
                Masterpage.CurrUser.config2 = rv.value;
                SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                var nc = Masterpage.CurrUser.config2;
            }

            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 管理员用户中心
        [UserCenter(Admin = true)]
        public ActionResult mainframe()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.roles = accountService.GetUserRoleList(Masterpage.CurrUser.client_code); 
            var list = accountService.GetCustomerUserList(Masterpage.CurrUser.client_code,Masterpage.CurrUser.IsEmployee).ToList();
            data.users = list;
            data.code = Masterpage.CurrUser.client_code;
            data.isemployee = Masterpage.CurrUser.IsEmployee;
            data.guid = Masterpage.CurrUser.guid.ToString();
            if (Masterpage.CurrUser.IsEmployee && list.Count > 0) data.guid = list[0].guid.ToString();

            LogHelper.Info(Masterpage.CurrUser.alias, "901011:客户," + Masterpage.CurrUser.client_code + ",管理员用户中心");
            return View(data);
        }
        #region 耗材申请

        [UserCenter(Admin = true)]
        public ActionResult applymaterial(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = centerService.GetBasicDataApplications(Masterpage.CurrUser.client_code, "");
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = ""; 
            LogHelper.Info(Masterpage.CurrUser.alias, "903011:客户," + Masterpage.CurrUser.client_code + ",用户中心耗材申请列表" );
            return PartialView(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult materialpage(string t, int? sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r;
            CustomerBasicDataApplication one = new CustomerBasicDataApplication(); ;
            if (!sn.HasValue || t == null)
            {
                r = new ReturnValue { status = "error", message = "参数错误" };
            }
            else
            {
                if (t == "edit")
                {
                    one = centerService.GetOneBasicDataApplications(Masterpage.CurrUser.client_code, sn.Value);
                    if (one == null)
                    {
                        r = new ReturnValue { status = "error", message = "申请不存在" };
                        LogHelper.Info(Masterpage.CurrUser.alias, "903012:客户," + Masterpage.CurrUser.client_code + ",用户中心修改耗材申请，sn:" + sn + "申请不存在");
                    }
                    else
                    {
                        r = new ReturnValue { status = "ok", message = "" };
                        LogHelper.Info(Masterpage.CurrUser.alias, "903012:客户," + Masterpage.CurrUser.client_code + ",用户中心修改耗材申请，sn:" + sn);
                    }
                }
                else if (t == "add")
                {
                    r = new ReturnValue { status = "ok", message = "" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "903012:客户," + Masterpage.CurrUser.client_code + ",用户中心新增耗材申请");

                }
                else r = new ReturnValue { status = "error", message = "参数错误" };
            }
            var cates = centerService.GetMaterialCategory().Select(p => new SelectListItem { Text = p.value, Value = p.value }).ToList();
            data.cates = cates;
            data.t = t;
            data.r = r;
            data.person=Masterpage.CurrUser.alias;
            data.one = one;
            return View(data);
        }
        [UserCenter(Admin = true)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult savematerial(string t, int? sn)
        {
            string c = WebRequest.GetString("cate", true);
            string r = WebRequest.GetString("content", true);
            ReturnValue rv = new ReturnValue();
            if (c == "" || r == "" || t == null || !sn.HasValue)
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                ////耗材|硫酸|98%|营养济|Kg
                string content = c + "|" + r;
                rv = centerService.SaveBasicDataApplications(Masterpage.CurrUser.client_code, t, sn.Value, Masterpage.CurrUser.guid, content, Masterpage.CurrUser.alias, "");
                LogHelper.Info(Masterpage.CurrUser.alias, "903013:客户," + Masterpage.CurrUser.client_code + ",用户中心保存耗材申请，sn:" + sn + "类别：" + c + "，内容：" + r);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [UserCenter(Admin = true)]
        public ActionResult delmaterial(int? sn)
        {
            ReturnValue rv = new ReturnValue();
            if (!sn.HasValue)
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                rv = centerService.DelBasicDataApplications(Masterpage.CurrUser.client_code, sn.Value);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 供应商申请

        [UserCenter(Admin = true)]
        public ActionResult applysupplier(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = centerService.GetBasicDataApplications(Masterpage.CurrUser.client_code, "供应商");
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "";
            return PartialView(data);
        }

        [UserCenter(Admin = true)]
        public ActionResult supplierpage(string t, int? sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r;
            CustomerBasicDataApplication one = new CustomerBasicDataApplication(); ;
            if (!sn.HasValue || t == null)
            {
                r = new ReturnValue { status = "error", message = "参数错误" };
            }
            else
            {
                if (t == "edit")
                {
                    one = centerService.GetOneBasicDataApplications(Masterpage.CurrUser.client_code, sn.Value);
                    if (one == null) r = new ReturnValue { status = "error", message = "申请不存在" };
                    else r = new ReturnValue { status = "ok", message = "" };
                }
                else if (t == "add")
                {
                    r = new ReturnValue { status = "ok", message = "" };

                }
                else r = new ReturnValue { status = "error", message = "参数错误" };
            }
            var cates = centerService.GetSupperCategory().Select(p => new SelectListItem { Text = p.value, Value = p.value }).ToList();
            data.cates = cates;
            data.t = t;
            data.r = r;
            data.person = Masterpage.CurrUser.alias;
            data.one = one;
            return View(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult savesupplier(string t, int? sn)
        {
            string  name = WebRequest.GetString("name", false);
            string address = WebRequest.GetString("address", true);
            string phone = WebRequest.GetString("phone", true);
            string fax = WebRequest.GetString("fax", true);
            string websit = WebRequest.GetString("websit", true);
            string cate = WebRequest.GetString("cate", true);
            string person = WebRequest.GetString("person", true);
            string mobile = WebRequest.GetString("mobile", true);
            string email = WebRequest.GetString("email", true);
            string writer = WebRequest.GetString("writer", true);
            string remark = WebRequest.GetString("remark", true);
            ReturnValue rv = new ReturnValue();
            if (name == "" || cate == "" || !sn.HasValue)
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                ////耗材|硫酸|98%|营养济|Kg
                string content = "供应商|" + name + "|" + cate + "|" + address + "|" + phone + "|" + fax + "|" + websit + "|" + person + "|" + mobile + "|" + email;
                rv = centerService.SaveBasicDataApplications(Masterpage.CurrUser.client_code, t, sn.Value, Masterpage.CurrUser.guid, content, writer, remark);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [UserCenter(Admin = true)]
        public ActionResult delsupplier(int? sn)
        {
            ReturnValue rv = new ReturnValue();
            if (!sn.HasValue)
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                rv = centerService.DelBasicDataApplications(Masterpage.CurrUser.client_code, sn.Value);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [UserCenter(Admin = true)]
        public ActionResult rolepowers(string role)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue() ;            
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            Guid guid;
            try
            {
                guid = Guid.Parse(role);
                var allq = accountService.GetCustomerAdminModuleFunction(Masterpage.CurrUser.client_code);
                var hadq = accountService.GetModuleFunctionByRole(guid);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                LogHelper.Info(Masterpage.CurrUser.alias, "904011:客户," + Masterpage.CurrUser.client_code + ",用户中心角色权限，角色:" + role );
                r = new ReturnValue { status = "ok", message = "" };
            }
            catch
            {
                guid = Guid.Empty;
                r = new ReturnValue {status="error",message="读取失败" };
            }
            data.all = all;
            data.had = had;
            data.r = r;
            data.guid = guid;
            return PartialView(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult saverolepowers(Guid guid)
        {
            ReturnValue rv = new ReturnValue();
            try
            {
                string addstr = WebRequest.GetString("add", true);
                string delstr = WebRequest.GetString("del", true);
                if (addstr.EndsWith("#")) addstr=addstr.Substring(0, addstr.Length - 1);
                if (delstr.EndsWith("#")) delstr=delstr.Substring(0, delstr.Length - 1);
                string[] add = addstr.Split('#');
                string[] del = delstr.Split('#');
                bool b = accountService.SaveRoleRigths(guid, add, del);
                if (b) rv = new ReturnValue { status = "ok", message = "" };
                else rv = new ReturnValue { status = "error", message = "保存失败" };
                LogHelper.Info(Masterpage.CurrUser.alias, "904012:客户," + Masterpage.CurrUser.client_code + ",用户中心保存角色权限，角色:" + guid + "，权限添加：" + addstr + "，权限减少：" + delstr);
          
            }
            catch
            { 
                rv = new ReturnValue { status = "error", message = "保存失败" }; 
            } 
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult userinfo(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r;
            UserMdoel one;
            List<KeyValue> units = new List<KeyValue>();
            List<KeyValue> weekly = new List<KeyValue>();
            string chart1_1 = "";
            string chart1_2 = "";
            string chart2_1 = "";
            string chart2_2 = "";
            string chart3_1 = "";
            string chart3_2 = "";
            try
            {
                Guid g = Guid.Parse(guid); bool b = false;
                if (g == Masterpage.CurrUser.guid && Masterpage.CurrUser.IsEmployee) b = true;
                one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g, b);
                if (!Masterpage.CurrUser.IsAdmin)
                {
                    r = new ReturnValue { status = "error", message = "非法操作" };
                }
                else if (one != null && one.loginid != "")
                {
                    var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, one.config1);

                    #region 配置一
                    chart1_1 = chartconfig.chart1_1;
                    chart1_2 = chartconfig.chart1_2;
                    chart2_1 = chartconfig.chart2_1;
                    chart2_2 = chartconfig.chart2_2;
                    chart3_1 = chartconfig.chart3_1;
                    chart3_2 = chartconfig.chart3_2;
                    #endregion
                    #region  配置2
                    units = centerService.GetCustomerHaveStandardUnit(Masterpage.CurrUser.client_code);
                    weekly = centerService.GetServiceWeeklyConfig(Masterpage.CurrUser.client_code, units, one.config2);

                    #endregion
                    r = new ReturnValue { status = "ok", message = "" };
                }
                else
                {
                    r = new ReturnValue { status = "error", message = "参数有误" };
                }
            }
            catch
            {
                one = new UserMdoel();
                r = new ReturnValue { status = "error", message = "程序异常" };
            }
            data.roles = accountService.GetUserRoleDDL();
            #region 配置一
            data.chart1_1 = chart1_1;
            data.chart1_2 = chart1_2;
            data.chart2_1 = chart2_1;
            data.chart2_2 = chart2_2;
            data.chart3_1 = chart3_1;
            data.chart3_2 = chart3_2;
            #endregion
            data.user = one;
            data.units = units;
            data.weekly = weekly;
            data.r = r;
            return PartialView("userinfo", data);
        }
        #endregion

    }
}
