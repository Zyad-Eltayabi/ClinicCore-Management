using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///     Controller for managing payment records in the clinic system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    ///     Initializes a new instance of the PaymentController.
    /// </summary>
    /// <param name="paymentService">The payment service for handling business logic.</param>
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    ///     Retrieves all payment records.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Only authorized users with view permissions can access
    ///     - Returns empty list if no payments exist (status 200)
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     [
    ///     {
    ///     "paymentID": 1,
    ///     "paymentDate": "2023-05-15T10:30:00",
    ///     "amountPaid": 150.50,
    ///     "additionalNotes": "Insurance covered 80%"
    ///     },
    ///     {
    ///     "paymentID": 2,
    ///     "paymentDate": "2023-06-20T14:15:00",
    ///     "amountPaid": 75.25,
    ///     "additionalNotes": "Copayment for visit"
    ///     }
    ///     ]
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "No payments found"
    ///     ```
    ///     **Service Unavailable (503):**
    ///     ```json
    ///     "Database connection failed"
    ///     ```
    /// </remarks>
    /// <response code="200">Returns list of payments</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If no payments exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPayments)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> Get()
    {
        var payments = await _paymentService.GetAll();
        return payments.ErrorType switch
        {
            ServiceErrorType.Success => Ok(payments.Data),
            ServiceErrorType.NotFound => NotFound(payments.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, payments.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Retrieves a specific payment by ID.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Payment must exist in the system
    ///     - User must have view permissions
    ///     ### Success Response Example (200 OK):
    ///     ```json
    ///     {
    ///     "paymentID": 1,
    ///     "paymentDate": "2023-05-15T10:30:00",
    ///     "amountPaid": 150.50,
    ///     "additionalNotes": "Insurance covered 80%"
    ///     }
    ///     ```
    ///     ### Error Response Examples:
    ///     **Not Found (404):**
    ///     ```json
    ///     "Payment with ID 1 not found"
    ///     ```
    /// </remarks>
    /// <param name="id">The ID of the payment to retrieve</param>
    /// <response code="200">Returns the requested payment</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks view permissions</response>
    /// <response code="404">If payment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    [Authorize(Policy = AuthorizationPolicies.CanViewPayments)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        var payment = await _paymentService.GetById(id);
        return payment.ErrorType switch
        {
            ServiceErrorType.Success => Ok(payment.Data),
            ServiceErrorType.NotFound => NotFound(payment.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, payment.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }

    /// <summary>
    ///     Updates an existing payment record.
    /// </summary>
    /// <remarks>
    ///     ### Business Rules:
    ///     - Payment must exist
    ///     - Payment date cannot be in the future
    ///     - Amount must be greater than 0
    ///     - Additional notes cannot exceed 200 characters
    ///     - User must have process payment permissions
    ///     ### Sample Request:
    ///     ```json
    ///     {
    ///     "paymentID": 1,
    ///     "paymentDate": "2023-05-15T10:30:00",
    ///     "amountPaid": 175.50,
    ///     "additionalNotes": "Updated insurance coverage - now 85%"
    ///     }
    ///     ```
    ///     ### Success Response (200 OK):
    ///     Empty response body
    ///     ### Error Response Examples:
    ///     **Validation Error (400):**
    ///     ```json
    ///     "Amount paid must be greater than 0"
    ///     ```
    ///     **Not Found (404):**
    ///     ```json
    ///     "Payment with ID 1 not found"
    ///     ```
    /// </remarks>
    /// <param name="paymentDto">The updated payment data</param>
    /// <response code="200">Payment successfully updated</response>
    /// <response code="400">If request data is invalid</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user lacks process permissions</response>
    /// <response code="404">If payment doesn't exist</response>
    /// <response code="500">If internal server error occurs</response>
    /// <response code="503">If service is temporarily unavailable</response>
    [Authorize(Policy = AuthorizationPolicies.CanProcessPayment)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> Update(PaymentDto paymentDto)
    {
        var result = await _paymentService.Update(paymentDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            ServiceErrorType.ValidationError => BadRequest(result.Message),
            ServiceErrorType.NotFound => NotFound(result.Message),
            ServiceErrorType.DatabaseError => StatusCode(StatusCodes.Status503ServiceUnavailable, result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
    }
}