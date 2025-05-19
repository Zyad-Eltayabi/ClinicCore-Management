using DomainLayer.DTOs;
using FluentValidation;

namespace BusinessLayer.Validations;

public class DoctorValidator : AbstractValidator<DoctorDto>
{
    public DoctorValidator()
    {
        ApplyValidations();
    }

    public void ApplyValidations()
    {
        RuleFor(p => p.FullName)
            .NotNull().WithMessage("Full Name is required.")
            .NotEmpty().WithMessage("Full Name cannot be empty.")
            .MaximumLength(100).WithMessage($"Full Name does not accept length bigger than 100");

        RuleFor(p => p.DateOfBirth)
            .Must(date => date < DateTime.Now.AddYears(-18))
            .WithMessage("Patient must be at least 18 years old.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage($"Email does not accept length bigger than 100");

        RuleFor(p => p.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .MaximumLength(20).WithMessage($"Phone number does not accept length bigger than 20")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Phone Number must be between 10 and 15 digits and may start with a '+' sign.");  // Ensures correct phone number format

        RuleFor(p => p.Address)
            .MaximumLength(200).WithMessage($"Address does not accept length bigger than 200")
            .NotEmpty().WithMessage("Address must not be empty.")
            .NotNull().WithMessage("Address must not be null.");
        
        RuleFor(p => p.Specialization)
            .MaximumLength(100).WithMessage($"Specialization does not accept length bigger than 100")
            .NotEmpty().WithMessage("Specialization must not be empty.")
            .NotNull().WithMessage("Specialization must not be null.");

    }

}