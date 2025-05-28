using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IPrescriptionService
{
    Task<Result<IEnumerable<CreateOrUpdatePrescriptionDto>>> GetAll();
    
    Task<Result<CreateOrUpdatePrescriptionDto>> GetById(int id);
    
    Task<Result<CreateOrUpdatePrescriptionDto>> Add(CreateOrUpdatePrescriptionDto prescriptionDto);
    
    Task<Result<CreateOrUpdatePrescriptionDto>> Update(CreateOrUpdatePrescriptionDto prescriptionDto);
    
    Task<Result<bool>> Delete(int id);
}