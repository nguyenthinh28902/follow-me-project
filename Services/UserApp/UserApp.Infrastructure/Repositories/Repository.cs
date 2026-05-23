using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FollowMe.Library.Core.Repository.Abstractions.Persistence;
using UserApp.Infrastructure.DbContexts;

namespace UserApp.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FollowMeIdentityDbContext _context;

        public Repository(FollowMeIdentityDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity) 
            => await _context.Set<T>().AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) 
            => await _context.Set<T>().AddRangeAsync(entities);

        public void Remove(T entity) 
            => _context.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) 
            => _context.Set<T>().RemoveRange(entities);

        public void Update(T entity) 
            => _context.Update(entity);

        public void UpdateRange(IEnumerable<T> entities) 
            => _context.UpdateRange(entities);

        public async Task<T?> FindAsync(object Id) 
            => await _context.Set<T>().FindAsync(Id);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) 
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> predicate)
        => await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> ToListAsync() 
            => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> predicate) 
            => await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) 
            => predicate != null 
                ? await _context.Set<T>().AsNoTracking().CountAsync(predicate)
                : await _context.Set<T>().AsNoTracking().CountAsync();

        /// <summary>
        /// Hàm tổng quát để build query động với đầy đủ các tùy chọn Filter, Include, OrderBy và Tracking.
        /// </summary>
        public IQueryable<T> GetAll(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include, 
            bool disableTracking = true, 
            bool ignoreQueryFilters = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public void Detached(T entity) 
            => _context.Entry(entity).State = EntityState.Detached;

        /// <summary>
        /// Nạp chồng các điều kiện Include động bằng Lambda Expression.
        /// </summary>
        public IQueryable<T> Includes(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            
            return query;
        }
    }
}