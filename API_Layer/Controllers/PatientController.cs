using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> Get()
        {
            var patients = await _patientService.GetAll();
            return patients switch
            {
                null => NoContent(),
                var list => Ok(list)
            };
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDto>> Add([FromBody] PatientDto patientDTO)
        {
            var result = await _patientService.Add(patientDTO);

            return result.ErrorType switch
            {
                ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),

                ServiceErrorType.ValidationError => BadRequest(result.Message),

                ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable,
                result.Message),

                _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred while processing your request")
            };

        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDto>> GetById([FromRoute] int id)
        {
            var result = await _patientService.GetById(id);
            return result.ErrorType switch
            {
                ServiceErrorType.Success => Ok(result.Data),
                ServiceErrorType.NotFound => NotFound(result.Message),
                _ => StatusCode(500, "internal database error !!")
            };
        }

        [HttpDelete, Route("{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int patientId)
        {
            var result = await _patientService.Delete(patientId);
            return result.ErrorType switch
            {
                ServiceErrorType.Success => Ok(),
                ServiceErrorType.NotFound => NotFound(result.Message),
                ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable,
                result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError,
                result.Message)
            };
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(PatientDto patientDto)
        {
            var result = await _patientService.Update(patientDto);
            return result.ErrorType switch
            {
                ServiceErrorType.Success => NoContent(),
                ServiceErrorType.ValidationError => BadRequest(result.Message),
                ServiceErrorType.NotFound => NotFound(result.Message),
                ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable,
                result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError,
                result.Message)
            };
        }
    }
}
