using BusinessLayer.Mapping;
using BusinessLayer.Services;
using ClinicAPI.Middlewares;
using DataAccessLayer.Persistence;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Interfaces.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using FluentValidation;


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

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddExceptionHandler<GlobalErrorHandling>();
builder.Services.AddProblemDetails();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();