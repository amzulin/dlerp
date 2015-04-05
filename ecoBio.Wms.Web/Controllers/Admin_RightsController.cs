using ecoBio.Wms.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Entities = ecoBio.Wms.Data.Entities;
using ecoBio.Wms.ViewModel;
using System.Configuration;
using System.IO;
 

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_RightsController : AdminController//Controller //
    {


        private ecoBio.Wms.Service.Management.RoleService _roleRepos = null;
        private ecoBio.Wms.Service.Management.DynamicTokenService _tokenRepos = null;
        private ecoBio.Wms.Service.Management.UserService _userRepos = null;
        private ecoBio.Wms.Service.Management.EmployeeService _employeeservice = null;         
        private ecoBio.Wms.Service.Management.CustomerService _customerRepos = null;
        private ecoBio.Wms.Service.Management.ModuleFunctionService _moduleFunctionRepos = null;
        private ecoBio.Wms.Service.Management.DepartmentListService _departmentListRepos = null;
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="reRepos"></param>
        public Admin_RightsController(ecoBio.Wms.Backstage.Repositories.IRoleRepository roleRepos,
            ecoBio.Wms.Backstage.Repositories.IDynamicToken tokenRepos,
            ecoBio.Wms.Backstage.Repositories.IUserRepository userRepos,           
            ecoBio.Wms.Backstage.Repositories.IModuleFunctionRepository moduleFunctionRepos, ecoBio.Wms.Backstage.Repositories.IEmployeeRepository employeeRepos,
            ecoBio.Wms.Backstage.Repositories.ICustomerRepository customerRepos,
              ecoBio.Wms.Backstage.Repositories.IDepartmentListRepository departmentListRepos
            )
        {
            _tokenRepos = new Service.Management.DynamicTokenService(tokenRepos);
            _roleRepos = new Service.Management.RoleService(roleRepos);
            _userRepos = new Service.Management.UserService(userRepos);           
            _moduleFunctionRepos = new Service.Management.ModuleFunctionService(moduleFunctionRepos);
            _customerRepos = new Service.Management.CustomerService(customerRepos);
            _employeeservice = new Service.Management.EmployeeService(employeeRepos);
            _departmentListRepos = new Service.Management.DepartmentListService(departmentListRepos);
        }

        #region ================== Token Beginning ==================

        public ActionResult DynamicToken()
        {
            LogHelper.BackInfo("007011", Masterpage.AdminCurrUser.userid, "访问令牌");
            return View();
        }
        [AjaxAction(ForAction = "DynamicToken", ForController = "Admin_Rights")]
        public ActionResult IndexToken(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _tokenRepos.GetTokenById(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new DynamicToken();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.TokenCode == first);
                var firspage = vs.IndexOf(firstone);
                if (firspage == -1)
                {
                    vs.Insert(0, firstone);
                }
                else if (firspage > 0)
                {
                    vs.Remove(firstone);
                    vs.Insert(0, firstone);
                }
            }
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加一个令牌
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 

        [AjaxAction(ForAction = "DynamicToken", ForController = "Admin_Rights")]
        public ActionResult AddToken(Entities::Models.DynamicToken instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.DynamicToken();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_tokenRepos.GetDynamicTokenCode(code));
            }
            
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "DynamicToken", ForController = "Admin_Rights")]
        public ActionResult saveToken(Entities::Models.DynamicToken instance, string hidtype)
        {
            
            try
            {
                if (hidtype == "add")
                {
                    _tokenRepos.AddService(instance);                   
                    LogHelper.BackInfo("007012", Masterpage.AdminCurrUser.userid, "添加令牌:" + "[令牌号："+instance.TokenCode+"]"+"[种子码："+instance.TokenSeed+"]"+"[备注："+instance.TokenRemark+"]"+"[令牌许可："+instance.TokenPermit+"]");
                }
                if (hidtype == "update")
                {
                    _tokenRepos.UpdateService(instance);
                    LogHelper.BackInfo("007013", Masterpage.AdminCurrUser.userid, "修改令牌:" + "[令牌号：" + instance.TokenCode + "]" + "[种子码：" + instance.TokenSeed + "]" + "[备注：" + instance.TokenRemark + "]" + "[令牌许可：" + instance.TokenPermit + "]");
                }
                
            }
            catch
            {

            }
            return RedirectToAction("DynamicToken", new { first = instance.TokenCode });

        }

        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "DynamicToken", ForController = "Admin_Rights")]
        public ActionResult GetDynamicTokenCode(string code)
        {

            var list = _tokenRepos.GetTokenByCode(code) ;
            var c = new
            {
                a = list.TokenCode,
                b = list.TokenSeed,
                e = list.CurSucc,
                f = list.CurDft,
                g = list.TokenPermit,
                h = list.TokenRemark
            };
            return Json(c);
        }
     [AjaxAction(ForAction = "DynamicToken", ForController = "Admin_Rights")]
        public ActionResult IsDynamicTokenExit(string code, string hidtype)
        {
            var one = _tokenRepos.GetTokenByCode(code);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("DynamicToken");
        }
        #endregion =================== Token Ending ==================

        #region ================== User Beginning ==================
        public ActionResult User()
     {
         LogHelper.BackInfo("004021", Masterpage.AdminCurrUser.userid, "访问用户");
            return View();

        }
        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult IndexUser(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _userRepos.GetAllUser(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new User();
            if (first != null && first != "")
            {
                Guid g = Guid.Parse(first);
                firstone = list.FirstOrDefault(p => p.UserGuid == g);
                var firspage = vs.IndexOf(firstone);
                if (firspage == -1)
                {
                    vs.Insert(0, firstone);
                }
                else if (firspage > 0)
                {
                    vs.Remove(firstone);
                    vs.Insert(0, firstone);
                }
            }
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>

        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult AddUser(string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var token = _tokenRepos.GetDynamicToken();
            data.token = token;
            var role = _roleRepos.GetRole();
            data.role = role;
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            data.one = new Entities::Models.User();
            if (hidtype == "add")
            {
                return View(new User());
            }
            if (hidtype == "update")
            {

                Guid guid = new Guid();
                return View(_userRepos.GetUserByGuid(guid));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult SaveUser(Entities::Models.User instance, string hidtype)
        {
          
            try
            {
                if (hidtype == "add")
                {
                    instance.UserGuid = Guid.NewGuid();
                    instance.UserCreateDate = DateTime.Now;
                    _userRepos.AddService(instance);
                    LogHelper.BackInfo("004022", Masterpage.AdminCurrUser.userid, "添加用户:" + "[登录标识：" + instance.UserLoginId + "[用户名：" + instance.UserChineseName + "[客户号：" + instance.CustomerCode + "[令牌号：" + instance.TokenCode + "[角色：" + instance.RoleGuid + "[用户编码：" + instance.UserGuid + "]" + "[用户手机：" + instance.UserMobile + "[用户邮箱：" + instance.UserEmail + "[用户备注：" + instance.UserRemark);

                }
                if (hidtype == "update")
                {
                    _userRepos.UpdateService(instance);
                    LogHelper.BackInfo("004023", Masterpage.AdminCurrUser.userid, "修改用户:" + "[登录标识：" + instance.UserLoginId + "[用户名：" + instance.UserChineseName + "[客户号：" + instance.CustomerCode + "[令牌号：" + instance.TokenCode + "[角色：" + instance.RoleGuid + "[用户编码：" + instance.UserGuid + "]" + "[用户手机：" + instance.UserMobile + "[用户邮箱：" + instance.UserEmail + "[用户备注：" + instance.UserRemark);
                }
                #region 增加用户客户关系
                var g = instance.RoleGuid;
                if (g == null) g = Guid.Empty;
                if (instance.RoleGuid.HasValue)
                    _userRepos.SaveUserOwerCustomer(instance.UserGuid, instance.CustomerCode, instance.RoleGuid.Value);
                else
                    _userRepos.SaveUserOwerCustomer(instance.UserGuid, instance.CustomerCode, Guid.Empty);
                #endregion
            }
            catch
            {

            }
            //data.one = instance;
            return RedirectToAction("User", new { first = instance.UserGuid });
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult DeleteUser(Guid guid)
        {
            try
            {
                _userRepos.DeleteService(guid);
                LogHelper.BackInfo("004024", Masterpage.AdminCurrUser.userid, "删除用户:" + guid);
            }
            catch
            {

            }

            return RedirectToAction("User");

        }


        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult GetUserByGuid1(Guid guid)
        {   
            var list = _userRepos.GetUserByGuid(guid);
            var token = _tokenRepos.GetDynamicToken();
            var role = _roleRepos.GetRole();
            var mytoken = token.Where(p => p.TokenRemark.StartsWith(list.CustomerCode)).Select(p => new { tokencode = p.TokenCode });
            var myrole = role.Where(p => p.RoleName.StartsWith(list.CustomerCode)).Select(p => new { rolename = p.RoleName, roleguid = p.RoleGuid.ToString() });

            var user = new
            {
                UserGuid = list.UserGuid,
                TokenCode = list.TokenCode,
                CustomerCode = list.CustomerCode,
                RoleGuid = list.RoleGuid,
                UserLoginId = list.UserLoginId,
                UserChineseName = list.UserChineseName,
                UserMobile = list.UserMobile,
                UserEmail = list.UserEmail,
                UserCreateDate = list.UserCreateDate.ToString("yyyy-MM-dd"),
                UserValidPeriod = list.UserValidPeriod,
                a = list.UserAccountPermit,
                UserConf1 = list.UserConf1,
                UserConf2 = list.UserConf2,
                UserConf3 = list.UserConf3,
                UserConf4 = list.UserConf4,
                UserConf5 = list.UserConf5,
                b = list.UserIsAdmin,
                c = list.UserIsOnline,
                UserSession = list.UserSession,
                UserRemark = list.UserRemark,
                tokens = mytoken,
                roles = myrole
            };
            return Json(user);
        }
        [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult IsLoginExit(string loginid, string type)
        {
            var one = _userRepos.GetUserByLoginid(loginid);
            if (type == "add")
                return one == null ? Content("0") : Content("1");
            else return Content("0");
        }
         [AjaxAction(ForAction = "User", ForController = "Admin_Rights")]
        public ActionResult ChangeCustomer(string code)
        {
            var token = _tokenRepos.GetDynamicToken();
            var role = _roleRepos.GetRole();
            var mytoken = token.Where(p => p.TokenRemark.StartsWith(code)).Select(p => new { tokencode = p.TokenCode });
            var myrole = role.Where(p => p.RoleName.StartsWith(code)).Select(p => new { rolename = p.RoleName, roleguid = p.RoleGuid.ToString() });

            return Json(new { tokens = mytoken, roles = myrole }, JsonRequestBehavior.AllowGet);
        }
        #endregion =================== User Ending ==================

        #region ================== Role Beginning ==================
        public ActionResult Role()
         {
             LogHelper.BackInfo("004011", Masterpage.AdminCurrUser.userid, "访问角色");
            return View();
        }
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult IndexRole(int? page, int? pagesize, string name)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _roleRepos.GetAllRole(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            data.otherParam = otherparam;
            return PartialView(data);


        }
        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult AddRole(string hidtype)
        {
           
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();

            var userrols = _roleRepos.GetDictionaryRoleList("USER_ROLE_CATEGORY");
            List<SelectListItem> roles1 = userrols.Select(p => new SelectListItem { Text = p.text, Value = p.value }).ToList();

            var ecoroles = _roleRepos.GetDictionaryRoleList("ECOBIO_EMPLOYEE_ROLE_CATEGORY");
            List<SelectListItem> roles2 = ecoroles.Select(p => new SelectListItem { Text = p.text, Value = p.value }).ToList();

            var adminroles = _roleRepos.GetDictionaryRoleList("ADMIN_ROLE_CATEGORY");
            List<SelectListItem> roles3 = adminroles.Select(p => new SelectListItem { Text = p.text, Value = p.value }).ToList();
            data.customer = customer;
            data.roles1 = roles1; 
            data.roles2 = roles2;
            data.roles3 = roles3;
            data.one = new Entities::Models.Role();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.Role());
            }
            if (hidtype == "update")
            {
                // Guid guid=Guid.NewGuid();
                Guid guid = new Guid();
                return View(_roleRepos.GetRoleByGuid1(guid));
            }
            return PartialView(data);

        }

        [HttpPost]
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult AddRole(Entities::Models.Role instance, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            try
            {
                if (hidtype == "add")
                {
                    instance.RoleGuid = Guid.NewGuid();

                    _roleRepos.AddService(instance);
                    LogHelper.BackInfo("004012", Masterpage.AdminCurrUser.userid, "添加角色:" + instance.RoleGuid);
                }
                if (hidtype == "update")
                {
                    _roleRepos.UpdateService(instance);
                    LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, "修改角色:"+instance.RoleGuid);
                }
            }
            catch
            {

            }
            data.one = instance;
            return PartialView(data);
        }

        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult DeleteRole(Guid guid)
        {
            try
            {
                _roleRepos.DeleteService(guid);
                LogHelper.BackInfo("004014", Masterpage.AdminCurrUser.userid, "删除角色:" + guid);
            }
            catch
            {

            }

            return RedirectToAction("Role");

        }

        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult GetRoleByGuid1(Guid guid)
        {

            var list = _roleRepos.GetRoleByGuid(guid);
            var c = new
            {
                a = list.RoleGuid,
                b = list.RoleName,
                e = list.RolePermit,
                f = list.RoleRemark
            };
            return Json(c);
        } 
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult UpdateRolePerimt(bool type,Guid guid)
        {

            bool r = _roleRepos.UpdateRolePerimt(type, guid);
            LogHelper.BackInfo("004013", Masterpage.AdminCurrUser.userid, "修改角色" + guid + "启用状态为：" + type);
            if (r) return Json(new { status = "ok" });
            else return Json(new { status = "error" });
        }



        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult SetBackModuleFunction(Guid guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int r = 0;
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            try
            {
                LogHelper.BackInfo("004011", Masterpage.AdminCurrUser.userid, "查看角色" + guid + "后台权限列表");
                var allq = _moduleFunctionRepos.GetBackModuleByAll();
                var hadq = _moduleFunctionRepos.GetModuleByRole(guid);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                r = 1;
            }
            catch
            {
                guid = Guid.Empty;
                r = 0;
            }
            data.all = all;
            data.had = had;
            data.r = r;
            data.guid = guid;
            return View(data);
        }
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult SetModuleFunction(Guid guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int r = 0;
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            try
            {
                LogHelper.BackInfo("004011", Masterpage.AdminCurrUser.userid, "查看角色" + guid + "前台权限列表");
                var allq = _moduleFunctionRepos.GetModuleByAll();
                var hadq = _moduleFunctionRepos.GetModuleByRole(guid);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                r = 1;
            }
            catch
            {
                guid = Guid.Empty;
                r = 0;
            }
            data.all = all;
            data.had = had;
            data.r = r;
            data.guid = guid;
            return View(data);
        }

        [AjaxAction(ForAction = "CustomerPower", ForController = "Admin_Customer")]
        public ActionResult SetCustomerFunction(string customer)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var roles = _roleRepos.GetRolesByCustomer(customer);
            var admin = roles.FirstOrDefault(p => p.text.StartsWith(customer + "_ADMIN"));
            Guid guid;
            int r = 0;
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            try
            {
                guid = admin.guid;
                LogHelper.BackInfo("004011", Masterpage.AdminCurrUser.userid, "查看角色" + guid + "前台权限列表");
                var allq = _moduleFunctionRepos.GetModuleByAll();
                var hadq = _moduleFunctionRepos.GetModuleByRole(guid);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                r = 1;
            }
            catch
            {
                guid = Guid.Empty;
                r = 0;
            }
            data.all = all;
            data.had = had;
            data.r = r;
            data.guid = guid;
            return View(data);
        }



        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult saverolepowers(Guid guid, string add, string del)
        {
            try
            {
                LogHelper.BackInfo("004013", Masterpage.AdminCurrUser.userid, "修改角色" + guid + "后台权限，添加" + add + "权限，移除" + del + "权限");
                if (add.EndsWith("#")) add = add.Substring(0, add.Length - 1);
                if (del.EndsWith("#")) del = del.Substring(0, del.Length - 1);
                string[] addl = add.Split('#');
                string[] dell = del.Split('#');
                bool b = _moduleFunctionRepos.SaveRoleRigths(guid, addl, dell);
                if (b) return Json(new { status = "ok" });
                else return Json(new { status = "error" });
            }
            catch
            {
                return Json(new { status = "error" });
            }
        }
        
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult GetRolesByCustomer(string code)
        {
            var list = _roleRepos.GetRolesByCustomer(code);
            return Json(list);
        }

        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult SaveCustomerRole(string code, string add, string del, bool vaild, string remark)
        {
            //code: code, add: add, del: del, vaild: vaild,remark:remark 
            try
            {
                LogHelper.BackInfo("004013", Masterpage.AdminCurrUser.userid, "设置客户" + code + "的角色，添加" + add + "角色，移除" + del + "角色");
                if (add.EndsWith("#")) add = add.Substring(0, add.Length - 1);
                if (del.EndsWith("#")) del = del.Substring(0, del.Length - 1);
                string[] addl = add.Split('#');
                string[] dell = del.Split('#');
                bool b = _roleRepos.SaveCustomerRole(code, addl, dell, vaild, remark);
                if (b) return Json(new { status = "ok" });
                else return Json(new { status = "error" });
            }
            catch
            {
                return Json(new { status = "error" });
            }
        }
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult SaveCategoryPower(Guid role)
        {
            LogHelper.BackInfo("004015", Masterpage.AdminCurrUser.userid, "设置角色" + role + "的权限与客户分类权限一至");
            bool b = _roleRepos.SaveCategoryPower(role);
            if (b) return Json(new { status = "ok" });
            else return Json(new { status = "error" });
        } 
        #endregion =================== Role Ending ==================

        #region 为员工用户分配客户
        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult SetCustomer(Guid guid, string type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int r = 0;
            List<Customer> all = new List<Customer>();
            List<OwnCustomer> had = new List<OwnCustomer>();
            List<Role> allrole = new List<Role>();
            ecoBio.Wms.Data.Entities.Models.User user = new User();
            try
            {
                if (type == "user") user = _userRepos.GetUserByGuid(guid);
                var allq = _customerRepos.GetAllCustomers();
                var hadq = _customerRepos.GetOwnCustomer(type, guid);
                var allr = _roleRepos.GetAllRole("");
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                if (allr != null) allrole = allr.ToList();
                r = 1;
            }
            catch
            {
                guid = Guid.Empty;
                r = 0;
            }
            data.user = user;
            data.all = all;
            data.allrole = allrole;
            data.type = type;
            data.had = had;
            data.r = r;
            data.guid = guid;
            return View(data);
        }

        [AjaxAction(ForAction = "Role", ForController = "Admin_Rights")]
        public ActionResult saveusercustomer(Guid guid, string type, string add, string del, string addr)
        {
            try
            {
                if (add.EndsWith("#")) add = add.Substring(0, add.Length - 1);
                if (del.EndsWith("#")) del = del.Substring(0, del.Length - 1);
                if (addr.EndsWith("#")) addr = addr.Substring(0, addr.Length - 1);
                string[] addl = add.Split('#');
                string[] dell = del.Split('#');
                string[] addrl = addr.Split('#');
                bool b = _customerRepos.SaveHaveCustomer(type, guid, addl, dell, addrl);
                if (b) return Json(new { status = "ok" });
                else return Json(new { status = "error" });
            }
            catch
            {
                return Json(new { status = "error" });
            }
        }
        #endregion

        #region 员工分配后台角色

        public ActionResult EmployeeRole(int? page, int? pagesize, string name)
        {
            LogHelper.BackInfo("007011", Masterpage.AdminCurrUser.userid, "访问员工后台角色配置页面");
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var roles = _roleRepos.GetEmployeeAdminRoles();
            var list = _employeeservice.EmployeeList(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 20;
            var vs = list.ToPagedList(_page, _pagesize);
            data.name = name;
            data.roles = roles;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            data.otherParam = otherparam;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "EmployeeRole", ForController = "Admin_Rights")]
        public ActionResult SaveEmployeeRole(Guid employee, Guid? role)
        {
            LogHelper.BackInfo("007013", Masterpage.AdminCurrUser.userid, "修改员工" + employee + "的角色为" + role);
            if (!role.HasValue) role = Guid.Empty;
            bool r = _roleRepos.SaveEmployeeRole(employee, role.Value);
            var c = new { status = r ? "ok" : "error" };
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ================== DepartmentList Beginning ===================

        public ActionResult DepartmentList()
        {
            LogHelper.BackInfo("004011", Masterpage.AdminCurrUser.userid, "访问部门");
            return View();
        }
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult IndexDepartmentList(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _departmentListRepos.GetAllDepartmentList(name).ToList();
            data.list = list;

            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个部门
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult AddDepartmentList(Entities::Models.DepartmentList instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var emp = _employeeservice.GetEmployee();
            data.emp = emp;
            data.one = new Entities::Models.DepartmentList();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_departmentListRepos.GetDepartmentListByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult saveDepartmentList(Entities::Models.DepartmentList instance, string hidtype)
        {

            try
            {
                if (hidtype == "add")
                {
                    _departmentListRepos.AddService(instance);
                    LogHelper.BackInfo("004012", Masterpage.AdminCurrUser.userid, "添加部门:" + "[部门编号：" + instance.DepartmentCode + "]" + "[部门父级:" + instance.DepartmentCode_parent + "]" + "[部门名称:" + instance.DepartmentName + "]" + "[备注:" + instance.DepartmentRemark + "]" + "[员工:" + instance.EmployeeGuid + "]");
                }
                if (hidtype == "update")
                {
                    _departmentListRepos.UpdateService(instance);
                    LogHelper.BackInfo("004013", Masterpage.AdminCurrUser.userid, "修改部门:" + "[部门编号：" + instance.DepartmentCode + "]" + "[部门父级:" + instance.DepartmentCode_parent + "]" + "[部门名称:" + instance.DepartmentName + "]" + "[备注:" + instance.DepartmentRemark + "]" + "[员工:" + instance.EmployeeGuid + "]");
                }
            }
            catch
            {

            }
            return RedirectToAction("DepartmentList");
        }
        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult DeleteDepartmentList(string code)
        {
            
            try
            {
                _departmentListRepos.DeleteService(code);
                LogHelper.BackInfo("004014", Masterpage.AdminCurrUser.userid, "删除部门:" + "[部门编号：" + code + "]");
            }
            catch
            { }
            return RedirectToAction("DepartmentList");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult GetDepartmentListCode(string code)
        {

            var list = _departmentListRepos.GetDepartmentListByCode(code);
            var c = new
            {
                a = list.DepartmentCode,
                b=list.EmployeeGuid,
                e = list.DepartmentCode_parent,
                f = list.DepartmentName,
                g = list.DepartmentRemark
            };
            return Json(c);
        }
        [AjaxAction(ForAction = "DepartmentList", ForController = "Admin_Rights")]
        public ActionResult IsDepartmentListExit(string code, string hidtype)
        {
            var one = _departmentListRepos.GetDepartmentListByCode(code);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("DepartmentList");
        }
        #endregion =================== DepartmentList Ending ==================


        #region 数据导入
        [AjaxAction(ForAction = "DataImport", ForController = "Admin_Rights")]
        public ActionResult uploadtoshare()
        {
            var imgpath = ConfigurationManager.AppSettings["ServiceShare"];
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".csv" && exn.ToLower() != ".txt")
                {
                    r = new ReturnValue { status = "error", message = "请上传.csv或.txt" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                var uploadsPath = Path.Combine(imgpath, fn);
                httpfile.SaveAs(uploadsPath);
                LogHelper.BackInfo("6-3", Masterpage.AdminCurrUser.userid, "上传导入文件到目录" + uploadsPath);
                r = new ReturnValue { status = "ok", value = uploadsPath };
            }
            else
            {
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataImport()
        {
            LogHelper.BackInfo("6-1", Masterpage.AdminCurrUser.userid, "访问数据导入页面");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            return View(data);
        }
        [AjaxAction(ForAction = "DataImport", ForController = "Admin_Rights")]
        public ActionResult StartDataImport(string customer, string path, int clear, int type)
        {
            var r = _customerRepos.StartDataImport(customer, path, clear,type);
            LogHelper.BackInfo("6-1", Masterpage.AdminCurrUser.userid, "导入" + customer + "客户采集点，文件路径为" + path + "，是否清除原有数据：" + clear);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
