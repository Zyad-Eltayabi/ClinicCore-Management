using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        IGenericRepository<Appointment> Appointments { get; }

        Task<bool> SaveChanges();
        
        //Start the database Transaction
        Task CreateTransaction();
        //Commit the database Transaction
        Task Commit();
        //Rollback the database Transaction
        Task Rollback();
    }
}
