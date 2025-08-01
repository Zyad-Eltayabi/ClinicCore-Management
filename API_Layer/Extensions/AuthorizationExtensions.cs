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

            // 🔹 Appointment-related Policies
            options.AddPolicy(AuthorizationPolicies.CanViewAppointments,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewAppointments));

            options.AddPolicy(AuthorizationPolicies.CanCreateAppointment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.CreateAppointment));

            options.AddPolicy(AuthorizationPolicies.CanEditAppointment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditAppointment));

            options.AddPolicy(AuthorizationPolicies.CanCancelAppointment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.CancelAppointment));

            options.AddPolicy(AuthorizationPolicies.CanRescheduleAppointment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.RescheduleAppointment));

            options.AddPolicy(AuthorizationPolicies.CanCompleteAppointment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.CompleteAppointment));

            // 🔹 MedicalRecord-related Policies
            options.AddPolicy(AuthorizationPolicies.CanViewMedicalRecords,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewMedicalRecords));

            options.AddPolicy(AuthorizationPolicies.CanCreateMedicalRecord,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.CreateMedicalRecord));

            options.AddPolicy(AuthorizationPolicies.CanEditMedicalRecord,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditMedicalRecord));

            options.AddPolicy(AuthorizationPolicies.CanDeleteMedicalRecord,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.DeleteMedicalRecord));

            // 🔹 Prescription-related Policies
            options.AddPolicy(AuthorizationPolicies.CanViewPrescriptions,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewPrescriptions));

            options.AddPolicy(AuthorizationPolicies.CanCreatePrescription,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.CreatePrescription));

            options.AddPolicy(AuthorizationPolicies.CanEditPrescription,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.EditPrescription));

            options.AddPolicy(AuthorizationPolicies.CanDeletePrescription,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.DeletePrescription));

            // 🔹 payment-related Policies
            options.AddPolicy(AuthorizationPolicies.CanViewPayments,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ViewPayments));

            options.AddPolicy(AuthorizationPolicies.CanProcessPayment,
                policy => policy.RequireClaim(ClaimConstants.Permission, ClaimConstants.ProcessPayment));
        });

        return services;
    }
}