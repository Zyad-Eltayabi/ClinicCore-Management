using DomainLayer.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Persistence.Configurations;

public class RoleClaimsConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var Roleclaims = new Dictionary<string, List<string>>();

        Roleclaims.Add(Roles.SuperAdmin, new List<string>
        {
            // 🔹 Patient-related Claims
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.DeletePatient,
            ClaimConstants.ViewPatients,

            // 🔹 Doctor-related Claims
            ClaimConstants.ViewDoctors,
            ClaimConstants.AddDoctor,
            ClaimConstants.EditDoctor,
            ClaimConstants.DeleteDoctor,

            // 🔹 Appointment-related Claims
            ClaimConstants.ViewAppointments,
            ClaimConstants.CreateAppointment,
            ClaimConstants.EditAppointment,
            ClaimConstants.CancelAppointment,
            ClaimConstants.CompleteAppointment,

            // 🔹 medical-record related claims
            ClaimConstants.ViewMedicalRecords,
            ClaimConstants.CreateMedicalRecord,
            ClaimConstants.EditMedicalRecord,
            ClaimConstants.DeleteMedicalRecord,

            // 🔹 Prescription-related claims
            ClaimConstants.ViewPrescriptions,
            ClaimConstants.CreatePrescription,
            ClaimConstants.EditPrescription,
            ClaimConstants.DeletePrescription,

            // 🔹 Payment-related claims
            ClaimConstants.ViewPayments,
            ClaimConstants.ProcessPayment
        });

        Roleclaims.Add(Roles.ClinicManager, new List<string>
        {
            // 🔹 Patient-related Claims
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.DeletePatient,
            ClaimConstants.ViewPatients,

            // 🔹 Doctor-related Claims
            ClaimConstants.ViewDoctors,
            ClaimConstants.AddDoctor,
            ClaimConstants.EditDoctor,
            ClaimConstants.DeleteDoctor,

            // 🔹 Appointment-related Claims
            ClaimConstants.ViewAppointments,
            ClaimConstants.CreateAppointment,
            ClaimConstants.EditAppointment,
            ClaimConstants.CancelAppointment,
            ClaimConstants.CompleteAppointment,

            // 🔹 medical-record related claims
            ClaimConstants.ViewMedicalRecords,
            ClaimConstants.CreateMedicalRecord,
            ClaimConstants.EditMedicalRecord,
            ClaimConstants.DeleteMedicalRecord,

            // 🔹 Prescription-related claims
            ClaimConstants.ViewPrescriptions,
            ClaimConstants.CreatePrescription,
            ClaimConstants.EditPrescription,
            ClaimConstants.DeletePrescription,

            // 🔹 Payment-related claims
            ClaimConstants.ViewPayments,
            ClaimConstants.ProcessPayment
        });

        Roleclaims.Add(Roles.Receptionist, new List<string>
        {
            // 🔹 Patient-related Claims
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.ViewPatients,

            // 🔹 Doctor-related Claims
            ClaimConstants.ViewDoctors,

            // 🔹 Appointment-related Claims
            ClaimConstants.ViewAppointments,
            ClaimConstants.CreateAppointment,
            ClaimConstants.EditAppointment,
            ClaimConstants.CancelAppointment,
            ClaimConstants.CompleteAppointment,

            // 🔹 medical-record related claims
            ClaimConstants.ViewMedicalRecords,

            // 🔹 Prescription-related claims
            ClaimConstants.ViewPrescriptions,

            // 🔹 Payment-related claims
            ClaimConstants.ViewPayments,
            ClaimConstants.ProcessPayment
        });

        Roleclaims.Add(Roles.MedicalAdmin, new List<string>
        {
            // 🔹 Patient-related Claims
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.ViewPatients,

            // 🔹 Doctor-related Claims
            ClaimConstants.ViewDoctors,

            // 🔹 Appointment-related Claims
            ClaimConstants.ViewAppointments,

            // 🔹 medical-record related claims
            ClaimConstants.ViewMedicalRecords,
            ClaimConstants.CreateMedicalRecord,
            ClaimConstants.EditMedicalRecord,

            // 🔹 Prescription-related claims
            ClaimConstants.ViewPrescriptions,
            ClaimConstants.CreatePrescription,
            ClaimConstants.EditPrescription
        });
    }
}