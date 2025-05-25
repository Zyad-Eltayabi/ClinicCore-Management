namespace DomainLayer.DTOs;

public class PaymentDto
{
    public int PaymentID { get; set; }
    public DateTime PaymentDate { get; set; }
    public float AmountPaid { get; set; }
    public string? AdditionalNotes { get; set; }
}