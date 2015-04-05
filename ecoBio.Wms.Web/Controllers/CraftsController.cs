using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecoBio.Wms.Web;
using ecoBio.Wms.Data.Entities.Models;

using Calabonga.Mvc.PagedListExt;//只关心后台分页

//MvcContrib
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using ecoBio.Wms.Common;
using ecoBio.Wms.ViewModel;
using System.Data;
using iTextSharp.text.pdf;

namespace ecoBio.Wms.Web.Controllers
{
    /// <summary>
    /// 工艺监管
    /// </summary>
    public class CraftsController : BaseController
    {
        //
        // GET: /Crafts/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 进水负荷
        /// </summary>
        /// <returns></returns>
        public ActionResult waterload()
        {
            var c = new ecoBioWmsContext();
            //var list = from s in c.DataManualCollections orderby s.manual_collection_sn ascending select s;
           // list = list.OrderByDescending(s => s.manual_collection_sn);
            //return View(list.ToPagedList(1, 11));
            return View();
        }

        [HttpGet]
        [AjaxAction(ForAction = "waterload", ForController = "Crafts")]
        public JsonResult waterloaddata(int? page, string search)
        {
            var c = new ecoBioWmsContext();
            #region 2013年4月22日注释

            //var list = from s in c.DataManualCollections
            //           select new ecoBio.Wms.ViewModel.TableData
            //           {
            //               date = s.manual_collection_date,
            //               guid = s.process_unit_guid.Value,
            //               name = s.client_code,
            //               sn = s.manual_collection_sn,
            //               value = s.manual_collection_value.Value
            //           };

            //if (!String.IsNullOrEmpty(search))
            //{
            //    double v = Convert.ToDouble(search);
            //    list = list.Where(p => p.value == v);
            //}
            //list = list.OrderBy(s => s.sn);
            //int pageSize = 11;
            //int pageNumber = (page ?? 1);
            //PagedList<TableData> vs = list.ToPagedList(pageNumber, pageSize);
            //GridData gd = new GridData { Data = vs, IsNextPage = vs.IsNextPage, IsPreviousPage = vs.IsPreviousPage, PageIndex = vs.PageIndex, PageSize = vs.PageSize, TotalCount = vs.TotalPages, TotalPages = vs.TotalPages };

            ////object o=list.ToPagedList(pageNumber, pageSize);
            //var json = JsonHelper.ToJson(gd);
            //return Json(gd, JsonRequestBehavior.AllowGet);

            #endregion
            var json = JsonHelper.ToJson("");
            return Json(json);

        }

        /// <summary>
        /// 实时监控
        /// </summary>
        /// <returns></returns>
        public ActionResult monitoring()
        {
            return View();
        }


        /// <summary>
        /// 水质管理"searchString", ViewBag.search
        /// </summary>
        /// <returns></returns>
        public ActionResult waterquality(int? page, string searchString, GridSortOptions model)
        {
            var c = new ecoBioWmsContext();
            ViewBag.model = model;
            ViewBag.page = page;
            ViewBag.search = searchString;
            ViewBag.Column = "";
            ViewBag.Direction = "";

            //IEnumerable<DataManualCollection> list = (IEnumerable<DataManualCollection>)from s in c.DataManualCollections orderby s.manual_collection_sn descending select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    double v = Convert.ToDouble(searchString);
            //    list = list.Where(p => p.manual_collection_value == v);
            //    ViewBag.search = searchString;
            //}
            //if (!string.IsNullOrEmpty(model.Column))
            //{
            //    ViewBag.Column = model.Column;
            //    ViewBag.Direction = model.Direction;
            //    list = list.OrderBy(model.Column, model.Direction);
            //}

            //list = list.AsPagination(page ?? 1, 5);
            //return View(list); 
            
            return View();
        }


        /// <summary>
        /// 历史趋势//search,sn,date,value,sort
        /// </summary>
        /// <returns></returns>
        public ActionResult historytrend(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var c = new ecoBioWmsContext();
         
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
          


            //if (Request.HttpMethod == "GET")
            //{
            //    searchString = currentFilter;
            //}
            //else
            //{
            //    page = 1;
            //}
            //var list = from s in c.DataManualCollections  select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    double v = Convert.ToDouble(searchString);
            //    list = list.Where(p => p.manual_collection_value == v);
            //}
            //switch (sortOrder)
            //{
            //    case "Name desc":
            //        list = list.OrderByDescending(s => s.process_unit_guid); break;
            //    case "Date":
            //        list = list.OrderBy(s => s.manual_collection_date); break;
            //    case "Date desc":
            //        list = list.OrderByDescending(s => s.manual_collection_date); break;
            //    default:
            //        list = list.OrderBy(s => s.process_unit_guid); break;
            //}
            //int pageSize = 11; 
            //int pageNumber = (page ?? 1);

            //return View(list.ToPagedList(pageNumber, pageSize));

            return View();
        }


