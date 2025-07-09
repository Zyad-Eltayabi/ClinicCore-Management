using DomainLayer.Interfaces.Repositories;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Patient> Patients { get; }
        IGenericRepository<Doctor> Doctors { get; }
        IGenericRepository<Prescription> Prescriptions { get; }
        IGenericRepository<Payment> Payments { get; }

        IGenericRepository<MedicalRecord> MedicalRecords { get; }

        IGenericRepository<Appointment> Appointments { get; }

        IGenericRepository<RefreshToken> RefreshTokens { get; }

        Task<bool> SaveChanges();

        //Start the database Transaction
        Task CreateTransaction();

        //Commit the database Transaction
        Task Commit();

        //Rollback the database Transaction
        Task Rollback();
    }
}