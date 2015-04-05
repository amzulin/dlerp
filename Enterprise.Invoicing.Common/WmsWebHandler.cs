using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Enterprise.Invoicing.Common
{
    public class WmsWebHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // 获取文件服务器端物理路径
            string FileName = context.Server.MapPath(context.Request.FilePath);
            // 如果UrlReferrer为空，则显示一张默认的禁止盗链的图片
            if (context.Request.UrlReferrer.Host == null)
            {
                context.Response.ContentType = "image/JPEG";
                context.Response.WriteFile("/error.jpg");
            }
            else
            {
                // 如果 UrlReferrer中不包含自己站点主机域名，则显示一张默认的禁止盗链的图片
                //string serverHost = context.Request.Url.Host;
                //Uri u = context.Request.UrlReferrer;
                //if (u == null || u.Host.ToLower() != serverHost.ToLower())
                if (context.Request.UrlReferrer.Host.IndexOf("yourdomain.com") > 0)
                {
                    context.Response.ContentType = "image/JPEG";
                    context.Response.WriteFile(FileName);
                }
                else
                {
                    //context.Response.ContentType = "image/JPEG";
                    //context.Response.WriteFile("/error.jpg");
                }
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }

    #region Corp
    // By : 公子哥 
    // .NET 实现ISAPI过滤器,文件防下载
    #endregion
    public class DownloadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        #region IHttpHandler 成员
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            //从Request对象中获取所请求文件的物理路径 
            string strNew = "";
            string strtmp = req.PhysicalPath;
            //unsafe
            //{
            //    fixed (char* pValue = strtmp)
            //    {
            //        char* pnew = pValue;
            //        pnew = pnew + strtmp.LastIndexOf(".");
            //        for (; *pnew != '\0'; ++pnew)
            //        {
            //            strNew += Convert.ToString(*pnew);
            //        }
            //    }
            //}
            //根据Session中UserName是否存在判断用户是否登陆
            if (context.Session["UserName"] == null)
            {
                //未登陆 
                System.Web.HttpContext.Current.Server.Transfer(ConfigurationSettings.AppSettings["LoginShow"].ToString());
            }
            else
            {
                //防.bokee文件,如果有多个文件应加入多个文件判断
                if (strNew.ToLower().Equals(ConfigurationSettings.AppSettings["FileTypes"].ToString().ToLower()))
                {
                    System.Web.HttpContext.Current.Server.Transfer(ConfigurationSettings.AppSettings["Error"].ToString());
                }
            }
        }
        #endregion

        #region 用户
        //最后配置 web.config文件

        //<appSettings> 
        //<add key="FileTypes" value=".bokee" /><!-- 要过滤的文件扩展名 -->
        //<add key="Error" value="./messageshow.html" /> <!-- 如果用户登陆但没有权限 -->
        //<add key="LoginShow" value="./loginshow.html" /> <!--用户未登陆时候的提示 -->

        //</appSettings> 


        //<httpHandlers>
        //<add verb="*" path="*.bokee" type="CheckDownload.DownloadHandler, download" />
        //</httpHandlers>
        #endregion
    }
}