using DomainLayer.DTOs;
using FluentValidation;

namespace BusinessLayer.Validations;

public class MedicalRecordValidator : AbstractValidator<MedicalRecordDto>
{
    public MedicalRecordValidator()
    {
        ApplyValidations();
    }

    private void ApplyValidations()
    {
        RuleFor(m => m.VisitDescription)
            .NotEmpty().WithMessage("Visit description is required.")
            .NotNull().WithMessage("Visit description cannot be null.")
            .MinimumLength(10).WithMessage("Visit description must be at least 10 characters long.")
            .MaximumLength(200).WithMessage("Visit description cannot exceed 200 characters.");

        RuleFor(m => m.Diagnosis)
            .MaximumLength(200).WithMessage("Diagnosis cannot exceed 200 characters.")
            .When(m => m.Diagnosis != null);

        RuleFor(m => m.AdditionalNotes)
            .MaximumLength(200).WithMessage("Additional notes cannot exceed 200 characters.")
            .When(m => m.AdditionalNotes != null);

        RuleFor(m => m.MedicalRecordID)
            .GreaterThan(0).When(m => m.MedicalRecordID != 0)
            .WithMessage("Medical Record ID must be greater than 0 when specified.");
    }
}