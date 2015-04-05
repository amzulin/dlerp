using Enterprise.Invoicing.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public abstract class BasicRepository<T> : IBasicRepository<T> where T:class
    {
        public InvoicingContext context { get; private set; }

        public DbSet<T> dbSet { get; private set; }

        public BasicRepository(InvoicingContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        #region 公共方法
        public string GetRequireNo()
        {
            var last = (from r in context.PurchaseRequires orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.requireNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "NE" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }

        public string GetDelegateNo()
        {
            var last = (from r in context.DelegateOrders orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.delegateNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "WX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        #endregion
    }
}
