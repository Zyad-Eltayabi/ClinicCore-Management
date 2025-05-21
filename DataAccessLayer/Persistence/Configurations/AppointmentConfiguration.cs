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
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.AppointmentID);

            // configure relation between appointment and patient
            builder.HasOne<Patient>(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID);

            // configure relation between appointment and doctor
            builder.HasOne<Doctor>(a => a.Doctor)
              .WithMany(d => d.Appointments)
              .HasForeignKey(a => a.DoctorID);

        }
    }
}
