using System.Security.Claims;
using DataAccessLayer.Persistence;
using DomainLayer.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Seeding;

public class PermissionSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;

    public PermissionSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        // check if any claims exist
        var hasClaims = await _dbContext.Set<IdentityRoleClaim<string>>().AnyAsync();
        if (hasClaims)
            return;

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

        // seed data into database
        foreach (var roleName in Roleclaims.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is not null)
                foreach (var claim in Roleclaims[roleName])
                    await _roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, claim));
        }
    }
}