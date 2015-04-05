using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web
{
    public class ActionHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //异常处理
            //Enterprise.Invoicing.Common.SessionHelper.SetSession("ErrorMessage", filterContext.Exception.Message);//

            //页面跳转到error
            filterContext.RequestContext.HttpContext.Response.Redirect("~/Account/error_action");  //无权限     
        }
    }
}