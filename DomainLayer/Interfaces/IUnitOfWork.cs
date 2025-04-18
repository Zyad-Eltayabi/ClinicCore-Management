using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
         IGenericRepository<Patient> Patients { get; }

        Result<int> SaveChanges();
    }
}
