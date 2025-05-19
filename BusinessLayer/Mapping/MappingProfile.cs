using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Models;

namespace BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientDto, Patient>().ReverseMap();
            CreateMap<DoctorDto, Doctor>().ReverseMap();
        }
    }
}
