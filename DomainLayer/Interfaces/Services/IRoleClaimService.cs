using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IRoleClaimService
{
    Task<Result<string>> CreateRoleClaim(CreateRoleClaimDto roleClaimDto);
}