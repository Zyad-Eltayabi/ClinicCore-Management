using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;

namespace BusinessLayer.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private ValidationsResult ValidateAppointment(AppointmentDto appointmentDto)
    {
        var validator = new AppointmentValidator();
        var validationResult = validator.Validate(appointmentDto);
        if (!validationResult.IsValid)
        {
            string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true, "");
    }

    private async Task<ValidationsResult> ValidateEntitiesExist(int doctorId, int patientId)
    {
        // Check if doctor exists
        bool doctorExists = await _unitOfWork.Doctors.ExistsAsync(d => d.Id == doctorId);
        if (!doctorExists)
            return new ValidationsResult(false, $"Doctor with ID {doctorId} does not exist");

        // Check if patient exists
        bool patientExists = await _unitOfWork.Patients.ExistsAsync(p => p.Id == patientId);
        if (!patientExists)
            return new ValidationsResult(false, $"Patient with ID {patientId} does not exist");

        return new ValidationsResult(true);
    }


    public async Task<Result<AppointmentDto>> Add(AppointmentDto appointmentDto)
    {
        // Validate appointment data
        var validationResult = ValidateAppointment(appointmentDto);
        if (!validationResult.IsValid)
        {
            return Result<AppointmentDto>.Failure(validationResult.ErrorMessage,
                ServiceErrorType.ValidationError);
        }

        // Ensure doctor and patient exist
        var entitiesExistResult = await ValidateEntitiesExist(appointmentDto.DoctorID, appointmentDto.PatientID);
        if (!entitiesExistResult.IsValid)
        {
            return Result<AppointmentDto>.Failure(entitiesExistResult.ErrorMessage,
                ServiceErrorType.NotFound);
        }

        // Map, save, and return the appointment
        return await SaveAppointment(appointmentDto);
    }

    private async Task<Result<AppointmentDto>> SaveAppointment(AppointmentDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        appointment.MedicalRecord = _mapper.Map<MedicalRecord>(appointmentDto.MedicalRecordDto);

        await _unitOfWork.Appointments.Add(appointment);
        var saveSucceeded = await _unitOfWork.SaveChanges();

        if (!saveSucceeded)
        {
            return Result<AppointmentDto>.Failure("Failed to save the appointment to the database",
                ServiceErrorType.DatabaseError);
        }

        // Update the DTO with generated IDs from the saved entity
        _mapper.Map(appointment, appointmentDto);
        _mapper.Map(appointment.MedicalRecord, appointmentDto.MedicalRecordDto);

        return Result<AppointmentDto>.Success(appointmentDto);
    }

    private async Task<Result<RescheduleAppointmentDto>> ValidateRescheduleRequest(
        Appointment appointment,
        RescheduleAppointmentDto rescheduleAppointmentDto)
    {
        if (appointment is null)
            return Result<RescheduleAppointmentDto>.Failure(
                "Invalid appointment id, the appointment with this id is not found",
                ServiceErrorType.NotFound);

        // validate doctor 
        if (rescheduleAppointmentDto.DoctorID != 0)
        {
            var isDoctorExists = await _unitOfWork.Doctors.ExistsAsync(d => d.Id == rescheduleAppointmentDto.DoctorID);
            if (!isDoctorExists)
                return Result<RescheduleAppointmentDto>.Failure(
                    "Doctor with this id does not exist",
                    ServiceErrorType.NotFound);
        }


        if (rescheduleAppointmentDto.NewAppointmentDateTime < appointment.AppointmentDateTime)
            return Result<RescheduleAppointmentDto>.Failure(
                "New appointment date time must be greater than the current appointment date time",
                ServiceErrorType.ValidationError);

        return null;
    }

    public async Task<Result<RescheduleAppointmentDto>> Reschedule(RescheduleAppointmentDto rescheduleAppointmentDto)
    {
        var appointment = await _unitOfWork.Appointments.GetById(rescheduleAppointmentDto.AppointmentID);

        var validationResult = await ValidateRescheduleRequest(appointment, rescheduleAppointmentDto);
        if (validationResult != null)
            return validationResult;


        // update appointment with new reschedule appointment 
        appointment.AppointmentStatus = (int)AppointmentStatus.Rescheduled;
        appointment.AppointmentDateTime = rescheduleAppointmentDto.NewAppointmentDateTime;
        if (rescheduleAppointmentDto.DoctorID > 0)
            appointment.DoctorID = (int)rescheduleAppointmentDto.DoctorID;

        _unitOfWork.Appointments.Update(appointment);
        var result = await _unitOfWork.SaveChanges();

        return result
            ? Result<RescheduleAppointmentDto>.Success(rescheduleAppointmentDto)
            : Result<RescheduleAppointmentDto>.Failure("Failed to update the appointment",
                ServiceErrorType.DatabaseError);
    }
}