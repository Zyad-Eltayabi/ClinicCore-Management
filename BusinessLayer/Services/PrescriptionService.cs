using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;

namespace BusinessLayer.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<CreateOrUpdatePrescriptionDto>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Result<CreateOrUpdatePrescriptionDto>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    private async Task<ValidationsResult> ValidatePrescriptionDto(CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var prescriptionValidator = new CreateOrUpdatePrescriptionValidator(_unitOfWork);
        var validations = await prescriptionValidator.ValidateAsync(prescriptionDto);
        if (!validations.IsValid)
        {
            string message = string.Join("; ", validations.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true);
    }

    public async Task<Result<CreateOrUpdatePrescriptionDto>> Add(CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var validations = await ValidatePrescriptionDto(prescriptionDto);
        if (!validations.IsValid)
            return Result<CreateOrUpdatePrescriptionDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);
    
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _unitOfWork.Prescriptions.Add(prescription);
        await _unitOfWork.SaveChanges();
    
        var createdPrescription = _mapper.Map<CreateOrUpdatePrescriptionDto>(prescription);
        return Result<CreateOrUpdatePrescriptionDto>.Success(createdPrescription);
    }

    public Task<Result<CreateOrUpdatePrescriptionDto>> Update(CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CreateOrUpdatePrescriptionDto>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}