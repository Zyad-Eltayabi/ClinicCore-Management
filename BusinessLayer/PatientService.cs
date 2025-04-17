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

        public GeneralEnum.SaveMode SaveMode { get; set; }


        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SaveMode = GeneralEnum.SaveMode.Add;
        }

        public bool Add(Patient patient)
        {
            if (patient.Id == 0)
            {
                _unitOfWork.Patients.Add(patient);
                return _unitOfWork.SaveChanges() > 0;
            }
            return false;
        }

        public bool Update(Patient patient)
        {
            _unitOfWork.Patients.Update(patient);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _unitOfWork.Patients.GetAll();
        }

        public Patient GetById(int id)
        {
            return _unitOfWork.Patients.GetById(id);
        }
    }
}
