using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Repositories
{
    public class ServiceOrderRepository: GenericRepository<ServiceOrder>, IServiceOrderRepository
    {
        private readonly SWP391G3DbContext _context;
        private readonly ICurrentTime _currentTime;
        
        public ServiceOrderRepository(SWP391G3DbContext context, ICurrentTime currentTime) : base(context)
        {
            _context = context;
            _currentTime = currentTime;
        }
        public ServiceOrder GetOrderById(Guid id)
        {
            var order = _context.ServiceOrders
                .Include(o => o.Service)
                .FirstOrDefault(x=>x.ServiceOrderId==id);
            return order;
        }
        public ServiceOrder GetLastestOrderByParentId(Guid parentId)
        {
            return _context.ServiceOrders
                .Include(o => o.Service)
                .Include(o => o.Parent)
                .Where(o => o.ParentId == parentId)
                .OrderByDescending(o => o.CreateDate)
                .FirstOrDefault();
        }
        public List<ServiceOrder> GetListOrderByParentId(Guid parentId)
        {
            return _context.ServiceOrders
                .Include(o => o.Service)
                .Include(o => o.Parent)
                .Where(o => o.ParentId == parentId)
                .OrderByDescending(o => o.CreateDate)
                .ToList();
        }
        
        public async Task AddAsync(ServiceOrder serviceOrder)
        {
            await _context.ServiceOrders.AddAsync(serviceOrder);
            await _context.SaveChangesAsync();
        }
        public async Task<ServiceOrder> CreateServiceOrderAsync(float totalAmount)
        {
            var order = new ServiceOrder
            {
                TotalPrice = totalAmount,
                Status = "Pending"
            };

            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateServiceOrderStatusAsync(Guid orderId, int status)
        {
            var order = await _context.ServiceOrders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status.ToString();
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ServiceOrder>> GetExpiredOrdersAsync()
        {
            return await _context.ServiceOrders
                .Where(o => o.Status == "Completed" && o.EndDate <= _currentTime.GetCurrentTime())
                .ToListAsync();
        }
        public async Task UpdateOrdersAsync(List<ServiceOrder> orders)
        {
            _context.ServiceOrders.UpdateRange(orders);
            await _context.SaveChangesAsync();
        }

    }
}
