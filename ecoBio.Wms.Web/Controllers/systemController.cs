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
using Enterprise.Invoicing.Entities.Models;
namespace Enterprise.Invoicing.Web.Controllers
{
    public class systemController : Controller
    {
        private ManageService manageService;
        private SystemService systemService;
        public systemController(IManageRepository _manageRepository, ISystemRepository _systemrepository)
        {
            manageService = new ManageService(_manageRepository);
            systemService = new SystemService(_systemrepository);
        }

        #region 用户管理

        public ActionResult user(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = systemService.GetEmployeeList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        [AjaxAction(ForAction = "user", ForController = "system")]
        public ActionResult userone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            EmployeeModel model = new EmployeeModel();
            if (id > 0 && type == "edit")
            {
                model = systemService.GetEmployeeList("").FirstOrDefault(p => p.staffId == id);
            }
            else type = "add";
            var roles = systemService.GetRoleList("").
                Select(p => new SelectListItem { Text = p.roleName, Value = p.roleSn.ToString() }).ToList();
            data.roles = roles;
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "user", ForController = "system")]
        public ActionResult saveuser()
        {
            //id,depId, string name, string mobile, string email, string duty, string remark
            int id = WebRequest.GetInt("id", 0);
            int rad = WebRequest.GetInt("rad", 0);
            int isuser = WebRequest.GetInt("isuser", 0);
            int utype = WebRequest.GetInt("utype", 0);
            int role = WebRequest.GetInt("role", 0);
            string userid = WebRequest.GetString("userid");
            string pwd = WebRequest.GetString("pwd");
            string remark = WebRequest.GetString("remark");
            bool valid = rad != 0 ? true : false;
            bool foruser = isuser != 0 ? true : false;
            if (pwd != "") pwd = MD5Helper.MD5_32(pwd).ToLower();
            var r = systemService.SetUser(id, foruser, userid, pwd, role, valid, remark, utype);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 角色管理

        public ActionResult role(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = systemService.GetRoleList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult roleone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            Role model = new Role();
            if (id > 0 && type == "edit")
            {
                model = systemService.GetRoleList("").FirstOrDefault(p => p.roleSn == id);
            }
            else type = "add";
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult saverole()
        {
            //int id, string depName, string phone, string leader, string remark
            int id = WebRequest.GetInt("id", 0);
            string name = WebRequest.GetString("name");
            string remark = WebRequest.GetString("remark");
            int price = WebRequest.GetInt("price", 0);
            var r = systemService.SaveRole(id, name, remark, price == 1);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult deleterole()
        {
            int id = WebRequest.GetInt("id", 0);
            var r = systemService.DeleteRole(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult rolemenu()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            var model = systemService.GetRoleList("").FirstOrDefault(p => p.roleSn == id);
            var all = systemService.GetAllMenu();
            var had = systemService.GetHadMenu(id);
            data.id = id;
            data.had = had;
            data.all = all;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult saverolemenu(int id)
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
                bool b = systemService.SaveRoleMenu(id, add, del);
                if (b) rv = new ReturnValue { status = true, message = "" };
                else rv = new ReturnValue { status = false, message = "保存失败" };

            }
            catch (Exception ex)
            {
                rv = new ReturnValue { status = false, message = "程序异常:" + ex.Message };
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult rolefunction()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            var model = systemService.GetRoleList("").FirstOrDefault(p => p.roleSn == id);
            var all = systemService.GetAllFunction();
            var had = systemService.GetHadFunction(id);
            data.id = id;
            data.had = had;
            data.all = all;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "role", ForController = "system")]
        public ActionResult saverolefunction(int id)
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
                bool b = systemService.SaveRoleFunction(id, add, del);
                if (b) rv = new ReturnValue { status = true, message = "" };
                else rv = new ReturnValue { status = false, message = "保存失败" };

            }
            catch (Exception ex)
            {
                rv = new ReturnValue { status = false, message = "程序异常:" + ex.Message };
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 字典管理

        public ActionResult dictionary(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = systemService.GetDictionaryList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        [AjaxAction(ForAction = "dictionary", ForController = "system")]
        public ActionResult dictionaryone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string id = WebRequest.GetString("id");
            string type = WebRequest.GetString("type");
            Dictionary model = new Dictionary();
            if (id != "" && type == "edit")
            {
                model = systemService.GetDictionaryList("").FirstOrDefault(p => p.dictionaryKey == id);
            }
            else type = "add";
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "dictionary", ForController = "system")]
        public ActionResult savedictionary()
        {
            //id,depId, string name, string mobile, string email, string duty, string remark
            string type = WebRequest.GetString("type");
            string no = WebRequest.GetString("no");
            string name = WebRequest.GetString("name");
            string value = WebRequest.GetString("value");
            string remark = WebRequest.GetString("remark");
            var r = systemService.SaveDictionary(type, no, value, name, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "dictionary", ForController = "system")]
        public ActionResult deletedictionary()
        {
            string id = WebRequest.GetString("id");
            var r = systemService.DeleteDictionary(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
