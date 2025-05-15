using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DomainLayer.BaseClasses;
using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ServiceResult<int>> SaveChanges()
        {
            try
            {
                int affectedRows =await _context.SaveChangesAsync();
                return affectedRows > 0
                    ? ServiceResult<int>.Success(affectedRows, "Changes saved successfully.")
                    : ServiceResult<int>.Failure("No changes were saved to the database.");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure("An unexpected error occurred: " + ex.Message);
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
