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
            ServiceErrorType.Success => CreatedAtAction(
                nameof(GetById),
                new { id = response.Data.Id }
                , response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpPut]
    [Route("UpdateRole")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> UpdateRole([FromBody] RoleDto roleDto)
    {
        // Call the service method to update a role
        var response = await _roleService.UpdateRole(roleDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpGet]
    [Route("GetById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<RoleDto>> GetById(string id)
    {
        // Call the service method to get a role by ID
        var response = await _roleService.GetRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    [HttpDelete]
    [Route("DeleteRole/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> DeleteRole(string id)
    {
        // Call the service method to delete a role
        var response = await _roleService.DeleteRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }
}