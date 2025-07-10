using DomainLayer.DTOs;

namespace DomainLayer.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<AuthResponseDto> RefreshToken(string token);
        Task<bool> RevokeToken(string token);
    }
}