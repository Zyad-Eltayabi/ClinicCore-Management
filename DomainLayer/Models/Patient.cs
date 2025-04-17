using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;

namespace DomainLayer.Models
{
    public class Patient : Person 
    {
        public DateTime DateOfRegistration { get; set; }
    }
}
