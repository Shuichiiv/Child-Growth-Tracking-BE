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
        private readonly ICurrentTime _currentTime;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepositoy serviceRepositoy, IMapper mapper, ICurrentTime currentTime) 
        {
            _serviceRepositoy = serviceRepositoy;
            _mapper = mapper;
            _currentTime = currentTime;

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
        public async Task<ServiceResponseDTO> GetServiceById(int id)
        {
            try
            {
                var serviceExisting = _serviceRepositoy.GetByID(id);
                if(serviceExisting == null)
                {
                    throw new Exception("Service is not existing!!!");
                }
                var result = _mapper.Map<ServiceResponseDTO>(serviceExisting);
                return result;
            }catch(Exception ex) { throw ex; }
        }
        public async Task<ServiceResponseDTO> CreateService(CreateServiceModel model)
        {
            try
            {
                var service = new Service();
                service.ServiceName = model.ServiceName;
                service.ServicePrice = model.ServicePrice;
                service.ServiceDescription = model.ServiceDescription;
                service.ServiceDuration = model.ServiceDuration;
                service.ServiceCreateDate = _currentTime.GetCurrentTime().Date;
                service.IsActive = model.IsActive;
                var result = _mapper.Map<ServiceResponseDTO>(service);
                await _serviceRepositoy.AddAsync(service);
                _serviceRepositoy.Save();
                return result;
            }catch (Exception ex) { throw ex; }
        }
        public async Task<ServiceResponseDTO> UpdateService(UpdateServiceModel model, int id)
        {
            try
            {
                var serviceExisting = _serviceRepositoy.GetByID(id);
                if(serviceExisting == null)
                {
                    throw new Exception("Service is not existing!!!");
                }
                serviceExisting.ServiceName = model.ServiceName;
                serviceExisting.ServicePrice = model.ServicePrice;
                serviceExisting.ServiceDescription = model.ServiceDescription;
                serviceExisting.ServiceDuration = model.ServiceDuration;
                serviceExisting.IsActive = model.IsActive;
                var result = _mapper.Map<ServiceResponseDTO>(serviceExisting);
                _serviceRepositoy.Update(serviceExisting);
                _serviceRepositoy.Save();
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<ServiceResponseDTO> SoftRemoveService(int id)
        {
            try
            {
                var serviceExisting = _serviceRepositoy.GetByID(id);
                if(serviceExisting == null)
                {
                    throw new Exception("Service is not existing!!!");
                }

                serviceExisting.IsActive = false;
                var result = _mapper.Map<ServiceResponseDTO>(serviceExisting);
                _serviceRepositoy.Update(serviceExisting);
                _serviceRepositoy.Save();
                return result;
            }catch(Exception ex) { throw ex; }  
        }
        
    }
}
