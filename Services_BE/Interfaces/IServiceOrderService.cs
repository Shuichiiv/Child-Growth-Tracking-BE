using DTOs_BE.ServiceOrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Interfaces
{
    public interface IServiceOrderService
    {
        Task<ServiceOrderResponseDTO> GetServiceOrderById(string orderId);
        Task<List<ServiceOrderResponseDTO>> GetListServiceOrder();
        Task<ServiceOrderResponseDTO> CreateServiceOrder(CreateServiceOrderModel model);
        Task<ServiceOrderResponseDTO> UpdateServiceOrder(UpdateServiceOrderModel model, string orderId);
    }
}
