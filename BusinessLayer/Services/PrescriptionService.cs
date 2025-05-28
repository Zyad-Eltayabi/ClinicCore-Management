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

    public async Task<Result<IEnumerable<CreateOrUpdatePrescriptionDto>>> GetAll()
    {
        var prescriptions = await _unitOfWork.Prescriptions.GetAll();
        if (prescriptions == null)
            return Result<IEnumerable<CreateOrUpdatePrescriptionDto>>.Failure("No prescriptions found", ServiceErrorType.NotFound);

        var prescriptionDtos = _mapper.Map<IEnumerable<CreateOrUpdatePrescriptionDto>>(prescriptions);
        return Result<IEnumerable<CreateOrUpdatePrescriptionDto>>.Success(prescriptionDtos);
    }

    public async Task<Result<CreateOrUpdatePrescriptionDto>> GetById(int id)
    {
        var prescription = await _unitOfWork.Prescriptions.GetById(id);
        if (prescription == null)
            return Result<CreateOrUpdatePrescriptionDto>.Failure("Prescription not found", ServiceErrorType.NotFound);

        var prescriptionDto = _mapper.Map<CreateOrUpdatePrescriptionDto>(prescription);
        return Result<CreateOrUpdatePrescriptionDto>.Success(prescriptionDto);
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
            return Result<CreateOrUpdatePrescriptionDto>.Failure(validations.ErrorMessage,
                ServiceErrorType.ValidationError);

        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        await _unitOfWork.Prescriptions.Add(prescription);
        await _unitOfWork.SaveChanges();

        var createdPrescription = _mapper.Map<CreateOrUpdatePrescriptionDto>(prescription);
        return Result<CreateOrUpdatePrescriptionDto>.Success(createdPrescription);
    }

    public async Task<Result<CreateOrUpdatePrescriptionDto>> Update(CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var validations = await ValidatePrescriptionDto(prescriptionDto);
        if (!validations.IsValid)
            return Result<CreateOrUpdatePrescriptionDto>.Failure(validations.ErrorMessage,
                ServiceErrorType.ValidationError);

        var existingPrescription = await _unitOfWork.Prescriptions.GetById(prescriptionDto.PrescriptionID);
        if (existingPrescription == null)
            return Result<CreateOrUpdatePrescriptionDto>.Failure("Prescription not found", ServiceErrorType.NotFound);

        _mapper.Map(prescriptionDto, existingPrescription);
        _unitOfWork.Prescriptions.Update(existingPrescription);
        await _unitOfWork.SaveChanges();

        var updatedPrescription = _mapper.Map<CreateOrUpdatePrescriptionDto>(existingPrescription);
        return Result<CreateOrUpdatePrescriptionDto>.Success(updatedPrescription);
    }

    public Task<Result<CreateOrUpdatePrescriptionDto>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}