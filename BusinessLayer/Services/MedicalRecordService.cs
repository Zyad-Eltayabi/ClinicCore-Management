using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;

namespace BusinessLayer.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<Result<IEnumerable<MedicalRecordDto>>> GetAll()
    {
        var medicalRecords = await _unitOfWork.MedicalRecords.GetAll();
        if (medicalRecords == null)
            return Result<IEnumerable<MedicalRecordDto>>.Failure("No medical records found", ServiceErrorType.NotFound);

        var medicalRecordDtos = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
        return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
    }

    public async Task<Result<MedicalRecordDto>> GetById(int id)
    {
        var medicalRecord = await _unitOfWork.MedicalRecords.GetById(id);
    
        if (medicalRecord == null)
            return Result<MedicalRecordDto>.Failure("Medical record not found", ServiceErrorType.NotFound);
    
        var medicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
        return Result<MedicalRecordDto>.Success(medicalRecordDto);
    }

    public Task<Result<MedicalRecordDto>> Add(MedicalRecordDto medicalRecordDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> Update(MedicalRecordDto medicalRecordDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}