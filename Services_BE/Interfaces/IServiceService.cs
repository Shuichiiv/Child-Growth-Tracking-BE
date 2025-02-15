using DTOs_BE.ServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Interfaces
{
    public interface IServiceService
    {
        Task<List<ServiceResponseDTO>> ListService();
    }
}
