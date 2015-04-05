using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecoBio.Wms.Service.Monitor;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Common;
using ecoBio.Wms.Data.Entities.Models;

namespace ecoBio.Wms.Web.Controllers
{
    public class ucenterrightController : BaseController
    {
        private UserCenterService ucenterService;

        public ucenterrightController(IUserCenterRepository _ucenterRepository)
        {
            ucenterService = new UserCenterService(_ucenterRepository);
        }

        [UserCenter]
        public ActionResult rolelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = ucenterService.GetRoleListByCustomerCode(Masterpage.CurrUser.client_code);
            data.list = list;
            data.message = SessionHelper.GetSession("rolelistmessage");
            SessionHelper.Del("rolelistmessage");
            return View(data);
        }

        [UserCenter]
        [AjaxAction(ForAction = "rolelist", ForController = "ucenterright")]
        public ActionResult modulefuntione(string guid)
        {
            var q_guid = WebRequest.GetQueryString("guid");
            Guid g = new Guid(q_guid);
            var myrole = ucenterService.GetRole(Masterpage.CurrUser.role_guid);
            var forrole = ucenterService.GetRole(g);
            List<ModuleFunction> allmfs = new List<ModuleFunction>();
            List<string> hadmoduelid = new List<string>();
            var rights = forrole.Rights;

            if (rights!=null)
            {
                foreach (var item in rights)
                {

                    hadmoduelid.Add(item.ModuleFunctionId);
                }
            }
            dynamic data = new System.Dynamic.ExpandoObject();
            data.hadmoduelid = hadmoduelid;
            data.allmodules = ucenterService.GetMyModuleFunction(myrole);
            data.guid = guid;
            return View(data);
        }

        [UserCenter]
        [AjaxAction(ForAction = "rolelist", ForController = "ucenterright")]
        public ActionResult changeright(string guid, string data)
        {
            if (data.EndsWith(",")) data = data.Substring(0, data.Length - 1);
            if (data.StartsWith(",")) data = data.Substring(1);
            Guid g = new Guid(guid);
            string[] modules = data.Split(',');
            ucenterService.UpdateRoleRight(Masterpage.CurrUser.role_guid, g, modules);
            return Content("修改成功");
        }
    }
}
