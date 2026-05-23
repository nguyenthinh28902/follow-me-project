using CachePulse.WorkerService.Interfaces;
using StackExchange.Redis;

namespace CachePulse.WorkerService.Services
{
    public class BloomFilterService : IBloomFilterService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public BloomFilterService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        // Khởi tạo cấu hình Bloom Filter (tỷ lệ sai số và sức chứa tối đa)
        public async Task InitFilterAsync(string filterName, long capacity, double errorRate = 0.01)
        {
            // Kiểm tra xem filter đã tồn tại chưa bằng cách gọi thử, nếu chưa thì tạo mới
            // Lệnh Redis: BF.RESERVE {filterName} {errorRate_thập_phân} {capacity}
            // Ví dụ sai số 1% tương đương 0.01
            try
            {
                await _db.ExecuteAsync("BF.RESERVE", filterName, errorRate, capacity);
            }
            catch (RedisServerException ex) when (ex.Message.Contains("item already exists"))
            {
                // Nếu filter đã tồn tại ngầm trong Redis rồi thì bỏ qua lỗi này
            }
        }

        // Thêm giá trị vào Bloom Filter
        public async Task AddAsync(string filterName, string value)
        {
            // Lệnh Redis: BF.ADD {filterName} {value}
            await _db.ExecuteAsync("BF.ADD", filterName, value.ToLower().Trim());
        }

        // Kiểm tra xem giá trị có tồn tại trong hệ thống không
        public async Task<bool> ExistsAsync(string filterName, string value)
        {
            // Lệnh Redis: BF.EXISTS {filterName} {value}
            var result = await _db.ExecuteAsync("BF.EXISTS", filterName, value.ToLower().Trim());
            return (int)result == 1;
        }
    }
}
