using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Policy = AuthorizationPolicies.CanCreatePrescription)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Add(
        [FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Add(prescriptionDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }

    [Authorize(Policy = AuthorizationPolicies.CanEditPrescription)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Update(
        [FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Update(prescriptionDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }

    [Authorize(Policy = AuthorizationPolicies.CanViewPrescriptions)]
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

    [Authorize(Policy = AuthorizationPolicies.CanViewPrescriptions)]
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

    [Authorize(Policy = AuthorizationPolicies.CanDeletePrescription)]
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