using System.Linq.Expressions;
using DomainLayer.BaseClasses;
using DomainLayer.DTOs;
using DomainLayer.Models;

namespace DomainLayer.Interfaces.ServicesInterfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAll();
        Task<Patient> GetById(int id);
        Task<ServiceResult<PatientDto>> Add(PatientDto patient);
        Task<ServiceResult<Patient>> Update(Patient patient);
        Task<ServiceResult<Patient>> Delete(Patient patient);
        Task<ServiceResult<Patient>> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
