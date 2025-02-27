using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.ServiceDTOs;
using DTOs_BE.ServiceOrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs_BE.AppointmentDtos;
using DTOs_BE.FeedbackDTOs;
using DTOs_BE.RatingDTOs;

namespace Services_BE.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<ServiceResponseDTO, Service>().ReverseMap();
            CreateMap<ServiceOrderResponseDTO, ServiceOrder>().ReverseMap();
            
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Account.UserName))
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.Account.UserName))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.FirstName + " " + src.Child.LastName));

            CreateMap<AppointmentCreateDto, Appointment>();
            CreateMap<AppointmentUpdateDto, Appointment>();
            CreateMap<FeedbackResponseDTO,Feedback>().ReverseMap();
            CreateMap<RatingResponseDTO, Rating>().ReverseMap();
        }
    }
}
