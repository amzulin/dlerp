using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web
{
    public class LogExceptionFilterAttribute : FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            LogHelper.Error(string.Format("{0}.{1} {2}",
                filterContext.RouteData.Values["controller"],
                filterContext.RouteData.Values["action"],
                filterContext.Exception.Message));

        }
    }
}