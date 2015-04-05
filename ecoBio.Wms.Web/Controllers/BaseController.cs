using Enterprise.Invoicing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web.Controllers
{
    /// <summary>
    /// 默认需要验证有无权限，若不验证，请在action前加LoginAllow或Anonymous
    /// </summary>
    [PermissionFilter]
    public class BaseController:Controller
    {
        /// <summary>
        /// 发生异常后调用
        /// 如果本方法不Redirect，如果action的HandleError没有指定view，则页面跳转到默认error视图，url不变，指定了view则url也为error的url
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var rs = filterContext.HttpContext.Request.ServerVariables;
            string ip = filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
            //异常处理 
            string host = WebRequest.GetCurrentFullHost();
            //ecoBio.Wms.Common.SessionHelper.SetSession("ErrorMessage", filterContext.Exception.Message);

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            //LogHelper.Info(Masterpage.CurrUser.alias, "程序异常,controller:" + (controllerName != null ? controllerName : "未知") + ",action:" + (actionName != null ? actionName : "未知") + ",异常信息:" + filterContext.Exception.Message + "，客户IP:" + ip + ",主机：" + host);
            //页面跳转到error


            //filterContext.RequestContext.HttpContext.Response.Redirect("~/Account/Error");  //无权限         
        }
    }
}