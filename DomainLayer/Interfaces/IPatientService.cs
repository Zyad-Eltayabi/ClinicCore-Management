using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient> GetById(int id);
        Result<Patient> Add(Patient patient);
        Result<Patient> Update(Patient patient);
        Result<Patient> Delete(Patient patient);
        public Result<Patient> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
