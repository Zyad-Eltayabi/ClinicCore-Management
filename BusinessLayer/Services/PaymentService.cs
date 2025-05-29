using AutoMapper;
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

    public Task<Result<bool>> Update(PaymentDto paymentDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}