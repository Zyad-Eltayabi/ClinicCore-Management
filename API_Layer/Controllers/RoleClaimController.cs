using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.SuperAdmin)]
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

    [HttpPut]
    [Route("UpdateRoleClaim")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> UpdateRoleClaim([FromBody] UpdateRoleClaimDto roleClaimDto)
    {
        var response = await _roleClaimService.UpdateRoleClaim(roleClaimDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpDelete("DeleteRoleClaim")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> DeleteRoleClaim([FromBody] DeleteRoleClaimDto dto)
    {
        var response = await _roleClaimService.DeleteRoleClaimAsync(dto);

        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpGet("GetAllRoleClaims")]
    [ProducesResponseType(typeof(List<RoleClaimDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RoleClaimDto>>> GetAllRoleClaims()
    {
        var response = await _roleClaimService.GetAllRoleClaims();

        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpGet("GetClaimsByRoleId/{roleId}")]
    [ProducesResponseType(typeof(List<RoleClaimDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RoleClaimDto>>> GetClaimsByRoleId(string roleId)
    {
        var response = await _roleClaimService.GetClaimsByRoleId(roleId);

        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }
}