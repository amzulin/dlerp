using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web
{
    /// <summary>
    /// 匿名访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousAttribute:Attribute
    {
    }

    /// <summary>
    /// 登录即可
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LoginAllowAttribute : Attribute
    {
    }
    /// <summary>
    /// 用于前台ajax请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxActionAttribute : Attribute
    {
        /// <summary>
        /// 需要的上级权限action名称,多个actionr用,分割
        /// </summary>
        public string ForAction { get; set; }
        /// <summary>
        /// 需要的上级权限Controller名称
        /// </summary>
        public string ForController { get; set; }
    }

    /// <summary>
    /// 用户中心权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UserCenterAttribute : Attribute
    {
        /// <summary>
        /// 管理员权限
        /// </summary>
        //public Nullable<bool> Admin { get; set; }
        public bool Admin { get; set; }
    }

    /// <summary>
    /// 权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple=false)]
    public class PermissionFilterAttribute : System.Web.Mvc.ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //bool IsIgnored = false;
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            Enterprise.Invoicing.Service.AccountService monitorservice = new Enterprise.Invoicing.Service.AccountService(new AccountRepository());

            #region 如果有配置允许的页面，则遍历
            ////获取当前配置保存起来的允许页面
            //var path = filterContext.HttpContext.Request.Path.ToLower();
            //IList<string> allowPages = ConfigSettings.GetAllAllowPage();
            //foreach (string page in allowPages)
            //{
            //    if (page.ToLower() == path)
            //    {
            //        IsIgnored = true;
            //        break;
            //    }
            //}
            //if (IsIgnored)
            //    return;
            #endregion

            #region 是否有加密码狗
            //Dog dog = new Dog(100);
            //// Read the string variable from the dog
            //dog.DogAddr = 0;			// The address read
            //dog.DogBytes = 10;			// The number of bytes read

            //dog.ReadDog();
            //if (dog.Retcode != 0)
            //{
            //    RedirectLogin(filterContext, "AutoNeedLogin", "未检测到加密锁！");
            //    return;
            //}

            #endregion

            #region 第一步：验证当前action是否是匿名访问action
            if (Checkanonymous(filterContext)) return;
            #endregion


            #region 第二步：登录验证

            if (filterContext.HttpContext.Session["LoginUser"] == null)//SessionHelper.GetSession("LoginUser")
            {
                RedirectLogin(filterContext, "AutoNeedLogin", "");
                return;
            }
            #endregion


            object[] isajax = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AjaxActionAttribute), true);
            object[] hasloginallow = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAllowAttribute), true);

            #region 第三步：如果是AjaxAction，则验证其需要的权限，不验证自身的权限
            if (isajax.Length >= 1)
            {
                var attr = (AjaxActionAttribute)isajax[0];
                var conName = attr.ForController;
                var actName = attr.ForAction;
                LoginUser loginuser = (LoginUser)SessionHelper.GetSession("LoginUser");
                //if (!WebAccountHelper.CheckHasModuleFunction(loginuser.role_guid, conName, actName))
                if (!monitorservice.CheckAjaxRight(loginuser.role_sn, conName, actName))
                {
                    RedirectLogin(filterContext, "AutoNeedLogin", "无ajax访问权限");
                    LogHelper.Info(loginuser.userid, "用户无:../" + controllerName + "/" + actionName + "的访问权限");
                }
            }
            #endregion

            #region 是否为用户中心权限
            object[] iscu = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UserCenterAttribute), true);
            if (iscu.Length >= 1)
            {
                var attr = (UserCenterAttribute)iscu[0];
                var isadmin = attr.Admin;
                LoginUser loginuser = (LoginUser)SessionHelper.GetSession("LoginUser");
                if (isadmin && !loginuser.IsAdmin)
                {
                    RedirectLogin(filterContext, "AutoNeedLogin", "需要管理员权限");
                }
            }
            #endregion

            #region 第四步：权限验证，如果没有找到LoginAllow标记也没有找到AjaxAction标记则需验证权限

            if (hasloginallow.Length < 1 && isajax.Length < 1 && iscu.Length < 1)
            {
                LoginUser loginuser = (LoginUser)SessionHelper.GetSession("LoginUser");
                if (!monitorservice.CheckAjaxRight(loginuser.role_sn, controllerName, actionName))
                {
                    RedirectLogin(filterContext, "AutoNeedLogin", "无访问权限");
                    LogHelper.Info(loginuser.userid, "用户无:../" + controllerName + "/" + actionName + "的访问权限");
                }
            }
            #endregion
        }


        public bool Checkanonymous(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了  
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AnonymousAttribute), true); 
            //是否是LoginAllowView  
            var ViewMethod = attrs.Length == 1;  
            return ViewMethod;
        }
        /// <summary>
        /// [LoginAllowView标记]验证是否登录就可以访问(如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了)
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckLoginAllowView(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAllowAttribute), true);
            //是否是LoginAllowView
            var ViewMethod = attrs.Length == 1;
            return ViewMethod;
        }

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="sessionname"></param>
        /// <param name="message"></param>
        public void RedirectLogin(ActionExecutingContext filterContext,string sessionname, string message)
        {
            if (sessionname != "" && message != "") SessionHelper.SetSession(sessionname, message);
            try
            {
                string host = WebRequest.GetCurrentFullHost();
                var login = "http://" + host + "/account/login";
                filterContext.Result = new ContentResult()
                {
                    Content = "<script type='text/javascript'>"
                    + "if(self!=top)"
                    + "parent.window.location.href='http://" + host + "/account/login';"
                    + "else "
                    + "window.location.href='http://" + host + "/account/login';"
                    + "</script>"
                };

            }
            catch (Exception ex)
            {
                LogHelper.Info("system", "前台页面跳转至登录页异常" + ex.Message);
            }
        }
    }
}