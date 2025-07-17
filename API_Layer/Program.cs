using BusinessLayer.Mapping;
using ClinicAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddDbServices(builder.Configuration)
    .AddIdentityServices(builder.Configuration)
    .AddAppServices()
    .AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseApiConfiguration();

app.Run();