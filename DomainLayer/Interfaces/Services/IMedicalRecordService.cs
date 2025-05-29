using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IMedicalRecordService
{
    Task<Result<IEnumerable<MedicalRecordDto>>> GetAll();
    
    Task<Result<MedicalRecordDto>> GetById(int id);
    
    Task<Result<MedicalRecordDto>> Add(MedicalRecordDto medicalRecordDto);
    
    Task<Result<bool>> Update(MedicalRecordDto medicalRecordDto);
    
    Task<Result<bool>> Delete(int id);
}