using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.DTOs;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDTO>> GetAll();
        Task<Patient> GetById(int id);
        Task<ServiceResult<Patient>> Add(Patient patient);
        Task<ServiceResult<Patient>> Update(Patient patient);
        Task<ServiceResult<Patient>> Delete(Patient patient);
        Task<ServiceResult<Patient>> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
