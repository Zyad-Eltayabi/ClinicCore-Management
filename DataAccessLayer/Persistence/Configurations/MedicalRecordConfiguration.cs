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
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(m => m.MedicalRecordID);

            builder.Property(m => m.VisitDescription)
                .HasMaxLength(200);

            builder.Property(m => m.Diagnosis)
                .HasMaxLength(200);

            builder.Property(m => m.AdditionalNotes)
                .HasMaxLength(200);
        }
    }
}
