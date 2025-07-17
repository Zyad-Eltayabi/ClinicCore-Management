using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Extensions;

public static class DbServicesExtensions
{
    public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        return services;
    }
}