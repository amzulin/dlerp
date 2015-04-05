using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Entities = ecoBio.Wms.Data.Entities;

namespace ecoBio.Wms.Web.Controllers
{
    public class AdminWmsManagementController : AdminController//Controller //
    {
        private ecoBio.Wms.Service.Management.ModuleFunctionService _ModuleFunctionRepos = null;

        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="moduleFunctionRepos"></param>
        public AdminWmsManagementController(ecoBio.Wms.Backstage.Repositories.IModuleFunctionRepository moduleFunctionRepos)
        {
            _ModuleFunctionRepos = new Service.Management.ModuleFunctionService(moduleFunctionRepos);
        }

        public ActionResult Index()
        {
            dynamic data = new System.Dynamic.ExpandoObject();         
            var mslist = _ModuleFunctionRepos.GetModuleByRole(Masterpage.AdminCurrUser.role_guid); 
            var ms =  mslist.Where(f => f.ModuleFunctionType == "M" && f.ModuleFunctionId.StartsWith("B")).ToList();
            data.list1 = ms;
            data.person = Masterpage.AdminCurrUser.alias;
            data.list2 = mslist;
            return View(data);

        }

    }
}
