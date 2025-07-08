using DomainLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register (RegisterDto registerDto);
    }
}
