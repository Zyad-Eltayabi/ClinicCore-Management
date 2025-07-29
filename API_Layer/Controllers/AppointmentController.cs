using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

/// <summary>
///     Controller for managing patient appointments in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
[Consumes("application/json")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    ///     Creates a new appointment.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Appointment date must be in the future
    ///     - Doctor and patient must exist in the system
    ///     - Medical record must be valid if provided
    ///     ### AppointmentStatus
    ///     Pending = 1,
    ///     Rescheduled = 2,
    ///     Canceled = 3,
    ///     Completed = 4,
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "appointmentDateTime": "2023-12-15T10:00:00",
    ///     "appointmentStatus": 1,
    ///     "patientID": 123,
    ///     "doctorID": 456,
    ///     "medicalRecordDto": {
    ///     "diagnosis": "Annual checkup",
    ///     "treatment": "Physical examination"
    ///     }
    ///     }
    ///     ```
    ///     ### Success Response Example (201 Created):
    ///     ```json
    ///     {
    ///     "appointmentID": 789,
    ///     "appointmentDateTime": "2023-12-15T10:00:00",
    ///     "appointmentStatus": 1,
    ///     "patientID": 123,
    ///     "doctorID": 456,
    ///     "medicalRecordID": 101,
    ///     "medicalRecordDto": {
    ///     "medicalRecordID": 101,
    ///     "diagnosis": "Annual checkup",
    ///     }
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Appointment date must be in the future"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Doctor with ID 456 does not exist"
    ///     ```
    /// </remarks>
    /// <param name="AppointmentDto">The appointment data to create</param>
    /// <response code="201">Returns the newly created appointment</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks create appointment permission</response>
    /// <response code="404">If referenced entities don't exist</response>
    /// <response code="500">If there was an internal server error</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanCreateAppointment)]
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    [Authorize(Policy = AuthorizationPolicies.CanCreateAppointment)]
    public async Task<ActionResult<AppointmentDto>> Add(AppointmentDto appointmentDto)
    {
        var result = await _appointmentService.Add(appointmentDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => CreatedAtAction(nameof(Add), result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }

    /// <summary>
    ///     Reschedules an existing appointment.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - New date must be in the future
    ///     - New date must be after current appointment date
    ///     - Doctor must exist if changing doctor
    ///     - Appointment must not be completed or canceled
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "appointmentID": 789,
    ///     "newAppointmentDateTime": "2023-12-20T14:00:00",
    ///     "doctorID": 456
    ///     }
    ///     ```
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "New appointment date must be after current date"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Appointment not found"
    ///     ```
    /// </remarks>
    /// <param name="rescheduleAppointmentDto">Reschedule data</param>
    /// <response code="200">Appointment successfully rescheduled</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks reschedule permission</response>
    /// <response code="404">If appointment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">Database service unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanRescheduleAppointment)]
    [HttpPut("reschedule-appointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDto rescheduleAppointmentDto)
    {
        var result = await _appointmentService.Reschedule(rescheduleAppointmentDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Cancels an existing appointment.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Appointment must exist
    ///     - Appointment must not be already completed
    ///     ### Sample Request:
    ///     `PUT /api/appointment/cancel-appointment/789`
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Appointment is already completed and cannot be canceled"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Appointment not found"
    ///     ```
    /// </remarks>
    /// <param name="appointmentId">ID of appointment to cancel</param>
    /// <response code="200">Appointment successfully canceled</response>
    /// <response code="400">If appointment cannot be canceled</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks cancel permission</response>
    /// <response code="404">If appointment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">Database service unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanCancelAppointment)]
    [HttpPut("cancel-appointment/{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelAppointment([FromRoute] int appointmentId)
    {
        var result = await _appointmentService.Cancel(appointmentId);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            _ => StatusCode((int)result.ErrorType, result.Message)
            /*ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")*/
        };
    }

    /// <summary>
    ///     Marks an appointment as completed with payment and prescription details.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Appointment must exist and be in a completable state
    ///     - Payment amount must be positive
    ///     - Prescription dates must be valid
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "appointmentID": 789,
    ///     "completePrescriptionDto": {
    ///     "medicationName": "Ibuprofen",
    ///     "dosage": "200mg",
    ///     "frequencyPerDay": 2,
    ///     "startDate": "2023-12-15",
    ///     "endDate": "2023-12-22",
    ///     "specialInstructions": "Take with food"
    ///     },
    ///     "completePaymentDto": {
    ///     "paymentDate": "2023-12-15T11:00:00",
    ///     "amountPaid": 150.00,
    ///     "additionalNotes": "Insurance covered"
    ///     }
    ///     }
    ///     ```
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Appointment does not exist."
    ///     "Appointment status is not valid to complete."
    ///     "Medication name is required"
    ///     "Medication name is required."
    ///     "Dosage is required."
    ///     "Frequency per day is required."
    ///     "Payment date is required."
    ///     "Amount paid must be greater than 0."
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Appointment not found"
    ///     ```
    /// </remarks>
    /// <param name="completeAppointment">Completion data</param>
    /// <response code="200">Appointment successfully completed</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks complete permission</response>
    /// <response code="404">If appointment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanCompleteAppointment)]
    [HttpPut("complete-appointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CompleteAppointment(CompleteAppointmentDto completeAppointment)
    {
        var result = await _appointmentService.Complete(completeAppointment);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }

    /// <summary>
    ///     Retrieves all appointments in the system.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only returns appointments visible to the current user
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "appointmentID": 789,
    ///     "appointmentDateTime": "2023-12-15T10:00:00",
    ///     "appointmentStatus": 1,
    ///     "patientID": 123,
    ///     "doctorID": 456,
    ///     "medicalRecordID": 101,
    ///     "medicalRecordDto": {
    ///     "medicalRecordID": 101,
    ///     "diagnosis": "Annual checkup",
    ///     "visitDescription": "string",
    ///     "additionalNotes": "string"
    ///     }
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "No appointments found"
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of appointments</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permission</response>
    /// <response code="404">If no appointments exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewAppointments)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> Get()
    {
        var appointments = await _appointmentService.GetAll();
        return appointments.ErrorType switch
        {
            ServiceErrorType.Success => Ok(appointments.Data),
            _ => StatusCode((int)appointments.ErrorType, appointments.Message)
        };
    }

    /// <summary>
    ///     Retrieves a specific appointment by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - User must have permission to view the specific appointment
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "appointmentID": 789,
    ///     "appointmentDateTime": "2023-12-15T10:00:00",
    ///     "appointmentStatus": 1,
    ///     "patientID": 123,
    ///     "doctorID": 456,
    ///     "medicalRecordID": 101,
    ///     "medicalRecordDto": {
    ///     "medicalRecordID": 101,
    ///     "diagnosis": "Annual checkup",
    ///     "visitDescription": "string",
    ///     "additionalNotes": "string"
    ///     }
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Appointment not found"
    ///     ```
    /// </remarks>
    /// <param name="id">ID of the appointment to retrieve</param>
    /// <response code="200">Returns the requested appointment</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permission</response>
    /// <response code="404">If appointment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewAppointments)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById([FromRoute] int id)
    {
        var result = await _appointmentService.GetById(id);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(result.Data),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
}