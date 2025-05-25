namespace DomainLayer.DTOs;

public class CompleteAppointmentDto
{
    public int AppointmentID { get; set; }
    public CompletePrescriptionDto CompletePrescriptionDto { get; set; }
    public CompletePaymentDto CompletePaymentDto { get; set; }
}