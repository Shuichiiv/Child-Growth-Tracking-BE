using DTOs_BE.ServiceDTOs;
using DTOs_BE.ServiceOrderDTOs;
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
        Task<ServiceResponseDTO> GetServiceById(int id);
        Task<ServiceResponseDTO> CreateService(CreateServiceModel model);
        Task<ServiceResponseDTO> UpdateService(UpdateServiceModel model, int id);
    }
}
