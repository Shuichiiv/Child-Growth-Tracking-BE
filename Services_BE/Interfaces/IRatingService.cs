using DTOs_BE.RatingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDTO> GetRatingById(string id);
        Task<List<RatingResponseDTO>> GetListRating();
        Task<RatingResponseDTO> CreateRating(CreateRatingModel model);
        Task<RatingResponseDTO> UpdateRating(UpdateRatingModel model, string id);
        Task<RatingResponseDTO> ChangeActiveRating(string id);
    }
}
