using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.SuperAdmin)]
public class UserRoleController : ControllerBase
{
    private readonly IUserRoleService _userRoleService;

    public UserRoleController(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    /// <summary>
    ///     Assigns a user to a specific role in the system.
    /// </summary>
    /// <remarks>
    ///     This endpoint allows super administrators to add users to roles for access control purposes.
    ///     Sample request:
    ///     POST /api/UserRole/AddUserToRole
    ///     {
    ///     "userId": "8e445865-a24d-4543-a6c6-9443d048cdb9",
    ///     "roleId": "1e445865-a24d-4543-a6c6-9443d048cdb9"
    ///     }
    /// </remarks>
    /// <param name="userRoleDto">The user and role mapping information</param>
    /// <returns>A success message if the user was added to the role</returns>
    /// <response code="200">Returns success message when the user is successfully added to the role</response>
    /// <response code="400">If the request body is invalid or validation fails</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not a SuperAdmin)</response>
    /// <response code="404">If the user or role is not found</response>
    /// <response code="405">If the request method is not allowed</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the database service is unavailable</response>
    [HttpPost("AddUserToRole")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDto userRoleDto)
    {
        var result = await _userRoleService.AddUserToRole(userRoleDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }

    /// <summary>
    ///     Removes a user from a specific role in the system.
    /// </summary>
    /// <remarks>
    ///     This endpoint allows super administrators to remove users from roles for access control purposes.
    ///     Sample request:
    ///     DELETE /api/UserRole/RemoveUserFromRole
    ///     {
    ///     "userId": "8e445865-a24d-4543-a6c6-9443d048cdb9",
    ///     "roleId": "1e445865-a24d-4543-a6c6-9443d048cdb9"
    ///     }
    /// </remarks>
    /// <param name="userRoleDto">The user and role mapping information</param>
    /// <returns>A success message if the user was removed from the role</returns>
    /// <response code="200">Returns success message when the user is successfully removed from the role</response>
    /// <response code="400">If the request body is invalid or validation fails</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized (not a SuperAdmin)</response>
    /// <response code="404">If the user or role is not found</response>
    /// <response code="405">If the request method is not allowed</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the database service is unavailable</response>
    [HttpDelete("RemoveUserFromRole")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<IActionResult> RemoveUserFromRole([FromBody] UserRoleDto userRoleDto)
    {
        var result = await _userRoleService.RemoveUserFromRole(userRoleDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
}