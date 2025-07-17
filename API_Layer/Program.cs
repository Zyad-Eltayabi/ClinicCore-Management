using BusinessLayer.Mapping;
using BusinessLayer.Services;
using ClinicAPI.Extensions;
using ClinicAPI.Middlewares;
using DataAccessLayer.Persistence;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Interfaces.ServicesInterfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleClaimService, RoleClaimService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddExceptionHandler<GlobalErrorHandling>();
builder.Services.AddProblemDetails();

builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();