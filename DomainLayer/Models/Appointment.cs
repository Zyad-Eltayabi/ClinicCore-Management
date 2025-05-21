using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public short AppointmentStatus { get; set; }

        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int MedicalRecordID { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public int? PaymentID { get; set; }
        public Payment Payment { get; set; }
    }
}
