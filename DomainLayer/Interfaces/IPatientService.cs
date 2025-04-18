using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAll();
        Patient GetById(int id);
        Result<Patient> Add(Patient patient);
        Result<Patient> Update(Patient patient);
    }
}
