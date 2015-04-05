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
using ecoBio.Wms.Data.Entities.Models;
using System.IO;
using System.Configuration;

namespace ecoBio.Wms.Web.Controllers
{
    public class diagnosticController : BaseController
    {
        private DiagnosticService diagnosticService;
        public diagnosticController(IDiagnosticRepository _diagnosticRepository)
        {
            diagnosticService = new DiagnosticService(_diagnosticRepository);
        }

        #region 诊断建议
        public ActionResult recoommend(int? page, int? pagesize)
        {
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = diagnosticService.GetCustomerDiaglosisList(Masterpage.CurrUser.client_code, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.ddlstatus = diagnosticService.DiaglosisStatusDDL();
            string otherparam = "";
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "501011:客户," + Masterpage.CurrUser.client_code + ",诊断建议列表，第" + _page + "页");
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult recoommendpage(string guid, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerDiaglosisList one = new CustomerDiaglosisList();
            string person = Masterpage.CurrUser.alias;
            string type = t;
            string number = "";
            //string units="";
            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try { g = Guid.Parse(guid); }
                catch { return RedirectToAction("recoommend"); }
                one = diagnosticService.GetOneCustomerDiaglosisList(g);
                data.hv = 1;
                person = one.CustomerDiaglosisApplicant;
                number = one.CustomerDiaglosisNumber.Replace(Masterpage.CurrUser.client_code + "-", "");
                LogHelper.Info(Masterpage.CurrUser.alias, "501012:客户," + Masterpage.CurrUser.client_code + ",查看诊断建议填写的信息，单号为：" + number);
                //units=one.un;
            }
            else
            {
                one = new CustomerDiaglosisList();
                number = diagnosticService.GetNewDiaglosisNumber(Masterpage.CurrUser.client_code).Replace(Masterpage.CurrUser.client_code + "-", "");
                LogHelper.Info(Masterpage.CurrUser.alias, "501012:客户," + Masterpage.CurrUser.client_code + ",打开新增诊断建议弹出页，生成诊断单号为：" + number);
            }
            //data.ddlh = diagnosticService.StandardUnitAerobic(Masterpage.CurrUser.client_code);
            //data.ddly = diagnosticService.StandardUnitAnaerobic(Masterpage.CurrUser.client_code);
            data.one = one;
            data.person = person;
            //data.units = units;
            data.number = number;
            data.type = t;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult recoommendprocess(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerDiaglosisList one = new CustomerDiaglosisList();
            CustomerDiaglosisConfirm confirm = new CustomerDiaglosisConfirm();
            ReturnValue r;
            string person = "";
            string date = "";
            int stepno = 0;
            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try
                {
                    g = Guid.Parse(guid);
                    one = diagnosticService.GetOneCustomerDiaglosisList(g);
                    if (one == null)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501013:客户," + Masterpage.CurrUser.client_code + ",查看诊断建议流程页面失败，申请不存在");
                        r = new ReturnValue { message = "申请不存在", status = "error" };
                    }
                    else
                    {
                        stepno = one.CustomerDiaglosisStepNo.Value;
                        confirm = diagnosticService.GetCustomerDiaglosisConfirm(one.CustomerDiaglosisGuid);
                        if (confirm != null)
                        {
                            person = confirm.CustomerDiaglosisConfirmPerson;
                            date = confirm.CustomerDiaglosisConfirmTime.Value.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            person = Masterpage.CurrUser.alias;
                            date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        LogHelper.Info(Masterpage.CurrUser.alias, "501013:客户," + Masterpage.CurrUser.client_code + ",查看诊断建议流程页面，单号为：" + one.CustomerDiaglosisNumber);
                        r = new ReturnValue { message = "", status = "ok" };
                    }
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501013:客户," + Masterpage.CurrUser.client_code + ",查看诊断建议流程页面失败，参数有误");
                    r = new ReturnValue { message = "参数有误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501013:客户," + Masterpage.CurrUser.client_code + ",查看诊断建议流程页面失败，缺少参数");
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }
            data.stepno = stepno;
            data.person = person;
            data.date = date;
            data.one = one;
            data.r = r;
            data.confirm = confirm;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deleterecoommend(string guid)
        {
            ReturnValue r;
            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try
                {
                    g = Guid.Parse(guid);
                    bool b = diagnosticService.DeleteCustomerDiaglosis(g);
                    if (b)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501014:客户," + Masterpage.CurrUser.client_code + ",删除诊断建议，诊断guid为：" + guid);
                        r = new ReturnValue { message = "", status = "ok" };
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501014:客户," + Masterpage.CurrUser.client_code + ",删除诊断建议失败，删除失败");
                        r = new ReturnValue { message = "删除失败", status = "error" };
                    }
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501014:客户," + Masterpage.CurrUser.client_code + ",删除诊断建议失败，参数错误");
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501014:客户," + Masterpage.CurrUser.client_code + ",删除诊断建议失败，缺少参数");
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult saverecoommend(string content)
        {
            var guid = WebRequest.GetString("guid", true);
            var type = WebRequest.GetString("type", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var fault = WebRequest.GetString("fault", true);
            var units = WebRequest.GetString("units", true);
            var remark = WebRequest.GetString("remark", true);

            ReturnValue r;
            try
            {
                if (type != "edit" && type != "add")
                {
                    r = new ReturnValue { message = "操作参数有误", status = "error" };
                }
                else
                {
                    Guid g;
                    if (type == "edit")
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501015:客户," + Masterpage.CurrUser.client_code + ",编辑诊断信息，日期为：" + date + "，问题描述：" + fault + "，单元：" + units + "，备注：" + remark + "");
                        g = Guid.Parse(guid);
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501015:客户," + Masterpage.CurrUser.client_code + ",添加诊断信息，日期为：" + date + "，问题描述：" + fault + "，单元：" + units + "，备注：" + remark + "");

                        g = new Guid();
                    }
                    r = diagnosticService.SaveOneCustomerDiaglosis(type, Masterpage.CurrUser.client_code, g, "申请", units, p, content, fault, remark);
                }
            }
            catch
            {
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult recoommendconfirm()
        {
            var guid = WebRequest.GetString("guid", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var content = WebRequest.GetString("content", true);
            var remark = WebRequest.GetString("remark", true);

            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);

                r = diagnosticService.ConfirmCustomerDiaglosis(g, p, content, remark, "确认");
                LogHelper.Info(Masterpage.CurrUser.alias, "501016:客户," + Masterpage.CurrUser.client_code + ",诊断确认信息，诊断guid:" + guid + ",内容：" + content + "，确认人：" + p + "，备注：" + remark + "，保存结果：" + r.status);

            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501016:客户," + Masterpage.CurrUser.client_code + ",诊断确认信息失败，诊断guid:" + guid + "，内容：" + content + "，确认人：" + p + "，备注：" + remark);
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #region 诊断流程

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult uploadpdf()
        {
            string imgpath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic";
            string backpath = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic";
            // string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(imgpath, identifier.ToString());
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".pdf")
                {
                    r = new ReturnValue { status = "error", message = "请上传pdf类型文档" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                LogHelper.Info(Masterpage.CurrUser.alias, "501017:客户," + Masterpage.CurrUser.client_code + ",上传诊断文档" + fn + "上传路径为：" + uploadsPath);
                httpfile.SaveAs(uploadsPath + exn);
                r = new ReturnValue { status = "ok", value = backpath + "/" + identifier.ToString() + exn, value2 = identifier.ToString() + exn };
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501017:客户," + Masterpage.CurrUser.client_code + ",上传诊断文档失败");
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Content(JsonHelper.ToJson(r));
            //return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult downloadpdf()
        {
            string name = WebRequest.GetString("name", true);
            #region 查找
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic/" + name;
                if (FileHelper.IsFileExist(path))
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501017:客户," + Masterpage.CurrUser.client_code + ",下载诊断文档" + name + "成功，路径为：" + path);
                    FileHelper.DownLoadFile(path, Masterpage.CurrUser.client_name + name);
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501017:客户," + Masterpage.CurrUser.client_code + ",下载诊断文档" + name + "失败，文件不存在");
                    return Content("文件不存在");
                }
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501017:客户," + Masterpage.CurrUser.client_code + ",下载诊断文档" + name + "失败，下载异常");
                return Content("下载异常");
            }
            #endregion
            return View();
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult partsit(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g = Guid.Parse(guid);
            var one = diagnosticService.GetOneCustomerDiaglosisList(g);
            data.close = one.CustomerDiaglosisIsClosed.Value;
            List<CustomerDiaglosisSiteResearch> listsit = new List<CustomerDiaglosisSiteResearch>();
            if (one != null)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501018:客户," + Masterpage.CurrUser.client_code + ",诊断单号为" + one.CustomerDiaglosisNumber + "的现场分析列表");
                listsit = diagnosticService.GetCustomerDiaglosisSiteResearch(g).ToList();
            }
            data.person = Masterpage.CurrUser.alias;
            data.guid = guid;
            data.listsit = listsit;
            return PartialView("partsit", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult savesit(string content)
        {
            var guid = WebRequest.GetString("guid", true);
            var type = WebRequest.GetString("type", true);
            var sn = WebRequest.GetString("sn", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var pdfname = WebRequest.GetString("pdfname", true);
            var remark = WebRequest.GetString("remark", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                r = diagnosticService.SaveDiaglosisSiteResearch(g, type, sn, p, date, pdfname, remark);
                LogHelper.Info(Masterpage.CurrUser.alias, "501020:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的现场分析报告" + pdfname);
                if (type == "add")
                {
                    //#region 自动生成模型计算pdf
                    //#region pdf文档
                    //string filename="";
                    //#endregion
                    //#region 添加记录
                    //r = diagnosticService.SaveOptimizationSuggestion(g, Convert.ToInt32(r.value), Masterpage.CurrUser.client_code, "BCO-1", filename);
                    //#endregion
                    //#endregion
                }
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501020:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的现场分析报告" + pdfname + "失败");
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deletesit(string sn)
        {
            ReturnValue r;
            if (sn != null && sn.ToString() != "")
            {
                var guid = WebRequest.GetString("guid", true);
                Guid g = Guid.Parse(guid);
                try
                {
                    var b = diagnosticService.DeleteDiaglosisSiteResearch(g, sn);
                    if (b != "")
                    {
                        r = new ReturnValue { message = "", status = "ok" };
                        string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic/" + b;
                        FileHelper.DeleteFile(path);
                        LogHelper.Info(Masterpage.CurrUser.alias, "501021:客户," + Masterpage.CurrUser.client_code + ",删除诊断的现场分析报告，报告编号为：" + sn);
                    }
                    else r = new ReturnValue { message = "删除失败", status = "error" };
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501021:客户," + Masterpage.CurrUser.client_code + ",删除诊断的现场分析报告失败，参数错误，报告编号为：" + sn);
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501021:客户," + Masterpage.CurrUser.client_code + ",删除诊断的现场分析报告失败，缺少参数，报告编号为：" + sn);
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #region 模型计算
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult partopt(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g = Guid.Parse(guid);
            var one = diagnosticService.GetOneCustomerDiaglosisList(g);
            data.close = one.CustomerDiaglosisIsClosed.Value;
            List<CustomerDiaglosisModelCal> listopt = new List<CustomerDiaglosisModelCal>();
            if (one != null)
            {
                listopt = diagnosticService.GetCustomerDiaglosisModelCal(g).ToList();
            }
            data.guid = guid;
            data.listopt = listopt;
            return PartialView("partopt", data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult saveopt()
        {
            var guid = WebRequest.GetString("guid", true);
            var date = WebRequest.GetString("date", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                r = diagnosticService.SaveCustomerDiaglosisModelCal(g, date, "重新计算");
            }
            catch
            {
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deleteopt(string sn)
        {
            ReturnValue r;
            if (sn != null && sn.ToString() != "")
            {
                var guid = WebRequest.GetString("guid", true);
                Guid g = Guid.Parse(guid);
                try
                {
                    g = Guid.Parse(guid);
                    var b = diagnosticService.DeleteCustomerDiaglosisModelCal(g, sn);
                    if (b != "")
                    {
                        string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic/" + b;
                        FileHelper.DeleteFile(path);
                        r = new ReturnValue { message = "", status = "ok" };
                    }
                    else r = new ReturnValue { message = "删除失败", status = "error" };
                }
                catch
                {
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 实验分析
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult partlab(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g = Guid.Parse(guid);
            var one = diagnosticService.GetOneCustomerDiaglosisList(g);
            data.close = one.CustomerDiaglosisIsClosed.Value;
            List<CustomerTestAnalysisReport> listlab = new List<CustomerTestAnalysisReport>();
            if (one != null)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501025:客户," + Masterpage.CurrUser.client_code + ",诊断单号为" + one.CustomerDiaglosisNumber + "的实验分析列表");

                listlab = diagnosticService.GetCustomerTestAnalysisReport(g).ToList();
            }
            data.guid = guid; data.person = Masterpage.CurrUser.alias;
            data.ddl = diagnosticService.AnalysisItem();
            data.listlab = listlab;
            return PartialView("partlab", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult savelab(string content)
        {
            var guid = WebRequest.GetString("guid", true);
            var type = WebRequest.GetString("type", true);
            var sn = WebRequest.GetString("sn", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var pdfname = WebRequest.GetString("pdfname", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                r = diagnosticService.SaveTestAnalysisReport(g, type, sn, p, date, pdfname);
                LogHelper.Info(Masterpage.CurrUser.alias, "501026:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的实验分析报告" + pdfname);
                if (type == "add" && r.value2 == "1")//第一次添加实验室分析报告
                {
                    #region 自动生成模型计算pdf
                    #region pdf文档
                    string filename = "自动计算";
                    #endregion
                    #region 添加记录
                    r = diagnosticService.SaveCustomerDiaglosisModelCal(g, DateTime.Now.ToString(), filename);
                    #endregion
                    #endregion
                }
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501026:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的实验分析报告" + pdfname + "失败");
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deletelab(string sn)
        {
            ReturnValue r;
            if (sn != null && sn.ToString() != "")
            {
                var guid = WebRequest.GetString("guid", true);
                Guid g = Guid.Parse(guid);
                try
                {
                    var b = diagnosticService.DeleteTestAnalysisReport(g, sn);
                    if (b != "")
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "501027:客户," + Masterpage.CurrUser.client_code + ",删除诊断的实验分析报告，报告编号为：" + sn);
                        string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic/" + b;
                        FileHelper.DeleteFile(path);
                        r = new ReturnValue { message = "", status = "ok" };
                    }
                    else r = new ReturnValue { message = "删除失败", status = "error" };
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501027:客户," + Masterpage.CurrUser.client_code + ",删除诊断的实验分析报告失败，缺少参数，报告编号为：" + sn);
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501027:客户," + Masterpage.CurrUser.client_code + ",删除诊断的实验分析报告失败，缺少参数，报告编号为：" + sn);
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 解决方案
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult partsln(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g = Guid.Parse(guid);
            var one = diagnosticService.GetOneCustomerDiaglosisList(g);
            data.close = one.CustomerDiaglosisIsClosed.Value;
            List<CustomerDiaglosisSolution> listsln = new List<CustomerDiaglosisSolution>();
            if (one != null)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501028:客户," + Masterpage.CurrUser.client_code + ",诊断单号为" + one.CustomerDiaglosisNumber + "的解决方案列表");
                listsln = diagnosticService.GetCustomerDiaglosisSolution(g).ToList();
            }
            data.person = Masterpage.CurrUser.alias;
            data.guid = guid;
            data.listsln = listsln;
            return PartialView("partsln", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult savesln(string content)
        {
            var guid = WebRequest.GetString("guid", true);
            var type = WebRequest.GetString("type", true);
            var sn = WebRequest.GetString("sn", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var remark = WebRequest.GetString("remark", true);
            var pdfname = WebRequest.GetString("pdfname", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                LogHelper.Info(Masterpage.CurrUser.alias, "501034:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的解决方案报告" + pdfname);
                r = diagnosticService.SaveDiaglosisSolution(g, type, sn, p, date, pdfname, remark);
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501034:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的解决方案报告" + pdfname + "失败");
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deletesln(string sn)
        {
            ReturnValue r;
            if (sn != null && sn.ToString() != "")
            {
                var guid = WebRequest.GetString("guid", true);
                Guid g = Guid.Parse(guid);
                try
                {
                    var b = diagnosticService.DeleteDiaglosisSolution(g, sn);
                    if (b != "")
                    {
                        r = new ReturnValue { message = "", status = "ok" };
                        string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/diagnostic/" + b;
                        FileHelper.DeleteFile(path);
                        LogHelper.Info(Masterpage.CurrUser.alias, "501029:客户," + Masterpage.CurrUser.client_code + ",删除诊断的解决方案报告，报告编号为：" + sn);
                    }
                    else r = new ReturnValue { message = "删除失败", status = "error" };
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501029:客户," + Masterpage.CurrUser.client_code + ",删除诊断的解决方案报告失败，参数错误，报告编号为：" + sn);
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501029:客户," + Masterpage.CurrUser.client_code + ",删除诊断的解决方案报告失败，缺少参数，报告编号为：" + sn);
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 结果跟踪
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult partres(string guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g = Guid.Parse(guid);
            var one = diagnosticService.GetOneCustomerDiaglosisList(g);
            data.close = one.CustomerDiaglosisIsClosed.Value;
            List<CustomerDiaglosisResultTrace> listres = new List<CustomerDiaglosisResultTrace>();
            if (one != null)
            {

                LogHelper.Info(Masterpage.CurrUser.alias, "501030:客户," + Masterpage.CurrUser.client_code + ",诊断单号为" + one.CustomerDiaglosisNumber + "的结果跟踪列表");
                listres = diagnosticService.GetCustomerDiaglosisResultTrace(g).ToList();
            }
            data.person = Masterpage.CurrUser.alias;
            data.guid = guid;
            data.listres = listres;
            return PartialView("partres", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult saveres(string content)
        {
            var guid = WebRequest.GetString("guid", true);
            var type = WebRequest.GetString("type", true);
            var sn = WebRequest.GetString("sn", true);
            var p = WebRequest.GetString("p", true);
            var date = WebRequest.GetString("date", true);
            var pdfname = WebRequest.GetString("pdfname", true);
            var remark = WebRequest.GetString("remark", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                LogHelper.Info(Masterpage.CurrUser.alias, "501031:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的结果跟踪报告" + pdfname);
                r = diagnosticService.SaveDiaglosisResultTrace(g, type, sn, p, date, pdfname, remark);
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501031:客户," + Masterpage.CurrUser.client_code + ",保存诊断单号为" + guid + "的结果跟踪报告" + pdfname + "失败");
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult deleteres(string sn)
        {
            ReturnValue r;
            if (sn != null && sn.ToString() != "")
            {
                var guid = WebRequest.GetString("guid", true);
                Guid g = Guid.Parse(guid);
                try
                {
                    bool b = diagnosticService.DeleteDiaglosisResultTrace(g, sn);
                    if (b)
                    {
                        r = new ReturnValue { message = "", status = "ok" };
                        LogHelper.Info(Masterpage.CurrUser.alias, "501032:客户," + Masterpage.CurrUser.client_code + ",删除诊断的结果跟踪报告，报告编号为：" + sn);
                    }
                    else r = new ReturnValue { message = "删除失败", status = "error" };
                }
                catch
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "501032:客户," + Masterpage.CurrUser.client_code + ",删除诊断的结果跟踪报告失败，缺少参数，报告编号为：" + sn);
                    r = new ReturnValue { message = "参数错误", status = "error" };
                }
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501032:客户," + Masterpage.CurrUser.client_code + ",删除诊断的结果跟踪报告失败，缺少参数，报告编号为：" + sn);
                r = new ReturnValue { message = "缺少参数", status = "error" };
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 结束

        [AjaxAction(ForAction = "recoommend", ForController = "diagnostic")]
        public ActionResult recoommendclose()
        {
            var guid = WebRequest.GetString("guid", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                LogHelper.Info(Masterpage.CurrUser.alias, "501033:客户," + Masterpage.CurrUser.client_code + ",关闭诊断单号为" + guid + "的诊断流程");
                r = diagnosticService.CloseCustomerDiaglosis(g);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "501033:客户," + Masterpage.CurrUser.client_code + ",关闭诊断单号为" + guid + "的诊断流程失败" + ex.Message);
                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion
        #endregion

        #region 系统优化

        public ActionResult optimizationday(int? page, int? pagesize)
        {
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = diagnosticService.GetProcessUnitOptimizationSuggestion(Masterpage.CurrUser.client_code, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            data.IsExpert = Masterpage.CurrUser.IsExpert;
            LogHelper.Info(Masterpage.CurrUser.alias, "502011:客户," + Masterpage.CurrUser.client_code + ",系统优化列表，第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "optimizationday", ForController = "diagnostic")]
        public ActionResult optimizationdayview()
        {
            string guid = WebRequest.GetString("guid", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            List<StandardProcessUnit> units = new List<StandardProcessUnit>();
            List<DiagnosticUnit> list = new List<DiagnosticUnit>();
            CustomerProcessUnitOptimizationSuggestion one = new CustomerProcessUnitOptimizationSuggestion();
            ReturnValue r;
            try
            {
                units = diagnosticService.GetDiagnosticUnit();
                one = diagnosticService.GetOneProcessUnitOptimizationSuggestion(Masterpage.CurrUser.client_code, Guid.Parse(guid));
                if (one == null) r = new ReturnValue { message = "数据不存在", status = "error" };
                else
                {
                    list = diagnosticService.GetDiagnosticUnitValue(Masterpage.CurrUser.client_code, units, one.CustomerOptimizationDate.Value);
                    LogHelper.Info(Masterpage.CurrUser.alias, "502012:客户," + Masterpage.CurrUser.client_code + ",查看系统优化" + one.CustomerOptimizationGuid + "的详情信息");
                    r = new ReturnValue { message = "", status = "ok" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "502012:客户," + Masterpage.CurrUser.client_code + ",查看系统优化" + one.CustomerOptimizationGuid + "的详情信息，打开失败" + ex.Message);
                r = new ReturnValue { message = "参数错误", status = "error" };
            }
            data.r = r;
            data.units = units;
            data.list = list;
            data.one = one;
            return View(data);
        }
        public ActionResult optimizationmonth(int? page, int? pagesize)
        {
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = diagnosticService.GetProcessUnitOptimizationSuggestion(Masterpage.CurrUser.client_code, 30, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "optimizationmonth", ForController = "diagnostic")]
        public ActionResult optimizationmonthview()
        {
            string date = WebRequest.GetString("date", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r;
            List<OptimizationSuggestionUnit> list;
            try
            {
                DateTime dt = Convert.ToDateTime(date);
                list = diagnosticService.GetOptimizationSuggestionUnit(Masterpage.CurrUser.client_code, 30, dt.Date);
                r = new ReturnValue { message = "", status = "ok" };
            }
            catch
            {
                r = new ReturnValue { message = "参数错误", status = "error" };
                list = new List<OptimizationSuggestionUnit>();
            }
            data.r = r;
            data.list = list;
            data.date = date;
            return View(data);
        }

        [AjaxAction(ForAction = "optimizationday", ForController = "diagnostic")]
        public ActionResult optimizationpage(string guid, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            SystemOptimization one = new SystemOptimization();
            string type = t;

            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try { g = Guid.Parse(guid); }
                catch { return RedirectToAction("siteservice"); }
                var suggestion = diagnosticService.GetOneProcessUnitOptimizationSuggestion(Masterpage.CurrUser.client_code, g);
                one = JsonHelper.FromJson<SystemOptimization>(suggestion.CustomerOptimizationSuggestion); 
                data.hv = 1;
                LogHelper.Info(Masterpage.CurrUser.alias, "502012:客户," + Masterpage.CurrUser.client_code + ",查看操作建议信息，操作建议guid" + guid);
            }
            else
            {
                data.hv = 0;
                one = new SystemOptimization();
                LogHelper.Info(Masterpage.CurrUser.alias, "502012:客户," + Masterpage.CurrUser.client_code + ",新增操作建议");
            }
            data.guid = guid;
            data.one = one;
            data.type = t;
            return View(data);
        }
         [AjaxAction(ForAction = "optimizationday", ForController = "diagnostic")]
        public ActionResult saveoptimization()
        {
            string guid = WebRequest.GetString("guid", true);
            string type = WebRequest.GetString("type", true);
            string dates = WebRequest.GetString("date", true);
            string wind = WebRequest.GetString("wind", true);
            string mud = WebRequest.GetString("mud", true);
            string water = WebRequest.GetString("water", true);
            string nutrition = WebRequest.GetString("nutrition", true);
            string other = WebRequest.GetString("other", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            Guid g2;
            if (type == "add") g2 = Guid.Empty;
            else g2 = Guid.Parse(guid);
            SystemOptimization model = new SystemOptimization();
            model.wind = wind;
            model.water = water;
             model.mud=mud;
              model.nutrition=nutrition;
              model.other = other;

              r = diagnosticService.SaveOptimizationSuggestionUnit(Masterpage.CurrUser.client_code, g2, type, DateTime.Parse(dates), model, 1);

              LogHelper.Info(Masterpage.CurrUser.alias, "502013:客户," + Masterpage.CurrUser.client_code + ",保存优化建议，优化建议guid" + guid + "，操作类别：" + type + "，建议内容：" + JsonHelper.ToJson(model) + "操作结果：" + r.status);
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "optimizationday", ForController = "diagnostic")]
         public ActionResult deloptimization()
        {
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                bool b = diagnosticService.DeleteOptimizationSuggestionUnit(Masterpage.CurrUser.client_code, g);
                if (b) r = new ReturnValue { status = "ok" };
                else r = new ReturnValue { status = "error", message = "删除失败！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "502014:客户," + Masterpage.CurrUser.client_code + ",优化建议" + guid + "删除" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "502014:客户," + Masterpage.CurrUser.client_code + ",优化建议" + guid + ",删除失败，" + ex.Message);
                r = new ReturnValue { status = "error", message = "删除失败！" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 自助诊断

        public ActionResult selfdiag()
        {
            LogHelper.Info(Masterpage.CurrUser.alias, "803011:客户," + Masterpage.CurrUser.client_code + ",查看自助诊断");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.category = diagnosticService.Knowledgebasecategory();
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult faqlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var list = diagnosticService.GetFAQ(key);
            var category = diagnosticService.Faqunitcategory();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.key = key;
            data.list = vs;
            data.category = category;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            int expert = Masterpage.CurrUser.IsExpert ? 1 : 0;
            data.expert = expert;
            LogHelper.Info(Masterpage.CurrUser.alias, "803021:客户," + Masterpage.CurrUser.client_code + ",查看FAQ列表，第" + _page + "页");
            return PartialView("faqlist", data);
        }

        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult faqedit(string t, int? sn)
        {
            ReturnValue r = new ReturnValue();
            FAQ one = new FAQ();
            var category = diagnosticService.Faqunitcategory();
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                if (!Masterpage.CurrUser.IsExpert)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "803022:客户," + Masterpage.CurrUser.client_code + ",编辑FAQ" + sn + "失败，不是专家，权限不足");
                    r = new ReturnValue { status = "error", message = "权限不足!" };
                }
                else
                {
                    if (t == "edit") one = diagnosticService.GetOneFAQ(sn.Value);
                    LogHelper.Info(Masterpage.CurrUser.alias, "803022:客户," + Masterpage.CurrUser.client_code + ",编辑FAQ" + sn);
                    r = new ReturnValue { status = "ok" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "803022:客户," + Masterpage.CurrUser.client_code + ",编辑FAQ" + sn + "失败" + ex.Message);
                r = new ReturnValue { status = "error", message = "读取失败!" };
            }
            data.category = category;
            data.r = r;
            data.one = one;
            data.sn = sn;
            data.t = t;
            return PartialView("faqedit", data);
        }
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult uploadknow()
        {
            string imgpath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/knewledgebase";
            string backpath = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/knewledgebase";
            //string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(imgpath, identifier.ToString());
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".pdf")
                {
                    r = new ReturnValue { status = "error", message = "请上传pdf类型文档" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                httpfile.SaveAs(uploadsPath + exn);
                LogHelper.Info(Masterpage.CurrUser.alias, "803032:客户," + Masterpage.CurrUser.client_code + ",上传知识库文档,路径为：" + uploadsPath + exn);
                r = new ReturnValue { status = "ok", value = backpath + "/" + identifier.ToString() + exn, value2 = identifier.ToString() + exn };
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "803032:客户," + Masterpage.CurrUser.client_code + ",上传知识库文档失败，未添加文档");
                r = new ReturnValue { status = "error", message = "未添加文档" };
            } 
            return Content(JsonHelper.ToJson(r));
            //return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult knowedit(string t, Guid guid)
        {
            ReturnValue r = new ReturnValue();
            KnewledgeBase one = new KnewledgeBase();
            var category = diagnosticService.Knowledgebasecategory();
            var units = diagnosticService.GetStandardProcessUnit();
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                if (!Masterpage.CurrUser.IsExpert)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "803033:客户," + Masterpage.CurrUser.client_code + ",编辑知识库" + guid + "信息失败，权限不足");
                    r = new ReturnValue { status = "error", message = "权限不足!" };
                }
                else
                {
                    if (t == "edit") one = diagnosticService.GetOneKnewledgeBase(guid);
                    r = new ReturnValue { status = "ok" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "803033:客户," + Masterpage.CurrUser.client_code + ",编辑知识库" + guid + "信息");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "803033:客户," + Masterpage.CurrUser.client_code + ",编辑知识库" + guid + "信息失败，" + ex.Message);
                r = new ReturnValue { status = "error", message = "读取失败!" };
            }
            data.category = category;
            data.units = units;
            data.r = r;
            data.one = one;
            data.guid = guid;
            data.t = t;
            return PartialView("knowedit", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult faqview(string sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            FAQ one = new FAQ();
            string type = "";
            try
            {
                one = diagnosticService.GetOneFAQ(Convert.ToInt32(sn));
                var category = diagnosticService.Faqunitcategory();
                if (one.FaqSn > 0)
                {
                    var h = category.FirstOrDefault(p => p.value == one.FaqCategory);
                    if (h != null) type = h.text;
                    r.message = "";
                    r.status = "ok";
                    LogHelper.Info(Masterpage.CurrUser.alias, "803023:客户," + Masterpage.CurrUser.client_code + ",查看FAQ信息，对应sn:" + sn);
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "803023:客户," + Masterpage.CurrUser.client_code + ",查看FAQ信息失败，对应sn:" + sn + ",读取不存在");
                    r.message = "读取不存在";
                    r.status = "error";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "803023:客户," + Masterpage.CurrUser.client_code + ",查看FAQ信息失败，对应sn:" + sn + ex.Message);
                r.message = "读取失败";
                r.status = "error";
            }
            data.one = one;
            data.type = type;
            data.r = r;
            return PartialView("faqview", data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult knowlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var type = WebRequest.GetString("type", true);
            var list = diagnosticService.GetKnewledgeBase(type, key);
            var units = diagnosticService.GetStandardProcessUnit();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 13;
            var vs = list.ToPagedList(_page, _pagesize);
            data.key = key;
            data.type = type;
            data.units = units;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            int expert = Masterpage.CurrUser.IsExpert ? 1 : 0;
            data.expert = expert;
            LogHelper.Info(Masterpage.CurrUser.alias, "803031:客户," + Masterpage.CurrUser.client_code + ",知识库列表,第" + _page + "页");
            return PartialView("knowlist", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult knowview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r = new ReturnValue();
            KnewledgeBase one = new KnewledgeBase();
            string type = "";
            string unitname = "";
            string filepath = "";
            try
            {
                one = diagnosticService.GetOneKnewledgeBase(Guid.Parse(guid));
                var category = diagnosticService.Knowledgebasecategory();
                if (one != null)
                {
                    bool f = FileHelper.IsFileExist(ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/knewledgebase/" + one.KnowledgeBaseUrl);
                    if (f)
                    {
                        var h = category.FirstOrDefault(p => p.value == one.KnowledgeBaseCategory);
                        if (h != null) type = h.text;
                        diagnosticService.ReadOneKnewledge(Guid.Parse(guid));
                        if (one.StandardProcessUnit != null) unitname = one.StandardProcessUnit.StandardProcessUnitName;
                        filepath = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/knewledgebase/" + one.KnowledgeBaseUrl;
                        r.message = "";
                        r.status = "ok";
                        LogHelper.Info(Masterpage.CurrUser.alias, "803034:客户," + Masterpage.CurrUser.client_code + ",查看知识库信息,对应guid为：" + guid + ",访谈录路径为：" + filepath);
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "803034:客户," + Masterpage.CurrUser.client_code + ",查看知识库信息,对应guid为：" + guid + "失败,文件不存在，" + filepath);
                        r.message = "文件不存在";
                        r.status = "error";
                    }
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "803034:客户," + Masterpage.CurrUser.client_code + ",查看知识库信息,对应guid为：" + guid + "失败,读取失败，" + filepath);
                    r.message = "读取失败";
                    r.status = "error";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "803034:客户," + Masterpage.CurrUser.client_code + ",查看知识库信息,对应guid为：" + guid + "失败,读取失败，" + filepath + ex.Message);
                r.message = "读取失败";
                r.status = "error";
            }
            data.unitname = unitname;
            data.filepath = filepath;
            data.one = one;
            data.type = type;
            data.r = r;
            return PartialView("knowview", data);
        }


        #region faq 知识库管理
        [ValidateInput(false)]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult faqsave(string type, int? sn, string content)
        {
            var title = WebRequest.GetString("title", true);
            var cate = WebRequest.GetString("cate", true);
            var keyword = WebRequest.GetString("keyword", true);
            //var content = WebRequest.GetString("content", false);
            var r = diagnosticService.SaveFAQ(type, sn.Value, title, content, cate, keyword);
            LogHelper.Info(Masterpage.CurrUser.alias, "803024:客户," + Masterpage.CurrUser.client_code + ",保存FAQ，操作类别：" + type + ",sn:" + sn + "，类别：" + cate + "，关键字：" + keyword + "，内容：" + content + ",结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult knowsave(string type, Guid guid)
        {
            var title = WebRequest.GetString("title", true);
            var cate = WebRequest.GetString("cate", true);
            var keyword = WebRequest.GetString("keyword", true);
            var unit = WebRequest.GetString("unit", false);
            var content = WebRequest.GetString("url", false);
            var r = diagnosticService.SaveKnewledge(type, guid, title, unit, content, cate, keyword);
            LogHelper.Info(Masterpage.CurrUser.alias, "803035:客户," + Masterpage.CurrUser.client_code + ",保存知识库，操作类别：" + type + ",guid:" + guid + "，类别：" + cate + "，关键字：" + keyword + "，内容：" + content + ",结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult delfaq(int? sn)
        {
            bool r = diagnosticService.DeleteFAQ(sn.Value);
            ReturnValue rv = new ReturnValue { message = "删除失败", status = r ? "ok" : "error" };
            LogHelper.Info(Masterpage.CurrUser.alias, "803025:客户," + Masterpage.CurrUser.client_code + ",删除FAQ：" + sn + ",结果：" + rv.status);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult delknow(Guid guid)
        {
            bool r = diagnosticService.DeleteKnewledge(guid);
            ReturnValue rv = new ReturnValue { message = "删除失败", status = r ? "ok" : "error" };
            LogHelper.Info(Masterpage.CurrUser.alias, "803036:客户," + Masterpage.CurrUser.client_code + ",删除知识库：" + guid + ",结果：" + rv.status);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult postlist(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var list = diagnosticService.GetInteractivePlatform(key);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.key = key;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            LogHelper.Info(Masterpage.CurrUser.alias, "804011:客户," + Masterpage.CurrUser.client_code + ",在线答疑列表，第" + _page + "页");
            return PartialView("postlist", data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult createpost()
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            LogHelper.Info(Masterpage.CurrUser.alias, "804012:客户," + Masterpage.CurrUser.client_code + ",创建在线答疑");
            return PartialView("createpost", data);
        }

        [ValidateInput(false)]
        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult savepost(string content)
        {
            var title = WebRequest.GetString("title", true);
            var forid = WebRequest.GetString("forid", true);
            var rootid = WebRequest.GetString("rootid", true);
            ReturnValue r;
            try
            {
                if (forid != "" && forid != "0" && rootid != "" && rootid != "0")
                {
                    int id = Convert.ToInt32(forid);
                    int rid = Convert.ToInt32(rootid);
                    if (id == rid)
                        r = diagnosticService.ReplyInteractivePlatform(id, title, content, Masterpage.CurrUser.alias);
                    else
                        r = diagnosticService.ReplyInteractivePlatform(rid, id, title, content, Masterpage.CurrUser.alias);
                }
                else
                    r = diagnosticService.CreateInteractivePlatform(title, content, Masterpage.CurrUser.alias);

                LogHelper.Info(Masterpage.CurrUser.alias, "804013:客户," + Masterpage.CurrUser.client_code + ",保存在线答疑，标题：" + title + "，内容:" + content + ",结果:" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "804013:客户," + Masterpage.CurrUser.client_code + ",保存在线答疑失败，标题：" + title + "，内容:" + content + ",结果:" + ex.Message);

                r = new ReturnValue { message = "程序异常", status = "error" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "selfdiag", ForController = "diagnostic")]
        public ActionResult postview(string sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            InteractivePlatformModel one = new InteractivePlatformModel();
            try
            {
                one = diagnosticService.GetOneInteractivePlatformModel(Convert.ToInt32(sn));

                if (one.InteractivePlatformId > 0)
                {
                    r.message = "";
                    r.status = "ok";
                    LogHelper.Info(Masterpage.CurrUser.alias, "804014:客户," + Masterpage.CurrUser.client_code + ",查看在线答疑，标题：" + one.InteractivePlatformTitle);
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "804014:客户," + Masterpage.CurrUser.client_code + ",查看在线答疑失败，" + sn + "提问不存在");
                    r.message = "提问不存在";
                    r.status = "error";
                }
            }
            catch
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "804014:客户," + Masterpage.CurrUser.client_code + ",查看在线答疑失败，" + sn + "读取失败");
                r.message = "读取失败";
                r.status = "error";
            }
            data.one = one;
            data.r = r;
            return PartialView("postview", data);
        }

        #endregion
    }
}
