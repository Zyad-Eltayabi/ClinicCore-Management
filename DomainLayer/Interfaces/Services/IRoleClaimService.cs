using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IRoleClaimService
{
    Task<Result<string>> CreateRoleClaim(CreateRoleClaimDto roleClaimDto);
    Task<Result<string>> UpdateRoleClaim(UpdateRoleClaimDto roleClaimDto);
    Task<Result<string>> DeleteRoleClaimAsync(DeleteRoleClaimDto roleClaimDto);
    Task<Result<List<RoleClaimDto>>> GetAllRoleClaims();
    Task<Result<List<RoleClaimDto>>> GetClaimsByRoleId(string roleId);
}