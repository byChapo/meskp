using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        int Create(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        List<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetWithInclude(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
    }

    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(TKey id);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(TKey id);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetWithInclude(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
    }
} 
