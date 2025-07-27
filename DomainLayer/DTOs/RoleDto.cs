using System.ComponentModel.DataAnnotations;

namespace DomainLayer.DTOs;

/// <summary>
///     Data transfer object representing a role in the system
/// </summary>
public class RoleDto
{
    /// <summary>
    ///     The unique identifier for the role
    /// </summary>
    /// <example>1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p</example>
    [Required]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The name of the role
    /// </summary>
    /// <example>Administrator</example>
    [Required]
    public string Name { get; set; } = string.Empty;
}