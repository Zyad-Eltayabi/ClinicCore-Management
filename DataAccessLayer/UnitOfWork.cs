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
        public Result<int> SaveChanges()
        {
            try
            {
                int affectedRows = _context.SaveChanges();
                return affectedRows > 0
                    ? Result<int>.Success(affectedRows, "Changes saved successfully.")
                    : Result<int>.Failure("No changes were saved to the database.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("An unexpected error occurred: " + ex.Message);
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
