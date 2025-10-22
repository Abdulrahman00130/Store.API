using Store.API.Domain.Entities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TKey,TEntity> GetRepository<TKey,TEntity>() where TEntity : BaseEntity<TKey>;
        Task<int> SaveChangesAsync();
    }
}
