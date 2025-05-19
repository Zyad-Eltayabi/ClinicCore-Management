using DomainLayer.Models;

namespace DomainLayer.DTOs;

public class DoctorDto : Person
{
    public DateTime DateOfRegistration { get; set; }
    public string Specialization { get; set; }
}