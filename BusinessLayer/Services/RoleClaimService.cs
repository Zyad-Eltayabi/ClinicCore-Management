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

    public async Task<Result<string>> UpdateRoleClaim(UpdateRoleClaimDto roleClaimDto)
    {
        // Validate that the claim values are not null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(roleClaimDto.OldClaimValue) ||
            string.IsNullOrWhiteSpace(roleClaimDto.NewClaimValue))
            return Result<string>.Failure("Old and new claim values are required", ServiceErrorType.ValidationError);

        // Retrieve the role by its ID
        var role = await _roleManager.FindByIdAsync(roleClaimDto.RoleId);
        if (role is null)
            return Result<string>.Failure("Role not found", ServiceErrorType.NotFound);

        // Fetch all claims assigned to this role
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        // Check if the old claim exists
        var matchedClaim = roleClaims.FirstOrDefault(c =>
            c.Type == ClaimConstants.Permission &&
            c.Value == roleClaimDto.OldClaimValue);

        if (matchedClaim is null)
            return Result<string>.Failure("Old claim not found for this role", ServiceErrorType.ValidationError);

        // Ensure the new claim does not already exist
        var isNewClaimExists = roleClaims.Any(c =>
            c.Type == ClaimConstants.Permission &&
            c.Value == roleClaimDto.NewClaimValue);

        if (isNewClaimExists)
            return Result<string>.Failure("New claim already exists for this role", ServiceErrorType.ValidationError);

        // Remove the old claim
        var removeResult = await _roleManager.RemoveClaimAsync(role, matchedClaim);
        if (!removeResult.Succeeded)
            return Result<string>.Failure("Failed to remove old claim", ServiceErrorType.DatabaseError);

        // Add the new claim
        var newClaim = new Claim(ClaimConstants.Permission, roleClaimDto.NewClaimValue);
        var addResult = await _roleManager.AddClaimAsync(role, newClaim);
        if (!addResult.Succeeded)
            return Result<string>.Failure("Failed to add new claim", ServiceErrorType.DatabaseError);

        return Result<string>.Success("Role claim updated successfully");
    }
}