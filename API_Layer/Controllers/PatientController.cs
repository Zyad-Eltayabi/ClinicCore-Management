using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///     Controller for managing patient information in the clinic system.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    /// <summary>
    ///     Initializes a new instance of the PatientController.
    /// </summary>
    /// <param name="patientService">The patient service for handling business logic.</param>
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    ///     Retrieves all patients in the system.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only authorized users with view permissions can access
    ///     - Returns empty list (204) if no patients exist
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     [
    ///     {
    ///     "id": 1,
    ///     "fullName": "John Doe",
    ///     "dateOfBirth": "1985-05-15T00:00:00",
    ///     "gender": true,
    ///     "phoneNumber": "+1234567890",
    ///     "email": "john.doe@example.com",
    ///     "address": "123 Main St",
    ///     "dateOfRegistration": "2023-01-10T00:00:00"
    ///     },
    ///     {
    ///     "id": 2,
    ///     "fullName": "Jane Smith",
    ///     "dateOfBirth": "1990-08-22T00:00:00",
    ///     "gender": false,
    ///     "phoneNumber": "+1987654321",
    ///     "email": "jane.smith@example.com",
    ///     "address": "456 Oak Ave",
    ///     "dateOfRegistration": "2023-02-05T00:00:00"
    ///     }
    ///     ]
    ///     ```
    ///     ### Empty Response (204 NoContent):
    ///     Empty response body
    /// </remarks>
    /// <response code="200">Returns list of patients</response>
    /// <response code="204">If no patients exist</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="500">If an unexpected error occurs</response>
    /// <response code="503">If the service is unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPatients)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> Get()
    {
        var patients = await _patientService.GetAll();
        return patients switch
        {
            null => NoContent(),
            var list => Ok(list)
        };
    }

    /// <summary>
    ///     Creates a new patient record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Patient must be at least 18 years old
    ///     - Email must be unique in the system
    ///     - Phone number must be in valid format (10-15 digits)
    ///     - All required fields must be provided
    ///     - New patient should not have an ID (will be generated)
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "id" : "0",
    ///     "fullName": "Michael Johnson",
    ///     "dateOfBirth": "1988-07-30",
    ///     "gender": true,
    ///     "phoneNumber": "+1122334455",
    ///     "email": "michael.j@example.com",
    ///     "address": "789 Pine Rd",
    ///     "dateOfRegistration": "2023-03-15"
    ///     }
    ///     ```
    ///     ### Success Response Example (201 Created):
    ///     ```json
    ///     {
    ///     "id": 3,
    ///     "fullName": "Michael Johnson",
    ///     "dateOfBirth": "1988-07-30T00:00:00",
    ///     "gender": true,
    ///     "phoneNumber": "+1122334455",
    ///     "email": "michael.j@example.com",
    ///     "address": "789 Pine Rd",
    ///     "dateOfRegistration": "2023-03-15T00:00:00"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Patient must be at least 18 years old"
    ///     ```
    ///     **Database Error (503):**
    ///     ```json
    ///     "Failed to add new patient in database"
    ///     ```
    /// </remarks>
    /// <param name="patientDTO">The patient data to create</param>
    /// <response code="201">Returns the newly created patient</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks add permissions</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanAddPatient)]
    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatientDto>> Add([FromBody] PatientDto patientDTO)
    {
        var result = await _patientService.Add(patientDTO);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Retrieves a specific patient by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Patient must exist in the system
    ///     - User must have view permissions
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "id": 1,
    ///     "fullName": "John Doe",
    ///     "dateOfBirth": "1985-05-15T00:00:00",
    ///     "gender": true,
    ///     "phoneNumber": "+1234567890",
    ///     "email": "john.doe@example.com",
    ///     "address": "123 Main St",
    ///     "dateOfRegistration": "2023-01-10T00:00:00"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Invalid patient id, the patient with this id is not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the patient to retrieve</param>
    /// <response code="200">Returns the requested patient</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If patient doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPatients)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetById([FromRoute] int id)
    {
        var result = await _patientService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Deletes a patient record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Patient must exist
    ///     - User must have delete permissions
    ///     ### Sample Request:
    ///     `DELETE /api/patient/1`
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Invalid patient ID, there was no patient with id = 1"
    ///     ```
    ///     **Database Error (503):**
    ///     ```json
    ///     "Some error occurred during deleting in database"
    ///     ```
    /// </remarks>
    /// <param name="patientId">The ID of the patient to delete</param>
    /// <response code="200">If deletion was successful</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks delete permissions</response>
    /// <response code="404">If patient doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanDeletePatient)]
    [HttpDelete("{patientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int patientId)
    {
        var result = await _patientService.Delete(patientId);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Updates an existing patient record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Patient must exist
    ///     - All validation rules apply
    ///     - User must have edit permissions
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "id": 1,
    ///     "fullName": "John Doe Updated",
    ///     "dateOfBirth": "1985-05-15",
    ///     "gender": true,
    ///     "phoneNumber": "+1234567890",
    ///     "email": "john.doe.updated@example.com",
    ///     "address": "123 Main St, Apt 4B",
    ///     "dateOfRegistration": "2023-01-10"
    ///     }
    ///     ```
    ///     ### Success Response (204 NoContent):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Email is required"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Patient is not found to update"
    ///     ```
    /// </remarks>
    /// <param name="patientDto">The updated patient data</param>
    /// <response code="204">Patient successfully updated</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks edit permissions</response>
    /// <response code="404">If patient doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanEditPatient)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(PatientDto patientDto)
    {
        var result = await _patientService.Update(patientDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => NoContent(),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }
}