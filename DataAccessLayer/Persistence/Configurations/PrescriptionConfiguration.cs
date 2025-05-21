using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Persistence.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.PrescriptionID);

            builder.Property(p => p.MedicationName)
                .HasMaxLength(100);

            builder.Property(p => p.Dosage)
              .HasMaxLength(50);

            builder.Property(p => p.SpecialInstructions)
              .HasMaxLength(200);
        }
    }
}
