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
    }
}
