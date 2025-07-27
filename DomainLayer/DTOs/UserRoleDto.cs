using System.ComponentModel.DataAnnotations;

namespace DomainLayer.DTOs;

public class UserRoleDto
{
    /// <summary>
    ///     The unique identifier of the user to be assigned to or removed from a role
    /// </summary>
    /// <example>a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6</example>
    [Required]
    [RegularExpression(@"^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$",
        ErrorMessage = "Invalid GUID format for UserId")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     The unique identifier of the role to assign to or remove from the user
    /// </summary>
    /// <example>b2c3d4e5-f6g7-h8i9-j0k1-l2m3n4o5p6q7</example>
    [Required]
    [RegularExpression(@"^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$",
        ErrorMessage = "Invalid GUID format for RoleId")]
    public string RoleId { get; set; } = string.Empty;
}