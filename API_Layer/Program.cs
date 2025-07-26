using BusinessLayer.Mapping;
using ClinicAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.UseSerilogRequestLogging();
builder.Services
    .AddApiServices()
    .AddDbServices(builder.Configuration)
    .AddIdentityServices(builder.Configuration)
    .AddAuthorizationPolicies()
    .AddAppServices()
    .AddAutoMapper(typeof(MappingProfile))
    .AddLoggingService();

var app = await builder.Build().UseApiConfiguration();


await app.RunAsync();