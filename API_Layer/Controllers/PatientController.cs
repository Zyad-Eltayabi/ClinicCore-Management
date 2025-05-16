using System.Threading.Tasks;
using DomainLayer.DTOs;
using DomainLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<IEnumerable<PatientDTO>>> Get()
        {
            var patients = await _patientService.GetAll();
            return patients switch
            {
                null => NoContent(),
                var list => Ok(list)
            };
        }

        [HttpPost]
        public async Task<ActionResult<PatientDTO>> Add(PatientDTO patientDTO)
        {
            var result = await _patientService.Add(patientDTO);
            return result.IsSuccess switch
            {
                true => Ok(result),
                _ => BadRequest(result.Message)
            };
        }

    }
}
