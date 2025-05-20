using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : Controller
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DoctorDto>> Add(DoctorDto doctorDto)
    {
        var result = await _doctorService.Add(doctorDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(503, result.Message),
            _ => StatusCode(500, "An error occurred while processing your request.")
        };
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DoctorDto>> Update(DoctorDto doctorDto)
    {
        var result = await _doctorService.Update(doctorDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(503, result.Message),
            _ => StatusCode(500, "An error occurred while processing your request.")
        };
    }

    [HttpGet,Route("{id}")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
    {
        var result = await _doctorService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            _ => StatusCode(500, "An error occurred while processing your request.")
        };
    }
}