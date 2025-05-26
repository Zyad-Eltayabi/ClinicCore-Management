using DataAccessLayer.Persistence;
using DataAccessLayer.Repositories;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        public IGenericRepository<Patient> Patients { get; }
        public IGenericRepository<Doctor> Doctors { get; }
        public IGenericRepository<Prescription> Prescriptions { get; }
        public IGenericRepository<Payment> Payments { get; }

        public IGenericRepository<Appointment> Appointments { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patients = new GenericRepository<Patient>(_context);
            Doctors = new GenericRepository<Doctor>(_context);
            Appointments = new GenericRepository<Appointment>(_context);
            Prescriptions = new GenericRepository<Prescription>(_context);
            Payments = new GenericRepository<Payment>(_context);
        }
        public async Task<bool> SaveChanges()
        {
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task CreateTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
           await _transaction.CommitAsync();
        }

        public async Task Rollback()
        {
           await _transaction.RollbackAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
