namespace DomainLayer.DTOs;

/// <summary>
///     Data transfer object for creating a new role claim.
/// </summary>
public class CreateRoleClaimDto
{
    /// <summary>
    ///     The unique identifier of the role to which the claim will be assigned.
    /// </summary>
    /// <example>a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8</example>
    public string RoleId { get; set; }

    /// <summary>
    ///     The claim value representing the permission being assigned.
    ///     Must be in the format 'resource.action' (e.g., 'view-doctors').
    /// </summary>
    /// <example>view-doctors</example>
    public string ClaimValue { get; set; } = string.Empty;
}