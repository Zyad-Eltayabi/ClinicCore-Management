using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordController : ControllerBase
{
   private readonly IMedicalRecordService _medicalRecordService;

   public MedicalRecordController(IMedicalRecordService medicalRecordService)
   {
      _medicalRecordService = medicalRecordService;
   }

   [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> Get()
   {
      var medicalRecords = await _medicalRecordService.GetAll();
      return medicalRecords.ErrorType switch
      {
        ServiceErrorType.Success => Ok(medicalRecords.Data),
        _ => StatusCode((int)medicalRecords.ErrorType, medicalRecords.Message)
      };
   }
  
   [HttpGet("{id}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<MedicalRecordDto>> Get(int id)
   {
      var medicalRecord = await _medicalRecordService.GetById(id);
      return medicalRecord.ErrorType switch
      {
         ServiceErrorType.Success => Ok(medicalRecord.Data),
         _ => StatusCode((int)medicalRecord.ErrorType, medicalRecord.Message)
      };
   }
   
   [HttpPost]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<MedicalRecordDto>> Add([FromBody] MedicalRecordDto medicalRecordDto)
   {
      var result = await _medicalRecordService.Add(medicalRecordDto);
      return result.ErrorType switch
      {
         ServiceErrorType.Success => CreatedAtAction(nameof(Get), result.Data),
         _ => StatusCode((int)result.ErrorType, result.Message)
      };
   }
   
}