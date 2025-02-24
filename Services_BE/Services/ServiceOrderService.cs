using AutoMapper;
using Services_BE.Interfaces;
using Repositories_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs_BE.ServiceOrderDTOs;
using DataObjects_BE.Entities;
using Org.BouncyCastle.Tls;

namespace Services_BE.Services
{
    public class ServiceOrderService: IServiceOrderService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IServiceRepositoy _serviceRepositoy;
        private readonly IParentRepository _parentRepository;
        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository, IMapper mapper, ICurrentTime currentTime, IServiceRepositoy serviceRepositoy, IParentRepository parentRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceRepositoy = serviceRepositoy;
            _parentRepository = parentRepository;
            _mapper = mapper;
            _currentTime = currentTime;
        }
        public async Task<ServiceOrderResponseDTO> GetServiceOrderById(string orderId)
        {
            try
            {
                var id = Guid.Parse(orderId);
                var orderExisting = _serviceOrderRepository.GetOrderById(id);
                if(orderExisting == null) 
                {
                    throw new Exception("Service Order is not existing!!!");
                }
                var result = _mapper.Map<ServiceOrderResponseDTO>(orderExisting);
                return  result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ServiceOrderResponseDTO>> GetListServiceOrder()
        {
            try
            {
                var list = _serviceOrderRepository.Get(includeProperties: "Service,Parent");
                if(list == null)
                {
                    throw new Exception("List is empty!!!");
                }
                var result = _mapper.Map<List<ServiceOrderResponseDTO>>(list);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ServiceOrderResponseDTO> CreateServiceOrder(CreateServiceOrderModel model)
        {
            try
            {
                var serviceExisting = _serviceRepositoy.GetByID(model.ServiceId);
                if(serviceExisting == null)
                {
                    throw new Exception("Service is not existing!!!");
                }
                var parent = _parentRepository.GetByID(model.ParentId);
                var price = float.Parse(serviceExisting.ServicePrice.ToString());
                ServiceOrder newOrder = new ServiceOrder()
                {
                    ServiceOrderId = Guid.NewGuid(),
                    ParentId = model.ParentId,
                    ServiceId = model.ServiceId,
                    Quantity = model.Quantity,
                    UnitPrice = price,
                    TotalPrice = model.Quantity * price,
                    CreateDate = _currentTime.GetCurrentTime().Date,
                    EndDate = _currentTime.GetCurrentTime().Date.AddDays(serviceExisting.ServiceDuration*model.Quantity),
                };
                
                await _serviceOrderRepository.AddAsync(newOrder);
                _serviceOrderRepository.Save();
                var result = _mapper.Map<ServiceOrderResponseDTO>(newOrder);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ServiceOrderResponseDTO> UpdateServiceOrder(UpdateServiceOrderModel model, string orderId)
        {
            try
            {
                Guid id = Guid.Parse(orderId);
                var orderExisting = _serviceOrderRepository.GetOrderById(id);
                if(orderExisting == null)
                {
                    throw new Exception("ServiceOrder is not existing!!!");
                }
                var timecurrent = _currentTime.GetCurrentTime().Date;
                if(orderExisting.EndDate< timecurrent)
                {
                    orderExisting.Quantity = orderExisting.Quantity + model.Quantity;
                    orderExisting.TotalPrice = orderExisting.UnitPrice * orderExisting.Quantity;
                    orderExisting.EndDate = _currentTime.GetCurrentTime().Date.AddDays(orderExisting.Service.ServiceDuration * model.Quantity);
                }
                else
                {
                    orderExisting.Quantity = orderExisting.Quantity + model.Quantity;
                    orderExisting.TotalPrice = orderExisting.UnitPrice * orderExisting.Quantity;
                    orderExisting.EndDate = orderExisting.EndDate.AddDays(orderExisting.Service.ServiceDuration * model.Quantity);
                }
               
                _serviceOrderRepository.Update(orderExisting);
                _serviceOrderRepository.Save();
                var result = _mapper.Map<ServiceOrderResponseDTO>(orderExisting);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
