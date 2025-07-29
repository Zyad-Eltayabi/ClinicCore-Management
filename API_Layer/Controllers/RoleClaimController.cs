using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

/// <summary>
///     Controller for managing role-based claims in the system.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.SuperAdmin)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
[Consumes("application/json")]
public class RoleClaimController : Controller
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimController(IRoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    /// <summary>
    ///     Creates a new claim assignment for a specified role.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - The role must exist in the system
    ///     - The claim value cannot be empty or whitespace
    ///     - The claim must not already exist for the specified role
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "claimValue": "documents.read"
    ///     }
    ///     ```
    ///     ### Success Response Example:
    ///     ```json
    ///     "Role claim created successfully"
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Claim value is required"
    ///     ```
    ///     **Role Not Found (404):**
    ///     ```json
    ///     "Role not found"
    ///     ```
    ///     **Claim Exists (400):**
    ///     ```json
    ///     "Claim already exists for this role"
    ///     ```
    ///     **Database Error (500):**
    ///     ```json
    ///     "Failed to create role claim"
    ///     ```
    /// </remarks>
    /// <param name="roleClaimDto">The role claim creation data transfer object</param>
    /// <response code="200">Returns success message when claim is created</response>
    /// <response code="400">If the request is invalid (missing claim value or claim exists)</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user is not SuperAdmin</response>
    /// <response code="404">If the specified role doesn't exist</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is temporarily unavailable</response>
    [HttpPost("CreateRoleClaim")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateRoleClaim([FromBody] CreateRoleClaimDto roleClaimDto)
    {
        var response = await _roleClaimService.CreateRoleClaim(roleClaimDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Updates an existing claim assignment for a specified role.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only SuperAdmin users can perform this action
    ///     - The role must exist in the system
    ///     - Both old and new claim values cannot be empty or whitespace
    ///     - The old claim must exist for the specified role
    ///     - The new claim must not already exist for the role
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "oldClaimValue": "documents.read",
    ///     "newClaimValue": "documents.read-write"
    ///     }
    ///     ```
    ///     ### Success Response Example:
    ///     ```json
    ///     "Role claim updated successfully"
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Old and new claim values are required"
    ///     ```
    ///     **Role Not Found (404):**
    ///     ```json
    ///     "Role not found"
    ///     ```
    ///     **Old Claim Not Found (404):**
    ///     ```json
    ///     "Old claim not found for this role"
    ///     ```
    ///     **Conflict (409) - New claim exists:**
    ///     ```json
    ///     "New claim already exists for this role"
    ///     ```
    ///     **Database Error (500):**
    ///     ```json
    ///     "Failed to create role claim"
    ///     ```
    /// </remarks>
    /// <param name="roleClaimDto">The role claim update data transfer object</param>
    /// <response code="200">Returns success message when claim is updated</response>
    /// <response code="400">If the request is invalid or old claim doesn't exist</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user is not SuperAdmin</response>
    /// <response code="404">If the specified role doesn't exist</response>
    /// <response code="409">If the new claim already exists for the role</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is temporarily unavailable</response>
    [HttpPut("UpdateRoleClaim")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateRoleClaim([FromBody] UpdateRoleClaimDto roleClaimDto)
    {
        var response = await _roleClaimService.UpdateRoleClaim(roleClaimDto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Deletes a specific claim assignment from a role.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - The role must exist in the system
    ///     - The claim value cannot be empty or whitespace
    ///     - The specified claim must exist for the role
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "claimValue": "view-doctors"
    ///     }
    ///     ```
    ///     ### Success Response Example:
    ///     ```json
    ///     "Role claim removed successfully"
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Claim value is required"
    ///     ```
    ///     **Role Not Found (404):**
    ///     ```json
    ///     "Role not found"
    ///     ```
    ///     **Claim Not Found (404):**
    ///     ```json
    ///     "Claim not found for this role"
    ///     ```
    ///     **Database Error (500):**
    ///     ```json
    ///     "Failed to remove claim"
    ///     ```
    /// </remarks>
    /// <param name="DeleteRoleClaimDto">The role claim deletion data transfer object</param>
    /// <response code="200">Returns success message when claim is removed</response>
    /// <response code="400">If the request is invalid (missing claim value)</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user is not SuperAdmin</response>
    /// <response code="404">If the specified role or claim doesn't exist</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If the service is temporarily unavailable</response>
    [HttpDelete("DeleteRoleClaim")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteRoleClaim([FromBody] DeleteRoleClaimDto dto)
    {
        var response = await _roleClaimService.DeleteRoleClaimAsync(dto);
        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Retrieves all role-claim assignments in the system.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only authorized users can access this endpoint
    ///     - Returns all role-claim mappings in the system
    ///     - Returns empty list if no role claims exist (status 200)
    ///     ### Success Response Example:
    ///     ```json
    ///     [
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "roleName": "Administrator",
    ///     "claimType": "Permission",
    ///     "claimValue": "documents.manage"
    ///     },
    ///     {
    ///     "roleId": "b2c3d4e5-f6g7-8901-h2i3-j4k5l6m7n8o9",
    ///     "roleName": "Editor",
    ///     "claimType": "Permission",
    ///     "claimValue": "documents.edit"
    ///     }
    ///     ]
    ///     ```
    ///     ### Error Response Examples:
    ///     **No Roles Found (404):**
    ///     ```json
    ///     "No roles found"
    ///     ```
    ///     **No Role Claims Found (404):**
    ///     ```json
    ///     "No role claims found"
    ///     ```
    ///     **Server Error (500):**
    ///     ```json
    ///     "An error occurred while retrieving role claims"
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of all role-claim assignments</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks required permissions</response>
    /// <response code="404">If no roles or role claims exist</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("GetAllRoleClaims")]
    [ProducesResponseType(typeof(List<RoleClaimDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RoleClaimDto>>> GetAllRoleClaims()
    {
        var response = await _roleClaimService.GetAllRoleClaims();

        return response.ErrorType switch
        {
            ServiceErrorType.Success => Ok(response.Data),
            _ => StatusCode((int)response.ErrorType, response.Message)
        };
    }

    /// <summary>
    ///     Retrieves all claims assigned to a specific role.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Requires valid role ID in GUID format
    ///     - Role must exist in the system
    ///     - Returns empty list if role exists but has no claims (status 200)
    ///     - Only accessible by authorized users with appropriate permissions
    ///     ### Sample Request:
    ///     `GET /api/role-claims/get-claims-by-role-id/a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8`
    ///     ### Success Response Example:
    ///     ```json
    ///     [
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "roleName": "Admin",
    ///     "claimType": "Permission",
    ///     "claimValue": "view-doctors"
    ///     },
    ///     {
    ///     "roleId": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
    ///     "roleName": "Admin",
    ///     "claimType": "Permission",
    ///     "claimValue": "delete-doctors"
    ///     }
    ///     ]
    ///     ```
    ///     ### Empty Success Response:
    ///     ```json
    ///     []
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Role ID is required"
    ///     ```
    ///     **Role Not Found (404):**
    ///     ```json
    ///     "Role not found"
    ///     ```
    ///     **No Claims Found (404):**
    ///     Returns empty array (considered success case)
    ///     **Server Error (500):**
    ///     ```json
    ///     "An error occurred while retrieving role claims"
    ///     ```
    /// </remarks>
    /// <param name="roleId">The ID of the role to retrieve claims for (GUID format)</param>
    /// <response code="200">Returns list of claims for specified role (empty if no claims exist)</response>
    /// <response code="400">If role ID is missing or invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks required permissions</response>
    /// <response code="404">If specified role doesn't exist</response>
    /// <response code="500">If there was an internal server error</response>
    [ProducesResponseType(typeof(List<RoleClaimDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [HttpGet("GetClaimsByRoleId/{roleId}")]
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