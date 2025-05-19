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


        Task<bool> SaveChanges();
    }
}
