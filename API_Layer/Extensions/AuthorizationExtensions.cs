using DomainLayer.Constants;

namespace ClinicAPI.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.CanAddPatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.AddPatient));

            options.AddPolicy(AuthorizationPolicies.CanViewPatients,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewPatients));

            options.AddPolicy(AuthorizationPolicies.CanEditPatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditPatient));

            options.AddPolicy(AuthorizationPolicies.CanDeletePatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.DeletePatient));
        });

        return services;
    }
}