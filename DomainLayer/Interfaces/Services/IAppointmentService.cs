using DomainLayer.DTOs;
using DomainLayer.Helpers;

namespace DomainLayer.Interfaces.Services;

public interface IAppointmentService
{
    Task<Result<AppointmentDto>> Add(AppointmentDto appointmentDto);
    Task<Result<RescheduleAppointmentDto>> Reschedule(RescheduleAppointmentDto rescheduleAppointmentDto);
    
    Task<Result<AppointmentDto>> Cancel(int appointmentId);
    
    Task<Result<CompleteAppointmentDto>> Complete(CompleteAppointmentDto completeAppointmentDto);

    Task<Result<IEnumerable<AppointmentDto>>> GetAll();
}