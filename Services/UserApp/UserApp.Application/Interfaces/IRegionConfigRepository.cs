using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Entities;

namespace UserApp.Application.Interfaces
{
    public interface IRegionConfigRepository
    {
        Task<RegionConfig?> GetRegionConfigAsync(string region);
    }
}
