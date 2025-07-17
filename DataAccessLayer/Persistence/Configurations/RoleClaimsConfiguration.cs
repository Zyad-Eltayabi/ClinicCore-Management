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
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.DeletePatient,
            ClaimConstants.ViewPatients
        });

        Roleclaims.Add(Roles.ClinicManager, new List<string>
        {
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.DeletePatient,
            ClaimConstants.ViewPatients
        });

        Roleclaims.Add(Roles.Receptionist, new List<string>
        {
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.ViewPatients
        });

        Roleclaims.Add(Roles.MedicalAdmin, new List<string>
        {
            ClaimConstants.AddPatient,
            ClaimConstants.EditPatient,
            ClaimConstants.ViewPatients
        });
    }
}