using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private ILogger<AppointmentService> _logger;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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


    private ValidationsResult ValidateCancelRequest(Appointment appointment)
    {
        if (appointment is null)
            return new ValidationsResult(false,
                "Invalid appointment id, the appointment with this id is not found");

        // check the current appointment status
        if (appointment.AppointmentStatus == (int)AppointmentStatus.Completed)
            return new ValidationsResult(false,
                "The appointment is already completed, it cannot be canceled");

        return new ValidationsResult(true);
    }

    public async Task<Result<AppointmentDto>> Cancel(int appointmentId)
    {
        // check if appointment exists
        var appointment = await _unitOfWork.Appointments.GetById(appointmentId);

        var validationResult = ValidateCancelRequest(appointment);
        if (!validationResult.IsValid)
            return Result<AppointmentDto>.Failure(validationResult.ErrorMessage, ServiceErrorType.ValidationError);

        // update appointment status
        appointment.AppointmentStatus = (int)AppointmentStatus.Canceled;
        _unitOfWork.Appointments.Update(appointment);
        var result = await _unitOfWork.SaveChanges();

        return result
            ? Result<AppointmentDto>.Success()
            : Result<AppointmentDto>.Failure("Failed to update the appointment",
                ServiceErrorType.DatabaseError);
    }


    private async Task<ValidationsResult> ValidateCompleteRequest(CompleteAppointmentDto completeAppointmentDto)
    {
        var completeAppointmentValidator = new CompleteAppointmentValidator(_unitOfWork);
        var validations = await completeAppointmentValidator.ValidateAsync(completeAppointmentDto);
        if (!validations.IsValid)
        {
            string message = string.Join("; ", validations.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true);
    }

    private async Task SaveNewPrescription(CompletePrescriptionDto prescriptionDto, int medicalRecordId)
    {
        // map prescriptionDto with a Prescription model
        var prescription = _mapper.Map<Prescription>(prescriptionDto);
        prescription.MedicalRecordId = medicalRecordId;
        await _unitOfWork.Prescriptions.Add(prescription);
        await _unitOfWork.SaveChanges();
    }

    private async Task<int> SaveNewPayment(CompletePaymentDto paymentDto)
    {
        // map paymentDto with a Payment model
        var payment = _mapper.Map<Payment>(paymentDto);
        await _unitOfWork.Payments.Add(payment);
        await _unitOfWork.SaveChanges();
        return payment.PaymentID;
    }

    public async Task<Result<CompleteAppointmentDto>> Complete(CompleteAppointmentDto completeAppointmentDto)
    {
        // validate completeAppointmentDto object
        var validations = await ValidateCompleteRequest(completeAppointmentDto);
        if (!validations.IsValid)
            return Result<CompleteAppointmentDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // get appointment
        var appointment = await _unitOfWork.Appointments.GetById(completeAppointmentDto.AppointmentID);
        try
        {
            await _unitOfWork.CreateTransaction();
            // save new prescription
            await SaveNewPrescription(completeAppointmentDto.CompletePrescriptionDto, appointment.MedicalRecordID);
            // save new payment and get new payment Id
            var paymentId = await SaveNewPayment(completeAppointmentDto.CompletePaymentDto);
            // update appointment status
            appointment.AppointmentStatus = (int)AppointmentStatus.Completed;
            appointment.PaymentID = paymentId;
            _unitOfWork.Appointments.Update(appointment);
            var result = await _unitOfWork.SaveChanges();
            await _unitOfWork.Commit();
            return Result<CompleteAppointmentDto>.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to complete appointment {AppointmentId}",
                completeAppointmentDto.AppointmentID, ConsoleColor.DarkRed);
            await _unitOfWork.Rollback();
            return Result<CompleteAppointmentDto>.Failure("Failed to complete the appointment",
                ServiceErrorType.DatabaseError);
        }
    }

    public async Task<Result<IEnumerable<AppointmentDto>>> GetAll()
    {
        var appointments = await _unitOfWork.Appointments.GetAll();

        if (appointments is null)
            return Result<IEnumerable<AppointmentDto>>.Failure("There were no appointments found",
                ServiceErrorType.NotFound);

        var appointmentsDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        
        // map medical medical record dto in each appointmentsDtos items  
        foreach (var item in appointmentsDtos)
        {
            var medicalRecord = await _unitOfWork.MedicalRecords.GetById((int)item.MedicalRecordID);
            item.MedicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
        }
        return Result<IEnumerable<AppointmentDto>>.Success(appointmentsDtos);
    }

    public async Task<Result<AppointmentDto>> GetById(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetById(id);
        if (appointment is null)
            return Result<AppointmentDto>.Failure("Appointment not found", ServiceErrorType.NotFound);
    
        var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
        var medicalRecord = await _unitOfWork.MedicalRecords.GetById((int)appointmentDto.MedicalRecordID);
        
        if (medicalRecord is not null)
            appointmentDto.MedicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
        
        return Result<AppointmentDto>.Success(appointmentDto);
    }
}