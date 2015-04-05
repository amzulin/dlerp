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
    /// 工艺参数
    /// </summary>
    public class processparaController : BaseController
    {
        private DataCenterService centerService;
        public processparaController(IDataCenterRepository _centerRepository)
        {
            centerService = new DataCenterService(_centerRepository);
        }



        public ActionResult overall(string from)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "202011:客户," + Masterpage.CurrUser.client_code + ",查看工艺参数全局参数30日趋势图表");
            dynamic data = new System.Dynamic.ExpandoObject();
            string select = "";
            #region 从工作台链接
            if (from != null && from == "platform")
            {
                var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
                select = chartconfig.chart2_1;
                if (chartconfig.chart2_2 != "") select += (select != "" ? ("," + chartconfig.chart2_2) : chartconfig.chart2_2);
            }
            #endregion
            data.select = select;
            return View(data);
        }


        public ActionResult aerobic()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "202021:客户," + Masterpage.CurrUser.client_code + ",查看工艺参数好氧参数30日趋势图表");
            return View();
        }

        public ActionResult anaerobic()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "202031:客户," + Masterpage.CurrUser.client_code + ",查看工艺参数厌氧参数30日趋势图表");
            return View();
        }
        public ActionResult efficiency()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "202041:客户," + Masterpage.CurrUser.client_code + ",查看工艺参数生物增效参数30日趋势图表");
            return View();
        }
    }
}
