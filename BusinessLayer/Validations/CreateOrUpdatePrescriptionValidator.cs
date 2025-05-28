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
        
        // Prevent duplicate prescriptions by validating that a medical record ID doesn't exist in prescriptions table
        RuleFor(p => p)
            .CustomAsync(async (prescription, context, cancellation) =>
            {
                if (prescription.PrescriptionID == 0)
                {
                    bool exists = await IsMedicalRecordIdExistInPrescriptionsTable(prescription.MedicalRecordId);
                    
                    if (exists)
                        context.AddFailure("MedicalRecordId", 
                            "Medical Record ID already exists in prescriptions table.");
                }
            });
    }

    private async Task<bool> IsMedicalRecordExist(int medicalRecordId)
    {
        return await _unitOfWork.MedicalRecords.ExistsAsync(m => m.MedicalRecordID == medicalRecordId);
    }

    private async Task<bool> IsMedicalRecordIdExistInPrescriptionsTable(int medicalRecordId)
    {
        return await _unitOfWork.Prescriptions.ExistsAsync(p => p.MedicalRecordId == medicalRecordId);
    }
}