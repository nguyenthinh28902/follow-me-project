using FollowMe.Library.Core.Repository.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Infrastructure.DbContexts;

namespace UserApp.Infrastructure.Repositories
{
    public class ReplicationUnitOfWork : UnitOfWork, IReplicationUnitOfWork
    {
        public ReplicationUnitOfWork(ReplicationDbContext context, IServiceProvider serviceProvider)
           : base(context, serviceProvider)
        {
        }

       
    }
}
