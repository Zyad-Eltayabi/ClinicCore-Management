using DomainLayer.BaseClasses;
using DomainLayer.DTOs;
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
        
    }
}
