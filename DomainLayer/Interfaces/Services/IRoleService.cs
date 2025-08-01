using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IRoleService
{
    Task<Result<RoleDto>> CreateRole(RoleDto roleDto);
    Task<Result<RoleDto>> GetRole(string id);
    Task<Result<List<RoleDto>>> GetAllRoles();
    Task<Result<RoleDto>> UpdateRole(RoleDto roleDto);
    Task<Result<RoleDto>> DeleteRole(string id);
}