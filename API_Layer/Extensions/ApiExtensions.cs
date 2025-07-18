using System.Diagnostics;
using ClinicAPI.Middlewares;
using DataAccessLayer.Persistence;
using DataAccessLayer.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.FluentValidation.AspNetCore;

namespace ClinicAPI.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFluentValidationRulesToSwagger();
        services.AddExceptionHandler<GlobalErrorHandling>();
        services.AddProblemDetails();
        return services;
    }

    public static async Task<WebApplication> UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Seeding default role claims data to database
        try
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await dbContext.Database.MigrateAsync();

                var seeder = new PermissionSeeder(dbContext, roleManager);
                await seeder.SeedAsync();
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }


        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}