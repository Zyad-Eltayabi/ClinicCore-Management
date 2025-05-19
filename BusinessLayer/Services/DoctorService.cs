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


    public Task<IEnumerable<DoctorDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Result<DoctorDto>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<DoctorDto>> Add(DoctorDto doctorDto)
    {
        var validations = ValidateDoctor(doctorDto);
        if (!validations.IsValid)
            return Result<DoctorDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

        // check if email exists 
        var isEmailExists = await _unitOfWork.Doctors.ExistsAsync(d => d.Email == doctorDto.Email);
        if (isEmailExists)
            return Result<DoctorDto>.Failure("Email already exists", ServiceErrorType.ValidationError);

        var doctor = _mapper.Map<Doctor>(doctorDto);
        await _unitOfWork.Doctors.Add(doctor);
        var result = await _unitOfWork.SaveChanges();

        return result
            ? Result<DoctorDto>.Success(_mapper.Map<DoctorDto>(doctor))
            : Result<DoctorDto>.Failure("Failed to add new doctor in database", ServiceErrorType.DatabaseError);
    }

    public Task<Result<DoctorDto>> Update(DoctorDto patient)
    {
        throw new NotImplementedException();
    }

    public Task<Result<DoctorDto>> Delete(int id)
    {
        throw new NotImplementedException();
    }
}