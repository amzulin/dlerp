using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;

using Entities = ecoBio.Wms.Data.Entities;
using ecoBio.Wms.Data.Entities.Models;

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_SysConfigurationController : AdminController//Controller //
    {
        private ecoBio.Wms.Service.Management.ModuleFunctionService _ModuleFunctionRepos = null;
        private ecoBio.Wms.Service.Management.DataDictionaryService _dataDictionaryRepos = null;
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="moduleFunctionRepos"></param>
        public Admin_SysConfigurationController(ecoBio.Wms.Backstage.Repositories.IModuleFunctionRepository moduleFunctionRepos,
            ecoBio.Wms.Backstage.Repositories.IDataDictionary dataDictionaryRepos)
        {
            _ModuleFunctionRepos = new Service.Management.ModuleFunctionService(moduleFunctionRepos);
            _dataDictionaryRepos = new Service.Management.DataDictionaryService(dataDictionaryRepos);
        }

        #region ================== ModuleFunction Beginning ==================

        public ActionResult ModuleFunction()
        {
            LogHelper.BackInfo("1-2", Masterpage.AdminCurrUser.userid, "访问模块功能");
            return View();
        }
        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        public ActionResult IndexModuleFunction(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _ModuleFunctionRepos.GetModuleById(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new ModuleFunction();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.ModuleFunctionId == first);
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
        /// 添加和更新一个模块功能
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        public ActionResult AddModuleFunction(Entities::Models.ModuleFunction instance, string id, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.ModuleFunction();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.ModuleFunction());
            }
            if (hidtype == "update")
            {
                return View(_ModuleFunctionRepos.GetModuleFunctionById(id));
            }
            return PartialView(data);
        }
        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        [HttpPost]
        public ActionResult saveModuleFunction(Entities::Models.ModuleFunction instance, string hidtype)
        {

            if (hidtype == "add")
            {
                _ModuleFunctionRepos.AddService(instance);
                LogHelper.BackInfo("1-2", Masterpage.AdminCurrUser.userid, "添加模块功能"+instance.ModuleFunctionId);
            }
            if (hidtype == "update")
            {
                _ModuleFunctionRepos.UpdateService(instance);
                LogHelper.BackInfo("1-2", Masterpage.AdminCurrUser.userid, "修改模块功能" + instance.ModuleFunctionId);

            }
            return RedirectToAction("ModuleFunction", new { first = instance.ModuleFunctionId });
        }


        /// <summary>
        /// 删除一个模块功能
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 

        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        public ActionResult DeleteModuleFunction(string id)
        {
            try
            {
                _ModuleFunctionRepos.DeleteService(id);
                LogHelper.BackInfo("1-2", Masterpage.AdminCurrUser.userid, "删除模块功能" + id);
            }
            catch
            { }

            return RedirectToAction("ModuleFunction");
        }

        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///       

        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        public ActionResult getGetModuleById2(string id)
        {
            var list = _ModuleFunctionRepos.GetModuleFunctionById(id);
            var c = new
            {
                a = list.ModuleFunctionId,
                b = list.ModuleFunctionId_parent,
                c = list.ModuleFunctionName,
                g = list.ModuleFunctionPermit,
                e = list.ModuleFunctionRoute,
                f = list.ModuleFunctionTips,
                h = list.ModuleFunctionType
            };
            return Json(c);
        }
        [AjaxAction(ForAction = "ModuleFunction", ForController = "Admin_SysConfiguration")]
        public ActionResult IsModuleFunctionExit(string id, string hidtype)
        {
            var one = _ModuleFunctionRepos.GetModuleFunctionById(id);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("ModuleFunction");
        }
        #endregion =================== ModuleFunction Ending ==================


        #region ================== DataDictionary Beginning ==================

        public ActionResult IndexData()
        {
            LogHelper.BackInfo("1-1", Masterpage.AdminCurrUser.userid, "访问数据字典"  );
            return View();
        }
        [AjaxAction(ForAction = "IndexData", ForController = "Admin_SysConfiguration")]
        public ActionResult IndexDataDictionary(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var cates = _dataDictionaryRepos.GetDataDictionaryAll();

            if (name == null) name = "";
            var list = _dataDictionaryRepos.GetDataDictionaryAll(name);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new DataDictionary();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.DataDictionaryKey == first);
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
        /// 添加一个数据字典
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "IndexData", ForController = "Admin_SysConfiguration")]
        public ActionResult AddDataDictionary(Entities::Models.DataDictionary instance, string id, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.DataDictionary();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return View(_dataDictionaryRepos.GetDataDictionaryById(id));
            }
            return PartialView(data);

        }

        [HttpPost]
        [AjaxAction(ForAction = "IndexData", ForController = "Admin_SysConfiguration")]
        public ActionResult saveDataDictionary(Entities::Models.DataDictionary instance, string hidtype)
        {


            if (hidtype == "add")
            {
                _dataDictionaryRepos.AddService(instance);
                LogHelper.BackInfo("1-1", Masterpage.AdminCurrUser.userid, "添加数据字典"+instance.DataDictionaryKey);
            }
            if (hidtype == "update")
            {
                _dataDictionaryRepos.UpdateService(instance);
                LogHelper.BackInfo("1-1", Masterpage.AdminCurrUser.userid, "修改数据字典" + instance.DataDictionaryKey);
            }
            return RedirectToAction("IndexData", new { first = instance.DataDictionaryKey });

        }


        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "IndexData", ForController = "Admin_SysConfiguration")]
        public ActionResult GetDataDictionaryKey1(string key)
        {

            var list = _dataDictionaryRepos.GetDataDictionaryById(key);

            var c = new
            {
                a = list.DataDictionaryKey,
                b = list.DataDictionaryValue,
                e = list.DataDictionaryRemark
            };
            return Json(c);
        }
        [AjaxAction(ForAction = "IndexData", ForController = "Admin_SysConfiguration")]
        public ActionResult IsDataDictionaryExit(string key, string hidtype)
        {
            var one = _dataDictionaryRepos.GetDataDictionaryById(key);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("IndexData");
        }
        #endregion =================== DataDictionary Ending ==================
    }
}
