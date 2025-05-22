namespace DomainLayer.DTOs;

public class AppointmentDto
{
    public int AppointmentID { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public short AppointmentStatus { get; set; }

    public int PatientID { get; set; }

    public int DoctorID { get; set; }

    public int? MedicalRecordID { get; set; }
    
    public MedicalRecordDto MedicalRecordDto { get; set; }
}