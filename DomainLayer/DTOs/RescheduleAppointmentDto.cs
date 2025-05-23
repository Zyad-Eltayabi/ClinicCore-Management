namespace DomainLayer.DTOs;

public class RescheduleAppointmentDto
{
    public int AppointmentID { get; set; }
    public DateTime NewAppointmentDateTime { get; set; }
    public int? DoctorID { get; set; }
}