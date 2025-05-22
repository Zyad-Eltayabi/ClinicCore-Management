namespace DomainLayer.DTOs;

public class MedicalRecordDto
{
    public int MedicalRecordID { get; set; }
    public string VisitDescription { get; set; }
    public string? Diagnosis { get; set; }
    public string? AdditionalNotes { get; set; }
}
