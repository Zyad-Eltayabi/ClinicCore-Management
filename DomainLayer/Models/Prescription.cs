using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public short FrequencyPerDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? SpecialInstructions { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
        public int MedicalRecordId { get; set; }
    }
}
