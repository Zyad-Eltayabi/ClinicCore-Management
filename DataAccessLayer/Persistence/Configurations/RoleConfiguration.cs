using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new List<IdentityRole>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "ClinicManager",
                    NormalizedName = "ClinicManager".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "MedicalAdmin",
                    NormalizedName = "MedicalAdmin".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Receptionist ",
                    NormalizedName = "Receptionist ".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            });
    }
}