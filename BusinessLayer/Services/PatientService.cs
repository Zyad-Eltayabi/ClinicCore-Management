using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.Validations;
using DomainLayer.BaseClasses;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.ServicesInterfaces;
using DomainLayer.Models;

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

        public async Task<ServiceResult<PatientDto>> Add(PatientDto patientDto)
        {
            var validator = new PatientValidator(GeneralEnum.SaveMode.Add);
            var validationResult = validator.Validate(patientDto);
            if (!validationResult.IsValid)
            {
                //collect all errors
                string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ServiceResult<PatientDto>.Failure(message, ServiceErrorType.ValidationError);
            }

            var patient = _mapper.Map<Patient>(patientDto);
            await _unitOfWork.Patients.Add(patient);
            var result = await _unitOfWork.SaveChanges();

            if (result)
            {
                return ServiceResult<PatientDto>.Success(_mapper.Map<PatientDto>(patient));
            }

            return ServiceResult<PatientDto>.Failure("Failed to add new patient in database",
              ServiceErrorType.DatabaseError);
        }

        public async Task<ServiceResult<Patient>> Update(Patient patient)
        {
            //var validator = new PatientValidator(GeneralEnum.SaveMode.Update);
            //var validationResult = validator.Validate(patient);

            //if (!validationResult.IsValid)
            //{
            //    //collect all errors
            //    string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            //    return ServiceResult<Patient>.Failure(message);
            //}
            _unitOfWork.Patients.Update(patient);

            var result = await _unitOfWork.SaveChanges();
            return result ?
                ServiceResult<Patient>.Success(patient)
                : ServiceResult<Patient>.Failure("internal error occurred", ServiceErrorType.ServerError);
        }

        public async Task<IEnumerable<PatientDto>> GetAll()
        {
            var patients = await _unitOfWork.Patients.GetAll();

            if (patients == null)
                return null;

            var result = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return result;
        }

        public async Task<Patient> GetById(int id)
        {
            return await _unitOfWork.Patients.GetById(id);
        }

        public async Task<ServiceResult<Patient>> Delete(Patient patient)
        {
            if (patient == null)
                return ServiceResult<Patient>.Failure("patient is null, can not delete it", ServiceErrorType.ValidationError);

            if (patient.Id <= 0)
                return ServiceResult<Patient>.Failure("Invalid patient ID, can not delete it", ServiceErrorType.ValidationError);

            _unitOfWork.Patients.Delete(patient);
            var result = await _unitOfWork.SaveChanges();

            return result ?
                    ServiceResult<Patient>.Success(patient)
                    : ServiceResult<Patient>.Failure("Some error occurred during deleting in database.",
                    ServiceErrorType.ServerError);
        }

        public async Task<ServiceResult<Patient>> Delete(Expression<Func<Patient, bool>> predicate)
        {
            try
            {
                var result = await _unitOfWork.Patients.Delete(predicate);
                return ServiceResult<Patient>.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult<Patient>.Failure("Some error occurred during deleting in database.", ServiceErrorType.ServerError);
            }
        }
    }
}
