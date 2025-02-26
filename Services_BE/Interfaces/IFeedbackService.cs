using DTOs_BE.FeedbackDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseDTO> GetFeedbackById(string id);
        Task<List<FeedbackResponseDTO>> GetListFeedBack();
        Task<FeedbackResponseDTO> CreateFeedback(CreateFeedbackModel model);
        Task<FeedbackResponseDTO> UpdateFeedback(UpdateFeedbackModel model, string id);
        Task<FeedbackResponseDTO> ChangeActiveOfFeedback(string id);
    }
}
