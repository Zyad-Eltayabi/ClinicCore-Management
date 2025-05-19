using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAll();
    Task<Result<DoctorDto>> GetById(int id);
    Task<Result<DoctorDto>> Add(DoctorDto doctorDto);
    Task<Result<DoctorDto>> Update(DoctorDto patient);
    Task<Result<DoctorDto>> Delete(int id);
}