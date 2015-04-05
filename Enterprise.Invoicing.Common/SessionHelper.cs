using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Enterprise.Invoicing.Common
{
    public class SessionHelper
    {
        /// <summary>
        /// 当前SessionID
        /// </summary>
        public static string SessionId { get { return HttpContext.Current.Session.SessionID; } }
        // Methods
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 20;
        }

        public static void Add(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        public static void Adds(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 20;
        }

        public static void Adds(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
            HttpContext.Current.Session.Remove(strSessionName);
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
        }

        public static string Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return "";
            }
            return HttpContext.Current.Session[strSessionName].ToString();
        }

        public static string[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            return (string[])HttpContext.Current.Session[strSessionName];
        }

        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public static void SetSession(string name, object val)
        {
            //HttpContext.Current.Session.Remove(name);
            //HttpContext.Current.Session.Add(name, val);
            HttpContext.Current.Session[name] = val;
        }
    }

 

}
