using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.ViewModel;
using Enterprise.Invoicing.Service;
using Enterprise.Invoicing.Entities;

namespace Enterprise.Invoicing.Web
{
    public static class Masterpage
    {

        /// <summary>
        /// 当前前台登录用户，只读
        /// 登录时初始化Session("LoginUser")得到
        /// </summary>
        public static LoginUser CurrUser
        {
            get
            {
                LoginUser loginuser = new LoginUser();
                if (SessionHelper.GetSession("LoginUser") != null)
                {
                    loginuser = (LoginUser)SessionHelper.GetSession("LoginUser");
                }
                return loginuser;
            }

        }

        /// <summary>
        /// 消息中心未读消息数量
        /// </summary>
        public static int MsgCount
        {
            get
            {

                string where = " recestaffid=" + Masterpage.CurrUser.staffid + " and isDelete=0 and isRead=0";
                var list = ServiceDB.Instance.QueryModelList<MsgReceModel>("select * from MsgReceModel where " + where);
                return list.Count;
            }
        }
        public static string GetIP()
        {
            string ip = "";
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip += Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            //if (string.IsNullOrEmpty(ip))
            ip += Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }

        /// <summary>
        /// 判断是否有权限
        /// 有 返回true 否则返回false
        /// </summary>
        /// <param name="functionNo"></param>
        /// <returns></returns>
        public static bool CheckRight(string functionNo)
        {
            if (SessionHelper.GetSession("MyFunctionString") != null)
            {
                var str = SessionHelper.GetSession("MyFunctionString").ToString();
                if (str.ToLower().Contains(functionNo.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

    }


}