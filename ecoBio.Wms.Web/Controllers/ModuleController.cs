using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.Common;
using ecoBio.Wms.Data.Entities.Models;
using ecoBio.Wms.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoBio.Wms.Web.Controllers
{
    public class ModuleController : BaseController
    { 
        public ActionResult grid()
        {
            return View();
        }
      
        public ActionResult uedit()
        {
            return View();
        }

        /// <summary>
        /// 表格数据page	1pagesize	20sortname	snsortorder	asc
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [AjaxAction(ForAction = "grid", ForController = "Module")]
        public ActionResult griddata(int? page, int? pagesize, string sortname, string snsortorder, string search)
        {
            var c = new ecoBioWmsContext();

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
            //int pageSize = (pagesize ?? 20); ;
            //int pageNumber = (page ?? 1);
            //PagedList<TableData> vs = list.ToPagedList(pageNumber, pageSize);
            //ligerGrid ligd = new ligerGrid { Rows=vs , Total=vs.TotalCount };
            //GridData gd = new GridData { Data = vs, IsNextPage = vs.IsNextPage, IsPreviousPage = vs.IsPreviousPage, PageIndex = vs.PageIndex, PageSize = vs.PageSize, TotalCount = vs.TotalPages, TotalPages = vs.TotalPages };

            //var json = JsonHelper.ToJson(ligd);
            //return Json(ligd, JsonRequestBehavior.AllowGet);


            var json = JsonHelper.ToJson("");
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult upload()
        {
            return View();
        }

        [AjaxAction(ForAction = "upload", ForController = "Module")]
        public Guid Swfupload()
        {
            string uploadsFolder = HttpContext.Server.MapPath("~/Content/uploadfile");
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(uploadsFolder, identifier.ToString());
            var httpfile = Request.Files["Filedata"];
            var fn = httpfile.FileName;
            var exn = fn.Substring(fn.LastIndexOf("."));
            if (httpfile != null)
            {
                httpfile.SaveAs(uploadsPath + exn);
            }

            return identifier;
        }
        public ActionResult mail()
        {
            return View();
        }

        [ValidateInput(false)]
        [AjaxAction(ForAction = "mail", ForController = "Module")]
        public ContentResult sendmail(string email, string title, string content)
        {

            string MessageTo = email; //收件人邮箱地址 
            string MessageSubject = title; //邮件主题 
            string MessageBody = content + new Random().Next(1000, 999999).ToString(); //邮件内容 
            //bool c = ecoBio.Wms.Common.MailHelper.Send(MessageTo, MessageSubject, MessageBody);
            bool c = true;
            MailHelper mh = new MailHelper();
            var img = Image64Base.GetImageStr("d:\\b.jpg");
            var html = "<html><head><title>title</title></head><body>这是内容<hr><br><img src=\"data:image/png;base64," + img + "\" /><br><img src=\"http://www.ecobiotech.com.cn/images/logo.gif\" /></body></html>";
            mh.Mail("amzulin@163.com", "zulin.meng@ecobiotech.com.cn", html, "title", "doumiao@ytk");
            //mh.Mail("5124743@qq.com", "zulin.meng@ecobiotech.com.cn", "<html><head><title>title</title></head><body>这是内容<hr><br><img src=\"data:image/png;base64," + img + "\" /></body></html>", "title", "doumiao@ytk");
           mh.Attachments("~\\Content\\my_pdf\\7459.pdf");
            string m = "成功";
            try
            {
                mh.Send();
            }
            catch (Exception ex)
            { m = ex.Message; }
            return Content(m);
        }
        public ActionResult md5()
        {
            return View();
        }

        [HttpPost]
        [AjaxAction(ForAction = "md5", ForController = "Module")]
        public ActionResult md5(string txtkey)
        {
            ViewBag.txt = txtkey;
            ViewBag.md5_16 = MD5Helper.MD5_16(txtkey);
            ViewBag.md5_32 = MD5Helper.MD5_32(txtkey);

            string des = DESHelper.DESEncrypt(txtkey, DESHelper.GetKey());
            ViewBag.des_E = des;
            ViewBag.des_D = DESHelper.DESDecrypt(des, DESHelper.GetKey());

            //LoginLog one = new LoginLog();
            //one.belong_to_company = "ecobio";
            //one.login_time = DateTime.Now;
            //one.login_user_alias = txtkey;
            //string xml = XmlSerializerHelper.SaveXmlFromObj<LoginLog>(one);
           

            string mdc = Image64Base.GetImageStr("d:\\a.jpg");
            ViewBag.xml = mdc;
            return View();
        }

        public ActionResult json()
        {
            return View();
        }

        public ActionResult pdf()
        {
            return View();
        }

        [HttpPost]
        [AjaxAction(ForAction = "pdf", ForController = "Module")]
        public ActionResult createpdf(string t)
        {
            //int ic = new Random().Next(100, 10000);
            //PDFbyiTextSharp.Genera("~\\Content\\my_pdf\\" + ic.ToString() + ".pdf", t);
            //PDFbyiTextSharp.pdfencry("~\\Content\\my_pdf\\" + ic.ToString() + ".pdf");
            //string str = ic.ToString();

            ////SessionHelper.SetSession("newpdfname", (ic.ToString() + ".pdf"));
            //OfficeHelper.XLSConvertToPDF("~\\Content\\excel\\new_2010.xlsx", "~\\Content\\excel\\new_2010.pfd");
            return Content("a");
        }

        [AjaxAction(ForAction = "pdf", ForController = "Module")]
        public ActionResult showpdf()
        {
            ViewBag.name = SessionHelper.Get("newpdfname");
            return View();
        }
        public ActionResult excel()
        {
            return View();
        }
        [AjaxAction(ForAction = "excel", ForController = "Module")]
        public ActionResult showexcel(string name)
        {
            if (name.EndsWith(".xlsx"))
            {
                ViewBag.excel = ExcelHelper.Read2007ToString("~\\Content\\excel\\" , name);
            }
            if (name.EndsWith(".xls"))
            {
                ViewBag.excel = ExcelHelper.Read2003ToString("~\\Content\\excel\\", name);
            }
            return View();
        }
        public ActionResult image()
        {
            return View();
        }
        [HttpPost]
        [AjaxAction(ForAction = "image", ForController = "Module")]
        public ContentResult imgpress(string name,int height,int width,int per)
        {

            string sourceFile = Server.MapPath("~/Content/images/" + name);//源图存放目录
            string newNewDir = Server.MapPath("~/Content/pressimg/");   //新图存放目录

            string newFile = string.Empty; //新图路径
            string result = "";
            string n = name.Substring(0, name.Length - 4);

            newFile = Path.Combine(newNewDir, n + "w_定高按宽度缩放.jpg");
            bool r = ImageCompress.Thumbnail(sourceFile, newFile, height, width, 80, ImageCompress.ZoomType.H);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "w_定宽按高度缩放.jpg");
            r = ImageCompress.Thumbnail(sourceFile, newFile, height, width, 80, ImageCompress.ZoomType.W);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "wh_指定高宽缩放.jpg");
            r = ImageCompress.Thumbnail(sourceFile, newFile, height, width, 80, ImageCompress.ZoomType.WH);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "wh_指定高宽裁剪.jpg");
            r = ImageCompress.Thumbnail(sourceFile, newFile, height, width, 80, ImageCompress.ZoomType.Cut);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "原图%" + per + ".jpg");
            r = ImageCompress.Cut(sourceFile, newFile, height, width, per, true);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "h_w指定高宽缩放.jpg");
            r = ImageCompress.Cut(sourceFile, newFile, height, width, 30, false);
            result += r.ToString();

            newFile = Path.Combine(newNewDir, n + "正方形裁剪.jpg");
            ImageCutZoom.CutForSquare(sourceFile, newFile, 200, 90);
            newFile = Path.Combine(newNewDir, n + "180_240.jpg");
            ImageCutZoom.CutForCustom(sourceFile, newFile, 240, 180, 100);
            newFile = Path.Combine(newNewDir, n + "等比180_240.jpg");
            ImageCutZoom.ZoomAuto(sourceFile, newFile, 240, 180, "", "");
            return Content(result);
        }

        public ActionResult flex()
        {
            return View();
        }
        [HttpPost]
        [AjaxAction(ForAction = "flex", ForController = "Module")]
        public ContentResult flex(string code, string type, string name, int row, string param)
        {
           // string r2 = FlexData.FlexService.GetDataSource(code, row, param);
            return Content("");
        }

        public ActionResult dailog()
        {
            return View();
        }
        [AjaxAction(ForAction = "dailog", ForController = "Module")]
        public ActionResult dailog_value()
        {
            return View();
        }
    }
}
