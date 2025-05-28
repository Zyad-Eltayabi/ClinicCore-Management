using DomainLayer.DTOs;
using DomainLayer.Interfaces;
using FluentValidation;

namespace BusinessLayer.Validations;

public class CreateOrUpdatePrescriptionValidator : AbstractValidator<CreateOrUpdatePrescriptionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateOrUpdatePrescriptionValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Include(new CompletePrescriptionValidator());
        
        RuleFor(p => p.PrescriptionID)
            .GreaterThan(0)
            .When(p => p.PrescriptionID != 0)
            .WithMessage("Prescription ID must be greater than 0 when specified.");
        
        RuleFor(p => p.MedicalRecordId)
            .MustAsync(async (id, cancellation) => await IsMedicalRecordExist(id))
            .WithMessage("Medical Record does not exist.");
    }
    
    private async Task<bool> IsMedicalRecordExist(int medicalRecordId)
    {
        return await _unitOfWork.MedicalRecords.ExistsAsync(m => m.MedicalRecordID == medicalRecordId);
    }
}