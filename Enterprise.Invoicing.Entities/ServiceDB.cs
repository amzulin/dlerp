
using Enterprise.Invoicing.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Invoicing.Entities
{
    public class ServiceDB:IServiceDB
    {
        
        private static ServiceDB instance = new ServiceDB();
        private InvoicingContext context = new InvoicingContext();
        public static ServiceDB Instance
        {
            get { return instance; }
        }

        private ServiceDB()
        {
 
        }

        public IList<T> QueryModelList<T>(string sql, params object[] param)
        {
            return context.Database.SqlQuery<T>(sql, param).ToList();
        }

        public T QueryOneModel<T>(string sql, params object[] param)
        {
            var query = context.Database.SqlQuery<T>(sql, param).ToList();
            return query.FirstOrDefault();
        }

        public int ExecuteSqlCommand(string sql, params object[] param)
        {
            return context.Database.ExecuteSqlCommand(sql, param);
        }

        public object ExecuteSqlScale(string sql, params object[] param)
        {
            // return context.Database.SqlQuery(typeof(object), sql, param).ToString();
            var list = context.Database.SqlQuery<string>(sql, param).ToList();
            if (list != null && list.Count > 0)
            {
                return (object)list[0];
            }
            return null;
        }


        public IList<T> QueryModelList<T>(System.Data.SqlClient.SqlParameter[] param, ref int TotalPage)
        {
            TotalPage = 0;
            var list = QueryModelList<T>("exec usp_CommonPage @Field,@Table,@Condition,@Orderby,@PageIndex,@PageSize,@TotalPage output ", param);
            TotalPage = Convert.ToInt32(param[6].Value);
            return list;
        }
    }
}
