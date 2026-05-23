using System;
using System.Collections.Generic;
using System.Text;

namespace CachePulse.WorkerService.Models.Settings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public List<string> Topics { get; set; } = new();
    }
}
