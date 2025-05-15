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
        ServiceResult<Patient> Add(Patient patient);
        ServiceResult<Patient> Update(Patient patient);
        ServiceResult<Patient> Delete(Patient patient);
        public ServiceResult<Patient> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
