using System;
using System.Data;
using System.Configuration;


using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Enterprise.Invoicing.Service
{
    /// <summary>
    /// SqlHelper 的摘要说明
    /// </summary>
    public class SqlHelper
    {
        private static readonly string strCon = ConfigurationManager.AppSettings["sql_2012"];
        static SqlConnection conn;
        static SqlCommand cmd;
        public SqlHelper()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

                /// <summary>
    /// Open the sql Connection
    /// </summary>
       static void GetSqlCon()
        {
            if (conn == null)
            {
                conn = new SqlConnection(strCon);
            }
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public static object ExecuteScale(string sql)
    {
        return ExecuteScale(sql, null, CommandType.Text);
    }
        public static object ExecuteScale(string sql, SqlParameter[] paras)
    {
        return ExecuteScale(sql, paras, CommandType.Text);
    }
    /// <summary>
    /// 通过sql查询，返回受影响行数
    /// </summary>
    /// <param Name="sql">sql语句</param>
    /// <param Name="paras">参数</param>
    /// <param Name="cmdType">命令类型</param>
    /// <returns>返回受影响行数</returns>
     public static object ExecuteScale(string sql,SqlParameter[] paras ,CommandType cmdType)
    {
        GetSqlCon();
        cmd = new SqlCommand(sql, conn);
        if (paras != null)
        {
            cmd.Parameters.AddRange(paras);
        }
        cmd.CommandType = cmdType;

        object o = cmd.ExecuteScalar();
        conn.Close();
        return o;
    }

        public static int ExecureQuery(string sql, SqlParameter[] paras)
        {
            return ExecureQuery(sql, paras, CommandType.Text);
         }
        /// <summary>
    /// 执行 SQL 语句  
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paras"></param>
    /// <param name="cmdType"></param>
    /// <returns></returns>
     public static int ExecureQuery(string sql, SqlParameter[] paras, CommandType cmdType)
    {
        GetSqlCon();
        SqlCommand cmd = new SqlCommand(sql, conn);
        if (paras != null)
        {
            cmd.Parameters.AddRange(paras);
        }
        cmd.CommandType = cmdType;
        int i = cmd.ExecuteNonQuery();
        conn.Close();
        return i;
    }

    /// 执行查询语句，返回DataSet
    public static DataTable Query(string SQLString, params SqlParameter[] cmdParms)
    {
        using (SqlConnection connection = new SqlConnection(strCon))
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null,CommandType.Text, SQLString, cmdParms);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable ds = new DataTable();
                try
                {
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return ds;
            }
        }
    }

    public static DataTable QueryStored(string SQLString, params SqlParameter[] cmdParms)
    {
        using (SqlConnection connection = new SqlConnection(strCon))
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null,CommandType.StoredProcedure, SQLString, cmdParms);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable ds = new DataTable();
                try
                {
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return ds;
            }
        }
    }

    public static IList<T> Query<T>(string SQLString, CommandType cmdType, params SqlParameter[] cmdParms)
    {
        using (SqlConnection connection = new SqlConnection(strCon))
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, SQLString, cmdParms);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable ds = new DataTable();
                try
                {
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                IList<T> list = CreateObject<T>(ds);
                return list;
            }
        }
    }

    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans,CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
            cmd.Transaction = trans;
        cmd.CommandType = cmdType;// CommandType.Text;//cmdType;
        if (cmdParms != null)
        {
            foreach (SqlParameter parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                cmd.Parameters.Add(parameter);
            }
        }
    }


    /// <summary>
    /// 执行多条SQL语句，实现数据库事务。
    /// </summary>
    /// <param name="SQLStringList">多条SQL语句</param>       
    public static bool ExecuteSqlTran(ArrayList SQLStringList)
    {
        bool flag = true;
        //using (conn = new SqlConnection(strCon))
        //{
        //    conn.Open();
            GetSqlCon();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction(); //事务
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (SqlException E)
            {
                tx.Rollback();
                flag = false;
               // throw new Exception(E.Message);
              
            }
       // }
        return flag;
    }


    private static IList<T> CreateObject<T>(DataTable table)
    {
        IList<T> list = new List<T>();
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        string name = string.Empty;
        var count = table.Columns.Count;
        foreach (DataRow row in table.Rows)
        {
            T local = Activator.CreateInstance<T>();

            for (int i = 0; i < count; i++)
            {
                name = table.Columns[i].ColumnName;
                foreach (PropertyInfo info in properties)
                {
                    if (name.Equals(info.Name)) { info.SetValue(local, Convert.ChangeType(row[info.Name], info.PropertyType), null); break; }
                }
            }
            list.Add(local);
        }
        return list;
    }
}

}