using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        ///     Registers a new user in the system with the specified role.
        /// </summary>
        /// <param name="registerDto">
        ///     The registration details containing user information:
        ///     FirstName, LastName, Email, UserName, Password and RoleName
        /// </param>
        /// <remarks>
        ///     ## This endpoint allows super admin to create new user accounts with specified roles.
        ///     ### Business Rules:
        ///     - Only Super Administrators can register new users
        ///     - Email and username must be unique across the system
        ///     - Password must meet complexity requirements
        ///     - Specified role must exist in the system
        ///     ### Validation Rules:
        ///     - First name: Required, max 100 characters
        ///     - Last name: Required, max 100 characters
        ///     - Email: Required, valid format, max 128 characters, unique
        ///     - Username: Required, max 50 characters, unique
        ///     - Password: 6-256 chars, with uppercase, lowercase, number and special character
        ///     - Role name: Required, min 3 chars, must exist in system
        ///     ### Sample request:
        ///     POST /api/auth/register
        ///     ``` json
        ///     {
        ///     "firstName": "John",
        ///     "lastName": "Doe",
        ///     "email": "john.doe@example.com",
        ///     "userName": "johndoe",
        ///     "password": "StrongP@ssw0rd",
        ///     "roleName": "Receptionist"
        ///     }
        ///     ```
        ///     ### Sample success response:
        ///     ``` json
        ///     {
        ///     "message": "User registered successfully",
        ///     "token": "eyJhbGci...[JWT token]",
        ///     "isAuthenticated": true,
        ///     "userName": "johndoe",
        ///     "email": "john.doe@example.com",
        ///     "roles": ["Receptionist"],
        ///     "refreshTokenExpiresOn": "2024-01-01T00:00:00"
        ///     }
        ///     ```
        /// </remarks>
        /// <response code="200">Returns the authentication response with JWT token when registration is successful</response>
        /// <response code="400">If the registration request is invalid or validation fails</response>
        /// <response code="401">If the requester is not authenticated</response>
        /// <response code="403">If the requester is not a super administrator</response>
        /// <response code="500">If an unexpected error occurs during registration</response>
        [Authorize(Roles = Roles.SuperAdmin)]
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _authService.Register(registerDto);

            if (response.IsAuthenticated is false)
                return BadRequest(response);

            SetTokenCookie(response.RefreshToken, response.RefreshTokenExpiresOn);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.Login(loginDto);

            if (response.IsAuthenticated is false)
                return BadRequest(response.Message);

            SetTokenCookie(response.RefreshToken, response.RefreshTokenExpiresOn);

            return Ok(response);
        }

        private void SetTokenCookie(string token, DateTime expirationDate)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expirationDate.ToLocalTime()
            };
            Response.Cookies.Append("RefreshToken", token, cookieOptions);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Invalid token");
            var response = await _authService.RefreshToken(refreshToken);
            if (response.IsAuthenticated is false)
                return BadRequest(response.Message);
            SetTokenCookie(response.RefreshToken, response.RefreshTokenExpiresOn);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RevokeToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto revokeTokenDto)
        {
            var token = revokeTokenDto.Token ?? Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid token");
            var response = await _authService.RevokeToken(token);
            if (response is false)
                return BadRequest("Invalid token");
            return Ok(response);
        }
    }
}