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

namespace ecoBio.Wms.Web.Controllers
{
    public class Admin_TeachController : AdminController//Controller //
    {
        private ecoBio.Wms.Service.Management.StandardUnitService _StandardUnitRepos = null;
        private ecoBio.Wms.Service.Management.MSpecificationService _MSpecificationRepos = null;
        private ecoBio.Wms.Service.Management.StandardTipService _StandardTipRepos = null;
        private ecoBio.Wms.Service.Management.StandardChartService _StandardChartRepos = null;
        private ecoBio.Wms.Service.Management.StandardReportService _StandardReportRepos = null;
        private ecoBio.Wms.Service.Management.SupplierService _SupplierRepos = null;
        private ecoBio.Wms.Service.Management.ProcessConstService _ProcessConstRepos = null;       
        private ecoBio.Wms.Service.Management.CollectionPointService _CollectionPointRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentSpecService _EquipmentSpecRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentKeyParameterLibService _EquipmentKeyParameterLibRepos = null;
        private ecoBio.Wms.Service.Management.CustomerConstructionKeyParameterService _CustomerConstructionKeyParameterRepos = null;
        private ecoBio.Wms.Service.Management.CustomerConstructionService _CustomerConstructionRepos = null;
        private ecoBio.Wms.Service.Management.EquipmentKeyParameterService _EquipmentKeyParameterRepos = null;
        
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="moduleFunctionRepos"></param>
        public Admin_TeachController(
            ecoBio.Wms.Backstage.Repositories.IStandardUnitRepository StandardUnitRepos,
                ecoBio.Wms.Backstage.Repositories.IMSpecificationRepository MSpecificationRepos,
                ecoBio.Wms.Backstage.Repositories.IStandardTipRepository StandardTipRepos,
            ecoBio.Wms.Backstage.Repositories.IStandardChartRepository StandardChartRepos,
            ecoBio.Wms.Backstage.Repositories.IStandardReportRepository StandardReportRepos,
                ecoBio.Wms.Backstage.Repositories.ISupplierRepository SupplierRepos,
                 ecoBio.Wms.Backstage.Repositories.IProcessConstRepository ProcessConstRepos,
            ecoBio.Wms.Backstage.Repositories.ICollectionPointRepository CollectionPointRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentSpecRepository EquipmentSpecRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentKeyParameterLibRepository EquipmentKeyParameterLibRepos,
             ecoBio.Wms.Backstage.Repositories.ICustomerConstructionKeyParameterRepository CustomerConstructionKeyParameterRepos, 
            ecoBio.Wms.Backstage.Repositories.ICustomerConstructionRepository CustomerConstructionRepos,
            ecoBio.Wms.Backstage.Repositories.IEquipmentKeyParameterRepository EquipmentKeyParameterRepos)
        {
            _StandardUnitRepos = new Service.Management.StandardUnitService(StandardUnitRepos);
            _MSpecificationRepos = new Service.Management.MSpecificationService(MSpecificationRepos);
            _StandardTipRepos = new Service.Management.StandardTipService(StandardTipRepos);
            _StandardChartRepos = new Service.Management.StandardChartService(StandardChartRepos);
            _StandardReportRepos = new Service.Management.StandardReportService(StandardReportRepos);
            _SupplierRepos = new Service.Management.SupplierService(SupplierRepos);
            _ProcessConstRepos = new Service.Management.ProcessConstService(ProcessConstRepos);
            _CollectionPointRepos = new Service.Management.CollectionPointService(CollectionPointRepos);
            _EquipmentSpecRepos = new Service.Management.EquipmentSpecService(EquipmentSpecRepos);
            _EquipmentKeyParameterLibRepos = new Service.Management.EquipmentKeyParameterLibService(EquipmentKeyParameterLibRepos);
            _CustomerConstructionKeyParameterRepos = new Service.Management.CustomerConstructionKeyParameterService(CustomerConstructionKeyParameterRepos);
            _CustomerConstructionRepos = new Service.Management.CustomerConstructionService(CustomerConstructionRepos);
            _EquipmentKeyParameterRepos = new Service.Management.EquipmentKeyParameterService(EquipmentKeyParameterRepos);
           
        }


        #region ================== CollectionPoint Beginning ===================

