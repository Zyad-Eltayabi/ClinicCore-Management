using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<IEnumerable<PaymentDto>>> GetAll();
    
    Task<Result<PaymentDto>> GetById(int id);
    
    Task<Result<bool>> Update(PaymentDto paymentDto);
    
}