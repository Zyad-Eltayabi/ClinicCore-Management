using Microsoft.AspNetCore.Mvc;
using DomainLayer.DTOs;
using DomainLayer.Interfaces.Services;
using DomainLayer.Helpers;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Add([FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Add(prescriptionDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            _ => StatusCode((int)result.ErrorType,result.Message)
        };
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Update([FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Update(prescriptionDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<IEnumerable<CreateOrUpdatePrescriptionDto>>>> GetAll()
    {
        var result = await _prescriptionService.GetAll();
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> GetById(int id)
    {
        var result = await _prescriptionService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> Delete(int id)
    {
        var result = await _prescriptionService.Delete(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
    
}