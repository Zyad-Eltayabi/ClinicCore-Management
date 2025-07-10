using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BusinessLayer.Validations;
using ClinicAPI.Helpers;
using DomainLayer.DTOs;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<JwtOptions> jwt, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new AuthResponseDto
                {
                    Message = "Email or Password Incorrect",
                    IsAuthenticated = false
                };

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var refreshToken = await GetActiveRefreshToken(user);
            return new AuthResponseDto
            {
                Message = "Login successful",
                Token = token,
                IsAuthenticated = true,
                UserName = user.UserName,
                Email = user.Email,
                //ExpiresOn = DateTime.Now.AddMinutes(_jwt.ExpirationInMinutes),
                Roles = roles.ToList(),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn
            };
        }

        public async Task<AuthResponseDto> RefreshToken(string token)
        {
            // check if refresh token belongs to user
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return new AuthResponseDto
                {
                    Message = "Invalid token",
                    IsAuthenticated = false
                };

            // check if refresh token is active
            var refreshToken = await _unitOfWork.RefreshTokens.Find(t => t.Token == token);
            if (!refreshToken.IsActive)
                return new AuthResponseDto
                {
                    Message = "Invalid token",
                    IsAuthenticated = false
                };

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var newJwtToken = await CreateJwtToken(user);
            return new AuthResponseDto
            {
                Message = "Token refreshed successfully",
                Token = newJwtToken,
                IsAuthenticated = true,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        public async Task<bool> RevokeToken(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return false;

            var refreshToken = await _unitOfWork.RefreshTokens.Find(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            // validate registerDto
            var validator = new RegisterValidator(_userManager, _roleManager);
            var validationResult = await validator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return new AuthResponseDto
                {
                    Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                };
            }

            var newUser = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    IsAuthenticated = false
                };
            }

            await _userManager.AddToRoleAsync(newUser, registerDto.RoleName);
            var token = await CreateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken();
            newUser.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(newUser);
            return new AuthResponseDto
            {
                Message = "User registered successfully",
                Token = token,
                IsAuthenticated = true,
                UserName = newUser.UserName,
                Email = newUser.Email,
                Roles = new List<string> { registerDto.RoleName },
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn
            };
        }

        private async Task<RefreshToken> GetActiveRefreshToken(ApplicationUser user)
        {
            // check if user has a refresh token
            var activeRefreshToken = await _unitOfWork.RefreshTokens.Find(t =>
                t.UserId == user.Id &&
                t.ExpiresOn > DateTime.Now &&
                t.RevokedOn == null);

            if (activeRefreshToken is not null)
                return activeRefreshToken;

            // at this point, the user has no active refresh token
            var newRefreshToken = GenerateRefreshToken();
            // save the new refresh token
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            return newRefreshToken;
        }

        private async Task<string> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            // add roles and role claims to claims
            foreach (var roleName in roles)
            {
                // add role to claims
                claims.Add(new Claim("role", roleName));
                // Get claims from role
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is not null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            var allClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }
                .Union(userClaims)
                .Union(claims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                allClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(_jwt.RefreshTokenExpirationInDays),
                CreatedOn = DateTime.Now
            };
        }
    }
}