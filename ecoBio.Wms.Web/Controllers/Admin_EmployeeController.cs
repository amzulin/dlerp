using ecoBio.Wms.Backstage.Repositories;
using ecoBio.Wms.Service.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calabonga.Mvc.PagedListExt;
using System.Web.Mvc;
using ecoBio.Wms.ViewModel;
using ecoBio.Wms.Data.Entities.Models;
using ecoBio.Wms.Common;

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_EmployeeController : AdminController//Controller //
    {
        private RoleService _roleservice = null;
        private EmployeeService _employeeservice = null;
        private CustomerService _custoimerservice = null;
        private ecoBio.Wms.Service.Management.DynamicTokenService _tokenRepos = null;
        private ecoBio.Wms.Service.Management.DepartmentListService _departmentListRepos = null;
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="moduleFunctionRepos"></param>
        public Admin_EmployeeController(IEmployeeRepository employeeRepos, IRoleRepository rolerepos, ICustomerRepository customerrepos, ecoBio.Wms.Backstage.Repositories.IDynamicToken tokenRepos,
              ecoBio.Wms.Backstage.Repositories.IDepartmentListRepository departmentListRepos)
        {
            _roleservice = new RoleService(rolerepos);
            _employeeservice = new EmployeeService(employeeRepos);
            _custoimerservice = new CustomerService(customerrepos);
            _tokenRepos = new Service.Management.DynamicTokenService(tokenRepos);
            _departmentListRepos = new Service.Management.DepartmentListService(departmentListRepos);
        }

        public ActionResult Index()
        {
            return View();
        }
        [AjaxAction(ForAction = "Index", ForController = "Admin_Employee")]
        public ActionResult employeelist(int? page, int? pagesize, string name)
        {
            LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, "访问员工管理");
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _employeeservice.EmployeeList(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 20;
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
        [AjaxAction(ForAction = "Index", ForController = "Admin_Employee")]
        public ActionResult oneemployee(string guid, string type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            Employee one = new Employee();
            if (guid != null && guid != "")
            {
                Guid g = Guid.Parse(guid);
                one = _employeeservice.GetEmployeeByGuid(g);
            }
            var token = _tokenRepos.GetDynamicToken();
            data.token = token;
            var roles = _roleservice.GetEmployeeFrontRoles();
            data.roles = roles;
            var demps = _departmentListRepos.GetAllDepartmentList("").ToList();
            data.demps = demps;
            data.r = r;
            data.one = one;
            data.guid = guid;
            data.type = type;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "Index", ForController = "Admin_Employee")]
        public ActionResult delemployee()
        {
            Employee one = new Employee();
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r = new ReturnValue { status = "ok" };
            Guid g = Guid.Empty;
            g = Guid.Parse(guid);
            _employeeservice.DeleteEmployee(g);
            LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, "删除员工:" + g);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "Index", ForController = "Admin_Employee")]
        public ActionResult saveemployee(Employee instance, string code, string hidtype)
        {

            string guid = WebRequest.GetString("guid", true);
            string type = WebRequest.GetString("type", true);
            string login = WebRequest.GetString("login", true);
            string name = WebRequest.GetString("name", true);
            string token = WebRequest.GetString("token", true);
            string department = WebRequest.GetString("department", true);
            string mobile = WebRequest.GetString("mobile", true);
            string email = WebRequest.GetString("email", true).Replace("#", "@");
            string duty = WebRequest.GetString("duty", true);
            string role = WebRequest.GetString("role", true);
            string vaild = WebRequest.GetString("vaild", true);
            string expert = WebRequest.GetString("expert", true);
            string remark = WebRequest.GetString("remark", true);
            ReturnValue r = new ReturnValue();
            Guid g = Guid.Empty;
            Guid roleg = Guid.Empty;
            if (role != "") roleg = Guid.Parse(role);
            if (type == "edit") g = Guid.Parse(guid);
            r = _employeeservice.SaveEmpolyee(g, type, login, name, token,department, mobile, email, duty, roleg, Convert.ToBoolean(vaild), Convert.ToBoolean(expert), remark);
            if (type == "edit")
            {
                type = "修改";
            }
            if (type == "add")
            {
                type = "添加";
            }
            LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, type + "员工(" + "员工姓名:" + name + "," + "员工登录标识：" + login + ")");
            return Json(r, JsonRequestBehavior.AllowGet);
        }
    }
}
