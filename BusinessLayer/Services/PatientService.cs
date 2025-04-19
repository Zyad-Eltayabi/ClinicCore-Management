using System.Linq.Expressions;
using BusinessLayer.Validations;
using DomainLayer.BaseClasses;
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

        public Result<Patient> Add(Patient patient)
        {
            var validator = new PatientValidator(GeneralEnum.SaveMode.Add);
            var validationResult = validator.Validate(patient);
            if (!validationResult.IsValid)
            {
                //collect all errors
                string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<Patient>.Failure(message);
            }
            _unitOfWork.Patients.Add(patient);
            var result = _unitOfWork.SaveChanges();
            return result.IsSuccess ?
                Result<Patient>.Success(patient)
                : Result<Patient>.Failure(result.Message);
        }

        public Result<Patient> Update(Patient patient)
        {
            var validator = new PatientValidator(GeneralEnum.SaveMode.Update);
            var validationResult = validator.Validate(patient);

            if (!validationResult.IsValid)
            {
                //collect all errors
                string message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<Patient>.Failure(message);
            }

            _unitOfWork.Patients.Update(patient);

            var result = _unitOfWork.SaveChanges();
            return result.IsSuccess ?
                Result<Patient>.Success(patient)
                : Result<Patient>.Failure(result.Message);
        }

        public IEnumerable<Patient> GetAll()
        {
            return _unitOfWork.Patients.GetAll();
        }

        public Patient GetById(int id)
        {
            return _unitOfWork.Patients.GetById(id);
        }

        public Result<Patient> Delete(Patient patient)
        {
            if (patient == null)
                return Result<Patient>.Failure("patient is null, can not delete it");

            if (patient.Id <= 0)
                return Result<Patient>.Failure("Invalid patient ID, can not delete it");

            _unitOfWork.Patients.Delete(patient);
            var result = _unitOfWork.SaveChanges();

            return result.IsSuccess ?
                    Result<Patient>.Success(patient)
                    : Result<Patient>.Failure(result.Message);
        }

        public Result<Patient> Delete(Expression<Func<Patient, bool>> predicate)
        {
            var result = _unitOfWork.Patients.Delete(predicate);
            return result;

        }
    }
}
