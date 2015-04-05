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
    /// 水质管理
    /// </summary>
    public class waterqualityController : BaseController
    {
           private DataCenterService centerService;
           public waterqualityController(IDataCenterRepository _centerRepository)
           {
               centerService = new DataCenterService(_centerRepository);
           }



           public ActionResult overall(string from)
           {
               LogHelper.Info(Masterpage.CurrUser.alias, "203011:客户," + Masterpage.CurrUser.client_code + ",查看水质管理全局水质30日趋势图表");
               dynamic data = new System.Dynamic.ExpandoObject();
               string select = "";
               #region 从工作台链接
               if (from != null && from == "platform")
               {
                   var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
                   select = chartconfig.chart3_1;
                   if (chartconfig.chart3_2 != "") select += (select != "" ? ("," + chartconfig.chart3_2) : chartconfig.chart3_2);
               }
               #endregion
               data.select = select;
               return View(data);
           }


        public ActionResult aerobic()
           {
               LogHelper.Info(Masterpage.CurrUser.alias, "203021:客户," + Masterpage.CurrUser.client_code + ",查看水质管理好氧水质30日趋势图表");
            return View();
        }

        public ActionResult anaerobic()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "203031:客户," + Masterpage.CurrUser.client_code + ",查看水质管理厌氧水质30日趋势图表");
            return View();
        }
        public ActionResult efficiency()
        {
            return View();
        }



    }
}
