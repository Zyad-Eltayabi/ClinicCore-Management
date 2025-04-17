using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainLayer;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using DomainLayer.Models;

namespace BusinessLayer
{
    public class PatientService : IPatientService
    {
        private IUnitOfWork _unitOfWork;

        private GeneralEnum.SaveMode _saveMode;


        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _saveMode = GeneralEnum.SaveMode.Add;
        }

        private bool Add(Patient patient)
        {
            _unitOfWork.Patients.Add(patient);
            return _unitOfWork.SaveChanges() > 0;
        }

        private bool Update(Patient patient)
        {
            _unitOfWork.Patients.Update(patient);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool Save(Patient patient)
        {
            switch (_saveMode)
            {
                case GeneralEnum.SaveMode.Add:
                    _saveMode = GeneralEnum.SaveMode.Update;
                    return Add(patient);
                case GeneralEnum.SaveMode.Update:
                    return Update(patient);
                default:
                    return false;
            }
        }

        public IEnumerable<Patient> GetAll()
        {
            return _unitOfWork.Patients.GetAll();
        }
    }
}
