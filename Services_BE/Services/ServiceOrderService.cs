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
using Microsoft.Extensions.Logging;

namespace Services_BE.Services
{
    public class ServiceOrderService: IServiceOrderService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IServiceRepositoy _serviceRepositoy;
        private readonly IParentRepository _parentRepository;
        private readonly ILogger<ServiceOrderService> _logger;
        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository, ILogger<ServiceOrderService> logger, IMapper mapper, ICurrentTime currentTime, IServiceRepositoy serviceRepositoy, IParentRepository parentRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceRepositoy = serviceRepositoy;
            _parentRepository = parentRepository;
            _mapper = mapper;
            _currentTime = currentTime;
            _logger = logger;
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

        public async Task<ServiceOrderResponseDTO> GetLastestServiceOrderByParentId(string parentId)
        {
            try
            {
                var id = Guid.Parse(parentId);
                var order = _serviceOrderRepository.GetLastestOrderByParentId(id);
                if(order == null)
                {
                    throw new Exception("This parent don't have any service order");
                }
                var result = _mapper.Map<ServiceOrderResponseDTO>(order);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ServiceOrderResponseDTO>> GetListServiceOrderByParentId(string parentId)
        {
            try
            {
                var id = Guid.Parse(parentId);
                var list = _serviceOrderRepository.GetListOrderByParentId(id);
                if( list == null)
                {
                    throw new Exception("This parent don't have any service order");
                }
                var result = _mapper.Map<List<ServiceOrderResponseDTO>>(list);
                return result;
            }catch (Exception ex)
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
                    Status = model.Status,
                    CreateDate = _currentTime.GetCurrentTime(),
                    EndDate = _currentTime.GetCurrentTime().AddDays(serviceExisting.ServiceDuration*model.Quantity),
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
                    orderExisting.EndDate = _currentTime.GetCurrentTime().AddDays(orderExisting.Service.ServiceDuration * model.Quantity);
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
        public async Task<CheckServiceRightsModel> CheckServiceRightsOfParent(string parentId)
        {
            try
            {
                var pId = Guid.Parse(parentId);
                var list = _serviceOrderRepository.GetListOrderByParentId(pId);
                bool check = false;
                int sId=-1;
                if(list == null||!list.Any())
                {
                    return new CheckServiceRightsModel
                    {
                        ServiceId = null,
                        IsValid = false
                    };
                }
                foreach(var i in list)
                {
                    if (i.Status=="Complete")
                    {
                        check = true;
                        sId = i.ServiceId;
                        break;
                    }
                }
                return new CheckServiceRightsModel
                {
                    ServiceId = sId,
                    IsValid = check
                };
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateExpiredOrdersAsync()
        {
            try
            {
                var expiredOrders = await _serviceOrderRepository.GetExpiredOrdersAsync();

                if (expiredOrders.Count > 0)
                {
                    foreach (var order in expiredOrders)
                    {
                        order.Status = "Cancel";
                    }

                    await _serviceOrderRepository.UpdateOrdersAsync(expiredOrders);
                    _logger.LogInformation($"Updated {expiredOrders.Count} service orders to 'Cancel'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating expired service orders: {ex.Message}");
            }
        }
    }
}
