using BusinessLayer.Services;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Interfaces.ServicesInterfaces;

namespace ClinicAPI.Extensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleClaimService, RoleClaimService>();
        services.AddScoped<IUserRoleService, UserRoleService>();

        return services;
    }
}