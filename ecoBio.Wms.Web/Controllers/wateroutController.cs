using ecoBio.Wms.Common;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;

namespace ecoBio.Wms.Web.Controllers
{
    /// <summary>
    /// 出水管理
    /// </summary>
    public class wateroutController : BaseController
    {
        private ProcessService processService;
        public wateroutController(IProcessRepository _processRepository)
        {
            processService = new ProcessService(_processRepository);
        }


        public ActionResult systemout()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "204011:客户," + Masterpage.CurrUser.client_code + ",查看排放监管30日趋势图表");
            return View();
        }



        public ActionResult aerobic()
        {
            return View();
        }

        public ActionResult anaerobic()
        {
            return View();
        }


    }
}
