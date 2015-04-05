using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;

using Entities = ecoBio.Wms.Data.Entities;
using ecoBio.Wms.Common;
using ecoBio.Wms.ViewModel;

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_LogController : AdminController
    {
        private ecoBio.Wms.Service.Management.LogService _logRepos = null;
        public Admin_LogController(ecoBio.Wms.Backstage.Repositories.Backstage.SysLog.ILogRepository logRepos)
           {
               _logRepos = new Service.Management.LogService(logRepos);
        }
        public ActionResult Log()
        {
            return View();
        }
        [AjaxAction(ForAction = "Log", ForController = "Admin_Log")]
        public ActionResult IndexLog(int? page, int? pagesize, string name,string level)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            if (level == null) level = "";
            var list = _logRepos.GetAllLog(name,level);

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
        /// 删除一个日志
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Log", ForController = "Admin_Log")]
        public ActionResult DeleteLog(int id)
        {
            try
            {
                _logRepos.DeleteService(id);
            }
            catch
            {

            }
            return RedirectToAction("Log");
        }
    }
}