        /// <summary>
        /// 工艺报警  [LoginAllow]
        /// </summary>
        /// <returns></returns>    
        public ActionResult alarm()
        {
            return View();
        }

        /// <summary>
        /// 服务周报
        /// </summary>
        /// <returns></returns>
        public ActionResult weeklyreport()
        {
            return View();
        }
        [LoginAllow]
        public ActionResult ViewReport(string id)
        {
            ViewBag.name = id;
            return View();
        }
        [LoginAllow]
        public ActionResult showreport(string id)
        {
            ViewBag.name = id;
            return View();
        }

        [LoginAllow]
        [HttpPost]
        public ActionResult pdftoswf(int t)
        {
            bool r = false;
            string str = "";
            //switch (t)
            //{
            //    case 1:
            //        OfficeHelper.PDF2Swf("~\\Content\\pdf_swf\\", "~\\Content\\pdf_swf\\", "4.pdf");
            //        str = "成功";
            //        break;
            //    case 2:
            //        r = OfficeHelper.DOCConvertToPDF("~\\Content\\office_pdf\\1.docx", "~\\Content\\office_pdf\\1_doc.pdf");
            //        OfficeHelper.PDF2Swf("~\\Content\\office_pdf\\", "~\\Content\\pdf_swf\\", "1_doc.pdf");
            //        str = r.ToString();
            //        break;
            //    case 3:
            //        r = OfficeHelper.XLSConvertToPDF("~\\Content\\office_pdf\\1.xlsx", "~\\Content\\office_pdf\\1_xls.pdf");
            //        str = r.ToString();
            //        OfficeHelper.PDF2Swf("~\\Content\\office_pdf\\", "~\\Content\\pdf_swf\\", "1_xls.pdf");
            //        break;
            //    case 4:
            //        r = OfficeHelper.PPTConvertToPDF("~\\Content\\office_pdf\\1.pptx", "~\\Content\\office_pdf\\1_ppt.pdf");
            //        OfficeHelper.PDF2Swf("~\\Content\\office_pdf\\", "~\\Content\\pdf_swf\\", "1_ppt.pdf");
            //        str = r.ToString();
            //        break;
            //    case 5:
            //        int i = new Random().Next(100, 10000);
            //        str = i.ToString();
            //        break;
            //    case 6:
            //        int ic = new Random().Next(100, 10000);
            //        //PDFGenerator.PDFCreate("~\\Content\\my_pdf\\" + i.ToString() + ".pdf");
            //        PDFbyiTextSharp.Genera("~\\Content\\my_pdf\\" + ic.ToString() + ".pdf","这是传入的内容");
            //        str = "i.ToString()";
            //        SessionHelper.SetSession("NEWPDF", (ic.ToString() + ".pdf"));
            //        break;
            //    case 7:                    
            //        //OfficeHelper.ExcelRead("~\\Content\\excel\\a.xlsx", "~\\Content\\excel\\b.xlsx");

            //        DataTable dt1 = ExcelHelper.Read2003ToTable("~\\Content\\excel\\2003.xls");
            //        DataTable dt2 = ExcelHelper.Read2007ToTable("~\\Content\\excel\\2010.xlsx");
            //        string strex = ExcelHelper.Read2007ToString("~\\Content\\excel\\2010.xlsx");
            //        ViewBag.excel = strex;
            //        //str = i.ToString();
            //        break;
            //    default:
            //        break;
            //}
            return Content(str);
        }
        [LoginAllow]
        public ActionResult showhtml()
        {
            ViewBag.excel = ExcelHelper.Read2007ToString("~\\Content\\excel\\2010.xlsx");
            return View();
        }

        [LoginAllow]
        public ActionResult showpdf()
        {
            //ViewBag.excel = ExcelHelper.Read2007ToString("~\\Content\\excel\\2010.xlsx");
            
            return View();
        }
    }
}
