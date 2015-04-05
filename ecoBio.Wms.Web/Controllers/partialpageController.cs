using ecoBio.Wms.Common;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.ViewModel;
using ecoBio.Wms.FlexData;
using ecoBio.Wms.Data.Entities.Models;

namespace ecoBio.Wms.Web.Controllers
{
    public class partialpageController : BaseController
    {
        private DataCenterService centerService;
        public partialpageController(IDataCenterRepository _centerRepository)
        {
            centerService = new DataCenterService(_centerRepository);
        }
        #region 工艺监管

        /// <summary>
        /// 工艺监管中工艺参数的查询分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="key">查询关键字</param>
        /// <param name="select">以选参数,英文逗号分隔</param>
        /// <param name="c">请求的controller</param>
        /// <param name="a">请求的action</param>
        /// <returns></returns>
        [HidNowLocal]
        [LoginAllow]
        [ValidateInput(false)]
        public ActionResult ParamsQuery(int? page, int? pagesize, string title, string key, string select,  int? t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();            
            var list = centerService.GetParamsQueryList(Masterpage.CurrUser.client_code, key, t.HasValue ? t.Value : 0).OrderBy(p => p.StandardProcessUnitCode).ToList();

            var list2 = list.OrderBy(x => x.collent_point_order).ToList();
            #region 分页计算
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 11;
            var vs = list2.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            #endregion            
            
            #region 页面参数
            data.key = key;
            data.title = title;
            data.t = t.Value;  
            var pn1 = "";
            var pn2 = "";
            var un1 = "";
            var un2 = "";

            if (select == null||select=="") select = "";
            else
            {
                string[] sa = select.Split(',');
                if (sa[0] != "")
                {
                    var sh = list.FirstOrDefault(p => p.CustomerCollectionCode == sa[0]);
                    if (sh != null)
                    {
                        pn1 = sh.collent_point_html;
                        un1 = sh.collent_unit_name;
                    }
                } 
                if (sa[1] != "")
                {
                    var sh = list.FirstOrDefault(p => p.CustomerCollectionCode == sa[1]);
                    if (sh != null)
                    {
                        pn2 = sh.collent_point_html;
                        un2 = sh.collent_unit_name;
                    }
                }
            }
            data.pn1 = pn1;
            data.pn2 = pn2;
            data.un1 = un1;
            data.un2 = un2;
            data.select = select; 
          
            #endregion
            LogHelper.Info(Masterpage.CurrUser.alias, "201011:客户," + Masterpage.CurrUser.client_code + ",查看采集点" + select + "的30日趋势图表,菜单为：" + title);
            return PartialView("ParamsQuery", data);
        }

