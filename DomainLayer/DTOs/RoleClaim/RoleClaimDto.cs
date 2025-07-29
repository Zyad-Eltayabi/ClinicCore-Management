namespace DomainLayer.DTOs;

/// <summary>
///     Data transfer object representing a role-claim assignment.
/// </summary>
public class RoleClaimDto
{
    /// <summary>
    ///     The unique identifier of the role.
    /// </summary>
    /// <example>a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8</example>
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    ///     The name of the role.
    /// </summary>
    /// <example>Admin</example>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    ///     The type of the claim (typically "Permission").
    /// </summary>
    /// <example>Permission</example>
    public string ClaimType { get; set; } = string.Empty;

    /// <summary>
    ///     The value of the claim representing the permission.
    /// </summary>
    /// <example>view-doctors</example>
    public string ClaimValue { get; set; } = string.Empty;
}