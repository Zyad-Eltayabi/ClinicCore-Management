using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;

namespace BusinessLayer.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PaymentDto>>> GetAll()
    {
        var payments =await _unitOfWork.Payments.GetAll();
        if (payments == null)
            return Result<IEnumerable<PaymentDto>>.Failure("No payments found", ServiceErrorType.NotFound);
        
        // Map payment to paymentDto
        var paymentsDto = _mapper.Map<IEnumerable<PaymentDto>>(payments);
        return Result<IEnumerable<PaymentDto>>.Success(paymentsDto);
    }

    public async Task<Result<PaymentDto>> GetById(int id)
    {
        var payment = await _unitOfWork.Payments.GetById(id);
    
        if (payment == null)
            return Result<PaymentDto>.Failure($"Payment with ID {id} not found", ServiceErrorType.NotFound);
    
        // Map payment to paymentDto
        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return Result<PaymentDto>.Success(paymentDto);
    }

    public async Task<Result<bool>> Update(PaymentDto paymentDto)
    {
        // Validate the payment DTO
        var validations = await ValidatePaymentDto(paymentDto);
        if (!validations.IsValid)
            return Result<bool>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // Get the existing payment
        var existingPayment = await _unitOfWork.Payments.GetById(paymentDto.PaymentID);
        
        if (existingPayment == null)
            return Result<bool>.Failure($"Payment with ID {paymentDto.PaymentID} not found", ServiceErrorType.NotFound);

        // Map the updated data to the existing entity
        _mapper.Map(paymentDto, existingPayment);
        
        // Update the payment
        _unitOfWork.Payments.Update(existingPayment);
        
        // Save changes to the database
        bool saveResult = await _unitOfWork.SaveChanges();

        if (!saveResult)
            return Result<bool>.Failure("Failed to update payment in database", ServiceErrorType.DatabaseError);

        return Result<bool>.Success();
    }

    private async Task<ValidationsResult> ValidatePaymentDto(PaymentDto paymentDto)
    {
        var paymentValidator = new PaymentValidator();
        var validations =  paymentValidator.Validate(paymentDto);
        if (!validations.IsValid)
        {
            string message = string.Join("; ", validations.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true);
    }
}