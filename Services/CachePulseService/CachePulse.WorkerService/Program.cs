using CachePulse.WorkerService;
using CachePulse.WorkerService.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSettingOption(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddApplicationServices(); 

var host = builder.Build();
host.Run();
