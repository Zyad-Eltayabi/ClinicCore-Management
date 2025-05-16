using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessLayer.Validations;
using DomainLayer.BaseClasses;
using DomainLayer.DTOs;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using DomainLayer.Models;

namespace BusinessLayer.Services
{
    public class PatientService : IPatientService
    {
        private IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<PatientDTO>> Add(PatientDTO patientDto)
        {
            var validator = new PatientValidator(GeneralEnum.SaveMode.Add);
            var validationResult = validator.Validate(patientDto);
            if (!validationResult.IsValid)
            {
                //collect all errors
                string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ServiceResult<PatientDTO>.Failure(message);
            }

            // manual mapping patient entity with patient dto
            Patient patient = new()
            {
                FullName = patientDto.FullName,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                PhoneNumber = patientDto.PhoneNumber,
                Email = patientDto.Email,
                Address = patientDto.Address,
            };

            await _unitOfWork.Patients.Add(patient);
            var result = await _unitOfWork.SaveChanges();
            return result ?
                ServiceResult<PatientDTO>.Success(patientDto)
                : ServiceResult<PatientDTO>.Failure("internal error occurred");
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
                : ServiceResult<Patient>.Failure("internal error occurred");
        }

        public async Task<IEnumerable<PatientDTO>> GetAll()
        {
            try
            {
                var patients = await _unitOfWork.Patients.GetAll();

                if (patients == null)
                    return null;

                return patients.Select(p => new PatientDTO
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    Address = p.Address,
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Patient> GetById(int id)
        {
            return await _unitOfWork.Patients.GetById(id);
        }

        public async Task<ServiceResult<Patient>> Delete(Patient patient)
        {
            if (patient == null)
                return ServiceResult<Patient>.Failure("patient is null, can not delete it");

            if (patient.Id <= 0)
                return ServiceResult<Patient>.Failure("Invalid patient ID, can not delete it");

            _unitOfWork.Patients.Delete(patient);
            var result = await _unitOfWork.SaveChanges();

            return result ?
                    ServiceResult<Patient>.Success(patient)
                    : ServiceResult<Patient>.Failure("internal error occurred");
        }

        public async Task<ServiceResult<Patient>> Delete(Expression<Func<Patient, bool>> predicate)
        {
            try
            {
                var result = await _unitOfWork.Patients.Delete(predicate);
                return ServiceResult<Patient>.Success("Successful deleting.");
            }
            catch (Exception ex)
            {
                return ServiceResult<Patient>.Failure("Some error occurred during deleting in database.");
            }
        }
    }
}
