using DomainLayer.Models;

namespace DomainLayer.DTOs;

public class CompletePrescriptionDto
{
    public string MedicationName { get; set; }
    public string Dosage { get; set; }
    public short FrequencyPerDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? SpecialInstructions { get; set; }
}