using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Invoicing.Entities
{
    /// <summary>
    /// 数据库公共方法
    /// </summary>
    public interface IServiceDB
    {
        /// <summary>
        /// 读取模型列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IList<T> QueryModelList<T>(string sql, params object[] param);
        /// <summary>
        /// 读取分页模型列表，返回总页数
        /// usp_CommonPage @Field,@Table,@Condition,@Orderby,@PageIndex,@PageSize,@TotalPage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IList<T> QueryModelList<T>(System.Data.SqlClient.SqlParameter[] param, ref int TotalPage);
        /// <summary>
        /// 读取单个模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryOneModel<T>(string sql, params object[] param);
        /// <summary>
        /// 执行sql，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] param);
        /// <summary>
        ///  执行sql，返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        object ExecuteSqlScale(string sql, params object[] param);
    }
}
