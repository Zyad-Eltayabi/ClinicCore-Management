using System.Data;
using DomainLayer.DTOs;
using FluentValidation;

namespace BusinessLayer.Validations;

public class CompletePrescriptionValidator : AbstractValidator<CompletePrescriptionDto>
{
    public CompletePrescriptionValidator()
    {
        RuleFor(p => p.MedicationName)
            .NotEmpty().WithMessage("Medication name is required.")
            .MaximumLength(100).WithMessage("Medication name cannot exceed 100 characters.");

        RuleFor(p => p.Dosage)
            .NotEmpty().WithMessage("Dosage is required.")
            .MaximumLength(50).WithMessage("Dosage cannot exceed 50 characters.");

        RuleFor(p => (int)p.FrequencyPerDay)
            .NotNull().WithMessage("Frequency per day is required.")
            .GreaterThan(0).WithMessage("Frequency per day must be greater than 0.");

        RuleFor(p => p.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must(date => date >= DateTime.Now)
            .WithMessage("Start date must be in the future.");

        RuleFor(p => p)
            .Custom((pre, context) =>
                {
                    if(pre.EndDate <= pre.StartDate)
                        context.AddFailure("End date must be after start date.");
                }
            );

        RuleFor(p => p.SpecialInstructions)
            .MaximumLength(200).WithMessage("Special instructions cannot exceed 200 characters.")
            .When(p => !string.IsNullOrEmpty(p.SpecialInstructions));
    }
}