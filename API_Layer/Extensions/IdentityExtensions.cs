using DataAccessLayer.Persistence;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddJwtAuthentication(configuration);

        return services;
    }
}