using AutoMapper;
using ClinicAPI.DTOs;
using ClinicAPI.Models;

namespace ClinicAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();
        }
    }
}
