
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TDS.Core.Entities;

namespace Clarit.Core.Repository
{
    public interface IBaseRepository<TEntity, TKey> : IDisposable where TEntity : BaseEntity<TKey>
    {

        TEntity Get(TKey id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(TKey id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        bool Any(Expression<Func<TEntity, bool>> predicate);

        //Async  operations
        Task<TEntity> GetAsync(TKey id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
       
    }
}
