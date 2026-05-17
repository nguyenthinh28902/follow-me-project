using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeProject.Library.Core.Abstractions.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync(); // Trả về số dòng bị ảnh hưởng
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
    
}
