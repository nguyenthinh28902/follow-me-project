using CachePulse.WorkerService.Interfaces;
using CachePulse.WorkerService.Models.Enums;
using CachePulse.WorkerService.Models.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CachePulse.WorkerService.WorkerSevices
{
    public class GlobalCacheRefresherWorker : BackgroundService
    {
        private readonly ILogger<GlobalCacheRefresherWorker> _logger;
        private readonly IBloomFilterService _bloomFilter;
        private readonly KafkaSettings _kafkaSettings;
        private readonly IConsumer<Ignore, string> _kafkaConsumer;
        public GlobalCacheRefresherWorker(ILogger<GlobalCacheRefresherWorker> logger,
            IBloomFilterService bloomFilter,
            IOptions<KafkaSettings> kafkaSettings)
        {
            _bloomFilter = bloomFilter;
            _kafkaSettings = kafkaSettings.Value;

            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _kafkaConsumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _kafkaConsumer.Subscribe(_kafkaSettings.Topics);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _kafkaConsumer.Consume(stoppingToken);
                    if (consumeResult == null) continue;

                    var rawJson = consumeResult.Message.Value;

                    using var doc = JsonDocument.Parse(rawJson);
                    var payload = doc.RootElement.GetProperty("payload");

                    if (payload.TryGetProperty("after", out var afterElement) && afterElement.ValueKind != JsonValueKind.Null)
                    {
                        string email = afterElement.GetProperty("Email").GetString() ?? string.Empty;
                        string username = afterElement.GetProperty("Username").GetString() ?? string.Empty;

                        if (!string.IsNullOrEmpty(email))
                            await _bloomFilter.AddAsync(FilterRedis.FilterEmail, email);
                        
                        if (!string.IsNullOrEmpty(username))
                            await _bloomFilter.AddAsync(FilterRedis.FilterUsername, username);
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Kafka-Worker-Error]: {ex.Message}");
                }
            }

        }
        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
            base.Dispose();
        }
    }
}
