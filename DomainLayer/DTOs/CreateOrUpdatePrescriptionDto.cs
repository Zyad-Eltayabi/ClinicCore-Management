namespace DomainLayer.DTOs;

public class CreateOrUpdatePrescriptionDto : CompletePrescriptionDto
{
    public int PrescriptionID { get; set; }
    public int MedicalRecordId { get; set; }

}