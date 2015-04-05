using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
//using Enterprise.Invoicing.Web.Filters;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Entities.Models;
using System.Runtime.InteropServices;
using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.ViewModel;
using Enterprise.Invoicing.Service;
using System.Security.Cryptography;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class AccountController : Controller
    {
        private AccountService accountService;
        

        public AccountController(IAccountRepository _accountRepository)
        {
            accountService = new AccountService(_accountRepository);
        }

        public ActionResult Login(string url, string v1, string v2, string remember_me)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.url = url;
            data.v1 = v1;
            data.v2 = v2;
            data.message = SessionHelper.GetSession("AutoNeedLogin");//异地登录时，会有提示信息
            Session["WebSessionId"] = Session.SessionID;
            var sid = SessionHelper.SessionId;
            SessionHelper.SetSession("SessionHelperSessionId", sid);
            string uid = "";
            bool remember = false;
            var rmb = CookieHelper.GetCookieValue("remember_me");
            if (rmb != null && rmb == "on")
            {
                uid = CookieHelper.GetCookieValue("remember_userid");
                remember = true;
            }
            data.remember = remember;
            data.uid = uid;
            #region 加密狗
            //Dog dog = new Dog(100);
            //// Read the string variable from the dog
            //dog.DogAddr = 0;			// The address read
            //dog.DogBytes = 10;			// The number of bytes read

            //dog.ReadDog();
            //bool hdog = (dog.Retcode == 0);
              bool hdog = true;
            #endregion
             data.dog = hdog;
            return View(data);
        }


        
        #region ajax 登录后台
        [HttpPost]
        public ActionResult verification(string action, string pwd, string userid, string remember_me)
        {
            var id2 = Session.SessionID;
            var model = accountService.GetLoginModel(userid);
            if (model != null)
            {
                if (model.userpwd.ToLower() == MD5Helper.MD5_32(pwd).ToLower())
                {
                    model.login_time = DateTime.Now;
                    Session["LoginUser"] = model;
                    var sid = SessionHelper.SessionId;
                    var functions = accountService.GetMyFunctionString(Masterpage.CurrUser.role_sn);
                    Session["MyFunctionString"] = functions;
                    if (remember_me != null && remember_me == "on")
                    {
                        CookieHelper.SetCookie("remember_me", remember_me, DateTime.Now.AddMonths(1));
                        CookieHelper.SetCookie("remember_userid", userid, DateTime.Now.AddMonths(1));
                    }
                    else
                    {
                        CookieHelper.ClearCookie("remember_userid");
                        CookieHelper.ClearCookie("remember_me");
                    }
                    return Redirect("../main/index");
                }
                else
                {
                    return RedirectToAction(action, new { url = "", v1 = "", v2 = "密码输入错误" });
                    //back = new ReturnValue { status = false, value2 = "密码输入错误" }; 
                }
            }
            else
            {
                return RedirectToAction(action, new { url = "", v1 = "用户名不存在", v2 = "" });
                //return View("login", new { url = "", v1 = "用户名不存在", v2 = "" });
                //back = new ReturnValue { status = false,value="用户名不存在" };
            }
            //return Json(back, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult GetValidateCode(string time)
        {
            var id1 = Session.SessionID;
            //string validatecode = SessionHelper.Get("ValidateCode");
            ValidateCode vCode = new ValidateCode();
            string code =  vCode.CreateRandomCode(6);
            Session["LoginValidateCode"] = code;
            SessionHelper.SetSession("ValidateCode", code);
            byte[] bytes = vCode.CreateValidateGraphic(code);
           
            return File(bytes, @"image/jpeg");
        }

        public ActionResult LogOff()
        {
            #region 更新登录状态
            try
            {
                
                LogHelper.Info(Masterpage.CurrUser.name, "000002:退出前台");
            }
            catch
            { }
            #endregion

            SessionHelper.Clear();
         
            //清除保存用户的令牌号和类别到cookie
            //CookieHelper.ClearCookie("UserTokenCode");
            //CookieHelper.ClearCookie("IsEmployee");
            //CookieHelper.ClearCookie("UserClientCode");
            //CookieHelper.ClearCookie("UserClientName");

            return RedirectToAction("login", "account");
        }

        

    }
}
