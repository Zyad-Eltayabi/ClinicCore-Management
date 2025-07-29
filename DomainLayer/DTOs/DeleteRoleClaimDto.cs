namespace DomainLayer.DTOs;

/// <summary>
///     Data transfer object for deleting a role claim.
/// </summary>
public class DeleteRoleClaimDto
{
    /// <summary>
    ///     The unique identifier of the role from which the claim will be removed.
    /// </summary>
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    ///     The claim value representing the permission to be removed.
    ///     Must be in the format 'resource.action' (e.g., 'view-doctors').
    /// </summary>
    public string ClaimValue { get; set; } = string.Empty;
}