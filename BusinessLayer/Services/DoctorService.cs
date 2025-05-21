using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Services;
using DomainLayer.Models;

namespace BusinessLayer.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private ValidationsResult ValidateDoctor(DoctorDto doctorDto)
    {
        var validator = new DoctorValidator();
        var validationResult = validator.Validate(doctorDto);
        if (!validationResult.IsValid)
        {
            string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new ValidationsResult(false, message);
        }

        return new ValidationsResult(true, "");
    }


    public async Task<Result<IEnumerable<DoctorDto>>> GetAll()
    {
        var doctors = await _unitOfWork.Doctors.GetAll();
        if (doctors != null)
        {
            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return Result<IEnumerable<DoctorDto>>.Success(doctorsDto);
        }

        return Result<IEnumerable<DoctorDto>>
            .Failure("There are no doctors found", ServiceErrorType.NotFound);
    }

    public async Task<Result<DoctorDto>> GetById(int id)
    {
        var doctor = await _unitOfWork.Doctors.GetById(id);

        if (doctor is null)
            return Result<DoctorDto>.Failure("Invalid doctor id, the doctor with this id is not found",
             ServiceErrorType.NotFound);

        var doctorDto = _mapper.Map<DoctorDto>(doctor);
        return Result<DoctorDto>.Success(doctorDto);
    }

    private async Task<Result<DoctorDto>> ValidateNewDoctor(DoctorDto doctorDto)
    {
        var validations = ValidateDoctor(doctorDto);
        if (!validations.IsValid)
            return Result<DoctorDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // check if email exists 
        var isEmailExists = await _unitOfWork.Doctors.ExistsAsync(d => d.Email == doctorDto.Email);
        if (isEmailExists)
            return Result<DoctorDto>.Failure("Email already exists", ServiceErrorType.ValidationError);

        return Result<DoctorDto>.Success();
    }

    public async Task<Result<DoctorDto>> Add(DoctorDto doctorDto)
    {
        var validateNewDoctor = await ValidateNewDoctor(doctorDto);
        if (!validateNewDoctor.IsSuccess)
            return validateNewDoctor;

        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _unitOfWork.Doctors.Add(doctor);
        var saved = await _unitOfWork.SaveChanges();

        return saved
            ? Result<DoctorDto>.Success(_mapper.Map<DoctorDto>(doctor))
            : Result<DoctorDto>.Failure("Failed to add new doctor in database", ServiceErrorType.DatabaseError);
    }

    public async Task<Result<DoctorDto>> Update(DoctorDto doctor)
    {
        // validate doctorDto object
        var validations = ValidateDoctor(doctor);
        if (!validations.IsValid)
            return Result<DoctorDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // map doctorDto to doctor object
        var updatedDoctor = await _unitOfWork.Doctors.GetById(doctor.Id);
        if (updatedDoctor is null)
            return Result<DoctorDto>.Failure("Doctor is not found to update", ServiceErrorType.NotFound);

        _mapper.Map(doctor, updatedDoctor);
        var result = await _unitOfWork.SaveChanges();

        return result ?
            Result<DoctorDto>.Success()
            : Result<DoctorDto>.Failure("Failed to update the doctor", ServiceErrorType.DatabaseError);
    }

    public async Task<Result<DoctorDto>> Delete(int id)
    {
        var doctor = await _unitOfWork.Doctors.GetById(id);
        if (doctor is null)
            return Result<DoctorDto>.Failure($"There is no doctor with id = {id}", ServiceErrorType.NotFound);
        _unitOfWork.Doctors.Delete(doctor);
        var deleted = await _unitOfWork.SaveChanges();
        return deleted ?
            Result<DoctorDto>.Success()
            : Result<DoctorDto>.Failure("Some error occurred during deleting in database.", ServiceErrorType.DatabaseError);
    }
}