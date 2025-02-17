using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.ServiceDTOs;
using DTOs_BE.ServiceOrderDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Services
{
    public class ServiceService: IServiceService
    {
        private readonly IServiceRepositoy _serviceRepositoy;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepositoy serviceRepositoy, IMapper mapper) 
        {
            _serviceRepositoy = serviceRepositoy;
            _mapper = mapper;
        }

        public async Task<List<ServiceResponseDTO>> ListService()
        {
            try
            {
                var list =  _serviceRepositoy.Get(includeProperties: "ServiceOrders").ToList();
                if (list == null) { throw new Exception("List is empty"); }
                var result = _mapper.Map<List<ServiceResponseDTO>>(list);
                return result;
            }catch (Exception ex) { throw ex; }
        }
        public async Task<ServiceOrderResponseDTO> CreateService(CreateServiceModel model)
        {
            try
            {
                
            }catch (Exception ex) { throw ex; }
        }

    }
}
