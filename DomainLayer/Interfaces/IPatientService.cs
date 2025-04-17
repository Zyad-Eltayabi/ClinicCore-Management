using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IPatientService
    {
        bool Save(Patient patient); // Save new Patient or update
        IEnumerable<Patient> GetAll();
    }
}
