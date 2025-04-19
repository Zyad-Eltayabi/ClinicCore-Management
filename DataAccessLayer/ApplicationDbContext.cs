using DataAccessLayer.Configurations;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TablesConfigurations.ConfigurePatients(modelBuilder.Entity<Patient>());
        }

        public DbSet<Patient> Patients { get; set; }
    }

}
