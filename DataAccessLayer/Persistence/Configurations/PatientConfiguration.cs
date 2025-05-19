using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> modelBuilder)
        {
            modelBuilder.Property(p => p.FullName)
                .HasMaxLength(150);

            modelBuilder.Property(p => p.PhoneNumber)
                .HasMaxLength(50);

            modelBuilder.Property(p => p.Email)
                .HasMaxLength(50);

            modelBuilder.Property(p => p.Address)
                .HasMaxLength(250);

            modelBuilder.Property(p => p.DateOfRegistration)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
