namespace DomainLayer.DTOs;

public class RoleClaimDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
}