        public ActionResult CollectionPoint()
        {
            LogHelper.BackInfo("5-4", Masterpage.AdminCurrUser.userid, "访问采集点");
            return View();
        }
          [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
        public ActionResult IndexCollectionPoint(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _CollectionPointRepos.GetAllCollectionPoint(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new CollectionPoint();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.CollectionPointCode == first);
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
        /// 添加和更新一个采集点
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
        public ActionResult AddCollectionPoint(Entities::Models.CollectionPoint instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.CollectionPoint();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_CollectionPointRepos.GetCollectionPointByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
        public ActionResult SaveCollectionPoint(Entities::Models.CollectionPoint instance, string hidtype)
        {
           // dynamic data = new System.Dynamic.ExpandoObject();
             
            try
            {
                if (hidtype == "add")
                {
                    _CollectionPointRepos.AddService(instance);
                    LogHelper.BackInfo("5-4", Masterpage.AdminCurrUser.userid, "添加采集点" +instance.CollectionPointCode);
                }
                if (hidtype == "update")
                {
                    
                    _CollectionPointRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-4", Masterpage.AdminCurrUser.userid, "修改采集点"+instance.CollectionPointCode);
                }
            }
            catch
            {

            }
           // data.one = instance;
           // return PartialView(data);
            return RedirectToAction("CollectionPoint", new { first=instance.CollectionPointCode});
        }
        /// <summary>
        /// 删除一个采集点
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
        public ActionResult DeleteCollectionPoint(string code)
        {
            try
            {
                _CollectionPointRepos.DeleteService(code);
                LogHelper.BackInfo("5-4", Masterpage.AdminCurrUser.userid, "删除采集点"+code);
            }
            catch
            { }
            return RedirectToAction("CollectionPoint");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
        public ActionResult GetCollectionPointCode(string code)
        {

            var list = _CollectionPointRepos.GetCollectionPointByCode(code);
            var c = new
            {
                a = list.CollectionPointCode,
                b = list.CollectionPointName,
                e = list.CollectionPointHtml,
                f = list.CollectionPointRemark,
                g = list.CollectionPointUnit
            };
            return Json(c);
        }


          [AjaxAction(ForAction = "CollectionPoint", ForController = "Admin_Teach")]
          public ActionResult IsCollectionPointExit(string code, string hidtype)
          {
              var one = _CollectionPointRepos.GetCollectionPointByCode(code);
              if (hidtype == "add")
              {

                  return one == null ? Content("0") : Content("1");
              }
              return RedirectToAction("CollectionPoint");
          }
        #endregion =================== CollectionPoint Ending ==================

        #region ================== StandardChart Beginning ===================

        public ActionResult StandardChart(string first)
        {
            LogHelper.BackInfo("5-5", Masterpage.AdminCurrUser.userid, "访问标准图表");
            dynamic data = new System.Dynamic.ExpandoObject();
            data.first = first;
            return View();
        }
          [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
        public ActionResult IndexStandardChart(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _StandardChartRepos.GetAllStandardChart(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);

            var firstone = new StandardChart();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.StandardChartCode == first);
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
        /// 添加和更新一个标准图表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
  [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
        public ActionResult AddStandardChart(Entities::Models.StandardChart instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.StandardChart();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_StandardChartRepos.GetStandardChartByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
        public ActionResult saveStandardChart(Entities::Models.StandardChart instance, string hidtype)
        {
            
            try
            {
                if (hidtype == "add")
                {
                    _StandardChartRepos.AddService(instance);
                    LogHelper.BackInfo("5-5", Masterpage.AdminCurrUser.userid, "添加标准图表"+instance.StandardChartCode);
                }
                if (hidtype == "update")
                {
                    _StandardChartRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-5", Masterpage.AdminCurrUser.userid, "修改标准图表"+instance.StandardChartCode);
                }
            }
            catch
            {

            }

            return RedirectToAction("StandardChart", new { first = instance.StandardChartCode });

        }
        /// <summary>
        /// 删除一个标准图表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
        public ActionResult DeleteStandardChart(string code)
        {
            try
            {
                _StandardChartRepos.DeleteService(code);
                LogHelper.BackInfo("5-5", Masterpage.AdminCurrUser.userid, "删除标准图表"+code);
            }
            catch
            { }
            return RedirectToAction("StandardChart");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
        public ActionResult GetStandardChartCode(string code)
        {

            var list = _StandardChartRepos.GetStandardChartByCode(code);
            var c = new
            {
                a = list.StandardChartCode,
                b = list.StandardChartCategory,
                e = list.StandardChartWidth,
                f = list.StandardChartHeight,
                g = list.StandardChartTitle,
                h = list.StandardChartServiceUrl,
                i = list.StandardChartRequestAmount,
                j = list.StandardChartReturnRows,
                k = list.StandardChartLeftPrecision,
                l = list.StandardChartRightPrecision
            };
            return Json(c);
        }

          [AjaxAction(ForAction = "StandardChart", ForController = "Admin_Teach")]
          public ActionResult IsStandardChartExit(string code, string hidtype)
          {
              var one = _StandardChartRepos.GetStandardChartByCode(code);
              if (hidtype == "add")
              {

                  return one == null ? Content("0") : Content("1");
              }
              return RedirectToAction("StandardChart");
          }
        #endregion =================== StandardChart Ending ==================

        #region ================== StandardReport Beginning ===================

        public ActionResult StandardReport()
          {
              LogHelper.BackInfo("5-6", Masterpage.AdminCurrUser.userid, "访问标准报表");
            return View();
        }
          [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
        public ActionResult IndexStandardReport(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _StandardReportRepos.GetAllStandardReport(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new StandardReport();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.StandardReportCode == first);
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
        /// 添加和更新一个标准报表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
        public ActionResult AddStandardReport(Entities::Models.StandardReport instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.StandardReport();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_StandardReportRepos.GetStandardReportByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
        public ActionResult saveStandardReport(Entities::Models.StandardReport instance, string hidtype)
        {
            
            try
            {
                if (hidtype == "add")
                {
                    _StandardReportRepos.AddService(instance);
                    LogHelper.BackInfo("5-6", Masterpage.AdminCurrUser.userid, "添加标准报表"+instance.StandardReportCode);
                }
                if (hidtype == "update")
                {
                    _StandardReportRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-6", Masterpage.AdminCurrUser.userid, "修改标准报表"+instance.StandardReportCode);
                }
            }
            catch
            {

            }
            return RedirectToAction("StandardReport", new { first = instance.StandardReportCode });

        }
        /// <summary>
        /// 删除一个标准报表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
        public ActionResult DeleteStandardReport(string code)
        {
            try
            {
                _StandardReportRepos.DeleteService(code);
                LogHelper.BackInfo("5-6", Masterpage.AdminCurrUser.userid, "删除标准报表"+code);
            }
            catch
            { }
            return RedirectToAction("StandardReport");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
        public ActionResult GetStandardReportCode(string code)
        {

            var list = _StandardReportRepos.GetStandardReportByCode(code);
            var c = new
            {
                a = list.StandardReportCode,
                b = list.StandardReportCycle,
                e = list.StandardReportTemplate
            };
            return Json(c);
        }
          [AjaxAction(ForAction = "StandardReport", ForController = "Admin_Teach")]
          public ActionResult IStandardReportExit(string code, string hidtype)
          {
              var one = _StandardReportRepos.GetStandardReportByCode(code);
              if (hidtype == "add")
              {

                  return one == null ? Content("0") : Content("1");
              }
              return RedirectToAction("StandardReport");
          }
        #endregion =================== StandardReport Ending ==================

        #region ================== StandardTip Beginning ===================

        public ActionResult StandardTip()
          {
              LogHelper.BackInfo("5-7", Masterpage.AdminCurrUser.userid, "访问标准提醒");
            return View();
        }
          [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
        public ActionResult IndexStandardTip(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _StandardTipRepos.GetAllStandardTip(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new StandardTip();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.StandardTipsCode == first);
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
        /// 添加和更新一个标准报表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
        public ActionResult AddStandardTip(Entities::Models.StandardTip instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.StandardTip();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_StandardTipRepos.GetStandardTipByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
        public ActionResult saveStandardTip(Entities::Models.StandardTip instance, string hidtype)
        {
            
            try
            {
                if (hidtype == "add")
                {
                    _StandardTipRepos.AddService(instance);
                    LogHelper.BackInfo("5-7", Masterpage.AdminCurrUser.userid, "添加标准提醒"+instance.StandardTipsCode);
                }
                if (hidtype == "update")
                {
                    _StandardTipRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-7", Masterpage.AdminCurrUser.userid, "修改标准提醒"+instance.StandardTipsCode);
                }
            }
            catch
            {

            }
            return RedirectToAction("StandardTip", new { first = instance.StandardTipsCode });

        }
        /// <summary>
        /// 删除一个标准报表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
        public ActionResult DeleteStandardTip(string code)
        {
            try
            {
                _StandardTipRepos.DeleteService(code);
                LogHelper.BackInfo("5-7", Masterpage.AdminCurrUser.userid, "删除标准提醒"+code);
            }
            catch
            { }
            return RedirectToAction("StandardTip");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
        public ActionResult GetStandardTipCode(string code)
        {

            var list = _StandardTipRepos.GetStandardTipByCode(code);
            var c = new
            {  a = list.StandardTipsCode,
                b = list.StandardTipsCategory,
                e = list.StandardTipsTemplate,
                f = list.StandardTipsLength
            };
            return Json(c);
        }

          [AjaxAction(ForAction = "StandardTip", ForController = "Admin_Teach")]
          public ActionResult IsStandardTipExit(string code, string hidtype)
          {
              var one = _StandardTipRepos.GetStandardTipByCode(code);
              if (hidtype == "add")
              {

                  return one == null ? Content("0") : Content("1");
              }
              return RedirectToAction("StandardTip");
          }
        #endregion =================== StandardTip Ending ==================

        #region ================== MaterialSpecification Beginning ===================
       [ValidateInput(false)]
        public ActionResult MaterialSpecification()
        {
            LogHelper.BackInfo("5-8", Masterpage.AdminCurrUser.userid, "访问耗材规格");
            return View();
        }
          [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
        public ActionResult IndexMSpecification(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _MSpecificationRepos.GetAllMaterialSpecification(name);

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
        /// 添加和更新一个耗材规格表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [ValidateInput(false)]
        [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
        public ActionResult AddMSpecification(Entities::Models.MaterialSpecification instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var cc = _MSpecificationRepos.GetMaterialSpecificationList();
            data.cc = cc;
            data.one = new Entities::Models.MaterialSpecification();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_MSpecificationRepos.GetMSpecificationByCode(code));
            }
            return PartialView(data);
        }
          [ValidateInput(false)]
        [HttpPost]
        [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
        public ActionResult SaveMSpecification(Entities::Models.MaterialSpecification instance, string hidtype,string code,string name)
        {
            //dynamic data = new System.Dynamic.ExpandoObject();
            try
            {
                if (hidtype == "add")
                {
                    instance.MaterialSpecificationDisplayNo = _MSpecificationRepos.CreateNewDisplayNo(instance.MaterialSpecificationCode, instance.MaterialSpecificationName);
                    _MSpecificationRepos.AddService(instance);
                    LogHelper.BackInfo("5-8", Masterpage.AdminCurrUser.userid, "添加耗材规格"+instance.MaterialSpecificationCode);
                }
                if (hidtype == "update")
                {
                    instance.MaterialSpecificationDisplayNo = _MSpecificationRepos.CreateNewDisplayNo(instance.MaterialSpecificationCode, instance.MaterialSpecificationName);
                    _MSpecificationRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-8", Masterpage.AdminCurrUser.userid, "修改耗材规格"+instance.MaterialSpecificationCode);
                }
            }
            catch
            {

            }
            return RedirectToAction("MaterialSpecification");
            //data.one = instance;
            //return PartialView(data);

        }
        /// <summary>
        /// 删除一个耗材规格表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
        public ActionResult DeleteMSpecification(string code)
        {
            try
            {
                _MSpecificationRepos.DeleteService(code);
                LogHelper.BackInfo("5-8", Masterpage.AdminCurrUser.userid, "删除耗材规格"+code);
            }
            catch
            { }
            return RedirectToAction("MaterialSpecification");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
        public ActionResult GetMaterialSpecificationCode(string code)
        {

            var list = _MSpecificationRepos.GetMSpecificationByCode(code);
            var c = new
            {
                a = list.MaterialSpecificationCode,
                b = list.MaterialSpecificationName,
                e = list.MaterialSpecificationModel,
                f = list.MaterialSpecificationDisplayNo,
                g = list.MaterialSpecificationDisplayName,
                h = list.MaterialSpecificationKeyword,
                i = list.MaterialSpecificationCategeory,
                j = list.MaterialSpecificationUnit,
                k = list.ErpMaterialSpecificationCode,
                l = list.MaterialSpecificationRemark
            };
            return Json(c);
        }

         [AjaxAction(ForAction = "MaterialSpecification", ForController = "Admin_Teach")]
         public ActionResult IsMSpecificationExit(string code, string hidtype)
         {
             var one = _MSpecificationRepos.GetMSpecificationByCode(code);
             if (hidtype == "add")
             {

                 return one == null ? Content("0") : Content("1");
             }
             return RedirectToAction("MaterialSpecification");
         }
        #endregion =================== MaterialSpecification Ending ==================

        #region ================== Supplier Beginning ===================

         public ActionResult Supplier(string first)
        {
            LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "访问供应商");
            dynamic data = new System.Dynamic.ExpandoObject();           
            data.first = first;
            return View(data);
             
        }
         [AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
         public ActionResult IndexSupplier(int? page, int? pagesize, string name, string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _SupplierRepos.GetAllSupplier(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new Supplier();
            int firspage = -1;
            if (first != null && first != "")
            {
                try
                {
                    Guid g = Guid.Parse(first);
                    firstone = list.FirstOrDefault(p => p.SupplierGuid == g);
                    if (firstone != null)
                    {
                        firspage = vs.IndexOf(firstone);
                        if (firspage == -1)
                        {
                            vs.Insert(0, firstone);
                        }
                        else if (firspage > 0)
                        {
                            vs.Remove(firstone);
                            vs.Insert(0, firstone);
                        }
                        LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "供应商列表首行到顶,name:" + name + ",first:" + first + ",firstone:" + (firstone == null ? "null" : firstone.SupplierFullName) + ",firspage:" + firspage);
                    }
                    else
                    {
                        LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "供应商列表首行到顶出错，首行数据不存在,name:" + name + ",first:" + first + ",firstone:" + (firstone == null ? "null" : firstone.SupplierFullName) + ",firspage:" + firspage);

                    }
                }
                catch (Exception ex)
                {
                    LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "供应商列表异常，" + ex.Message + "name:" + name + ",first:" + first + ",firstone:" + (firstone == null ? "null" : firstone.SupplierFullName) + ",firspage:" + firspage);
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
            LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "供应商列表视图,name:" + name + ",_pagesize:" + _pagesize + ",_page:" + _page);
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个供应商
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
         public ActionResult AddSupplier(string hidtype)
         {
             dynamic data = new System.Dynamic.ExpandoObject();
             var cate = _SupplierRepos.GetSupplierList();
             data.cate = cate;
             data.one = new Entities::Models.Supplier();
             try
             {
                 if (hidtype == "add")
                 {
                     return PartialView(new Entities::Models.Supplier());
                 }
                 if (hidtype == "update")
                 {
                     Guid guid = new Guid();
                     return PartialView(_SupplierRepos.GetSupplierByGuid(guid));
                 }
             }
             catch (Exception ex)
             {
                 LogHelper.Info(Masterpage.AdminCurrUser.userid, "5-9:" + "AddSupplier" + (hidtype == "add" ? "添加供应商" : "修改供应商") + "异常，" + ex.Message);
             }
             return PartialView(data);
         }

         [HttpPost]
         [AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
         public ActionResult SaveSupplier(Entities::Models.Supplier instance, string hidtype)
         {
             //  dynamic data = new System.Dynamic.ExpandoObject();
             try
             {
                 if (hidtype == "add")
                 {
                     instance.SupplierGuid = Guid.NewGuid();
                     _SupplierRepos.AddService(instance);
                     LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "添加供应商" + instance.SupplierGuid + ":" + JsonHelper.ToJson(instance));
                 }
                 if (hidtype == "update")
                 {
                     _SupplierRepos.UpdateService(instance);
                     LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "修改供应商" + instance.SupplierGuid + ":" + JsonHelper.ToJson(instance));
                 }
             }
             catch (Exception ex)
             {
                 LogHelper.Info(Masterpage.AdminCurrUser.userid, "5-9:" + (hidtype == "add" ? "添加供应商" : "修改供应商") + "出错：" + ex.Message + ",供应商信息:" + JsonHelper.ToJson(instance));
             }
             //  data.one = instance;
             //  return PartialView(data);
             return RedirectToAction("Supplier", new { first = instance.SupplierGuid });
         }
        /// <summary>
        /// 删除一个供应商
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
        public ActionResult DeleteSupplier(Guid guid)
        {
            try
            {
                _SupplierRepos.DeleteService(guid);
                LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "删除供应商" + guid);
            }
            catch (Exception ex)
            {
                LogHelper.BackInfo("5-9", Masterpage.AdminCurrUser.userid, "删除供应商出错，" + ex.Message);
            }
            return RedirectToAction("Supplier");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
        public ActionResult GetSupplierCode(Guid guid)
        {

            var list = _SupplierRepos.GetSupplierByGuid(guid);
            var c = new
            {
                a = list.SupplierGuid,
                b = list.SupplierSimpleName,
                e = list.SupplierFullName,
                f = list.SupplierCategeory,
                g = list.SupplierAddress,
                h = list.SupplierTel,
                i = list.SupplierFax,
                j = list.SupplierWebsite,
                k = list.SupplierContact,
                l = list.SupplierContactMobile,
                m = list.SupplierContactMail,
                n = list.SupplierRemark

            };
            return Json(c);
        }

       //[AjaxAction(ForAction = "Supplier", ForController = "Admin_Teach")]
       //  public ActionResult IsSupplierExit(string name,string hidtype)
       //  {
       //      Guid guid = new Guid();
       //      var one = _SupplierRepos.GetSupplierByGuid(guid);
       //      if (hidtype == "add")
       //      {                
       //          return one == null ? Content("0") : Content("1");
       //      }
       //      return RedirectToAction("Supplier");
       //  }
        #endregion =================== Supplier Ending ==================

        #region ================== ProcessConst Beginning ===================

        public ActionResult ProcessConst()
        {
            LogHelper.BackInfo("5-10", Masterpage.AdminCurrUser.userid, "访问工艺常数"  );
            return View();
        }
         [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
        public ActionResult IndexProcessConst(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _ProcessConstRepos.GetAllProcessConst(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new ProcessConst();
            if (first != null && first != "")
            {
                firstone = list.FirstOrDefault(p => p.ProcessConstCode == first);
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
        /// 添加和更新一个工艺常数
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
        public ActionResult AddProcessConst(Entities::Models.ProcessConst instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.ProcessConst();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_ProcessConstRepos.GetProcessConstByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
        public ActionResult saveProcessConst(Entities::Models.ProcessConst instance, string hidtype)
        {
           
            try
            {
                if (hidtype == "add")
                {
                    _ProcessConstRepos.AddService(instance);
                    LogHelper.BackInfo("5-10", Masterpage.AdminCurrUser.userid, "添加工艺常数"+instance.ProcessConstCode);
                }
                if (hidtype == "update")
                {
                    _ProcessConstRepos.UpdateService(instance);
                    LogHelper.BackInfo("5-10", Masterpage.AdminCurrUser.userid, "修改工艺常数" + instance.ProcessConstCode);
                }
            }
            catch
            {

            }
            return RedirectToAction("ProcessConst", new { first = instance.ProcessConstCode });

        }
        /// <summary>
        /// 删除一个工艺常数
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
        public ActionResult DeleteProcessConst(string code)
        {
            try
            {
                _ProcessConstRepos.DeleteService(code);
                LogHelper.BackInfo("5-10", Masterpage.AdminCurrUser.userid, "删除工艺常数" + code);
            }
            catch
            { }
            return RedirectToAction("ProcessConst");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
         [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
        public ActionResult GetProcessConstCode(string code)
        {

            var list = _ProcessConstRepos.GetProcessConstByCode(code);
            var c = new
            {
                a = list.ProcessConstCode,
                b = list.ProcessConstName,
                e = list.ProcessConstValue,
                f = list.ProcessConstUnit,
                g = list.ProcessConstRemark
            };
            return Json(c);
        }
         [AjaxAction(ForAction = "ProcessConst", ForController = "Admin_Teach")]
         public ActionResult IsProcessConstExit(string code, string hidtype)
         {
             var one = _ProcessConstRepos.GetProcessConstByCode(code);
             if (hidtype == "add")
             {

                 return one == null ? Content("0") : Content("1");
             }
             return RedirectToAction("ProcessConst");
         }
        #endregion =================== ProcessConst Ending ==================

        #region ================== StandardProcessUnit Beginning ===================

        public ActionResult StandardProcessUnit()
         {
             LogHelper.BackInfo("5-1", Masterpage.AdminCurrUser.userid, "访问标准单元"  );
            return View();
        }
         [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
        public ActionResult IndexStandardUnit(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _StandardUnitRepos.GetAllStandardUnit(name).ToList();
            data.list = list;         
         
            return PartialView(data);

        }
        /// <summary>
        /// 添加和更新一个标准处理单元
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
        public ActionResult AddStandardUnit(Entities::Models.StandardProcessUnit instance, string code, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.StandardProcessUnit();
            if (hidtype == "add")
            {
                return PartialView(instance);
            }
            if (hidtype == "update")
            {

                return PartialView(_StandardUnitRepos.GetStandardUnitByCode(code));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
        public ActionResult saveStandardUnit(Entities::Models.StandardProcessUnit instance, string hidtype)
        {
             
            try
            {
                if (hidtype == "add")
                {
                    _StandardUnitRepos.AddService(instance);
                }
                if (hidtype == "update")
                {
                    _StandardUnitRepos.UpdateService(instance);
                }
            }
            catch
            {

            }
            return RedirectToAction("StandardProcessUnit");
        }
        /// <summary>
        /// 删除一个标准处理单元
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
          [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
        public ActionResult DeleteStandardUnit(string code)
        {
            try
            {
                _StandardUnitRepos.DeleteService(code);
            }
            catch
            { }
            return RedirectToAction("StandardProcessUnit");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
        public ActionResult GetStandardUnitCode(string code)
        {

            var list = _StandardUnitRepos.GetStandardUnitByCode(code);
            var c = new
            {
                a = list.StandardProcessUnitCode,
                b = list.StandardProcessUnitCode_parent,
                e = list.StandardProcessUnitName,
                f = list.StandardProcessUnitRemark
            };
            return Json(c);
        }
  [AjaxAction(ForAction = "StandardProcessUnit", ForController = "Admin_Teach")]
          public ActionResult IsStandardProcessUnitExit(string code, string hidtype)
          {
              var one = _StandardUnitRepos.GetStandardUnitByCode(code);
              if (hidtype == "add")
              {

                  return one == null ? Content("0") : Content("1");
              }
              return RedirectToAction("StandardProcessUnit");
          }
        #endregion =================== StandardProcessUnit Ending ==================

        #region ================== EquipmentSpec Beginning ===================

          public ActionResult EquipmentSpec(string first)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              data.first = first;
              return View(data);

          }
          [AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
        public ActionResult IndexEquipmentSpec(int? page, int? pagesize, string name,string first)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _EquipmentSpecRepos.GetAllEquipmentSpec(name);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
            var vs = list.ToPagedList(_page, _pagesize);
            var firstone = new EquipmentSpec();
            if (first != null && first != "")
            {
                Guid g = Guid.Parse(first);
                firstone = list.FirstOrDefault(p => p.EquipmentSpecGuid == g);
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
        [AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
        public ActionResult AddEquipmentSpec(string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var sup = _SupplierRepos.GetSupplier();
            data.sup = sup;
            var cate = _EquipmentSpecRepos.GetEquipmentSpecList();
            data.cate = cate; 
            data.one = new Entities::Models.EquipmentSpec();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.EquipmentSpec());
            }
            if (hidtype == "update")
            {
                Guid guid = new Guid();
                return PartialView(_EquipmentSpecRepos.GetEquipmentSpecByGuid(guid));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
        public ActionResult SaveEquipmentSpec(Entities::Models.EquipmentSpec instance, string hidtype)
        {
          //  dynamic data = new System.Dynamic.ExpandoObject();
          //  var sup = _SupplierRepos.GetSupplier();
           
          //  data.sup = sup;
            try
            {
                if (hidtype == "add")
                {
                    instance.EquipmentSpecGuid = Guid.NewGuid();
                    _EquipmentSpecRepos.AddService(instance);
                }
                if (hidtype == "update")
                {
                    _EquipmentSpecRepos.UpdateService(instance);
                }
            }
            catch
            {

            }
           // data.one = instance;
           /// return PartialView(data);
            return RedirectToAction("EquipmentSpec", new { first = instance.EquipmentSpecGuid });
        }
        /// <summary>
        /// 删除一个设备
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 

        [AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
        public ActionResult DeleteEquipmentSpec(Guid guid)
        {
            try
            {
                _EquipmentSpecRepos.DeleteService(guid);
            }
            catch
            { }
            return RedirectToAction("EquipmentSpec");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 

        [AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
        public ActionResult GetEquipmentSpecCode(Guid guid)
        {

            var list = _EquipmentSpecRepos.GetEquipmentSpecByGuid(guid);
            var c = new
            {
                a = list.EquipmentSpecGuid,
                b = list.EquipmentSpecName,
                e = list.EquipmentSpecType,
                f = list.EquipmentCategory,
                g = list.EquipmentUnit,
                h = list.EquipmentMaterial,
                i=list.SupplierGuid,
                j = list.EquipmentSpecRemark,
                k = list.EquipmentPic
            };
            return Json(c);
        }

      //[AjaxAction(ForAction = "EquipmentSpec", ForController = "Admin_Teach")]
      //  public ActionResult IsEquipmentSpecExit(string hidtype)
      //  {
      //     Guid guid=new Guid();
      //      var one = _EquipmentSpecRepos.GetEquipmentSpecByGuid(guid);
      //      if (hidtype == "add")
      //      {

      //          return one == null ? Content("0") : Content("1");
      //      }
      //      return RedirectToAction("EquipmentSpec");
      //  }
        #endregion =================== EquipmentSpec Ending ==================

        #region ================== KeyParameter Beginning ===================

        public ActionResult EquipmentKeyParameterLib()
        {
            return View();
        }
        [AjaxAction(ForAction = "EquipmentKeyParameterLib", ForController = "Admin_Teach")]
        public ActionResult IndexEquipmentKeyParameterLib(int? page, int? pagesize, string name)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (name == null) name = "";
            var list = _EquipmentKeyParameterLibRepos.GetAllEquipmentKeyParameterLib("设备",name);

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
        /// 添加和更新一个设备关键参数库
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "EquipmentKeyParameterLib", ForController = "Admin_Teach")]
        public ActionResult AddEquipmentKeyParameterLib(string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();           
            data.one = new Entities::Models.KeyParameter();
            if (hidtype == "add")
            {
                return PartialView(new Entities::Models.KeyParameter());
            }
            if (hidtype == "update")
            {
                Guid guid = new Guid();
                return PartialView(_EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLibByGuid(guid));
            }
            return PartialView(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "EquipmentKeyParameterLib", ForController = "Admin_Teach")]
        public ActionResult AddEquipmentKeyParameterLib(Entities::Models.KeyParameter instance, string hidtype)
        {
            dynamic data = new System.Dynamic.ExpandoObject();          
            try
            {
                if (hidtype == "add")
                {
                    instance.KeyParameterGuid = Guid.NewGuid();
                    _EquipmentKeyParameterLibRepos.AddService(instance);
                }
                if (hidtype == "update")
                {
                    _EquipmentKeyParameterLibRepos.UpdateService(instance);
                }
            }
            catch
            {

            }
            data.one = instance;
            return PartialView(data);

        }
        /// <summary>
        /// 删除一个设备关键参数库
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [AjaxAction(ForAction = "EquipmentKeyParameterLib", ForController = "Admin_Teach")]
        public ActionResult DeleteKeyParameter(Guid guid)
        {
            try
            {
                _EquipmentKeyParameterLibRepos.DeleteService(guid);
            }
            catch
            { }
            return RedirectToAction("EquipmentKeyParameterLib");
        }
        /// <summary>
        /// 编辑时获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
           [AjaxAction(ForAction = "EquipmentKeyParameterLib", ForController = "Admin_Teach")]
        public ActionResult GetKeyParameterCode(Guid guid)
        {

            var list = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLibByGuid(guid);
            var c = new
            {
                a = list.KeyParameterGuid,
                b = list.KeyParameterName,
                e = list.KeyParameterUnit,
                f = list.KeyParameterAlias,
                g = list.KeyParameterRemark,
                c= list.KeyParameterCategory              
            };
            return Json(c);
        }
        #endregion =================== KeyParameter Ending ==================

        #region ================== ConstructionKeyParameter Beginning ===================
         [ValidateInput(false)]
        public ActionResult ConstructionKeyParameter()
        {
            return View();
        }
        //  [AjaxAction(ForAction = "ConstructionKeyParameter", ForController = "Admin_Teach")]
        //public ActionResult IndexConstructionKeyParameter(int? page, int? pagesize, string name)
        //{
        //    dynamic data = new System.Dynamic.ExpandoObject();
        //    if (name == null) name = "";
        //    var list = _ConstructionKeyParameterRepos.GetAllConstructionKeyParameter(name);

        //    int _page = page.HasValue ? page.Value : 1;
        //    int _pagesize = pagesize.HasValue ? pagesize.Value : 12;
        //    var vs = list.ToPagedList(_page, _pagesize);
        //    data.name = name;
        //    data.list = vs;
        //    data.pageSize = _pagesize;
        //    data.pageIndex = _page;
        //    data.totalCount = vs.TotalCount;
        //    string otherparam = "";
        //    if (name != "") otherparam += "&name=" + name;
        //    data.otherParam = otherparam;
        //    return PartialView(data);

        //}
        ///// <summary>
        ///// 添加和更新一个构筑物关键参数库
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        ///// 
        // [ValidateInput(false)]
        // [AjaxAction(ForAction = "ConstructionKeyParameter", ForController = "Admin_Teach")]
        //public ActionResult AddConstructionKeyParameter(string hidtype)
        //{
        //    dynamic data = new System.Dynamic.ExpandoObject();
        //    data.one = new Entities::Models.ConstructionKeyParameter();
        //    if (hidtype == "add")
        //    {
        //        return PartialView(new Entities::Models.ConstructionKeyParameter());
        //    }
        //    if (hidtype == "update")
        //    {
        //        Guid guid = new Guid();
        //        return PartialView(_ConstructionKeyParameterRepos.GetConstructionKeyParameterByGuid(guid));
        //    }
        //    return PartialView(data);
        //}
        // [ValidateInput(false)]
        //[HttpPost]
        //[AjaxAction(ForAction = "ConstructionKeyParameter", ForController = "Admin_Teach")]
        //public ActionResult AddConstructionKeyParameter(Entities::Models.ConstructionKeyParameter instance, string hidtype)
        //{
        //    dynamic data = new System.Dynamic.ExpandoObject();
        //    try
        //    {
        //        if (hidtype == "add")
        //        {
        //            _ConstructionKeyParameterRepos.AddService(instance);
        //        }
        //        if (hidtype == "update")
        //        {
        //            _ConstructionKeyParameterRepos.UpdateService(instance);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    data.one = instance;
        //    return PartialView(data);

        //}
        ///// <summary>
        ///// 删除一个构筑物关键参数库
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        ///// 
        //  [AjaxAction(ForAction = "ConstructionKeyParameter", ForController = "Admin_Teach")]
        //public ActionResult DeleteConstructionKeyParameter(Guid guid)
        //{
        //    try
        //    {
        //        _ConstructionKeyParameterRepos.DeleteService(guid);
        //    }
        //    catch
        //    { }
        //    return RedirectToAction("ConstructionKeyParameter");
        //}
        ///// <summary>
        ///// 编辑时获取数据
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        ///// 
        //  [AjaxAction(ForAction = "ConstructionKeyParameter", ForController = "Admin_Teach")]
        //public ActionResult GetConstructionKeyParameterCode(Guid guid)
        //{

        //    var list = _ConstructionKeyParameterRepos.GetConstructionKeyParameterByGuid(guid);
        //    var c = new
        //    {
        //        a = list.ConstructionKeyParameterGuid,
        //        b = list.ConstructionKeyParameterName,
        //        e = list.ConstructionKeyParameterUnit,
        //        f = list.ConstructionKeyParameterAlias,
        //        g = list.ConstructionKeyParameterRemark
        //    };
        //    return Json(c);
        //}
        #endregion =================== ConstructionKeyParameter Ending ==================

          #region ================== CustomerConstructionKeyParameter Beginning ===================

          public ActionResult CustomerConstructionKeyParameter()
          {
              return View();
          }
          [AjaxAction(ForAction = "CustomerConstructionKeyParameter", ForController = "Admin_Teach")]
          public ActionResult IndexCustomerConstructionKeyParameter(int? page, int? pagesize, string name)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              if (name == null) name = "";
              var list = _CustomerConstructionKeyParameterRepos.GetAllCustomerConstructionKeyParameter(name);

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
          /// 添加和更新一个采集点
          /// </summary>
          /// <param name="guid"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "CustomerConstructionKeyParameter", ForController = "Admin_Teach")]
          public ActionResult AddCustomerConstructionKeyParameter( string hidtype)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              var con = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLib("构筑物");
              data.con = con;
              var cc = _CustomerConstructionRepos.GetCustomerConstruction();
              data.cc = cc;
              data.one = new Entities::Models.CustomerConstructionKeyParameter();
              if (hidtype == "add")
              {
                  return PartialView(new Entities::Models.CustomerConstructionKeyParameter());
              }
              if (hidtype == "update")
              {
                  Guid guid = new Guid();
                  return PartialView(_CustomerConstructionKeyParameterRepos.GetCustomerConstructionKeyParameterByCode(guid));
              }
              return PartialView(data);
          }

          [HttpPost]
          [AjaxAction(ForAction = "CustomerConstructionKeyParameter", ForController = "Admin_Teach")]
          public ActionResult AddCustomerConstructionKeyParameter(Entities::Models.CustomerConstructionKeyParameter instance, string hidtype)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              var con = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLib("构筑物");
              data.con = con;
              var cc = _CustomerConstructionRepos.GetCustomerConstruction();
              data.cc = cc;
              try
              {
                  if (hidtype == "add")
                  {
                      _CustomerConstructionKeyParameterRepos.AddService(instance);
                  }
                  if (hidtype == "update")
                  {
                      _CustomerConstructionKeyParameterRepos.UpdateService(instance);
                  }
              }
              catch
              {

              }
              data.one = instance;
              return PartialView(data);

          }
          /// <summary>
          /// 删除一个采集点
          /// </summary>
          /// <param name="guid"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "CustomerConstructionKeyParameter", ForController = "Admin_Teach")]
          public ActionResult DeleteCustomerConstructionKeyParameter(Guid guid)
          {
              try
              {
                  _CustomerConstructionKeyParameterRepos.DeleteService(guid);
              }
              catch
              { }
              return RedirectToAction("CustomerConstructionKeyParameter");
          }
          /// <summary>
          /// 编辑时获取数据
          /// </summary>
          /// <param name="code"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "CustomerConstructionKeyParameter", ForController = "Admin_Teach")]
          public ActionResult GetCustomerConstructionKeyParameterCode(Guid guid)
          {

              var list = _CustomerConstructionKeyParameterRepos.GetCustomerConstructionKeyParameterByCode(guid);
              var c = new
              {
                  a = list.KeyParameterGuid,
                  b = list.CustomerConstructionPositionNumber,
                  e = list.CustomerConstructionKeyParameterValue
              };
              return Json(c);
          }
          #endregion =================== CustomerConstructionKeyParameter Ending ==================

          #region ================== EquipmentKeyParameter Beginning ===================

          public ActionResult EquipmentKeyParameter()
          {
              return View();
          }
          [AjaxAction(ForAction = "EquipmentKeyParameter", ForController = "Admin_Teach")]
          public ActionResult IndexEquipmentKeyParameter(int? page, int? pagesize, string name, string specName)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              if (name == null) name = "";
              var list = _EquipmentKeyParameterRepos.GetEquipmentKeyParameterName(name, specName);

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
          /// 添加和更新一个设备关键参数
          /// </summary>
          /// <param name="guid"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "EquipmentKeyParameter", ForController = "Admin_Teach")]
          public ActionResult AddEquipmentKeyParameter(string hidtype)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              var con = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLib("设备");
              data.con = con;
              var cc = _EquipmentSpecRepos.GetEquipmentSpec();
              data.cc = cc;
              data.one = new Entities::Models.EquipmentKeyParameter();
              if (hidtype == "add")
              {
                  return PartialView(new Entities::Models.EquipmentKeyParameter());
              }
              if (hidtype == "update")
              {
                  Guid guid = new Guid();
                  return PartialView(_EquipmentKeyParameterRepos.GetEquipmentKeyParameterByCode(guid));
              }
              return PartialView(data);
          }

          [HttpPost]
          [AjaxAction(ForAction = "EquipmentKeyParameter", ForController = "Admin_Teach")]
          public ActionResult AddEquipmentKeyParameter(Entities::Models.EquipmentKeyParameter instance, string hidtype)
          {
              dynamic data = new System.Dynamic.ExpandoObject();
              var con = _EquipmentKeyParameterLibRepos.GetEquipmentKeyParameterLib("设备");
              data.con = con;
              var cc = _EquipmentSpecRepos.GetEquipmentSpec();
              data.cc = cc;
              try
              {
                  if (hidtype == "add")
                  {
                      _EquipmentKeyParameterRepos.AddService(instance);
                  }
                  if (hidtype == "update")
                  {
                      _EquipmentKeyParameterRepos.UpdateService(instance);
                  }
              }
              catch
              {

              }
              data.one = instance;
              return PartialView(data);

          }
          /// <summary>
          /// 删除一个设备关键参数
          /// </summary>
          /// <param name="guid"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "EquipmentKeyParameter", ForController = "Admin_Teach")]
          public ActionResult DeleteEquipmentKeyParameter(Guid guid)
          {
              try
              {
                  _EquipmentKeyParameterRepos.DeleteService(guid);
              }
              catch
              { }
              return RedirectToAction("EquipmentKeyParameter");
          }
          /// <summary>
          /// 编辑时获取数据
          /// </summary>
          /// <param name="code"></param>
          /// <returns></returns>
          /// 
          [AjaxAction(ForAction = "EquipmentKeyParameter", ForController = "Admin_Teach")]
          public ActionResult GetEquipmentKeyParameterCode(Guid guid)
          {

              var list = _EquipmentKeyParameterRepos.GetEquipmentKeyParameterByGuid(guid);
              if (list == null)
              {
                  return Content("0");
              }
              else
              {
                  var c = list.Select(p => new { a = p.KeyParameterGuid, b = p.EquipmentKeyParameterValue, e = p.EquipmentSpecGuid });
                  return Json(c);
              }
          }
          #endregion =================== EquipmentKeyParameter Ending ==================

     
    }
}
