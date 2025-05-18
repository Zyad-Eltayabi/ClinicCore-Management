using System.Linq.Expressions;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Models;

namespace DomainLayer.Interfaces.ServicesInterfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAll();
        Task<Result<PatientDto>> GetById(int id);
        Task<Result<PatientDto>> Add(PatientDto patient);
        Task<Result<PatientDto>> Update(PatientDto patient);
        Task<Result<PatientDto>> Delete(int id);
        Task<Result<Patient>> Delete(Expression<Func<Patient, bool>> predicate);
    }
}
