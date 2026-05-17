using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FollowMeProject.Library.Core.Abstractions.Persistence
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        IQueryable<T> EntitiesNoTracking { get; }
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        public void Detached(T entity);
    }

}
