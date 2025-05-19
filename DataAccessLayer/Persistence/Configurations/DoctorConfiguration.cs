using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        
        builder.Property(d => d.FullName)
            .HasMaxLength(100);
        
        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(20);
        
        builder.Property(d => d.Email)
            .HasMaxLength(100);

        builder.HasIndex(d => d.Email)
            .IsUnique();
        
        builder.Property(d => d.Address)
            .HasMaxLength(200);
        
        builder.Property(d => d.Specialization)
            .HasMaxLength(100);
        
        builder.Property(d => d.DateOfRegistration)
            .HasDefaultValueSql("GETDATE()");
    }
}