using DTOs_BE.RatingDTOs;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpGet("get-rating-by-id/{id}")]
        public async Task<IActionResult> GetRatingById(string id)
        {
            var response = _ratingService.GetRatingById(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("get-list-ratings")]
        public async Task<IActionResult> GetListRatings()
        {
            var response = _ratingService.GetListRating();
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost("create-rating")]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = _ratingService.CreateRating(model);
                if(response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("update-rating/{id}")]
        public async Task<IActionResult> UpdateRating([FromBody] UpdateRatingModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = _ratingService.UpdateRating(model, id);
                if (response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("change-active-rating/{id}")]
        public async Task<IActionResult> ChangeActiveRating(string id)
        {
            try
            {
                var response = _ratingService.ChangeActiveRating(id);
                if (response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
