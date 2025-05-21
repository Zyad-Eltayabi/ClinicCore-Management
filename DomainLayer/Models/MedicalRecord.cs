using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class MedicalRecord
    {
        public int MedicalRecordID { get; set; }
        public string VisitDescription { get; set; }
        public string? Diagnosis { get; set; }
        public string? AdditionalNotes { get; set; }
        public Prescription Prescription { get; set; }
    }
}
