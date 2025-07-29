using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///     Controller for managing medical records in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    /// <summary>
    ///     Initializes a new instance of the MedicalRecordController.
    /// </summary>
    /// <param name="medicalRecordService">The medical record service for handling business logic.</param>
    public MedicalRecordController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    /// <summary>
    ///     Retrieves all medical records.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only authorized users with view permissions can access
    ///     - Returns empty list if no records exist (status 200)
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     [
    ///     {
    ///     "medicalRecordID": 1,
    ///     "visitDescription": "Annual physical examination",
    ///     "diagnosis": "Healthy",
    ///     "additionalNotes": "Recommended follow-up in 6 months"
    ///     },
    ///     {
    ///     "medicalRecordID": 2,
    ///     "visitDescription": "Follow-up for hypertension",
    ///     "diagnosis": "Controlled hypertension",
    ///     "additionalNotes": "Adjust medication dosage"
    ///     }
    ///     ]
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "No medical records found"
    ///     ```
    ///     **Service Unavailable (503):**
    ///     ```json
    ///     "Database connection failed"
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of medical records</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If no records exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewMedicalRecords)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicalRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> Get()
    {
        var medicalRecords = await _medicalRecordService.GetAll();
        return medicalRecords.ErrorType switch
        {
            ServiceErrorType.Success => Ok(medicalRecords.Data),
            ServiceErrorType.NotFound => NotFound(medicalRecords.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable,
                medicalRecords.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Retrieves a specific medical record by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Record must exist in the system
    ///     - User must have view permissions
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "medicalRecordID": 1,
    ///     "visitDescription": "Annual physical examination",
    ///     "diagnosis": "Healthy",
    ///     "additionalNotes": "Recommended follow-up in 6 months"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Medical record not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the medical record to retrieve</param>
    /// <response code="200">Returns the requested medical record</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If record doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewMedicalRecords)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicalRecordDto>> Get(int id)
    {
        var medicalRecord = await _medicalRecordService.GetById(id);
        return medicalRecord.ErrorType switch
        {
            ServiceErrorType.Success => Ok(medicalRecord.Data),
            ServiceErrorType.NotFound => NotFound(medicalRecord.Message),
            ServiceErrorType.DatabaseError =>
                StatusCode(StatusCodes.Status503ServiceUnavailable, medicalRecord.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Creates a new medical record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Visit description is required (10-200 characters)
    ///     - Diagnosis and notes are optional (max 200 characters each)
    ///     - User must have create permissions
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "visitDescription": "Patient complaint of persistent headaches",
    ///     "diagnosis": "Migraine",
    ///     "additionalNotes": "Prescribed medication and recommended hydration"
    ///     }
    ///     ```
    ///     ### Success Response Example (201 Created):
    ///     ```json
    ///     {
    ///     "medicalRecordID": 3,
    ///     "visitDescription": "Patient complaint of persistent headaches",
    ///     "diagnosis": "Migraine",
    ///     "additionalNotes": "Prescribed medication and recommended hydration"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Visit description must be at least 10 characters long"
    ///     ```
    /// </remarks>
    /// <param name="medicalRecordDto">The medical record data to create</param>
    /// <response code="201">Returns the newly created medical record</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks create permissions</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanCreateMedicalRecord)]
    [HttpPost]
    [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MedicalRecordDto>> Add([FromBody] MedicalRecordDto medicalRecordDto)
    {
        var result = await _medicalRecordService.Add(medicalRecordDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Get), new { id = result.Data.MedicalRecordID },
                result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Updates an existing medical record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Record must exist
    ///     - All validation rules apply
    ///     - User must have edit permissions
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "medicalRecordID": 1,
    ///     "visitDescription": "Updated annual physical examination",
    ///     "diagnosis": "Healthy - no issues found",
    ///     "additionalNotes": "Patient should return in 1 year"
    ///     }
    ///     ```
    ///     ### Success Response (200 OK):
    ///     ```json
    ///     true
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Visit description is required"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Medical record not found"
    ///     ```
    /// </remarks>
    /// <param name="medicalRecordDto">The updated medical record data</param>
    /// <response code="200">Returns true if update was successful</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks edit permissions</response>
    /// <response code="404">If record doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanEditMedicalRecord)]
    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> Update([FromBody] MedicalRecordDto medicalRecordDto)
    {
        var result = await _medicalRecordService.Update(medicalRecordDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Deletes a medical record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Record must exist
    ///     - User must have delete permissions
    ///     ### Sample Request:
    ///     `DELETE /api/medicalrecord/1`
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Medical record not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the medical record to delete</param>
    /// <response code="200">If deletion was successful</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks delete permissions</response>
    /// <response code="404">If record doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanDeleteMedicalRecord)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _medicalRecordService.Delete(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }
}