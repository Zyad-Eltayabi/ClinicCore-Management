using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.DTOs
{
    public class PatientDto : Person
    {
        public DateTime DateOfRegistration { get; set; }
    }
}