        /// <summary>
        /// 过滤关键字
        /// </summary>
        /// <returns></returns>
        [HidNowLocal]
        [LoginAllow]
        [HttpGet]
        public ActionResult filterparam()
        {
            int t = WebRequest.GetQueryInt("t", 0);
            string key = WebRequest.GetQueryString("key", true);
            List<object> r = new List<object>();
            var list = centerService.GetFilterparam(Masterpage.CurrUser.client_code, key, t);
            LogHelper.Info(Masterpage.CurrUser.alias, "201011:客户," + Masterpage.CurrUser.client_code + ",查看30日趋势图表,过滤客户采集点，类别为：" + t + "，关键字：" + key);
          
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 更新图表和统计数据
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        [HidNowLocal]
        [LoginAllow]
        [HttpGet]
        public ActionResult upchartandgrid(string select, string title, string t, string m)
        {
            #region 统计数据

            if (select.EndsWith(",")) select = select.Substring(0, select.Length - 1);
            string[] getsel = select.Split(',');
            if (getsel.Length < 1) { return Json("", JsonRequestBehavior.AllowGet); }
            #region  读取
            DateTime start;
            DateTime end;
            if (m == null || m == "")
            {
                start = DateTime.Now.Date.AddMonths(-1);
            }
            else
            {
                start = Convert.ToDateTime(m);
            }
            end = start.AddMonths(1);
            var l = WebAccountHelper.ProcessStatistic(select, start, end, t);
            #endregion
            #endregion
            #region 图表
            FlexChart chart = new FlexChart();
            chart = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, title, select);
            chart.url = Utils.GetFlexAddress();
            chart.title = title + "变化趋势";
            chart.queryparms = t + "|" + m;

            #endregion
            var result = new { grid = l, chart = chart };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion        

        #region 历史查询

        /// <summary>
        /// 工艺监管中工艺参数的查询分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="key">查询关键字</param>
        /// <param name="select">以选参数,英文逗号分隔</param>
        /// <param name="c">请求的controller</param>
        /// <param name="a">请求的action</param>
        /// <returns></returns>
        [HidNowLocal]
        [LoginAllow]
        [ValidateInput(false)]
        public ActionResult HistoryProcessQuery(int? page, int? pagesize, string key, string select)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string[] type = centerService.GetCollectionType();
            //var list = centerService.GetCustomerCollectionPageMode(Masterpage.CurrUser.client_code, key,"");
            var list = centerService.GetHistoryProcessList(Masterpage.CurrUser.client_code, key); 
            var list2 = list.OrderBy(x => x.collent_point_order).ToList();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 13;
            var vs = list2.ToPagedList(_page, _pagesize);
            data.customercode = Masterpage.CurrUser.client_code;
            data.type = type;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            if(select==null) select="";
            if(select.EndsWith(",")) select=select.Substring(0,select.Length-1);
            #region 图表
            if (select != "" )
            {
                FlexChart chart = new FlexChart();
                chart = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "历史查询", select);
                chart.url = Utils.GetFlexAddress(); 
                chart.processparms = select;
                chart.title = "历史查询";
                chart.queryparms = "";
                data.chart =JsonHelper.ToJson( chart);
            }
            #endregion
            if(select==null) select="";
            data.select = select;
            data.key = key;
            LogHelper.Info(Masterpage.CurrUser.alias, "201012:客户," + Masterpage.CurrUser.client_code + ",查看采集点" + select + "历史查看图表");
            return PartialView("HistoryProcessQuery", data);
        }
        
        [HidNowLocal]
        [LoginAllow]
        [HttpGet]
        public ActionResult uphistorychart(string select)
        {              
            if(select.EndsWith(",")) select=select.Substring(0,select.Length-1);
            FlexChart chart = new FlexChart();
            chart = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, "历史查询", select);

            #region 图表
    
            //chart.charttype = chart.standardtype + getsel.Length.ToString();
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.customername = Masterpage.CurrUser.client_name;
            chart.url = Utils.GetFlexAddress();
            chart.processparms = select;
            chart.title =  "历史查询";
            #endregion   
            var result = new {  chart = chart };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion        

        #region 产率
        [HidNowLocal]
        [LoginAllow]
        public ActionResult YieldQuery(int? page, int? pagesize, string key, string select1, string select2, int? count, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            //var list = centerService.GetCustomerCollectionPageMode(Masterpage.CurrUser.client_code, key, t);
            var list = centerService.GetParamsQueryList(Masterpage.CurrUser.client_code, key,  0);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            #region 统计数据图表
            if ((select1 == null || select1.ToString() == "") && vs.Count > 0)
            {
                data.select1 = vs[0].CustomerCollectionCode;
            }
            else
                data.select1 = select1;
            if ((select2 == null || select2.ToString() == "") && vs.Count > 1)
            {
                data.select2 = vs[1].CustomerCollectionCode;
            }           
            else
            {
                data.select2 = select2;
            }
            #endregion
            data.key = key;
            data.t = t;
            return PartialView("YieldQuery", data);
        }
        #endregion

        #region session维持

        [HidNowLocal]
        [LoginAllow]
        [HttpPost]
        public ActionResult autopost()
        {
            ReturnValue r = new ReturnValue();
            r.status = "ok";
            r.message = SessionHelper.SessionId;
            Session["autopostsession"] = r.message;
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
    }
}
