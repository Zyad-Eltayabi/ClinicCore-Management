using System.Security.Claims;
using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            return Result<string>.Failure("Old claim not found for this role", ServiceErrorType.NotFound);

        // Ensure the new claim does not already exist
        var isNewClaimExists = roleClaims.Any(c =>
            c.Type == ClaimConstants.Permission &&
            c.Value == roleClaimDto.NewClaimValue);

        if (isNewClaimExists)
            return Result<string>.Failure("New claim already exists for this role", ServiceErrorType.Conflict);

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

    public async Task<Result<string>> DeleteRoleClaimAsync(DeleteRoleClaimDto roleClaimDto)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(roleClaimDto.ClaimValue))
            return Result<string>.Failure("Claim value is required", ServiceErrorType.ValidationError);

        // Retrieve the role by its ID
        var role = await _roleManager.FindByIdAsync(roleClaimDto.RoleId);
        if (role is null)
            return Result<string>.Failure("Role not found", ServiceErrorType.NotFound);

        // Fetch all claims assigned to the role
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        // Find the claim to remove
        var matchedClaim = roleClaims.FirstOrDefault(c =>
            c.Type == ClaimConstants.Permission &&
            c.Value == roleClaimDto.ClaimValue);

        if (matchedClaim is null)
            return Result<string>.Failure("Claim not found for this role", ServiceErrorType.NotFound);

        // Remove the claim from the role
        var result = await _roleManager.RemoveClaimAsync(role, matchedClaim);
        if (!result.Succeeded)
            return Result<string>.Failure("Failed to remove claim", ServiceErrorType.DatabaseError);

        return Result<string>.Success("Role claim removed successfully");
    }

    public async Task<Result<List<RoleClaimDto>>> GetAllRoleClaims()
    {
        // Retrieve all roles
        var roles = await _roleManager.Roles.ToListAsync();
        if (roles.Count == 0)
            return Result<List<RoleClaimDto>>.Failure("No roles found", ServiceErrorType.NotFound);

        var roleClaims = new List<RoleClaimDto>();

        foreach (var role in roles)
        {
            // Get all claims assigned to this role
            var claims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
                roleClaims.Add(new RoleClaimDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
        }

        if (roleClaims.Count == 0)
            return Result<List<RoleClaimDto>>.Failure("No role claims found", ServiceErrorType.NotFound);

        return Result<List<RoleClaimDto>>.Success(roleClaims);
    }

    public async Task<Result<List<RoleClaimDto>>> GetClaimsByRoleId(string roleId)
    {
        // Validate roleId
        if (string.IsNullOrWhiteSpace(roleId))
            return Result<List<RoleClaimDto>>.Failure("Role ID is required", ServiceErrorType.ValidationError);

        // Find role
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Result<List<RoleClaimDto>>.Failure("Role not found", ServiceErrorType.NotFound);

        // Get claims for this role
        var claims = await _roleManager.GetClaimsAsync(role);

        if (claims.Count == 0)
            return Result<List<RoleClaimDto>>.Failure("No claims found for this role", ServiceErrorType.NotFound);

        // Map claims to DTOs
        var roleClaims = claims.Select(c => new RoleClaimDto
        {
            RoleId = role.Id,
            RoleName = role.Name,
            ClaimType = c.Type,
            ClaimValue = c.Value
        }).ToList();

        return Result<List<RoleClaimDto>>.Success(roleClaims);
    }
}