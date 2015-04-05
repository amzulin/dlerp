using ecoBio.Wms.Common;
using ecoBio.Wms.ExcelRead;
using ecoBio.Wms.FlexData;
using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using ecoBio.Wms.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ecoBio.Wms.Web.Controllers
{
    public class datacenterController : BaseController
    {
        private DataCenterService centerService;
        public datacenterController(IDataCenterRepository _centerRepository)
        {
            centerService = new DataCenterService(_centerRepository);
        }
        #region     数据录入
        public ActionResult datainput(DateTime? date)
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "701011:客户," + Masterpage.CurrUser.client_code + ",数据录入");
            dynamic data = new System.Dynamic.ExpandoObject();
            DateTime fordate;
            int onlyread = 1;
            if (date.HasValue)
            {
                if (date.Value.Date >= DateTime.Now.Date)
                {
                    fordate = DateTime.Now.Date;
                    onlyread = 0;
                }
                else fordate = date.Value.Date;
            }
            else
            {
                fordate = DateTime.Now.Date;
                onlyread = 0;
            }
            var pf = WebAccountHelper.GetDataCenterTxt(Masterpage.CurrUser.client_code, fordate, 1);
            var jx = WebAccountHelper.GetDataCenterTxt(Masterpage.CurrUser.client_code, fordate, 2);
            var gy = WebAccountHelper.GetDataCenterTxt(Masterpage.CurrUser.client_code, fordate, 3);
            data.pf = pf;
            data.jx = jx;
            data.gy = gy;
            data.fordate = fordate;
            data.onlyread = 0;// onlyread;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult savecollectionvalue(DateTime date, string type, string unit, string ids, string values)
        {
            //var date = DateTime.Now; 
            #region 参数验证
            if (date.Date > DateTime.Now.Date)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值失败，传递的日期不能大于当前日期");
                return Content("-1");
            }
            if (ids.EndsWith(",")) ids = ids.Substring(0, ids.Length - 1);
            //if (values.EndsWith(",")) values = values.Substring(0, values.Length - 1);
            string[] idlist = ids.Split(',');
            string[] valueslist = values.Split(',');
            if (idlist.Length != valueslist.Length || idlist.Length < 1)
            {
                return Content("-1");
            }
            #endregion
            #region 存入临时内存
            var model = new DataCenterInput();
            var session = "DataInput_" + Masterpage.CurrUser.client_code + "_" + date.ToString("yyyyMMdd");
            model = (DataCenterInput)SessionHelper.GetSession(session);
            if (model == null)
            {
                model = new DataCenterInput();
                model.counts.Add(idlist.Length);
                model.customercode = Masterpage.CurrUser.client_code;
                model.date = date;
                model.datestr = date.ToString("yyyyMMdd");
                model.types.Add(type);
                model.units.Add(unit);
                for (int i = 0; i < idlist.Length; i++)
                {
                    model.idlist.Add(idlist[i]);
                    model.valuelist.Add(valueslist[i]);
                }
            }
            else
            {
                var ui = model.units.IndexOf(unit);
                if (ui != -1)
                {
                    #region  已存在 则先删除
                    int index = 0;
                    for (int i = 0; i < ui; i++)
                    {
                        var c = model.counts[i];
                        for (int k = 0; k < c; k++)
                        {
                            index++;
                        }
                    }
                    model.idlist.RemoveRange(index, model.counts[ui]);
                    model.valuelist.RemoveRange(index, model.counts[ui]);
                    model.counts.RemoveAt(ui);
                    model.types.RemoveAt(ui);
                    model.units.RemoveAt(ui);

                    #endregion
                }
                #region 重新添加
                model.counts.Add(idlist.Length);
                model.types.Add(type);
                model.units.Add(unit);
                for (int i = 0; i < idlist.Length; i++)
                {
                    model.idlist.Add(idlist[i]);
                    model.valuelist.Add(valueslist[i]);
                }

                #endregion
            }
            SessionHelper.SetSession(session, model);
            #endregion
            //int count = centerService.SaveCustomerCollectionValue(Masterpage.CurrUser.client_code, unit, idlist, valueslist, date);
            //LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存单元的数据录入值，单元为" + unit + "，采集点列表：" + idlist + "，对应值为：" + valueslist + "");
            return Content(idlist.Length.ToString());
        }

        [HttpPost]
        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult savecollectionvalueall(DateTime date)
        {
            if (date.Date > DateTime.Now.Date)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值失败，传递的日期不能大于当前日期");
                return Content("-1");
            }
            #region 验证
            int index = 0;
            var model = new DataCenterInput();
            var session = "DataInput_" + Masterpage.CurrUser.client_code + "_" + date.ToString("yyyyMMdd");
            model = (DataCenterInput)SessionHelper.GetSession(session);
            if (model == null)
            {
                return Content("0");
            }
            else
            {
                #region 提交
                int count = 0;
                for (int i = 0; i < model.units.Count; i++)
                {
                    var c = model.counts[i];
                    string[] pidlist = new string[c];
                    string[] pvaluelist = new string[c];
                    for (int j = 0; j < c; j++)
                    {
                        pidlist[j] = model.idlist[index];
                        pvaluelist[j] = model.valuelist[index];
                        index++;
                    }
                    count += centerService.SaveCustomerCollectionValue(Masterpage.CurrUser.client_code, model.units[i].Replace("-","#"), pidlist, pvaluelist, date);
                }
                #endregion
            }
            #endregion

            LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值，共成功" + index + "个");
            return Content(index.ToString());
        }

        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult savedateinpuvalue(DateTime date, string sendids, string sendvalues)
        {
            if (date.Date > DateTime.Now.Date)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值失败，传递的日期不能大于当前日期");
                return Content("-1");
            }
            #region 验证
            int count = 0;
            string[] ids = sendids.Split(',');
            string[] values = sendvalues.Split(',');
            if (ids.Length != values.Length || ids.Length < 1 || ids[0] == "")
            {
                return Content("-1");
            }
            #region 提交

            count = centerService.SaveCustomerCollectionValue(Masterpage.CurrUser.client_code, ids, values, date);
            #endregion

            #endregion

            LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值，共成功" + count + "个");
            return Content(count.ToString());
        }

        [HttpPost]
        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult loadunitform(string unitid, DateTime? date)
        {
            unitid = unitid.Replace("-", "#");
            var unit = centerService.GetCustomerPointInput2(Masterpage.CurrUser.client_code, unitid, date.Value);
            LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",保存所有单元的数据录入值失败，传递的参数有误");
            if (unit != null)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",加载数据录入的单元：" + unit);
                return Json(unit, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",加载数据录入的单元失败，单元为空");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 历史查询
        public ActionResult dataquery()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "702011:客户," + Masterpage.CurrUser.client_code + ",历史查询");
            return View();
        }
        #endregion

        #region 图表全屏
        [LoginAllow]
        public ActionResult chartfullscreen(string title,string s1,string s2, string t, string w,string h,  string m)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string type = "";
            if (s2 != "") type = "line2"; else type = "line1";


            #region 图表
            //var point = centerService.GetOneCustomerCollection(s1);
            //var chart = centerService.GetParamsQueryFlexChart("C03");
            //chart.charttype = type;
            //chart.customercode = Masterpage.CurrUser.client_code;
            //chart.customername = Masterpage.CurrUser.client_name;
            //chart.height = int.Parse(h);
            //chart.width = int.Parse(w);
            //#region 上下限

            //if (point.CustomerCollectionLowerLimit.HasValue)
            //{
            //    chart.needlower = 1;
            //    chart.lowerlimit = point.CustomerCollectionLowerLimit.Value;
            //}
            //else chart.needlower = 0;
            //if (point.CustomerCollectionLowLimit.HasValue)
            //{
            //    chart.needlow = 1;
            //    chart.lowlimit = point.CustomerCollectionLowLimit.Value;
            //}
            //else chart.needlow = 0;

            //if (point.CustomerCollectionUpLimit.HasValue)
            //{
            //    chart.needup = 1;
            //    chart.uplimit = point.CustomerCollectionUpLimit.Value;
            //}
            //else chart.needup = 0;

            //if (point.CustomerCollectionUpperLimit.HasValue)
            //{
            //    chart.needuper = 1;
            //    chart.uperlimit = point.CustomerCollectionUpperLimit.Value;
            //}
            //else chart.needuper = 0;
            //#endregion
            //chart.processparms = s1 + (s2 != "" ? ("," + s2) : "");
            //chart.title = title + "变化趋势";
            //chart.queryparms = t;

            #endregion


            #region 图表
            FlexChart chart = new FlexChart();
            string select = s1 +(s2 != "" ? ("," + s2) : "");
            chart = centerService.GetFlexChartBySelects(Masterpage.CurrUser.client_code, Masterpage.CurrUser.client_name, title, select);
            chart.charttype = type;
            chart.customercode = Masterpage.CurrUser.client_code;
            chart.customername = Masterpage.CurrUser.client_name;
            chart.height = int.Parse(h);
            chart.width = int.Parse(w);
            chart.title = title + "变化趋势";
            chart.queryparms = t + "|" + m;
            chart.url = Utils.GetFlexAddress();
            #endregion



            data.chart = JsonHelper.ToJson(chart);
            LogHelper.Info(Masterpage.CurrUser.alias, "702011:客户," + Masterpage.CurrUser.client_code + ",查看全屏图表，采集点为：" + s1);
            return View("chartfullscreen", data);
        }
        #endregion

        #region 数据录入第二，三部

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult calandtrend(DateTime date)
        {
            var rc = centerService.CalCollectionAndThirtyDaysTrend(Masterpage.CurrUser.client_code, date);
            rc = (rc == "" ? "成功" : rc);
            LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",数据中心录入数据，采集点值和30日趋势重新计算，计算开始日期为：" + date.ToString("yyyy-MM-dd") + "，操作结果: "+ rc);
            return Content(rc == "成功" ? "1" : "-1");
        }
        /// <summary>
        /// 数据录入后五步骤：1计算，2 30天趋势，3生成日报，4更新周报，5更新月报
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [AjaxAction(ForAction = "datainput", ForController = "datacenter")]
        public ActionResult inputstep(DateTime date, int step, string content,string remark)
        {
            var rc = centerService.CalCollectionForReort(Masterpage.CurrUser.client_code, date, step,content,remark);
            #region 日报更新日志和备注
            if (rc == ""&&step==2)
            {
               var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceday/" + date.ToString("yyyy-MM-dd") + ".txt";
               try
               {
                   if (ecoBio.Wms.Common.FileHelper.IsFileExist(filepath))
                   {
                       var b = ServiceWeekly.TxtRemark(filepath, remark,content);
                   }
               }
               catch
               { }
            }
            #endregion
            rc = (rc == "" ? "成功" : rc);
            LogHelper.Info(Masterpage.CurrUser.alias, "701012:客户," + Masterpage.CurrUser.client_code + ",数据中心录入数据运行步骤"+step.ToString()+"，日期为：" + date.ToString("yyyy-MM-dd") + "，操作结果:" + rc);
            return Content(rc == "成功" ? "1" : "-1");
        }

        #endregion
    }
}
