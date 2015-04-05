using ecoBio.Wms.Repositories;
using ecoBio.Wms.Service.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.Common;
using ecoBio.Wms.Data.Entities.Models;
using ecoBio.Wms.ViewModel;
using System.IO;
using System.Configuration;
using ecoBio.Wms.ExcelRead;
using ecoBio.Wms.FlexData;
using System.Diagnostics;


namespace ecoBio.Wms.Web.Controllers
{
    public class managementController : BaseController
    {
        private ManagementService managementService;
        private AccountService accountService;
        public managementController(IManagementRepository _managementRepository, IAccountRepository _accountRepository)
        {
            managementService = new ManagementService(_managementRepository);
            accountService = new AccountService(_accountRepository);
        }


        #region 设备管理
        public ActionResult equipment(int? page, int? pagesize, string unit, string number)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (unit == null) unit = ""; if (number == null) number = "";
            var list = managementService.GetCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, unit, number);
            //var paramlist = managementService.GetCustomerEquipmentSpecsParameters().ToList();

            var ddl = managementService.GetStandardProcessUnitDDL(Masterpage.CurrUser.client_code, unit);
            var cates = managementService.GetEquipmentCategory();
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.unit = unit;
            data.cates = cates;
            data.number = number;
            data.ddl = ddl;
            data.list = vs;
            //data.paramlist = paramlist;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            if (number != "") otherparam += "&number=" + number;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "601011:客户," + Masterpage.CurrUser.client_code + ",设备列表,第" + _page + "页");
            return View(data);
        }
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult equipmentview(Guid guid)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            CustomerEquipmentSpecModel one = new CustomerEquipmentSpecModel();
            var paramlist = managementService.GetCustomerEquipmentSpecsParameters().ToList();
            try
            {
                one = managementService.GetOneCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, guid);
                LogHelper.Info(Masterpage.CurrUser.alias, "601012:客户," + Masterpage.CurrUser.client_code + ",设备信息查看,设备位号" + one.customer_equipment_position_number + ",设备规格：" + one.equpment_spec_model);
            }
            catch (Exception ex)
            {
                r = new ReturnValue { status = "error", message = "查找！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "601012:客户," + Masterpage.CurrUser.client_code + ",设备信息查看,设备读取失败" + ex.Message);
            }
            data.paramlist = paramlist;
            data.r = r;
            data.one = one;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult repairlist(int? page, int? pagesize)
        {
            string equipment = WebRequest.GetString("equipment", true);
            string sn = WebRequest.GetString("sn", true);
            string number = WebRequest.GetString("number", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g;
            try { g = Guid.Parse(equipment); }
            catch { return RedirectToAction("equipment"); }
            var list = managementService.GetCustomerEquipmentRepairs(g, sn, number, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.sn = sn;
            data.customer = Masterpage.CurrUser.client_code;
            data.number = number;
            data.device = equipment;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            //data.ddltype = managementService.GetRepairType();
            //data.ddllevel = managementService.GetFaultLevel();
            string otherparam = ""; otherparam += "&equipment=" + equipment;
            if (sn != "") otherparam += "&sn=" + sn;
            if (number != "") otherparam += "&number=" + number;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "601021:客户," + Masterpage.CurrUser.client_code + ",设备" + equipment + ",维修列表，第" + _page + "页");
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult repairpage(string device, string guid, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerEquipmentRepair one = new CustomerEquipmentRepair();
            string number = "";
            string person = Masterpage.CurrUser.alias;
            string type = t;
            string devicenumber = "";
            Guid deviceguid;
            try { deviceguid = Guid.Parse(device); }
            catch { return RedirectToAction("equipment"); }

            var d = managementService.GetOneCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, deviceguid);
            devicenumber = d.customer_equipment_position_number;
            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try { g = Guid.Parse(guid); }
                catch { return RedirectToAction("equipment"); }
                one = managementService.GetOneCustomerEquipmentRepair(g);
                data.hv = 1;
                number = one.CustomerEquipmentRepairNum;
                person = one.CustomerEquipmentRepairPerson;
                LogHelper.Info(Masterpage.CurrUser.alias, "601022:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",查看维修信息，维修guid" + guid);
            }
            else
            {
                number = managementService.CreatRepairNumber(Masterpage.CurrUser.client_code).Replace(Masterpage.CurrUser.client_code + "-", "");
                data.hv = 0;
                one = new CustomerEquipmentRepair();
                LogHelper.Info(Masterpage.CurrUser.alias, "601022:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",新增维修信息");
            }
            data.device = device;
            data.guid = guid;
            data.person = person;
            data.devicenumber = devicenumber;
            data.number = number;
            //data.ddltype = managementService.GetRepairType();
            //data.ddllevel = managementService.GetFaultLevel();
            data.one = one;
            data.type = t;
            data.images = new List<ecoBio.Wms.ViewModel.KeyValue>();
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult saverepair()
        {
            string device = WebRequest.GetString("device", true);
            string guid = WebRequest.GetString("guid", true);
            string type = WebRequest.GetString("type", true);
            string p = WebRequest.GetString("p", true);
            string date = WebRequest.GetString("date", true);
            string repairtype = WebRequest.GetString("repairtype", true);
            string ddllevel = WebRequest.GetString("ddllevel", true);
            string content = WebRequest.GetString("content", true);
            string fault = WebRequest.GetString("fault", true);
            string number = WebRequest.GetString("number", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            if (type == "add" && device != "")
            {
                Guid g = Guid.Parse(device);
                r = managementService.AddCustomerEquipmentRepair(Masterpage.CurrUser.client_code, g, date, p, ddllevel, fault, repairtype, content);
            }
            else if (type == "edit" && guid != "" && device != "")
            {
                Guid g1 = Guid.Parse(device);
                Guid g2 = Guid.Parse(guid);
                r = managementService.UpdateCustomerEquipmentRepair(Masterpage.CurrUser.client_code, g2, g1, date, p, ddllevel, fault, repairtype, content);
            }
            if (r.status == "ok")
            {
                //try
                //{
                //    #region 生成报表
                //    var d = managementService.GetOneCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, Guid.Parse(device));
                //    createrepair(d.equpment_spec_name, d.equipment_category, d.customer_equipment_position_number, d.supplier_name, d.equipment_unit, d.image,
                //number, DateTime.Now.ToString("yyyy-MM-dd"),p, date, content, fault);
                //    #endregion
                //}
                //catch (Exception e)
                //{
                //    r.status = "error";
                //    r.message = "记录添加成功，报表生成失败，原因：" + e.Message;

                //}
            }
            LogHelper.Info(Masterpage.CurrUser.alias, "601023:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",保存维修信息，维修guid" + guid + "，操作类别：" + type + "，日期：" + date + "，内容：" + content + "，问题描述：" + fault + "，单号：" + number + "操作结果：" + r.status);
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult maintenancelist(int? page, int? pagesize)
        {
            string equipment = WebRequest.GetString("equipment", true);
            string sn = WebRequest.GetString("sn", true);
            string number = WebRequest.GetString("number", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g;
            try { g = Guid.Parse(equipment); }
            catch { return RedirectToAction("equipment"); }
            var list = managementService.GetCustomerEquipmentMaintenances(g, sn, number, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.sn = sn;
            data.customer = Masterpage.CurrUser.client_code;
            data.number = number;
            data.device = equipment;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            // data.ddltype = managementService.GetMaintenanceStyle();
            string otherparam = "";
            otherparam += "&equipment=" + equipment;
            if (sn != "") otherparam += "&sn=" + sn;
            if (number != "") otherparam += "&number=" + number;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "601031:客户," + Masterpage.CurrUser.client_code + ",设备" + equipment + ",保养列表，第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult maintenancepage(string device, string guid, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerEquipmentMaintenance one = new CustomerEquipmentMaintenance();
            string number = "";
            string person = Masterpage.CurrUser.alias;
            string type = t;
            string devicenumber = "";
            Guid deviceguid;
            try { deviceguid = Guid.Parse(device); }
            catch { return RedirectToAction("equipment"); }

            var d = managementService.GetOneCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, deviceguid);
            devicenumber = d.customer_equipment_position_number;
            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try { g = Guid.Parse(guid); }
                catch { return RedirectToAction("equipment"); }
                one = managementService.GetOneCustomerEquipmentMaintenance(g);
                data.hv = 1;
                number = one.CustomerEqupmentMaintenanceNo;
                person = one.CustomerEquipmentMaintenancePerson;
                LogHelper.Info(Masterpage.CurrUser.alias, "601032:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",查看保养信息，保养guid" + guid);
            }
            else
            {
                number = managementService.CreatMaintenanceNumber(Masterpage.CurrUser.client_code).Replace(Masterpage.CurrUser.client_code + "-", "");
                data.hv = 0;
                one = new CustomerEquipmentMaintenance();
                LogHelper.Info(Masterpage.CurrUser.alias, "601032:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",新增保养信息");
            }
            data.device = device;
            data.guid = guid;
            data.person = person;
            data.devicenumber = devicenumber;
            data.number = number;
            data.ddltype = managementService.GetMaintenanceStyle();
            data.one = one;
            data.type = t;
            data.images = new List<ecoBio.Wms.ViewModel.KeyValue>();
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult savemaintenance()
        {
            string device = WebRequest.GetString("device", true);
            string guid = WebRequest.GetString("guid", true);
            string type = WebRequest.GetString("type", true);
            string p = WebRequest.GetString("p", true);
            string date = WebRequest.GetString("date", true);
            string repairtype = WebRequest.GetString("repairtype", true);
            string content = WebRequest.GetString("content", true);
            string number = WebRequest.GetString("number", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            if (type == "add" && device != "")
            {
                Guid g = Guid.Parse(device);
                r = managementService.AddCustomerEquipmentMaintenance(Masterpage.CurrUser.client_code, g, date, p, repairtype, content);
            }
            else if (type == "edit" && guid != "" && device != "")
            {
                Guid g1 = Guid.Parse(device);
                Guid g2 = Guid.Parse(guid);
                r = managementService.UpdateCustomerEquipmentMaintenance(Masterpage.CurrUser.client_code, g2, g1, date, p, repairtype, content);
            }
            if (r.status == "ok")
            {
                //try
                //{
                //    #region 生成报表
                //    var d = managementService.GetOneCustomerEquipmentSpecs(Masterpage.CurrUser.client_code, Guid.Parse(device));
                //    createmaintenance(d.equpment_spec_name, d.equipment_category, d.customer_equipment_position_number, d.supplier_name, d.equipment_unit, d.image,
                //number, DateTime.Now.ToString("yyyy-MM-dd"), p, date, content);
                //    #endregion
                //}
                //catch (Exception e)
                //{
                //    r.status = "error";
                //    r.message = "记录添加成功，报表生成失败，原因：" + e.Message;

                //}
            }
            #endregion
            LogHelper.Info(Masterpage.CurrUser.alias, "601033:客户," + Masterpage.CurrUser.client_code + ",设备" + device + ",保存保养信息，保养guid" + guid + "，操作类别：" + type + "，日期：" + date + "，内容：" + content + "，单号：" + number + "操作结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public void createrepair(string s13, string s111, string s23, string s211, string s33, string s311,
            string s63, string s611, string s73, string s711, string s103, string s113)
        {
            //string path = "~/res/" + Masterpage.CurrUser.client_code + "/files/repair/" + s63;
            //FileHelper.CheckDirectory("~/res/" + Masterpage.CurrUser.client_code + "/files/repair");
            //bool exist = FileHelper.IsFileExist(path + ".pdf");
            //if (!exist)
            //{
            //    FileHelper.CreateRepairReport("~/res/public/templates/repair.xlsx", path + ".xlsx",
            //          s13, s111, s23, s211, s33, s311,
            // s63, s611, s73, s711, s103, s113);
            //    OfficeHelper.XLSConvertToPDF(path + ".xlsx", path + ".pdf");
            //    FileHelper.DeleteFile(path + ".xlsx");
            //}
        }

        public void createmaintenance(string s13, string s111, string s23, string s211, string s33, string s311,
           string s63, string s611, string s73, string s711, string s103)
        {
            //string path = "~/res/" + Masterpage.CurrUser.client_code + "/files/maintenance/" + s63;
            //FileHelper.CheckDirectory("~/res/" + Masterpage.CurrUser.client_code + "/files/maintenance");
            //bool exist = FileHelper.IsFileExist(path + ".pdf");
            //if (!exist)
            //{
            //    FileHelper.CreateMaintenanceReport("~/res/public/templates/maintenance.xlsx", path + ".xlsx",
            //          s13, s111, s23, s211, s33, s311,
            // s63, s611, s73, s711, s103);
            //    OfficeHelper.XLSConvertToPDF(path + ".xlsx", path + ".pdf");
            //    FileHelper.DeleteFile(path + ".xlsx");
            //}
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult repairdown()
        {
            string no = WebRequest.GetString("no", true);
            #region 查找
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/repair/" + no + ".pdf";
                FileHelper.DownLoadFile(path, no + ".pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "601024:客户," + Masterpage.CurrUser.client_code + ",设备维修单" + no + ",维修单报表下载");
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "601025:客户," + Masterpage.CurrUser.client_code + ",设备维修单" + no + ",维修单报表下载失败" + ex.Message);
                return Content("文件不存在");
            }
            #endregion
            return View();
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult maintenancedown()
        {
            string no = WebRequest.GetString("no", true);
            #region 查找
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/maintenance/" + no + ".pdf";
                FileHelper.DownLoadFile(path, no + ".pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "601034:客户," + Masterpage.CurrUser.client_code + ",设备保养单" + no + ",保养单报表下载");
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "601035:客户," + Masterpage.CurrUser.client_code + ",设备保养单" + no + ",保养单报表下载失败" + ex.Message);
                return Content("文件不存在");
            }
            #endregion
            return View();
        }

        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult delmaintenance()
        {
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                bool b = managementService.DeleteCustomerEquipmentMaintenance(Masterpage.CurrUser.client_code, g);
                if (b) r = new ReturnValue { status = "ok" };
                else r = new ReturnValue { status = "error", message = "删除失败！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "601036:客户," + Masterpage.CurrUser.client_code + ",设备保养单" + guid + ",保养单删除" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "601036:客户," + Masterpage.CurrUser.client_code + ",设备保养单" + guid + ",保养删除失败，" + ex.Message);
                r = new ReturnValue { status = "error", message = "删除失败！" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult delrepair()
        {
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                bool b = managementService.DeleteCustomerEquipmentRepair(Masterpage.CurrUser.client_code, g);
                if (b) r = new ReturnValue { status = "ok" };
                else r = new ReturnValue { status = "error", message = "删除失败！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "601026:客户," + Masterpage.CurrUser.client_code + ",设备维修单" + guid + ",维修删除" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "601026:客户," + Masterpage.CurrUser.client_code + ",设备维修单" + guid + ",维修删除失败，" + ex.Message);
                r = new ReturnValue { status = "error", message = "删除失败！" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [Anonymous]
        public ActionResult pdfrepair(string para)
        {
            //para='+$customer+'_'+$logo+'_'+$inno+'_'+$indate+'_'+$person+'_'+$insn+'_'+$detailstr
            //dgklsz_1B2FDAC4-FD68-41FD-84B8-AB4EF4C2F171_D5E5DAB8-DF5E-48FD-B95E-1E26BB7A1C72_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg
            //string 客户号0, string logo图片1, string 入库单号2, string 入库日期3, string 入库人4, string 入库流水号5, string 入库单明细6
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerEquipmentSpecModel device = new CustomerEquipmentSpecModel();
            CustomerEquipmentRepair repair = new CustomerEquipmentRepair();
            string logo = "";
            try
            {
                string[] t = para.Split('_');
                string code = t[0];
                Guid dguid = Guid.Parse(t[1]);
                Guid rguid = Guid.Parse(t[2]);
                device = managementService.GetOneCustomerEquipmentSpecs(code, dguid);
                repair = managementService.GetOneCustomerEquipmentRepair(rguid);
                logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + t[3];
            }
            catch
            {
            }
            data.device = device;
            data.repair = repair;
            data.logo = logo;
            return View(data);
        }
        [Anonymous]
        public ActionResult pdfmaintenance(string para)
        {
            //para='+$customer+'_'+$logo+'_'+$inno+'_'+$indate+'_'+$person+'_'+$insn+'_'+$detailstr
            //dgklsz_1B2FDAC4-FD68-41FD-84B8-AB4EF4C2F171_D5E5DAB8-DF5E-48FD-B95E-1E26BB7A1C72_3a659635-d7a2-4a0a-b78d-ae0a861a8a64.jpg
            //string 客户号0, string logo图片1, string 入库单号2, string 入库日期3, string 入库人4, string 入库流水号5, string 入库单明细6
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerEquipmentSpecModel device = new CustomerEquipmentSpecModel();
            CustomerEquipmentMaintenance repair = new CustomerEquipmentMaintenance();
            string logo = "";
            try
            {
                string[] t = para.Split('_');
                string code = t[0];
                Guid dguid = Guid.Parse(t[1]);
                Guid rguid = Guid.Parse(t[2]);
                device = managementService.GetOneCustomerEquipmentSpecs(code, dguid);
                repair = managementService.GetOneCustomerEquipmentMaintenance(rguid);
                logo = ConfigurationManager.AppSettings["VirtualRes"] + t[0] + "/images/" + t[3];
            }
            catch
            {
            }
            data.device = device;
            data.repair = repair;
            data.logo = logo;
            return View(data);
        }


        #region 维修保养查看
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult repairview(int? page, int? pagesize)
        {
            string equipment = WebRequest.GetString("equipment", true);
            string sn = WebRequest.GetString("sn", true);
            string number = WebRequest.GetString("number", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g;
            try { g = Guid.Parse(equipment); }
            catch { g = Guid.Empty; }
            var list = managementService.GetCustomerEquipmentRepairs(g, sn, number, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.sn = sn;
            data.customer = Masterpage.CurrUser.client_code;
            data.number = number;
            data.device = equipment;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = ""; otherparam += "&equipment=" + equipment;
            if (sn != "") otherparam += "&sn=" + sn;
            if (number != "") otherparam += "&number=" + number;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "601021:客户," + Masterpage.CurrUser.client_code + ",设备" + equipment + ",维修列表，第" + _page + "页");
            return PartialView(data);
        }

        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult maintenanceview(int? page, int? pagesize)
        {
            string equipment = WebRequest.GetString("equipment", true);
            string sn = WebRequest.GetString("sn", true);
            string number = WebRequest.GetString("number", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            Guid g;
            try { g = Guid.Parse(equipment); }
            catch { g = Guid.Empty; }
            var list = managementService.GetCustomerEquipmentMaintenances(g, sn, number, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.sn = sn;
            data.customer = Masterpage.CurrUser.client_code;
            data.number = number;
            data.device = equipment;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            // data.ddltype = managementService.GetMaintenanceStyle();
            string otherparam = "";
            otherparam += "&equipment=" + equipment;
            if (sn != "") otherparam += "&sn=" + sn;
            if (number != "") otherparam += "&number=" + number;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "601031:客户," + Masterpage.CurrUser.client_code + ",设备" + equipment + ",保养列表，第" + _page + "页");
            return PartialView(data);
        }
        #endregion
        #endregion

        #region 构筑物管理
        public ActionResult structures(int? page, int? pagesize, string unit, string number)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (unit == null) unit = ""; if (number == null) number = "";
            var list = managementService.GetCustomerConstructions(Masterpage.CurrUser.client_code, unit, number);
            var paramlist = managementService.GetCustomerConstructionsParameters(Masterpage.CurrUser.client_code).ToList();
            var ddl = managementService.GetStandardProcessUnitDDL(Masterpage.CurrUser.client_code, unit);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.unit = unit;
            data.number = number;
            data.ddl = ddl;
            data.list = vs;
            data.paramlist = paramlist;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            if (number != "") otherparam += "&number=" + number;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "602011:客户," + Masterpage.CurrUser.client_code + ",构筑物列表");
            return View(data);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult structuresone(string number)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            CustomerConstructionModel item = new CustomerConstructionModel();
            List<ParameterValue> paramlist = new List<ParameterValue>();
            #region 读取
            try
            {
                item = managementService.GetCustomerConstructions(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.customer_construction_position_number == number);
                paramlist = managementService.GetCustomerConstructionsParameters(Masterpage.CurrUser.client_code).ToList();
                if (item == null)
                {
                    r = new ReturnValue { status = "error", message = "构筑物不存在" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "602012:客户," + Masterpage.CurrUser.client_code + ",查看构筑物信息，位号：" + number + ",构筑物不存在");
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "602012:客户," + Masterpage.CurrUser.client_code + ",查看构筑物信息，位号：" + number);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "602012:客户," + Masterpage.CurrUser.client_code + ",查看构筑物信息，位号：" + number + "," + ex.Message);
                r = new ReturnValue { status = "error", message = "构筑物不存在" };
            }
            #endregion
            data.r = r;
            data.item = item;
            data.paramlist = paramlist;
            return View(data);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult transformlist(int? page, int? pagesize)
        {
            string number = WebRequest.GetString("number", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = managementService.GetConstructionRemake(Masterpage.CurrUser.client_code, number, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.number = number;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = ""; otherparam += "&number=" + number;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "602021:客户," + Masterpage.CurrUser.client_code + ",查看构筑物改造，位号：" + number + ",改造列表，第" + _page + "页");
            return View(data);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult transformview(int? page, int? pagesize)
        {
            string number = WebRequest.GetString("number", true);
        
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = managementService.GetConstructionRemake(Masterpage.CurrUser.client_code, number);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.number = number;       
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = ""; otherparam += "&number=" + number;     
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "602021:客户," + Masterpage.CurrUser.client_code + ",查看构筑物改造，位号：" + number + ",改造列表，第" + _page + "页");
            return PartialView(data);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult transformpage(string number, string type, int? sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            CustomerConstructionRemake one = new CustomerConstructionRemake();
            ReturnValue r = new ReturnValue();
            string person = Masterpage.CurrUser.alias;
            try
            {
                if (type == "edit" || type == "show")
                {
                    one = managementService.GetOneConstructionRemake(Masterpage.CurrUser.client_code, number, sn.Value);
                    if (one == null)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "602022:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",改造sn" + sn + "改造记录不存在");
                        r = new ReturnValue { status = "error", message = "改造记录不存在" };
                    }
                    else
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "602022:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",查看改造信息，sn" + sn);
                    }
                    person = one.RemakeConstracter;
                }
                r = new ReturnValue { status = "ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "602022:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",改造sn" + sn + ex.Message);
                r = new ReturnValue { status = "error", message = "读取失败" };
            }
            data.person = person;
            data.sn = sn;
            data.r = r;
            data.number = number;
            data.one = one;
            data.type = type;
            data.images = new List<ecoBio.Wms.ViewModel.KeyValue>();
            return View(data);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult savetransform()
        {
            string number = WebRequest.GetString("number", true);
            string sn = WebRequest.GetString("sn", true);
            string type = WebRequest.GetString("type", true);
            string p = WebRequest.GetString("p", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            string content = WebRequest.GetString("content", true);
            string note = WebRequest.GetString("note", true);
            #region  类别
            ReturnValue r = new ReturnValue();
            if (type == "add")
            {
                r = managementService.AddConstructionRemake(Masterpage.CurrUser.client_code, number, content, start, end, p, note);
            }
            else if (type == "edit" && sn != "")
            {
                int g = int.Parse(sn);
                r = managementService.UpdateConstructionRemake(Masterpage.CurrUser.client_code, g, number, content, start, end, p, note);
            }
            #endregion
            LogHelper.Info(Masterpage.CurrUser.alias, "602023:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",保存改造信息，操作类别：" + type + "，人员：" + p + "，日期：" + start + end + "，内容：" + content + "，备注：" + note + "，结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "structures", ForController = "management")]
        public ActionResult deltransform()
        {
            string sn = WebRequest.GetString("sn", true);
            string number = WebRequest.GetString("number", true);
            ReturnValue r;
            try
            {
                int g = int.Parse(sn);
                bool b = managementService.DeleteConstructionRemake(number, g);
                if (b) r = new ReturnValue { status = "ok" };
                else r = new ReturnValue { status = "error", message = "删除失败！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "602024:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",删除改造信息，sn：" + sn + ",操作结果：" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "602024:客户," + Masterpage.CurrUser.client_code + ",构筑物改造，位号：" + number + ",删除改造信息，sn：" + sn + ",删除失败：" + ex.Message);
                r = new ReturnValue { status = "error", message = "删除失败！" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SOP
        public ActionResult sop(int? page, int? pagesize)
        {
            string unit = WebRequest.GetString("unit", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetCustomerSOP(Masterpage.CurrUser.client_code, unit, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.unit = unit;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.ddlunit = managementService.GetStandardProcessUnitDDL(Masterpage.CurrUser.client_code);
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "603011:客户," + Masterpage.CurrUser.client_code + ",SOP列表,第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "sop", ForController = "management")]
        public ActionResult sopview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string path = "";
            string name = "";
            int find = 0;
            string guid = WebRequest.GetString("guid", true);
            try
            {
                Guid g = Guid.Parse(guid);
                var one = managementService.GetOneCustomerSOP(Masterpage.CurrUser.client_code, g);
                if (one != null)
                {
                    //path = one.CustomerSopPath;
                    var file = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                    if (FileHelper.IsFileExist(file))
                    {
                        find = 1;
                        path = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看SOP详情,file" + file);
                    }
                    else
                    {
                        find = 0;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看SOP详情,file" + file + "，SOP不存在");
                    }
                    name = guid + ".pdf";
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + path + "，SOP不存在");
                    find = 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + ex.Message);
                data.find = 0;
            }
            data.find = find;
            data.name = name;
            data.path = path;
            return View(data);
        }
        [AjaxAction(ForAction = "sop", ForController = "management")]
        public ActionResult sopview2()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string path = "";
            string name = "";
            int find = 0;
            string guid = WebRequest.GetString("guid", true);
            try
            {
                Guid g = Guid.Parse(guid);
                var one = managementService.GetOneCustomerSOP(Masterpage.CurrUser.client_code, g);
                if (one != null)
                {
                    //path = one.CustomerSopPath;
                    var file = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                    if (FileHelper.IsFileExist(file))
                    {
                        find = 1;
                        path = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看SOP详情,file" + file);
                    }
                    else
                    {
                        find = 0;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看SOP详情,file" + file + "，SOP不存在");
                    }
                    name = guid + ".pdf";
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + path + "，SOP不存在");
                    find = 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "603012:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + ex.Message);
                data.find = 0;
            }
            data.find = find;
            data.name = name;
            data.path = path;
            return View(data);
        }
        [AjaxAction(ForAction = "sop", ForController = "management")]
        public ActionResult sopview3()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            
            return View(data);
        }
        #endregion

        #region 操作日志
        public ActionResult operationlog(int? page, int? pagesize)
        {
            string person = WebRequest.GetString("person", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetOperationLog(Masterpage.CurrUser.client_code, person, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 13;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.person = person;
            data.code = Masterpage.CurrUser.client_code;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.ddlperson = managementService.GetUserDDL(Masterpage.CurrUser.client_code);
            data.ddlperson2 = managementService.GetUserDDL(Masterpage.CurrUser.client_code);
            string otherparam = "";
            if (person != "") otherparam += "&person=" + person;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "604011:客户," + Masterpage.CurrUser.client_code + ",操作日志列表,第" + _page + "页");
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult operationlogimg1()
        {
            // string imgpath = "~/Res/" + Masterpage.CurrUser.client_code + "/images/operationlog";
            string imgpath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/images/operationlog";
            string backpath = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/images/operationlog";
            //  string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(imgpath, identifier.ToString());
            var httpfile = Request.Files["mypic1"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".jpg" && exn.ToLower() != ".png")
                {
                    r = new ReturnValue { status = "error", message = "请上传jpg或png格式图片" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                httpfile.SaveAs(uploadsPath + exn);
                #region 图片自动裁剪
                if (exn.ToLower().Contains("jpg"))
                {
                    //string sourceFile = uploadsPath + exn;// Server.MapPath("~/Content/images/" + name);//源图存放目录
                    //string newFile = string.Empty; //新图路径
                    //string newNewDir = Server.MapPath("~/Content/pressimg/");   //新图存放目录
                    //newFile = Path.Combine(newNewDir, "正方形裁剪.jpg");
                    //ImageCutZoom.CutForSquare(sourceFile, newFile, 200, 90);
                    //newFile = Path.Combine(newNewDir, "180_240.jpg");
                    //ImageCutZoom.CutForCustom(sourceFile, newFile, 240, 180, 100);
                    //newFile = Path.Combine(newNewDir, "等比180_240.jpg");
                    //ImageCutZoom.ZoomAuto(sourceFile, newFile, 240, 180, "", "");
                }
                #endregion
                LogHelper.Info(Masterpage.CurrUser.alias, "604012:客户," + Masterpage.CurrUser.client_code + ",上传操作日志图片1," + uploadsPath + exn);
                r = new ReturnValue { status = "ok", value = backpath + "/" + identifier.ToString() + exn, value2 = identifier.ToString() + exn };
            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "604012:客户," + Masterpage.CurrUser.client_code + ",上传操作日志图片1失败,未添加图片");
                r = new ReturnValue { status = "error", message = "未添加图片" };
            }
            return Content(JsonHelper.ToJson(r));
            //return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult operationlogimg2()
        {
            string imgpath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/images/operationlog";
            string backpath = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/images/operationlog";
            // string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(imgpath, identifier.ToString());
            var httpfile = Request.Files["mypic2"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".jpg" && exn.ToLower() != ".png")
                {
                    r = new ReturnValue { status = "error", message = "请上传jpg或png格式图片" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                httpfile.SaveAs(uploadsPath + exn);
                #region 图片自动裁剪
                if (exn.ToLower().Contains("jpg"))
                {
                    //string sourceFile = uploadsPath + exn;// Server.MapPath("~/Content/images/" + name);//源图存放目录
                    //string newFile = string.Empty; //新图路径
                    //string newNewDir = Server.MapPath("~/Content/pressimg/");   //新图存放目录
                    //newFile = Path.Combine(newNewDir, "正方形裁剪.jpg");
                    //ImageCutZoom.CutForSquare(sourceFile, newFile, 200, 90);
                    //newFile = Path.Combine(newNewDir, "180_240.jpg");
                    //ImageCutZoom.CutForCustom(sourceFile, newFile, 240, 180, 100);
                    //newFile = Path.Combine(newNewDir, "等比180_240.jpg");
                    //ImageCutZoom.ZoomAuto(sourceFile, newFile, 240, 180, "", "");
                }
                #endregion
                LogHelper.Info(Masterpage.CurrUser.alias, "604012:客户," + Masterpage.CurrUser.client_code + ",上传操作日志图片2," + uploadsPath + exn);
                r = new ReturnValue { status = "ok", value = backpath + "/" + identifier.ToString() + exn, value2 = identifier.ToString() + exn };

            }
            else
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "604012:客户," + Masterpage.CurrUser.client_code + ",上传操作日志图片2失败,未添加图片");
                r = new ReturnValue { status = "error", message = "未添加图片" };
            }
            return Content(JsonHelper.ToJson(r)); 
            //return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult operationlogpage()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.ddlperson = managementService.GetUserDDL(Masterpage.CurrUser.client_code);
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult saveoperationlog()
        {
            string img1 = WebRequest.GetString("img1", true);
            string img2 = WebRequest.GetString("img2", true);

            string p = WebRequest.GetString("p", true);
            string date = WebRequest.GetString("date", true);

            string t = WebRequest.GetString("t", true);
            string sn = WebRequest.GetString("sn", true);

            string content = WebRequest.GetString("content", true);
            content = content.Replace("\n", "<br />");
            ReturnValue r = new ReturnValue();
            if (t == "add")
                r = managementService.AddOperationLog(Masterpage.CurrUser.client_code, Masterpage.CurrUser.alias, p, date, content, img1, img2);
            else if (t == "edit")
                r = managementService.UpdateOperationLog(Masterpage.CurrUser.client_code, int.Parse(sn), Masterpage.CurrUser.alias, p, date, content, img1, img2);
            LogHelper.Info(Masterpage.CurrUser.alias, "604013:客户," + Masterpage.CurrUser.client_code + ",保存操作日志,操作类别：" + t + "，人员：" + p + "，日期：" + date + "，内容：" + content + "，结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult operationlogview(int sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            OperationLog one = new OperationLog();
            #region 读取
            try
            {
                one = managementService.GetOneOperationLog(Masterpage.CurrUser.client_code, sn);
                if (one == null)
                {
                    r = new ReturnValue { status = "error", message = "日志不存在" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "604014:客户," + Masterpage.CurrUser.client_code + ",查看操作日志失败,操作日志sn：" + sn + "日志不存在");
                }
                else
                {
                    r = new ReturnValue { status = "ok" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "604014:客户," + Masterpage.CurrUser.client_code + ",查看操作日志,操作日志sn：" + sn);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "604014:客户," + Masterpage.CurrUser.client_code + ",查看操作日志失败,操作日志sn：" + sn + ex.Message);
                r = new ReturnValue { status = "error", message = "日志不存在" };
            }
            #endregion
            data.one = one;
            data.r = r;
            return View(data);
        }

        [AjaxAction(ForAction = "operationlog", ForController = "management")]
        public ActionResult deletelog(int sn)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            try
            {
                bool b = managementService.DeleteOperationLog(Masterpage.CurrUser.client_code, Convert.ToInt32(sn));
                if (!b) r = new ReturnValue { status = "error", message = "删除失败！" };
                else r = new ReturnValue { status = "ok", message = "" };
                LogHelper.Info(Masterpage.CurrUser.alias, "604015:客户," + Masterpage.CurrUser.client_code + ",删除操作日志,操作日志sn：" + sn + ",删除结果：" + r.status);
            }
            catch (Exception ex)
            {
                r = new ReturnValue { status = "error", message = "删除异常！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "604015:客户," + Masterpage.CurrUser.client_code + ",删除操作日志失败,操作日志sn：" + sn + ",异常：" + ex.Message);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 系统控制
        public ActionResult syscontrol()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var u = managementService.GetStandardProcessUnitDDL(Masterpage.CurrUser.client_code);
            //var p = managementService.GetProcessParameterDDL(Masterpage.CurrUser.client_code);
            data.units = u;
            //data.paras = p;
            LogHelper.Info(Masterpage.CurrUser.alias, "605011:客户," + Masterpage.CurrUser.client_code + ",系统控制");
            return View(data);
        }

        [AjaxAction(ForAction = "syscontrol", ForController = "management")]
        public ActionResult syscontrolprocess(int? page, int? pagesize)
        {
            string unit = WebRequest.GetString("unit", true);
            string para = WebRequest.GetString("para", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetCustomerProcessParameter(Masterpage.CurrUser.client_code, unit, para);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 13;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.unit = unit;
            data.para = para;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            if (para != "") otherparam += "&para=" + para;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "605012:客户," + Masterpage.CurrUser.client_code + ",系统控制工艺参数列表");
            return PartialView(data);
        }

        [AjaxAction(ForAction = "syscontrol", ForController = "management")]
        public ActionResult syscontrolswitch(int? page, int? pagesize)
        {
            string unit = WebRequest.GetString("unit", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            //var list = managementService.GetCustomerCollectionAlarmGroup(Masterpage.CurrUser.client_code, unit);
            var list = managementService.GetCustomerCollectionAlarmRes(Masterpage.CurrUser.client_code, unit);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.unit = unit;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "605013:客户," + Masterpage.CurrUser.client_code + ",系统控制客户采集点报警列表");
            return PartialView(data);
        }
        [AjaxAction(ForAction = "syscontrol", ForController = "management")]
        public ActionResult closecustomeralarm(int? sn)
        {
            var r = managementService.CloseCollectionAlarmRes(Masterpage.CurrUser.client_code, sn.Value);
            LogHelper.Info(Masterpage.CurrUser.alias, "605014:客户," + Masterpage.CurrUser.client_code + ",解除系统控制客户采集点报警，报警sn为" + sn.Value + "，操作结果:" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 服务周报
        public ActionResult weeklyservice(int? page, int? pagesize)
        {
            string month = WebRequest.GetQueryString("month", true);
            string week = WebRequest.GetQueryString("week", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var pathm = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" ;
            var virtualpathm = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" ;


            var pathw = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceweek/";
            var virtualpathw = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/serviceweek/";

            var pathd = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceday/";
            var virtualpathd = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/serviceday/";

            var list = managementService.GetServiceMonthList(pathm, virtualpathm); //月报
            var listw = managementService.GetServiceWeeklyList(pathw, virtualpathw);//周报
            var listd = managementService.GetServiceDaylyList(pathd, virtualpathd);//日报
           // List<PdfFileModel> list = 
            list.AddRange(listw);
            list.AddRange(listd);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.name = Masterpage.CurrUser.client_name;
            data.month = month;
            data.week = week;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (week != "") otherparam += "&week=" + week;
            if (month != "") otherparam += "&month=" + month;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "606011:客户," + Masterpage.CurrUser.client_code + ",服务报告列表");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult serviceweeklydown(string month)
        {
            #region 查找
            try
            {
                var path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + month + ".pdf";
                if (FileHelper.IsFileExist(path))
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "606012:客户," + Masterpage.CurrUser.client_code + ",服务报告下载，path：" + path);
                    FileHelper.DownLoadFile(path, Masterpage.CurrUser.client_name + "服务报告.pdf");
                    return Content("");
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "606012:客户," + Masterpage.CurrUser.client_code + ",服务报告下载文件不存在，path：" + path + "");
                    return Content("文件不存在");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "606012:客户," + Masterpage.CurrUser.client_code + ",服务报告下载失败" + ex.Message);
                return Content("下载失败");
            }
            #endregion
            //return View();
        }


        [HidNowLocal]
        [AjaxAction(ForAction = "weeklyservice,weeklyservicelist", ForController = "management")]
        public ActionResult weeklyserviceview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string path = "";
            int find = 0;
            string name = WebRequest.GetString("name", true);
            try
            {
                var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicereport/" + Masterpage.CurrUser.guid.ToString() + "/" + name;
                path = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/servicereport/" + Masterpage.CurrUser.guid.ToString() + "/" + name;
                if (FileHelper.IsFileExist(filepath)) find = 1;
                else find = 0;

            }
            catch
            {
                data.find = 0;
            }
            data.find = find;
            data.name = name;
            data.path = path;
            return View(data);
        }

        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult serviceday()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Customer one = managementService.GetCustomerInfo(Masterpage.CurrUser.client_code);
            string html = "";
            int find = 0;
            string week = WebRequest.GetString("day", true);
            string remark = "";
            string log = "";
            var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceday/" + week + ".txt";

            try
            {
                if (FileHelper.IsFileExist(filepath))
                {
                    find = 1;
                    html = FileHelper.ReadTxtFile(filepath);
                    var rs = html.IndexOf("$remarkstart$");
                    var re = html.IndexOf("$remarkend$");
                    var s4 = "";
                    if (rs != -1 && re != -1)
                    {
                        remark = html.Substring(rs + 13, re - rs - 13);
                        s4 = html.Substring(html.IndexOf("$remarkend$"));
                        html = html.Substring(0, rs) + s4.Replace("$remarkend$", "");
                    }
                    var rs2 = html.IndexOf("$logstart$");
                    var re2 = html.IndexOf("$logend$");
                    if (rs2 != -1 && re2 != -1)
                    {
                        log = html.Substring(rs2 + 10, re2 - rs2 - 10);
                        html = html.Substring(0, rs2);
                    }

                    LogHelper.Info(Masterpage.CurrUser.alias, "606016:客户," + Masterpage.CurrUser.client_code + ",服务日报查看，path：" + filepath);
                }
                else
                {
                    find = 0;
                    html = "不存在服务日报";
                    LogHelper.Info(Masterpage.CurrUser.alias, "606016:客户," + Masterpage.CurrUser.client_code + ",服务日报查看，path：" + filepath + "服务日报不存在");
                }

            }
            catch
            {
                data.find = 0;
                LogHelper.Info(Masterpage.CurrUser.alias, "606016:客户," + Masterpage.CurrUser.client_code + ",服务日报查看，path：" + filepath + "服务日报读取错误");
                html = "服务日报读取错误 ";
            }
            var nweek =DateTime.Now.ToString("yyyy-MM-dd");
            data.find = find;
            data.cansub = week == nweek.ToString();
            data.one = one;
            data.html = html;
            data.remark = remark;
            data.log = log;
            data.day = week;
            return View(data);
        }     
        /// <summary>
        /// 周报html
        /// </summary>
        /// <returns></returns>
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult servicehtml()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Customer one = managementService.GetCustomerInfo(Masterpage.CurrUser.client_code);
            string html = "";
            int find = 0;
            string week = WebRequest.GetString("week", true); 
            string remark = "";
            var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceweek/" + week+".txt";
                
            try
            {
                if (FileHelper.IsFileExist(filepath))
                {
                    find = 1;
                    html = FileHelper.ReadTxtFile(filepath);
                    var rs = html.IndexOf("$remarkstart$");
                    var re = html.IndexOf("$remarkend$");
                    if (rs != -1 && re != -1)
                    {
                        remark = html.Substring(rs + 13, re - rs - 13);
                        html = html.Substring(0, rs);
                    }
                    LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务周报查看，path：" + filepath);
                }
                else
                {
                    find = 0;
                    html = "不存在服务周报";
                    LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务周报查看，path：" + filepath + "服务周报不存在");
                }

            }
            catch
            {
                data.find = 0;
                LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务周报查看，path：" + filepath + "服务周报读取错误");
                html = "服务周报读取错误 ";
            }
            var nweek = Utils.GetWeekOfYear(DateTime.Now);
            data.find = find;
            data.cansub = week == nweek.ToString();
            data.one = one;
            data.html = html;
            data.remark = remark;
            data.week = week;
            return View(data);
        }     
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult servicemonth()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            Customer one = managementService.GetCustomerInfo(Masterpage.CurrUser.client_code);
            #region 结算日
            int day = 1;
            try
            {
                day = Convert.ToInt32(one.CustomerDtu);
            }
            catch
            {
                day = 1;
            }
            #endregion
            string html = "";
            string html1 = "";
            string html2 = "";
            string html3 = ""; 
            string html12 = "";
            string html22 = "";
            string html32 = "";
            string path = "";
            string remark = "";
            int find = 0;
            bool canremark=true;
            bool canpdf=false;
            string month = WebRequest.GetString("month", true);
            try
            {
                var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + month + ".txt";
                if (FileHelper.IsFileExist(filepath))
                {
                    #region 工艺部分

                    find = 1;
                    html = FileHelper.ReadTxtFile(filepath);
                    //$remarkstart$_$remarkend$
                    var rs = html.IndexOf("$remarkstart$");
                    var re = html.IndexOf("$remarkend$");
                    if (rs != -1 && re != -1)
                    {
                        remark = html.Substring(rs + 13, re - rs - 13);
                        html = html.Substring(0, rs);
                    }
                    string[] gyt = html.Split('|');
                    html1 = gyt[0];
                    html12 = gyt[1];
                    #endregion

                    #region 统计部分
                    bool hm = false;
                    if (Masterpage.CurrUser.IsEmployee) hm = accountService.IsEmployeeHasMaterial(Masterpage.CurrUser.role_guid, Masterpage.CurrUser.guid);
                    else hm = accountService.IsUserHasMaterial(Masterpage.CurrUser.role_guid);
                    if (hm)
                    {
                        var filepath2 = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicematerial/" + month + ".txt";
                        if (FileHelper.IsFileExist(filepath2))
                        {
                            html = FileHelper.ReadTxtFile(filepath2); 
                            string[] yl = html.Split('|');
                            html2 = yl[0];
                            html22 = yl[1];
                        }
                    }
                    #endregion
                    #region 成本部分
                    bool hc = false;
                    if (Masterpage.CurrUser.IsEmployee) hc = accountService.IsEmployeeHasCost(Masterpage.CurrUser.role_guid, Masterpage.CurrUser.guid);
                    else hc = accountService.IsUserHasCost(Masterpage.CurrUser.role_guid);
                    if (hc)
                    {
                        var filepath2 = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicecost/" + month + ".txt";
                        if (FileHelper.IsFileExist(filepath2))
                        {
                            html = FileHelper.ReadTxtFile(filepath2);
                            string[] cb = html.Split('|');
                            html3 = cb[0];
                            html32 = cb[1];
                        }
                    }
                    #endregion
                }
                else
                {
                    find = 0;
                    html = "不存在服务月报";
                    LogHelper.Info(Masterpage.CurrUser.alias, "606014:客户," + Masterpage.CurrUser.client_code + ",服务月报查看，path：" + path + "不存在服务月报");
                }
                #region 是否有pdf
                 var pdfpath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + month + ".pdf";
                 if (!FileHelper.IsFileExist(pdfpath))
                 {
                     string[] t = month.Split('-');
                     DateTime start = Convert.ToDateTime(t[0].Substring(0, 4) + "-" + t[0].Substring(4, 2) + "-" + t[0].Substring(6, 2));
                     DateTime end = Convert.ToDateTime(t[1].Substring(0, 4) + "-" + t[1].Substring(4, 2) + "-" + t[1].Substring(6, 2));
                     if (DateTime.Now > end)
                     {
                         canpdf = true;
                     }
                 }
                 else
                 {
                     canremark = false;
                 }
                #endregion

            }
            catch(Exception ex)
            {
                data.find = 0;
                html = "服务月报读取错误 ";
                LogHelper.Info(Masterpage.CurrUser.alias, "606014:客户," + Masterpage.CurrUser.client_code + ",服务月报查看，path：" + path + "服务月报读取错误:"+ex.Message);
            }
            var nweek = DateTime.Now.ToString("yyyyMM");
            data.cansub = month == nweek.ToString();
            data.find = find;
            data.path = path;
            data.one = one;
            data.day = day;
            data.html1 = html1;
            data.html2 = html2;
            data.html3 = html3;
            data.html12 = html12;
            data.html22 = html22;
            data.html32 = html32;
            data.remark = remark;
            data.month = month;
            data.canpdf = canpdf;
            data.canremark = canremark;
            return View(data);
        }
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult writeremark()
        {
            //type: 'weekly', remark: remark, name: week
            ReturnValue r = new ReturnValue();
            string type = WebRequest.GetString("type", true);
            string name = WebRequest.GetString("name", true);
            string remark = WebRequest.GetString("remark", true);
            string filepath = "";
            string typename = "";
            #region 文件路径
            switch (type)
            {
                case "dayly":
                    typename = name+"日报";
                    filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceday/" + name + ".txt";
                    break;
                case "weekly":
                    typename = name + "周报";
                    filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/serviceweek/" + name + ".txt";
                    break;
                case "month":
                    typename = name + "月报";
                    filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + name + ".txt";
                    break;
                default:
                    break;
            }
            #endregion
            try
            {
                if (ecoBio.Wms.Common.FileHelper.IsFileExist(filepath))
                {
                    var b = ServiceWeekly.TxtRemark(filepath, remark);
                    if (b)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "606015:客户," + Masterpage.CurrUser.client_code + ",服务" + typename + "备注填写，path：" + filepath +  "备注添加成功，备注：" + remark);
                    }
                    else
                    {
                        r = new ReturnValue { status = "error", message = "服务" + typename + "备注添加失败" };
                        LogHelper.Info(Masterpage.CurrUser.alias, "606015:客户," + Masterpage.CurrUser.client_code + ",服务" + typename + "备注填写，path：" + filepath + "备注添加失败，备注：" + remark);
                    }
                }
                else
                {
                    r = new ReturnValue { status = "error", message = "服务" + typename + "不存在" };
                    LogHelper.Info(Masterpage.CurrUser.alias, "606015:客户," + Masterpage.CurrUser.client_code + ",服务" + typename + "备注填写，path：" + filepath  + "文件不存在，备注：" + remark);
                }

            }
            catch
            {
                r = new ReturnValue { status = "error", message = "服务" + typename + "读取错误" };
                LogHelper.Info(Masterpage.CurrUser.alias, "606015:客户," + Masterpage.CurrUser.client_code + ",服务" + typename + "备注填写，path：" + filepath + "文件读取错误，备注：" + remark);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult weeklyserviceemail()
        {
            string name = WebRequest.GetString("name", true);

            #region 查找
            try
            {
                if (Masterpage.CurrUser.email == null || Masterpage.CurrUser.email.ToString() == "")
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",邮箱为空，无法发送邮件");
                    return Json(new ReturnValue { message = "邮箱为空，无法发送邮件", status = "error" }, JsonRequestBehavior.AllowGet);
                }
                var filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicereport/" + Masterpage.CurrUser.guid.ToString() + "/" + name;
                if (FileHelper.IsFileExist(filepath))
                {
                    #region 发送邮件
                    var send = managementService.GetSendEmailModel();
                    if (send == null)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务报告邮件发送，发送失败未设置系统邮箱");
                        return Json(new ReturnValue { message = "未设置系统邮箱", status = "error" }, JsonRequestBehavior.AllowGet);
                    }
                    MailHelper mh = new MailHelper();
                    var html = "";
                    var title = Masterpage.CurrUser.client_name + "服务报告";
                    mh.Mail(Masterpage.CurrUser.email, send.text, html, title, send.value);
                    mh.Attachments(filepath);

                    try
                    {
                        mh.Send();
                        LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务报告邮件发送成功" + filepath);
                        return Json(new ReturnValue { message = "", status = "ok" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务报告邮件发送，发送失败" + ex.Message);
                        return Json(new ReturnValue { message = ex.Message, status = "error" }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务报告邮件发送，发送失败，文件不存在" + filepath);
                    return Json(new ReturnValue { message = "文件不存在", status = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exx)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "606013:客户," + Masterpage.CurrUser.client_code + ",服务报告邮件发送，发送失败" + exx.Message);
                return Json(new ReturnValue { message = "发送失败", status = "error" }, JsonRequestBehavior.AllowGet);
            }
            #endregion

        }

        [HidNowLocal]
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult serviceweeklyview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string path = "";
            int find = 0;
            string name = WebRequest.GetString("name", true);
            try
            {
                path = "~/res/" + Masterpage.CurrUser.client_code + "/files/servicereport/" + Masterpage.CurrUser.guid.ToString() + "/" + name;
                if (FileHelper.IsFileExist(path)) find = 1;
                else find = 0;
            }
            catch
            {
                find = 0;
            }
            data.find = find;
            data.name = name;
            data.path = "/res/" + Masterpage.CurrUser.client_code + "/files/servicereport/" + Masterpage.CurrUser.guid.ToString() + "/" + name; ;
            return View(data);
        }

        public ActionResult weeklyservicelist(int? page, int? pagesize)
        {
            string month = WebRequest.GetString("month", true);
            string week = WebRequest.GetString("week", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetCustomerServiceWeekly(Masterpage.CurrUser.client_code, month, week);//.Where(p => p. != "");
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.name = Masterpage.CurrUser.client_name;
            data.month = month;
            data.week = week;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (week != "") otherparam += "&week=" + week;
            if (month != "") otherparam += "&month=" + month;
            data.otherParam = otherparam;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult weeklyservicecontent()
        {
            string guid = WebRequest.GetString("guid", true);
            string content = WebRequest.GetString("content", true);
            ReturnValue r = new ReturnValue();
            #region 保存
            try
            {
                Guid g = Guid.Parse(guid);
                r = managementService.UpdateCustomerServiceWeekly(Masterpage.CurrUser.client_code, g, Masterpage.CurrUser.guid, content);
            }
            catch
            {
                return RedirectToAction("weeklyservice");
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [Anonymous]
        public ActionResult forpdf(string para)
        {
            string html = "";
            string html1 = "";
            string html12 = "";
            string path = "";
            string[] t=para.Split('_');
            dynamic data = new System.Dynamic.ExpandoObject();
            Customer one = managementService.GetCustomerInfo(t[0]);
            #region 结算日
            int day = 1;
            try
            {
                day = Convert.ToInt32(one.CustomerDtu);
            }
            catch
            {
                day = 1;
            }
            #endregion
            string remark = "";
            int find = 0;
            try
            {
                var filepath = ConfigurationManager.AppSettings["CustomerRes"] + t[0] + "/files/servicemonth/" + t[1] + ".txt";
                if (FileHelper.IsFileExist(filepath))
                {
                    find = 1;

                    #region 工艺部分

                    find = 1;
                    html = FileHelper.ReadTxtFile(filepath);
                    //$remarkstart$_$remarkend$
                    var rs = html.IndexOf("$remarkstart$");
                    var re = html.IndexOf("$remarkend$");
                    if (rs != -1 && re != -1)
                    {
                        remark = html.Substring(rs + 13, re - rs - 13);
                        html = html.Substring(0, rs);
                    }
                    string[] gyt = html.Split('|');                 
                   
                    #region 表头临时读
                    string str1 = "";
                    string str2 = "";
                    string str3 = "";
                    str1 = WebAccountHelper.MonthReportHead(t[0], 1);
                    str2 = WebAccountHelper.MonthReportHead(t[0], 2);
                    str3 = WebAccountHelper.MonthReportHead(t[0], 3);
                    #endregion
                    html1 = ExcelRead.ServiceWeekly.CreateMonthTab1Head(str1, str2, str3);
                    html12 = gyt[1];
                    #endregion

                    #region 统计部分
                    //var filepath2 = ConfigurationManager.AppSettings["CustomerRes"] + t[0] + "/files/servicestatistic/" + t[1] + ".txt";
                    //if (FileHelper.IsFileExist(filepath2))
                    // {
                    //     html2 = FileHelper.ReadTxtFile(filepath2);
                    // }
                    #endregion
                    LogHelper.Info(Masterpage.CurrUser.alias, "606014:客户," + t[0] + ",服务月报查看，path：" + path);
                }
                else
                {
                    find = 0;
                    html = "不存在服务月报";
                    LogHelper.Info(Masterpage.CurrUser.alias, "606014:客户," + t[0] + ",服务月报查看，path：" + path + "不存在服务月报");
                }

            }
            catch (Exception ex)
            {
                data.find = 0;
                html = "服务月报读取错误 ";
                LogHelper.Info(Masterpage.CurrUser.alias, "606014:客户," + t[0] + ",服务月报查看，path：" + path + "服务月报读取错误:" + ex.Message);
            }
            data.find = find;
            data.path = path;
            data.one = one;
            data.day = day;
            data.html = html;
            data.html1 = html1;
            data.html12 = html12;
            data.remark = remark;
            data.month = t[1];
            return View(data);
        } 
        [AjaxAction(ForAction = "weeklyservice", ForController = "management")]
        public ActionResult createpdf()
        {
            //type: 'weekly', remark: remark, name: week
            ReturnValue r = new ReturnValue();
            string type = WebRequest.GetString("type", true);
            string name = WebRequest.GetString("name", true);
            string filepath = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + name + ".txt";

            try
            {
                //因为Web 是多线程环境，避免甲产生的文件被乙下载去，所以档名都用唯一 
                string fileNameWithOutExtention = Guid.NewGuid().ToString();

                //执行wkhtmltopdf.exe 
                var url = Utils.GetRootUrl("/management/forpdf?para=" + Masterpage.CurrUser.client_code + "_" + name);
                var save=ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/servicemonth/" + name + ".pdf";
                Process p = System.Diagnostics.Process.Start(@"C:\Program Files (x86)\wkhtmltopdf\wkhtmltopdf.exe", @"" + url + " " + save);

                //若不加这一行，程序就会马上执行下一句而抓不到文件发生意外：System.IO.FileNotFoundException: 找不到文件 ''。 
                p.WaitForExit();

                r.status = "ok";
                ////把文件读进文件流 
                //FileStream fs = new FileStream(save, FileMode.Open);
                //byte[] file = new byte[fs.Length];
                //fs.Read(file, 0, file.Length);
                //fs.Close();

                ////Response给客户端下载 
                //Response.Clear();
                //Response.AddHeader("content-disposition", "attachment; filename=" + name + ".pdf");//强制下载 
                //Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(file);

            }
            catch
            {

                r.status = "error";
                //LogHelper.Info(Masterpage.CurrUser.alias, "606015:客户," + Masterpage.CurrUser.client_code + ",服务" + typename + "备注填写，path：" + filepath + "文件读取错误，备注：" + remark);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region 生物增效SOP
        public ActionResult bioefficiencysop(int? page, int? pagesize)
        {
            string unit = WebRequest.GetString("unit", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetCustomerBioefficiencySop(Masterpage.CurrUser.client_code, unit, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.unit = unit;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.ddlunit = managementService.GetStandardProcessUnitDDL(Masterpage.CurrUser.client_code);
            string otherparam = "";
            if (unit != "") otherparam += "&unit=" + unit;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "607011:客户," + Masterpage.CurrUser.client_code + ",生物增效SOP列表,第" + _page + "页");
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "bioefficiencysop", ForController = "management")]
        public ActionResult biosopview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string path = "";
            string name = "";
            int find = 0;
            string guid = WebRequest.GetString("guid", true);
            try
            {
                Guid g = Guid.Parse(guid);
                var one = managementService.GetOneCustomerBioefficiencySOP(Masterpage.CurrUser.client_code, g);
                if (one != null)
                {
                    var file = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                    if (FileHelper.IsFileExist(file))
                    {
                        find = 1;
                        path = ConfigurationManager.AppSettings["VirtualRes"] + Masterpage.CurrUser.client_code + "/files/sop/" + one.CustomerSopPath;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603017:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + file);
                    }
                    else
                    {
                        find = 0;
                        LogHelper.Info(Masterpage.CurrUser.alias, "603017:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + file + "，SOP不存在");
                    }
                    name = guid + ".pdf";
                }
                else
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "603017:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + path + "，SOP不存在");
                    find = 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "603017:客户," + Masterpage.CurrUser.client_code + ",查看生物增效SOP详情,file" + ex.Message);
                data.find = 0;
            }
            data.find = find;
            data.name = name;
            data.path = path;
            return View(data);
        }
        #endregion

        #region 现场异常
        public ActionResult fieldabnormal(int? page, int? pagesize)
        {
            string number = WebRequest.GetString("number", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            data.show = Masterpage.CurrUser.IsEmployee ? 1 : 0;

            var list = managementService.GetAbnormalManagementList(Masterpage.CurrUser.client_code, number);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.number = number;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.ddlstatus = managementService.GetAbnormalStatus();
            string otherparam = "";
            if (number != "") otherparam += "&number=" + number;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "608011:客户," + Masterpage.CurrUser.client_code + ",现场异常,第" + _page + "页");

            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "equipment", ForController = "management")]
        public ActionResult fieldabnormalpage(string t, int? list_id)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            AbnormalManagementList one = new AbnormalManagementList();
            string number = "";
            if (t == "edit" && t != "" && t != "0")
            {
                one = managementService.GetOneAbnormalManagementList(Masterpage.CurrUser.client_code, list_id.Value);
                number = one.AbnormalManagementNO;
            }
            else
            {
                list_id = 0;
                number = managementService.CreatAbnormalManagementNumber();
            }
            string person = Masterpage.CurrUser.alias;
            //data.ddltype = managementService.GetAbnormalType();
            data.ddllevel = managementService.GetAbnormalLevel();
            data.ddlstatus = managementService.GetAbnormalStatus();
            data.one = one;
            data.number = number;
            data.person = person;
            data.type = t;
            data.list_id = list_id;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult saveofieldabnormal()
        {
            //string customercode, Guid employee, int id,string date, string title, string rate, string content, string status, string microorganism, string question
            string id = WebRequest.GetString("id", true);
            string date = WebRequest.GetString("date", true);
            string type = WebRequest.GetString("type", true);
            string title = WebRequest.GetString("title", true);
            string content = WebRequest.GetString("content", true);
            string status = "已新建";
            string rate = WebRequest.GetString("ddllevel", true);
            string microorganism = "";// WebRequest.GetString("microorganism", true);
            string question = "";// WebRequest.GetString("ddltype", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            if (type == "add")
            {
                r = managementService.AddAbnormalManagement(Masterpage.CurrUser.client_code, Masterpage.CurrUser.guid, date, title, rate, content, status, microorganism, question);
            }
            else if (type == "edit" && id != "" && id != "0")
            {
                int _id = int.Parse(id);
                r = managementService.UpdateAbnormalManagement(Masterpage.CurrUser.client_code, _id, Masterpage.CurrUser.guid, date, title, rate, content, status, microorganism, question);
            }
            #endregion
            LogHelper.Info(Masterpage.CurrUser.alias, "608012:客户," + Masterpage.CurrUser.client_code + ",保存现场异常,操作类别：" + type + "，日期：" + date + "，标题：" + title + "，内容：" + content + "，操作结果：" + r.status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult deletefieldabnormal()
        {
            string id = WebRequest.GetString("list_id", true);
            ReturnValue r = new ReturnValue();
            try
            {
                bool d = managementService.DeleteAbnormalManagement(Masterpage.CurrUser.client_code, int.Parse(id));
                if (d)
                {
                    r.status = "ok";
                }
                else
                {
                    r.status = "error";
                    r.message = "删除失败";
                }
                LogHelper.Info(Masterpage.CurrUser.alias, "608013:客户," + Masterpage.CurrUser.client_code + ",删除现场异常,异常ID：" + id + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "608013:客户," + Masterpage.CurrUser.client_code + ",删除现场异常,异常ID：" + id + ex.Message);
                r.status = "error";
                r.message = "程序异常";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult closefieldabnormal()
        {
            string id = WebRequest.GetString("list_id", true);
            ReturnValue r = new ReturnValue();
            try
            {
                bool d = managementService.ChangeAbnormalManagementStatus(Masterpage.CurrUser.client_code, int.Parse(id), "已关闭");
                if (d)
                {
                    r.status = "ok";
                }
                else
                {
                    r.status = "error";
                    r.message = "删除失败";
                }
            }
            catch
            {
                r.status = "error";
                r.message = "程序异常";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult fieldabnormalview(int? page, int? pagesize)
        {
            string list_id = WebRequest.GetString("list_id", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            AbnormalManagementList one = new AbnormalManagementList();
            AbnormalProcessDetail last = new AbnormalProcessDetail();
            #region 单个对象
            try
            {
                one = managementService.GetOneAbnormalManagementList(Masterpage.CurrUser.client_code, int.Parse(list_id));
                if (one == null) return RedirectToAction("fieldabnormal");
                if (one.AbnormalStatus == "已关闭") last = managementService.GetLastAbnormalProcessDetail(Masterpage.CurrUser.client_code, int.Parse(list_id));
            }
            catch
            {
                return RedirectToAction("fieldabnormal");
            }
            #endregion
            var list = managementService.GetAbnormalProcessDetail(Masterpage.CurrUser.client_code, int.Parse(list_id));
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.one = one;
            data.last = last;
            data.ddltype = managementService.GetAbnormalType();
            string otherparam = "";
            if (list_id != "") otherparam += "&list_id=" + list_id;
            data.otherParam = otherparam;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult fieldabnormaldetailpage(string t, int? list_id, int? detail_id)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            AbnormalManagementList ml = new AbnormalManagementList();
            string number = "";
            #region 现场异常
            try
            {
                ml = managementService.GetOneAbnormalManagementList(Masterpage.CurrUser.client_code, list_id.Value);
                if (ml == null) return RedirectToAction("fieldabnormal");
                number = ml.AbnormalManagementNO;
            }
            catch
            {
                return RedirectToAction("fieldabnormal");
            }
            #endregion
            AbnormalProcessDetail one = new AbnormalProcessDetail();
            string person = Masterpage.CurrUser.alias;
            if ((t == "edit" || t == "show") && t != "" && t != "0")
            {
                one = managementService.GetOneAbnormalProcessDetail(Masterpage.CurrUser.client_code, list_id.Value, detail_id.Value);
                person = one.Employee.EmployeeChineseName;
            }
            data.one = one;
            data.person = person;
            data.number = number;
            data.type = t;
            data.list_id = list_id.Value;
            data.detail_id = detail_id.Value;
            return View(data);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult saveofieldabnormaldetail(int? list_id)
        {
            string id = WebRequest.GetString("id", true);
            string type = WebRequest.GetString("type", true);
            string advised = WebRequest.GetString("advised", true);
            string analysis = WebRequest.GetString("analysis", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            if (type == "add")
            {
                r = managementService.AddAbnormalProcessDetail(Masterpage.CurrUser.client_code, list_id.Value, Masterpage.CurrUser.guid, Masterpage.CurrUser.alias, analysis);
            }
            else if (type == "edit" && id != "" && id != "0")
            {
                int _id = int.Parse(id);
                r = managementService.UpdatebnormalProcessDetail(Masterpage.CurrUser.client_code, list_id.Value, _id, Masterpage.CurrUser.guid, Masterpage.CurrUser.alias, analysis);
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "fieldabnormal", ForController = "management")]
        public ActionResult deletefieldabnormaldetail()
        {
            string id = WebRequest.GetString("list_id", true);
            string detailid = WebRequest.GetString("detailid", true);
            ReturnValue r = new ReturnValue();
            try
            {
                bool d = managementService.DeleteAbnormalProcessDetail(Masterpage.CurrUser.client_code, int.Parse(id), int.Parse(detailid));
                if (d)
                {
                    r.status = "ok";
                }
                else
                {
                    r.status = "error";
                    r.message = "删除失败";
                }
            }
            catch
            {
                r.status = "error";
                r.message = "程序异常";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 生物增效服务监管(填写)
        public ActionResult servicesregulatory(int? page, int? pagesize)
        {
            string person = WebRequest.GetString("person", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code, person, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.person = person;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.customer = Masterpage.CurrUser.client_name;
            data.ddlstatus = managementService.GetSiteServiceManagementStatusDDL();
            string otherparam = "";
            if (person != "") otherparam += "&person=" + person;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult servicesdown()
        {
            string guid = WebRequest.GetString("guid", true);
            SiteServiceManagementModel one = new SiteServiceManagementModel();
            #region 查找
            try
            {
                Guid g = Guid.Parse(guid);
                one = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.service_management_guid == g);
                if (one == null || one.service_content == "")
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "609013:客户," + Masterpage.CurrUser.client_code + ",服务监管下载失败，guid：" + guid + "服务监管不存在");
                    return RedirectToAction("siteservice");
                }
                string path = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/localservice/" + guid + ".pdf";
                FileHelper.DownLoadFile(path, Masterpage.CurrUser.client_name + one.service_start_time.Value.ToString("yyyyMMdd") + "现场服务报告.pdf");
                LogHelper.Info(Masterpage.CurrUser.alias, "609013:客户," + Masterpage.CurrUser.client_code + ",服务监管下载，guid：" + guid + "文件路径：" + path);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "609013:客户," + Masterpage.CurrUser.client_code + ",服务监管下载失败，guid：" + guid + ex.Message);
                return RedirectToAction("siteservice");
            }
            #endregion
            return View();
        }

        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult servicesemail()
        {
            string guid = WebRequest.GetString("guid", true);
            SiteServiceManagementModel one = new SiteServiceManagementModel();
            #region 查找
            try
            {
                if (Masterpage.CurrUser.email == null || Masterpage.CurrUser.email.ToString() == "")
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送失败，guid：" + guid + "用户邮箱为空，无法发送邮件");
                    return Json(new ReturnValue { message = "邮箱为空，无法发送邮件", status = "error" }, JsonRequestBehavior.AllowGet);
                }
                Guid g = Guid.Parse(guid);
                one = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.service_management_guid == g);
                if (one == null || one.service_content == "")
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送失败，guid：" + guid + "未报表未生成");
                    return Json(new ReturnValue { message = "未报表未生成", status = "error" }, JsonRequestBehavior.AllowGet);
                }
                string title = Masterpage.CurrUser.client_name + "[" + one.service_start_time.Value.ToString("yyyyMMdd") + "]现场服务报告";
                //html = html.Replace("$title$", title);
                //html = html.Replace("$number$", one.employee_guid.ToString());
                //html = html.Replace("$employeename$", Masterpage.CurrUser.client_name);
                //html = html.Replace("$starttime$", one.service_start_time.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //html = html.Replace("$content$", one.service_content);
                //html = html.Replace("$completetime$", one.service_completion_time.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //html = html.Replace("$checkperson$", one.employee_chinese_name);
                //html = html.Replace("$checktime$", one.service_validate_time.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //html = html.Replace("$checkcontent$", one.service_validate_comments);

                //createservices(one);
                //string attachment = "~/content/management/" + Masterpage.CurrUser.client_code + "/" + guid + ".pdf";
                string attachment = ConfigurationManager.AppSettings["CustomerRes"] + Masterpage.CurrUser.client_code + "/files/localservice/" + guid + ".pdf";
                #region 发送邮件
                var send = managementService.GetSendEmailModel();
                if (send == null)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送失败，guid：" + guid + "未设置系统邮箱");
                    return Json(new ReturnValue { message = "未设置系统邮箱", status = "error" }, JsonRequestBehavior.AllowGet);
                }
                MailHelper mh = new MailHelper();
                mh.Mail(Masterpage.CurrUser.email, send.text, title, title, send.value);
                mh.Attachments(attachment);

                try
                {
                    mh.Send();
                }
                catch (Exception ex)
                {
                    LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送失败，guid：" + guid + ex.Message);
                    return Json(new ReturnValue { message = ex.Message, status = "error" }, JsonRequestBehavior.AllowGet);
                }
                #endregion
                LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送成功，guid：" + guid + attachment);
                return Json(new ReturnValue { message = "", status = "ok" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",服务监管邮件发送失败，guid：" + guid + ex.Message);
                return Json(new ReturnValue { message = "发送失败" + ex.Message, status = "error" }, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult servicescontent()
        {
            string guid = WebRequest.GetString("guid", true);
            SiteServiceManagementModel one = new SiteServiceManagementModel();
            dynamic data = new System.Dynamic.ExpandoObject();
            #region 保存
            try
            {
                Guid g = Guid.Parse(guid);
                one = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.service_management_guid == g);
                if (one == null)
                {
                    return RedirectToAction("servicesregulatory");
                }
                data.one = one;
            }
            catch
            {
                return RedirectToAction("servicesregulatory");
            }
            #endregion
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult saveservicescontent()
        {
            string guid = WebRequest.GetString("guid", true);
            string content = WebRequest.GetString("content", true);
            ReturnValue r = new ReturnValue();
            #region 保存
            try
            {
                Guid g = Guid.Parse(guid);
                r = managementService.UpdateSiteServiceManagement(Masterpage.CurrUser.client_code, g, false, Masterpage.CurrUser.guid, Masterpage.CurrUser.alias, content);
                LogHelper.Info(Masterpage.CurrUser.alias, "609012:客户," + Masterpage.CurrUser.client_code + ",填写服务监管，guid" + guid + "，内容为：" + content + "操作结果：" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "609012:客户," + Masterpage.CurrUser.client_code + ",填写服务监管保存失败，guid" + guid + "，" + ex.Message);
                return RedirectToAction("servicesregulatory");
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public void createservices(SiteServiceManagementModel one)
        {
            //string path = "~/res/" + Masterpage.CurrUser.client_code + "/files/localservice/" + one.service_management_guid.ToString();
            //FileHelper.CheckDirectory("~/res/" + Masterpage.CurrUser.client_code + "/files/localservice");
            //bool exist = FileHelper.IsFileExist(path + ".pdf");
            //if (!exist)
            //{
            //    FileHelper.CreateServiceregulatory("~/res/public/templates/localservice.xlsx", path + ".xlsx", 
            //        Masterpage.CurrUser.client_name + "[" + one.service_start_time.Value.ToString("yyyyMMdd") + "]现场服务报告",
            //       Masterpage.CurrUser.client_name,  one.service_start_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),one.service_person,
            //        one.service_completion_time.Value.ToString("yyyy-MM-dd HH:mm:ss"), 
            //        one.service_content,
            //        one.service_validate_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),one.check_person,
            //        one.service_validate_comments);
            //    OfficeHelper.XLSConvertToPDF(path + ".xlsx", path + ".pdf");
            //    FileHelper.DeleteFile(path + ".xlsx");
            //}
        }

        #endregion

        #region 生物增效服务监管（审核）
        public ActionResult servicesregulatorycheck(int? page, int? pagesize)
        {
            string person = WebRequest.GetString("person", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code, person, start, end);//"02",
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 14;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.person = person;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.customer = Masterpage.CurrUser.client_name;
            data.ddlstatus = managementService.GetSiteServiceManagementStatusDDL();
            string otherparam = "";
            if (person != "") otherparam += "&person=" + person;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult servicescheck()
        {
            string guid = WebRequest.GetString("guid", true);
            SiteServiceManagementModel one = new SiteServiceManagementModel();
            dynamic data = new System.Dynamic.ExpandoObject();
            #region 保存
            try
            {
                Guid g = Guid.Parse(guid);
                one = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.service_management_guid == g);
                if (one == null)
                {
                    return RedirectToAction("servicesregulatorycheck");
                }
                data.one = one;
                data.check = Masterpage.CurrUser.config3;
                data.person = Masterpage.CurrUser.alias;
            }
            catch
            {
                return RedirectToAction("servicesregulatorycheck");
            }
            #endregion
            return View(data);
        }
        [HidNowLocal]
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult saveservicescheck()
        {
            string guid = WebRequest.GetString("guid", true);
            string content = WebRequest.GetString("content", true);
            ReturnValue r = new ReturnValue();
            #region 保存
            try
            {
                Guid g = Guid.Parse(guid);
                r = managementService.UpdateSiteServiceManagement(Masterpage.CurrUser.client_code, g, true, Masterpage.CurrUser.guid, Masterpage.CurrUser.alias, content);

                if (r.status == "ok")
                {
                    //try
                    //{
                    //    #region 生成报表
                    //    SiteServiceManagementModel one = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code).FirstOrDefault(p => p.service_management_guid == g);

                    //    createservices(one);
                    //    #endregion
                    //}
                    //catch (Exception e)
                    //{
                    //    r.status = "error";
                    //    r.message = "记录添加成功，报表生成失败，原因：" + e.Message;

                    //}
                }
            }
            catch
            {
                return RedirectToAction("servicesregulatorycheck");
            }
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 服务监管合并
        public ActionResult siteservice(int? page, int? pagesize)
        {
            string person = WebRequest.GetString("person", true);
            string start = WebRequest.GetString("start", true);
            string end = WebRequest.GetString("end", true);
            dynamic data = new System.Dynamic.ExpandoObject();
            var h = managementService.HadSiteServiceManagement(Masterpage.CurrUser.client_code, Masterpage.CurrUser.guid, DateTime.Now);
            var list = managementService.GetSiteServiceManagement(Masterpage.CurrUser.client_code, Masterpage.CurrUser.guid, person, start, end);
            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 13;
            var vs = list.ToPagedList(_page, _pagesize);
            data.check = Masterpage.CurrUser.config4;
            data.list = vs;
            data.h = h;
            data.person = person;
            data.start = start;
            data.end = end;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.customer = Masterpage.CurrUser.client_name;
            data.ddlstatus = managementService.GetSiteServiceManagementStatusDDL();
            string otherparam = "";
            if (person != "") otherparam += "&person=" + person;
            if (start != "") otherparam += "&start=" + start;
            if (end != "") otherparam += "&end=" + end;
            data.otherParam = otherparam;
            LogHelper.Info(Masterpage.CurrUser.alias, "609011:客户," + Masterpage.CurrUser.client_code + ",服务监管列表，第" + _page + "页");
            return View(data);
        }

        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult siteservicepage(string guid, string t)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            SiteServiceManagementModel one = new SiteServiceManagementModel();
            string person = Masterpage.CurrUser.alias;
            string type = t;

            if (guid != null && guid.ToString() != "")
            {
                Guid g;
                try { g = Guid.Parse(guid); }
                catch { return RedirectToAction("siteservice"); }
                one = managementService.GetOneSiteServiceManagement(Masterpage.CurrUser.client_code, g);
                data.hv = 1;
                person = one.service_person;
                LogHelper.Info(Masterpage.CurrUser.alias, "609012:客户," + Masterpage.CurrUser.client_code + ",查看工作日志信息，工作日志guid" + guid);
            }
            else
            {
                data.hv = 0;
                one = new SiteServiceManagementModel();
                LogHelper.Info(Masterpage.CurrUser.alias, "609012:客户," + Masterpage.CurrUser.client_code + ",新增工作日志");
            }
            data.guid = guid;
            data.person = person;
            data.one = one;
            data.type = t;
            return View(data);
        }
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult savesiteservice()
        {
            string guid = WebRequest.GetString("guid", true);
            string type = WebRequest.GetString("type", true);
            string dates = WebRequest.GetString("start", true);
            string datee = WebRequest.GetString("end", true);
            string datesh = WebRequest.GetString("starth", true);
            string dateeh = WebRequest.GetString("endh", true);
            string content = WebRequest.GetString("content", true);
            ReturnValue r = new ReturnValue();
            #region  类别
            Guid g2 ;
            if (type == "add") g2 = Guid.Empty;
            else g2 = Guid.Parse(guid);
            DateTime dts = DateTime.Parse(dates).AddHours(int.Parse(datesh));
            DateTime dte = DateTime.Parse(datee).AddHours(int.Parse(dateeh));
            r = managementService.SaveSiteServiceManagement(Masterpage.CurrUser.client_code, g2, type, Masterpage.CurrUser.guid, Masterpage.CurrUser.alias, content, dts, dte);

            LogHelper.Info(Masterpage.CurrUser.alias, "609013:客户," + Masterpage.CurrUser.client_code + ",保存工作日志，工作日志guid" + guid + "，操作类别：" + type + "，开始日期：" + dates + "，结束日期：" + datee + "，内容：" + content + "操作结果：" + r.status);
            #endregion
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "siteservice", ForController = "management")]
        public ActionResult delsiteservice()
        {
            string guid = WebRequest.GetString("guid", true);
            ReturnValue r;
            try
            {
                Guid g = Guid.Parse(guid);
                bool b = managementService.DeleteSiteServiceManagement(Masterpage.CurrUser.client_code, g);
                if (b) r = new ReturnValue { status = "ok" };
                else r = new ReturnValue { status = "error", message = "删除失败！" };
                LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code+  ",工作日志"+guid+"删除" + r.status);
            }
            catch (Exception ex)
            {
                LogHelper.Info(Masterpage.CurrUser.alias, "609014:客户," + Masterpage.CurrUser.client_code + ",工作日志" + guid + ",删除失败，" + ex.Message);
                r = new ReturnValue { status = "error", message = "删除失败！" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
