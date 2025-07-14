using System.Security.Claims;
using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services;

public class RoleClaimService : IRoleClaimService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleClaimService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<string>> CreateRoleClaim(CreateRoleClaimDto roleClaimDto)
    {
        // Validate that the claim value is not null, empty, or whitespace only
        if (string.IsNullOrWhiteSpace(roleClaimDto.ClaimValue))
            return Result<string>.Failure("Claim value is required", ServiceErrorType.ValidationError);

        // Retrieve the role by its ID
        var role = await _roleManager.FindByIdAsync(roleClaimDto.RoleId);
        if (role is null)
            return Result<string>.Failure("Role not found", ServiceErrorType.NotFound);

        // Fetch all claims currently assigned to this role
        var existingClaims = await _roleManager.GetClaimsAsync(role);

        // Check if the claim already exists for this role to avoid duplicates
        var exists = existingClaims.Any(c =>
            c.Type == ClaimConstants.Permission &&
            c.Value == roleClaimDto.ClaimValue
        );

        if (exists)
            return Result<string>.Failure("Claim already exists for this role", ServiceErrorType.ValidationError);

        // Create a new claim object with the specified type and value
        var claim = new Claim(ClaimConstants.Permission, roleClaimDto.ClaimValue);

        // Add the claim to the role in the database
        var result = await _roleManager.AddClaimAsync(role, claim);

        // Check if the claim was added successfully
        if (!result.Succeeded)
            return Result<string>.Failure("Failed to create role claim", ServiceErrorType.DatabaseError);

        // Return success message
        return Result<string>.Success("Role claim created successfully");
    }
}