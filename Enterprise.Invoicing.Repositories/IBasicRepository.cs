using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IBasicRepository<TEntity> : IDisposable where TEntity : class
    {
    }
}
