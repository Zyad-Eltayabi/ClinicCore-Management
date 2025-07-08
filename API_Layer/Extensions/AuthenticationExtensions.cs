using ClinicAPI.Helpers;

namespace ClinicAPI.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("Jwt");
            services.Configure<JwtOptions>(JwtOptions);

        }

    }
}
