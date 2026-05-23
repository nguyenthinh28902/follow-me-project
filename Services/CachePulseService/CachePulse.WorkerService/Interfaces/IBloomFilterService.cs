using System;
using System.Collections.Generic;
using System.Text;

namespace CachePulse.WorkerService.Interfaces
{
    public interface IBloomFilterService
    {
        Task InitFilterAsync(string filterName, long capacity, double errorRate);
        Task AddAsync(string filterName, string value);
        Task<bool> ExistsAsync(string filterName, string value);
    }
}
