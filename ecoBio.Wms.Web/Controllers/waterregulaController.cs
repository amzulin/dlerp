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
    public class waterregulaController : BaseController
    {
        private DataCenterService centerService;
        public waterregulaController(IDataCenterRepository _centerRepository)
           {
               centerService = new DataCenterService(_centerRepository);
           }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult regulapool(string from)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "201011:客户," + Masterpage.CurrUser.client_code + ",查看进水监管系统进水30日趋势图表");
            dynamic data = new System.Dynamic.ExpandoObject();
            string select = "";
            #region 从工作台链接
            if (from != null && from == "platform")
            {
                var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
                select = chartconfig.chart1_1;
                if (chartconfig.chart1_2 != "") select = select != "" ? (select + "," + chartconfig.chart1_2) : chartconfig.chart1_2;
            }
            #endregion
            data.select = select;
            return View(data);
        }




        public ActionResult aerobic(string from)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "201021:客户," + Masterpage.CurrUser.client_code + ",查看进水监管好氧进水30日趋势图表");
            dynamic data = new System.Dynamic.ExpandoObject();
            string select = "";
            #region 从工作台链接
            if (from != null && from == "platform")
            {
                var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
                select = chartconfig.chart1_1;
                if (chartconfig.chart1_2 != "") select += (select != "" ? ("," + chartconfig.chart1_2) : chartconfig.chart1_2);
            }
            #endregion
            data.select = select;
            return View(data);
        }



        public ActionResult anaerobic(string from)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "201031:客户," + Masterpage.CurrUser.client_code + ",查看进水监管厌氧进水30日趋势图表");
            dynamic data = new System.Dynamic.ExpandoObject();
            string select = "";
            #region 从工作台链接
            if (from != null && from == "platform")
            {
                var chartconfig = centerService.GetPlatFormThreeChartCode(Masterpage.CurrUser.client_code, Masterpage.CurrUser.config1);
                select = chartconfig.chart1_1;
                if (chartconfig.chart1_2 != "") select += (select != "" ? ("," + chartconfig.chart1_2) : chartconfig.chart1_2);
            }
            #endregion
            data.select = select;
            return View(data);
        }
      

    }
}
