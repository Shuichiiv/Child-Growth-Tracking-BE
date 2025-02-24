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
        
        public ServiceOrderRepository(SWP391G3DbContext context) : base(context)
        {
            _context = context;
        }
        public ServiceOrder GetOrderById(Guid id)
        {
            var order = _context.ServiceOrders
                .Include(o => o.Service)
                .FirstOrDefault(x=>x.ServiceOrderId==id);
            return order;
        }
    }
}
