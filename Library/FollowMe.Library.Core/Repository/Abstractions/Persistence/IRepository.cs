using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FollowMe.Library.Core.Repository.Abstractions.Persistence
{
    public interface IRepository<T> where T : class
    {

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task<T?> FindAsync(object Id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ToListAsync();
        Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> predicate);
        public IQueryable<T> Includes(params Expression<Func<T, object>>[] includes);
        public IQueryable<T> GetAll(
           Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);
        public void Detached(T entity);
    }

}
