using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.ServiceDTOs;
using DTOs_BE.ServiceOrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<ServiceResponseDTO, Service>().ReverseMap();
            CreateMap<ServiceOrderResponseDTO, ServiceOrder>().ReverseMap();
        }
    }
}
