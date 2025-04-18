using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.Enums;
using DomainLayer.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessLayer.Validations
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator(GeneralEnum.SaveMode mode)
        {
            ApplyValidations(mode);
        }

        public void ApplyValidations(GeneralEnum.SaveMode mode)
        {
            RuleFor(p => p.FullName)
                .NotNull().WithMessage("Full Name is required.")  
                .NotEmpty().WithMessage("Full Name cannot be empty.");  

            RuleFor(p => p.DateOfBirth)
                .Must(date => date < DateTime.Now.AddYears(-18)) 
                .WithMessage("Patient must be at least 18 years old.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")  
                .EmailAddress().WithMessage("Invalid email format.");  

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")  
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone Number must be between 10 and 15 digits and may start with a '+' sign.");  // Ensures correct phone number format

            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Address must not be empty.")  
                .NotNull().WithMessage("Address must not be null.");  

            if (mode == GeneralEnum.SaveMode.Add)
            {
                RuleFor(p => p.Id)
                    .Equal(0).WithMessage("New patient should not have an ID.");
            }

        }
    }

}
