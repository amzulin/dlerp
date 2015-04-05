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
    public class useradminController : BaseController
    {
        private MaterialService materialService;
        private CostanalysisService costanalysisService;
        private DataCenterService centerService;
        private AccountService accountService;
        public useradminController(IAccountRepository _rightRepository, IDataCenterRepository _datacenterRepository, IMaterialRepository _materialRepository, ICostanalysisRepository _costanalysisRepository)
        {
            accountService = new AccountService(_rightRepository);
            centerService = new DataCenterService(_datacenterRepository);
            materialService = new MaterialService(_materialRepository);
            costanalysisService = new CostanalysisService(_costanalysisRepository);
        }


        #region 管理员用户中心
        //[UserCenter(Admin = true)]

        [UserCenter]
        [LoginAllow]
        public ActionResult mainframe()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.roles = accountService.GetUserRoleList(Masterpage.CurrUser.client_code);
            data.code = Masterpage.CurrUser.client_code;
            data.isemployee = Masterpage.CurrUser.IsEmployee;
            data.isadmin = Masterpage.CurrUser.IsAdmin;
            data.guid = Masterpage.CurrUser.guid.ToString();

            var list = accountService.GetCustomerUserList(Masterpage.CurrUser.client_code,Masterpage.CurrUser.IsEmployee).ToList();
            data.users = list;
            
            //if (Masterpage.CurrUser.IsEmployee && list.Count > 0) data.guid = list[0].guid.ToString();

            LogHelper.Info(Masterpage.CurrUser.alias, "901010:客户," + Masterpage.CurrUser.client_code + ",管理员用户中心");
            return View(data);
        }

        #region 用户管理
        [UserCenter(Admin = true)]
        public ActionResult userlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = accountService.GetCustomerUserList(Masterpage.CurrUser.client_code, false);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "";
            LogHelper.Info(Masterpage.CurrUser.alias, "901011:客户," + Masterpage.CurrUser.client_code + ",用户中心用户列表");
            return PartialView(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult oneuser(Guid? guid, string type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            UserMdoel one = new UserMdoel();
            if (type == "edit")
            {
                bool b = false;
                if (guid== Masterpage.CurrUser.guid && Masterpage.CurrUser.IsEmployee) b = true;
                one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, guid.Value, false);
                if (one == null)
                {
                    one = new UserMdoel();
                    type = "add";
                }
            }
            else one = new UserMdoel();
            var rolelist = accountService.GetUserRoleDDL(Masterpage.CurrUser.client_code);
            data.rolelist = rolelist;
            data.one = one;
            data.type = type;
            data.guid = guid;
            return View(data);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult saveuser()
        {
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
        [UserCenter(Admin = true)]
        public ActionResult deloneuser(Guid guid)
        {
            var one = accountService.DeleteCustomerUser(Masterpage.CurrUser.client_code, guid);
            LogHelper.Info(Masterpage.CurrUser.alias, "901013:客户," + Masterpage.CurrUser.client_code + ",用户中心删除用户，用户guid:"+guid.ToString());
            return Json(one, JsonRequestBehavior.AllowGet);
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
            string chart4_1 = "";
            string chart4_2 = "";
            string unitname_1 = "";
            string unitname_2 = "";
            string unitname_3 = "";
            List<string> units1 = new List<string>();
            List<string> units2 = new List<string>();
            List<string> units3 = new List<string>();
            List<KeyValue> myroles = new List<KeyValue>();
            try
            {
                Guid g = Guid.Parse(guid); 
                bool b = false;
                if (g == Masterpage.CurrUser.guid && Masterpage.CurrUser.IsEmployee) b = true;
                one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g, b);
                 if (one != null && one.loginid != "")
                {
                    var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, one.config1);

                    #region 配置一
                    chart1_1 = chartconfig.chart1_1;
                    chart1_2 = chartconfig.chart1_2;
                    chart2_1 = chartconfig.chart2_1;
                    chart2_2 = chartconfig.chart2_2;
                    chart3_1 = chartconfig.chart3_1;
                    chart3_2 = chartconfig.chart3_2;
                    unitname_1 = chartconfig.unitname1;
                    unitname_2 = chartconfig.unitname2;
                    unitname_3 = chartconfig.unitname3;
                    List<long> long1 = new List<long> { 7, 8, 9 };
                    List<long> long2 = new List<long> { 10, 11, 12, 13 };
                    List<long> long3 = new List<long> { 14, 15, 16 };
                    units1 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long1);
                    units2 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long2);
                    units3 = centerService.GetThreeChartStandardProcessUnit(Masterpage.CurrUser.client_code, long3);
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
                    myroles = accountService.GetUserAllRoles(g, Masterpage.CurrUser.IsEmployee);
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
            var roles = accountService.GetUserRoleDDL(Masterpage.CurrUser.client_code);
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
            data.unitname_1 = unitname_1;
            data.unitname_2 = unitname_2;
            data.unitname_3 = unitname_3;
            data.unitlist1 = units1;
            data.unitlist2 = units2;
            data.unitlist3 = units3;
            #endregion
            data.user = one;
            data.myroles = myroles;
            data.units = units;
            data.weekly = weekly;
            data.r = r;
            LogHelper.Info(Masterpage.CurrUser.alias, "901014:客户," + Masterpage.CurrUser.client_code + ",用户中心用户配置页面，用户：" + one.chinesename);
            return PartialView("userinfo", data);
        }

        [UserCenter]
        [LoginAllow]
        public ActionResult configplatchart(string unit, string items, int? isguoup, string select1, string select2)//,int? index, int? config, int? isguoup,
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string[] array = items.Split(',');
            List<long> ids = new List<long>();
            foreach (var item in array)
            {
                if (item != "") ids.Add(long.Parse(item));
            }
            var list = accountService.UCGetPlatChartPoints(Masterpage.CurrUser.client_code, unit, ids).Distinct().ToList();


            #region  处理组合采集点
            List<CustomerCollectionModel> models = new List<CustomerCollectionModel>();
            var t = items;
            if (list != null && list.Count > 0)
            {
                if (t == "7,8,9")
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
                else if (t == "14, 15, 16")
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


            data.unit = unit;
            data.items = items;
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
        public ActionResult configplatchart4(string select1, string select2)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var hc = materialService.GetMaterialSpecification(Masterpage.CurrUser.client_code).Distinct().ToList();
            data.hc = hc;
            data.number = select1;
            data.material = select2;
            return PartialView(data);
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
            string unit1 = WebRequest.GetString("unit1", true);
            string unit2 = WebRequest.GetString("unit2", true);
            string unit3 = WebRequest.GetString("unit3", true);

            ReturnValue rv = new ReturnValue();
            UserMdoel one=new UserMdoel();
            if (guid == "")// || v1_1 == "" || v1_2 == "" || v2_1 == "" || v2_2 == "" || v3_1 == "" || v3_2 == "" || v4_1 == ""
            {
                rv.status = "error";
                rv.message = "缺少参数";
            }
            else
            {
                Guid g = Guid.Parse(guid); 
                bool b = false;
                if (g == Masterpage.CurrUser.guid&&Masterpage.CurrUser.IsEmployee) b = true;
                one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g, b);
                if (one.config1 == null) one.config1 = "";
                if (one.config3 == null) one.config3 = "";
                rv = accountService.SavePlatChartConfig(Masterpage.CurrUser.client_code, one.config1.Trim(), one.isemployee, g, v1_1, v1_2, v2_1, v2_2, v3_1, v3_2, unit1, unit2, unit3, one.config3.Trim(), v4_1, v4_2);
                if (rv.status == "ok" && Masterpage.CurrUser.guid == g)
                {
                    Masterpage.CurrUser.config1 = rv.value;
                    Masterpage.CurrUser.config3 = rv.value2;
                    SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                    var nc = Masterpage.CurrUser.config1;
                }
            }
            LogHelper.Info(Masterpage.CurrUser.alias, "901015:客户," + Masterpage.CurrUser.client_code + ",用户中心保存用户工作台图表配置，用户：" + one.chinesename);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }



        [UserCenter]
        [LoginAllow]
        public ActionResult configweekly(int? index, string config, string unit)
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
        public ActionResult saveuserweeklyconfig(string guid)
        {
            string select = WebRequest.GetString("select", true);
            select = select.Replace("#", ",");
            ReturnValue rv = new ReturnValue();
            Guid g = Guid.Parse(guid); 
            bool b = false;
            if (g == Masterpage.CurrUser.guid&&Masterpage.CurrUser.IsEmployee) b = true;
            var one = accountService.GetOneCustomerUserModel(Masterpage.CurrUser.client_code, g,b);
            if (one.config2 == null) one.config2 = "";
            rv = centerService.SaveServiceWeeklyConfig(Masterpage.CurrUser.client_code, one.config2, one.isemployee, g, select);
            if (rv.status == "ok" && g == Masterpage.CurrUser.guid)
            {
                Masterpage.CurrUser.config2 = rv.value;
                SessionHelper.SetSession("LoginUser", Masterpage.CurrUser);
                var nc = Masterpage.CurrUser.config2;
            }

            LogHelper.Info(Masterpage.CurrUser.alias, "901016:客户," + Masterpage.CurrUser.client_code + ",用户中心保存用户报表配置，用户：" + one.chinesename);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 角色管理
        [UserCenter(Admin = true)]
        public ActionResult rolelist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = accountService.GetCustomerRole(Masterpage.CurrUser.client_code);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "";
            var rolestr = accountService.GetSystemCustomerRoleStr();
            data.rolestr = rolestr;
            data.code = Masterpage.CurrUser.client_code;
            LogHelper.Info(Masterpage.CurrUser.alias, "902011:客户," + Masterpage.CurrUser.client_code + ",用户中心角色列表");
            return PartialView(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult onerole(Guid? guid, string type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Role one = new Role();
            if (type == "edit")
            {
                one = accountService.GetOneCustomerRole(Masterpage.CurrUser.client_code, guid.Value);
                if (one == null)
                {
                    type = "add";
                }
            }
            var rolestr = accountService.GetSystemCustomerRoleStr();
            data.rolestr = rolestr;
            data.code = Masterpage.CurrUser.client_code;
            data.one = one;
            data.type = type;
            data.guid = guid;
            return View(data);
        }

        [UserCenter(Admin = true)]
        public ActionResult saverole()
        {
            string type = WebRequest.GetString("type", true);
            string rguid = WebRequest.GetString("guid", true);
            string name = WebRequest.GetString("name", true);
            string valid = WebRequest.GetString("valid", true);
            string remark = WebRequest.GetString("remark", true);

            ReturnValue r;
            bool _valid;
            Guid guid = Guid.Empty;
            #region 验证
            try
            {
                if (rguid != "")
                {
                    guid = Guid.Parse(rguid);
                }
                _valid = Convert.ToBoolean(valid);
                r = accountService.SaveCustomerRole(Masterpage.CurrUser.client_code, type, guid, name, _valid, remark);
                LogHelper.Info(Masterpage.CurrUser.alias, "902012:客户," + Masterpage.CurrUser.client_code + ",用户中心保存角色信息：操作类别：" + type + "，角色名称：" + name + "，是否启用：" + _valid + "，备注：" + remark + ",操作结果：" + r.status);
            }
            catch
            {
                r = new ReturnValue { status = "error", message = "参数有误" };
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [UserCenter(Admin = true)]
        public ActionResult delonerole(Guid guid)
        {
            var one = accountService.DeleteRole(Masterpage.CurrUser.client_code, guid);
            LogHelper.Info(Masterpage.CurrUser.alias, "902013:客户," + Masterpage.CurrUser.client_code + ",用户中心删除角色：guid：" + guid.ToString());
            return Json(one, JsonRequestBehavior.AllowGet);
        }

        [UserCenter(Admin = true)]
        public ActionResult rolepowers(string role)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            Guid guid;
            try
            {
                guid = Guid.Parse(role);
                var model = accountService.GetOneCustomerRole(Masterpage.CurrUser.client_code, guid);
                data.name = model.RoleName.Replace(Masterpage.CurrUser.client_code + "_", "");
                var allq = accountService.GetCustomerAdminModuleFunction(Masterpage.CurrUser.client_code);
                var hadq = accountService.GetModuleFunctionByRole(guid);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                LogHelper.Info(Masterpage.CurrUser.alias, "904011:客户," + Masterpage.CurrUser.client_code + ",用户中心角色权限，角色:" + role);
                r = new ReturnValue { status = "ok", message = "" };
            }
            catch
            {
                guid = Guid.Empty;
                r = new ReturnValue { status = "error", message = "读取失败" };
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
                if (addstr.EndsWith("#")) addstr = addstr.Substring(0, addstr.Length - 1);
                if (delstr.EndsWith("#")) delstr = delstr.Substring(0, delstr.Length - 1);
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

        [UserCenter(Admin = true)]
        public ActionResult roleuser(string role)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            Role one = new Role();
            string tree = "";
            Guid guid;
            try
            {
                guid = Guid.Parse(role);
                one = accountService.GetOneCustomerRole(Masterpage.CurrUser.client_code, guid);
                tree = accountService.GetCustomerTree(Masterpage.CurrUser.client_code);

                LogHelper.Info(Masterpage.CurrUser.alias, "904011:客户," + Masterpage.CurrUser.client_code + ",用户中心角色权限，角色:" + role);
                r = new ReturnValue { status = "ok", message = "" };
            }
            catch
            {
                guid = Guid.Empty;
                r = new ReturnValue { status = "error", message = "读取失败" };
            }
            data.code = Masterpage.CurrUser.client_code;
            data.one = one;
            data.tree = tree;
            data.r = r;
            data.guid = guid;
            return PartialView(data);

        }
        [UserCenter(Admin = true)]
        public ActionResult roleuserlist(Guid? role, string parent, string mycode)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = accountService.GetGroupUserRoleInfo(parent, mycode);
            data.list = list;
            data.code = mycode;
            data.parent = parent;
            data.role = role;
            return PartialView(data);
        }
        [UserCenter(Admin = true)]
        public ActionResult saveroleusers(Guid role, string customer)
        {
            ReturnValue rv = new ReturnValue();
            try
            {
                string addstr = WebRequest.GetString("add", true);
                string delstr = WebRequest.GetString("del", true);
                if (addstr.EndsWith("#")) addstr = addstr.Substring(0, addstr.Length - 1);
                if (delstr.EndsWith("#")) delstr = delstr.Substring(0, delstr.Length - 1);
                string[] add = addstr.Split('#');
                string[] del = delstr.Split('#');
                bool b = accountService.SaveRoleUsers(role, customer, add, del);
                if (b) rv = new ReturnValue { status = "ok", message = "" };
                else rv = new ReturnValue { status = "error", message = "保存失败" };
                LogHelper.Info(Masterpage.CurrUser.alias, "904012:客户," + Masterpage.CurrUser.client_code + ",用户中心保存角色权限，角色:" + role + "，权限添加：" + addstr + "，权限减少：" + delstr);

            }
            catch
            {
                rv = new ReturnValue { status = "error", message = "保存失败" };
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 资料申请
        [UserCenter]
        [LoginAllow]
        public ActionResult supplierlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = materialService.GetAllSupplier();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = ""; 
            data.isemployee = Masterpage.CurrUser.IsEmployee;
            LogHelper.Info(Masterpage.CurrUser.alias, "903021:客户," + Masterpage.CurrUser.client_code + ",用户中心供应商列表");
            return PartialView(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult materiallist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = materialService.GetAllMaterial();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = ""; 
            data.isemployee = Masterpage.CurrUser.IsEmployee;
            LogHelper.Info(Masterpage.CurrUser.alias, "903011:客户," + Masterpage.CurrUser.client_code + ",用户中心耗材列表");
            return PartialView(data);
        }
        [UserCenter]
        [LoginAllow]
        public ActionResult applydata(int? type,int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = centerService.GetBasicDataApplications(Masterpage.CurrUser.client_code, "");
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.type = type;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "";
            LogHelper.Info(Masterpage.CurrUser.alias, "903011:客户," + Masterpage.CurrUser.client_code + ",用户中心耗材申请列表");
            return PartialView(data);
        }
        [UserCenter]
        [LoginAllow]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult saveapply(string t, int? sn)
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
        public ActionResult delapply(int? sn)
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
                LogHelper.Info(Masterpage.CurrUser.alias, "903014:客户," + Masterpage.CurrUser.client_code + ",用户中心删除资料申请，sn:" + sn);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

    }
}
