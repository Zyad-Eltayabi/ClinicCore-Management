namespace DomainLayer.Models;

public class Doctor : Person
{
    public DateTime DateOfRegistration { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public ICollection<Appointment> Appointments { get; set; }
}