using DomainLayer.Constants;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize(Policy = AuthorizationPolicies.CanViewPayments)]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> Get()
    {
        var payments = await _paymentService.GetAll();
        return payments.ErrorType switch
        {
            ServiceErrorType.Success => Ok(payments.Data),
            _ => StatusCode((int)payments.ErrorType, payments.Message)
        };
    }

    [Authorize(Policy = AuthorizationPolicies.CanViewPayments)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        var payment = await _paymentService.GetById(id);
        return payment.ErrorType switch
        {
            ServiceErrorType.Success => Ok(payment.Data),
            _ => StatusCode((int)payment.ErrorType, payment.Message)
        };
    }

    [Authorize(Policy = AuthorizationPolicies.CanProcessPayment)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> Update(PaymentDto paymentDto)
    {
        var result = await _paymentService.Update(paymentDto);
        return result.ErrorType switch
        {
            ServiceErrorType.Success => Ok(),
            _ => StatusCode((int)result.ErrorType, result.Message)
        };
    }
}