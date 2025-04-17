using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DomainLayer.Interfaces;
using DomainLayer.Models;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Patient> Patients { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patients = new GenericRepository<Patient>(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
