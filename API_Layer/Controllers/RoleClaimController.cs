using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleClaimController : Controller
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimController(IRoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    [HttpPost]
    [Route("CreateRoleClaim")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateRoleClaim([FromBody] CreateRoleClaimDto roleClaimDto)
    {
        var response = await _roleClaimService.CreateRoleClaim(roleClaimDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }
}