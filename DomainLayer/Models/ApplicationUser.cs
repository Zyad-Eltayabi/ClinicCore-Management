using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DomainLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] [MaxLength(50)] public string FirstName { get; set; }

        [Required] [MaxLength(50)] public string LastName { get; set; }

        public virtual List<RefreshToken>? RefreshTokens { get; set; } = new();
    }
}