using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///     Controller for managing prescription records in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    /// <summary>
    ///     Initializes a new instance of the PrescriptionController.
    /// </summary>
    /// <param name="prescriptionService">The prescription service for handling business logic.</param>
    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    /// <summary>
    ///     Creates a new prescription record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Medication name is required (max 100 chars)
    ///     - Dosage is required (max 50 chars)
    ///     - Frequency must be greater than 0
    ///     - Start date must be in the future
    ///     - End date must be after start date
    ///     - Special instructions are optional (max 200 chars)
    ///     - Medical record must exist
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen",
    ///     "dosage": "200mg",
    ///     "frequencyPerDay": 2,
    ///     "startDate": "2023-12-01",
    ///     "endDate": "2023-12-15",
    ///     "specialInstructions": "Take with food"
    ///     }
    ///     ```
    ///     ### Success Response Example (201 Created):
    ///     ```json
    ///     {
    ///     "prescriptionID": 1,
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen",
    ///     "dosage": "200mg",
    ///     "frequencyPerDay": 2,
    ///     "startDate": "2023-12-01T00:00:00",
    ///     "endDate": "2023-12-15T00:00:00",
    ///     "specialInstructions": "Take with food"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Start date must be in the future"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Medical record not found"
    ///     ```
    /// </remarks>
    /// <param name="prescriptionDto">The prescription data to create</param>
    /// <response code="201">Returns the newly created prescription</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks create permissions</response>
    /// <response code="404">If medical record doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If the service is unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanCreatePrescription)]
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrUpdatePrescriptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Add(
        [FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Add(prescriptionDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Updates an existing prescription record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Prescription must exist
    ///     - All validation rules apply
    ///     - Medical record must exist
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "prescriptionID": 1,
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen Updated",
    ///     "dosage": "400mg",
    ///     "frequencyPerDay": 3,
    ///     "startDate": "2023-12-01",
    ///     "endDate": "2023-12-20",
    ///     "specialInstructions": "Take with food, twice daily"
    ///     }
    ///     ```
    ///     ### Success Response (200 OK):
    ///     ```json
    ///     {
    ///     "prescriptionID": 1,
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen Updated",
    ///     "dosage": "400mg",
    ///     "frequencyPerDay": 3,
    ///     "startDate": "2023-12-01T00:00:00",
    ///     "endDate": "2023-12-20T00:00:00",
    ///     "specialInstructions": "Take with food, twice daily"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "End date must be after start date"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Prescription not found"
    ///     ```
    /// </remarks>
    /// <param name="prescriptionDto">The updated prescription data</param>
    /// <response code="200">Returns the updated prescription</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks edit permissions</response>
    /// <response code="404">If prescription doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If the service is unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanEditPrescription)]
    [HttpPut]
    [ProducesResponseType(typeof(CreateOrUpdatePrescriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> Update(
        [FromBody] CreateOrUpdatePrescriptionDto prescriptionDto)
    {
        var result = await _prescriptionService.Update(prescriptionDto);
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
    ///     Retrieves all prescription records.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only authorized users with view permissions can access
    ///     - Returns empty list if no prescriptions exist (status 200)
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     [
    ///     {
    ///     "prescriptionID": 1,
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen",
    ///     "dosage": "200mg",
    ///     "frequencyPerDay": 2,
    ///     "startDate": "2023-12-01T00:00:00",
    ///     "endDate": "2023-12-15T00:00:00",
    ///     "specialInstructions": "Take with food"
    ///     },
    ///     {
    ///     "prescriptionID": 2,
    ///     "medicalRecordId": 2,
    ///     "medicationName": "Amoxicillin",
    ///     "dosage": "500mg",
    ///     "frequencyPerDay": 3,
    ///     "startDate": "2023-12-05T00:00:00",
    ///     "endDate": "2023-12-20T00:00:00",
    ///     "specialInstructions": "Take with water"
    ///     }
    ///     ]
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "No prescriptions found"
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of prescriptions</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If no prescriptions exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPrescriptions)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CreateOrUpdatePrescriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<IEnumerable<CreateOrUpdatePrescriptionDto>>>> GetAll()
    {
        var result = await _prescriptionService.GetAll();
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Retrieves a specific prescription by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Prescription must exist
    ///     - User must have view permissions
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "prescriptionID": 1,
    ///     "medicalRecordId": 1,
    ///     "medicationName": "Ibuprofen",
    ///     "dosage": "200mg",
    ///     "frequencyPerDay": 2,
    ///     "startDate": "2023-12-01T00:00:00",
    ///     "endDate": "2023-12-15T00:00:00",
    ///     "specialInstructions": "Take with food"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Prescription not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the prescription to retrieve</param>
    /// <response code="200">Returns the requested prescription</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If prescription doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPrescriptions)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CreateOrUpdatePrescriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<CreateOrUpdatePrescriptionDto>>> GetById(int id)
    {
        var result = await _prescriptionService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Deletes a prescription record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Prescription must exist
    ///     - User must have delete permissions
    ///     ### Sample Request:
    ///     `DELETE /api/prescription/1`
    ///     ### Success Response (200 OK):
    ///     ```json
    ///     true
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Prescription not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the prescription to delete</param>
    /// <response code="200">If deletion was successful</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks delete permissions</response>
    /// <response code="404">If prescription doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If database error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanDeletePrescription)]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<bool>>> Delete(int id)
    {
        var result = await _prescriptionService.Delete(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }
}