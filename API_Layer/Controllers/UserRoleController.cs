using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly IUserRoleService _userRoleService;

    public UserRoleController(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    [HttpGet("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole([FromQuery] UserRoleDto userRoleDto)
    {
        var result = await _userRoleService.AddUserToRole(userRoleDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
}