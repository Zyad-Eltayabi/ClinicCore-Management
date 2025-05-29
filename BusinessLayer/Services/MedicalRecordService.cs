using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;

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
    
    private async Task<ValidationsResult> ValidateMedicalRecordDto(MedicalRecordDto medicalRecordDto)
    {
        var medicalValidator = new MedicalRecordValidator();
        var validations = await medicalValidator.ValidateAsync(medicalRecordDto);
        if (!validations.IsValid)
        {
            string message = string.Join("; ", validations.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true);
    }
    
    public async Task<Result<MedicalRecordDto>> Add(MedicalRecordDto medicalRecordDto)
    {
        // Validate the medical record DTO
        var validations = await ValidateMedicalRecordDto(medicalRecordDto);
        if (!validations.IsValid)
            return Result<MedicalRecordDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // Map the DTO to a domain model
        var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordDto);
    
        // Add the medical record to the repository
        await _unitOfWork.MedicalRecords.Add(medicalRecord);
    
        // Save changes to the database
        bool saveResult = await _unitOfWork.SaveChanges();
    
        if (!saveResult)
            return Result<MedicalRecordDto>.Failure("Failed to add medical record to database", ServiceErrorType.DatabaseError);

        // Map the created entity back to DTO and return success
        var createdMedicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
        return Result<MedicalRecordDto>.Success(createdMedicalRecordDto);
    }

    public async Task<Result<bool>> Update(MedicalRecordDto medicalRecordDto)
    {
        // Validate the medical record DTO
        var validations = await ValidateMedicalRecordDto(medicalRecordDto);
        if (!validations.IsValid)
            return Result<bool>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // Get the existing record
        var existingRecord = await _unitOfWork.MedicalRecords.GetById(medicalRecordDto.MedicalRecordID);
        
        if (existingRecord == null)
            return Result<bool>.Failure("Medical record not found", ServiceErrorType.NotFound);

        // Map the updated data to the existing entity
        _mapper.Map(medicalRecordDto, existingRecord);
        
        // Save changes to the database
        bool saveResult = await _unitOfWork.SaveChanges();
    
        if (!saveResult)
            return Result<bool>.Failure("Failed to update medical record in database", ServiceErrorType.DatabaseError);

        return Result<bool>.Success(true);
    }

    public Task<Result<bool>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}