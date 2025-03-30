using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Interfaces
{
    public interface IServiceOrderRepository: IGenericRepository<ServiceOrder>
    {
        ServiceOrder GetOrderById(Guid id);
        ServiceOrder GetLastestOrderByParentId(Guid parentId);
        List<ServiceOrder> GetListOrderByParentId(Guid parentId);
        Task AddAsync(ServiceOrder serviceOrder);
        
        Task<ServiceOrder> CreateServiceOrderAsync(float totalAmount);
        Task<bool> UpdateServiceOrderStatusAsync(Guid orderId, int status);
        Task UpdateOrdersAsync(List<ServiceOrder> orders);
        Task<List<ServiceOrder>> GetExpiredOrdersAsync();

        Task<List<ServiceOrder>> GetServiceOrdersByParentIdAndServiceIds(Guid parentId, List<int> serviceIds);
        Task<List<ServiceOrder>> GetLatestServiceOrdersByParentId(Guid parentId);
        
        //New
        Task<List<ServiceOrder>> GetServiceOrdersByParentIdAsync(Guid parentId);
        Task<ServiceOrder?> GetLatestServiceOrderByParentIdAsync(Guid parentId);
        Task<ServiceOrder?> GetServiceOrderByIdAsync(Guid serviceOrderId);
        Task AddServiceOrderAsync(ServiceOrder serviceOrder);
        Task UpdateServiceOrderAsync(ServiceOrder serviceOrder);
        Task SaveChangesAsync();
    }
    
}
