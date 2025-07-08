using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class AuthResponseDto
    {
        public string message { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public bool isAuthenticated { get; set; } = false;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
