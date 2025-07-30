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

        /// <summary>
        ///     Authenticates a user and generates access and refresh tokens.
        /// </summary>
        /// <param name="loginDto">
        ///     The login credentials containing:
        ///     - Email (required): User's registered email address
        ///     - Password (required): User's password
        /// </param>
        /// <remarks>
        ///     ## This endpoint authenticates users and provides JWT tokens for API access.
        ///     ### Validation Rules:
        ///     - Email: Required, must be registered in system
        ///     - Password: Required, must match stored password
        ///     ### Sample request:
        ///     POST /api/auth/login
        ///     ``` json
        ///     {
        ///     "email": "john.doe@example.com",
        ///     "password": "MyP@ssw0rd"
        ///     }
        ///     ```
        ///     ### Sample success response:
        ///     ``` json
        ///     {
        ///     "message": "Login successful",
        ///     "token": "eyJhbGci...[JWT token]",
        ///     "isAuthenticated": true,
        ///     "userName": "johndoe",
        ///     "email": "john.doe@example.com",
        ///     "roles": ["User"],
        ///     "refreshTokenExpiresOn": "2024-01-01T00:00:00"
        ///     }
        ///     ```
        /// </remarks>
        /// <response code="200">Returns the authentication response with JWT token when login is successful</response>
        /// <response code="400">If the login credentials are invalid</response>
        /// <response code="500">If an unexpected error occurs during authentication</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.Login(loginDto);

            if (response.IsAuthenticated is false)
                return BadRequest(response);

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

        /// <summary>
        ///     Refreshes an expired JWT access token using a valid refresh token.
        /// </summary>
        /// <remarks>
        ///     ## This endpoint allows users to obtain a new access token using their refresh token.
        ///     ### Business Rules:
        ///     - Refresh token must be valid and not expired
        ///     - Refresh token must not be revoked
        ///     - A new refresh token is generated with each use
        ///     ### Sample success response:
        ///     ``` json
        ///     {
        ///     "message": "Token refreshed successfully",
        ///     "token": "eyJhbGci...[JWT token]",
        ///     "isAuthenticated": true,
        ///     "userName": "johndoe",
        ///     "email": "john.doe@example.com",
        ///     "roles": ["User"],
        ///     "refreshTokenExpiresOn": "2024-01-01T00:00:00"
        ///     }
        ///     ```
        /// </remarks>
        /// <response code="200">Returns new access token and refresh token when successful</response>
        /// <response code="400">If the refresh token is invalid, expired or revoked</response>
        /// <response code="500">If an unexpected error occurs during token refresh</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        ///     Revokes a refresh token, preventing it from being used for future token refreshes.
        /// </summary>
        /// <param name="revokeTokenDto">
        ///     The DTO containing the refresh token to revoke. If not provided, attempts to revoke the refresh token from cookies.
        /// </param>
        /// <remarks>
        ///     ## This endpoint invalidates refresh tokens for security purposes.
        ///     ### Business Rules:
        ///     - Token must be valid and not already revoked
        ///     - Token must belong to an existing user
        ///     - Once revoked, token cannot be used again
        ///     ### Sample request:
        ///     POST /api/auth/revoke-token
        ///     ``` json
        ///     {
        ///     "token": "base64EncodedRefreshToken"
        ///     }
        ///     ```
        ///     ### Sample success response:
        ///     ``` json
        ///     true
        ///     ```
        /// </remarks>
        /// <response code="200">Returns true when token is successfully revoked</response>
        /// <response code="400">If the token is invalid, expired or already revoked</response>
        /// <response code="500">If an unexpected error occurs during token revocation</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("revoke-token")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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