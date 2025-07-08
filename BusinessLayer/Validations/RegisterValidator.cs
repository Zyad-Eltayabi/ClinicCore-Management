using DomainLayer.DTOs;
using DomainLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validations
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterValidator(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            ApplyValidations();
        }

        private void ApplyValidations()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(128).WithMessage("Email must not exceed 128 characters")
                .EmailAddress().WithMessage("The email must be valid");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(256).WithMessage("Password must not exceed 256 characters")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[#@$!%*?&]").WithMessage("Password must contain at least one special character (@$!%*?&#)");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters");

            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellation) => await EmailExists(email))
                .WithMessage("User with this email already exists");

            RuleFor(x => x.UserName)
                .MustAsync(async (userName, cancellation) => await UserNameExists(userName))
                .WithMessage("User with this username already exists");

            RuleFor(x => x.RoleName)
                .MustAsync(async (roleName, cancellation) => await RoleExists(roleName))
                .WithMessage("This Role does not exist");
        }

        private async Task<bool> EmailExists(string email)
            => await _userManager.FindByEmailAsync(email) is null;
       
        private async Task<bool> UserNameExists(string userName)
            => await _userManager.FindByNameAsync(userName) is null;

        private async Task<bool> RoleExists(string roleName)
            => await _roleManager.FindByNameAsync(roleName) is not null;
       
    }
}