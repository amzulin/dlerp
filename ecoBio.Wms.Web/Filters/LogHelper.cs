using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.Invoicing.Web
{
    public class LogHelper
    {
        private static readonly log4net.ILog dbLog = log4net.LogManager.GetLogger("ecoBioInfo");
            //log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private static readonly log4net.ILog fileLog = log4net.LogManager.GetLogger("ecoBioError");

        private LogHelper(){}

        //保存至数据库
        public static void Info(string userId, string contents)
        {
            dbLog.Info(new LogContent {  Contents = contents,  UserId = userId });
        }

        /// <summary>
        /// 添加后台日志
        /// level功能位号:1-1,1-2,模块-功能序号
        /// </summary>
        /// <param name="level"></param>
        /// <param name="userId">当前用户</param>
        /// <param name="contents">内容</param>
        public static void BackInfo(string level, string userId, string contents)
        {
            dbLog.Info(new LogContent { Contents = "" + level + "=" + contents, UserId = userId });
        }
        /// <summary>
        /// 添加前台日志
        /// level功能位号:1-1,1-2,模块-功能序号
        /// </summary>
        /// <param name="level"></param>
        /// <param name="userId">当前用户</param>
        /// <param name="contents">内容</param>
        public static void FrontInfo(string level, string userId, string contents)
        {
            dbLog.Info(new LogContent { Contents = "F" + level + "=" + contents, UserId = userId });
        }

        //保存至日增长文件
        public static void Error(string message)
        {
            fileLog.Error(message);
        }
    }
}