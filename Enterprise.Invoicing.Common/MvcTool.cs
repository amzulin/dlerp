using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Common
{
    public static class MvcTool
    {
        /// <summary> 
        /// 获取IP      
        /// </summary>  
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }


        #region 扩展方法
        public static bool IsNullOrEmpty<T>(this T data)
        {
            return ((data == null) || (((data.GetType() == typeof(string)) && (string.IsNullOrEmpty(data.ToString().Trim()) || (data.ToString() == ""))) || (data.GetType() == typeof(DBNull))));
        }
        #endregion

    }
}
