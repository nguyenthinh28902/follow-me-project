using UserApp.Application.DependencyInjection;
using UserApp.Infrastructure.DependencyInjection;
using UserApp.Infrastructure.Data;
using UserApp.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddMappingAplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddAplication();
builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<IdentityGrpcServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
await app.Services.SeedSystemRegions();
app.Run();