using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }


    [HttpPost]
    [Route("AddRole")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> AddRole([FromBody] RoleDto roleDto)
    {
        // Check if roleDto.Id is not empty
        if (!string.IsNullOrEmpty(roleDto.Id))
            return BadRequest("Role Id should be empty");

        // Call the service method to add a new role
        var response = await _roleService.CreateRole(roleDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(AddRole), response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }
}