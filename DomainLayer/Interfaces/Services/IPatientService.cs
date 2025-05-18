using System.Linq.Expressions;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Models;

namespace DomainLayer.Interfaces.ServicesInterfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAll();
        Task<ServiceResult<PatientDto>> GetById(int id);
        Task<ServiceResult<PatientDto>> Add(PatientDto patient);
        Task<ServiceResult<Patient>> Update(Patient patient);
        Task<ServiceResult<Patient>> Delete(int id);
        Task<ServiceResult<Patient>> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
