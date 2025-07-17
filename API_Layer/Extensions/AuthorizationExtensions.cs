using DomainLayer.Constants;

namespace ClinicAPI.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // 🔹 Patient-related Policies
            options.AddPolicy(AuthorizationPolicies.CanAddPatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.AddPatient));

            options.AddPolicy(AuthorizationPolicies.CanViewPatients,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewPatients));

            options.AddPolicy(AuthorizationPolicies.CanEditPatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditPatient));

            options.AddPolicy(AuthorizationPolicies.CanDeletePatient,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.DeletePatient));

            // 🔹 Doctor-related Policies
            options.AddPolicy(AuthorizationPolicies.CanViewDoctors,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewDoctors));

            options.AddPolicy(AuthorizationPolicies.CanAddDoctor,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.AddDoctor));

            options.AddPolicy(AuthorizationPolicies.CanEditDoctor,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditDoctor));

            options.AddPolicy(AuthorizationPolicies.CanDeleteDoctor,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.DeleteDoctor));
        });

        return services;
    }
}