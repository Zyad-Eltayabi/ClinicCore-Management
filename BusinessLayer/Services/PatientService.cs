using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Helpers;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.ServicesInterfaces;
using DomainLayer.Models;
using FluentValidation.Results;

namespace BusinessLayer.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PatientDto>> Add(PatientDto patientDto)
        {
            var validations = ValidatePatient(patientDto, GeneralEnum.SaveMode.Update);
            if (!validations.IsValid)
                return Result<PatientDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

            var patient = _mapper.Map<Patient>(patientDto);
            await _unitOfWork.Patients.Add(patient);
            var result = await _unitOfWork.SaveChanges();

            if (result)
            {
                return Result<PatientDto>.Success(_mapper.Map<PatientDto>(patient));
            }

            return Result<PatientDto>.Failure("Failed to add new patient in database",
              ServiceErrorType.DatabaseError);
        }

        private ValidationsResult ValidatePatient(PatientDto patient, GeneralEnum.SaveMode saveMode)
        {
            var validator = new PatientValidator(saveMode);
            var validationResult = validator.Validate(patient);
            if (!validationResult.IsValid)
            {
                string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ValidationsResult(false, message);
            }
            return new ValidationsResult(true, "");
        }

        public async Task<Result<PatientDto>> Update(PatientDto patient)
        {
            if (patient == null)
                return Result<PatientDto>.Failure("Patient can not be null", ServiceErrorType.ValidationError);

            // validate patientDto object
            var validations = ValidatePatient(patient, GeneralEnum.SaveMode.Update);
            if (!validations.IsValid)
                return Result<PatientDto>.Failure(validations.ErrorMessage, ServiceErrorType.ValidationError);

            var updatedPatient = await _unitOfWork.Patients.GetById(patient.Id);

            if (updatedPatient == null)
                return Result<PatientDto>.Failure("Patient is not found to update", ServiceErrorType.NotFound);

            _mapper.Map(patient, updatedPatient);
            var result = await _unitOfWork.SaveChanges();

            return result ?
                Result<PatientDto>.Success()
                : Result<PatientDto>.Failure("Failed to update the patient", ServiceErrorType.DatabaseError);
        }

        public async Task<IEnumerable<PatientDto>> GetAll()
        {
            var patients = await _unitOfWork.Patients.GetAll();

            if (patients == null)
                return null;

            var result = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return result;
        }

        public async Task<Result<PatientDto>> GetById(int id)
        {
            var patient = await _unitOfWork.Patients.GetById(id);
            if (patient == null)
            {
                return Result<PatientDto>
                    .Failure("Invalid patient id, the patient with this id is not found.",
                    ServiceErrorType.NotFound);
            }

            var result = _mapper.Map<PatientDto>(patient);
            return Result<PatientDto>.Success(result);
        }

        public async Task<Result<PatientDto>> Delete(int patientId)
        {
            var patient = await _unitOfWork.Patients.GetById(patientId);

            if (patient is null)
                return Result<PatientDto>
                    .Failure($"Invalid patient ID, there was not patient with id  = {patientId}",
                    ServiceErrorType.NotFound);

            _unitOfWork.Patients.Delete(patient);
            var result = await _unitOfWork.SaveChanges();

            return result ?
                    Result<PatientDto>.Success(null)
                    : Result<PatientDto>.Failure("Some error occurred during deleting in database.",
                    ServiceErrorType.DatabaseError);
        }

        public async Task<Result<Patient>> Delete(Expression<Func<Patient, bool>> predicate)
        {
            try
            {
                var result = await _unitOfWork.Patients.Delete(predicate);
                return Result<Patient>.Success();
            }
            catch (Exception ex)
            {
                return Result<Patient>.Failure("Some error occurred during deleting in database.", ServiceErrorType.ServerError);
            }
        }
    }
}
