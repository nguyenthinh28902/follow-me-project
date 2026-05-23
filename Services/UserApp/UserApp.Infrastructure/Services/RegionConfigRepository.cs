using FollowMe.Library.Core.Repository.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Application.Interfaces;
using UserApp.Core.Entities;

namespace UserApp.Infrastructure.Services
{
    public class RegionConfigRepository : IRegionConfigRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RegionConfig> _regionConfigRepository;
        public RegionConfigRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _regionConfigRepository = _unitOfWork.Repository<RegionConfig>();
        }
        public async Task<RegionConfig?> GetRegionConfigAsync(string region)
        =>  await _regionConfigRepository.FirstOrDefaultAsNoTrackingAsync(rc => rc.RegionCode == region);
        
    }
}
