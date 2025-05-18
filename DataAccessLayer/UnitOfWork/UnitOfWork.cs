using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Persistence;
using DataAccessLayer.Repositories;
using DomainLayer.BaseClasses;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.UnitOfWork
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
        public async Task<bool> SaveChanges()
        {
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
