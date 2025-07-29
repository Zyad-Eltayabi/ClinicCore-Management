using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            return Result<RoleDto>.Failure("Role already exists", ServiceErrorType.Conflict);

        // create role
        var role = new IdentityRole(roleDto.Name);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return Result<RoleDto>.Failure("Failed to create role", ServiceErrorType.DatabaseError);

        // map role to roleDto
        roleDto.Id = role.Id;
        return Result<RoleDto>.Success(roleDto);
    }

    public async Task<Result<RoleDto>> GetRole(string id)
    {
        // get role by id
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Result<RoleDto>.Failure("Role not found", ServiceErrorType.NotFound);

        // map role to roleDto
        var roleDto = new RoleDto { Id = role.Id, Name = role.Name };
        return Result<RoleDto>.Success(roleDto);
    }

    public async Task<Result<List<RoleDto>>> GetAllRoles()
    {
        // get all roles
        var roles = await _roleManager.Roles.ToListAsync();
        if (roles.Count == 0)
            return Result<List<RoleDto>>.Failure("Roles not found", ServiceErrorType.NotFound);

        // map roles to roleDto
        var rolesDto = roles.Select(r =>
            new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

        return Result<List<RoleDto>>.Success(rolesDto);
    }

    public async Task<Result<RoleDto>> UpdateRole(RoleDto roleDto)
    {
        // check if role name is empty or whitespace or null
        if (string.IsNullOrEmpty(roleDto.Name) || string.IsNullOrWhiteSpace(roleDto.Name))
            return Result<RoleDto>.Failure("Role name is required", ServiceErrorType.ValidationError);

        // check if role exists
        var role = await _roleManager.FindByIdAsync(roleDto.Id);
        if (role is null)
            return Result<RoleDto>.Failure("Role not found", ServiceErrorType.NotFound);

        // update role
        role.Name = roleDto.Name;
        var result = await _roleManager.UpdateAsync(role);
        return result.Succeeded
            ? Result<RoleDto>.Success()
            : Result<RoleDto>.Failure("Failed to update role", ServiceErrorType.DatabaseError);
    }

    public async Task<Result<RoleDto>> DeleteRole(string id)
    {
        // get role by id
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Result<RoleDto>.Failure("Role not found", ServiceErrorType.NotFound);

        // delete role
        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded
            ? Result<RoleDto>.Success()
            : Result<RoleDto>.Failure("Failed to delete role", ServiceErrorType.DatabaseError);
    }
}