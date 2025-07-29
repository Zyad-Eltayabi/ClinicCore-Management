using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///     Controller for managing doctor information in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    /// <summary>
    ///     Initializes a new instance of the DoctorController.
    /// </summary>
    /// <param name="doctorService">The doctor service for handling business logic.</param>
    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    /// <summary>
    ///     Creates a new doctor record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Doctor must be at least 18 years old
    ///     - Email must be unique in the system
    ///     - Phone number must be in valid format
    ///     - All required fields must be provided
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "fullName": "Dr. Sarah Johnson",
    ///     "dateOfBirth": "1980-05-15",
    ///     "email": "s.johnson@clinic.com",
    ///     "phoneNumber": "+1234567890",
    ///     "address": "123 Medical Center Dr",
    ///     "specialization": "Cardiology",
    ///     "dateOfRegistration": "2023-01-10"
    ///     }
    ///     ```
    ///     ### Success Response Example (201 Created):
    ///     ```json
    ///     {
    ///     "id": 1,
    ///     "fullName": "Dr. Sarah Johnson",
    ///     "dateOfBirth": "1980-05-15T00:00:00",
    ///     "email": "s.johnson@clinic.com",
    ///     "phoneNumber": "+1234567890",
    ///     "address": "123 Medical Center Dr",
    ///     "specialization": "Cardiology",
    ///     "dateOfRegistration": "2023-01-10T00:00:00"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Email already exists"
    ///     ```
    ///     **Database Error (503):**
    ///     ```json
    ///     "Failed to add new doctor in database"
    ///     ```
    /// </remarks>
    /// <param name="doctorDto">The doctor data to create</param>
    /// <response code="201">Returns the newly created doctor</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks add doctor permission</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanAddDoctor)]
    [HttpPost]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<DoctorDto>> Add(DoctorDto doctorDto)
    {
        var result = await _doctorService.Add(doctorDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request.")
        };
    }

    /// <summary>
    ///     Updates an existing doctor record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Doctor must exist in the system
    ///     - Updated data must pass all validation rules
    ///     - Email uniqueness is not validated on update (can't be changed)
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "id": 1,
    ///     "fullName": "Dr. Sarah Johnson",
    ///     "dateOfBirth": "1980-05-15",
    ///     "email": "s.johnson@clinic.com",
    ///     "phoneNumber": "+1234567890",
    ///     "address": "123 Medical Center Dr, Suite 200",
    ///     "specialization": "Cardiology",
    ///     "dateOfRegistration": "2023-01-10"
    ///     }
    ///     ```
    ///     ### Success Response (204 NoContent):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Phone number must be between 10 and 15 digits"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Doctor is not found to update"
    ///     ```
    /// </remarks>
    /// <param name="doctorDto">The updated doctor data</param>
    /// <response code="204">Doctor successfully updated</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks edit doctor permission</response>
    /// <response code="404">If doctor doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanEditDoctor)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<DoctorDto>> Update(DoctorDto doctorDto)
    {
        var result = await _doctorService.Update(doctorDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request.")
        };
    }

    /// <summary>
    ///     Retrieves a specific doctor by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Doctor must exist in the system
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "id": 1,
    ///     "fullName": "Dr. Sarah Johnson",
    ///     "dateOfBirth": "1980-05-15T00:00:00",
    ///     "email": "s.johnson@clinic.com",
    ///     "phoneNumber": "+1234567890",
    ///     "address": "123 Medical Center Dr",
    ///     "specialization": "Cardiology",
    ///     "dateOfRegistration": "2023-01-10T00:00:00"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Invalid doctor id, the doctor with this id is not found"
    ///     ```
    /// </remarks>
    /// <param name="id">ID of the doctor to retrieve</param>
    /// <response code="200">Returns the requested doctor</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view doctors permission</response>
    /// <response code="404">If doctor doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewDoctors)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
    {
        var result = await _doctorService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request.")
        };
    }

    /// <summary>
    ///     Retrieves all doctors in the system.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Returns empty list if no doctors exist (status 200)
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     [
    ///     {
    ///     "id": 1,
    ///     "fullName": "Dr. Sarah Johnson",
    ///     "dateOfBirth": "1980-05-15T00:00:00",
    ///     "email": "s.johnson@clinic.com",
    ///     "phoneNumber": "+1234567890",
    ///     "address": "123 Medical Center Dr",
    ///     "specialization": "Cardiology",
    ///     "dateOfRegistration": "2023-01-10T00:00:00"
    ///     },
    ///     {
    ///     "id": 2,
    ///     "fullName": "Dr. Michael Chen",
    ///     "dateOfBirth": "1975-11-22T00:00:00",
    ///     "email": "m.chen@clinic.com",
    ///     "phoneNumber": "+1987654321",
    ///     "address": "456 Health Parkway",
    ///     "specialization": "Neurology",
    ///     "dateOfRegistration": "2022-06-05T00:00:00"
    ///     }
    ///     ]
    ///     ```
    ///     ### Empty Response:
    ///     ```json
    ///     []
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of doctors</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view doctors permission</response>
    /// <response code="404">If doctors don't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewDoctors)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
    {
        var result = await _doctorService.GetAll();
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => Ok(result.Message), // Return empty list rather than 404
            _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request.")
        };
    }

    /// <summary>
    ///     Deletes a doctor record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Doctor must exist in the system
    ///     - Doctor must not have any associated appointments
    ///     ### Sample Request:
    ///     `DELETE /api/doctor/1`
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "There is no doctor with id = 1"
    ///     ```
    ///     **Database Error (503):**
    ///     ```json
    ///     "Some error occurred during deleting in database"
    ///     ```
    /// </remarks>
    /// <param name="id">ID of the doctor to delete</param>
    /// <response code="200">Doctor successfully deleted</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks delete doctor permission</response>
    /// <response code="404">If doctor doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanDeleteDoctor)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _doctorService.Delete(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request.")
        };
    }
}