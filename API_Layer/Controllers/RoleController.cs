using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

/// <summary>
///     Controller for managing application roles
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.SuperAdmin)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
[ProducesResponseType(StatusCodes.Status405MethodNotAllowed, Type = typeof(string))]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    ///     Creates a new role in the system
    /// </summary>
    /// <param name="roleDto">The role details to create</param>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/Role/AddRole
    ///     {
    ///     "id": "",
    ///     "name": "Manager",
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created role</response>
    /// <response code="400">If the role ID is not empty or the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpPost]
    [Route("AddRole")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<ActionResult> AddRole([FromBody] RoleDto roleDto)
    {
        if (!string.IsNullOrEmpty(roleDto.Id))
            return BadRequest("Role Id should be empty");

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

    /// <summary>
    ///     Updates an existing role
    /// </summary>
    /// <param name="roleDto">The role details to update</param>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/Role/UpdateRole
    ///     {
    ///     "id": "1",
    ///     "name": "Senior Manager",
    ///     }
    /// </remarks>
    /// <response code="204">If the role was successfully updated</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the role was not found</response>
    [HttpPut]
    [Route("UpdateRole")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<ActionResult> UpdateRole([FromBody] RoleDto roleDto)
    {
        var response = await _roleService.UpdateRole(roleDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Retrieves a specific role by id
    /// </summary>
    /// <param name="id">The ID of the role to retrieve</param>
    /// <remarks>
    ///     Sample request:
    ///     GET /api/Role/GetById/1
    /// </remarks>
    /// <response code="200">Returns the requested role</response>
    /// <response code="404">If the role was not found</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpGet]
    [Route("GetById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<ActionResult<RoleDto>> GetById(string id)
    {
        var response = await _roleService.GetRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Deletes a role from the system
    /// </summary>
    /// <param name="id">The ID of the role to delete</param>
    /// <remarks>
    ///     Sample request:
    ///     DELETE /api/Role/DeleteRole/1
    /// </remarks>
    /// <response code="200">If the role was successfully deleted</response>
    /// <response code="404">If the role was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete]
    [Route("DeleteRole/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<ActionResult> DeleteRole(string id)
    {
        var response = await _roleService.DeleteRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Retrieves all roles in the system
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     GET /api/Role
    /// </remarks>
    /// <response code="200">Returns the list of all roles</response>
    /// <response code="404">If no roles were found</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoleDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        var response = await _roleService.GetAllRoles();
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }
}