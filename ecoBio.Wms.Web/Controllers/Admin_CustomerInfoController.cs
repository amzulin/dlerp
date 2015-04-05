using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Entities = ecoBio.Wms.Data.Entities;
using ecoBio.Wms.Common;
using ecoBio.Wms.ViewModel;
using ecoBio.Wms.Data.Entities.Models;
using System.IO;
using System.Configuration;

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_CustomerInfoController : AdminController
    {
        #region  实现控制反转

        private ecoBio.Wms.Service.Management.CustomerService _customerRepos = null;
        private ecoBio.Wms.Service.Management.CustomerCategoryService _customerCategoryRepos = null;
        private ecoBio.Wms.Service.Management.CustomerEquipmentSpecService _CustomerEquipmentSpecRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentSpecService _EquipmentSpecRepos = null;
        private ecoBio.Wms.Service.Management.StandardTipService _StandardTipRepos = null;
        private ecoBio.Wms.Service.Management.StandardUnitService _StandardUnitRepos = null;
        private ecoBio.Wms.Service.Management.CustomerConstructionService _CustomerConstructionRepos = null;
        private ecoBio.Wms.Service.Management.InventoryService _InventoryRepos = null;
        private ecoBio.Wms.Service.Management.MSpecificationService _MSpecificationRepos = null;
        private ecoBio.Wms.Service.Management.CustomerBasicDataApplicationService _BasicDataApplicationRepos = null;
        private ecoBio.Wms.Service.Management.ModuleFunctionService _moduleFunctionRepos = null;
        private ecoBio.Wms.Service.Management.CustomerSOPService _CustomerSOPRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentKeyParameterLibService _EquipmentKeyParameterLibRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentKeyParameterService _EquipmentKeyParameterRepos = null;
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="reRepos"></param>

        public Admin_CustomerInfoController(ecoBio.Wms.Backstage.Repositories.ICustomerRepository customerRepos,
                                  ecoBio.Wms.Backstage.Repositories.ICustomerCategoryRepository customerCategoryRepos,
            ecoBio.Wms.Backstage.Repositories.ICustomerEquipmentSpecRepository CustomerEquipmentSpecRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentSpecRepository EquipmentSpecRepos,
            ecoBio.Wms.Backstage.Repositories.IStandardTipRepository StandardTipRepos,
              ecoBio.Wms.Backstage.Repositories.IStandardUnitRepository StandardUnitRepos,
            ecoBio.Wms.Backstage.Repositories.ICustomerConstructionRepository CustomerConstructionRepos,
            ecoBio.Wms.Backstage.Repositories.IInventoryRepository InventoryRepos,
            ecoBio.Wms.Backstage.Repositories.IMSpecificationRepository MSpecificationRepos,
            ecoBio.Wms.Backstage.Repositories.ICustomerBasicDataApplicationRepository BasicDataApplicationRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentKeyParameterLibRepository EquipmentKeyParameterLibRepos,
            ecoBio.Wms.Backstage.Repositories.IModuleFunctionRepository moduleFunctionRepos, ecoBio.Wms.Backstage.Repositories.ICustomerSOPRepository sopRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentKeyParameterRepository EquipmentKeyParameterRepos)
        {
            _customerRepos = new Service.Management.CustomerService(customerRepos);
            _customerCategoryRepos = new Service.Management.CustomerCategoryService(customerCategoryRepos);
            _CustomerEquipmentSpecRepos = new Service.Management.CustomerEquipmentSpecService(CustomerEquipmentSpecRepos);
            _EquipmentSpecRepos = new Service.Management.EquipmentSpecService(EquipmentSpecRepos);
            _StandardUnitRepos = new Service.Management.StandardUnitService(StandardUnitRepos);
            _StandardTipRepos = new Service.Management.StandardTipService(StandardTipRepos);
            _CustomerConstructionRepos = new Service.Management.CustomerConstructionService(CustomerConstructionRepos);
            _InventoryRepos = new Service.Management.InventoryService(InventoryRepos);
            _MSpecificationRepos = new Service.Management.MSpecificationService(MSpecificationRepos);
            _BasicDataApplicationRepos = new Service.Management.CustomerBasicDataApplicationService(BasicDataApplicationRepos);
            _moduleFunctionRepos = new Service.Management.ModuleFunctionService(moduleFunctionRepos);
            _CustomerSOPRepos = new Service.Management.CustomerSOPService(sopRepos);
            _EquipmentKeyParameterLibRepos = new Service.Management.EquipmentKeyParameterLibService(EquipmentKeyParameterLibRepos);
            _EquipmentKeyParameterRepos = new Service.Management.EquipmentKeyParameterService(EquipmentKeyParameterRepos);
        }
        #endregion

        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult Index(string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            //var mslist = _ModuleFunctionRepos.GetModuleByRole(Masterpage.AdminCurrUser.role_guid);
            //var ms = mslist.Where(f => f.ModuleFunctionType == "M" && f.ModuleFunctionId.StartsWith("B")).ToList();
            //data.list1 = ms;
            //data.person = Masterpage.AdminCurrUser.alias;
            //data.list2 = mslist;
            var units = _customerRepos.GetCustomerUnits(code);
           var one= _customerRepos.GetCustomerByCode(code);
           data.one = one;
           data.units = units;
            data.code = code;
            return View(data);

        }

        #region ================== Inventory Beginning ===================
        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult Inventory(string first, string code)
        {
            LogHelper.BackInfo("2-4", Masterpage.AdminCurrUser.userid, "访问客户耗材");
            dynamic data = new System.Dynamic.ExpandoObject();

            data.first = first;
            data.code = code;
            return View(data);
        }
        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult IndexInventory(int? page, int? pagesize, string name, string first, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _InventoryRepos.GetAllInventory(name, code);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new Inventory();
            if (first != null && first != "")
            {
                Guid g = Guid.Parse(first);
                firstone = list.FirstOrDefault(p => p.InventoryGuid == g);
                var firspage = vs.IndexOf(firstone);
                if (firspage == -1)
                {
                    vs.Insert(0, firstone);
                }
                else if (firspage > 0)
                {
                    vs.Remove(firstone);
                    vs.Insert(0, firstone);
                }
            }
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            if (code != "") otherparam += "&code=" + code;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 

        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult AddInventory(string hidtype, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();          
            data.code = code;
            var tip = _StandardTipRepos.GetStandardTip();
            data.tip = tip;
            var ms = _MSpecificationRepos.GetMaterialSpecification();
            data.ms = ms;
            data.one = new Entities::Models.Inventory();
            if (hidtype == "add")
            {

                return PartialView(new Entities::Models.Inventory());

            }
            if (hidtype == "update")
            {
                Guid guid = new Guid();
                return PartialView(_InventoryRepos.GetInventoryByCode(guid));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult saveInventory(Entities::Models.Inventory instance, string hidtype)
        {

            try
            {
                if (hidtype == "add")
                {
                    instance.InventoryGuid = Guid.NewGuid();
                    _InventoryRepos.AddService(instance);
                    LogHelper.BackInfo("2-4", Masterpage.AdminCurrUser.userid, "添加客户耗材" + instance.InventoryGuid);
                }
                if (hidtype == "update")
                {
                    _InventoryRepos.UpdateService(instance);
                    LogHelper.BackInfo("2-4", Masterpage.AdminCurrUser.userid, "修改客户耗材" + instance.InventoryGuid);
                }
            }
            catch
            {

            }
            return RedirectToAction("Inventory", new { first = instance.InventoryGuid, code = instance.CustomerCode });

        }
        /// <summary>
        /// 删除一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult DeleteInventory(Guid guid)
        {
            try
            {
                _InventoryRepos.DeleteService(guid);
                LogHelper.BackInfo("2-4", Masterpage.AdminCurrUser.userid, "删除客户耗材" + guid);
            }
            catch
            { }
            return RedirectToAction("Inventory");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Inventory", ForController = "Admin_Customer")]
        public ActionResult GetInventoryCode(Guid guid)
        {

            var list = _InventoryRepos.GetInventoryByCode(guid);
            var c = new
            {
                a = list.InventoryGuid,
                b = list.CustomerCode,
                e = list.StandardTipsCode,
                f = list.MaterialSpecificationCode,
                g = list.InventoryAmount,
                h = list.LocaleAmount,
                i = list.InventorySafetyAmount,
                j = list.InventoryCost
            };
            return Json(c);
        }
        #endregion =================== Inventory Ending ==================

        #region ================== CustomerSOP Beginning ===================
         [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult CustomerSOP(string first,string code)
        {
            LogHelper.BackInfo("2-7", Masterpage.AdminCurrUser.userid, "访问客户SOP");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.first = first;
            data.code = code;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
         public ActionResult IndexCustomerSOP(int? page, int? pagesize, string name, string first, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerSOPRepos.GetAllCustomerSOP(name, code);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new CustomerSOP();
            if (first != null && first != "")
            {
                Guid g = Guid.Parse(first);
                firstone = list.FirstOrDefault(p => p.CustomerSopGuid == g);
                var firspage = vs.IndexOf(firstone);
                if (firspage == -1)
                {
                    vs.Insert(0, firstone);
                }
                else if (firspage > 0)
                {
                    vs.Remove(firstone);
                    vs.Insert(0, firstone);
                }
            }
            data.name = name;
            data.code = code;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            if (code != "") otherparam += "&code=" + code;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult AddCustomerSOP(string hidtype, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.code = code;
            var unit = _StandardUnitRepos.GetStandardProcessUnit();
            data.unit = unit;
            data.one = new Entities::Models.CustomerSOP();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.CustomerSOP());
            }
            if (hidtype == "update")
            {
                Guid guid = new Guid();
                return PartialView(_CustomerSOPRepos.GetCustomerSOPByCode(guid));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult SaveCustomerSOP(Entities::Models.CustomerSOP instance, string hidtype, bool? CustomerSopIsBioaugmentation)
        {


            try
            {
                if (!CustomerSopIsBioaugmentation.HasValue) instance.CustomerSopIsBioaugmentation = true;
                if (hidtype == "add")
                {
                    instance.CustomerSopGuid = Guid.NewGuid();
                    instance.CustomerSopDate = DateTime.Now;
                    _CustomerSOPRepos.AddService(instance);
                    LogHelper.BackInfo("2-7", Masterpage.AdminCurrUser.userid, "添加客户SOP" + instance.CustomerSopGuid);
                }
                if (hidtype == "update")
                {
                    _CustomerSOPRepos.UpdateService(instance);
                    LogHelper.BackInfo("2-7", Masterpage.AdminCurrUser.userid, "修改客户SOP" + instance.CustomerSopGuid);
                }
            }
            catch
            {

            }

            return RedirectToAction("CustomerSOP", new { first = instance.CustomerSopGuid,code=instance.CustomerCode });

        }
        /// <summary>
        /// 删除一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomerSOP(Guid guid)
        {
            try
            {
                _CustomerSOPRepos.DeleteService(guid);
                LogHelper.BackInfo("2-7", Masterpage.AdminCurrUser.userid, "修改客户SOP" + guid);
            }
            catch
            { }
            return RedirectToAction("CustomerSOP");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult GetCustomerSOPCode(Guid guid)
        {

            var list = _CustomerSOPRepos.GetCustomerSOPByCode(guid);
            var c = new
            {
                a = list.CustomerSopGuid,
                b = list.StandardProcessUnitCode,
                e = list.CustomerCode,
                f = list.CustomerSopName,
                g = list.CustomerSopVersion,
                h = list.CustomerSopPath,
                i = list.CustomerSopIsBioaugmentation,
                j = list.CustomerSopRemark,
                k = list.CustomerSopDate.HasValue ? list.CustomerSopDate.Value.ToString("yyyy-MM-dd") : ""
            };
            return Json(c);
        }
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult uploadpdf(string code)
        {
            if (code == null)
            {
                var t = new ReturnValue { status = "error", message = "请选择客户号" };
                return Json(t, JsonRequestBehavior.AllowGet);
            }

            //var cc = _customerRepos.GetCustomerByCode(code);
            string path = ConfigurationManager.AppSettings["CustomerRes"] + code + "/files/sop";
            string backpath = "../res/" + code + "/files/sop";

            //string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(path, identifier.ToString());
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".pdf")
                {
                    r = new ReturnValue { status = "error", message = "请上传.pdf" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                httpfile.SaveAs(uploadsPath + exn);
                r = new ReturnValue { status = "ok", value = backpath + "/" + identifier.ToString() + exn, value2 = identifier.ToString() + exn };
            }
            else
            {
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion =================== CustomerSOP Ending ==================


        #region 采集点
        #region 客户采集点配置
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollection(string code,string unit)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "访问客户采集点页面");
            #region 下拉框
            //var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            //var units = _customerRepos.GetDDLAllStandardUnit();
            //var points = _customerRepos.GetDDLAllCollectionPoint();
            var tips = _customerRepos.GetDDLAllStandardTip();
            var reports = _customerRepos.GetDDLAllStandardReport();
            var charts = _customerRepos.GetDDLAllStandardChart();
            var methods = _customerRepos.GetDDLAllCalMethod();
            //var pointcates = _customerRepos.GetDDLPointCategory();
            //var diacates = _customerRepos.GetDDLDiangosticCategory();
            //data.customers = customers;
            //data.units = units;
            //data.points = points;
            data.code = code;
            data.unit = unit;
            data.tips = tips;
            data.reports = reports;
            data.charts = charts;
            data.methods = methods;
            //data.pointcates = pointcates;
            //data.diacates = diacates;
            #endregion
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionList(int? page, int? pagesize, string code, string unit, string key)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (unit == null || unit == "") unit = "0000";
            var list = _customerRepos.GetCustomerCollection(code, unit, key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.code = code;
            data.unit = unit;
            data.key = key;
            var pointcates = _customerRepos.GetDDLPointCategory();
            var diacates = _customerRepos.GetDDLDiangosticCategory();
            data.pointcates = pointcates;
            data.diacates = diacates;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (code != "") otherparam += "&code=" + code;
            if (unit != "") otherparam += "&unit=" + unit;
            if (key != "") otherparam += "&key=" + key;
            data.otherParam = otherparam;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult savecustomercollection()
        {
            //ddlcustomer,ddlunit,ddlpoint,ddlchart,ddltip,ddlreport,ddlpointcate,ddldiacate,ddlmethod,ddnumber,ddclcye,txtclcye,txtunit,txtlower,txtlow,txtup,txtuper
            string editcode = WebRequest.GetString("editcode", true);
            string customer = WebRequest.GetString("customer", true);
            string ddlunit = WebRequest.GetString("ddlunit", true);
            string ddlpoint = WebRequest.GetString("ddlpoint", true);
            string ddlreport = WebRequest.GetString("ddlreport", true);
            string ddlchart = WebRequest.GetString("ddlchart", true);
            string ddltip = WebRequest.GetString("ddltip", true);
            string ddlpointcate = WebRequest.GetString("ddlpointcate", true);
            string ddldiacate = WebRequest.GetString("ddldiacate", true);
            string ddlmethod = WebRequest.GetString("ddlmethod", true);
            string ddnumber = WebRequest.GetString("ddnumber", true);
            string ddclcye = WebRequest.GetString("ddclcye", true);
            string txtclcye = WebRequest.GetString("txtclcye", true);
            string txtunit = WebRequest.GetString("txtunit", true);
            string txtlower = WebRequest.GetString("txtlower", true);
            string txtlow = WebRequest.GetString("txtlow", true);
            string txtup = WebRequest.GetString("txtup", true);
            string txtuper = WebRequest.GetString("txtuper", true);

            string txtcode = WebRequest.GetString("txtcode", true);
            ReturnValue r = new ReturnValue();
            if (txtcode == null || txtcode == "")
            {
                r = new ReturnValue { status = "error", message = "请填写客户采集点编码" };

            }
            else
            {
                //DGKLSZ_BCO-1_I306_03_00_00_2-4 	
                string[] array = txtcode.Split('_');
                if (array.Length != 7 && array[0] != customer)
                {
                    r = new ReturnValue { status = "error", message = "客户采集点编码格式不正确" };
                }
                else
                {
                    ddlunit = array[1];
                    ddlpoint = array[2];
                    ddlpointcate = array[3];
                    ddldiacate = array[4];
                    ddnumber = array[5];
                    txtunit = array[6];
                    if (editcode != "") LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "修改客户采集点" + editcode + "信息");
                    else LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "添加" + customer + "客户采集点,标准单元：" + ddlunit + "，采集点：" + ddlpoint + "");
                    r = _customerRepos.SaveCustomerCollection(editcode, customer, ddlunit, ddlpoint, ddlchart, ddltip, ddlreport, ddlpointcate, ddldiacate, ddlmethod, ddnumber, ddclcye, txtclcye, txtunit, txtlower, txtlow, txtup, txtuper);
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult getcustomercollection(string code)
        {
            var one = _customerRepos.GetOneCustomerCollection(code);
            var model = new
            {
                CustomerCollectionCode = one.CustomerCollectionCode,
                CustomerCollectionCycle = one.CustomerCollectionCycle,
                CustomerCollectionLowerLimit = one.CustomerCollectionLowerLimit,
                CustomerCollectionLowLimit = one.CustomerCollectionLowLimit,
                CustomerCollectionUpLimit = one.CustomerCollectionUpLimit,
                CustomerCollectionUpperLimit = one.CustomerCollectionUpperLimit,
                StandardProcessUnitCode = one.StandardProcessUnitCode,
                StandardChartCode = one.StandardChartCode,
                CollectionPointCode = one.CollectionPointCode,
                CustomerCode = one.CustomerCode,
                CustomerCollectionCalMethod = one.CustomerCollectionCalMethod

            };
            LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "读取单个客户采集点" + code + "的信息");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult delcustomercollection()
        {
            string code = WebRequest.GetString("code", true);

            var r = _customerRepos.DelOneCustomerCollection(code);
            LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "删除客户采集点" + code + "信息，" + (r.status == "ok" ? "成功" : "失败"));
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 客户采集点是否配置
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionConfig()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            #region 下拉框
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            var units = _customerRepos.GetDDLAllStandardUnit();
            var points = _customerRepos.GetDDLAllCollectionPoint();

            data.customers = customers;
            data.units = units;
            data.points = points;
            #endregion
            return View(data);
        }

        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionConfigList(int? page, int? pagesize, string customer, string unit, string point, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var one = _customerRepos.GetOneCustomerCollectionConfig(code);
            data.one = one;
            data.code = code;
            LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "读取单个客户采集点" + code + "的配置信息");
            return PartialView(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult SaveCustomerCollectionConfig()
        {
            string forcode = WebRequest.GetString("forcode", true);
            string type = WebRequest.GetString("type", true);
            string select = WebRequest.GetString("select", true);
            var r = _customerRepos.SaveCustomerCollectionConfig(type, forcode, select);
            LogHelper.BackInfo("2-2", Masterpage.AdminCurrUser.userid, "保存单个客户采集点" + forcode + "的配置信息，操作：" + type + ",配置为" + select);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region ================== CustomerEquipmentSpec Beginning ===================
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult CustomerEquipmentSpec(string code, string unit)
        {
            LogHelper.BackInfo("2-5", Masterpage.AdminCurrUser.userid, "访问客户设备管理 ");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.code = code;
            data.unit = unit;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerEquipmentSpec(int? page, int? pagesize, string name, string code,string unit)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerEquipmentSpecRepos.GetAllCustomerEquipmentSpec(name,code,unit);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.name = name;
            data.code = code;
            data.unit = unit;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            if (code != "") otherparam += "&code=" + code;
            if (unit != "") otherparam += "&unit=" + unit;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult AddCustomerEquipmentSpec(string guid, string type, string code, string unit)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var equ = _EquipmentSpecRepos.GetEquipmentSpec();
            data.equ = equ;
            data.code = code;
            var tip = _StandardTipRepos.GetStandardTip();
            data.tip = tip;
            data.unit = unit;
            data.one = new Entities::Models.CustomerEquipmentSpec();

            data.guid = guid;
            data.type = type;

            #region 设备参数库
            var list = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLib("设备");
            data.list = list;
            #endregion 设备参数库


            return PartialView(data);
        }

        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult saveeCustomerEquipmentSpec(string hidtype, string guidstr, string valuestr)
        {
            string CustomerEquipmentSpecGuid = WebRequest.GetString("g", true);
            string EquipmentSpecGuid = WebRequest.GetString("esguid", true);
            string type = WebRequest.GetString("type", true);
            string CustomerCode = WebRequest.GetString("CustomerCode", true);
            string StandardTipsCode = WebRequest.GetString("StandardTipsCode", true);
            string StandardProcessUnitCode = WebRequest.GetString("StandardProcessUnitCode", true);
            string CustomerEquipmentPositionNumber = WebRequest.GetString("CustomerEquipmentPositionNumber", true);
            int CustomerEquipmentMaintenanceCycle = WebRequest.GetInt("CustomerEquipmentMaintenanceCycle", 1);
            string CustomerEquipmentSn = WebRequest.GetString("CustomerEquipmentSn", true);
            string CustomerEquipmentIsTips = WebRequest.GetString("CustomerEquipmentIsTips", true);

            if (guidstr.EndsWith("#")) guidstr = guidstr.Substring(0, guidstr.Length - 1);
            if (valuestr.EndsWith("#")) valuestr = valuestr.Substring(0, valuestr.Length - 1);
            string[] eklg1 = guidstr.Split('#');
            string[] EquipmentKeyParameterValue1 = valuestr.Split('#');
            ReturnValue r = new ReturnValue();
            Guid g = Guid.Empty;
            Guid esguid = Guid.Empty;
            if (EquipmentSpecGuid != "") esguid = Guid.Parse(EquipmentSpecGuid);
            if (type == "edit") g = Guid.Parse(CustomerEquipmentSpecGuid);
            r = _CustomerEquipmentSpecRepos.SaveCustomerEquipmentSpec(g, esguid, type, CustomerCode, StandardTipsCode, StandardProcessUnitCode, CustomerEquipmentPositionNumber, CustomerEquipmentMaintenanceCycle, CustomerEquipmentSn, Convert.ToBoolean(CustomerEquipmentIsTips), eklg1, EquipmentKeyParameterValue1);
            if (type == "edit")
            {
                type = "修改";
            }
            if (type == "add")
            {
                type = "添加";
            }

            LogHelper.BackInfo("2-5", Masterpage.AdminCurrUser.userid, type + "客户设备:" + g);
            return Json(r, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 删除一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomerEquipmentSpec(Guid guid)
        {
            try
            {
                _CustomerEquipmentSpecRepos.DeleteService(guid);
                LogHelper.BackInfo("2-5", Masterpage.AdminCurrUser.userid, "删除客户设备:" + guid);

            }
            catch
            { }
            return RedirectToAction("CustomerEquipmentSpec");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ///
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult GetCustomerEquipmentSpecCode(Guid guid)
        {

            var list = _CustomerEquipmentSpecRepos.GetCustomerEquipmentSpecByCode(guid);
            var c = new
            {
                a = list.CustomerEquipmentSpecGuid,
                b = list.EquipmentSpecGuid,
                e = list.CustomerCode,
                f = list.StandardTipsCode,
                g = list.StandardProcessUnitCode,
                h = list.CustomerEquipmentPositionNumber,
                i = list.CustomerEquipmentMaintenanceCycle,
                j = list.CustomerEquipmentSn,
                k = list.CustomerEquipmentIsTips.Value,
                l = list.CustomerEquipmentBeginningDate.ToString("yyyy-MM-dd")
            };
            return Json(c);
        }
        #endregion =================== CustomerEquipmentSpec Ending ==================

        #region ================== CustomerConstruction Beginning ===================
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult CustomerConstruction(string code,string unit)
        {
            LogHelper.BackInfo("2-6", Masterpage.AdminCurrUser.userid, "访问客户构筑物 ");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.code = code;
            data.unit = unit;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerConstruction(int? page, int? pagesize, string name, string first, string code, string unit)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerConstructionRepos.GetAllCustomerConstruction(name,code,unit);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new CustomerConstruction();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.CustomerConstructionPositionNumber == first);
                var firspage = vs.IndexOf(firstone);
                if (firspage == -1)
                {
                    vs.Insert(0, firstone);
                }
                else if (firspage > 0)
                {
                    vs.Remove(firstone);
                    vs.Insert(0, firstone);
                }
            }
            data.code = code;
            data.unit = unit;
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            if (code != "") otherparam += "&code=" + code;
            if (unit != "") otherparam += "&unit=" + unit;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult AddCustomerConstruction(string code, string unit, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.unit = unit;
            data.code = code;
            data.one = new Entities::Models.CustomerConstruction();
            //if (hidtype == "add")
            //{
            //    return PartialView(new Entities::Models.CustomerConstruction());
            //}
            //if (hidtype == "update")
            //{

            //    return PartialView(_CustomerConstructionRepos.GetCustomerConstructionByCode(code));
            //}

            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult saveCustomerConstruction(Entities::Models.CustomerConstruction instance, string hidtype)
        {
            //dynamic data = new System.Dynamic.ExpandoObject();

            //var customer = _customerRepos.GetCustomer();
            //data.customer = customer;

            //var unit = _StandardUnitRepos.GetStandardProcessUnit();
            //data.unit = unit;
            try
            {
                if (hidtype == "add")
                {

                    var str = instance.CustomerCode + "_" + (instance.StandardProcessUnitCode == null ? "0000" : instance.StandardProcessUnitCode) + "_" + instance.CustomerConstructionPositionNumber;
                    instance.CustomerConstructionPositionNumber = str;
                    _CustomerConstructionRepos.AddService(instance);
                    LogHelper.BackInfo("2-6", Masterpage.AdminCurrUser.userid, "添加客户构筑物 " + str);
                }
                if (hidtype == "update")
                {
                    _CustomerConstructionRepos.UpdateService(instance);
                }
            }
            catch
            {

            }

            return RedirectToAction("CustomerConstruction", new { first = instance.CustomerConstructionPositionNumber,code=instance.CustomerCode,unit=instance.StandardProcessUnitCode });
        }
        /// <summary>
        /// 删除一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomerConstruction(string code)
        {
            try
            {
                _CustomerConstructionRepos.DeleteService(code);
            }
            catch
            { }
            LogHelper.BackInfo("2-6", Masterpage.AdminCurrUser.userid, "删除客户构筑物 " + code);
            return RedirectToAction("CustomerConstruction");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult GetCustomerConstructionCode(string code)
        {

            var list = _CustomerConstructionRepos.GetCustomerConstructionByCode(code);
            var c = new
            {
                a = list.CustomerConstructionPositionNumber,
                b = list.StandardProcessUnitCode,
                e = list.CustomerCode,
                f = list.CustomerConstructionMaterial,
                g = list.CustomerConstructionConstracter,
                h = list.CustomerConstructionConstructionTime.HasValue ? list.CustomerConstructionConstructionTime.Value.ToString("yyyy-MM-dd") : "",
                i = list.CustomerConstructiontOperationTime.HasValue ? list.CustomerConstructionConstructionTime.Value.ToString("yyyy-MM-dd") : "",
                j = list.CustomerConstructionCategory,
                k = list.CustomerConstructionRemark,
                l = list.CustomerConstructionPic
            };
            return Json(c);
        }

        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult IsCustomerConstructionExit(string num, string hidtype)
        {
            var one = _CustomerConstructionRepos.GetCustomerConstructionByCode(num);
            if (hidtype == "add")
            {
                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("CustomerConstruction");
        }

        // [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        //public ActionResult GetCustomerConstructionByCode1(string code)
        //{

        //    var list = _customerRepos.GetCustomerConstructionByCode1(code);
        //    if (list == null)
        //    {
        //        return Content("0");
        //    }
        //    else
        //    {
        //        var c = list.Select(p => new { a = p.CustomerCode });
        //        return Json(c);
        //    }
        //}
        #endregion =================== CustomerConstruction Ending ==================

    }
}
