using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing application roles and permissions.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.SuperAdmin)]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status405MethodNotAllowed)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    /// <summary>
    ///     Initializes a new instance of the RoleController.
    /// </summary>
    /// <param name="roleService">The role service for handling business logic.</param>
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Creates a new role in the system.
    /// </summary>
    /// <remarks>
    /// ### Business Rules:
    /// - Only SuperAdmin users can create roles
    /// - Role ID must be empty (will be generated)
    /// - Role name must be unique
    /// 
    /// ### Sample Request:
    /// ```json
    /// {
    ///       "id": "",
    ///     "name": "Manager"
    /// }
    /// ```
    /// 
    /// ### Success Response Example (201 Created):
    /// ```json
    /// {
    ///     "id": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "name": "Manager"
    /// }
    /// ```
    /// 
    /// ### Error Response Examples:
    /// **Validation Error (400):**
    /// ```json
    /// "Role name is required"
    /// ```
    /// 
    /// **Conflict (409):**
    /// ```json
    /// "Role name already exists"
    /// ```
    /// </remarks>
    /// <param name="roleDto">The role details to create</param>
    /// <response code="201">Returns the newly created role</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="409">If the role name already exists</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpPost("add-role")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddRole([FromBody] RoleDto roleDto)
    {
        if (!string.IsNullOrEmpty(roleDto.Id))
            return BadRequest("Role ID should be empty");

        var response = await _roleService.CreateRole(roleDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(
                nameof(GetById),
                new { id = response.Data.Id },
                response.Data),
            ServiceErrorType.ValidationError => BadRequest(response.Message),
            ServiceErrorType.Conflict => Conflict(response.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, response.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response.Message)
        };
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <remarks>
    /// ### Business Rules:
    /// - Only SuperAdmin users can update roles
    /// - Role must exist
    /// - Role name must be unique
    /// 
    /// ### Sample Request:
    /// ```json
    /// {
    ///     "id": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "name": "Admin"
    /// }
    /// ```
    /// 
    /// ### Success Response (204 NoContent):
    /// Empty response body
    /// 
    /// ### Error Response Examples:
    /// **Validation Error (400):**
    /// ```json
    /// "Role name is required"
    /// ```
    /// 
    /// **Not Found (404):**
    /// ```json
    /// "Role not found"
    /// ```
    /// </remarks>
    /// <param name="roleDto">The role details to update</param>
    /// <response code="204">If the role was successfully updated</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the role was not found</response>
    /// <response code="409">If the role name already exists</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpPut("update-role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateRole([FromBody] RoleDto roleDto)
    {
        var response = await _roleService.UpdateRole(roleDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            ServiceErrorType.ValidationError => BadRequest(response.Message),
            ServiceErrorType.NotFound => NotFound(response.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, response.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response.Message)
        };
    }

    /// <summary>
    /// Retrieves a specific role by ID.
    /// </summary>
    /// <remarks>
    /// ### Business Rules:
    /// - Only SuperAdmin users can view roles
    /// - Role must exist
    /// 
    /// ### Success Response Example (200 OK):
    /// ```json
    /// {
    ///     "id": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "name": "Manager"
    /// }
    /// ```
    /// 
    /// ### Error Response Examples:
    /// **Not Found (404):**
    /// ```json
    /// "Role not found"
    /// ```
    /// </remarks>
    /// <param name="id">The ID of the role to retrieve</param>
    /// <response code="200">Returns the requested role</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks permissions</response>
    /// <response code="404">If the role was not found</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetById(string id)
    {
        var response = await _roleService.GetRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            ServiceErrorType.NotFound => NotFound(response.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, response.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response.Message)
        };
    }

    /// <summary>
    /// Deletes a role from the system.
    /// </summary>
    /// <remarks>
    /// ### Business Rules:
    /// - Only SuperAdmin users can delete roles
    /// - Role must exist
    /// - Default roles cannot be deleted
    /// 
    /// ### Sample Request:
    /// `DELETE /api/role/a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8`
    /// 
    /// ### Success Response (200 OK):
    /// Empty response body
    /// 
    /// ### Error Response Examples:
    /// **Not Found (404):**
    /// ```json
    /// "Role not found"
    /// ```
    /// 
    /// 
    /// </remarks>
    /// <param name="id">The ID of the role to delete</param>
    /// <response code="200">If the role was successfully deleted</response>
    /// <response code="403">If attempting to delete a default role</response>
    /// <response code="404">If the role was not found</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRole(string id)
    {
        var response = await _roleService.DeleteRole(id);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.NotFound => NotFound(response.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, response.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response.Message)
        };
    }

    /// <summary>
    /// Retrieves all roles in the system.
    /// </summary>
    /// <remarks>
    /// ### Business Rules:
    /// - Only SuperAdmin users can view all roles
    /// 
    /// ### Success Response Example (200 OK):
    /// ```json
    /// [
    ///     {
    ///         "id": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///         "name": "Admin"
    ///     },
    ///     {
    ///         "id": "b2c3d4e5-f6g7-8901-h2i3-j4k5l6m7n8o9",
    ///         "name": "Manager"
    ///     }
    /// ]
    /// ```
    /// 
    /// ### Error Response Examples:
    /// **Not Found (404):**
    /// ```json
    /// "No roles found"
    /// ```
    /// </remarks>
    /// <response code="200">Returns the list of all roles</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks permissions</response>
    /// <response code="404">If no roles were found</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is unavailable</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        var response = await _roleService.GetAllRoles();
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            ServiceErrorType.NotFound => NotFound(response.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, response.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response.Message)
        };
    }
}