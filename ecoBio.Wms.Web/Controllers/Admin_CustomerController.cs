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
    public class Admin_CustomerController : AdminController//Controller //
    {
        private ecoBio.Wms.Service.Management.RoleService _roleRepos = null;
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

        public Admin_CustomerController(ecoBio.Wms.Backstage.Repositories.IRoleRepository roleRepos, 
            ecoBio.Wms.Backstage.Repositories.ICustomerRepository customerRepos,
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
            _roleRepos = new Service.Management.RoleService(roleRepos);
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

        #region ================== Customer Beginning ==================
        public ActionResult Customer(string first)
        {
            LogHelper.BackInfo("002011", Masterpage.AdminCurrUser.userid, "访问客户列表");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            data.customers = customers;
            data.first = first;
            return View(data);
        }
        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult IndexCustomer(int? page, int? pagesize, string name, string first)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _customerRepos.GetAllCustomer(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new Customer();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.CustomerCode == first);
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
            data.otherParam = otherparam;
            return PartialView(data);


        }
        /// <summary>
        /// 添加一个客户
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult AddCustomer(string hidtype)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var cates = _customerCategoryRepos.GetCustomerCategory();
            var sector = _customerRepos.GetCustomerSector();
            data.cates = cates;
            data.sector = sector;
            data.one = new Entities::Models.Customer();
            if (hidtype == "add")
            {

                return View(new Entities::Models.Customer());
            }
            if (hidtype == "update")
            {
                string code = "";
                return View(_customerRepos.GetCustomerByCode(code));
            }
            return PartialView(data);

        }
        [HttpPost]
        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult SaveCustomer(Entities::Models.Customer instance, string hidtype, string OldCustomerCategoryCode)
        {
            try
            {
                if (hidtype == "add")
                {
                    _customerRepos.AddService(instance);
                    LogHelper.BackInfo("002012", Masterpage.AdminCurrUser.userid, "添加客户:" + "[客户号:" + instance.CustomerCode + "]" + "[客户名称：" + instance.CustomerFullName + "]" + "[客户父级:" + instance.CustomerGroup + "]" + "[客户分类：" + instance.CustomerCategoryCode + "]" + "[客户地址:" + instance.CustomerAddress + "]" + "[客户电话：" + instance.CustomerTel + "]" + "[客户传真:" + instance.CustomerFax + "]" + "[客户用户数：" + instance.CustomerAccountNum + "]" + "[客户DTU:" + instance.CustomerDtu + "]" + "[客户概念：" + instance.CustomerProfile + "]" + "[客户所属行业:" + instance.CustomerCode + "]" + "[客户名称：" + instance.CustomerSector + "]" + "[项目联系人:" + instance.ProjectContact + "]" + "[项目联系人电话：" + instance.ProjectContactTel + "]" + "[项目联系人手机:" + instance.ProjectContactMobile + "]" + "[项目联系人邮箱：" + instance.ProjectContactMail + "]");
                    var userrols = _roleRepos.GetDictionaryRoleList("USER_ROLE_CATEGORY");
                    // List<SelectListItem> roles1 = userrols.Select(p => new SelectListItem { Text = p.text, Value = p.value }).ToList();
                    #region 增加角色并为管理员添加权限
                    string addstr = "";
                    string del = "";
                    for (int i = 0; i < userrols.Count; i++)
                    {
                        addstr += userrols[i].text;
                        if (i != userrols.Count - 1) addstr += "#";
                    }
                    string[] addl = addstr.Split('#');
                    string[] dell = del.Split('#');
                    bool b = _roleRepos.SaveCustomerRole(instance.CustomerCode, addl, dell, true, instance.CustomerFullName);
                    if (b) LogHelper.BackInfo("2-1", Masterpage.AdminCurrUser.userid, "添加客户的角色[" + "客户号:" + instance.CustomerCode + "," + "客户名称：" + instance.CustomerFullName + "," + "角色名称：" + addstr + "]");
                    bool c = _roleRepos.SetUserAdminCustomerCategroyRighs(instance.CustomerCategoryCode, instance.CustomerCode + "_ADMIN");
                    if (c) LogHelper.BackInfo("2-1", Masterpage.AdminCurrUser.userid, "添加客户的ADMIN角色分类权限");
                    #endregion
                }
                if (hidtype == "update")
                {
                    _customerRepos.UpdateService(instance);
                    LogHelper.BackInfo("0020113", Masterpage.AdminCurrUser.userid, "修改客户：" + "[客户号:" + instance.CustomerCode + "]" + "[客户名称：" + instance.CustomerFullName + "]" + "[客户父级:" + instance.CustomerGroup + "]" + "[客户分类：" + instance.CustomerCategoryCode + "]" + "[客户地址:" + instance.CustomerAddress + "]" + "[客户电话：" + instance.CustomerTel + "]" + "[客户传真:" + instance.CustomerFax + "]" + "[客户用户数：" + instance.CustomerAccountNum + "]" + "[客户DTU:" + instance.CustomerDtu + "]" + "[客户概念：" + instance.CustomerProfile + "]" + "[客户所属行业:" + instance.CustomerCode + "]" + "[客户名称：" + instance.CustomerSector + "]" + "[项目联系人:" + instance.ProjectContact + "]" + "[项目联系人电话：" + instance.ProjectContactTel + "]" + "[项目联系人手机:" + instance.ProjectContactMobile + "]" + "[项目联系人邮箱：" + instance.ProjectContactMail + "]");
                    #region 如果改了大类，则修改分类权限
                    var oldc = _customerRepos.GetCustomerByCode(instance.CustomerCode);
                    if (OldCustomerCategoryCode != null && OldCustomerCategoryCode!=instance.CustomerCategoryCode )
                    {
                        bool a = _roleRepos.UpdateCustomerCategoryRights(instance.CustomerCode, oldc.CustomerCategoryCode, instance.CustomerCategoryCode);
                        if (a) LogHelper.BackInfo("2-1", Masterpage.AdminCurrUser.userid, "修改了客户" + instance.CustomerCode + "的分类，ADMIN权限自动变更[原来分类:" + oldc.CustomerCategoryCode + "][现为分类:" + instance.CustomerCategoryCode + "]");
                    }
                    #endregion
                }
            }
            catch
            {

            }

            return RedirectToAction("Customer", new { first = instance.CustomerCode });
        }

        /// <summary>
        /// 删除一个客户
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomer(string code)
        {
            try
            {
                _customerRepos.DeleteService(code);
                LogHelper.BackInfo("002014", Masterpage.AdminCurrUser.userid, "删除客户：" + "[客户号:" + code + "]");

            }
            catch
            {

            }
            return RedirectToAction("Customer");
        }

        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult GetCustomerByGuid1(string code)
        {
            var list = _customerRepos.GetCustomerByCode(code);
            var customer = new
            {
                customer_code = list.CustomerCode,
                CustomerGroup = list.CustomerGroup,
                customer_category_code = list.CustomerCategoryCode,
                customer_full_name = list.CustomerFullName,
                customer_address = list.CustomerAddress,
                customer_tel = list.CustomerTel,
                customer_fax = list.CustomerFax,
                customer_account_num = list.CustomerAccountNum,
                customer_dtu = list.CustomerDtu,
                customer_logo_url = list.CustomerLogoUrl,
                customer_profile = list.CustomerProfile,
                customer_sector = list.CustomerSector,
                project_contact = list.ProjectContact,
                project_contact_tel = list.ProjectContactTel,
                project_contact_mobile = list.ProjectContactMobile,
                project_contact_mail = list.ProjectContactMail,

            };
            return Json(customer);
        }

        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult IsCustomerExit(string code, string hidtype)
        {
            var one = _customerRepos.GetCustomerByCode(code);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("Customer");
        }
        [AjaxAction(ForAction = "Customer", ForController = "Admin_Customer")]
        public ActionResult upload(string code)
        {

            string imgpath = "~/res/public/images";
            string backpath = "../res/public/images";
            string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(uploadsFolder, identifier.ToString());
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".jpg" && exn.ToLower() != ".png" && exn.ToLower() != ".gif")
                {
                    r = new ReturnValue { status = "error", message = "请上传.jpg或.png" };
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
        #endregion =================== Customer Ending ==================

        #region CustomerPower
    
        public ActionResult CustomerPower(int? page, int? pagesize, string name, string first)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _customerRepos.GetAllCustomer(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new Customer();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.CustomerCode == first);
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
            data.otherParam = otherparam;
            return View(data);


        }
        #endregion

        #region ================== CustomerCategory Beginning ==================
        public ActionResult CustomerCategory()
        {
            return View();
        }
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerCategory()
        {
            LogHelper.BackInfo("007021", Masterpage.AdminCurrUser.userid, "访问分类权限");

            var result = _customerCategoryRepos.GetCustomerCategory();
            //ViewBag.TotalPages = totalPages;
            //ViewBag.PageIndex = pageIndex;
            //ViewBag.PageSize = pageSize;

            return PartialView("IndexCustomerCategory", result);
        }
        /// <summary>
        /// 添加一个客户类型
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult AddCustomerCategory(string code, string hidtype)
        {
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.CustomerCategory());
            }
            if (hidtype == "update")
            {

                return View(_customerCategoryRepos.GetCustomerCategoryByCode(code));
            }

            return PartialView();

        }

        [HttpPost]
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult saveCustomerCategory(Entities::Models.CustomerCategory instance, string hidtype)
        {
            try
            {
                if (hidtype == "add")
                {
                    _customerCategoryRepos.AddService(instance);
                    LogHelper.BackInfo("007022", Masterpage.AdminCurrUser.userid, "添加分类权限：" + "[客户分类号:" + instance.CustomerCategoryCode + "]" + "[客户分类名称：" + instance.CustomerCategoryName + "]" + "[客户分类备注：" + instance.CustomerCategoryRemark + "]");
                }
                if (hidtype == "update")
                {
                    _customerCategoryRepos.UpdateService(instance);
                    LogHelper.BackInfo("007023", Masterpage.AdminCurrUser.userid, "修改分类权限：" + "[客户分类号:" + instance.CustomerCategoryCode + "]" + "[客户分类名称：" + instance.CustomerCategoryName + "]" + "[客户分类备注：" + instance.CustomerCategoryRemark + "]");
                }
            }
            catch
            {

            }
            return RedirectToAction("CustomerCategory");
        }

        /// <summary>
        /// 删除一个客户类型
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomerCategory(string code)
        {

            try
            {
                _customerCategoryRepos.DeleteService(code);
                LogHelper.BackInfo("007024", Masterpage.AdminCurrUser.userid, "删除分类权限:" + code);
            }
            catch
            {

            }


            return RedirectToAction("CustomerCategory");

        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult GetCustomerCategoryByCode(string code)
        {
            LogHelper.BackInfo("5-3", Masterpage.AdminCurrUser.userid, "读取" + code + "的分类权限界面GetCustomerCategoryByCode");
            var list = _customerCategoryRepos.GetCustomerCategoryByCode(code);
            var customerCategory = new
            {
                customer_category_code = list.CustomerCategoryCode,
                customer_category_name = list.CustomerCategoryName,
                customer_category_remark = list.CustomerCategoryRemark
            };
            return Json(customerCategory);
        }
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult SetCategoryRight(string category)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int r = 0;
            List<ModuleFunction> all = new List<ModuleFunction>();
            List<ModuleFunction> had = new List<ModuleFunction>();
            try
            {

                LogHelper.BackInfo("5-3", Masterpage.AdminCurrUser.userid, "读取" + category + "的分类权限界面");
                var allq = _moduleFunctionRepos.GetModuleByAll();
                var hadq = _moduleFunctionRepos.GetModuleByCustomerCategory(category);
                if (allq != null) all = allq.ToList();
                if (hadq != null) had = hadq.ToList();
                r = 1;
            }
            catch
            {
                r = 0;
            }
            data.all = all;
            data.had = had;
            data.r = r;
            data.category = category;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult SaveetCategoryRight(string category, string add, string del)
        {
            try
            {
                LogHelper.BackInfo("5-3", Masterpage.AdminCurrUser.userid, "设置" + category + "的分类权限，添加：" + add + "，移除：" + del + "");
                if (add.EndsWith("#")) add = add.Substring(0, add.Length - 1);
                if (del.EndsWith("#")) del = del.Substring(0, del.Length - 1);
                string[] addl = add.Split('#');
                string[] dell = del.Split('#');
                bool b = _moduleFunctionRepos.SaveCustomerCategoryRigths(category, addl, dell);
                if (b) return Json(new { status = "ok" });
                else return Json(new { status = "error" });
            }
            catch
            {
                return Json(new { status = "error" });
            }
        }
        [AjaxAction(ForAction = "CustomerCategory", ForController = "Admin_Customer")]
        public ActionResult IsCustomerCategoryExit(string code, string hidtype)
        {
            var one = _customerCategoryRepos.GetCustomerCategoryByCode(code);
            if (hidtype == "add")
            {

                return one == null ? Content("0") : Content("1");
            }
            return RedirectToAction("CustomerCategory");
        }
        #endregion =================== CustomerCategory Ending ==================

        #region 客户采集点配置
        public ActionResult CustomerCollection()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            LogHelper.BackInfo("002021", Masterpage.AdminCurrUser.userid, "访问客户采集点页面");
            #region 下拉框
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            var units = _customerRepos.GetDDLAllStandardUnit();
            var points = _customerRepos.GetDDLAllCollectionPoint();
            var tips = _customerRepos.GetDDLAllStandardTip();
            var reports = _customerRepos.GetDDLAllStandardReport();
            var charts = _customerRepos.GetDDLAllStandardChart();
            var methods = _customerRepos.GetDDLAllCalMethod();
            var pointcates = _customerRepos.GetDDLPointCategory();
            var diacates = _customerRepos.GetDDLDiangosticCategory();
            data.customers = customers;
            data.units = units;
            data.points = points;
            data.tips = tips;
            data.reports = reports;
            data.charts = charts;
            data.methods = methods;
            data.pointcates = pointcates;
            data.diacates = diacates;
            #endregion
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionList(int? page, int? pagesize, string customer, string unit, string point)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = _customerRepos.GetCustomerCollection(customer, unit, point);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.customer = customer;
            data.unit = unit;
            data.point = point;
            var pointcates = _customerRepos.GetDDLPointCategory();
            var diacates = _customerRepos.GetDDLDiangosticCategory();
            data.pointcates = pointcates;
            data.diacates = diacates;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (customer != "") otherparam += "&customer=" + customer;
            if (unit != "") otherparam += "&unit=" + unit;
            if (point != "") otherparam += "&point=" + point;
            data.otherParam = otherparam;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult savecustomercollection()
        {
            //ddlcustomer,ddlunit,ddlpoint,ddlchart,ddltip,ddlreport,ddlpointcate,ddldiacate,ddlmethod,ddnumber,ddclcye,txtclcye,txtunit,txtlower,txtlow,txtup,txtuper
            string editcode = WebRequest.GetString("editcode", true);
            string ddlcustomer = WebRequest.GetString("ddlcustomer", true);
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

            if (editcode != "") LogHelper.BackInfo("002023", Masterpage.AdminCurrUser.userid, "修改客户采集点" + editcode + "信息");
            else LogHelper.BackInfo("002022", Masterpage.AdminCurrUser.userid, "添加" + ddlcustomer + "客户采集点,标准单元：" + ddlunit + "，采集点：" + ddlpoint + "");
            ReturnValue r = new ReturnValue();

            r = _customerRepos.SaveCustomerCollection(editcode, ddlcustomer, ddlunit, ddlpoint, ddlchart, ddltip, ddlreport, ddlpointcate, ddldiacate, ddlmethod, ddnumber, ddclcye, txtclcye, txtunit, txtlower, txtlow, txtup, txtuper);
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
            LogHelper.BackInfo("002025", Masterpage.AdminCurrUser.userid, "读取单个客户采集点" + code + "的信息");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult delcustomercollection()
        {
            string code = WebRequest.GetString("code", true);

            var r = _customerRepos.DelOneCustomerCollection(code);
            LogHelper.BackInfo("002024", Masterpage.AdminCurrUser.userid, "删除客户采集点" + code + "信息，" + (r.status == "ok" ? "成功" : "失败"));
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

        [AjaxAction(ForAction = "CustomerCollectionConfig", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionConfigList(int? page, int? pagesize, string customer, string unit, string point, string code)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var one = _customerRepos.GetOneCustomerCollectionConfig(code);
            data.one = one;
            data.code = code;
            LogHelper.BackInfo("002025", Masterpage.AdminCurrUser.userid, "读取单个客户采集点" + code + "的配置信息");
            return PartialView(data);
        }
        [AjaxAction(ForAction = "CustomerCollection", ForController = "Admin_Customer")]
        public ActionResult SaveCustomerCollectionConfig()
        {
            string forcode = WebRequest.GetString("forcode", true);
            string type = WebRequest.GetString("type", true);
            string select = WebRequest.GetString("select", true);
            var r = _customerRepos.SaveCustomerCollectionConfig(type, forcode, select);
            LogHelper.BackInfo("002025", Masterpage.AdminCurrUser.userid, "保存单个客户采集点" + forcode + "的配置信息，操作：" + type + ",配置为" + select);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ================== CustomerEquipmentSpec Beginning ===================

        public ActionResult CustomerEquipmentSpec()
        {
            LogHelper.BackInfo("2-5", Masterpage.AdminCurrUser.userid, "访问客户设备管理 ");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            data.customers = customers;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerEquipmentSpec", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerEquipmentSpec(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerEquipmentSpecRepos.GetAllCustomerEquipmentSpec(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
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
        public ActionResult AddCustomerEquipmentSpec(string guid, string type)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var equ = _EquipmentSpecRepos.GetEquipmentSpec();
            data.equ = equ;
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            var tip = _StandardTipRepos.GetStandardTip();
            data.tip = tip;
            var unit = _StandardUnitRepos.GetStandardProcessUnit();
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

        public ActionResult CustomerConstruction()
        {
            LogHelper.BackInfo("2-6", Masterpage.AdminCurrUser.userid, "访问客户构筑物 ");
            dynamic data = new System.Dynamic.ExpandoObject();

            return View(data);
        }
        [AjaxAction(ForAction = "CustomerConstruction", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerConstruction(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerConstructionRepos.GetAllCustomerConstruction(name);

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
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
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
        public ActionResult AddCustomerConstruction(string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var customer = _customerRepos.GetCustomer();
            data.customer = customer;

            var unit = _StandardUnitRepos.GetStandardProcessUnit();
            data.unit = unit;
            data.one = new Entities::Models.CustomerConstruction();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.CustomerConstruction());
            }
            if (hidtype == "update")
            {

                return PartialView(_CustomerConstructionRepos.GetCustomerConstructionByCode(code));
            }

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
          
            return RedirectToAction("CustomerConstruction", new { first = instance.CustomerConstructionPositionNumber });
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

        #region ================== Inventory Beginning ===================

        public ActionResult Inventory(string first,string code)
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
            var list = _InventoryRepos.GetAllInventory(name,code);

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
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
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
            return RedirectToAction("Inventory", new { first = instance.InventoryGuid });

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

        #region ================== CustomerBasicDataApplication Beginning ===================
        [ValidateInput(false)]
        public ActionResult CustomerBasicDataApplication()
        {
            LogHelper.BackInfo("2-3", Masterpage.AdminCurrUser.userid, "访问资料申请审核");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            data.customers = customers;
            return View(data);
        }
        [ValidateInput(false)]
        [AjaxAction(ForAction = "CustomerBasicDataApplication", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerBasicDataApplication(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _BasicDataApplicationRepos.GetAllCustomerBasicDataApplication(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            data.otherParam = otherparam;
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个资料申请审核
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [ValidateInput(false)]
        [AjaxAction(ForAction = "CustomerBasicDataApplication", ForController = "Admin_Customer")]
        public ActionResult AddCustomerBasicDataApplication(int? code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.CustomerBasicDataApplication();

            if (hidtype == "update")
            {

                return PartialView(_BasicDataApplicationRepos.GetCustomerBasicDataApplicationByCode(code));
            }
            return PartialView(data);
        }
        [ValidateInput(false)]
        [HttpPost]
        [AjaxAction(ForAction = "CustomerBasicDataApplication", ForController = "Admin_Customer")]
        public ActionResult AddCustomerBasicDataApplication(Entities::Models.CustomerBasicDataApplication instance, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            try
            {

                if (hidtype == "update")
                {
                    _BasicDataApplicationRepos.UpdateService(instance);
                    LogHelper.Info(Masterpage.AdminCurrUser.userid, "2-3:" + "客户资料申请审核，" + JsonHelper.ToJson(instance));
                }
            }
            catch(Exception ex)
            {
                LogHelper.Info(Masterpage.AdminCurrUser.userid, "2-3:" + "客户资料申请审核异常，" + ex.Message+",审核内容为："+JsonHelper.ToJson(instance));           
            }
            data.one = instance;
            return PartialView(data);

        }
        /// <summary>
        /// 删除一个资料申请审核
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerBasicDataApplication", ForController = "Admin_Customer")]
        public ActionResult DeleteCustomerBasicDataApplication(int code)
        {
            try
            {
                _BasicDataApplicationRepos.DeleteService(code);
                LogHelper.BackInfo("2-3", Masterpage.AdminCurrUser.userid, "删除资料申请" + code);

            }
            catch
            { }
            return RedirectToAction("CustomerBasicDataApplication");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "CustomerBasicDataApplication", ForController = "Admin_Customer")]
        public ActionResult GetCustomerBasicDataApplicationCode(int code)
        {

            var list = _BasicDataApplicationRepos.GetCustomerBasicDataApplicationByCode(code);
            var c = new
            {
                a = list.CustomerBasicDataApplicationId,
                b = list.UserGuid,
                e = list.CustomerCode,
                f = list.CustomerBasicDataApplicationTime.HasValue ? list.CustomerBasicDataApplicationTime.Value.ToString("yyyy-MM-dd") : "",
                g = list.CustomerBasicDataApplicationContent,
                h = list.CustomerBasciDataApplicant,
                i = list.CustomerBasicDataApplicationApproval,
                j = list.CustomerBasicDataApplicationRemark
            };
            return Json(c);
        }
        #endregion =================== CustomerBasicDataApplication Ending ==================

        #region 客户价格管理

        public ActionResult CustomerPrice()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerPrice", ForController = "Admin_Customer")]
        public ActionResult GetCustomerPrice(string customer)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            LogHelper.BackInfo("2-8", Masterpage.AdminCurrUser.userid, "读取客户" + customer + "能资源价格表");
            var list = _customerRepos.GetEnergyPriceManagement(customer);
            if (list == null || list.Count < 1)
            {
                return Json(new CustomerEnergyPrice(), JsonRequestBehavior.AllowGet);
            }
            return Json(list[0], JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "CustomerPrice", ForController = "Admin_Customer")]
        public ActionResult CustomerPriceList(int? page, int? pagesize, string customer)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            LogHelper.BackInfo("2-8", Masterpage.AdminCurrUser.userid, "读取客户能资源价格表");
            var list = _customerRepos.GetEnergyPriceManagement(customer);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.customer = customer;

            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        //names: names, prices: prices, units: units, dates: dates, cates: cates
        [AjaxAction(ForAction = "CustomerPrice", ForController = "Admin_Customer")]
        public ActionResult SaveCustomerPrice(string customer, string names, string prices, string units, string dates, string cates)
        {
            string[] n = names.Split(',');
            string[] p = prices.Split(',');
            string[] u = units.Split(',');
            string[] d = dates.Split(',');
            string[] c = cates.Split(',');
            LogHelper.BackInfo("2-8", Masterpage.AdminCurrUser.userid, "修改" + customer + "能资源价格，对应值为:[names " + names + "];[prices " + prices + "];[units " + units + "];[dates " + dates + "];[cates " + cates + "]");
            bool r = _customerRepos.SaveCustomerPrice(customer, n, c, p, u, d);
            return Json(new { states = r }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 数据导入
        [AjaxAction(ForAction = "BulkCollection,BulkDailyAvgData", ForController = "Admin_Customer")]
        public ActionResult uploadtoshare()
        {
            var imgpath = ConfigurationManager.AppSettings["ServiceShare"];
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
                if (exn.ToLower() != ".csv" && exn.ToLower() != ".txt")
                {
                    r = new ReturnValue { status = "error", message = "请上传.csv或.txt" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                var uploadsPath = Path.Combine(imgpath, fn);
                httpfile.SaveAs(uploadsPath);
                LogHelper.BackInfo("6-3", Masterpage.AdminCurrUser.userid, "上传导入文件到目录" + uploadsPath);
                r = new ReturnValue { status = "ok", value = uploadsPath };
            }
            else
            {
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BulkCollection()
        {
            LogHelper.BackInfo("6-1", Masterpage.AdminCurrUser.userid, "访问导入客户采集点页面");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            return View(data);
        }
        [AjaxAction(ForAction = "BulkCollection", ForController = "Admin_Customer")]
        public ActionResult StartBulkCollection(string customer, string path, int clear)
        {
            var r = _customerRepos.StartBulkCollection(customer, path, clear);
            LogHelper.BackInfo("6-1", Masterpage.AdminCurrUser.userid, "导入" + customer + "客户采集点，文件路径为" + path + "，是否清除原有数据：" + clear);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "BulkCollection", ForController = "Admin_Customer")]
        public ActionResult CustomerCollectionListView(int? page, int? pagesize, string customer, string unit, string point)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = _customerRepos.GetCustomerCollection(customer, unit, point);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.customer = customer;
            data.unit = unit;
            data.point = point;

            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (customer != "") otherparam += "&customer=" + customer;
            if (unit != "") otherparam += "&unit=" + unit;
            if (point != "") otherparam += "&point=" + point;
            data.otherParam = otherparam;
            return PartialView(data);
        }


        public ActionResult BulkDailyAvgData()
        {
            LogHelper.BackInfo("6-2", Masterpage.AdminCurrUser.userid, "访问导入客户采集点历史数据页面");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            return View(data);
        }
        [AjaxAction(ForAction = "BulkDailyAvgData", ForController = "Admin_Customer")]
        public ActionResult StartBulkDailyAvgData(string customer, string path, int clear)
        {
            var r = _customerRepos.StartBulkDailyAvgData(customer, path, clear);
            LogHelper.BackInfo("6-2", Masterpage.AdminCurrUser.userid, "导入" + customer + "客户采集点历史数据，文件路径为" + path + "，是否清除原有数据：" + clear);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "BulkDailyAvgData", ForController = "Admin_Customer")]
        public ActionResult CustomerDailyAvgDataView(int? page, int? pagesize, string customer)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = _customerRepos.GetCollectionDailyAvgData(customer);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 10;
            var vs = list.ToPagedList(_page, _pagesize);
            data.customer = customer;

            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (customer != "") otherparam += "&customer=" + customer;
            data.otherParam = otherparam;
            return PartialView(data);
        }
        #endregion

        #region ================== CustomerSOP Beginning ===================

        public ActionResult CustomerSOP(string first)
        {
            LogHelper.BackInfo("2-7", Masterpage.AdminCurrUser.userid, "访问客户SOP");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.first = first;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerSOP", ForController = "Admin_Customer")]
        public ActionResult IndexCustomerSOP(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CustomerSOPRepos.GetAllCustomerSOP(name);

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
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
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
        public ActionResult AddCustomerSOP(string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;

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
        public ActionResult SaveCustomerSOP(Entities::Models.CustomerSOP instance, string hidtype)
        {


            try
            {
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

            return RedirectToAction("CustomerSOP", new { first = instance.CustomerSopGuid });

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
            string backpath = "../res/" + code + "/sop";

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

        #region 工艺流程图上传
        [AjaxAction(ForAction = "FlowChart", ForController = "Admin_Customer")]
        public ActionResult uploadflowchart(string child)
        {
            //var imgpath = ConfigurationManager.AppSettings["FlowChart"];
            string imgpath = "~/Scripts/flex/";
            if (child != null && child != "")
            {
                imgpath += child + "/";
            }
            string uploadsFolder = HttpContext.Server.MapPath(imgpath);
            var httpfile = Request.Files["myfile"];
            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fn = httpfile.FileName;
                var exn = fn.Substring(fn.LastIndexOf("."));
                if (exn.ToLower() != ".swf")
                {
                    r = new ReturnValue { status = "error", message = "请上传.csv或.txt" };
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                var uploadsPath = Path.Combine(uploadsFolder, fn);
                httpfile.SaveAs(uploadsPath);
                LogHelper.BackInfo("2-9", Masterpage.AdminCurrUser.userid, "上传Flex文件到目录" + uploadsPath);
                r = new ReturnValue { status = "ok", value = uploadsPath };
            }
            else
            {
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FlowChart()
        {
            LogHelper.BackInfo("2-9", Masterpage.AdminCurrUser.userid, "访问工艺流程图上传页面");
            dynamic data = new System.Dynamic.ExpandoObject();
            var customer = _customerRepos.GetCustomer();
            data.customer = customer;
            return View(data);
        }
        #endregion

        #region  客户标准单元
        public ActionResult CustomerStandardUnit()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var customers = _customerRepos.GetCustomer().Select(p => new SelectListItem { Text = p.CustomerFullName, Value = p.CustomerCode }).ToList();
            data.customer = customers;
            return View(data);
        }
        [AjaxAction(ForAction = "CustomerStandardUnit", ForController = "Admin_Customer")]
        public ActionResult CustomerStandardUnitList(int? page, int? pagesize,string customer, string name)
        {
            LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, "访问客户标准单元");
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _customerRepos.GetCustomerStandardProcessUnit(customer, name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 20;
            var vs = list.ToPagedList(_page, _pagesize);
            data.customer = customer;
            data.name = name;
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            string otherparam = "";
            if (name != "") otherparam += "&name=" + name;
            if (customer != "") otherparam += "&customer=" + customer;
            data.otherParam = otherparam;
            return PartialView(data);
        }
       [AjaxAction(ForAction = "CustomerStandardUnit", ForController = "Admin_Customer")]
        public ActionResult OneCustomerStandardUnit(string customer, int? unitid, string type)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            ReturnValue r = new ReturnValue();
            CustomerStandardProcessUnit one = new CustomerStandardProcessUnit();
            if (unitid.HasValue && unitid.Value != 0)
            {
                one = _customerRepos.GetOneCustomerStandardProcessUnit(customer, unitid.Value);
            }
            var units = _customerRepos.GetAllStandardUnit();
            data.units = units;
            data.r = r;
            data.one = one;
            data.customer = customer;
            data.unitid = unitid;
            data.type = type;
            return PartialView(data);
        }
       [AjaxAction(ForAction = "CustomerStandardUnit", ForController = "Admin_Customer")]
       public ActionResult DelCustomerStandardUnit(string customer, int unitid)
       {

           ReturnValue r = new ReturnValue { status = "ok" };

           r = _customerRepos.DeleteCustomerStandardUnit(customer, unitid);
           LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, "删除员工:");
           return Json(r, JsonRequestBehavior.AllowGet);
       }
       [AjaxAction(ForAction = "CustomerStandardUnit", ForController = "Admin_Customer")]
       public ActionResult SaveCustomerStandardUnit(string customer, string type, int unitid)
       {

           string name = WebRequest.GetString("name", true);
           string unit = WebRequest.GetString("unit", true);
           string postion = WebRequest.GetString("postion", true);
           string remark = WebRequest.GetString("remark", true);
           ReturnValue r = new ReturnValue();

           r = _customerRepos.SaveCustomerStandardUnit(type, unitid, customer, unit, name, postion, remark);
           if (type == "edit")
           {
               type = "修改";
           }
           if (type == "add")
           {
               type = "添加";
           }
           LogHelper.BackInfo("4-3", Masterpage.AdminCurrUser.userid, type + "客户标准单元(" + "单元编码:" + unit + "," + "单元名称：" + name + ")");
           return Json(r, JsonRequestBehavior.AllowGet);
       }
        #endregion
    }
}
