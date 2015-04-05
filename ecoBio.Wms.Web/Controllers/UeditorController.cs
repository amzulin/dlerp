using ecoBio.Wms.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoBio.Wms.Web.Controllers
{
    public class UeditorController : BaseController
    {
        //
        // GET: /Ueditor/

        public ActionResult Index()
        {
            return View();
        }

        [LoginAllow]
        public ActionResult fileup()
        {
            //上传配置
            String pathbase = "~/Content/ueditor/file/";                                      //保存路径
            string[] filetype = { ".rar", ".doc", ".docx", ".zip", ".pdf", ".txt", ".swf", ".wmv" };    //文件允许格式
            int size = 100;   //文件大小限制,单位MB,同时在web.config里配置环境默认为100MB


            //上传文件
            Hashtable info = new Hashtable();

            UeditUploader up = new UeditUploader();
            info = up.upFile(pathbase, filetype, size); //获取上传状态

            return Content("{'state':'" + info["state"] + "','url':'" + info["url"] + "','fileType':'" + info["currentType"] + "','original':'" + info["originalName"] + "'}"); //向浏览器返回数据json数据

        }

        [LoginAllow]
        public ActionResult imageup()
        { //上传配置
            int size = 2;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            UeditUploader up = new UeditUploader();

            string pathbase = null;
            int path = Convert.ToInt32(up.getOtherInfo("dir"));
            if (path == 1)
            {
                pathbase = "~/Content/ueditor/image/";

            }
            else
            {
                pathbase = "~/Content/ueditor/image/";
            }

            info = up.upFile(pathbase, filetype, size);                   //获取上传状态

            string title = up.getOtherInfo("pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo("fileName");                //获取原始文件名

            string url = info["url"].ToString();
            url = url.Substring(23);
            return Content("{'url':'" + url + "','title':'" + title + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        [LoginAllow]
        public ActionResult imagemanager()
        {
            string[] paths = { "upload", "upload1" }; //需要遍历的目录列表，最好使用缩略图地址，否则当网速慢时可能会造成严重的延时
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                //文件允许格式


            String str = String.Empty;

            foreach (string path in paths)
            {
                DirectoryInfo info = new DirectoryInfo(HttpContext.Server.MapPath(path));

                //目录验证
                if (info.Exists)
                {
                    DirectoryInfo[] infoArr = info.GetDirectories();
                    foreach (DirectoryInfo tmpInfo in infoArr)
                    {
                        foreach (FileInfo fi in tmpInfo.GetFiles())
                        {
                            if (Array.IndexOf(filetype, fi.Extension) != -1)
                            {
                                str += path + "/" + tmpInfo.Name + "/" + fi.Name + "ue_separate_ue";
                            }
                        }
                    }
                }
            }

            return Content(str);

        }

        [LoginAllow]
        public ActionResult uploadify()
        {
            string type = WebRequest.GetString("type", true);
            string code = WebRequest.GetString("code", true);
            string uploadsFolder = "";
            #region 图片存储路径
            if (type=="operationlog")
            {
                uploadsFolder = HttpContext.Server.MapPath("~/Content/operationlog/" + code);
            }
            #endregion
            //string uploadsFolder = HttpContext.Server.MapPath("~/Content/uploadfile");
            Guid identifier = Guid.NewGuid();
            var uploadsPath = Path.Combine(uploadsFolder, identifier.ToString());
            var httpfile = Request.Files["Filedata"];
            var fn = httpfile.FileName;
            var exn = fn.Substring(fn.LastIndexOf("."));
            if (httpfile != null)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                httpfile.SaveAs(uploadsPath + exn);
                #region 图片自动裁剪
                if (exn.ToLower().Contains("jpg"))
                {
                    string sourceFile = uploadsPath + exn;// Server.MapPath("~/Content/images/" + name);//源图存放目录
                    string newFile = string.Empty; //新图路径
                    string newNewDir = Server.MapPath("~/Content/pressimg/");   //新图存放目录
                    newFile = Path.Combine(newNewDir, "正方形裁剪.jpg");
                    ImageCutZoom.CutForSquare(sourceFile, newFile, 200, 90);
                    newFile = Path.Combine(newNewDir, "180_240.jpg");
                    ImageCutZoom.CutForCustom(sourceFile, newFile, 240, 180, 100);
                    newFile = Path.Combine(newNewDir, "等比180_240.jpg");
                    ImageCutZoom.ZoomAuto(sourceFile, newFile, 240, 180, "", "");
                }
                #endregion
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

    }
}
