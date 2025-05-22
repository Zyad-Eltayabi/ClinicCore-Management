using DomainLayer.DTOs;
using FluentValidation;

namespace BusinessLayer.Validations;

public class AppointmentValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentValidator()
    {
        ApplyValidations();
    }

    
    private void ApplyValidations()
    {
        RuleFor(a => a.AppointmentDateTime)
            .NotEmpty().WithMessage("Appointment date and time is required.")
            .Must(date => date > DateTime.Now)
            .WithMessage("Appointment date must be in the future.");

        RuleFor(a => a.AppointmentStatus)
            .InclusiveBetween((short)1, (short)4)
            .WithMessage("Appointment status must be between 1 and 4.");

        RuleFor(a => a.PatientID)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0.");

        RuleFor(a => a.DoctorID)
            .GreaterThan(0)
            .WithMessage("Doctor ID must be greater than 0.");

        RuleFor(a => a.MedicalRecordID)
            .GreaterThan(0)
            .When(a => a.MedicalRecordID.HasValue)
            .WithMessage("Medical Record ID must be greater than 0 when specified.");

        When(a => a.MedicalRecordDto != null, () =>
        {
            RuleFor(a => a.MedicalRecordDto)
                .SetValidator(new MedicalRecordValidator());
        });
    }
}