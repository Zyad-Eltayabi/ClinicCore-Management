using BusinessLayer.Services;
using DataAccessLayer;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Use SQL Server with the connection string from the configuration file.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer((builder.Configuration.GetConnectionString("Default")));
});

// Register Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();