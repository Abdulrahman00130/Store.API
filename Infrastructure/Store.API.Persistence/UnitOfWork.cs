using Microsoft.IdentityModel.Tokens;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities;
using Store.API.Persistence.Data.Contexts;
using Store.API.Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Persistence
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        #region GetRepository using Dictionary
        //private Dictionary<string, object> _repositories = new Dictionary<string, object>();
        //public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if(!_repositories.ContainsKey(type))
        //    {
        //        _repositories.Add(type, new GenericRepository<TKey, TEntity>(_context));
        //    }
        //    return _repositories[type] as IGenericRepository<TKey,TEntity>;
        //} 
        #endregion

        #region GetRepository using ConcurrentDictionary
        private ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();
        public IGenericRepository<TKey,TEntity> GetRepository<TKey,TEntity>() where TEntity : BaseEntity<TKey>
        {
            return _repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TKey, TEntity>(_context)) as IGenericRepository<TKey,TEntity>;
        }
        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
