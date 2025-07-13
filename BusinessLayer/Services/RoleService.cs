using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<RoleDto>> CreateRole(RoleDto roleDto)
    {
        // check if role exists
        if (await _roleManager.RoleExistsAsync(roleDto.Name))
            return Result<RoleDto>.Failure("Role already exists", ServiceErrorType.ValidationError);

        // create role
        var role = new IdentityRole(roleDto.Name);
        var result = await _roleManager.CreateAsync(role);
        return result.Succeeded
            ? Result<RoleDto>.Success()
            : Result<RoleDto>.Failure("Failed to create role", ServiceErrorType.DatabaseError);
    }

    public async Task<Result<RoleDto>> GetRole(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<RoleDto>>> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<RoleDto>> UpdateRole(RoleDto roleDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<RoleDto>> DeleteRole(int id)
    {
        throw new NotImplementedException();
    }
}